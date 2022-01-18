using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float standardTurretDropRate = 0.05f;
    public float missileLauncherDropRate = 0.02f;
    
    public float attackFrequency = 0.25f;
    private float attackCountdown = 0;
    private bool isActive;
    public float damage = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }
    private void Die() {
        // TODO: Effect
    
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
    }
    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (attackCountdown <= 0.0f) {
            attack();
            attackCountdown = 1f / attackFrequency;
        }
        attackCountdown -= Time.deltaTime;
    }
    // attack the player
    void attack() {
        PlayerStats.instance.TakeDamage(damage);
    }
    
    public void SetActive(bool active) {
        isActive = active;
    }
}
