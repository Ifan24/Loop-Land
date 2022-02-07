using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject gamePauseUI;
    [HideInInspector] public static bool isGameOver;
    private GameSpeedManager gameSpeedManager;
    public static GameManager instance;
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
        gameSpeedManager = GameSpeedManager.instance;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) {
            ToggleMenu();
        }
    }
    
    public void ToggleMenu() {
        gamePauseUI.SetActive( !gamePauseUI.activeSelf );
        
        if (gamePauseUI.activeSelf) {
            gameSpeedManager.ChangeGameSpeed(0);
        }
        else {
            gameSpeedManager.ChangeGameSpeedBack();
        }
    }
    public void GameOver() {
        if (isGameOver) return;
        isGameOver = true;
        gameOverUI.SetActive(true);
        AudioManager.instance.StopAll();
    }
    
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameSpeedManager.ChangeGameSpeedBack();
    }
    
    public void Menu () {
        // TODO:
        Debug.Log("go to menus");
    }
}
