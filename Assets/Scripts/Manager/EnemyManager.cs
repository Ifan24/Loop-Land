using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [Tooltip("The enemyMultiplies for the current loop")]
    public float enemyMultiplies = 1;
    [Tooltip("Each loop enemy will get stronger by x%")]
    [SerializeField] private float enemyUpgradeRate = 0.05f;
    
    public void EnterNextLoop(int loopCount) {
        enemyMultiplies += loopCount * enemyUpgradeRate;
    }
}
