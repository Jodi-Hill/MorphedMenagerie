using UnityEngine;
using System.Collections.Generic;
using SerializeReferenceEditor;

[CreateAssetMenu(menuName = "Data/Card")]
public class CardDataTut : ScriptableObject
{
    [field: SerializeField] public string Description {  get; private set; }
    [field: SerializeField] public int Mana {  get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeReference, SR] public List<Effect> Effects { get; private set; }
}
