using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gamePauseUI;
    [HideInInspector] public static bool isGameOver;
    public bool stopAtPortal;
    public bool stopAtStart;
    
    
    private GameSpeedManager gameSpeedManager;

    private void Start() {
        isGameOver = false;
        gameSpeedManager = GameSpeedManager.instance;
        if (stopAtStart) {
            gameSpeedManager.ChangeGameSpeed(0);
        }
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
        AudioManager.instance.Play("Gameover");
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
