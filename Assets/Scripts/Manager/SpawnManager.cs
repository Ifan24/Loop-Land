using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private List<SpawnEnemy> placeToSpawn;
    private List<EnemySpawnBuilding> spawnBuildings;
    
    
    public static SpawnManager instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one SpawnManager in scene!");
            return;
        }
        // Singleton
        instance = this;
        spawnBuildings = new List<EnemySpawnBuilding>();
        
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        placeToSpawn = new List<SpawnEnemy>();
        foreach(GameObject path in GameObject.FindGameObjectsWithTag("Path")) {
            placeToSpawn.Add(path.GetComponent<SpawnEnemy>());
        }
    }
    
    public void SpawnEnemies() {
        foreach(SpawnEnemy path in placeToSpawn) {
            path.SpawnEnemyOnTop();
        }
        
        foreach(EnemySpawnBuilding building in spawnBuildings) {
            building.SpawnEnemy();
        }
    }
    
    public void AddSpawnBuilding(EnemySpawnBuilding building) {
        spawnBuildings.Add(building);
    }
    
}