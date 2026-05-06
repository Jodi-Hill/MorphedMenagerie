using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [field: SerializeField] public HeroView HeroView {  get; set; }

    public void Setup(HeroData heroData)
    {
        HeroView.Setup(heroData);
    }
}
