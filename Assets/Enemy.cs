using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Drop rate")]

    // public List<float> buildingDropRate;
    public float standardTurretDropRate = 0.05f;
    public float missileLauncherDropRate = 0.02f;
    public float laserBeamerDropRate = 0.02f;
    public float destroyCardDropRate = 0.2f;
    
    
    [Header("Enemy stats")]
    public float health;
    public float originalAttackFrequency = 0.5f;
    // [HideInInspector]
    public float attackFrequency;
    private float attackCountdown;
    private bool isActive;
    public float damage = 10.0f;
    private bool isDie;
    
    [Header("Enemy effect")]
    public GameObject deathEffect;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        isDie = false;
        // to avoid divided by 0
        attackFrequency = Mathf.Max(0.00001f, originalAttackFrequency);
        attackCountdown = 1f / attackFrequency;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }
    private void Die() {
        if (isDie) return;
        isDie = true;
        // instantiate Effect and destroy it after 5 sec
        Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 5);
        // drop card
        DropCard();
        
        Destroy(gameObject);
    }
    void DropCard() {
        CardDeck cd = CardDeck.instance;
        if (Random.Range(0, 1.0f) <= standardTurretDropRate) {
            cd.addCardToDeck(cd.standardTurretCardPrefab);
        }
        if (Random.Range(0, 1.0f) <= missileLauncherDropRate) {
            cd.addCardToDeck(cd.missileLauncherCardPrefab);
        }
        if (Random.Range(0, 1.0f) <= laserBeamerDropRate) {
            cd.addCardToDeck(cd.laserBeamerCardPrefab);
        }
        if (Random.Range(0, 1.0f) <= destroyCardDropRate) {
            cd.addCardToDeck(cd.destroyCardPrefab);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (attackCountdown <= 0.0f) {
            Attack();
            attackCountdown = 1f / attackFrequency;
        }
        attackCountdown -= Time.deltaTime;
        attackFrequency = originalAttackFrequency;
    }
    // attack the player
    private void Attack() {
        if (gameObject != null) {
            PlayerStats.instance.TakeDamage(damage);
        }
    }
    
    // slowRate = [0.05, 1.0]
    public void SlowingAttack(float slowRate) {
        attackFrequency = originalAttackFrequency * slowRate;
    }
    
    public void SetActive(bool active) {
        isActive = active;
    }
}
