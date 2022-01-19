using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Ground : MonoBehaviour
{
    public Color hoverColor;
    public Color warningColor;
    private Vector3 offset;
    private Color defaultColor;
    private Renderer rd;
    private GameObject turret;
    private BuildManager buildManager;
    public GameObject buildEffect;
    public GameObject destroyEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Renderer>();
        defaultColor = rd.material.color;
        buildManager = BuildManager.instance;
    }
    
    public bool PlaceBuilding() {
        if (EventSystem.current.IsPointerOverGameObject() || turret != null) {
            return false;
        }
        GameObject turretToBuild = buildManager.GetTurretToBuild();
        if (turretToBuild != null) {
            offset = turretToBuild.GetComponent<Turret>().heightOffset;
            turret = (GameObject) Instantiate(turretToBuild, GetBuildPosition(), transform.rotation);
            GameObject effectGO = Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity) as GameObject;
            Destroy(effectGO, 5);
            
            return true;
        }
        return false;
    }
    public Vector3 GetBuildPosition() {
        return transform.position + offset;
    }
    // when dragging a card and hover over a ground with building
    // show warning color
    public void CardHoverIndicator() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (turret != null) {
            rd.material.color = warningColor;
        }
    }
    private void OnMouseEnter() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        rd.material.color = hoverColor;
    }
    
    public bool DestoryBuildingOnTop() {
        if (turret != null) {
            Destroy(turret);
            Destroy(Instantiate(destroyEffect, GetBuildPosition(), Quaternion.identity), 5);
            return true;
        }
        return false;
    }
    private void OnMouseExit() {
        rd.material.color = defaultColor;
    }
}
