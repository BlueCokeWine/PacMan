#pragma warning disable CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{

	public enum EState
	{
		Reset,
		Prepare,
		Play,
		PacManDie,
		GameOver,
		StageOver
	}

	const float PrepareTime = 3.0f;
	const float WaitResetTime = 3.0f;
	const float HightlightTime = 1.0f;
	const float StageOverWaitTime = 3.0f;
	const float GoToNextStageWaitTime = 5.0f;

	[SerializeField] CameraEffect cameraEffect;
	[SerializeField] GameObject prefPacMan;
	[SerializeField] GameObject prefBlinky;
	[SerializeField] GameObject prefPinky;
	[SerializeField] GameObject prefInky;
	[SerializeField] GameObject prefClyde;

	[SerializeField] List<GameObject> stageList;

	public EState GameState { get; private set; }
	public Stage CurrentStage { get; private set; }
	public PacMan Player { get; private set; }
	public List<Ghost> GhostList { get; private set; } = new List<Ghost>();
	public List<Food> FoodList { get; set; } = new List<Food>();
	public List<WarpTile> WarpGateList { get; set; } = new List<WarpTile>();

	public bool IsHighlightTime { get; private set; }

	int stageIndex = 0;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;

		

	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == SceneName.StageSceneName)
		{
			if (stageIndex == stageList.Count)
			{
				stageIndex = 0;
			}
			CreateStage(stageList[stageIndex]);
		}
	}

	public void CreateStage(GameObject stagePref)
	{
		CurrentStage = Instantiate(stagePref).GetComponent<Stage>();

		SetGameState(EState.Prepare);

		CreatePlayer();
		CreateGhosts();

		CameraEffect effect = Camera.main.GetComponent<CameraEffect>();
		effect.TargetObject = Player.transform;
		effect.SetOriginPosition(CurrentStage.CameraPosition);
	}

	void CreatePlayer()
	{
		Player = Instantiate(prefPacMan).GetComponent<PacMan>();
		Player.Init(CurrentStage.GetPlayerStartPlace());
	}

	void CreateGhosts()
	{
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

	public void CheckStageOver()
	{
		foreach (var child in FoodList)
		{
			if (!child.IsEaten)
			{
				return;
			}
		}

		SetGameState(EState.StageOver);
	}

	public void CheckGameOver()
	{

	}

	public void SetGameState(EState state)
	{
		GameState = state;

		switch (state)
		{
			case EState.Reset:
				Player.ResetData();

				foreach (var child in GhostList)
				{
					child.ResetData();
				}

				SetGameState(EState.Prepare);
				break;
			case EState.Prepare:
				CurrentStage.SetActiveReadyText(true);
				StartCoroutine(CountDownPrepareTime());
				break;
			case EState.Play:
				CurrentStage.SetActiveReadyText(false);
				break;
			case EState.PacManDie:
				Player.Die();
				StartCoroutine(CountDownResetTime());
				break;
			case EState.GameOver:
				break;
			case EState.StageOver:
				StartCoroutine(CountDownStageOverTime());
				break;
		}

	}

	public bool ComparePlayer(GameObject gameObject)
	{
		if (Player.gameObject == gameObject)
		{
			return true;
		}

		return false;
	}

	public bool CanMovePlace(Vector2Int place)
	{
		return CurrentStage.CanMove(place);
	}

	IEnumerator CountDownPrepareTime()
	{
		float timer = PrepareTime;

		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		SetGameState(EState.Play);
	}

	IEnumerator CountDownResetTime()
	{
		float timer = WaitResetTime;

		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		SetGameState(EState.Reset);
	}

	IEnumerator CountDownStageOverTime()
	{
		float timer = StageOverWaitTime;

		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		foreach (var child in GhostList)
		{
			child.gameObject.SetActive(false);
		}

		CurrentStage.SetActiveWallTwinkle(true);

		StartCoroutine(CountDownGoToNextStage());
	}

	IEnumerator CountDownGoToNextStage()
	{
		float timer = GoToNextStageWaitTime;

		while(timer > 0.0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		stageIndex++;
		SceneManager.LoadScene(SceneName.StageSceneName);
	}

	public IEnumerator StartHighlightTime()
	{
		float timer = HightlightTime;
		IsHighlightTime = true;
		cameraEffect.StartHighlightMode();

		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		cameraEffect.EndHighlightMode();
		IsHighlightTime = false;
	}

}
