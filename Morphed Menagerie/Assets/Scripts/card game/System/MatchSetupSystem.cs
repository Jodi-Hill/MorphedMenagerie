using UnityEngine;
using System.Linq;

public class MatchSetupSystem : MonoBehaviour
{
    public CharacterDeck heroDeck;
    public CharacterDeck enemyDeck;

    public void StartBattle()
    {
        //HeroSystem.Instance.Setup(heroData);
        //EnemySystem.Instance.Setup(enemyData);
        CardSystem.Instance.Setup(heroDeck.deck.ToList());
        DrawCardsGA drawCardsGA = new(5, true);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
