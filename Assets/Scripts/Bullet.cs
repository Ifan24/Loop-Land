using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject target;
    public float explosionRadius = 0.0f;
    public float speed = 70f;
    public GameObject bulletImpactEffect;
    public void SetTarget(GameObject _target) {
        target = _target;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (target == null) {
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
    }
    void Damage (GameObject enemy) {
        Destroy(enemy);
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
