using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    private List<SpawnEnemy> placeToSpawn;
    private List<EnemySpawnBuilding> spawnBuildings;
    
    protected override void Awake() {
        base.Awake();
        spawnBuildings = new List<EnemySpawnBuilding>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        placeToSpawn = new List<SpawnEnemy>();
        foreach(GameObject path in GameObject.FindGameObjectsWithTag("Path")) {
            var spawnEnemy = path.GetComponent<SpawnEnemy>();
            if (spawnEnemy != null) {
                placeToSpawn.Add(spawnEnemy);
            }
        }
    }
    
    public void SpawnEnemies() {
        foreach(SpawnEnemy path in placeToSpawn) {
            path.SpawnObjectOnTop();
        }
        
        foreach(EnemySpawnBuilding building in spawnBuildings) {
            building.SpawnEnemy();
        }
    }
    
    public void AddSpawnBuilding(EnemySpawnBuilding building) {
        spawnBuildings.Add(building);
    }
    
}
