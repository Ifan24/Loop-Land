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
    private void Start() {
        isDead = false;
        playerHealth = playerMaxHealth;
        setHealthText();
    }
    
    private void setHealthText() {
        healthUI.text = "Health " + playerHealth.ToString("F0") + " / " + playerMaxHealth.ToString("F0");
    }
    public void Attacked(float damage) {
        playerHealth -= damage;
        if (playerHealth <= 0) {
            isDead = true;
            playerHealth = 0;
            // TODO:
        }
        setHealthText();
    }
}
