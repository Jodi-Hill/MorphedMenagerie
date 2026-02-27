using UnityEngine;

namespace KeyframeMagician.UltimateParrotAnimsSkinsPack


{
public class HideInPlayMode : MonoBehaviour
{
    void Awake()
    {
        // Disable the GameObject during Play Mode
        gameObject.SetActive(false);
    }
}
}