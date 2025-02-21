using System;
using System.Collections;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

// DSP - digital signal proessor.
// FFT - fast fourier tramsform.

public class AudioSpectrum : MonoBehaviour{
    private Bus bus;
    private DSP fft_dsp;
    private ChannelGroup group;
    public event Action<float> magnitude_updated;
    private float smooth_factor = .1f;
    private int samples_length = 64;
    DSP_FFT_WINDOW_TYPE window_type = DSP_FFT_WINDOW_TYPE.RECT;

    public void initialize(Bus _bus){
        bus = _bus;
        bus.lockChannelGroup();
        // Flush commands to ensure the lock command is being called
        RuntimeManager.StudioSystem.flushCommands();
        RESULT result = bus.getChannelGroup(out group);
        RuntimeManager.CoreSystem.createDSPByType(DSP_TYPE.FFT, out fft_dsp);
        fft_dsp.setParameterInt((int)DSP_FFT.WINDOWSIZE, samples_length);
        fft_dsp.setParameterInt((int)DSP_FFT.WINDOW, (int)window_type);
        group.addDSP(0,fft_dsp);        
        UnityHook.instance.StartCoroutine(audio_data_loop());
    }

    public void uninitialize(){
        if(bus.IsUnityNull() == false){
            bus.unlockChannelGroup();
            group.removeDSP(fft_dsp);
            UnityHook.instance.StopCoroutine(audio_data_loop());
        }
    }

    IEnumerator audio_data_loop(){
        float magnitude = 0;
        while(true){
            float new_magnitude = 0f;
            // requesting spectrum data, outputing data (IntPtr) to the FFT data, and size of the data (length);
            fft_dsp.getParameterData((int) DSP_FFT.SPECTRUMDATA, out IntPtr data, out uint length);
            // if the data is not null.
            if(length > 0){
                // convert raw IntPtr data t structured DSP_PARAMTER_FFT.
                DSP_PARAMETER_FFT fft = (DSP_PARAMETER_FFT)System.Runtime.InteropServices.Marshal.PtrToStructure(data, typeof(DSP_PARAMETER_FFT));
                // if there is audio data available in a channel
                if(fft.numchannels > 0){
                    for(int i = 0; i < samples_length; i++)
                        new_magnitude += fft.spectrum[0][i];
                }
            }
            magnitude = Mathf.Lerp(magnitude, new_magnitude, smooth_factor);
            magnitude_updated?.Invoke(magnitude);            
            yield return new WaitForFixedUpdate();
        }
    }
}
