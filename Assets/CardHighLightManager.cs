using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHighLightManager : Singleton<CardHighLightManager>
{
    private Grids activeGrid;
    
    public void SetActiveGrid(Grids grid) {
        if (grid == null || activeGrid == grid) return;
        if (activeGrid != null) {
            activeGrid.SetToDefaultColor();
        }
        activeGrid = grid;
        activeGrid.CardHoverIndicator();
    }
    
    public void DeactivateGrid() {
        if (activeGrid != null) {
            activeGrid.SetToDefaultColor();
        }
    }

}
