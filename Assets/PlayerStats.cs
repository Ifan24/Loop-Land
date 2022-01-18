using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    public static float playerHealth;
    public static float playerMaxHealth = 100;
    public Text healthUI;
    public bool isDead;
    
    
    public static PlayerStats instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one PlayerStats in scene!");
            return;
        }
        // Singleton
        instance = this;
    }
    
    private void Start() {
        isDead = false;
        playerHealth = playerMaxHealth;
        SetHealthText();
    }
    
    private void SetHealthText() {
        healthUI.text = "Health " + playerHealth.ToString("F0") + " / " + playerMaxHealth.ToString("F0");
    }
    public void TakeDamage(float damage) {
        playerHealth -= damage;
        if (!isDead && playerHealth <= 0) {
            isDead = true;
            playerHealth = 0;
            GameManager.instance.GameOver();
        }
        SetHealthText();
    }

}
