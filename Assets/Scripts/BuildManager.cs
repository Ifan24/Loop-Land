using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject turretToBuild;
    // public GameObject standardTurretPrefab;
    // public GameObject missileLauncherPrefab;
    // public GameObject laserBeamerPrefab;
    // public GameObject hotelPrefab;
    
    
    /* 
        To add new building into the game
        1. create the prefab of the building
        2. create the prefab of the building card
        3. add card prefab to the card deck
    */
    public static BuildManager instance;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        // Singleton
        instance = this;
    }
    
    public GameObject GetTurretToBuild() {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret) {
        turretToBuild = turret;
    }
}
