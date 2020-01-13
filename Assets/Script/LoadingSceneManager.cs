#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
	static string nextScene = SceneName.MenuSceneName;

	[SerializeField] Transform startPosition;
	[SerializeField] Transform endPosition;
	[SerializeField] Transform cursurPosition;

	float length = 0.0f;

	private void Start()
	{
		switch (nextScene)
		{
			case SceneName.MenuSceneName:
				BundleManager.Instance.Init();
				StartCoroutine(MenuLoadScene());
				break;
			default:
				StartCoroutine(DefaultLoadScene());
				break;
		}
	}

	public static void LoadScene(string sceneName)
	{
		nextScene = sceneName;
		SceneManager.LoadScene(SceneName.LoadingSceneName);
	}

	IEnumerator DefaultLoadScene()
	{
		yield return null;

		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
		op.allowSceneActivation = false;

		float timer = 0.0f;

		while (!op.isDone)
		{
			yield return null;
			UpdateLoadCursur();
			timer += Time.deltaTime * 0.5f;

			if (op.progress < 0.9f)
			{
				length = Mathf.Lerp(length, op.progress, timer);
				if (length >= op.progress)
				{
					timer = 0f;
				}
			}
			else
			{
				length = Mathf.Lerp(length, 0.9f, timer);

				if (length == 0.9f)
				{
					length = 1.0f;
					UpdateLoadCursur();
					yield return new WaitForSeconds(0.2f);
					op.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}

	IEnumerator MenuLoadScene()
	{
		yield return null;

		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
		op.allowSceneActivation = false;

		float timer = 0.0f;

		while (!op.isDone)
		{
			yield return null;
			UpdateLoadCursur();
			timer += Time.deltaTime;

			if (op.progress < 0.9f)
			{
				length = Mathf.Lerp(length, op.progress, timer);
				if (length >= op.progress)
				{
					timer = 0f;
				}
			}
			else
			{
				length = Mathf.Lerp(length, 0.9f, timer);

				if (length == 0.9f && BundleManager.Instance.IsLoadComplete)
				{
					length = 1.0f;
					UpdateLoadCursur();
					yield return new WaitForSeconds(0.5f);
					op.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}

	void UpdateLoadCursur()
	{
		float value = Mathf.Lerp(startPosition.position.x, endPosition.position.x, length);

		Vector3 nowPosition = cursurPosition.position;
		nowPosition.x = value;
		cursurPosition.position = nowPosition;
	}

}
