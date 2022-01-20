using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("The enemyMultiplies for the current loop")]
    public float enemyMultiplies = 1;
    [Tooltip("Each loop enemy will get stronger by x%")]
    public float enemyUpgradeRate = 0.05f;
    
    public static EnemyManager instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one EnemyManager in scene!");
            return;
        }
        // Singleton
        instance = this;
    }
    public void EnterNextLoop(int loopCount) {
        enemyMultiplies += loopCount * enemyUpgradeRate;
    }
}
