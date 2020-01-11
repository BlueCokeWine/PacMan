using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{

	const string HighScoreSaveName = "HighScore";
	const string CurrentScoreSaveName = "CurrentScore";

	int addScore;
	int currentScore;
	int highScore;

	void Awake()
	{
		highScore = PlayerPrefs.GetInt(HighScoreSaveName, 0);
		currentScore = PlayerPrefs.GetInt(CurrentScoreSaveName, 0);
	}

	void Start()
	{
		StageUIManager.Instance.SetHighScoreText(highScore);
		StageUIManager.Instance.SetCurrentScore(currentScore);
	}

	void Update()
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

			if(addScore <= 0)
			{
				UpdateHighScore();
			}

			StageUIManager.Instance.SetCurrentScore(Mathf.RoundToInt(currentScore));
		}
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

}
