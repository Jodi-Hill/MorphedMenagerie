using UnityEngine;

public class CardBattle : MonoBehaviour
{
    public int turnCount = 0;
    public CardManager cardManager;
    public VFXTrail player;
    public VFXTrail enemy;

    private int playerDmg, playerHp, enemyDmg, enemyHp;

    public void Resolve()
    {
        turnCount++;
        cardManager.CalculateValues();

        // apply future and past to present for player
        playerDmg = cardManager.p_present.activeCard.tempAtk;
        playerHp = cardManager.p_present.activeCard.tempHp;
        // only use present for enemy
        if (cardManager.e_present.activeCard != null)
        {
            enemyDmg = cardManager.e_present.activeCard.presentVal.attack;
            enemyHp = cardManager.e_present.activeCard.presentVal.health;
        }

        player.damageCounter = playerDmg;
        player.cardHealth = enemyHp; 
        enemy.damageCounter = enemyDmg;
        enemy.cardHealth = playerHp;
        player.StartAnimation(PlayerHitTarget);
    }

    public void PlayerHitTarget()
    {
        cardManager.enemyView.Damage(Mathf.Clamp(playerDmg - enemyHp, 0, 100));
        enemy.StartAnimation(EnemyHitTarget);
    }

    public void EnemyHitTarget()
    {
        cardManager.playerView.Damage(Mathf.Clamp(enemyDmg - playerHp, 0, 100));
        cardManager.ContinueTurn();
    }

    public void WinGame()
    {
        cardManager.LoadWin();
    }

    public void LoseGame()
    {
        cardManager.LoadLose();
    }
}
