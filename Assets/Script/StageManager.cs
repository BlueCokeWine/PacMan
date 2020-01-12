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
	const float GameOverWaitTime = 4.0f;
	const int StartLifeCount = 5;

	[SerializeField] GameObject prefPacMan;
	[SerializeField] GameObject prefBlinky;
	[SerializeField] GameObject prefPinky;
	[SerializeField] GameObject prefInky;
	[SerializeField] GameObject prefClyde;
	[SerializeField] GameObject prefFruit;

	[SerializeField] List<GameObject> stageList;

	public EState GameState { get; private set; }
	public Stage CurrentStage { get; private set; }
	public PacMan Player { get; private set; }
	public List<Ghost> GhostList { get; private set; } = new List<Ghost>();
	public List<Food> FoodList { get; set; } = new List<Food>();
	public List<WarpTile> WarpGateList { get; set; } = new List<WarpTile>();

	public bool IsHighlightTime { get; private set; }

	int stageIndex = 0;
	int lifeCount;

	int fruitIndex = 0;
	bool isFruitCreated = false;
	LinkedList<Fruit.EType> eatenFruitList = new LinkedList<Fruit.EType>();

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
		StopAllCoroutines();
		switch (scene.name)
		{
			case SceneName.MenuSceneName:
				AddMenuFunction();
				stageIndex = 0;
				MenuUIManager.Instance.SetStageIndexText(stageIndex);
				break;
			case SceneName.StageSceneName:
				isFruitCreated = false;
				CreateStage(stageList[stageIndex]);
				AddCheatFunction();
				StageUIManager.Instance.SetLifeCount(lifeCount);
				break;
		}
	}

	void AddMenuFunction()
	{
		MenuUIManager.Instance.ProStageIndexAction += IncreaseStageIndex;
		MenuUIManager.Instance.PreStageIndexAction += DecreaseStageIndex;
		MenuUIManager.Instance.StartStageAction += StartNewGame;
	}

	void AddCheatFunction()
	{
		StageUIManager.Instance.InstantWinAction += InstantWin;
		StageUIManager.Instance.ToggleGhostAction += ToggleGhost;
		StageUIManager.Instance.AddLifeAction += AddLife;
	}

	public void CreateStage(GameObject stagePref)
	{
		GhostList.Clear();
		FoodList.Clear();
		WarpGateList.Clear();

		CurrentStage = Instantiate(stagePref).GetComponent<Stage>();

		SetGameState(EState.Prepare);

		CreatePlayer();
		CreateGhosts();

		CameraManager.Instance.SetMainCamera(Player);
		CameraManager.Instance.SetMinimapCamera(CurrentStage);
	}

	void CreatePlayer()
	{
		Player = Instantiate(prefPacMan).GetComponent<PacMan>();
		Player.Init(CurrentStage.GetPlayerStartPlace());
	}

	void CreateGhosts()
	{
		var ghostPlaceList = CurrentStage.GetGhostStartPlace();

		for(int i = 0; i < ghostPlaceList.Count; i++)
		{
			Ghost ghost;
			switch(i % 4)
			{
				case 1:
					ghost = Instantiate(prefPinky).GetComponent<Pinky>();
					break;
				case 2:
					ghost = Instantiate(prefInky).GetComponent<Inky>();
					break;
				case 3:
					ghost = Instantiate(prefClyde).GetComponent<Clyde>();
					break;
				default:
					ghost = Instantiate(prefBlinky).GetComponent<Blinky>();
					break;
			}
			ghost.Init(ghostPlaceList[i], CurrentStage.Min, CurrentStage.Max);
			GhostList.Add(ghost);
		}

	}

	public void MovePlayer(EDirX x, EDirY y)
	{
		Player?.JoyStickInput(x, y);
	}

	public void CheckStageOver()
	{
		int foodCount = FoodList.Count;
		int eatenFoodCount = 0;

		foreach (var child in FoodList)
		{
			if (child.IsEaten)
			{
				eatenFoodCount++;
			}
		}

		if(!isFruitCreated && eatenFoodCount > foodCount * 0.5f)
		{
			Vector3 createPosition = CurrentStage.GetFruitCreatePosition();
			Instantiate(prefFruit, createPosition, Quaternion.identity);
			isFruitCreated = true;
		}

		if(eatenFoodCount == foodCount)
		{
			SetGameState(EState.StageOver);
		}
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
				lifeCount--;
				StageUIManager.Instance.SetLifeCount(lifeCount);
				StartCoroutine(CountDownResetTime());
				break;
			case EState.GameOver:
				StartCoroutine(StartGameOverAnimation());
				break;
			case EState.StageOver:
				fruitIndex++;
				StartCoroutine(CountDownStageOverTime());
				ScoreManager.Instance.SaveCurrentScore();
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

	public bool IsDoorPlace(Vector2Int place)
	{
		return CurrentStage.IsDoorTile(place);
	}

	public void AddEatenFruit(Fruit.EType type)
	{
		eatenFruitList.AddFirst(type);
		// UI 추가
	}

	#region UI Func
	void IncreaseStageIndex()
	{
		stageIndex++;

		if(stageIndex == stageList.Count)
		{
			stageIndex = 0;
		}

		MenuUIManager.Instance.SetStageIndexText(stageIndex);
	}

	void DecreaseStageIndex()
	{
		stageIndex--;
		if(stageIndex < 0)
		{
			stageIndex = stageList.Count - 1;
		}

		MenuUIManager.Instance.SetStageIndexText(stageIndex);
	}

	void StartNewGame()
	{
		lifeCount = StartLifeCount;
		fruitIndex = 0;
		eatenFruitList.Clear();

		ScoreManager.ResetCurrentScore();
	}
	#endregion

	#region Cheat Func
	void InstantWin()
	{
		SetGameState(EState.StageOver);
	}

	void ToggleGhost()
	{
		foreach(var child in GhostList)
		{
			bool isActive = child.gameObject.activeInHierarchy;
			child.gameObject.SetActive(!isActive);
		}
	}

	void AddLife()
	{
		lifeCount++;
		StageUIManager.Instance.SetLifeCount(lifeCount);
	}
	#endregion

	#region Timer Func
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

		if(lifeCount < 0)
		{
			SetGameState(EState.GameOver);
		} else
		{
			SetGameState(EState.Reset);
		}
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

		if (stageIndex == stageList.Count)
		{
			stageIndex = 0;
		}

		SceneManager.LoadScene(SceneName.StageSceneName);
	}

	IEnumerator StartGameOverAnimation()
	{
		float timer = GameOverWaitTime;
		StageUIManager.Instance.StartGameOverAnimation();

		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		SceneManager.LoadScene(SceneName.MenuSceneName);
	}

	public IEnumerator StartHighlightTime()
	{
		float timer = HightlightTime;
		IsHighlightTime = true;
		CameraManager.Instance.CameraShaking(0.1f);
		//cameraEffect.StartHighlightMode();

		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}

		//cameraEffect.EndHighlightMode();
		IsHighlightTime = false;
	}
	#endregion

}
