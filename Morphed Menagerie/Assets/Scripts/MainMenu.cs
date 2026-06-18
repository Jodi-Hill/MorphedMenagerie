using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartStory()
    {
        ActLoader.Instance.LoadAct(Act.ZooRini);
    }

    public void StartBattle()
    {
        ActLoader.Instance.LoadAct(Act.BattleMode);
    }

    public void ThankYou()
    {
        ActLoader.Instance.LoadAct(Act.ThankYou);
    }

    public void Menu()
    {
        ActLoader.Instance.LoadAct(Act.Menu);
    }
}
