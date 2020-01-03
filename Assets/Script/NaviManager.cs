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


public class NaviManager : MonoBehaviour
{
	const int StraightDistance = 10;
	const int DiagonalDistance = 14;
	const string WallTileTag = "TileMap_Wall";

	public Vector2Int bottomLeft, topRight, startPos, targetPos;
	public List<Node> FinalNodeList;

	int sizeX, sizeY;
	Node[,] nodeArray;
	Node startNode, targetNode, currentNode;
	List<Node> openList, closedList;


	public void PathFinding()
	{
		// NodeArray의 크기 정해주고, isWall, x, y 대입
		sizeX = topRight.x - bottomLeft.x + 1;
		sizeY = topRight.y - bottomLeft.y + 1;
		nodeArray = new Node[sizeX, sizeY];

		for (int i = 0; i < sizeX; i++)
		{
			for (int j = 0; j < sizeY; j++)
			{
				bool isWall = false;
				foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
					if (col.gameObject.layer == LayerMask.NameToLayer(WallTileTag)) isWall = true;

				nodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
			}
		}


		// 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
		startNode = nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
		targetNode = nodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

		openList = new List<Node>() { startNode };
		closedList = new List<Node>();
		FinalNodeList = new List<Node>();


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
					FinalNodeList.Add(TargetCurNode);
					TargetCurNode = TargetCurNode.parentNode;
				}
				FinalNodeList.Add(startNode);
				FinalNodeList.Reverse();

				for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
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
		if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !closedList.Contains(nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
		{

			// 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
			if (nodeArray[currentNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || nodeArray[checkX - bottomLeft.x, currentNode.y - bottomLeft.y].isWall)
			{
				return;
			}


			// 이웃노드에 넣고, 직선은 10, 대각선은 14비용
			Node NeighborNode = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
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

	void OnDrawGizmos()
	{
		if (FinalNodeList.Count != 0)
		{
			for (int i = 0; i < FinalNodeList.Count - 1; i++)
			{
				Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
			}
		}
	}
}