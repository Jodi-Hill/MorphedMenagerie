using UnityEngine;
using TMPro;

public class SetCard : MonoBehaviour
{
    public CardStatistics activeCard;
    public InfoPanel infoPanel;
    public TextMeshPro atk, hp;
    private CardDetection cardDetection;
    private bool selected;
    public bool setStats;
    public CardOrientation timeType;

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

        if (setStats)
        {
            switch (timeType)
            {
                case CardOrientation.Past:
                    atk.text = activeCard.pastVal.attack.ToString();
                    hp.text = activeCard.pastVal.health.ToString();
                    break;
                case CardOrientation.Present:
                    atk.text = activeCard.presentVal.attack.ToString();
                    hp.text = activeCard.presentVal.health.ToString();
                    break;
                case CardOrientation.Future:
                    atk.text = activeCard.futureVal.attack.ToString();
                    hp.text = activeCard.futureVal.health.ToString();
                    break;
            }
        }
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
