using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFood
{
	DotFood
}

public abstract class Food : MonoBehaviour
{
	protected EFood foodType;

	void OnTriggerEnter(Collider other)
	{
		if (StageManager.Instance.ComparePlayer(other.gameObject))
		{
			EatEvent();
		}
	}

	protected abstract void EatEvent();
}
