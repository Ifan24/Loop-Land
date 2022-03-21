using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private BuildManager buildManager;
    [Header("Default with prefab")]
    public GameObject buildingPrefab;
    private GameObject buildingGO;
    private Vector3 origincalScale;
    private Camera cam;
    private Building BuildingInstance;
    private CardHighLightManager cardHighLightManager;
    
    [Header("Default No prefab")]
    [SerializeField] private bool useDestroyCard = false;
    private string groundTag = "Ground";
    // private string pathTag = "Path";
    [Header("Range indicator")]
    public GameObject forceFieldPrefab;
    private GameObject forceFieldGO;
    private float buildingRange;
    
    // cache the raycast variables
    private Ray ray;
    private RaycastHit hit;
    private int gridsLayerMask = (1 << 8);
    private bool changeSpeedBack;
    private void Start() {
        buildManager = BuildManager.instance;
        cardHighLightManager = CardHighLightManager.instance;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (cam == null) {
            Debug.LogError("No main camera in the scene!");
        }
        changeSpeedBack = true;
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
        buildingRange = BuildingInstance.GetRange();
        BuildingInstance.SetRange(0);
        
        // create force field to show the building range
        // scale = 2*range
        float forceFieldRange = buildingRange * 2;
        forceFieldGO = (GameObject)Instantiate(forceFieldPrefab, Input.mousePosition, Quaternion.identity);
        forceFieldGO.transform.localScale = new Vector3(forceFieldRange, forceFieldRange, forceFieldRange);
        
        
        // stop the game
        if (Time.timeScale == 0) changeSpeedBack = false;
        GameSpeedManager.instance.ChangeGameSpeed(0);
    }
    
    public void OnDrag(PointerEventData eventData) {
        // make sure that the building does not has a collider!!
        
        // only collides with ground layer
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridsLayerMask)) {
            if (!useDestroyCard) {
                buildingGO.transform.position = hit.point;
                forceFieldGO.transform.position = hit.point;
            }
            cardHighLightManager.SetActiveGrid(hit.transform.GetComponent<Grids>());
            // if (hit.transform.gameObject.CompareTag(groundTag) || hit.transform.gameObject.CompareTag(pathTag)) {
            //     // hit.transform.GetComponent<Grids>().CardHoverIndicator();
            // }
        }
        
    }
    public void OnEndDrag(PointerEventData eventData) {
        if (!useDestroyCard) {
            Destroy(buildingGO);
            Destroy(forceFieldGO);
        }
        ray = cam.ScreenPointToRay(Input.mousePosition);
        bool failToPlace = true;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridsLayerMask)) {
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
        
        cardHighLightManager.DeactivateGrid();
        // revert any changes
        if (failToPlace) {
            transform.localScale = origincalScale;
        }
        
        // start the game
        if (changeSpeedBack) {
            GameSpeedManager.instance.ChangeGameSpeedBack();
        }

    }
    private void OnDestroy() {
        // the card got destroy while holding it
        if (!useDestroyCard) {
            // remove the tmp game object
            if (buildingGO != null) {
                Destroy(buildingGO); 
            }
            if (forceFieldGO != null) {
                Destroy(forceFieldGO);     
            }
        }
    }
}
