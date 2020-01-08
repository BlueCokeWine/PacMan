using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (StageManager.Instance.ComparePlayer(other.gameObject))
		{
			gameObject.SetActive(false);
			EatEvent();
		}
	}

	protected abstract void EatEvent();
}
