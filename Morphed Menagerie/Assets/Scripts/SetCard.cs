using UnityEngine;

public class SetCard : MonoBehaviour
{
    public CardStatistics activeCard;
    public InfoPanel infoPanel;
    private CardDetection cardDetection;
    private bool selected;

    private void Start()
    {
        cardDetection = GetComponent<CardDetection>();
    }

    private void Update()
    {
        if (selected && Input.GetMouseButton(1) && activeCard != null)
        {
            infoPanel.gameObject.SetActive(true);
            infoPanel.ShowInfoPanel(activeCard);
        }
    }

    private void OnMouseEnter()
    {
        selected = true;
    }

    private void OnMouseExit()
    {
        selected = false;
    }

    public void NewCard (CardStatistics card)
    {
        activeCard = card;
        GetComponent<Renderer>().material.mainTexture = card.image;
    }

    public bool HasBeenSet()
    {
        if (cardDetection.firstTurnOnly && cardDetection.turn > 0)
        {
            return true;
        }
        return cardDetection.hasCard;
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
