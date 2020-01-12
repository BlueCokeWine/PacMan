#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

	[SerializeField] AudioStorage audioStorage;
	[SerializeField] AudioSource bgmSource;
	[SerializeField] AudioSource ghostSource;

	ESfxId currentGhostSound = ESfxId.Click;
	EBgmId prePlayBgm = EBgmId.End;
	bool cookieFlag;

	void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void PlaySound(ESfxId id)
	{
		AudioSource.PlayClipAtPoint(audioStorage.Get(id), Vector3.zero);
	}

	public void PlaySound(EBgmId id)
	{
		AudioSource.PlayClipAtPoint(audioStorage.Get(id), Vector3.zero);
	}

	public void PlayEatCookieSound()
	{
		if(cookieFlag)
		{
			AudioSource.PlayClipAtPoint(audioStorage.Get(ESfxId.EatCookie1), Vector3.zero);
		} else
		{
			AudioSource.PlayClipAtPoint(audioStorage.Get(ESfxId.EatCookie2), Vector3.zero);
		}
		cookieFlag = !cookieFlag;
	}

	public void SetGhostSoundClip(ESfxId id)
	{
		if(id == currentGhostSound)
		{
			return;
		}

		currentGhostSound = id;
		ghostSource.clip = audioStorage.Get(id);
		ghostSource.Play();
	}

	public void PlayGhostSound(bool play)
	{
		if (play)
		{
			ghostSource.Play();
		}
		else
		{
			ghostSource.Stop();
		}
	}

	public void PlayRandomBgm()
	{
		if (bgmSource.isPlaying)
		{
			return;
		}

		int start = (int)EBgmId.StartMusic + 1;
		int end = (int)EBgmId.End;

		EBgmId randomId;

		do
		{
			randomId = (EBgmId)Random.Range(start, end);
		} while (prePlayBgm == randomId);

		StartCoroutine(StartBgm(randomId));
	}

	public void StopBgm()
	{
		if (bgmSource.isPlaying)
		{
			bgmSource.Stop();
			StopCoroutine(nameof(StartBgm));
		}
	}

	IEnumerator StartBgm(EBgmId id)
	{
		AudioClip clip = audioStorage.Get(id);
		bgmSource.clip = clip;
		bgmSource.Play();

		yield return new WaitForSeconds(clip.length);

		PlayRandomBgm();
	}

}
