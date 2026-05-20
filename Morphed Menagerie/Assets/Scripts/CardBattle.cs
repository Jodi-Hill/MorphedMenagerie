using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CardBattle : MonoBehaviour
{
    public int turnCount = 0;
    public CardManager cardManager;

    public void Resolve()
    {
        turnCount++;

        if (turnCount > 1)
        {
            // apply future and past to present for player
            int playerDmg = cardManager.p_present.activeCard.tempAtk;
            int playerHp = cardManager.p_present.activeCard.tempHp;
            //int playerDmg = cardManager.p_present.activeCard.presentVal.attack + cardManager.p_past.activeCard.pastVal.attack + cardManager.p_future.activeCard.futureVal.attack;
            //int playerHp = cardManager.p_present.activeCard.presentVal.health + cardManager.p_past.activeCard.pastVal.health + cardManager.p_future.activeCard.futureVal.health;
            // only use present for enemy
            int enemyDmg = cardManager.e_present.activeCard.presentVal.attack;
            int enemyHp = cardManager.e_present.activeCard.presentVal.health;

            cardManager.playerView.Damage(Mathf.Clamp(enemyDmg - playerHp, 0, 100));
            cardManager.enemyView.Damage(Mathf.Clamp(playerDmg - enemyHp, 0, 100));

            Debug.Log("Battle: \nPlayer\n" + "dmg " + playerDmg + "\nhp " + playerHp + "\nEnemy\ndmg " + enemyDmg + "\nhp " + enemyHp);
        }

        cardManager.ContinueTurn();
    }
}
