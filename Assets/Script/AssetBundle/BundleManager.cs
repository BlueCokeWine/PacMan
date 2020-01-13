using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

public class BundleManager : Singleton<BundleManager>
{

	// 번들 다운 받을 서버의 주소(필자는 임시방편으로 로컬 파일 경로 쓸 것임)
	const string StageBundlePcURI = @"https://firebasestorage.googleapis.com/v0/b/neolife-c0ce4.appspot.com/o/AssetBundle%2Fstage_asset_pc?alt=media&token=67fbefdb-0497-4caf-970c-5d8075659ea6";
	const string StageBundleAndroidURI = @"https://firebasestorage.googleapis.com/v0/b/neolife-c0ce4.appspot.com/o/AssetBundle%2Fstage_asset_android?alt=media&token=34990629-547a-4550-9340-c74401e748f6";

	AssetBundle stageAssetBundle;

	#region Property
	public bool IsLoadComplete { get; private set; } = false;
	#endregion

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
		}

		DontDestroyOnLoad(this);
	}

	public void Init()
	{
		StartCoroutine(DownloadAndCache());
	}

	IEnumerator DownloadAndCache()
	{
		// cache 폴더에 AssetBundle을 담을 것이므로 캐싱시스템이 준비 될 때까지 기다림 
		while (!Caching.ready)
		{
			yield return null;
		}

		string uri;

#if UNITY_EDITOR || UNITY_STANDALONE
		uri = StageBundlePcURI;
#elif UNITY_ANDROID
		uri = StageBundleAndroidURI;
#else
		uri = StageBundlePcURI;
#endif

		// 에셋번들을 캐시에 있으면 로드하고, 없으면 다운로드하여 캐시폴더에 저장합니다.
		using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(uri))
		{
			yield return uwr.SendWebRequest();

			if (uwr.isNetworkError || uwr.isHttpError)
			{
				Debug.LogError(uwr.error);
			}
			else
			{
				// Get downloaded asset bundle
				stageAssetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
				IsLoadComplete = true;
			}
		}
	}

	public Object GetStageBundleObject(string objectName)
	{
		return stageAssetBundle.LoadAsset(objectName);
	}

}
