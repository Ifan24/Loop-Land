using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Required components")]
    public Text loopNumber;
    [SerializeField] private Animator playerAnimator;
    
    private int loopCount = 0;
    [Header("Player attack stats")]
    [SerializeField] private float originalAttackFrequency = 0.5f;
    private float attackFrequency;
    private float attackCountdown;
    [SerializeField] private float damage = 10.0f;
    
    [Header("Player movement stats")]
    [SerializeField] private float speed;
    [SerializeField] private float waypointStopDistance = 0.1f;
    private Transform target;
    private int waypointsIdx = 0;
    
    [HideInInspector]
    public List<GameObject> activeEnemies = new List<GameObject>();
    
    // Animator hash
    private int isBattleHash;
    private int isHitHash;
    private int isWalkingHash;
    private int isRunningHash;
    private int velocityHash;
    private int isDiedHash;
    public float turnSmoothness = 10.0f;
    private float velocity;
    
    // end of animator variables
    // =========================
    private Transform faceTarget;
    private Enemy targetEnemy;
    
    // singleton manager instances
    private PlayerStats playerStats;
    
    // Singleton
    public static PlayerController instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one PlayerController in scene!");
            return;
        }
        // Singleton
        instance = this;
    }
    void Start()
    {
        target = Waypoints.waypoints[0];
        faceTarget = target;
        // set up animation hash
        isBattleHash = Animator.StringToHash("isBattle");
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isHitHash = Animator.StringToHash("isHit");
        velocityHash = Animator.StringToHash("velocity");
        isDiedHash = Animator.StringToHash("isDied");
        
        velocity = 0;
        playerStats = PlayerStats.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAnimator.GetBool(isDiedHash)) return;
        activeEnemies.RemoveAll(enemy => enemy == null);
        LockOnTarget();
        
        // reset other states
        if (playerAnimator.GetBool(isBattleHash)) {
            playerAnimator.SetBool(isBattleHash, false);
        }
        if (playerAnimator.GetBool(isHitHash)) {
            playerAnimator.SetBool(isHitHash, false);
        }
        
        // walking && running state
        if (activeEnemies.Count == 0) {
            if (!playerAnimator.GetBool(isWalkingHash)) {
                playerAnimator.SetBool(isWalkingHash, true);
            }
            velocity += speed * Time.deltaTime;
            
            followWaypoints();
        } 
        // not walking or running states
        else {
            velocity -= speed * Time.deltaTime;
            
            if (playerAnimator.GetBool(isWalkingHash)) {
                playerAnimator.SetBool(isWalkingHash, false);
            }
            
            // attack target
            if (attackCountdown <= 0.0f) {
                Attack();
                attackCountdown = 1f / attackFrequency;
            }
            attackCountdown -= Time.deltaTime;
        
        }
        
        // set blend tree velocity
        velocity = Mathf.Clamp(velocity, 0, 5);
        playerAnimator.SetFloat(velocityHash, velocity);
        
    }
    
    private void Attack() {
        // TODO: always attack the first enemy in the list
        if (activeEnemies.Count > 0) {
            targetEnemy = activeEnemies[0].GetComponent<Enemy>();
        }
        if (targetEnemy != null) {
            targetEnemy.TakeDamage(damage);
            // special case for turtleshell
            if (targetEnemy.reflectRate > 0) {
                playerStats.TakeDamage(damage * targetEnemy.reflectRate);
            }
        }
        playerAnimator.SetBool(isBattleHash, true);
    }
    private void LockOnTarget() {
        if (faceTarget == null) return;
        Vector3 dir = faceTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSmoothness).eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }
    
    void followWaypoints() {
        // stop moving if game is over
        // if (GameManager.isGameOver) return;
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        // distance between target and player is less than waypointStopDistance
        if ((target.position - transform.position).sqrMagnitude <= waypointStopDistance * waypointStopDistance) {
            waypointsIdx++;
            SetNextWaypoint();
        }
    }
    void SetNextWaypoint() {
        // assuming the last waypoint is at the portal
        if (waypointsIdx == Waypoints.waypoints.Length) {
            // move to next loop
            waypointsIdx = 0;
            NextLoop();
        }
        target = Waypoints.waypoints[waypointsIdx];
        faceTarget = target;        
    }
    
    private void NextLoop() {
        loopCount++;
        // Set loop UI
        loopNumber.text = "Loop " + loopCount.ToString();
        EnemyManager.instance.EnterNextLoop(loopCount);
    }
    private void OnTriggerEnter(Collider other) {
        // enter battle state
        if (other.gameObject.CompareTag("Enemy")) {
            faceTarget = other.transform;
            activeEnemies.Add(other.gameObject);
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null) {
                e.SetActive(true, transform);
            }
            
            // setup attack frequency
            attackFrequency = Mathf.Max(0.00001f, originalAttackFrequency);
            attackCountdown = 1f / attackFrequency;
        }
    }
    
    public void EnterHitAnimation() {
        playerAnimator.SetBool(isHitHash, true);
    }
    
    public void EnterDieAnimation() {
        playerAnimator.SetBool(isDiedHash, true);
    }
}
