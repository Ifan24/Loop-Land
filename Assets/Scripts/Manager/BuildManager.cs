using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
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
    
    public GameObject GetTurretToBuild() {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret) {
        turretToBuild = turret;
    }
}
