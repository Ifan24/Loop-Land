using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // public string buildingType;
    private BuildManager buildManager;
    [Header("Default with prefab")]
    public GameObject buildingPrefab;
    private GameObject buildingGO;
    private Vector3 origincalScale;
    private Camera cam;
    private Building BuildingInstance;
    
    [Header("Default No prefab")]
    public bool useDestroyCard = false;
    private string groundTag = "Ground";
    private void Start() {
        buildManager = BuildManager.instance;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (cam == null) {
            Debug.LogError("No main camera in the scene!");
        }
    }
    public void OnBeginDrag(PointerEventData eventData) {
        // make the card invisible
        origincalScale = transform.localScale;
        transform.localScale = new Vector3();
        
        if (useDestroyCard) {
            return;
        }
        if (buildingPrefab == null) {
            Debug.LogError("Dragging a card without building prefab");
            return;
        }
        
        buildManager.SetTurretToBuild(buildingPrefab);
        // generate a small model of the card to preview the building
        buildingGO = (GameObject)Instantiate(buildingPrefab, Input.mousePosition, buildingPrefab.transform.rotation);
        BuildingInstance = buildingGO.GetComponent<Building>();
        if (BuildingInstance == null) {
            Debug.LogError("Prefab has not inherited building interface");
            return;
        }
        BuildingInstance.SetRange(0);
    }
    public void OnDrag(PointerEventData eventData) {
        // Debug.Log("dragging a card");
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (!useDestroyCard) {
                buildingGO.transform.position = hit.point;
            }
            if (hit.transform.gameObject.CompareTag(groundTag)) {
                hit.transform.GetComponent<Ground>().CardHoverIndicator();
            }
        }
        // show the range of the building
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(buildingGO.transform.position, BuildingInstance.GetOriginalRange());
    }
    public void OnEndDrag(PointerEventData eventData) {
        if (!useDestroyCard) {
            Destroy(buildingGO);
        }
        // TODO: use the last ray cast result
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool failToPlace = true;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.CompareTag(groundTag)) {
                Ground ground = hit.transform.gameObject.GetComponent<Ground>();
                // successfully place a card there
                if (useDestroyCard) {
                    if (ground.DestoryBuildingOnTop()) {
                        failToPlace = false;
                        Destroy(gameObject);
                    }
                }
                else {
                    if (ground.PlaceBuilding()) {
                        failToPlace = false;
                        // remove the card from the deck
                        Destroy(gameObject);
                    }
                }
            }
        }
        // revert any changes
        if (failToPlace) {
            transform.localScale = origincalScale;
        }
        
    }
    private void OnDestroy() {
        // the card got destroy while holding it
        if (!useDestroyCard) {
            // remove the tmp game object
            if (buildingGO != null) {
                Destroy(buildingGO);     
            }
        }
    }
}
