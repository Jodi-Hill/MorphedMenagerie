using UnityEngine;

public class AllySacrifice : MonoBehaviour
{
    public GameObject question;
    public SpriteRenderer allySprite;
    public CardStatistics riniCard;
    public GameObject futureSlot;
    public GameObject cardPrefab;

    private bool selected;
    private Color startColor;

    private void Start()
    {
        startColor = allySprite.color;
    }

    private void Update()
    {
        if (selected && Input.GetMouseButtonDown(0))
        {
            question.SetActive(true);
        }
    }

    public void Sacrifice()
    {
        SetCard setCard = futureSlot.GetComponent<SetCard>();
        CardDetection detection = futureSlot.GetComponent<CardDetection>();
        if (detection.hasCard)
        {
            // remove card from future slot if any
            CardView currentfuture = detection.card;
            currentfuture.ForceRemoval();
        }

        // set future slot data
        Card card = new(riniCard);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, futureSlot.transform.position, Quaternion.identity);
        cardView.name = cardView.cardInfo.name + " sacrifice";
        setCard.NewCard(cardView.cardInfo);
        detection.card = cardView;
        detection.cardTrans = cardView.transform;

        // set card and card detection on future slot
        CardManager.Instance.usedRini = true;
        CardManager.Instance.SetPlayerCard(card.cardInformation, CardOrientation.Future, cardView.transform);

        // prevent removal by freezing
        cardView.frozen = true;

        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        selected = false;
        allySprite.color = startColor;
    }

    private void OnMouseEnter()
    {
        selected = true;
        allySprite.color = Color.red;
    }

    private void OnMouseExit()
    {
        selected = false;
        allySprite.color = startColor;
    }
}
