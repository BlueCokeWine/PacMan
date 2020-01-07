using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
	public Node(bool isWall, int x, int y)
	{
		this.isWall = isWall;
		this.x = x;
		this.y = y;
	}

	public bool isWall;
	public Node parentNode;

	// G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
	public int x, y, G, H;
	public int F { get { return G + H; } }
}

public abstract class Ghost : Actor
{
	public enum EStat
	{
		Normal,
		Tracking,
		Timid
	}

	const int StraightDistance = 10;
	const int DiagonalDistance = 14;
	const string WallTileTag = "TileMap_Wall";

	[SerializeField]
	protected float moveSpeed;

	protected MoveHandler moveHandler;
	protected AnimationHandler animHandler;

	protected EStat currentStat;
	protected Direction direction;

	protected Vector2Int homePlace;
	protected Vector2Int stageMin, stageMax;
	protected Vector2Int startPlace, targetPlace;
	protected Queue<Vector2Int> waypointQueue = new Queue<Vector2Int>();

	Node[,] nodeArray;
	Node startNode, targetNode, currentNode;
	List<Node> finalNodeList = new List<Node>();
	List<Node> openList, closedList;

	// Debug
	protected Color gizmoColor;

	public void Init(Vector2Int homePlace,Vector2Int stageMin, Vector2Int stageMax)
	{
		moveHandler = GetComponent<MoveHandler>();
		animHandler = GetComponent<AnimationHandler>();
		moveHandler.Init(moveSpeed, SetNextPlace);

		this.homePlace = homePlace;
		this.stageMin = stageMin;
		this.stageMax = stageMax;

		SetPlace(homePlace);
		SetNextPlace();
	}

	void Update()
	{
		moveHandler.Move();
	}

	void OnTriggerEnter(Collider other)
	{
		if (StageManager.Instance.ComparePlayer(other.gameObject))
		{
			if(currentStat == EStat.Timid)
			{
				// 집 으로
			}
			else
			{
				// 게임 오버
			}
		}
	}

	protected void SetTargetPlace(Vector2Int targetPlace)
	{
		startPlace = CurrentPlace;
		this.targetPlace = targetPlace;

		PathFinding();
	}

	protected abstract void SetNextPlace();

	protected abstract void ActiveTracking();

	protected void UpdateAnimation(Vector2 nextPlace)
	{
		if(nextPlace.x > CurrentPlace.x)
		{
			direction.X = EDirX.Right;
		}
		else if(nextPlace.x < CurrentPlace.x)
		{
			direction.X = EDirX.Left;
		}
		else
		{
			direction.X = EDirX.None;
		}

		if(nextPlace.y > CurrentPlace.y)
		{
			direction.Y = EDirY.Up;
		}
		else if(nextPlace.y < CurrentPlace.y)
		{
			direction.Y = EDirY.Down;
		}
		else
		{
			direction.Y = EDirY.None;
		}

		animHandler.SetDirection(direction);
	}

	#region A* Algorithm
	void PathFinding()
	{
		// NodeArray의 크기 정해주고, isWall, x, y 대입
		Vector2Int size = StageManager.Instance.CurrentStage.Size;
		nodeArray = new Node[size.x + 1, size.y + 1];

		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.y; j++)
			{
				bool isWall = false;
				foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + stageMin.x, j + stageMin.y), 0.4f))
					if (col.gameObject.layer == LayerMask.NameToLayer(WallTileTag)) isWall = true;

				nodeArray[i, j] = new Node(isWall, i + stageMin.x, j + stageMin.y);
			}
		}


		// 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
		startNode = nodeArray[startPlace.x - stageMin.x, startPlace.y - stageMin.y];
		targetNode = nodeArray[targetPlace.x - stageMin.x, targetPlace.y - stageMin.y];

		openList = new List<Node>() { startNode };
		closedList = new List<Node>();
		finalNodeList = new List<Node>();


		while (openList.Count > 0)
		{
			// 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
			currentNode = openList[0];
			for (int i = 1; i < openList.Count; i++)
				if (openList[i].F <= currentNode.F && openList[i].H < currentNode.H) currentNode = openList[i];

			openList.Remove(currentNode);
			closedList.Add(currentNode);


			// 마지막
			if (currentNode == targetNode)
			{
				Node TargetCurNode = targetNode;
				while (TargetCurNode != startNode)
				{
					finalNodeList.Add(TargetCurNode);
					TargetCurNode = TargetCurNode.parentNode;
				}
				finalNodeList.Add(startNode);
				finalNodeList.Reverse();

				//for (int i = 0; i < finalNodeList.Count; i++) print(i + "번째는 " + finalNodeList[i].x + ", " + finalNodeList[i].y);

				foreach (var child in finalNodeList)
				{
					Vector2Int waypoint = new Vector2Int();
					waypoint.x = child.x;
					waypoint.y = child.y;

					waypointQueue.Enqueue(waypoint);
				}
				waypointQueue.Dequeue();

				return;
			}

			// ↑ → ↓ ←
			OpenListAdd(currentNode.x, currentNode.y + 1);
			OpenListAdd(currentNode.x + 1, currentNode.y);
			OpenListAdd(currentNode.x, currentNode.y - 1);
			OpenListAdd(currentNode.x - 1, currentNode.y);
		}
	}

	void OpenListAdd(int checkX, int checkY)
	{
		// 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
		if (checkX >= stageMin.x && checkX < stageMax.x + 1 && checkY >= stageMin.y && checkY < stageMax.y + 1 && !nodeArray[checkX - stageMin.x, checkY - stageMin.y].isWall && !closedList.Contains(nodeArray[checkX - stageMin.x, checkY - stageMin.y]))
		{

			// 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
			if (nodeArray[currentNode.x - stageMin.x, checkY - stageMin.y].isWall || nodeArray[checkX - stageMin.x, currentNode.y - stageMin.y].isWall)
			{
				return;
			}


			// 이웃노드에 넣고, 직선은 10, 대각선은 14비용
			Node NeighborNode = nodeArray[checkX - stageMin.x, checkY - stageMin.y];
			int MoveCost = currentNode.G + (currentNode.x - checkX == 0 || currentNode.y - checkY == 0 ? StraightDistance : DiagonalDistance);


			// 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
			if (MoveCost < NeighborNode.G || !openList.Contains(NeighborNode))
			{
				NeighborNode.G = MoveCost;
				NeighborNode.H = (Mathf.Abs(NeighborNode.x - targetNode.x) + Mathf.Abs(NeighborNode.y - targetNode.y)) * StraightDistance;
				NeighborNode.parentNode = currentNode;

				openList.Add(NeighborNode);
			}
		}
	}
	#endregion

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		try
		{
			if (finalNodeList.Count != 0)
			{
				for (int i = 0; i < finalNodeList.Count - 1; i++)
				{
					Gizmos.color = gizmoColor;
					Gizmos.DrawLine(new Vector2(finalNodeList[i].x, finalNodeList[i].y), new Vector2(finalNodeList[i + 1].x, finalNodeList[i + 1].y));
				}
			}
		}
		catch (Exception e)
		{

		}
	}
#endif

}
