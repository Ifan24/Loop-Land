using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotel : MonoBehaviour, Building
{
    private PlayerController player;
    private AudioManager audioManager;
    
    [Header("General")]
    public float range = 5.0f;
    public float healPower = 20.0f;
    
    public float awakeFrequency = 0.3f;
    public Vector3 heightOffset;
    public float fireRate = 1f;
    private float fireCountdown;
    private bool isActive;
    
    [Header("Unity Setup Field")]
    public GameObject healingEffect;
    private PlayerStats playerStats;
    void Start()
    {
        player = PlayerController.instance;
        playerStats = PlayerStats.instance;
        isActive = false;
        InvokeRepeating("AwakeTurret", 0, awakeFrequency);
        fireCountdown = 1f / fireRate;
        audioManager = AudioManager.instance;
    }

    void AwakeTurret() {    
        // if distance between player and the turret is less than the range
        isActive = ((player.gameObject.transform.position - transform.position).sqrMagnitude < range * range);
    }
    // Update is called once per frame
    void Update()
    {
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
