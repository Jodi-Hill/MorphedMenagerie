using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBattle : MonoBehaviour
{
    public int turnCount = 0;
    public CardManager cardManager;
    public string sceneWin;
    public string sceneLose;
    public VFXTrail player;
    public VFXTrail enemy;

    private int playerDmg, playerHp, enemyDmg, enemyHp;

    public void Resolve()
    {
        turnCount++;

        // apply future and past to present for player
        playerDmg = cardManager.p_present.activeCard.tempAtk;
        playerHp = cardManager.p_present.activeCard.tempHp;
        // only use present for enemy
        if (cardManager.e_present.activeCard != null)
        {
            enemyDmg = cardManager.e_present.activeCard.presentVal.attack;
            enemyHp = cardManager.e_present.activeCard.presentVal.health;
        }

        //Debug.Log("Battle: \nPlayer\n" + "dmg " + playerDmg + "\nhp " + playerHp + "\nEnemy\ndmg " + enemyDmg + "\nhp " + enemyHp);
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
        SceneManager.LoadScene(sceneWin);
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(sceneLose);
    }
}
