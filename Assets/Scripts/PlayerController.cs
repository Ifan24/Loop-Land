using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Required components")]
    public Text loopNumber;
    [SerializeField]
    private Animator playerAnimator;
    
    private int loopCount = 0;
    [Header("Player stats")]
    public float speed;
    public float waypointStopDistance = 0.1f;
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
    
    public float turnSmoothness = 10.0f;
    private Transform faceTarget;
    public float velocity;
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
        
        velocity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        LockOnTarget();
        
        // walking && running state
        if (activeEnemies.Count == 0) {
            // reset other states
            if (playerAnimator.GetBool(isBattleHash)) {
                playerAnimator.SetBool(isBattleHash, false);
            }
            if (playerAnimator.GetBool(isHitHash)) {
                playerAnimator.SetBool(isHitHash, false);
            }
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
        
        }
        
        velocity = Mathf.Clamp(velocity, 0, 5);
        playerAnimator.SetFloat(velocityHash, velocity);
    }
    
    
    private void LockOnTarget() {
        Vector3 dir = target.position - transform.position;
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
            playerAnimator.SetBool(isBattleHash, true);
            faceTarget = other.transform;
            activeEnemies.Add(other.gameObject);
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null) {
                e.SetActive(true, transform);
            }
        }
    }
    
    public void EnterHitAnimation() {
        playerAnimator.SetBool(isHitHash, true);
    }
    
    public void EnterDieAnimation() {
        playerAnimator.SetBool("isDie", true);
    }
}
