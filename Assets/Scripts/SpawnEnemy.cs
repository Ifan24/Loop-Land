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

    public void SpawnEnemyOnTop() {
        // Only spawn one enemy for each path for now
        if (enemy == null && Random.Range(0, 1.0f) <= spawnRate) {
            enemy = (GameObject)Instantiate(enemyPrefab, transform.position + offset, enemyPrefab.transform.rotation);
        }
    }
}
