using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

	[SerializeField] float shakeWeight = 0.0f;

	float shakeTime;
	Vector3 originPosition;

	void Update()
	{
		if(shakeTime > 0.0f)
		{
			transform.localPosition = Random.insideUnitSphere * shakeWeight + originPosition;
			shakeTime -= Time.deltaTime;

			if(shakeTime <= 0.0f)
			{
				transform.localPosition = originPosition;
			}
		}
		
	}

	public void StartShaking(float time)
	{
		originPosition = transform.localPosition;
		shakeTime = time;
	}

}
