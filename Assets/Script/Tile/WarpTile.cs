using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTile : MonoBehaviour
{

	[SerializeField] Transform exit;

	Color gizmoColor = Color.white;

	void OnTriggerEnter(Collider other)
	{
		other.TryGetComponent(out MoveHandler handler);
		handler?.Warp(exit);

		Debug.Log("Called");
	}

	public void LinkDoorway(Transform exit)
	{
		this.exit = exit;
	}

	void OnDrawGizmos()
	{
		try
		{
			Gizmos.color = gizmoColor;
			Gizmos.DrawLine(transform.position, exit.position);
		}
		catch (Exception e)
		{

		}
	}

}
