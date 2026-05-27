using UnityEngine;

public class SetCard : MonoBehaviour
{
    public CardStatistics activeCard;
    public InfoPanel infoPanel;

    public void NewCard (CardStatistics card)
    {
        activeCard = card;
        GetComponent<Renderer>().material.mainTexture = card.image;
    }

    public bool HasBeenSet()
    {
        return activeCard != null;
    }

    public void ShowInfo()
    {
        if (activeCard != null)
        {
            infoPanel.gameObject.SetActive(true);
            infoPanel.ShowInfoPanel(activeCard);
        }
    }
}
