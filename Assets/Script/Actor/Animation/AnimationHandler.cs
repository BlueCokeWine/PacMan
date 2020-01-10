using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AnimationParam
{
	public const string DirX = "DirX";
	public const string DirY = "DirY";
}

public class AnimationHandler : MonoBehaviour
{

	[SerializeField]
	protected Animator animator;

	public virtual void ResetParam()
	{
		SetDirection(new Direction(EDirX.None, EDirY.Up));
	}

	public void SetDirection(Direction direction)
	{
		if (direction == Direction.Empty)
		{
			//StopAnimation();
			return;
		}

		foreach (AnimatorControllerParameter param in animator.parameters)
		{
			if (param.name == AnimationParam.DirX)
			{
				animator.SetInteger(AnimationParam.DirX, (int)direction.X);
			}

			if(param.name == AnimationParam.DirY)
			{
				animator.SetInteger(AnimationParam.DirY, (int)direction.Y);
			}
		}
		//animator.speed = 0.8f;
	}

	public void ResumeAnimation()
	{
		animator.speed = 1.0f;
	}

	public void StopAnimation()
	{
		animator.speed = 0.0f;
	}

	public void SetAnimator(RuntimeAnimatorController controller)
	{
		animator.runtimeAnimatorController = controller;
	}

}
