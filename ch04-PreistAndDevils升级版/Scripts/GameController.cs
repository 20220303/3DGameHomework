using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using PerfabsController;
using interfaceApplication;


public class GameController: MonoBehaviour , SceneController, UserAction
{


    /*类的主要任务 
     * 
     *  1.加载资源
     *  2.移动对象
     *  3.计算游戏结果  
     *  
     */



    UserGUI userGUI;

    public WaterController Ocean;

    public CoastController rightCoast;
    public CoastController leftCoast;

    Judge judge;
    
    public BoatController boat;
    private PAndVController[] PVQueue;

    public ActionManager actionManager;

  



    //游戏启动之前用于初始化任何变量和游戏状态,在脚本实例生命周期中仅被调用一次,不能做协程
    void Awake()
    {

        Director director = Director.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = gameObject.AddComponent<ActionManager>() as ActionManager;

        PVQueue = new PAndVController[6];
       
        loadResources();
    }

    void Update()
    {
        //这两个状态整个游戏都有，不需要动作类的管理
        Ocean.WaterWave();
        boat.Shake();

    }


    //加载资源
    public void loadResources()
    {
        //加载准备好的预制件water
        //记得改预制件的名字
        //Object.Instantiate加载预制件的位置，如果不写其他参数的话就是预制件原本的位置

        //加载我的Ocean
        Ocean = new WaterController("ocean");
        //GameObject ocean = Object.Instantiate (Resources.Load("Perfabs/Ocean", typeof(GameObject)), ocean_pos, Quaternion.identity, null) as GameObject;
        //ocean.name = "ocean";

        //加载我的Coast
        rightCoast = new CoastController("right");
        leftCoast = new CoastController("left");


        //加载我的角色
        for(int i = 0; i < 3; i++)
        {
            PAndVController APriest = new PAndVController("priest", "priest" + i, rightCoast.getAvailablePosition());
            APriest.getOnCoast(rightCoast);
            rightCoast.getOnCoast(APriest);
            PVQueue[i] = APriest;

        }
        for(int i = 0; i < 3; i++)
        {
            PAndVController ADevils = new PAndVController("devil", "devil" + i, rightCoast.getAvailablePosition());
            ADevils.getOnCoast(rightCoast);
            rightCoast.getOnCoast(ADevils);
            PVQueue[i+3] = ADevils;
        }


        //加载船
         boat = new BoatController();
        //初始化裁判类
        judge = new Judge(rightCoast,leftCoast,boat);


    }



    public void restart()
    {

        //动作管理器管理
        if (boat.get_which_coast() == -1)
        {
            actionManager.moveBoat(boat.getGameobject(), boat.getToPosition(), boat.boatSpeed);
            boat.changeCoast();
        }
        
        boat.reset();
        rightCoast.reset();
        leftCoast.reset();
        for (int i = 0; i < PVQueue.Length; i++)
        {
            PVQueue[i].reset();
        }
    }


    //点击PV的组件
    public void PAndVIsClicked(PAndVController POrVCtrl)
    {

        Vector3 middlePos,endPos;

        //下船上岸
        if (POrVCtrl.isOnBoat())
        {
            //先判断现在是哪一个岸
            CoastController whichCoast;
            if (boat.get_which_coast() == 1)
            {
                whichCoast = rightCoast;
               
            }
            else
            {
                whichCoast = leftCoast;
            }
            //目的地
            endPos = whichCoast.getAvailablePosition();
            //中间位置
            middlePos = new Vector3(POrVCtrl.getGameObject().transform.position.x,endPos.y, endPos.z);


            //动作
            boat.GetOffBoat(POrVCtrl.getName());
            //动作管理器管理
            actionManager.moveRole(POrVCtrl.getGameObject(),middlePos,endPos,boat.boatSpeed);
            //其他操作
            //POrVCtrl.setPosition(whichCoast.getAvailablePosition());
            POrVCtrl.getOnCoast(whichCoast);
            whichCoast.getOnCoast(POrVCtrl);
            

        }
        else//下岸上船
        {
            CoastController whichCoast = POrVCtrl.getCoastController();

            if (boat.getEmptyIndex() == -1)
            {   //船满了
                return;
            }
            else if(whichCoast.get_which_coast() != boat.get_which_coast())
            {  
                //船没靠岸
                return;
            }
            else
            {

                //目的地
                endPos = boat.getAvailablePosition();
                //中间位置
                middlePos = new Vector3( endPos.x, POrVCtrl.getGameObject().transform.position.y, endPos.z);
                //动作
                whichCoast.getOffCoast(POrVCtrl.getName());
                //动作管理器管理
                actionManager.moveRole(POrVCtrl.getGameObject(),middlePos,endPos ,POrVCtrl.speed);
                //其他操作
                //POrVCtrl.setPosition(endPos);
                POrVCtrl.getOnBoat(boat);
                boat.GetOnBoat(POrVCtrl);

            } 
        }
        //检查游戏状态
        userGUI.status = judge.endGame();
    }



    //移动船的组件
    public void moveBoat()
    {

        
        if (boat.isEmpty())
            return;

        //boat.Move();
        //动作管理器管理
        actionManager.moveBoat(boat.getGameobject(), boat.getToPosition(), boat.boatSpeed);
        boat.changeCoast();

        //检查游戏状态
        userGUI.status = judge.endGame();
    }


}