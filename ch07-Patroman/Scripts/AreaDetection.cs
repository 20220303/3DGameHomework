using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测范围的函数
public class AreaDetection : MonoBehaviour
{
    //墙位置的标签，说明要用empty创建东西
    public int sign = 0;
    //场景Controller
    FirstSceneController sceneController;
    private void Start()
    {
        //利用单实例得到唯一的场景Controller
        sceneController = SSDirector.GetInstance().CurrentScenceController as FirstSceneController;
    }
    //碰撞器
    void OnTriggerEnter(Collider collider)
    {
        //如果是Player碰撞的话，这个场景就被激活了
        if (collider.gameObject.tag == "Player")
        {
            sceneController.wall_sign = sign;

            Debug.Log("你现在进入的教室是：" + sign);
        }
    }
}
