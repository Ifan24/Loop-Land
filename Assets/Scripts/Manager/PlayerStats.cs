using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class PlayerStats : Singleton<PlayerStats>
{
    private float playerHealth;
    [SerializeField] private float playerMaxHealth = 100;
    public Text healthUI;
    public Image healthBar;
    public bool isDead;
    private StringBuilder builder;
    private PlayerController playerController;
    
    private void Start() {
        isDead = false;
        playerHealth = playerMaxHealth;
        builder = new StringBuilder();
        playerController = PlayerController.instance;
        SetHealthUI();
    }
    
    private void SetHealthUI() {
        if (healthUI == null || healthBar == null) return;
        builder.Clear();
        builder.Append("Health ").Append(playerHealth.ToString("F0"));
        builder.Append(" / ").Append(playerMaxHealth.ToString("F0"));
        healthUI.text = builder.ToString();
        healthBar.fillAmount = playerHealth / playerMaxHealth;
    }
    public void TakeDamage(float damage) {
        playerHealth -= damage;
        playerController.EnterHitAnimation();
        
        if (!isDead && playerHealth <= 0) {
            PlayerDie();
        }
        SetHealthUI();
    }
    
    public void GetHeal(float hp) {
        playerHealth += hp;
        if (playerHealth > playerMaxHealth) {
            playerHealth = playerMaxHealth;
        }
        SetHealthUI();
    }
    private void PlayerDie() {
        isDead = true;
        playerController.EnterDieAnimation();
        
        playerHealth = 0;
        GameManager.instance.GameOver();
    }
}
