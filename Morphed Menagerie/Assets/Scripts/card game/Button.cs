using UnityEngine;
using UnityEngine.Events;

public class PlayerButton : MonoBehaviour
{
    public UnityEvent u_event;

    private void OnMouseDown()
    {
        u_event.Invoke();
    }
}
