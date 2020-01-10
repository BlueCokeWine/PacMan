using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
	const float MoveSpeed = 20.0f;
	//you need to say how far from the object the camera will stop
	const float MinimumDistanceFromTarget = 10.0f;

	bool IsHighlightMode = false;

	Vector3 originPosition;

	public Transform TargetObject { get; set; }

	// Update is called once per frame
	void Update()
	{
		if (IsHighlightMode)
		{
			MoveToWardsTarget();
		}
	}

	public void SetOriginPosition(Vector3 position)
	{
		originPosition = position;
		transform.position = position;
	}

	public void MoveToWardsTarget()
	{
		if (Vector3.Distance(transform.position, TargetObject.position) > MinimumDistanceFromTarget) //we move only if we are further than the minimum distance
		{
			transform.position = Vector3.MoveTowards(transform.position, TargetObject.position, MoveSpeed * Time.deltaTime);
		}
	}

	public void StartHighlightMode()
	{
		IsHighlightMode = true;
	}

	public void EndHighlightMode()
	{
		IsHighlightMode = false;
		transform.position = originPosition;
	}
}
