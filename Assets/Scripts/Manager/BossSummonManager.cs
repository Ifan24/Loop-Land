using UnityEngine;
using UnityEngine.UI;

public class BossSummonManager : MonoBehaviour
{
    [SerializeField] private int numberOfBuildingToSummon = 100;
    [SerializeField] private int numberOfBuilding;
    [SerializeField] private Image bossSummonBar;
    [HideInInspector] public bool isBossSummoned;
    [SerializeField] private SpawnEnemy portalPath;
    [SerializeField] private GameObject bossSummonEffect;
    [SerializeField] private GameObject portalGO;
    
    public static BossSummonManager instance;
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
        isBossSummoned = false;
    }
    
    public void addNumberOfBuilding(int n) {
        if (isBossSummoned) return;
        numberOfBuilding += n;
        bossSummonBar.fillAmount = (float)numberOfBuilding / (float)numberOfBuildingToSummon;
        if (numberOfBuilding == numberOfBuildingToSummon){
            isBossSummoned = true;
            SummonBoss();
        }
    }
    private void SummonBoss() {
        portalPath.SpawnEnemyOnTop();
        GameObject effectIns = (GameObject)Instantiate(bossSummonEffect, portalPath.transform.position, Quaternion.identity);
        Destroy(effectIns, 5f);
        // portalGO.SetActive(false);
        AudioManager.instance.Play("BossSummon");
        AudioManager.instance.Stop("BackgroundMusic");
        AudioManager.instance.Play("BossMusic");
    }
    
    
}
