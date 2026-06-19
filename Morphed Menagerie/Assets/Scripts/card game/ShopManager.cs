using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int startPoints = 30;
    public int rinicount, suncount, foolcount, devilcount;
    public int rinicost, suncost, foolcost, devilcost;
    public CharacterDeck customDeck, enemyDeck;
    public CardStatistics rini, sun, fool, devil;

    public TextMeshProUGUI points, amountRini, amountSun, amountFool, amountDevil, riniprice, sunprice, foolprice, devilprice;
    public Button startButton;

    public UnityEvent startEvents;

    private void Start()
    {
        riniprice.text = "Cost: " + rinicost;
        sunprice.text = "Cost: " + suncost;
        foolprice.text = "Cost: " + foolcost;
        devilprice.text = "Cost: " + devilcost;
    }

    public void StartGame()
    {
        List<CardStatistics> deck = new();
        for (int i = 0; i < rinicount; i++)
        {
            deck.Add(rini);
        }
        for (int i = 0; i < suncount; i++)
        {
            deck.Add(sun);
        }
        for (int i = 0; i < foolcount; i++)
        {
            deck.Add(fool);
        }
        for (int i = 0; i < devilcount; i++)
        {
            deck.Add(devil);
        }
        customDeck.deck = deck.ToArray();
        startEvents.Invoke();
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        points.text = "Points left: " + startPoints;
        amountRini.text = rinicount.ToString();
        amountSun.text = suncount.ToString();
        amountFool.text = foolcount.ToString();
        amountDevil.text = devilcount.ToString();

        if (startPoints <= 0)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void UpRini()
    {
        if (startPoints >= rinicost)
        {
            rinicount++;
            startPoints -= rinicost;
        }
    }
    public void DownRini()
    {
        if (rinicount >= 1)
        {
            rinicount--;
            startPoints += rinicost;
        }
    }

    public void UpDevil()
    {
        if (startPoints >= devilcost)
        {
            devilcount++;
            startPoints -= devilcost;
        }
    }
    public void DownDevil()
    {
        if (devilcount >= 1)
        {
            devilcount--;
            startPoints += devilcost;
        }
    }

    public void UpFool()
    {
        if (startPoints >= foolcost)
        {
            foolcount++;
            startPoints -= foolcost;
        }
    }
    public void DownFool()
    {
        if (foolcount >= 1)
        {
            foolcount--;
            startPoints += foolcost;
        }
    }

    public void UpSun()
    {
        if (startPoints >= suncost)
        {
            suncount++;
            startPoints -= suncost;
        }
    }
    public void DownSun()
    {
        if (suncount >= 1)
        {
            suncount--;
            startPoints += suncost;
        }
    }
}
