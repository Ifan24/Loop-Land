using UnityEngine;
using UnityEngine.UI;
public class GameSpeedManager : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color currentSpeedColor;
    private float prevSpeed;
    private bool isPause;
    
    // Singleton
    public static GameSpeedManager instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one GameSpeedManager in scene!");
            return;
        }
        instance = this;
    }
    
    private void Start() {
        prevSpeed = 1;
        isPause = false;
    }
    private void Update() {
        // pause game
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isPause) {
                isPause = false;
                ChangeGameSpeed(prevSpeed);
            }
            else {
                isPause = true;
                ChangeGameSpeed(0);
            }
        }
    }
    private void selectChild(int idx) {
        int i = 0;
        foreach(Transform childT in transform) {
            Image child = childT.GetComponent<Image>();
            if (i == idx) {
                child.color = currentSpeedColor;
            }
            else {
                child.color = defaultColor;
            }
            i++;
        }
    }
    public void ChangeGameSpeed(float gameSpeed) {
        prevSpeed = Time.timeScale;
        Time.timeScale = gameSpeed;
        
        int idx = Mathf.FloorToInt(gameSpeed);
        idx = Mathf.Clamp(idx, 0, transform.childCount - 1);
        if (idx > 0) {
            isPause = false;
        }
        // change the UI
        selectChild(idx);
    }
    
    public void ChangeGameSpeedBack() {
        ChangeGameSpeed(prevSpeed);
    }
}
