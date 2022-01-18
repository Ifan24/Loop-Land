using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private GameObject turretToBuild;
    public GameObject standarTurretPrefab;
    public GameObject missileLauncherPrefab;
    
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
    
    // public bool BuildTurretOn(Ground ground) {
    //     GameObject turretGO = Instantiate(turretToBuild, ground.GetBuildPosition(), Quaternion.identity) as GameObject;
    //     ground.turret = turretGO;
    //     return true;
    // }
    public void SetTurretToBuild(GameObject turret) {
        turretToBuild = turret;
    }
}
