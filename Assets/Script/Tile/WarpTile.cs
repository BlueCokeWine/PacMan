using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTile : MonoBehaviour
{

	[SerializeField] Transform exit;

	List<Vector2Int> range = new List<Vector2Int>();

	Color gizmoColor = Color.white;

	public Vector2Int Place { get; private set; }

	void Awake()
	{
		Place = Util.RoundToVectorInt((Vector2)transform.position);

		AddRangePlace(0, 0);	// Center
		AddRangePlace(1, 0);	// Right
		AddRangePlace(0, 1);	// Up
		AddRangePlace(-1, 0);	// Left
		AddRangePlace(0, -1);	// Down
	}

	void Start()
	{
		StageManager.Instance.WarpGateList.Add(this);
	}

	void AddRangePlace(int offsetX, int offsetY)
	{
		Vector2Int rangePlace = Place;
		rangePlace.x += offsetX;
		rangePlace.y += offsetY;
		range.Add(rangePlace);
	}

	void OnTriggerEnter(Collider other)
	{
		other.TryGetComponent(out MoveHandler handler);
		handler?.Warp(exit);
	}

	public void LinkDoorway(Transform exit)
	{
		this.exit = exit;
	}

	public bool IsInRange(Vector2Int checkPlace)
	{
		return range.Contains(checkPlace);
	}

	void OnDrawGizmos()
	{
		try
		{
			Gizmos.color = gizmoColor;
			Gizmos.DrawLine(transform.position, exit.position);
		}
		catch { }
	}

}
