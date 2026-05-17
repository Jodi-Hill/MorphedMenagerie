using UnityEngine;

[CreateAssetMenu(menuName = "Media Mermaid/Create Card")]
public class CardStatistics : ScriptableObject
{
    public string cardName = string.Empty;
    public string cardDescription = string.Empty;
    public int health = 5;
    public int attack = 2;
    public Texture image;
    public GameEffect past;
    public GameEffect present;
    public GameEffect future;
}

[System.Flags]
public enum GameEffect
{
    MoreDamage = 2,
    MoreHealth = 4,
    LessDamage = 8,
    LessHealth = 16,
    AddDamage = 32,
    AddHealth = 64,
    SubtractDamage = 128,
    SubtractHealth = 256,
    Skip = 512,
}
