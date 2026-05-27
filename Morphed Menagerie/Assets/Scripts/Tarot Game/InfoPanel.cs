using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public TextMeshProUGUI cardName, cardStatsPast, cardStatsPresent, cardStatsFuture, cardDescription;

    public void ShowInfoPanel(CardStatistics cardStatistics)
    {
        cardName.text = cardStatistics.cardName;
        cardDescription.text = cardStatistics.cardDescription;
        cardStatsPast.text = $"Past: ATK ({cardStatistics.pastVal.attack}) HP ({cardStatistics.pastVal.health})";
        cardStatsPresent.text = $"Present: ATK ({cardStatistics.presentVal.attack}) HP ({cardStatistics.presentVal.health})";
        cardStatsFuture.text = $"Future: ATK ({cardStatistics.futureVal.attack}) HP ({cardStatistics.futureVal.health})";
    }
}
