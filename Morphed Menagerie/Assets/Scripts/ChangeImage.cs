using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image original;
    public Sprite newSprite;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void NewImage()
    {
        original.sprite = newSprite;
    }
}
