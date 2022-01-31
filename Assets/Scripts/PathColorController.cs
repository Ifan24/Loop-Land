using UnityEngine;
using UnityEngine.EventSystems;

public class PathColorController : MonoBehaviour, Grids
{
    private Renderer rd;
    public Color hoverColor;
    public Color warningColor;
    public Color rangeColor;
    private Color defaultColor;
    private void Start() {
        rd = GetComponent<Renderer>();
        defaultColor = rd.material.color;
    }
    
    public void CardHoverIndicator() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        rd.material.color = warningColor;
    }
    public void RangeIndicator() {
        rd.material.color = rangeColor;
    }
    public void SetToDefaultColor() {
        rd.material.color = defaultColor;
    }
    private void OnMouseExit() {
        SetToDefaultColor();
    }
}
