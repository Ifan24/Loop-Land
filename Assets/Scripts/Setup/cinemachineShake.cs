using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cinemachineShake : MonoBehaviour
{
    private List<CinemachineBasicMultiChannelPerlin> camNoises;
    private float shakeTimer;
    public static cinemachineShake instance;
    
    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one cinemachineShake in scene!");
            return;
        }
        // Singleton
        instance = this;
        
        camNoises = new List<CinemachineBasicMultiChannelPerlin>();
        for(int i = 0; i < transform.childCount; i++) {
            var cam = transform.GetChild(i).GetComponent<CinemachineVirtualCamera>();
            var camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (camNoise != null) {
                camNoises.Add(camNoise);
            }
        }
        shakeTimer = 0;
    }
    
    public void ShakeCamera(float intensity, float time) {
        setAmplitude(intensity);
        shakeTimer = time;
    }
    
    private void Update() {
        if (shakeTimer > 0) {
            shakeTimer -= Time.unscaledDeltaTime;
            if (shakeTimer <= 0f) {
               setAmplitude(0);
            }
        }
    }
    
    private void setAmplitude(float amplitude) {
        foreach(var noise in camNoises) {
            noise.m_AmplitudeGain = amplitude;
        }
    }
}
