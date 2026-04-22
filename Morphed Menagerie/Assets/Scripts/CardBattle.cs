using UnityEngine;

public class CardBattle : MonoBehaviour
{
    public int turnCount = 0;
    public int playerHealth = 5;
    public int enemyHealth = 5;

    public CardManager cardManager;

    public void Resolve()
    {
        turnCount++;

        if (turnCount > 1)
        {
            int pdmg = cardManager.p_present.activeCard.attack;
            int edmg = cardManager.present.activeCard.attack;
            int pdef = cardManager.p_present.activeCard.defence;
            int edef = cardManager.present.activeCard.defence;

            playerHealth -= Mathf.Clamp(edmg - pdef, 0, 100);
            enemyHealth -= Mathf.Clamp(pdmg - edef, 0, 100);
        }

        cardManager.ContinueTurn();
    }
}
