using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Vector3 offset;
    public float spawnRate = 0.05f;
    
    private GameObject enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyOnTop();
    }

    // return true if spawn a enemy on top
    public bool SpawnEnemyOnTop(GameObject enemyToSpawn = null) {
        // Only spawn one enemy for each path for now
        if (enemyToSpawn != null && enemy == null) {
            enemy = (GameObject)Instantiate(enemyToSpawn, transform.position + offset, enemyPrefab.transform.rotation);
            return true;
        }
        if (enemy == null && Random.Range(0, 1.0f) <= spawnRate) {
            enemy = (GameObject)Instantiate(enemyPrefab, transform.position + offset, enemyPrefab.transform.rotation);
            return true;
        }
        return false;
    }
}
