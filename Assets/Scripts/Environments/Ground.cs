using UnityEngine;
using UnityEngine.EventSystems;
public class Ground : Grids
{
    private Vector3 offset;
    private GameObject turret;
    private BuildManager buildManager;
    public GameObject buildEffect;
    public GameObject destroyEffect;
    private BossSummonManager bossSummonManager;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        buildManager = BuildManager.instance;
        bossSummonManager = BossSummonManager.instance;
    }
    
    public bool PlaceBuilding() {
        // if (EventSystem.current.IsPointerOverGameObject() || turret != null) {
        if (turret != null) {
            return false;
        }
        GameObject turretToBuild = buildManager.GetTurretToBuild();
        if (turretToBuild != null) {
            offset = turretToBuild.GetComponent<Building>().GetheightOffset();
            turret = (GameObject) Instantiate(turretToBuild, GetBuildPosition(), transform.rotation);
            GameObject effectGO = Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity) as GameObject;
            Destroy(effectGO, 5);
            
            bossSummonManager.addNumberOfBuilding(1);
            hasBuilding = true;
            return true;
        }
        return false;
    }
    public Vector3 GetBuildPosition() {
        return transform.position + offset;
    }
    public bool DestoryBuildingOnTop() {
        if (turret != null) {
            Destroy(turret);
            Destroy(Instantiate(destroyEffect, GetBuildPosition(), Quaternion.identity), 5);
            bossSummonManager.addNumberOfBuilding(-1);
            hasBuilding = false;
            return true;
        }
        return false;
    }
}
