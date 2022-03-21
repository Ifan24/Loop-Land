using UnityEngine;
using UnityEngine.EventSystems;

public class Grids : MonoBehaviour {

    protected bool hasBuilding;
    [SerializeField] protected Color hoverColor;
    [SerializeField] protected Color warningColor;
    [SerializeField] protected Color rangeColor;
    protected Color defaultColor;
    protected Renderer rd;
    CardHighLightManager cardHighLightManager;
    protected virtual void Start() {
        rd = GetComponent<Renderer>();
        defaultColor = rd.material.color;
        hasBuilding = false;
        cardHighLightManager = CardHighLightManager.instance;
    }
    // when dragging a card and hover over a ground with building
    // show warning color
    public virtual void CardHoverIndicator() {
        
        // the invisible panel might block the building place
        
        // if (EventSystem.current.IsPointerOverGameObject()) {
        //     return;
        // }
        if (hasBuilding) {
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
        // if (EventSystem.current.IsPointerOverGameObject()) {
        //     return;
        // }
        rd.material.color = hoverColor;
        // cardHighLightManager.SetActiveGrid(this);
        
    }
    private void OnMouseExit() {
        SetToDefaultColor();
        // cardHighLightManager.DeactivateGrid();
    }
}
