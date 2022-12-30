using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace PerfabsController {

    //建立一个枚举类来储存叠加方式
    public enum Superposition
    {
        //周期叠加
        Cycle,
        //振幅叠加
        Amplitude
    }

    public class WaterController
    {
        //水的位置
        readonly Vector3 ocean_pos = new Vector3(0, .4F, 0);

        //Ocean
        GameObject Ocean;

        //水波振幅
        public float amplitude = .7f;

        //波动的速度
        public float speed = 2.8f;

        //振幅叠加方式
        public Superposition superpositon = Superposition.Amplitude;

        //模型网格
        private Mesh sharedMesh;

        //网格初始顶点坐标数组
        private Vector3[] baseVertex;

        //网格修改后的顶点坐标数组（也就是每一次update之后）
        private Vector3[] nowVertex;


        public WaterController(string _name) 
        {   
            Ocean = Object.Instantiate(Resources.Load("Perfabs/Ocean", typeof(GameObject)), ocean_pos, Quaternion.identity, null) as GameObject;
            Ocean.name = "ocean";
            //初始化
            sharedMesh = Ocean.GetComponent<MeshFilter>().sharedMesh;
            baseVertex = sharedMesh.vertices;
            nowVertex = sharedMesh.vertices;
        }

        //GameObject的Update调用
        public void WaterWave()
        {
            //更新波浪顶点
            for (int i = 0; i < baseVertex.Length; i++)
            {
                nowVertex[i] = baseVertex[i];
                switch (superpositon)
                {
                    case Superposition.Cycle:
                        nowVertex[i].y += Mathf.Sin(Time.time * speed + baseVertex[i].x + baseVertex[i].z) * amplitude;
                        break;
                    case Superposition.Amplitude:
                        nowVertex[i].y += Mathf.Sin(Time.time * speed + baseVertex[i].x) * amplitude + Mathf.Sin(Time.time * speed + baseVertex[i].z) * amplitude;
                        break;


                }
            }

            sharedMesh.vertices = nowVertex;
        }

    }

    /*******************************************************************************************/

    public class PAndVController
    {

        readonly GameObject POrV;
        readonly char PVType;

        //readonly MoveCharacter MoveComponent;
        readonly ClickGUI clickComponent;

        CoastController coastController;
        bool _isOnBoat;//判断该对象是不是在船上

        public float speed = 20;


        public PAndVController(string which_character, string _name, Vector3 _pos)
        {
            _isOnBoat = false;
            if (which_character == "priest")
            {
                POrV = Object.Instantiate(Resources.Load("Perfabs/Priest", typeof(GameObject))) as GameObject;
                PVType = 'p';
            }
            else
            {
                POrV = Object.Instantiate(Resources.Load("Perfabs/Devil", typeof(GameObject))) as GameObject;
                PVType = 'd';
            }
            POrV.name = _name;
            POrV.transform.position = _pos;

            //给预制件附上脚本
            //MoveComponent = POrV.AddComponent(typeof(MoveCharacter)) as MoveCharacter;
            clickComponent = POrV.AddComponent(typeof(ClickGUI)) as ClickGUI;
            clickComponent.setController(this);
        }



        public char getType()
        {   // p and d
            return PVType;
        }

        public string getName()
        {
            return POrV.name;
        }
        public void setPosition(Vector3 pos)
        {
            POrV.transform.position = pos;
        }

        //让POrV上船
        public void getOnBoat(BoatController boatCtrl)
        {
            coastController = null;
            POrV.transform.parent = boatCtrl.getGameobject().transform;
            _isOnBoat = true;
        }

        public void getOnCoast(CoastController coastCtrl)
        {
            coastController = coastCtrl;
            POrV.transform.parent = null;
            _isOnBoat = false;
        }
        //public void moveToPosition(Vector3 destination)
        //{
        //    //MoveComponent.setDestination(destination);
        //}
        public bool isOnBoat()
        {
            return _isOnBoat;
        }

        public CoastController getCoastController()
        {
            return coastController;
        }

        public GameObject getGameObject(){
            return POrV;
        }

        public void reset()
        {
            //MoveComponent.reset();
            coastController = (Director.getInstance().currentSceneController as GameController).rightCoast;
            getOnCoast(coastController);
            setPosition(coastController.getAvailablePosition());
            coastController.getOnCoast(this);
        }

    }

   
    /*******************************************************************************************/
    public class CoastController
    {

        /*类的主要任务 
            * 
            *  1.这里控制岸边的操作
            *  2.设置好站位
            *  3.增加P And V
            *  
            */



        //本控件的“coast”游戏对象
        readonly GameObject coast;

        //固定好码头的位置
        readonly Vector3 right_pos = new Vector3(10, 1, 0);
        readonly Vector3 left_pos = new Vector3(-10, 1, 0);

        //码头上游戏角色的固定位置
        readonly Vector3[] positions = new Vector3[] {new Vector3(7.5F,2.3F,0), new Vector3(8.5F,2.3F,0), new Vector3(9.5F,2.3F,0),
            new Vector3(10.5F,2.3F,0), new Vector3(11.5F,2.3F,0), new Vector3(12.5F,2.3F,0)};

        //"1->right" or "-1->left" coast
        readonly int which_coast;

        //规划六个位置，不用规划十二个，因为只有六个POrV
        PAndVController[] pvQueue;


        


         public CoastController(string _which_coast)
         {
            pvQueue = new PAndVController[6];

            if (_which_coast == "right")
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), right_pos, default, null) as GameObject;
                coast.name = "right_coast";
                which_coast = 1;
            }
            else
            {
                coast = Object.Instantiate(Resources.Load("Perfabs/Stone", typeof(GameObject)), left_pos, default, null) as GameObject;
                coast.name = "left_coast";
                which_coast = -1;
            }

         }

        //得到岸上空的位置
        public int getAvailableIndex()
        {
            for (int i = 0; i < pvQueue.Length; i++)
            {
                if (pvQueue[i] == null)
                {
                    return i;
                }
            }
            return -1;
            
        }


        //上岸
        public void getOnCoast(PAndVController POrV)
        {
            int i = getAvailableIndex();
            //上岸
            pvQueue[i] = POrV;

        }

        //得到位置
        public Vector3 getAvailablePosition()
        {
            int i = getAvailableIndex();
            //调整位置
            Vector3 pos = positions[i];
            pos.x *= which_coast;

            return pos;
        }


        //下岸
        public PAndVController getOffCoast(string pv_name)
        {   // 0->priest, 1->devil
            for (int i = 0; i < pvQueue.Length; i++)
            {
                if (pvQueue[i] != null && pvQueue[i].getName() == pv_name)
                {
                    PAndVController charactorCtrl = pvQueue[i];
                    pvQueue[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("cant find passenger on coast: " + pv_name);
            return null;
        }

        public int get_which_coast()
        {
            return which_coast;
        }

        public int[] getPVNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < pvQueue.Length; i++)
            {
                if (pvQueue[i] == null)
                    continue;
                if (pvQueue[i].getType() == 'p')
                {   
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            pvQueue = new PAndVController[6];
        }






    }

    /*******************************************************************************************/
    public class BoatController
    {
        readonly GameObject boat;
        //船的位置
        readonly Vector3 rightPosition = new Vector3(5, 0.4F, 0);
        readonly Vector3 leftPosition = new Vector3(-5, 0.4F, 0);
        //船上乘客的位置
        readonly Vector3[] right_Position = new Vector3[] { new Vector3(4.5F, 1, 0), new Vector3(5.5F, 1, 0) };
        readonly Vector3[] left_Position = new Vector3[] { new Vector3(-5.5F, 1, 0), new Vector3(-4.5F, 1, 0) };

        int which_coast;//left->-1,right->1

        //readonly MoveCharacter MoveComponent;
        PAndVController[] PVQueue = new PAndVController[2];

        //船的速度
        public float boatSpeed = 20;

        //绕z轴摇摆的速度
        float z_Speed = 3f;

        //绕x轴摇摆的速度
        float x_Speed = 3f;

        //绕y轴摇摆的速度
        float y_Speed = 3f;


        public BoatController()
        {
            //从右岸出发
            which_coast = 1;

            //加载船的预制件
            boat = Object.Instantiate(Resources.Load("Perfabs/Boat", typeof(GameObject)), rightPosition, new Quaternion(-.7F, .7F, .7F, .7F), null) as GameObject;
            boat.name = "boat";
            //MoveComponent =boat.AddComponent(typeof(MoveCharacter)) as MoveCharacter;
            boat.AddComponent(typeof(ClickGUI));
        }

        //摇晃这个船
        public void Shake()
        {
            //MoveComponent.setAngle();
            // 绕Z轴摇晃  
            if (boat.transform.eulerAngles.z >= 3 && boat.transform.eulerAngles.z <= 180)
            {
                z_Speed = -z_Speed;
            }
            else if (boat.transform.eulerAngles.z <= (360 - 3) && boat.transform.eulerAngles.z >= 180)
            {
                z_Speed = -z_Speed;
            }
            // 绕X轴摇晃  
            if (boat.transform.eulerAngles.x >= 3 && boat.transform.eulerAngles.x <= 180)
            {
                x_Speed = -x_Speed;
            }
            else if (boat.transform.eulerAngles.x >= 180 && boat.transform.eulerAngles.x <= (360 - 3))
            {
                x_Speed = -x_Speed;
            }
            boat.transform.Rotate(z_Speed * Time.deltaTime, 0, z_Speed * Time.deltaTime);

        }

        public Vector3 getToPosition()
        {
            if (which_coast == -1)
            {
                Debug.Log(rightPosition);

                return rightPosition;
                //MoveComponent.setDestination(rightPosition);
                //which_coast = 1;
            }
            else
            {
                Debug.Log(leftPosition);

                return leftPosition;
                //MoveComponent.setDestination(leftPosition);
                //which_coast = -1;
            }
        }

        public void changeCoast()
        {
            which_coast *= -1;
        }


        //public void Move()
        //{
        //    Debug.Log("Move");

        //    if (which_coast == -1)
        //    {
        //        Debug.Log(rightPosition);
        //        MoveComponent.setDestination(rightPosition);
        //        which_coast = 1;
        //    }
        //    else
        //    {
        //        Debug.Log(leftPosition);
        //        MoveComponent.setDestination(leftPosition);
        //        which_coast = -1;
        //    }
        //}

        public int getEmptyIndex()
        {
            for (int i = 0; i < PVQueue.Length; i++)
            {
                if (PVQueue[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool isEmpty()
        {

            for (int i = 0; i < PVQueue.Length; i++)
            {
                if (PVQueue[i] != null)
                {
                    return false;
                }
            }
            return true;
        }
        public GameObject getGameobject()
        {
            return boat;
        }
        public Vector3 getAvailablePosition()
        {
            Vector3 pos;
            if (which_coast == -1)
            {
               pos = left_Position[getEmptyIndex()];
            }
            else
            {
                pos = right_Position[getEmptyIndex()];
            }
            return pos;
        }

        //上船
        public void GetOnBoat(PAndVController characterCtrl)
        {
            int index = getEmptyIndex();
            PVQueue[index] = characterCtrl;
        }

        //下船
        public PAndVController GetOffBoat(string pv_name)
        {
            for (int i = 0; i < PVQueue.Length; i++)
            {
                if (PVQueue[i] != null && PVQueue[i].getName() == pv_name)
                {
                    PAndVController charactorCtrl = PVQueue[i];
                    PVQueue[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("Can not find any character on the boat: " + pv_name);
            return null;
        }

       

        public int get_which_coast()
        { 
            return which_coast;
        }

        public int[] getPVNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < PVQueue.Length; i++)
            {
                if (PVQueue[i] == null)
                    continue;
                if (PVQueue[i].getType() == 'p')
                {   
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            //MoveComponent.reset();


            //if (which_coast == -1)
            //{
            //    Move();
            //}


            PVQueue = new PAndVController[2];
        }



}

    /***************************************************************************/
//    //控制移动的，挂载在6个POrV身上
//    //这个参考了往届的代码
//    public class MoveCharacter : MonoBehaviour
//    {

//        readonly float move_speed = 20;
//        // 绕Z轴的摇摆的速度
//        float z_Speed = 3.0f;
//        // 绕X轴的摇摆的速度
//        float x_Speed = 1.0f;

//        int count = 0;

//        int moving_status; 
//        Vector3 target;

//        //移动
//        void Update()
//        {
//            if(moving_status==1)
//            {
           
//                transform.position = Vector3.MoveTowards(transform.position, target , move_speed * Time.deltaTime);
            
//                if (transform.position==target)moving_status = 0;
//            }

           

//            // 绕Z轴摇晃  
//            if (this.transform.eulerAngles.z >= 4 && this.transform.eulerAngles.z <= 180)
//            {
//                z_Speed = -z_Speed;
//            }
//            else if (this.transform.eulerAngles.z <= (360 - 4) && this.transform.eulerAngles.z >= 180)
//            {
//                z_Speed = -z_Speed;
//            }
//            // 绕X轴摇晃  
//            if (this.transform.eulerAngles.x >= 4 && this.transform.eulerAngles.x <= 180)
//            {
//                x_Speed = -x_Speed;
//            }
//            else if (this.transform.eulerAngles.x >= 180 && this.transform.eulerAngles.x <= (360 - 4))
//            {
//                x_Speed = -x_Speed;
//            }
//            this.transform.Rotate(z_Speed * Time.deltaTime, 0, z_Speed * Time.deltaTime);


            
        
           
//        }
//        public void setDestination(Vector3 _target)
//        {
//            target = _target;
//            moving_status = 1;
           
//        }

//        public void setAngle()
//        {
//            moving_status = 3;
//        }

//        public void reset()
//        {
//            moving_status = 0;
//        }
//    }






}
