using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum CardOrientation
{
    Past = 0,
    Present = 1,
    Future = 2,
}

public class CardManager : Singleton<CardManager>
{
    public CardBattle battle;
    public Button endTurn;

    [Header("Enemy")]
    public HeroView enemyView;
    public CharacterDeck enemyDeck;
    public SetCard e_future;
    public SetCard e_present;

    [Header("Player")]
    public HeroView playerView;
    public CharacterDeck playerDeck;
    public SetCard p_future;
    public SetCard p_present;
    public SetCard p_past;
    public CardDetection futureDetection;
    public CardDetection presentDetection;
    public CardDetection pastDetection;

    [Header("Dont change")]
    public CardStatistics p_card;
    public Transform futureTrans;
    public Transform presentTrans;
    public Transform pastTrans;
    private float duration = 0.25f;
    public bool usedRini;
    public bool usedFao;

    public void StartBattle()
    {
        enemyView.Setup(enemyDeck);
        playerView.Setup(playerDeck);
        e_future.NewCard(enemyDeck.deck[Random.Range(0, enemyDeck.deck.Length)]);
        e_present.NewCard(enemyDeck.deck[Random.Range(0, enemyDeck.deck.Length)]);
    }

    private void Update()
    {
        if (p_future.HasBeenSet() && p_present.HasBeenSet() && p_past.HasBeenSet())
        {
            endTurn.interactable = true;
        }
        else
        {
            endTurn.interactable = false;
        }
    }

    private void FixedUpdate()
    {
        if (presentTrans != null)
        {
            presentTrans.GetComponent<CardView>().UpdateCard(
                CardOrientation.Present,
                (pastTrans != null && pastTrans.GetComponent<CardView>().Card != null) ? pastTrans.GetComponent<CardView>().Card : null,
                (futureTrans != null && futureTrans.GetComponent<CardView>().Card != null) ? futureTrans.GetComponent<CardView>().Card : null
            );
        }
        p_present.CalculateAura();
    }

    /// <summary>
    /// Invoked by Unity Event
    /// </summary>
    public void PlayerTurn()
    {
        battle.Resolve();
    }

    public void ContinueTurn()
    {
        StartCoroutine(CardTransforms());
    }

    IEnumerator CardTransforms()
    {
        p_past.NewCard(p_present.activeCard);
        p_present.NewCard(p_future.activeCard);
        UnityEngine.Object.Destroy(pastTrans.gameObject);
        yield return new WaitForEndOfFrame();

        pastTrans = presentTrans;
        presentTrans = futureTrans;
        futureTrans = null;

        // Do past
        float timeA = 0f;
        Vector3 startPosA = pastTrans.position;
        Vector3 endPosA = pastDetection.transform.position;
        while (timeA < duration)
        {
            float t = timeA / duration;
            pastTrans.position = Vector3.Lerp(startPosA, endPosA, t);
            pastTrans.eulerAngles = new Vector3(0, 0, 90 + (t * 90f));
            timeA += Time.deltaTime;
            yield return null;
        }
        // Ensure exact final position
        pastTrans.GetComponent<CardView>().UpdateCard(CardOrientation.Past);
        pastTrans.position = endPosA;
        pastTrans.eulerAngles = new Vector3(0, 0, 180);

        // Do present
        float timeB = 0f;
        Vector3 startPosB = presentTrans.position;
        Vector3 endPosB = presentDetection.transform.position;
        while (timeB < duration)
        {
            float t = timeB / duration;
            presentTrans.position = Vector3.Lerp(startPosB, endPosB, t);
            presentTrans.eulerAngles = new Vector3(0, 0, (t * 90f));
            timeB += Time.deltaTime;
            yield return null;
        }
        // Ensure exact final position
        presentTrans.GetComponent<CardView>().UpdateCard(CardOrientation.Present);
        presentTrans.position = endPosB;
        presentTrans.eulerAngles = new Vector3(0, 0, 90);

        // Move over physical card data and then remove colliders
        pastDetection.card = presentDetection.card;
        pastDetection.cardTrans = presentDetection.cardTrans;
        yield return new WaitForEndOfFrame();
        presentDetection.card = futureDetection.card;
        presentDetection.cardTrans = futureDetection.cardTrans;
        yield return new WaitForEndOfFrame();
        pastDetection.card.GetComponent<BoxCollider>().enabled = false;
        presentDetection.card.GetComponent<BoxCollider>().enabled = false;
        futureDetection.card = null;
        futureDetection.cardTrans = null;
        futureDetection.ResetDetection();
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        e_present.NewCard(e_future.activeCard);
        e_future.NewCard(enemyDeck.deck[Random.Range(0, enemyDeck.deck.Length)]);
        DrawCardsGA drawCardsGA = new(5, true);
        ActionSystem.Instance.Perform(drawCardsGA);
    }

    public void SetPlayerCard(CardStatistics card, CardOrientation timeType, Transform cardTrans)
    {
        switch(timeType)
        {
            case CardOrientation.Past:
                p_past.NewCard(card);
                pastTrans = cardTrans;
                pastTrans.GetComponent<CardView>().UpdateCard(timeType);
                break;
            case CardOrientation.Present:
                p_present.NewCard(card);
                presentTrans = cardTrans;
                presentTrans.GetComponent<CardView>().UpdateCard(timeType);
                break;
            case CardOrientation.Future:
                p_future.NewCard(card);
                futureTrans = cardTrans;
                futureTrans.GetComponent<CardView>().UpdateCard(timeType);
                break;
        }
    }
}
