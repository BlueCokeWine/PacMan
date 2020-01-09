using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AnimationParam
{
	public const string RemainTimidTime = "RemainTimidTime";
}

public class GhostAnimationHandler : AnimationHandler
{

	public void SetRemainTimidTime(float reaminTime)
	{
		animator.SetFloat(AnimationParam.RemainTimidTime, reaminTime);
	}

}
