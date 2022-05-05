using UnityEngine;

public class Turret : MonoBehaviour, Building
{
    private GameObject target;
    private PlayerController player;
    
    [Header("General")]
    [SerializeField] private float range = 5.0f;
    
    [Header("Unity Setup Field")]
    [SerializeField] private float awakeFrequency = 0.3f;
    public Transform partToRotate;
    [SerializeField] private float turnSmoothness = 10f;
    public GameObject activeIndicator;
    [SerializeField] private Vector3 heightOffset;
    
    [Header("Use Bullets (Default)")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    private float fireCountdown;
    private Enemy targetEnemy;
    
    [Header("Use Laser")]
    [SerializeField] private bool useLaser = false;
    [SerializeField] private float damageOverTime = 30;
    public LineRenderer lineRenderer;
    public ParticleSystem laserEffect;
    public Light impactLight;
    [SerializeField] private float slowRate;
    private bool withinRange;
    private AudioManager audioManager;
    void Start()
    {
        player = PlayerController.instance;
        withinRange = false;
        InvokeRepeating("AwakeTurret", 0, awakeFrequency);
        fireCountdown = 1f / fireRate;
        audioManager = AudioManager.instance;
    }
    
    [SerializeField] private GameObject linkToPlayerPrefab;
    
    void AwakeTurret() {
        // if distance between player and the turret is less than the range
        if ((player.gameObject.transform.position - transform.position).sqrMagnitude < range * range) {
            // attack enemies that are combatting with the player
            if (player.activeEnemies.Count > 0) {
                // attack the first in the list
                target = player.activeEnemies[0];
                if (target != null) {
                    targetEnemy = target.GetComponent<Enemy>();
                }
                // start attack sound
                if (useLaser) {
                    audioManager.Play("Laser");
                }
            }
            withinRange = true;
        }
        else {
            withinRange = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (withinRange && !linkToPlayerPrefab.activeSelf) {
            linkToPlayerPrefab.SetActive(true);
        }
        if (!withinRange && linkToPlayerPrefab.activeSelf) {
            linkToPlayerPrefab.SetActive(false);
        }
        
        if (target == null) {
            //TODO: random rotate the head
            activeIndicator.SetActive(false);
            if (useLaser && lineRenderer.enabled) {
                lineRenderer.enabled = false;
                laserEffect.Stop();
                impactLight.enabled = false;
                // stop attack sound
                audioManager.Stop("Laser");
            }
            return;
        }
        // effect to show the turret is active
        activeIndicator.SetActive(true);
        
        LockOnTarget();
        
        if (useLaser) {
            LaserAttack();
        }
        else {
            if (fireCountdown <= 0.0f) {
                Attack();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }
    private void LaserAttack() {
        if (targetEnemy == null) return;
        
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.SlowingAttack(slowRate);

        // attack graphics
        if (!lineRenderer.enabled) {
            laserEffect.Play();
            lineRenderer.enabled = true;
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.transform.position);
        
        LockOnLaserEffect();
    }
    
    private void LockOnLaserEffect() {
        Vector3 dir = firePoint.position - target.transform.position;
        laserEffect.transform.rotation = Quaternion.LookRotation(dir);
        laserEffect.transform.position = target.transform.position + dir.normalized * 0.5f;
    }
    
    private void LockOnTarget() {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Slerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSmoothness).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
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
    private void Attack() {
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
