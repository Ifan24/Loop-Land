using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    public float speed;
    public float waypointStopDistance = 0.1f;
    private Transform target;
    private int waypointsIdx = 0;
    public Text loopNumber;
    private int loopCount = 0;
    public List<GameObject> activeEnemies = new List<GameObject>();
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
        rb = GetComponent<Rigidbody>();
        target = Waypoints.waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        if (activeEnemies.Count == 0) {
            followWaypoints();
        }
    }
    void followWaypoints() {
        // stop moving if game is over
        // if (GameManager.isGameOver) return;
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        if (Vector3.Distance(target.position, transform.position) <= waypointStopDistance) {
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
    }
    
    private void NextLoop() {
        loopCount++;
        // Set loop UI
        loopNumber.text = "Loop " + loopCount.ToString();
        EnemyManager.instance.EnterNextLoop(loopCount);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            activeEnemies.Add(other.gameObject);
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null) {
                e.SetActive(true);
            }
        }
    }
}
