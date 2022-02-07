using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float spawnRate = 0.05f;
    [SerializeField] private bool isSpawnOnStart = true;
    
    private GameObject enemy;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (isSpawnOnStart) {
            SpawnEnemyOnTop();
        }
    }

    // return true if spawn a enemy on top
    public bool SpawnEnemyOnTop(GameObject enemyToSpawn = null) {
        // Only spawn one enemy for each path for now
        if (enemy != null) return false;
        
        
        if (enemyToSpawn != null) {
            enemy = (GameObject)Instantiate(enemyToSpawn, transform.position + offset, enemyPrefab.transform.rotation);
            return true;
        }
        if (Random.Range(0, 1.0f) <= spawnRate) {
            enemy = (GameObject)Instantiate(enemyPrefab, transform.position + offset, enemyPrefab.transform.rotation);
            return true;
        }
        return false;
    }
}
