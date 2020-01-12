#pragma warning disable CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageUIManager : Singleton<StageUIManager>
{
	enum EPanel
	{
		None,
		Pause,
		Cheat,
		GameOver
	}

	[SerializeField] GameObject pausePanel;
	[SerializeField] GameObject cheatPanel;
	[SerializeField] GameObject gameOverPanel;

	[SerializeField] TextMeshProUGUI highScoreText;
	[SerializeField] TextMeshProUGUI currentScoreText;
	[SerializeField] TextMeshProUGUI remainLifeText;

	public Action InstantWinAction;
	public Action ToggleGhostAction;
	public Action AddLifeAction;

	Dictionary<EPanel, GameObject> panelDic;

	void Awake()
	{
		panelDic = new Dictionary<EPanel, GameObject>();

		panelDic.Add(EPanel.Pause, pausePanel);
		panelDic.Add(EPanel.Cheat, cheatPanel);
		panelDic.Add(EPanel.GameOver, gameOverPanel);
	}

	void ActivePanel(EPanel type)
	{
		foreach (var item in panelDic)
		{
			if (item.Key == type)
			{
				item.Value.SetActive(true);
			}
			else
			{
				item.Value.SetActive(false);
			}
		}
	}

	public void StartGameOverAnimation()
	{
		ActivePanel(EPanel.GameOver);
	}

	#region Game UI

	public void SetHighScoreText(int score)
	{
		string scoreStr = string.Format("{0:N0}", score);
		highScoreText.text = scoreStr;
	}

	public void SetCurrentScore(int score)
	{
		string scoreStr = string.Format("{0:N0}", score);
		currentScoreText.text = scoreStr;
	}

	public void SetLifeCount(int count)
	{
		remainLifeText.text = count.ToString();
	}

	#endregion

	#region Button Event
	public void ButtonEvent_Game_PauseGame()
	{
		AudioManager.Instance.PlaySound(ESfxId.Click);
		AudioManager.Instance.PlayGhostSound(false);
		ActivePanel(EPanel.Pause);
		Time.timeScale = 0.0f;
	}

	public void ButtonEvent_Menu_ResumeGame()
	{
		ActivePanel(EPanel.None);
		Time.timeScale = 1.0f;
		AudioManager.Instance.PlaySound(ESfxId.Click);
		AudioManager.Instance.PlayGhostSound(true);
	}

	public void ButtonEvent_Menu_OpenCheatMenu()
	{
		AudioManager.Instance.PlaySound(ESfxId.Click);
		ActivePanel(EPanel.Cheat);
	}

	public void ButtonEvent_Menu_ExitStage()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneName.MenuSceneName);
		AudioManager.Instance.PlaySound(ESfxId.Click);
		AudioManager.Instance.StopBgm();
	}

	public void ButtonEvent_Cheat_InstantWin()
	{
		Time.timeScale = 1.0f;
		ActivePanel(EPanel.None);
		InstantWinAction?.Invoke();
		AudioManager.Instance.PlaySound(ESfxId.Click);
	}

	public void ButtonEvent_Cheat_ToggleGhost()
	{
		AudioManager.Instance.PlaySound(ESfxId.Click);
		ToggleGhostAction?.Invoke();
	}

	public void ButtonEvent_Cheat_AddLife()
	{
		AudioManager.Instance.PlaySound(ESfxId.Click);
		AddLifeAction?.Invoke();
	}

	public void ButtonEvent_Cheat_OK()
	{
		AudioManager.Instance.PlaySound(ESfxId.Click);
		ActivePanel(EPanel.Pause);
	}
	#endregion

}
