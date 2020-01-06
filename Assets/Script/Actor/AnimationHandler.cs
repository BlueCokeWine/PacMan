using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

	[SerializeField]
	Animator animator;

	public const string AnimParamDirX = "DirX";
	public const string AnimParamDirY = "DirY";

	public void SetDirection(Direction direction)
	{
		if(direction == Direction.Empty)
		{
			StopAnimation();
			return;
		}

		animator.SetInteger(AnimParamDirX, (int)direction.X);
		animator.SetInteger(AnimParamDirY, (int)direction.Y);
		animator.speed = 0.8f;
	}

	public void StopAnimation()
	{
		animator.speed = 0.0f;
	}

}
