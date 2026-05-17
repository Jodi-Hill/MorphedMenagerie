using UnityEngine;
using TMPro;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText;

    public int AttackPower { get; set; }

    public void Setup(CharacterDeck enemyData)
    {
        AttackPower = 0;
        UpdateAttackText();
        SetupBase(enemyData.health, enemyData.image);
    }

    private void UpdateAttackText()
    {
        attackText.text = "ATK: " + AttackPower;
    }
}
