using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家与巡逻兵碰撞
public class PlayerCollideDetection : MonoBehaviour {
    //当玩家与巡逻兵碰撞
    void OnCollisionEnter(Collision other)
    {


        
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name=="yep")
        {

            Debug.Log(other.gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);
            this.gameObject.SetActive(false);
            return;
        }
        else if (other.gameObject.tag == "Player")
        {

            other.gameObject.GetComponent<Animator>().SetBool("death",true);
            this.GetComponent<Animator>().SetTrigger("attack");
            Singleton<GameEventManager>.Instance.PlayerGameover();
        }
    }
}
