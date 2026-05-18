using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private CharacterDeck heroData;
    [SerializeField] private List<CharacterDeck> enemyDatas;

    private void Start()
    {
        //HeroSystem.Instance.Setup(heroData);
        //EnemySystem.Instance.Setup(enemyDatas);
        CardSystem.Instance.Setup(heroData.deck.ToList());
        DrawCardsGA drawCardsGA = new(5, true);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
