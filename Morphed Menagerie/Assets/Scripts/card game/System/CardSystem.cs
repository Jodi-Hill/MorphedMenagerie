using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] public HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;
    public InfoPanel infoPanel;

    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> drawnPile = new();
    public List<Card> hand = new();

    private int cardsDrawn = 0;
    private int maxHandSize = 5;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    public void Setup(List<CardStatistics> deckData)
    {
        foreach (var cardDataTut in deckData)
        {
            Card card = new(cardDataTut);
            drawPile.Add(card);
        }
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA DrawCardsGA)
    {
        int actualAmount = 0;
        CardManager.Instance.cards = CardManager.Instance.cards.Where(item => item != null).ToList();
        int handSize = CardManager.Instance.cards.Count;
        foreach (GameObject card in CardManager.Instance.cards)
        {
            if (card != null && card.GetComponent<CardView>().frozen)
            {
                handSize--;
            }
        }

        if (DrawCardsGA.TurnStart)
        {
            actualAmount = Mathf.Min(DrawCardsGA.Amount - handSize, maxHandSize);
        }
        else
        {
            actualAmount = Mathf.Min(DrawCardsGA.Amount, maxHandSize);
        }
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        yield return null;
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);
        yield return DiscardCard(cardView);
        SpendManaGA spendManaGA = new(playCardGA.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);
    }

    private IEnumerator DrawCard()
    {
        if (drawPile.Count <= 0)
        {
            RefillDeck();
        }

        cardsDrawn++;
        Card card = drawPile.Draw();
        hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        cardView.name = cardView.cardInfo.name + cardsDrawn;
        cardView.infopanel = infoPanel;
        CardManager.Instance.cards.Add(cardView.gameObject);
        drawnPile.Add(card);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        drawPile.AddRange(drawnPile);
        drawnPile.Clear();
        //drawPile.AddRange(discardPile);
        //discardPile.Clear();
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.Card);
        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOScale(Vector3.zero, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}
