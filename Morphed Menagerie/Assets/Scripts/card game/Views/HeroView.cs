using UnityEngine;

public class HeroView : CombatantView
{
    public void Setup(CharacterDeck heroData)
    {
        SetupBase(heroData.health, heroData.image);
    }
}
