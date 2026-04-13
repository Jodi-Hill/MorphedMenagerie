using UnityEngine;
public enum MouseOptions
{
    Left=0,
    Middle=2,
    Right=1,
}

public class TextAppear : MonoBehaviour
{
    public bool Entered;
    public GameObject textAppear;
    public MouseOptions mouseOptions;

    void Update()
    {
        if (Entered)
        {
            if (Input.GetMouseButtonDown((int)mouseOptions))
            {
                Switch();
            }
        }
    }

    public void Switch()
    {
        textAppear.SetActive(!textAppear.activeSelf);
    }

    private void OnMouseEnter()
    {
        Entered = true;
    }

    private void OnMouseExit()
    {
        Entered = false;
    }
}
