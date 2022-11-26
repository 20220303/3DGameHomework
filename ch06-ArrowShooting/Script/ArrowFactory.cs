using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour
{

	private GameObject arrow = null;
	private GameObject AB = null;
	private List<GameObject> usingArrowList = new List<GameObject>();
	private Queue<GameObject> unusedArrowList = new Queue<GameObject>();
	public Controllor sceneControler;

	public void initiate(GameObject AB_)
    {
		AB = AB_;
	}


	public GameObject GetArrow()
	{
		//还原工厂
		AB.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0));
		AB.transform.localScale = new Vector3(1, 1, 1);

		if (unusedArrowList.Count == 0)
		{
			arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"));
		}
		else
		{
			arrow = unusedArrowList.Dequeue();
			arrow.gameObject.SetActive(true);
		}
		arrow.transform.parent = AB.transform;//将弓箭作为AB的子物体
		arrow.transform.position = new Vector3(0, 0, 0);
		arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
		usingArrowList.Add(arrow);
		return arrow;
	}
	public void FreeArrow()
	{
		for (int i = 0; i < usingArrowList.Count; i++)
		{
			if (usingArrowList[i].transform.position.y <= -7 || usingArrowList[i].transform.position.y >= 7)
			{
				usingArrowList[i].GetComponent<Rigidbody>().isKinematic = true;
				usingArrowList[i].SetActive(false);
				usingArrowList[i].transform.position = new Vector3(0, 0, 0);
				unusedArrowList.Enqueue(usingArrowList[i]);
				usingArrowList.Remove(usingArrowList[i]);
				i--;


			}
		}
	}
}