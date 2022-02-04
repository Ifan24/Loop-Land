using UnityEngine;
using UnityEngine.EventSystems;
public class Ground : MonoBehaviour, Grids
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color rangeColor;
    private Vector3 offset;
    private Color defaultColor;
    
    private Renderer rd;
    private GameObject turret;
    private BuildManager buildManager;
    public GameObject buildEffect;
    public GameObject destroyEffect;
    private BossSummonManager bossSummonManager;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Renderer>();
        defaultColor = rd.material.color;
        buildManager = BuildManager.instance;
        bossSummonManager = BossSummonManager.instance;
    }
    
    public bool PlaceBuilding() {
        if (EventSystem.current.IsPointerOverGameObject() || turret != null) {
            return false;
        }
        GameObject turretToBuild = buildManager.GetTurretToBuild();
        if (turretToBuild != null) {
            offset = turretToBuild.GetComponent<Building>().GetheightOffset();
            turret = (GameObject) Instantiate(turretToBuild, GetBuildPosition(), transform.rotation);
            GameObject effectGO = Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity) as GameObject;
            Destroy(effectGO, 5);
            
            bossSummonManager.addNumberOfBuilding(1);
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
        else {
            rd.material.color = hoverColor;
        }
    }
    public void RangeIndicator() {
        rd.material.color = rangeColor;
    }
    public void SetToDefaultColor() {
        rd.material.color = defaultColor;
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
            bossSummonManager.addNumberOfBuilding(-1);
            return true;
        }
        return false;
    }
    private void OnMouseExit() {
        SetToDefaultColor();
    }
}
