using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static float playerHealth;
    public static float playerMaxHealth = 100;
    private void Start() {
        playerHealth = playerMaxHealth;
    }
}
