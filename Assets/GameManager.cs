using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool gameOver;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one GameManager in scene!");
            return;
        }
        // Singleton
        instance = this;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void GameOver() {
        if (gameOver) return;
        
        gameOver = true;
        Debug.Log("Game end");
        
    }
}
