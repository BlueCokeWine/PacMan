using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : MonoBehaviour
{

	public bool IsEaten { get; protected set; } = false;

	void Start()
	{
		StageManager.Instance.FoodList.Add(this);
	}

	void OnTriggerEnter(Collider other)
	{
		if (StageManager.Instance.ComparePlayer(other.gameObject))
		{
			gameObject.SetActive(false);
			EatEvent();
			IsEaten = true;
			StageManager.Instance.CheckStageOver();
		}
	}

	protected abstract void EatEvent();
}
