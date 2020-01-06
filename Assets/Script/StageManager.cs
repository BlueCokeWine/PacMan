using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{

	[SerializeField]
	GameObject debugStage;
	[SerializeField]
	GameObject prefPacMan;

	Stage currentStage;
	PacMan player;

	void Awake()
	{
		InitStage();
	}

	public void InitStage()
	{
		currentStage = Instantiate(debugStage).GetComponent<Stage>();

		CreatePlayer();
	}

	public void ResetStage()
	{
		player.SetPlace(currentStage.GetPlayerStartPlace());
	}

	void CreatePlayer()
	{
		player = Instantiate(prefPacMan).GetComponent<PacMan>();
		player.SetPlace(currentStage.GetPlayerStartPlace());
	}

	void CreateGhosts()
	{

	}

	public bool ComparePlayer(GameObject gameObject)
	{
		if(player.gameObject == gameObject)
		{
			return true;
		}

		return false;
	}

	public bool CanMovePlace(Vector3Int place)
	{
		return currentStage.CanMove(place);
	}

}
