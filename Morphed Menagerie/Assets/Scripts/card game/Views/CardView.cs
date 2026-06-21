using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text attack;
    [SerializeField] private TMP_Text health;
    [SerializeField] private GameObject image;
    private Material imageR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    public Card Card { get; private set; }
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    private Vector3 startScale;
    private Vector3 startPosition;
    public InfoPanel infopanel;

    public CardOrientation orientation = CardOrientation.Past;
    public CardStatistics cardInfo;
    public Transform thief;
    public bool placed;
    public bool frozen;
    private bool selected;
    private Card past, future;

    public int attackValue = 0;
    public int healthValue = 0;

    private void Start()
    {
        startScale = Vector3.one;
    }

    private void Update()
    {
        if (selected && Input.GetMouseButton(1))
        {
            infopanel.gameObject.SetActive(true);
            infopanel.ShowInfoPanel(cardInfo);
        }
    }

    public void SetStartPos()
    {
        startPosition = transform.localPosition;
    }

    public void Setup(Card card)
    {
        imageR = image.GetComponent<Renderer>().material;
        Card = card;
        cardInfo = card.cardInformation;
        title.text = card.Title;
        description.text = card.Description;
        attack.text = card.cardInformation.futureVal.attack.ToString();
        health.text = card.cardInformation.futureVal.health.ToString();
        imageR.mainTexture = card.Image; 
    }

    public void UpdateCard(CardOrientation newOrientation, Card cardpast = null, Card cardfuture = null)
    {
        orientation = newOrientation;
        past = cardpast;
        future = cardfuture;
        CalculateValues();
    }

    public void CalculateValues()
    {
        switch (orientation)
        {
            case CardOrientation.Past:
                attackValue = cardInfo.pastVal.attack;
                healthValue = cardInfo.pastVal.health;
                break;
            case CardOrientation.Present:
                attackValue = cardInfo.presentVal.attack;
                healthValue = cardInfo.presentVal.health;
                break;
            case CardOrientation.Future:
                attackValue = cardInfo.futureVal.attack;
                healthValue = cardInfo.futureVal.health;
                break;
        }

        if (future != null)
        {
            attackValue += future.cardInformation.futureVal.attack;
            healthValue += future.cardInformation.futureVal.health;
        }
        if (past != null)
        {
            attackValue += past.cardInformation.pastVal.attack;
            healthValue += past.cardInformation.pastVal.health;
        }

        cardInfo.tempAtk = attackValue;
        cardInfo.tempHp = healthValue;

        attack.text = attackValue.ToString();
        health.text = healthValue.ToString();
    }

    private void OnMouseEnter()
    {
        selected = true;
        if (!Interactions.Instance.PlayerCanHover() || frozen) return;
        transform.localScale = startScale * 1.2f;
    }

    private void OnMouseExit()
    {
        selected = false;
        if (!Interactions.Instance.PlayerCanHover() || frozen) return;
        transform.localScale = startScale;
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract() || frozen) return;

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
        if (!Interactions.Instance.PlayerCanInteract() || frozen) return;

        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1) - Vector3.back * 4;
    }

    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract() || frozen) return;

        if (thief != null && Vector3.Distance(transform.position, thief.position) < 2f)
        {
            CardDetection cd = thief.GetComponent<CardDetection>();
            if ((!cd.hasCard || cd.card == this) && cd.CanThief())
            {
                transform.position = thief.transform.position - Vector3.forward;
                if (CardManager.Instance.futureTrans == transform) CardManager.Instance.futureTrans = null;
                if (CardManager.Instance.pastTrans == transform) CardManager.Instance.pastTrans = null;
                if (CardManager.Instance.presentTrans == transform) CardManager.Instance.presentTrans = null;
                CardManager.Instance.SetPlayerCard(cardInfo, thief.GetComponent<CardDetection>().timeType, transform);
                orientation = thief.GetComponent<CardDetection>().timeType;
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
                thief = null;
                if (CardManager.Instance.futureTrans == transform) CardManager.Instance.futureTrans = null;
                if (CardManager.Instance.pastTrans == transform) CardManager.Instance.pastTrans = null;
                if (CardManager.Instance.presentTrans == transform) CardManager.Instance.presentTrans = null;
                attackValue = cardInfo.futureVal.attack;
                healthValue = cardInfo.futureVal.health;
                attack.text = attackValue.ToString();
                health.text = healthValue.ToString();
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

    public void ForceRemoval()
    {
        thief = null;
        transform.position = dragStartPosition;
        transform.rotation = dragStartRotation;
    }
}
