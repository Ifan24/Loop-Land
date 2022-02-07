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
            SpawnObjectOnTop();
        }
    }

    // return true if spawn a enemy on top (default) 
    public bool SpawnObjectOnTop(GameObject enemyToSpawn = null, Vector3? objectOffset = null) {
        // Only spawn one enemy for each path for now
        if (enemy != null) return false;
        Vector3 useOffset = objectOffset == null ? offset : objectOffset.Value;
        
        // use the prefab passed in
        if (enemyToSpawn != null) {
            enemy = (GameObject)Instantiate(enemyToSpawn, transform.position + useOffset, enemyPrefab.transform.rotation);
            return true;
        }
        
        if (Random.Range(0, 1.0f) <= spawnRate) {
            enemy = (GameObject)Instantiate(enemyPrefab, transform.position + useOffset, enemyPrefab.transform.rotation);
            return true;
        }
        return false;
    }
}
