using UnityEngine;

public class EnterPortal : MonoBehaviour
{
    [SerializeField] private GameObject goBackButton;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (GameManager.instance.stopAtPortal) {
                GameSpeedManager.instance.ChangeGameSpeed(0);
            }
            goBackButton.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            goBackButton.SetActive(false);
        }
    }
}
