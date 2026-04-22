using UnityEditor;
using UnityEngine;

public enum CardType
{
    None = 0,
    Attack = 1,
    Defence = 2,
    Buff = 3,
    Debuff = 4
}

public enum CardEffect
{
    None = 0,
    Skip = 1,
}

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public CardEffect cardEffect;
    public int attack;
    public int defence;
    public Texture cardTexture;
}
