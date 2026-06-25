using System.Collections;
using System.Collections.Generic;
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
    public GameObject cardViewPrefab, tutoScreen, tutoPlayer, playerPortrait, enemyPortrait;
    public InfoPanel infoPanel;

    [Header("Enemy")]
    public HeroView enemyView;
    public CharacterDeck enemyDeck;
    public SetCard e_future;
    public SetCard e_present;
    public EnemyDetection e_futureCard;
    public EnemyDetection e_presentCard;
    public Transform e_futureTrans;
    public Transform e_presentTrans;

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
    public GameObject e_futCard;
    public GameObject e_presCard;
    public List<GameObject> cards = new();
    public Act winScene;
    public Act loseScene;
    private float duration = 0.25f;
    public bool usedRini;
    public bool usedFao;
    private bool inAnim;
    public bool playTuto;
    public bool cardTest;

    public void StartBattle()
    {
        enemyView.Setup(enemyDeck);
        playerView.Setup(playerDeck);

        GameObject cardObj1 = Instantiate(cardViewPrefab, e_future.transform.position, Quaternion.identity);
        cardObj1.transform.localScale = Vector3.one;
        Card card1 = new(enemyDeck.deck[Random.Range(0, enemyDeck.deck.Length)]);
        CardView view1 = cardObj1.GetComponent<CardView>();
        view1.Setup(card1);
        view1.UpdateCard(CardOrientation.Future);
        e_futCard = cardObj1;
        e_future.NewCard(cardObj1.GetComponent<CardView>().cardInfo);
        view1.infopanel = infoPanel;
        view1.frozen = true;

        GameObject cardObj2 = Instantiate(cardViewPrefab, e_present.transform.position, Quaternion.identity);
        cardObj2.transform.localScale = Vector3.one;
        Card card2 = new(enemyDeck.deck[Random.Range(0, enemyDeck.deck.Length)]);
        CardView view2 = cardObj2.GetComponent<CardView>();
        view2.Setup(card2);
        view2.UpdateCard(CardOrientation.Present);
        e_presCard = cardObj2;
        e_present.NewCard(cardObj2.GetComponent<CardView>().cardInfo);
        view2.infopanel = infoPanel;
        view2.frozen = true;

        if (!playTuto)
        {
            tutoScreen.SetActive(false);
            tutoPlayer.SetActive(false);
        }
    }

    private void Update()
    {
        if (p_future.HasBeenSet() && p_present.HasBeenSet() && p_past.HasBeenSet() && !inAnim)
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
    }

    /// <summary>
    /// Invoked by Unity Event
    /// </summary>
    public void PlayerTurn()
    {
        inAnim = true;
        battle.Resolve();
    }

    public void ContinueTurn()
    {
        StartCoroutine(CardTransforms());
    }

    public void LoadWin()
    {
        if (usedRini)
        {
            ActLoader.Instance.LoadAct(Act.KilledRini);
            return;
        }
        ActLoader.Instance.LoadAct(winScene);
    }

    public void LoadLose()
    {
        ActLoader.Instance.LoadAct(loseScene);
    }

    IEnumerator CardTransforms()
    {
        //---------- PLAYER TURN -----------
        pastTrans = pastDetection.cardTrans;
        presentTrans = presentDetection.cardTrans;
        futureTrans = futureDetection.cardTrans;

        p_present.DisableAura();
        if (pastTrans != null && pastTrans.gameObject != null)
            UnityEngine.Object.Destroy(pastTrans.gameObject);
        p_past.NewCard(p_present.activeCard);
        p_present.NewCard(p_future.activeCard);
        yield return new WaitForEndOfFrame();

        // good for debug
        //Debug.Log("1");
        //cardTest = false;
        //while (!cardTest)
        //{
        //    yield return null;
        //}

        pastTrans = presentTrans;
        presentTrans = futureTrans;
        futureTrans = null;

        // Do past (present to past)
        float timeA = 0f;
        if (pastTrans != null)
        {
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
        }

        // Do present (future to present)
        float timeB = 0f;
        if (presentTrans != null)
        {
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
        }
        else
        {
            p_present.cardDetection.hasCard = false;
        }

        // Move over physical card data and then remove colliders
        yield return new WaitForEndOfFrame();
        pastDetection.card = presentDetection.card;
        pastDetection.cardTrans = presentDetection.cardTrans;
        yield return new WaitForEndOfFrame();
        presentDetection.card = futureDetection.card;
        presentDetection.cardTrans = futureDetection.cardTrans;
        yield return new WaitForEndOfFrame();
        if (pastDetection.card != null) pastDetection.card.frozen = true;
        if (presentDetection.card != null) presentDetection.card.frozen = true;
        futureDetection.card = null;
        futureDetection.cardTrans = null;
        futureDetection.ResetDetection();

        //---------- ENEMY TURN -----------
        float timeC = 0f;
        Vector3 startPosC = e_futureTrans.position;
        Vector3 endPosC = e_presentTrans.position;
        Destroy(e_presCard);

        // move future to present
        if (e_futCard != null)
        {
            e_presCard = e_futCard;
            while (timeC < duration)
            {
                float t = timeC / duration;
                e_presCard.transform.position = Vector3.Lerp(startPosC, endPosC, t);
                timeC += Time.deltaTime;
                yield return null;
            }
            e_presCard.GetComponent<CardView>().UpdateCard(CardOrientation.Present);
            e_presCard.transform.position = endPosC;
            e_present.NewCard(e_future.activeCard);
        }
        else
        {
            // generate present
            GameObject cardObj1 = Instantiate(cardViewPrefab, e_present.transform.position, Quaternion.identity);
            cardObj1.transform.localScale = Vector3.one;
            Card cardE1 = new(enemyDeck.deck[Random.Range(0, enemyDeck.deck.Length)]);
            CardView view1 = cardObj1.GetComponent<CardView>();
            view1.Setup(cardE1);
            view1.UpdateCard(CardOrientation.Present);
            e_presCard = cardObj1;
            e_present.NewCard(cardObj1.GetComponent<CardView>().cardInfo);
            view1.infopanel = infoPanel;
            view1.frozen = true;
        }

        // generate future
        GameObject cardObj = Instantiate(cardViewPrefab, e_future.transform.position, Quaternion.identity);
        cardObj.transform.localScale = Vector3.one;
        Card cardE = new(enemyDeck.deck[Random.Range(0, enemyDeck.deck.Length)]);
        CardView view = cardObj.GetComponent<CardView>();
        view.Setup(cardE);
        view.UpdateCard(CardOrientation.Future);
        e_futCard = cardObj;
        e_future.NewCard(cardObj.GetComponent<CardView>().cardInfo);
        view.infopanel = infoPanel;
        view.frozen = true;
        yield return new WaitForEndOfFrame();

        pastTrans = pastDetection.cardTrans;
        presentTrans = presentDetection.cardTrans;
        futureTrans = futureDetection.cardTrans;
        //---------- CONTINUE -----------
        DrawCardsGA drawCardsGA = new(5, true);
        ActionSystem.Instance.Perform(drawCardsGA);
        inAnim = false;
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
