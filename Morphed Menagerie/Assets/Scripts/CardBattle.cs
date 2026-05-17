using UnityEngine;

public class CardBattle : MonoBehaviour
{
    public int turnCount = 0;
    public CardManager cardManager;

    public void Resolve()
    {
        turnCount++;

        if (turnCount > 1)
        {
            int pdmg = cardManager.p_present.activeCard.attack;
            int edmg = cardManager.present.activeCard.attack;
            int pdef = cardManager.p_present.activeCard.health;
            int edef = cardManager.present.activeCard.health;

            cardManager.player.health -= Mathf.Clamp(edmg - pdef, 0, 100);
            cardManager.enemy.health -= Mathf.Clamp(pdmg - edef, 0, 100);
        }

        Debug.Log("did battle");

        cardManager.ContinueTurn();
    }
}
