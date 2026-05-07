using UnityEngine;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    public string Title => data.name;
    public string Description => data.Description;
    public Sprite Image => data.Image;
    public Effect ManualTargetEffect => data.ManualTargetEffect;

    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;

    public int Mana {  get; private set; }

    private CardDataTut data;
    internal Sprite sprite;
    private CardDataTut cardDataTut;

    public Card(CardDataTut cardDataTut)
    {
        data = cardDataTut;
        Mana = cardDataTut.Mana;
        this.cardDataTut = cardDataTut;
    }
}
