using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public static GameManager instance;
    public static bool isGameOver;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one GameManager in scene!");
            return;
        }
        // Singleton
        instance = this;
    }

    private void Start() {
        isGameOver = false;
    }
    private void Update() {
        // if (Input.GetKeyDown(KeyCode.E)) {
        //     GameOver();
        // }
    }
    public void GameOver() {
        if (isGameOver) return;
        isGameOver = true;
        gameOverUI.SetActive(true);
    }
    
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Menu () {
        // TODO:
        Debug.Log("go to menus");
    }
}
