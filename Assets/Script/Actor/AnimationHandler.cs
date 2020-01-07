using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParam
{
	public const string DirX = "DirX";
	public const string DirY = "DirY";
}

public class AnimationHandler : MonoBehaviour
{

	[SerializeField]
	Animator animator;

	public void SetDirection(Direction direction)
	{
		if(direction == Direction.Empty)
		{
			//StopAnimation();
			return;
		}

		animator.SetInteger(AnimationParam.DirX, (int)direction.X);
		animator.SetInteger(AnimationParam.DirY, (int)direction.Y);
		//animator.speed = 0.8f;
	}

	public void StopAnimation()
	{
		animator.speed = 0.0f;
	}

}
