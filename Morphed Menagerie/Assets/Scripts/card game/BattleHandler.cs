using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    public CardManager cardManager;
    public MatchSetupSystem matchSetupSystem;
    public Sprite riniAlly;
    public SpriteRenderer allySlot;
    public GameObject allySlotPrefab;

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
                cardManager.winScene = Act.RiniOutro;
                cardManager.loseScene = Act.RiniLose;
                allySlotPrefab.SetActive(false);
                break;
            case Act.FaoBattle:
                cardManager.playerDeck = ActLoader.Instance.playerFao;
                matchSetupSystem.heroDeck = ActLoader.Instance.playerFao;
                cardManager.enemyDeck = ActLoader.Instance.fao;
                matchSetupSystem.enemyDeck = ActLoader.Instance.fao;
                cardManager.winScene = Act.FaoNoSacri;
                cardManager.loseScene = Act.FaoLose;
                allySlot.sprite = riniAlly;
                cardManager.playTuto = false;
                break;
        }

        matchSetupSystem.StartBattle();
        cardManager.StartBattle();
    }
}
