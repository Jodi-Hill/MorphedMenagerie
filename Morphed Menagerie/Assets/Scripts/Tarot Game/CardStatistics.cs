using UnityEngine;

[CreateAssetMenu(menuName = "Media Mermaid/Create Card")]
public class CardStatistics : ScriptableObject
{
    public string cardName = string.Empty;
    public string cardDescription = string.Empty;
    public Texture image;
    public CardValues pastVal;
    public CardValues presentVal;
    public CardValues futureVal;
    public CardEffect cardEffect;
}

[System.Serializable]
public class CardValues
{
    public int attack;
    public int health;
}
