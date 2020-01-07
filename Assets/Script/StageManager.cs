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

	PacMan player;
	List<Ghost> ghostList = new List<Ghost>();

	public PacMan Player { get { return player; } }
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
		player = Instantiate(prefPacMan).GetComponent<PacMan>();
		Player.Init(CurrentStage.GetPlayerStartPlace());
	}

	void CreateGhosts()
	{
		// Debug!
		var ghostPlaceList = CurrentStage.GetGhostStartPlace();

		var ghost = Instantiate(prefBlinky).GetComponent<Blinky>();
		ghost.Init(ghostPlaceList[0], CurrentStage.Min, CurrentStage.Max);
		ghostList.Add(ghost);

		var ghost2 = Instantiate(prefPinky).GetComponent<Pinky>();
		ghost2.Init(ghostPlaceList[1], CurrentStage.Min, CurrentStage.Max);
		ghostList.Add(ghost2);

		var ghost4 = Instantiate(prefClyde).GetComponent<Clyde>();
		ghost4.Init(ghostPlaceList[3], CurrentStage.Min, CurrentStage.Max);
		ghostList.Add(ghost4);
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
