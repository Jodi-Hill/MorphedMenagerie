using UnityEngine;

[CreateAssetMenu(menuName = "Media Mermaid/Create Character")]
public class CharacterDeck : ScriptableObject
{
    public string characterName = string.Empty;
    public int health = 25;
    public Sprite image;
    public CardStatistics[] deck;
}
