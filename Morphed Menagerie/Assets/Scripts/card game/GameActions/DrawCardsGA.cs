using UnityEngine;

public class DrawCardsGA : GameAction
{
    public bool TurnStart { get; set; }
    public int Amount { get; set; }

    public DrawCardsGA(int amount, bool start)
    {
        Amount = amount;
        TurnStart = start;
    }
}
