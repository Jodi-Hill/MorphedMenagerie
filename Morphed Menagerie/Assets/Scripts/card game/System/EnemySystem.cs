using System.Collections;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGa)
    {
        //Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(2f);
        //Debug.Log("End Enemy Turn");
        yield return new WaitForSeconds(2f);
        //DrawCardsGA drawCardsGA = new(5);
        //Debug.Log("Start Player Turn");
        //ActionSystem.Instance.Perform(drawCardsGA);
    }
}
