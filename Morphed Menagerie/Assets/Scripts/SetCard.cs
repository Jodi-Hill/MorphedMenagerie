using UnityEngine;

public class SetCard : MonoBehaviour
{
    public CardData activeCard;

    public void NewCard (CardData card)
    {
        activeCard = card;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = card.cardTexture;
    }
}
