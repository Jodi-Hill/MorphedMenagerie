using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerformEffectGA : GameAction
{
    public Effect Effect {  get; set; }

    public PerformEffectGA(Effect effect)
    {
        Effect = effect;
    }
}
