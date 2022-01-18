using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // public string buildingType;
    private BuildManager buildManager;
    public GameObject buildingPrefab;
    private GameObject buildingGO;
    private Vector3 origincalScale;
    private Camera cam;
    private Building BuildingInstance;
    private void Start() {
        buildManager = BuildManager.instance;
        cam = Camera.main;
    }
    public void OnBeginDrag(PointerEventData eventData) {
        // setBuildingType();
        if (buildingPrefab == null) {
            Debug.LogError("Dragging a card without building prefab");
            return;
        }
        buildManager.SetTurretToBuild(buildingPrefab);
        // allow it to detach from the deck
        // transform.SetParent(transform.root);
        // generate a small model of the card
        origincalScale = transform.localScale;
        transform.localScale = new Vector3();
        // preview the building
        buildingGO = (GameObject)Instantiate(buildingPrefab, Input.mousePosition, buildingPrefab.transform.rotation);
        // buildingGO.GetComponent<Building>().SetRange(0);
        BuildingInstance = buildingGO.GetComponent<Building>();
        if (BuildingInstance == null) {
            Debug.LogError("Prefab has not inherited building interface");
            return;
        }
        BuildingInstance.SetRange(0);
    }
    // void setBuildingType() {
        // if (buildingType == "Standard turret") {
        //     buildingPrefab = buildManager.standarTurretPrefab;
        // }
        // if (buildingType == "Missile Launcher") {
        //     buildingPrefab = buildManager.missileLauncherPrefab;
        // }
    // }
    public void OnDrag(PointerEventData eventData) {
        // Debug.Log("dragging a card");
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            buildingGO.transform.position = hit.point;
            if (hit.transform.gameObject.CompareTag("Ground")) {
                hit.transform.GetComponent<Ground>().CardHoverIndicator();
            }
        }
        // show the range of the building
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(buildingGO.transform.position, BuildingInstance.GetOriginalRange());
    }
    public void OnEndDrag(PointerEventData eventData) {
        Destroy(buildingGO);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool failToPlace = true;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.CompareTag("Ground")) {
                // successfully place a building there
                if (hit.transform.gameObject.GetComponent<Ground>().PlaceBuilding()) {
                    failToPlace = false;
                    // remove the card from the deck
                    Destroy(gameObject);
                    // reopen the deck
                }
            }
        }
        // revert any changes
        if (failToPlace) {
            transform.localScale = origincalScale;
        }
        
    }
    
}
