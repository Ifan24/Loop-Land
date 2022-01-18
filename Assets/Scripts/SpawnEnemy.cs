using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Vector3 offset;
    public float spawnRate = 0.05f;
    public float spawnInterval = 15.0f;
    
    private GameObject enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnEnemyOnTop", 0, spawnInterval);
    }

    void spawnEnemyOnTop() {
        // Only spawn one enemy for each path for now
        if (enemy == null && Random.Range(0, 1.0f) <= spawnRate) {
            enemy = (GameObject)Instantiate(enemyPrefab, transform.position + offset, enemyPrefab.transform.rotation);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
