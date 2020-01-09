using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
	const float MoveSpeed = 20.0f;
	//you need to say how far from the object the camera will stop
	const float MinimumDistanceFromTarget = 10.0f;

	Transform target;
	bool IsHighlightMode = false;

	Vector3 originPosition;

	void Awake()
	{
		originPosition = transform.position;
	}

	void Start()
	{
		target = StageManager.Instance.Player.transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (IsHighlightMode)
		{
			MoveToWardsTarget();
		}
	}

	public void MoveToWardsTarget()
	{
		if (Vector3.Distance(transform.position, target.position) > MinimumDistanceFromTarget) //we move only if we are further than the minimum distance
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, MoveSpeed * Time.deltaTime);
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
