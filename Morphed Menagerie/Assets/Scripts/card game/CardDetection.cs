using UnityEngine;

public class CardDetection : MonoBehaviour
{
    public bool firstTurnOnly;
    public int turn = 0;

    public CardView card;

    public void RaiseTurn()
    {
        turn++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firstTurnOnly && turn > 0 && card == null)
        {
            return;
        }

        card = other.GetComponent<CardView>();
        if (card != null)
        {
            card.thief = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (card != null)
        {
            card.thief = null;
            card = null;
        }
    }
}
