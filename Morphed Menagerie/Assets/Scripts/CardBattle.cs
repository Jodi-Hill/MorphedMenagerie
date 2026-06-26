using UnityEngine;

public class CardBattle : MonoBehaviour
{
    public int turnCount = 0;
    public int speedSlow, speedNormal, speedFast;
    public CardManager cardManager;
    public VFXTrail player, player2;
    public VFXTrail enemy, enemy2;

    private int readyCount = 0;
    private int enemyCount = 0;

    public void UpdateSpeed(int value)
    {
        switch(value)
        {
            case 0:
                player.ChangeSpeed(speedSlow);
                player2.ChangeSpeed(speedSlow);
                enemy.ChangeSpeed(speedSlow);
                enemy2.ChangeSpeed(speedSlow);
                break;
            case 1:
                player.ChangeSpeed(speedNormal);
                player2.ChangeSpeed(speedNormal);
                enemy.ChangeSpeed(speedNormal);
                enemy2.ChangeSpeed(speedNormal);
                break;
            case 2:
                player.ChangeSpeed(speedFast);
                player2.ChangeSpeed(speedFast);
                enemy.ChangeSpeed(speedFast);
                enemy2.ChangeSpeed(speedFast);
                break;
        }
    }

    public void Resolve()
    {
        readyCount = 0;
        turnCount++;
        enemyCount = 0;

        // TODO check formation to decide playing field
        // buff center card
        Transform[] buff1 = new Transform[] { cardManager.p_past.transform, cardManager.p_present.transform  };
        Transform[] buff2 = new Transform[] { cardManager.p_future.transform, cardManager.p_present.transform  };
        player.StartAnimation(PlayerBuffed, buff1, true);
        player2.StartAnimation(PlayerBuffed, buff2, true);
    }

    public void PlayerBuffed()
    {
        readyCount++;

        // TODO check formation to decide playing field
        if (readyCount >= 2)
        {
            Transform[] path = new Transform[] { cardManager.p_present.transform, cardManager.e_presentCard.transform, cardManager.enemyPortrait.transform };
            player.StartAnimation(PlayerHitTarget, path, false);
        }
    }

    public void PlayerHitTarget()
    {
        // TODO check formation to decide playing field
        if (cardManager.e_presentCard.card != null) enemyCount++;
        if (cardManager.e_futureCard.card != null) enemyCount++;

        if (cardManager.e_presentCard.card != null)
        {
            Transform[] path = new Transform[] { cardManager.e_presentCard.transform, cardManager.p_present.transform, cardManager.playerPortrait.transform };
            enemy.StartAnimation(EnemyHitTarget, path, false);
        }
        if (cardManager.e_futureCard.card != null)
        {
            Transform[] path2 = new Transform[] { cardManager.e_futureCard.transform, cardManager.p_future.transform, cardManager.playerPortrait.transform };
            enemy2.StartAnimation(EnemyHitTarget, path2, false);
        }
    }

    public void EnemyHitTarget()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            cardManager.ContinueTurn();
        }
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
