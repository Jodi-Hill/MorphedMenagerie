using UnityEngine;

public class CardDetection : MonoBehaviour
{
    public bool firstTurnOnly;
    public bool hasCard;
    public int turn = 0;
    public CardOrientation timeType = CardOrientation.Past;

    public CardView card;
    public Transform cardTrans;
    private SetCard linkedCard;
    private Collider cardCollider;

    private void Start()
    {
        linkedCard = GetComponent<SetCard>();
        cardCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!hasCard)
        {
            card = null;
            cardTrans = null;
        }
    }

    /// <summary>
    /// Invoked by unity events.
    /// </summary>
    public void RaiseTurn()
    {
        if ((firstTurnOnly && turn == 0) || !firstTurnOnly)
        {
            CardManager.Instance.SetPlayerCard(card.Card.cardInformation, timeType, cardTrans);
            CardSystem.Instance.hand.Remove(card.Card);
            CardSystem.Instance.handView.cards.Remove(card);
            card.placed = true;
        }
        turn++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firstTurnOnly && turn > 0)
        {
            return;
        }

        if (!other.GetComponent<CardView>())
        {
            return;
        }

        card = other.GetComponent<CardView>();
        cardTrans = other.transform;
        if (card != null && card.GetType() == typeof(CardView))
        {
            card.SetThief(transform);
            linkedCard.NewCard(card.Card.cardInformation);
            cardCollider.enabled = false;
            hasCard = true;
        }
    }

    public bool CanThief()
    {
        return !(firstTurnOnly && turn > 0);
    }

    public void RemovedThief()
    {
        if (card != null)
        {
            ResetDetection();
        }
    }

    public void ResetDetection()
    {
        cardCollider.enabled = true;
        linkedCard.activeCard = null;
        hasCard = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (firstTurnOnly && turn > 0)
        {
            return;
        }

        if (card != null)
        {
            card = null;
            cardTrans = null;
            linkedCard.activeCard = null;
            if (CanThief())
            {
                cardCollider.enabled = true;
            }
        }
    }
}
