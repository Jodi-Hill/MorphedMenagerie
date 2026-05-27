using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBattle : MonoBehaviour
{
    public int turnCount = 0;
    public CardManager cardManager;
    public SceneAsset sceneWin;
    public SceneAsset sceneLose;

    public void Resolve()
    {
        turnCount++;

        if (turnCount > 1)
        {
            // apply future and past to present for player
            int playerDmg = cardManager.p_present.activeCard.tempAtk;
            int playerHp = cardManager.p_present.activeCard.tempHp;
            // only use present for enemy
            int enemyDmg = cardManager.e_present.activeCard.presentVal.attack;
            int enemyHp = cardManager.e_present.activeCard.presentVal.health;

            cardManager.playerView.Damage(Mathf.Clamp(enemyDmg - playerHp, 0, 100));
            cardManager.enemyView.Damage(Mathf.Clamp(playerDmg - enemyHp, 0, 100));

            Debug.Log("Battle: \nPlayer\n" + "dmg " + playerDmg + "\nhp " + playerHp + "\nEnemy\ndmg " + enemyDmg + "\nhp " + enemyHp);
        }

        cardManager.ContinueTurn();
    }

    public void WinGame()
    {
        SceneManager.LoadScene(sceneWin.name);
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(sceneLose.name);
    }
}
