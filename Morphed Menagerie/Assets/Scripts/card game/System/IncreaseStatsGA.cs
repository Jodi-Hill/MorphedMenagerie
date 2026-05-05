using UnityEngine;

public class IncreaseStatsGA : GameAction
{
    public int AttackIncreaseAmount;
    public int HealthIncreaseAmount;

    public IncreaseStatsGA(DealDamageGA dealDamageGA, int attackIncreaseAmount, int healthIncreaseAmount)
    {
        AttackIncreaseAmount = attackIncreaseAmount;
        HealthIncreaseAmount = healthIncreaseAmount;
    }

    public object Target { get; internal set; }
}
