using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    public Card Card { get; private set; }
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;

    public Transform thief;

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;

        NormalizeSpriteSize();
    }

    //size of imageSR
    private void NormalizeSpriteSize()
    {
        if (imageSR.sprite == null) return;
        
        Sprite sprite = imageSR.sprite;

        float targetWidth = 3f;
        float targetHeight = 3f;

        Vector2 spriteSize = sprite.bounds.size;

        float scaleX = targetWidth / spriteSize.x;
        float scaleY = targetHeight / spriteSize.y;

        float scale = Mathf.Min(scaleX, scaleY);

        imageSR.transform.localScale = new Vector3(scale, scale, 1f);
    }

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, transform.position.y, 4.1f);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if(Card.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1) - Vector3.back * 4;
        }
    }

    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1) - Vector3.back * 4;
    }

    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        //if (Card.ManualTargetEffect != null)
        //{
        //    EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1) - Vector3.back * 4);
        //    if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
        //    {
        //        PlayCardGA playCardGA = new(Card, target);
        //        ActionSystem.Instance.Perform(playCardGA);
        //    }
        //}
        //else
        //{
            /*if (ManaSystem.Instance.HasEnoughMana(Card.Mana)
                    && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGA playCardGA = new(Card);
                ActionSystem.Instance.Perform(playCardGA);
            }*/
            if (thief != null)
            {
                transform.position = thief.transform.position - Vector3.forward;
            }
            else
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }
            Interactions.Instance.PlayerIsDragging = false;
        //}
    }
}
