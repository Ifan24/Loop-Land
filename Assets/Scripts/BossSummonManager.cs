using UnityEngine;
using UnityEngine.UI;

public class BossSummonManager : MonoBehaviour
{
    public static BossSummonManager instance;
    public int numberOfBuildingToSummon = 100;
    [SerializeField]
    private int numberOfBuilding;
    public Image bossSummonBar;
    public bool summonBoss;
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one BossSummonManager in scene!");
            return;
        }
        // Singleton
        instance = this;
    }
    
    private void Start() {
        numberOfBuilding = 0;
        bossSummonBar.fillAmount = 0;
        summonBoss = false;
    }
    
    public void addNumberOfBuilding(int n) {
        if (summonBoss) return;
        numberOfBuilding += n;
        bossSummonBar.fillAmount = (float)numberOfBuilding / (float)numberOfBuildingToSummon;
        if (numberOfBuilding == numberOfBuildingToSummon){
            summonBoss = true;
            // TODO: summon the boss
        }
    }
    
    
}
