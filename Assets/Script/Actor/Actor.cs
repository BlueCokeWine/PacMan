using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirX
{
	Right = 1,
	Left = -1,
	None = 0
}

public enum EDirY
{
	Up = 1,
	Down = -1,
	None = 0
}

public struct Direction
{
	public EDirX X { get; set; }
	public EDirY Y { get; set; }

	public Direction(EDirX x, EDirY y)
	{
		X = x;
		Y = y;
	}

	public static Direction Empty {
		get { return new Direction(EDirX.None, EDirY.None); }
	}

	public static bool operator ==(Direction op1, Direction op2)
	{
		if(op1.X == op2.X && op1.Y == op2.Y)
		{
			return true;
		}
		return false;
	}

	public static bool operator !=(Direction op1, Direction op2)
	{
		if (op1.X == op2.X && op1.Y == op2.Y)
		{
			return false;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		return obj is Direction direction &&
			   X == direction.X &&
			   Y == direction.Y;
	}

	public override int GetHashCode()
	{
		var hashCode = X.GetHashCode() * 10;
		hashCode += Y.GetHashCode();
		return hashCode;
	}
}

public class Actor : MonoBehaviour
{
	public Vector2Int CurrentPlace { get; protected set; }

	public void SetPlace(Vector2Int place)
	{
		Vector3 position = transform.position;
		position.x = place.x;
		position.y = place.y;
		transform.position = position;
		CurrentPlace = place;
	}

}
