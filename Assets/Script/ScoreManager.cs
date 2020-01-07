using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{

	#region Singleton
	private static ScoreManager instance;

	public static ScoreManager Instance {
		get {
			if (instance == null)
			{
				instance = new ScoreManager();
			}
			return instance;
		}
	}


	#endregion

	private ScoreManager()
	{
		Score = 0;
	}

	public int Score {
		get; private set;
	}

	public void AddFoodScore(EFood foodType)
	{
		switch (foodType)
		{
			case EFood.Cookie:
				Score += Cookie.Score;
				break;
		}
	}

}
