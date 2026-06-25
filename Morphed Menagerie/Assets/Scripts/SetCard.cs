using TMPro;
using UnityEngine;

public class SetCard : MonoBehaviour
{
    public CardStatistics activeCard;
    public InfoPanel infoPanel;
    public TextMeshPro atk, hp;
    public CardDetection cardDetection;
    private bool selected;
    public bool setStats;
    public CardOrientation timeType;

    public bool useAura;
    public GameObject aura1, aura2, aura3, aura4;

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

    public void DisableAura()
    {
        aura1.SetActive(false);
        aura2.SetActive(false);
        aura3.SetActive(false);
        aura4.SetActive(false);
    }

    public void CalculateAura(int attack)
    {
        if (activeCard == null)
        {
            aura1.SetActive(false);
            aura2.SetActive(false);
            aura3.SetActive(false);
            aura4.SetActive(false);
        }

        if (useAura && activeCard != null)
        {
            int aur = 0;
            if (attack >= 10)
            {
                aur = 4;
            }
            else if (attack >= 6)
            {
                aur = 3;
            }
            else if (attack >= 4)
            {
                aur = 2;
            }
            else if (attack >= 2)
            {
                aur = 1;
            }

            if (aur == 0)
            {
                aura1.SetActive(false);
                aura2.SetActive(false);
                aura3.SetActive(false);
                aura4.SetActive(false);
            }
            if (aur == 1 && !aura1.activeSelf)
            {
                aura1.SetActive(true);
                aura2.SetActive(false);
                aura3.SetActive(false);
                aura4.SetActive(false);
            }
            if (aur == 2 && !aura2.activeSelf)
            {
                aura1.SetActive(false);
                aura2.SetActive(true);
                aura3.SetActive(false);
                aura4.SetActive(false);
            }
            if (aur == 3 && !aura3.activeSelf)
            {
                aura1.SetActive(false);
                aura2.SetActive(false);
                aura3.SetActive(true);
                aura4.SetActive(false);
            }
            if (aur == 4 && !aura4.activeSelf)
            {
                aura1.SetActive(false);
                aura2.SetActive(false);
                aura3.SetActive(false);
                aura4.SetActive(true);
            }
        }
    }

    public bool HasBeenSet()
    {
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
