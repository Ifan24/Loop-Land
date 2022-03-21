using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject portal;
    public void Play() {
        StartCoroutine(LoadAsynchronously());
    }
    
    IEnumerator LoadAsynchronously() {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");
        
        while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            // portal.transform.localScale *= (progress * 10);
            
            yield return null;
        }
    }
}
