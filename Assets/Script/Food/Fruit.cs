using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Fruit : Food
{
    public enum EType
    {
        Cherry = 100,
        Strawberry = 300,
        Orange = 500,
        Apple = 700,
        Melon = 1000,
        GalaxianBoss = 2000,
        Bell = 3000,
        Key = 5000
    }

    [SerializeField] SpriteAtlas fruitAtlas;

    EType type;

    public void Init(EType type)
    {
        this.type = type;

        Sprite sprite = fruitAtlas.GetSprite(type.ToString());
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    protected override void EatEvent()
    {
        ScoreManager.Instance.AddScore((int)type);
    }

}
