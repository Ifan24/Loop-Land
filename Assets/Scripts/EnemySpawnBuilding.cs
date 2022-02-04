using System.Collections.Generic;
using UnityEngine;
using System;
    
public class EnemySpawnBuilding : MonoBehaviour, Building
{
    [Header("Building interface")]
    [SerializeField] private float range = 2.0f;
    [SerializeField] private Vector3 heightOffset;
    
    [Header("Building settings")]
    public GameObject enemyPrefab;
    [SerializeField] private int numberOfEnemyToSpawn = 1;
    private List<SpawnEnemy> paths;
    private static System.Random rng = new System.Random();  

    // Start is called before the first frame update
    void Start()
    {
        paths = new List<SpawnEnemy>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach(Collider collider in colliders) {
            if (collider.CompareTag("Path")) {
                SpawnEnemy path = collider.gameObject.GetComponent<SpawnEnemy>();
                if (path != null) {
                    paths.Add(path);
                }
            }
        }
        SpawnManager.instance.AddSpawnBuilding(this);
    }
    
    public void Shuffle (List<SpawnEnemy> list) {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            SpawnEnemy value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
    
    
    public void SpawnEnemy() {
        if (Mathf.Approximately(range, 0) || paths.Count == 0) return;
        
        // tried to randomly spawn n enemy around
        Shuffle(paths);
        int count = 0;
        foreach(SpawnEnemy path in paths) {
            if (path.SpawnEnemyOnTop(enemyPrefab)) {
                count++;
                if (count >= numberOfEnemyToSpawn) {
                    break;
                }
            }
        }
    }
    
    public void SetRange(int _range) {
        range = _range;
    }
    public float GetRange() {
        return range;
    }
    public Vector3 GetheightOffset() {
        return heightOffset;
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
