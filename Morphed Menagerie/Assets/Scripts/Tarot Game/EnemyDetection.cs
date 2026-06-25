using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public CardView card;
    public bool hasCard;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<CardView>())
        {
            return;
        }

        card = other.GetComponent<CardView>();
        hasCard = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (card != null)
        {
            card = null;
        }
        hasCard = false;
    }
    
    public void Damage(int value)
    {
        card.TakeDamage(value);
    }
}
