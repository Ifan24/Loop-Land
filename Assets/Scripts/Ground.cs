using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Ground : MonoBehaviour
{
    public Color hoverColor;
    public Color warningColor;
    public Vector3 offset;
    private Color defaultColor;
    private Renderer rd;
    private GameObject turret;
    private BuildManager buildManager;
    
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Renderer>();
        defaultColor = rd.material.color;
        buildManager = BuildManager.instance;
    }

    // private void OnMouseDown() {
    //     if (EventSystem.current.IsPointerOverGameObject()) {
    //         return;
    //     }
    //     if (turret != null) {
    //         // TODO: do something if the click ground already has building
    //         return;
    //     }
    //     GameObject turretToBuild = buildManager.GetTurretToBuild();
    //     if (turretToBuild != null) {
    //         turret = (GameObject) Instantiate(turretToBuild, transform.position + offset, transform.rotation);
    //     }
    // }
    
    public bool PlaceBuilding() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return false;
        }
        if (turret != null) {
            // TODO: do something if the click ground already has building
            return false;
        }
        GameObject turretToBuild = buildManager.GetTurretToBuild();
        if (turretToBuild != null) {
            turret = (GameObject) Instantiate(turretToBuild, transform.position + offset, transform.rotation);
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
    private void OnMouseExit() {
        rd.material.color = defaultColor;
    }
}
