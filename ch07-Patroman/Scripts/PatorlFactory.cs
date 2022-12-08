using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatorlFactory : MonoBehaviour {

    //工厂模式就负责生产，其他的不负责，所以可以看到这个类只和角色有关系和其他所有代码都没有关系
    private GameObject player = null;                                      //玩家
    private GameObject patrol = null;                                     //巡逻兵
    private List<GameObject> patrolList = new List<GameObject>();        //正在被使用的巡逻兵
    private Vector3[] vec = new Vector3[8];                             //保存每个巡逻兵的初始位置

    public GameObject LoadPlayer()
    {
        
        player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(8, 0, 8), Quaternion.Euler(new Vector3(0,180,0))) as GameObject;

        Debug.Log("初始化玩家");

        return player;
    }

    public List<GameObject> LoadPatrol()
    {
        
        //这里自定义八个位置，只有七个房间
        float[] pos_x = { 18, 18, 18, 18, -3.5f, -3.5f, -3.5f,18};
        float[] pos_z = { -0.5f,-11.5f, -22.5f, -32, -.5f,-11.5f, -22.3f,-34};
        int index = 0;

        for(int i=0;i < 8;i++)
        {
            vec[index] = new Vector3(pos_x[i], 0, pos_z[i]);
            index++;
            
        }
        for(int i=0; i < 8; i++)
        {
            int name = i + 1;
            patrol = Instantiate(Resources.Load<GameObject>("Prefabs/Patrol "+name));
            patrol.transform.position = vec[i];
            patrol.GetComponent<PatrolData>().sign = i + 1;
            if(i==7) patrol.GetComponent<PatrolData>().sign = i + 1;
            patrol.GetComponent<PatrolData>().start_position = vec[i];
            patrolList.Add(patrol);
            Debug.Log("初始化老师！" + patrol.name);
        }   
        return patrolList;
    }

    //游戏结束的时候会暂停所有动作
    public void StopPatrol()
    {
        for (int i = 0; i < patrolList.Count; i++)
        {
       
            patrolList[i].gameObject.GetComponent<Animator>().SetBool("run", false);
        }
    }
}
