using System.Collections.Generic;
using UnityEngine;

public class Hotel : MonoBehaviour, Building
{
    private PlayerController player;
    private AudioManager audioManager;
    
    [Header("General")]
    [SerializeField] private float range = 5.0f;
    [SerializeField] private float healPower = 20.0f;
    
    [SerializeField] private float awakeFrequency = 0.3f;
    [SerializeField] private Vector3 heightOffset;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float potionGenerateRate = 0.3f;
    
    private float fireCountdown;
    private float potionGenerateCountdown;
    
    private bool isActive;
    
    [Header("Unity Setup Field")]
    [SerializeField] private GameObject healingEffect;
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Vector3 potionOffset;
    
    [SerializeField] private int numberOfPotionToSpawn;
    private static System.Random rng = new System.Random();  
    private List<SpawnEnemy> paths;
    private PlayerStats playerStats;
    
    void Start()
    {
        player = PlayerController.instance;
        playerStats = PlayerStats.instance;
        isActive = false;
        InvokeRepeating("AwakeTurret", 0, awakeFrequency);
        fireCountdown = 1f / fireRate;
        potionGenerateCountdown = 1f / potionGenerateRate;
        
        audioManager = AudioManager.instance;
        
        paths = new List<SpawnEnemy>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach(Collider collider in colliders) {
            if (collider.CompareTag("Path")) {
                var path = collider.gameObject.GetComponent<SpawnEnemy>();
                if (path != null) {
                    paths.Add(path);
                }
            }
        }
    }

    void AwakeTurret() {    
        // if distance between player and the turret is less than the range
        isActive = ((player.gameObject.transform.position - transform.position).sqrMagnitude < range * range);
    }
    // Update is called once per frame
    void Update()
    {
        // place potion around the hotel
        if (potionGenerateCountdown <= 0.0f) {
            SpawnPotion();
            potionGenerateCountdown = 1f / potionGenerateRate;
        }
        potionGenerateCountdown -= Time.deltaTime;
        
        // active heal the player
        if (!isActive) {
            if (healingEffect.activeSelf) {
                healingEffect.SetActive(false);
            }
            return;
        }
        // activate healing partical effect
        if (!healingEffect.activeSelf) {
            healingEffect.SetActive(true);
        }
        if (fireCountdown <= 0.0f) {
            Heal();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
            
    }
    private void Heal() {
        audioManager.Play("Heal");
        playerStats.GetHeal(healPower);
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
    
    
    public void SpawnPotion() {
        if (Mathf.Approximately(range, 0) || paths.Count == 0) return;
        
        // tried to randomly spawn n potion around
        Shuffle(paths);
        int count = 0;
        foreach(SpawnEnemy path in paths) {
            if (path.SpawnObjectOnTop(potionPrefab, potionOffset)) {
                count++;
                if (count >= numberOfPotionToSpawn) {
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
