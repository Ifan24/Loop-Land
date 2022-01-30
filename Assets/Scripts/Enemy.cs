using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Header("Required components")]
    public Image healthBar;
    public GameObject deathEffect;
    public Animator enemyAnimator;
    
    
    [Header("Enemy stats")]
    public float health;
    public float maxHealth = 100;
    public float originalAttackFrequency = 0.5f;
    private float attackFrequency;
    private float attackCountdown;
    private bool isActive;
    public float damage = 10.0f;
    private bool isDie;
    public float dropCardRate = 1;
    
    [Header("Enemy effect")]
    private int isAttackHash;
    private int isHitHash;
    private PlayerStats playerStats;
    private Transform targetTransform;
    public float turnSmoothness = 10f;
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
        playerStats = PlayerStats.instance;
        isAttackHash = Animator.StringToHash("isAttack");
        isHitHash = Animator.StringToHash("isHit");
    }
    /// <summary>
    /// Upgrade enemes for each loop the player passed
    /// </summary>
    private void upgradeEnemy() {
        EnemyManager manager = EnemyManager.instance;
        maxHealth *= manager.enemyMultiplies;
        damage *= manager.enemyMultiplies;
    }
    
    public void TakeDamage(float damage) {
        health -= damage;
        enemyAnimator.SetBool(isHitHash, true);
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
    /// <summary>
    /// Turn the enemy head toword its target
    /// </summary>
    private void LockOnTarget() {
        if (!targetTransform) return;
        Vector3 dir = targetTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSmoothness).eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
    
    private void LateUpdate() {
        if (enemyAnimator.GetBool(isHitHash)) {
            enemyAnimator.SetBool(isHitHash, false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isActive || playerStats.isDead) return;
        
        LockOnTarget();
        // reset states
        if (enemyAnimator.GetBool(isAttackHash)) {
            enemyAnimator.SetBool(isAttackHash, false);
        }
        
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
            enemyAnimator.SetBool(isAttackHash, true);
            playerStats.TakeDamage(damage);
        }
    }
    
    // slowRate = [0.05, 1.0]
    public void SlowingAttack(float slowRate) {
        attackFrequency = originalAttackFrequency * slowRate;
    }
    
    public void SetActive(bool active, Transform _target) {
        isActive = active;
        if (active) {
            enemyAnimator.SetBool("isActive", true);
            targetTransform = _target;
        }
    }
}
