using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
	Ray2D ray;
	bool logHit;
    
	void Start()
    {
		ray = new Ray2D(transform.position, Vector2.right);
	}

    void Update()
	{
		RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction);
		Debug.DrawRay(ray.origin,ray.direction,Color.blue);

		if (info.collider != null)
		{
			Debug.Log(info.transform.gameObject.name);
			logHit = false;
			//if (info.transform.gameObject.CompareTag("Fly"))
			//{
			//	Debug.LogWarning("检测到敌人");
			//}
			//else
			//{
			//	Debug.Log(info.transform.gameObject.name);
			//}
		}
		else
		{
            if (!logHit)
            {
				Debug.Log("没有碰撞任何对象");
				logHit = true;
			}
		}
	}
}