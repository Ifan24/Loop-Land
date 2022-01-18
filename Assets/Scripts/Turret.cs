using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, Building
{
    private GameObject target;
    private Transform player;
    
    [Header("Attributes")]
    public float range = 5.0f;
    private float originalRange;
    public float fireRate = 1f;
    private float fireCountdown = 0;
    
    [Header("Unity Setup Field")]
    public float awakeFrequency = 0.3f;
    public Transform partToRotate;
    public float turnSmoothness = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject activeIndicator;
    void Start()
    {
        originalRange = range;
        player = GameObject.Find("Player").transform;
        InvokeRepeating("AwakeTurret", 0, awakeFrequency);
    }
    
    void AwakeTurret() {
        if (Vector3.Distance(player.position, transform.position) < range) {
            // attack enemies that are combatting with the player
            if (PlayerController.instance.activeEnemies.Count > 0) {
                // attack the first in the list
                target = PlayerController.instance.activeEnemies[0];
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            //TODO: random rotate the head
            activeIndicator.SetActive(false);
            return;
        }
        // effect to show the turret is active
        activeIndicator.SetActive(true);
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Slerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSmoothness).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
        
        if (fireCountdown <= 0.0f) {
            attack();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }
    public void SetRange(int _range) {
        range = _range;
    }
    public float GetOriginalRange() {
        return originalRange;
    }
    
    void attack() {
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, bulletPrefab.transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null) {
            bullet.SetTarget(target);
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
