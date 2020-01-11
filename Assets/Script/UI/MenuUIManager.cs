using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIManager : Singleton<MenuUIManager>
{

    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI stageIndexText;

    public Action PreStageIndexAction;
    public Action ProStageIndexAction;
    public Action StartStageAction;

    void Awake()
    {
        UpdateHighScore();
    }

    void UpdateHighScore()
    {
        int highScore = ScoreManager.GetHighScore();
        highScoreText.text = string.Format("{0:N0}", highScore);
    }

    public void SetStageIndexText(int index)
    {
        index++;
        stageIndexText.text = index.ToString();
    }

    public void ButtonEvent_PreStageIndex()
    {
        PreStageIndexAction?.Invoke();
    }

    public void ButtonEvent_ProStageIndex()
    {
        ProStageIndexAction?.Invoke();
    }

    public void ButtonEvent_StartStage()
    {
        StartStageAction?.Invoke();
        SceneManager.LoadScene(SceneName.StageSceneName);
    }

    public void ButtonEvent_ExitProgram()
    {
        Application.Quit();
    }
    
}
