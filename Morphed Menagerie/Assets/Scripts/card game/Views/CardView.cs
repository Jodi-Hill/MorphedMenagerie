using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private GameObject image;
    private Material imageR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    public Card Card { get; private set; }
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    private Vector3 startScale;

    public Transform thief;

    public void SetStartPos()
    {
        startScale = transform.localScale;
    }

    public void Setup(Card card)
    {
        imageR = image.GetComponent<Renderer>().material;
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageR.mainTexture = card.Image;
    }

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        transform.localScale = startScale * 1.2f;
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        transform.localScale = startScale;
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;

        Interactions.Instance.PlayerIsDragging = true;
        wrapper.SetActive(true);
        transform.localScale = startScale;
        if (thief == null)
        {
            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1) - Vector3.back * 4;
    }

    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;

        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1) - Vector3.back * 4;
    }

    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;

        if (thief != null && Vector3.Distance(transform.position, thief.position) < 2f)
        {
            if (thief.GetComponent<CardDetection>().CanThief())
            {
                transform.position = thief.transform.position - Vector3.forward;
            }
            else
            {
                thief = null;
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }
        }
        else
        {
            if (thief != null)
            {
                thief.GetComponent<CardDetection>().RemovedThief();
            }
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
        }
        Interactions.Instance.PlayerIsDragging = false;
    }

    public void SetThief(Transform transform)
    {
        if (thief != null && thief != transform)
        {
            thief.GetComponent<CardDetection>().RemovedThief();
        }
        thief = transform;
    }
}
