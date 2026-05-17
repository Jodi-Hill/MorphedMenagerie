using UnityEngine;

public class CardDetection : MonoBehaviour
{
    public bool firstTurnOnly;
    public int turn = 0;
    public int timeType = 0; // 0=past, 1=present, 2=future

    public CardView card;
    public Transform cardTrans;
    public SetCard linkedCard;
    public Collider cardCollider;

    /// <summary>
    /// Invoked by unity events.
    /// </summary>
    public void RaiseTurn()
    {
        turn++;
        CardManager.Instance.SetPlayerCard(card.Card.cardInformation, timeType, cardTrans);
        CardSystem.Instance.hand.Remove(card.Card);
        CardSystem.Instance.handView.cards.Remove(card);
        UnityEngine.Object.Destroy(card);
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
        card = null;
        cardTrans = null;
        linkedCard.activeCard = null;
        cardCollider.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
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
