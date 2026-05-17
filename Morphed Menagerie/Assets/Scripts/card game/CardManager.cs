using UnityEngine;
using UnityEngine.UI;

public class CardManager : Singleton<CardManager>
{
    public CardBattle battle;
    public Button endTurn;

    [Header("Enemy")]
    public CharacterDeck enemy;
    public SetCard future;
    public SetCard present;

    [Header("Player")]
    public CharacterDeck player;
    public SetCard p_future;
    public SetCard p_present;
    public SetCard p_past;
    public CardDetection futureDetection;
    [HideInInspector] public CardStatistics p_card;

    [Header("Debug so dont assign")]
    public Transform futureTrans;
    public Transform presentTrans;
    public Transform pastTrans;

    void Start()
    {
        future.NewCard(enemy.deck[Random.Range(0, enemy.deck.Length)]);
    }

    private void Update()
    {
        if (p_future.HasBeenSet() && p_present.HasBeenSet() && p_past.HasBeenSet())
        {
            endTurn.interactable = true;
        }
        else
        {
            endTurn.interactable = false;
        }
    }

    /// <summary>
    /// Invoked by Unity Event
    /// </summary>
    public void PlayerTurn()
    {
        battle.Resolve();
    }

    public void ContinueTurn()
    {
        // new assigns
        p_present.NewCard(p_future.activeCard);
        p_past.NewCard(p_present.activeCard);

        // set positions
        futureTrans.position = presentTrans.position;
        presentTrans.position = pastTrans.position;

        // set transforms
        Destroy(pastTrans.gameObject);
        pastTrans = presentTrans;
        presentTrans = futureTrans;

        // set future
        futureTrans = null;
        futureDetection.ResetDetection();

        Debug.Log("changed cards player");
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        present.NewCard(future.activeCard);
        future.NewCard(enemy.deck[Random.Range(0, enemy.deck.Length)]);
        Debug.Log("changed cards enemy");
        DrawCardsGA drawCardsGA = new(5, true);
        ActionSystem.Instance.Perform(drawCardsGA);
    }

    public void SetPlayerCard(CardStatistics card, int timeType, Transform cardTrans)
    {
        switch(timeType)
        {
            case 0:
                p_past.NewCard(card);
                pastTrans = cardTrans;
                break;
            case 1:
                p_present.NewCard(card);
                presentTrans = cardTrans;
                break;
            case 2:
                p_future.NewCard(card);
                futureTrans = cardTrans;
                break;
        }
    }
}
