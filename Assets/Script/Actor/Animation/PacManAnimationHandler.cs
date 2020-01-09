using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AnimationParam
{
	public const string IsDie = "IsDie";
}

public class PacManAnimationHandler : AnimationHandler
{
	public override void ResetParam()
	{
		animator.SetBool(AnimationParam.IsDie, false);
		SetDirection(new Direction(EDirX.Right, EDirY.None));
	}

	public void SetDie()
	{
		animator.SetBool(AnimationParam.IsDie, true);
	}
}
