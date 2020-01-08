using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{

	[SerializeField]
	GameObject debugStage;
	[SerializeField]
	GameObject prefPacMan;
	[SerializeField]
	GameObject prefBlinky;
	[SerializeField]
	GameObject prefPinky;
	[SerializeField]
	GameObject prefInky;
	[SerializeField]
	GameObject prefClyde;

	public PacMan Player { get; private set; }
	public List<Ghost> GhostList { get; private set; }
	public Stage CurrentStage { get; private set; }

	void Awake()
	{
		InitStage();
	}

	public void InitStage()
	{
		CurrentStage = Instantiate(debugStage).GetComponent<Stage>();

		CreatePlayer();
		CreateGhosts();
	}

	public void ResetStage()
	{
		Player.SetPlace(CurrentStage.GetPlayerStartPlace());
	}

	void CreatePlayer()
	{
		Player = Instantiate(prefPacMan).GetComponent<PacMan>();
		Player.Init(CurrentStage.GetPlayerStartPlace());
	}

	void CreateGhosts()
	{
		GhostList = new List<Ghost>();
		var ghostPlaceList = CurrentStage.GetGhostStartPlace();

		var ghost1 = Instantiate(prefBlinky).GetComponent<Blinky>();
		ghost1.Init(ghostPlaceList[0], CurrentStage.Min, CurrentStage.Max);
		GhostList.Add(ghost1);

		var ghost2 = Instantiate(prefPinky).GetComponent<Pinky>();
		ghost2.Init(ghostPlaceList[1], CurrentStage.Min, CurrentStage.Max);
		GhostList.Add(ghost2);

		var ghost3 = Instantiate(prefInky).GetComponent<Inky>();
		ghost3.Init(ghostPlaceList[2], CurrentStage.Min, CurrentStage.Max);
		GhostList.Add(ghost3);

		var ghost4 = Instantiate(prefClyde).GetComponent<Clyde>();
		ghost4.Init(ghostPlaceList[3], CurrentStage.Min, CurrentStage.Max);
		GhostList.Add(ghost4);
	}

	public bool ComparePlayer(GameObject gameObject)
	{
		if(Player.gameObject == gameObject)
		{
			return true;
		}

		return false;
	}

	public bool CanMovePlace(Vector2Int place)
	{
		return CurrentStage.CanMove(place);
	}

}
