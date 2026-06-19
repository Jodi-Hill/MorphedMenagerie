using UnityEngine;

public class Card
{
    public string Title => data.name;
    public string Description => data.cardDescription;
    public Texture Image => data.image;

    public int Mana {  get; private set; }

    private CardStatistics data;
    internal Sprite sprite;
    public CardStatistics cardInformation;

    public Card(CardStatistics cardDataTut)
    {
        data = cardDataTut;
        Mana = 0;
        this.cardInformation = cardDataTut;
    }
}
