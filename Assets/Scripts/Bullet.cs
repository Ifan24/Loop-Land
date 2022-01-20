using UnityEngine;
using EZCameraShake;
public class Bullet : MonoBehaviour
{
    private GameObject target;
    public float explosionRadius = 0.0f;
    public float speed = 70f;
    public float damage = 50.0f;
    public GameObject bulletImpactEffect;
    public void SetTarget(GameObject _target) {
        target = _target;
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
            AudioManager.instance.Play("BulletImpact");
            Damage(target);
        }
        Destroy(gameObject);
    }
    void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                Damage(collider.gameObject);
            }
        }
        CameraShaker.Instance.ShakeOnce(4, 4, 0.1f, 1);
        AudioManager.instance.Play("Explosion");
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
