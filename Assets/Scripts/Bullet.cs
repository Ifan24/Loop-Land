using UnityEngine;
using EZCameraShake;
public class Bullet : MonoBehaviour
{
    private GameObject target;
    [SerializeField] private float explosionRadius = 0.0f;
    [SerializeField] private float speed = 70f;
    [SerializeField] private float damage = 50.0f;
    
    [Header("Required setup fields")]
    public GameObject bulletImpactEffect;
    // add the sound to the audio manager 
    // and then add the name of the sound to the prefabs
    public string fireSound;
    public string targetHitSound;
    
    public void SetTarget(GameObject _target) {
        target = _target;
    }
    private void Start() {
        AudioManager.instance.Play(fireSound);
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            if (explosionRadius > 0f) {
                Explode();
            }
            Destroy(gameObject);
            return;
        }
        
        Vector3 dir = target.transform.position - transform.position;
        float flyDistance = speed * Time.deltaTime;
        if (dir.magnitude <= flyDistance) {
            hitTarget();
            return;
        }
        transform.Translate(dir.normalized * flyDistance, Space.World);
        transform.LookAt(target.transform);
    }
    void hitTarget() {
        GameObject effectIns = (GameObject)Instantiate(bulletImpactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);
        if (explosionRadius > 0f) {
            Explode();        
        } else {
            // Just a bullet
            Damage(target);
        }
        AudioManager.instance.Play(targetHitSound);
        Destroy(gameObject);
    }
    void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                Damage(collider.gameObject);
            }
        }
        cinemachineShake.instance.ShakeCamera(4, 0.2f);
    }
    void Damage (GameObject enemy) {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e == null) {
            return;
        }
        e.TakeDamage(damage);
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
