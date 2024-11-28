using System;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour{
    AudioSource source;
    public event Action<float> magnitude_updated;
    public float[] samples = new float[64];
    private float smoothedMagnitude = 0; // Smoothed magnitude
    [SerializeField] private float smoothFactor = 0.5f; // 0 = no smoothing, 1 = very smooth

    void Start() => AudioClipHandler.play(
        SoundID.DORMINS_VOICE, 
        randomise_pitch: false, 
        spatial_blend: false, 
        audio_player: this, 
        loop: true, 
        out source);

    void FixedUpdate(){
        float rawMagnitude = 0;
        source.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        foreach(float x in samples)
            rawMagnitude += x;
        rawMagnitude *= 10;

        // Apply smoothing
        smoothedMagnitude = Mathf.Lerp(smoothedMagnitude, rawMagnitude, smoothFactor);

        magnitude_updated?.Invoke(smoothedMagnitude);
    }
}
