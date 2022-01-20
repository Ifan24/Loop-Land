using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy stats")]
    public float health;
    public float maxHealth = 100;
    public Image healthBar;
    public float originalAttackFrequency = 0.5f;
    public float attackFrequency;
    private float attackCountdown;
    private bool isActive;
    public float damage = 10.0f;
    private bool isDie;
    public float dropCardRate = 1;
    
    [Header("Enemy effect")]
    public GameObject deathEffect;
    // Start is called before the first frame update
    void Start()
    {
        upgradeEnemy();
        isActive = false;
        isDie = false;
        health = maxHealth;
        // to avoid divided by 0
        attackFrequency = Mathf.Max(0.00001f, originalAttackFrequency);
        attackCountdown = 1f / attackFrequency;
        
    }
    private void upgradeEnemy() {
        EnemyManager manager = EnemyManager.instance;
        maxHealth *= manager.enemyMultiplies;
        damage *= manager.enemyMultiplies;
    }
    
    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
        healthBar.fillAmount = health / maxHealth;
    }
    private void Die() {
        if (isDie) return;
        isDie = true;
        // instantiate Effect and destroy it after 5 sec
        Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), 5);
        // drop card
        DropCardManager.instance.DropCard(dropCardRate);
        
        Destroy(gameObject);
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
