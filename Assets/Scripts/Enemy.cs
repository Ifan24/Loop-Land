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
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float originalAttackFrequency = 0.5f;
    private float attackFrequency;
    private float attackCountdown;
    private bool isActive;
    [SerializeField] private float damage = 10.0f;
    private bool isDie;
    [SerializeField] private float dropCardRate = 1;
    
    [Header("Enemy effect")]
    
    // animator variables
    private int isAttackHash;
    private int isHitHash;
    private int isPlayerDieHash;
    
    private PlayerStats playerStats;
    private Transform targetTransform;
    [SerializeField] private float turnSmoothness = 10f;
    // Start is called before the first frame update
    
    [SerializeField] private string enemyHitSound;
    
    [Header("Optional for reflect demage")]
    public float reflectRate = 0f;
    
    [Header("Optional for long range enemy")]
    [SerializeField] private float range = 0;
    [SerializeField] private float awakeFrequency = 0.3f;
    
    // Singleton manager instances
    private PlayerController player;
    private AudioManager audioManager;
    void Start()
    {
        upgradeEnemy();
        isActive = false;
        isDie = false;
        health = maxHealth;
        // to avoid divided by 0
        attackFrequency = Mathf.Max(0.00001f, originalAttackFrequency);
        attackCountdown = 1f / attackFrequency;
        isAttackHash = Animator.StringToHash("isAttack");
        isHitHash = Animator.StringToHash("isHit");
        isPlayerDieHash = Animator.StringToHash("isPlayerDie");
        
        if (range > 0) {
            player = PlayerController.instance;
            InvokeRepeating("AwakeEnemy", 0, awakeFrequency);
        }
        audioManager = AudioManager.instance;
        playerStats = PlayerStats.instance;
        
    }
    
    void AwakeEnemy() {
        // if distance between player and the enemy is less than the range
        if ((player.gameObject.transform.position - transform.position).sqrMagnitude < range * range) {
            SetActive(true, player.gameObject.transform);
        }
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
        
        // play victory animation and don't do anything
        if (playerStats.isDead && !enemyAnimator.GetBool(isPlayerDieHash)) {
            enemyAnimator.SetBool(isAttackHash, false);
            enemyAnimator.SetBool(isPlayerDieHash, true);
            return;
        }
        
        if (playerStats.isDead || !isActive) return;
        
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
            audioManager.Play(enemyHitSound);
            playerStats.TakeDamage(damage);
        }
    }
    
    // slowRate = [0.05, 1.0]
    public void SlowingAttack(float slowRate) {
        attackFrequency = originalAttackFrequency * slowRate;
    }
    
    public void SetActive(bool active, Transform _target) {
        if (!isActive && active) {
            enemyAnimator.SetBool("isActive", true);
            targetTransform = _target;
        }
        isActive = active;
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
