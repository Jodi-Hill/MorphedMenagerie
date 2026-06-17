using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    public CardManager cardManager;
    public MatchSetupSystem matchSetupSystem;

    void Start()
    {
        switch (ActLoader.Instance.currentAct)
        {
            default:
            case Act.RiniBattle:
                if (ActLoader.Instance.currentAct != Act.RiniBattle)
                {
                    Debug.Log("Invalid act, loading Rini Battle.");
                }
                cardManager.playerDeck = ActLoader.Instance.playerRini;
                matchSetupSystem.heroDeck = ActLoader.Instance.playerRini;
                cardManager.enemyDeck = ActLoader.Instance.rini;
                matchSetupSystem.enemyDeck = ActLoader.Instance.rini;
                break;
            case Act.FaoBattle:
                cardManager.playerDeck = ActLoader.Instance.playerFao;
                matchSetupSystem.heroDeck = ActLoader.Instance.playerFao;
                cardManager.enemyDeck = ActLoader.Instance.fao;
                matchSetupSystem.enemyDeck = ActLoader.Instance.fao;
                break;
        }

        matchSetupSystem.StartBattle();
        cardManager.StartBattle();
    }
}
