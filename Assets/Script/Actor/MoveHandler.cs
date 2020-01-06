using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{

	float moveSpeed;
	float moveTime;
	Vector3Int startPlace;
	Vector3Int destPlace;

	public Action arriveEvent;

	public void Init(float moveSpeed, Action arrived)
	{
		this.moveSpeed = moveSpeed;
		arriveEvent += arrived;
	}

	public void Move()
	{
		moveTime += Time.deltaTime * moveSpeed;

		var position = transform.position;
		position.x = Mathf.Lerp(startPlace.x, destPlace.x, moveTime);
		position.y = Mathf.Lerp(startPlace.y, destPlace.y, moveTime);
		transform.position = position;

		if (moveTime > 1.0f)
		{
			arriveEvent();
			moveTime = 0.0f;
		}
	}

	public void SetDestination(Vector3Int currentPlace, Vector3Int nextPlace)
	{
		startPlace = currentPlace;
		destPlace = nextPlace;
	}

}
