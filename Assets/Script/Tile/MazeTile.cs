using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public enum ETileType
{
	Ground,
	GroundEmpty,
	GroundEnemy,
	Wall,
	Door
}

public class MazeTile : MonoBehaviour
{
	protected Point point;
	protected ETileType type;
}
