using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private List<SpawnEnemy> placeToSpawn;
    
    
    public static SpawnManager instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one SpawnManager in scene!");
            return;
        }
        // Singleton
        instance = this;
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
    }
}
