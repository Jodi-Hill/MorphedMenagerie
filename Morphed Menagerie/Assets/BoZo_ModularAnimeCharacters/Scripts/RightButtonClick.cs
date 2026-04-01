using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RightButtonClick : MonoBehaviour
{
    public GameObject Panel;

    public void OpenPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;

            Panel.SetActive(!isActive);
        }
    }
}
