using System.Collections.Generic;
using UnityEngine;

struct Card {
    public GameObject gameObject;
    public float dropRate;
    public Card(GameObject _gameObject, float _dropRate) {
        gameObject = _gameObject;
        dropRate = _dropRate;
    }
}
public class DropCardManager : Singleton<DropCardManager>
{
    // each card has a drop rate and enemy has a drop multiplier
    private CardDeck cardDeck;
    private List<Card> cards;
    private void Start() {
        cardDeck = CardDeck.instance;
        cards = new List<Card>();
        foreach(GameObject card in cardDeck.cardPrefabs) {
            DropRate dr = card.GetComponent<DropRate>();
            cards.Add(new Card(card, dr.dropRate));
        }
    }
    public void DropCard(float enemyMultiplier) {
        foreach(Card card in cards) {
            if (Random.Range(0, 1.0f) < (card.dropRate * enemyMultiplier)) {
                if (card.gameObject != null) {
                    cardDeck.addCardToDeck(card.gameObject);
                }
            }
        }
    }
}
