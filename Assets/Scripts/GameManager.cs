using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gamePauseUI;
    private float gameSpeed;
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
        gameSpeed = 1;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            ToggleMenu();
        }
    }
    
    public void ToggleMenu() {
        gamePauseUI.SetActive( !gamePauseUI.activeSelf );
        
        if (gamePauseUI.activeSelf) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = gameSpeed;
        }
    }
    public void ChangeGameSpeed(float _gameSpeed) {
        gameSpeed = _gameSpeed;
        Time.timeScale = gameSpeed;
    }
    public void GameOver() {
        if (isGameOver) return;
        isGameOver = true;
        gameOverUI.SetActive(true);
    }
    
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = gameSpeed;
    }
    
    public void Menu () {
        // TODO:
        Debug.Log("go to menus");
    }
}
