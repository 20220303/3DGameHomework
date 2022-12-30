using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour
{

	private GameObject arrow = null;
	private GameObject AB = null;
	private GameObject ImageTargetAB = null;
	private List<GameObject> usingArrowList = new List<GameObject>();
	private Queue<GameObject> unusedArrowList = new Queue<GameObject>();
	public Controllor sceneControler;
	private int arrownum = 0;
	private List<GameObject> ArrowList = new List<GameObject>();

	public void initiate(GameObject AB_,GameObject ImageTargetAB_)
    {
		AB = AB_;
		ImageTargetAB = ImageTargetAB_;

	}

    private void Start()
    {

		GameObject a;
        for(int i = 0; i < 20; i++)
        {
			a = GameObject.Find("Arrow (" + i + ")");
			ArrowList.Add(a);
			a.SetActive(false);
		}
    }

    public GameObject GetArrow()
	{
		//还原工厂
		//AB.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 90, 0));
		//AB.transform.localScale = new Vector3(1, 1, 1);

		//if (unusedArrowList.Count == 0)
		//{
		//	//arrow = Instantiate(Resources.Load<GameObject>("Prefabs/Arrow"));
		//	arrow = GameObject.Find("Arrow");
		//	Debug.Log("Arrow的位置：" + arrow.transform.position);
		//	Debug.Log("Arrow的角度：" + arrow.transform.rotation);
		//}
		//else
		//{
		//	arrow = unusedArrowList.Dequeue();
		//	arrow.gameObject.SetActive(true);
		//}


		//arrow.transform.parent = AB.transform;
		//arrow.transform.SetPositionAndRotation(ImageTargetAB.transform.position, Quaternion.Euler(ImageTargetAB.transform.rotation.x, ImageTargetAB.transform.rotation.y+90, ImageTargetAB.transform.rotation.z ));
		//Debug.Log("Image的位置: " + ImageTargetAB.transform.position);
		//Debug.Log("Image的角度: " + ImageTargetAB.transform.rotation);
		//arrow.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);

		//usingArrowList.Add(arrow);
		//return arrow;

		//因为AR识别的图片角度不精确，没有办法使用工厂模式动态添加Arrow，使用这种方式能更好的保证体验感
		if (arrownum < 20)
		{

			arrow = ArrowList[arrownum];
			arrow.SetActive(true);
			arrownum++;
		}

		usingArrowList.Add(arrow);
		return arrow;
	}
	
	public void FreeArrow()
	{
		for (int i = 0; i < usingArrowList.Count; i++)
		{
			if (usingArrowList[i].transform.position.y <= -15 || usingArrowList[i].transform.position.y >= 15)
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