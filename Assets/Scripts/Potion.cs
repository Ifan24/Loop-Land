using UnityEngine;

public class Potion : MonoBehaviour
{
    public float healPower = 10;
    [SerializeField] private GameObject healEffect;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerStats.instance.GetHeal(healPower);
            GameObject effectIns = (GameObject)Instantiate(healEffect, transform.position, healEffect.transform.rotation);
            AudioManager.instance.Play("Heal");
            Destroy(effectIns, 5f);
            Destroy(gameObject);
        }
    }
}
