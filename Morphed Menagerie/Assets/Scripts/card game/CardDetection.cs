using UnityEngine;

public class CardDetection : MonoBehaviour
{
    public bool hasCard;
    public int turn = 0;
    public CardOrientation timeType = CardOrientation.Past;

    public CardView card;
    public Transform cardTrans;
    public SetCard linkedCard;

    private void Start()
    {
        linkedCard = GetComponent<SetCard>();
    }

    private void Update()
    {
        if (!hasCard)
        {
            card = null;
            cardTrans = null;
        }

        if (card == null)
        {
            hasCard = false;
        }
    }

    public void Damage(int value)
    {
        card.TakeDamage(value);
    }

    /// <summary>
    /// Invoked by unity events.
    /// </summary>
    public void RaiseTurn()
    {
        if (hasCard)
        {
            CardManager.Instance.SetPlayerCard(card.Card.cardInformation, timeType, cardTrans);
            CardSystem.Instance.hand.Remove(card.Card);
            CardSystem.Instance.handView.cards.Remove(card);
            card.placed = true;
            card.frozen = true;
        }
        turn++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<CardView>() || card != null)
        {
            return;
        }

        card = other.GetComponent<CardView>();
        cardTrans = other.transform;
        if (card != null && card.GetType() == typeof(CardView))
        {
            card.SetThief(transform);
            linkedCard.NewCard(card.Card.cardInformation);
            hasCard = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<CardView>() || card != null)
        {
            return;
        }

        card = other.GetComponent<CardView>();
        cardTrans = other.transform;
        if (card != null && card.GetType() == typeof(CardView))
        {
            card.SetThief(transform);
            linkedCard.NewCard(card.Card.cardInformation);
            hasCard = true;
        }
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
        linkedCard.activeCard = null;
        hasCard = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (card != null)
        {
            card = null;
            cardTrans = null;
            linkedCard.activeCard = null;
            hasCard = false;
        }
    }
}
