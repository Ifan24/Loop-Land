using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public GameObject standardTurretCardPrefab;
    public GameObject missileLauncherCardPrefab;
    public GameObject laserBeamerCardPrefab;
    public GameObject destroyCardPrefab;
    
    
    public static CardDeck instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one CardDeck in scene!");
            return;
        }
        // Singleton
        instance = this;
    }
    
    // To add card to deck
    // first get the instance of this class
    // then call this method and use the public prefab to add card to deck
    public void addCardToDeck(GameObject cardPrefab) {
        GameObject obj = Instantiate(cardPrefab) as GameObject;
        obj.transform.SetParent(transform);
    }
}
