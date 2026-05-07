using UnityEngine;
using System.Collections.Generic;

public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;

    public override GameAction GetGameAction(List<CombatantView> targets)
    {
       // List<CombatantView> targets = new(EnemySystem.Instance.Enemies);
        DealDamageGA dealDamageGA = new(damageAmount, targets);
        return dealDamageGA;
    }
}
