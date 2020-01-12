#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Fruit : Food
{
    public enum EType
    {
        Cherry,
        Strawberry,
        Orange,
        Apple,
        Melon,
        GalaxianBoss,
        Bell,
        Key
    }

    [SerializeField] SpriteAtlas fruitAtlas;

    public EType Type { get; private set; }

    public void Init(int index)
    {
        if(index > (int)EType.Key)
        {
            Type = EType.Key;
        } 
        else
        {
            Type = (EType)index;
        }

        Sprite sprite = fruitAtlas.GetSprite(Type.ToString());
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    protected override void EatEvent()
    {
        int score;

        switch (Type)
        {
            case EType.Cherry:
                score = 100;
                break;
            case EType.Strawberry:
                score = 300;
                break;
            case EType.Orange:
                score = 500;
                break;
            case EType.Apple:
                score = 700;
                break;
            case EType.Melon:
                score = 1000;
                break;
            case EType.GalaxianBoss:
                score = 2000;
                break;
            case EType.Bell:
                score = 3000;
                break;
            case EType.Key:
                score = 5000;
                break;
            default:
                score = 0;
                break;
        }

        ScoreManager.Instance.AddScore(score, Score.EType.Fruit, transform.position);
        AudioManager.Instance.PlaySound(ESfxId.EatFruit);
    }

}
