using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DissolveCard : MonoBehaviour
{
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private float dissolveTime;
    private float originalDissolveTime;
    private Image image;
    private string progressCache;
    private bool isDissolving;
    private void Start() {
        image = GetComponent<Image>();
        progressCache = "_Progress";
        isDissolving = false;
        originalDissolveTime = dissolveTime;
    }
    
    // replace destory card with this function call
    public void DissolveCardNow() {
        image.material = dissolveMaterial;
        dissolveMaterial.SetFloat(progressCache, 1);
        isDissolving = true;
        GetComponentInChildren<Text>().enabled = false;
    }
    
    private void Update() {
        if (!isDissolving) return;
        dissolveTime -= Time.deltaTime;
        if (dissolveTime <= 0) {
            Destroy(gameObject);
            return;
        }
        dissolveMaterial.SetFloat(progressCache, dissolveTime / originalDissolveTime);
    }
}
