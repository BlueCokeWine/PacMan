#pragma warning disable CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESfxId
{
	Click,
	StageClear,
	Death,
	EatFruit,
	EatGhost,
	EatCookie1,
	EatCookie2,
	EatPowerCookie,
	GhostRetreat,
	GhostSiren,
}

public enum EBgmId
{
	Entrance,
	StartMusic,
	PacRainbow,
	PacDimensions,
	PacAvenue,
	PacLogic,
	PacManCeBgm,
	End
}

[Serializable]
public struct SfxSrc
{
	[SerializeField] AudioClip soundFile;
	[SerializeField] ESfxId sfxId;

	public AudioClip SoundFile { get { return soundFile; } }
	public ESfxId SfxId { get { return sfxId; } }
}

[Serializable]
public struct BgmSrc
{
	[SerializeField] AudioClip soundFile;
	[SerializeField] EBgmId bgmId;

	public AudioClip SoundFile { get { return soundFile; } }
	public EBgmId BgmId { get { return bgmId; } }
}

[CreateAssetMenu]
public class AudioStorage : ScriptableObject
{

	[SerializeField] List<SfxSrc> sfxSrcList;
	[SerializeField] List<BgmSrc> bgmSrcList;

	Dictionary<ESfxId, AudioClip> sfxDic = new Dictionary<ESfxId, AudioClip>();
	Dictionary<EBgmId, AudioClip> bgmDic = new Dictionary<EBgmId, AudioClip>();

	public AudioClip Get(ESfxId id)
	{
		Debug.Assert(sfxSrcList.Count > 0, "No SFX Source Data!");

		if (sfxDic.Count == 0)
		{
			GenerateSfxDictionary();
		}

		return sfxDic[id];
	}

	public AudioClip Get(EBgmId id)
	{
		Debug.Assert(bgmSrcList.Count > 0, "No BGM Source Data!");

		if(bgmDic.Count == 0)
		{
			GenerateBgmDictionary();
		}

		return bgmDic[id];
	}

	void GenerateSfxDictionary()
	{
		foreach (var child in sfxSrcList)
		{
			sfxDic.Add(child.SfxId, child.SoundFile);
		}


	}

	void GenerateBgmDictionary()
	{
		foreach (var child in bgmSrcList)
		{
			bgmDic.Add(child.BgmId, child.SoundFile);
		}
	}

}
