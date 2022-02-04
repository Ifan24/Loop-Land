using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public List<GameObject> cardPrefabs;
    [SerializeField] private int maxNumberOfCard = 10;
    
    public static CardDeck instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one CardDeck in scene!");
            return;
        }
        // Singleton
        instance = this;
    }
    private void Update() {
        // too much cards at hand, destroy the first card    
        if (transform.childCount >= maxNumberOfCard) {
            // TODO: some effects?
            Destroy(transform.GetChild(0).gameObject);
        }
    }
    // To add card to deck
    // first get the instance of this class
    // then call this method and use the public prefab to add card to deck
    public void addCardToDeck(GameObject cardPrefab) {
        GameObject obj = Instantiate(cardPrefab) as GameObject;
        obj.transform.SetParent(transform);
    }
}
