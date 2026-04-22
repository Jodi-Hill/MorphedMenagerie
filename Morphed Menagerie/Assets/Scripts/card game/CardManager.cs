using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardBattle battle;

    [Header("Enemy")]
    public CardData[] enemyCards;
    public SetCard future;
    public SetCard present;

    [Header("Player")]
    public SetCard p_future;
    public SetCard p_present;
    public SetCard p_past;
    [HideInInspector]
    public CardData p_card;
    public CardData p_empty;

    void Start()
    {
        future.NewCard(enemyCards[Random.Range(0, enemyCards.Length)]);
    }

    public void EnemyTurn()
    {
        present.NewCard(future.activeCard);
        future.NewCard(enemyCards[Random.Range(0, enemyCards.Length)]);
    }

    /// <summary>
    /// Invoked by Unity Event
    /// </summary>
    public void PlayerTurn()
    {
        if (p_future.activeCard == null || p_future.activeCard == p_empty)
            return;

        battle.Resolve();
    }

    public void ContinueTurn()
    {
        if (p_present.activeCard == null)
            p_past.NewCard(p_empty);
        else
            p_past.NewCard(p_present.activeCard);
        p_present.NewCard(p_future.activeCard);
        //p_future.NewCard(p_empty); // resets future card to empty

        EnemyTurn();
    }

    public void SetPlayerCard(CardData card)
    {
        p_future.NewCard(card);
    }


    //if specific card (Devil) is on specific position (present) with specific card on specific opponent position (opponentpresent), this event happens: blabla.
}
