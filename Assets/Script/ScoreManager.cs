#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{

	const string HighScoreSaveName = "HighScore";
	const string CurrentScoreSaveName = "CurrentScore";
	const int MakeScoreCount = 5;

	[SerializeField] GameObject scorePref;

	int addScore;
	int currentScore;
	int highScore;

	LinkedList<Score> scorePool = new LinkedList<Score>();
	LinkedList<Score> activeScoreList = new LinkedList<Score>();
	LinkedList<Score> deactiveScoreList = new LinkedList<Score>();

	void Awake()
	{
		highScore = PlayerPrefs.GetInt(HighScoreSaveName, 0);
		currentScore = PlayerPrefs.GetInt(CurrentScoreSaveName, 0);

		MakeScore(MakeScoreCount);
	}

	void Start()
	{
		StageUIManager.Instance.SetHighScoreText(highScore);
		StageUIManager.Instance.SetCurrentScore(currentScore);
	}

	void Update()
	{
		UpdateCurrentScore();
		CheckDeactiveScore();
	}

	public void SaveCurrentScore()
	{
		PlayerPrefs.SetInt(CurrentScoreSaveName, currentScore);
	}

	public static void ResetCurrentScore()
	{
		PlayerPrefs.DeleteKey(CurrentScoreSaveName);
	}

	public static int GetHighScore()
	{
		int highScore = PlayerPrefs.GetInt(HighScoreSaveName, 0);
		return highScore;
	}

	public void AddScore(int score)
	{
		addScore += score;
	}

	public void AddScore(int score, Score.EType type, Vector3 position)
	{
		addScore += score;
		ShowScore(score, type, position);
	}

	void UpdateHighScore()
	{
		if (currentScore < highScore)
		{
			return;
		}

		highScore = currentScore;
		StageUIManager.Instance.SetHighScoreText(highScore);
		PlayerPrefs.SetInt(HighScoreSaveName, highScore);
	}

	void UpdateCurrentScore()
	{
		if (addScore > 0f)
		{
			if (addScore > 100)
			{
				addScore -= 100;
				currentScore += 100;
			}

			if (addScore > 10)
			{
				addScore -= 10;
				currentScore += 10;
			}

			if (addScore > 0)
			{
				addScore--;
				currentScore++;
			}

			if (addScore <= 0)
			{
				UpdateHighScore();
			}

			StageUIManager.Instance.SetCurrentScore(Mathf.RoundToInt(currentScore));
		}
	}

	#region Score Object Pool

	void MakeScore(int count)
	{
		for(int i = 0; i < count; i++)
		{
			var score = Instantiate(scorePref).GetComponent<Score>();

			score.transform.SetParent(transform);
			score.gameObject.SetActive(false);
			scorePool.AddLast(score);
		}
	}

	void ShowScore(int score, Score.EType type, Vector3 position)
	{
		if(scorePool.Count == 0)
		{
			MakeScore(MakeScoreCount);
		}

		var script = scorePool.First.Value;
		scorePool.RemoveFirst();

		script.gameObject.SetActive(true);
		script.Init(type, score);
		script.transform.position = position;
		activeScoreList.AddLast(script);
	}

	void CheckDeactiveScore()
	{
		foreach(var child in activeScoreList)
		{
			child.DoUpdate();

			if (child.CanDeactive)
			{
				deactiveScoreList.AddLast(child);
			}
		}

		foreach(var child in deactiveScoreList)
		{
			child.gameObject.SetActive(false);
			activeScoreList.Remove(child);
			scorePool.AddLast(child);
		}

		deactiveScoreList.Clear();
	}

	#endregion

}
