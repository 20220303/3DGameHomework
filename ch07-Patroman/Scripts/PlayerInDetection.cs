using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家进入巡逻兵的追踪范围
public class PlayerInDetection : MonoBehaviour
{
    //玩家进入巡逻兵的追踪范围
    void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("this.gameObject" + this.gameObject.name);
            this.gameObject.transform.GetComponent<PatrolData>().follow_player = true;
            this.gameObject.transform.GetComponent<PatrolData>().player = collider.gameObject;
            //触发巡逻兵向前扑的动作
            this.gameObject.transform.GetComponent<Animator>().SetTrigger("follow");
        }
    }
    //玩家离开巡逻兵的追踪范围
    void OnTriggerExit(Collider collider)
    {
        Debug.Log("玩家离开巡逻兵的追踪范围"+ collider.gameObject.tag);
        if (collider.gameObject.tag == "Player")
        {

            this.gameObject.transform.GetComponent<PatrolData>().follow_player = false;
            this.gameObject.transform.GetComponent<PatrolData>().player = null;
        }
    }
}
