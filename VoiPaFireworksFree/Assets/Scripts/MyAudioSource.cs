using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MyAudioSource : MonoBehaviour{
    /* Properties */
    // GameManager
    public GameObject GameManager;
    private bool isFireworks;
    // AudioAnalyser
    public MyAudioAnalyzer audioAnalyzer;
    // Fireworls
    public Fireworks fireworks;

    // UI
    // Audio Device UI
    public Dropdown DropdownAudioDevices;
    // Audio Spectrum UI
    public GameObject AudioSpectrumUI;
    public GameObject SpectrumBars;
    public GameObject SliderSpectrumThreshold;
    public GameObject SliderBandPass01;
    public GameObject SliderBandPass02;
    private List<GameObject> bar_list = new List<GameObject>();
    public GameObject SpectrumBarPrefab;
    public int AudioSpectrumOffsetPosition = -500;

    // Audio
    // Microphone Device Name
    private string[] devices;
    private string device = "";
    // Audio Source
    private AudioSource audioSource;
    private int clipLengthSec = 1;
    // Sampling Rate
    private int samplingRate = 44100;
    // FFT Sumple Number
    private int sample = 4096;
    private int sampleMax = 1024;
    // FFT Parameter
    private float df;
    private float freqSampleMax;
    // FFT Sample List
    private int[] sample_list = new int[] { 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
    private int sample_index = 6;
    // FFT Frequency
    static float[] frequency;

    // FFT Spectrum
    // SMA Window
    public int SpectrumSMAWindow = 3;
    // Normalized 0 div
    private float rmsNormDiv = 0.025f;
    // Filter
    private float freqMinHz = 65.4f;
    private float freqMaxHz;
    // Spectrum Threshold
    public float spectrumThresholdMin = 0.20f;
    private float spectrumThreshold = 0.25f;
    
    // Tone List
    private List<Tone> toneList = new List<Tone>();
    private List<Tone> toneListBuffer = new List<Tone>();
    private int toneCountCurrent = 0;

    // Chord List
    private List<Tone> chordToneList = new List<Tone>();
    private int chordToneListCountMax = 4;

    // RMS Buffer
    private List<float> rmsBuffer = new List<float>();
    private float rmsMeanCurrent = 0.0f;
    private float rmsSDCurrent = 0.0f;

    // Frame Count
    private int fps;
    private int frameCount = 0;
    public int frameCountMin = 3;

    // Shoot Span
    private int shootSpan;

    // BPM
    private int bpm;
    public int bpmDefault = 96;
    public int bpmMin = 40;
    public int bpmMax = 180;
    public int bpmIncrement = 2;

    /* START */
    // Start is called before the first frame update
    void Start(){
        /* Audio Device init */
        devices = Microphone.devices;
        if (devices.Any()) {
            device = devices[0];
        }

        /* Game Manager init */
        // Is Fireworks Flag
        isFireworks = GameManager.GetComponent<GameManager>().isFireworks;
        // FPS, BPM, Shoot Span
        fps = GameManager.GetComponent<GameManager>().targetFPS;
        bpm = bpmDefault;
        shootSpan = bpm2Frame(bpm, fps);

        /* UI init */
        // AudioDevices Dropdown Options
        DropdownAudioDevices.AddOptions(devices.ToList());

        // Audio Spectrum
        // Get Bar Width
        float bar_width = SpectrumBarPrefab.GetComponent<RectTransform>().sizeDelta.x;
        // First Bar position
        float pos_x = (Screen.width - sampleMax * bar_width) / 2 - (Screen.width / 2);
        for (int i=0; i < sampleMax; i++) {
            // Instantiate Bar Prefab
            var bar = Instantiate(SpectrumBarPrefab);
            // Add Bar List
            bar_list.Add(bar);
            // Attach Bar to UI
            bar.transform.SetParent(SpectrumBars.transform, false);
            // Change Height Scale
            bar.transform.localScale = new Vector2(1, 0.0f);
            // Bar Position
            float x = pos_x + i * bar_width;
            float y = (float)AudioSpectrumOffsetPosition;
            RectTransform bar_rect = bar.GetComponent<RectTransform>();
            bar_rect.localPosition = new Vector2(x, y);
        }
        // Spectrum Threshold Slider Position
        float slider_height = SliderSpectrumThreshold.GetComponent<RectTransform>().sizeDelta.y;
        SliderSpectrumThreshold.transform.localPosition = new Vector2(pos_x, AudioSpectrumOffsetPosition + slider_height / 2);
        // Spectrum Threshold init
        OnSliderSpectrumThresholdChanged();

        /* Analysis init */
        // FFT Parameter
        df = (float)samplingRate / 2 / (float)sample;
        freqSampleMax = df * sampleMax;
        // FFT Frequency
        frequency = new float[sample];
        for (int i = 0; i < frequency.Length; i++) {
            frequency[i] = i * df;
        }
        // Band Pass Filter
        freqMaxHz = freqSampleMax;
        // Set Slider Value
        SliderBandPass01.GetComponent<Slider>().value = freqMinHz / freqSampleMax;
        SliderBandPass02.GetComponent<Slider>().value = SliderBandPass02.GetComponent<Slider>().maxValue;
        
        /* Audio Source init */
        // Audio Setting
        var config = AudioSettings.GetConfiguration();
        config.sampleRate = samplingRate;
        AudioSettings.Reset(config);
        // Audio Clip Start
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(device, true, clipLengthSec, samplingRate);
        audioSource.Play();
    }

    /* UPDATE */
    // Update is called once per frame
    void Update() {
        // Audio Data
        float[] outputAll = new float[sample];
        float[] spectrumAll = new float[sample];
        float[] spectrum = new float[sampleMax];

        // Get Output
        audioSource.GetOutputData(outputAll, 0);

        // RMS
        float rms = RMS(outputAll);
        // Add RMS Buffer
        rmsBuffer.Add(rms);
        
        // Get All Spectrum
        audioSource.GetSpectrumData(spectrumAll, 0, FFTWindow.Rectangular);
        // Using Spectrum
        Array.Copy(spectrumAll, 0, spectrum, 0, sampleMax);

        // Spectrum PreProcessing
        // Normalize by RMS
        for (int i = 0; i < spectrum.Length; i++) {
            spectrum[i] /= (rms + rmsNormDiv);
            spectrum[i] = Mathf.Sqrt(spectrum[i]);
        }
        // Moving Average
        spectrum = SMA(spectrum, SpectrumSMAWindow);

        // Peak Detect
        // Spectrum Peak Index
        int[] peakIndex = PeakIndex(spectrum, spectrumThreshold);

        // Band Pass Filter
        peakIndex = BandPassFilterToPeaks(peakIndex, frequency, freqMinHz, freqMaxHz);

        // Tone List init
        toneList.Clear();

        // Peak Index to Tone List
        if (peakIndex.Any()) {
            for (int i = 0; i < peakIndex.Length; i++) {
                // Get Peak Frequency
                int index = peakIndex[i];
                float freq = frequency[index];

                // Create Tone Instance
                Tone tone = new Tone();
                tone.number = Freq2NoteNum(freq);
                tone.count = 0;
                tone.volume = spectrum[index];
                // Add Tone List
                toneList.Add(tone);
            }
        }

        // Remove Dupulicated Tone with Averaging Volume
        var groupList = toneList.GroupBy(x => x.number).ToList();
        toneList.Clear();
        foreach (var group in groupList){
            float volume_temp = 0.0f;
            foreach (var item in group){
                volume_temp += item.volume;
            }
            // Create Tone Copy
            Tone tone = new Tone();
            tone.number = group.Key;
            tone.count = 0;
            tone.volume = volume_temp / group.Count();
            // Add Tone List
            toneList.Add(tone);
        }

        // Update Fireworks
        if (isFireworks) {
            // Add Tone to Tone Buffer
            foreach (Tone tone in toneList){
                toneListBuffer.Add(tone);
            }

            // Fireworks Shoot Timing
            if (frameCount % shootSpan == 0) {

                // Fireworks Processing if Any Tone
                if (toneListBuffer.Any()) {

                    // Tone Count with Averaging Volume
                    var groupListBuffer = toneListBuffer.GroupBy(x => x.number).ToList();
                    toneListBuffer.Clear();
                    foreach (var group in groupListBuffer) {
                        int countTemp = 0;
                        float volumeTemp = 0.0f;
                        foreach (var item in group){
                            countTemp += 1;
                            volumeTemp += item.volume;
                        }
                        // Create Tone Copy
                        Tone tone = new Tone();
                        tone.number = group.Key;
                        tone.count = countTemp;
                        tone.volume = volumeTemp / group.Count();
                        // Add Tone
                        toneListBuffer.Add(tone);
                    }

                    // Count Filtering
                    toneListBuffer = toneListBuffer.Where(x => x.count > frameCountMin).ToList();

                    // Sort by Number
                    toneListBuffer.OrderBy(x => x.number);

                    // Get Chord
                    int[] toneArray = toneListBuffer.Select(x => x.number).ToArray();
                    var chordArray = audioAnalyzer.GetChordArray(toneArray);
                    for (int i = 0; i < toneListBuffer.Count; i++) {
                        toneListBuffer[i].chordRef = chordArray[i].Item2;
                        toneListBuffer[i].chordID = chordArray[i].Item3;
                    }

                    // Chord Progression
                    /*
                    // Get Main Chord Tone
                    var mainChordTone = MyAudioAnalyzer.GetMainChordTone(toneListBuffer.Where(x => x.number <= 60).ToList());
                    // Any Main Chord Tone
                    if (mainChordTone != null) {
                        // Add Chord Tone List
                        if (chordToneList.Count() > 0) {
                            // Last Tone
                            var lastChordTone = chordToneList.Last();
                            if (mainChordTone.number % 12 != lastChordTone.number % 12) {
                                chordToneList.Add(mainChordTone);
                            }
                        }
                        else {
                            chordToneList.Add(mainChordTone);
                        }
                    }
                    // Get Last Chords 
                    if (chordToneList.Count() > chordToneListCountMax) {
                        chordToneList = chordToneList.Skip(Math.Max(0, chordToneList.Count() - chordToneListCountMax)).ToList();
                    }

                    // Chord Progression
                    string chordProgression = MyAudioAnalyzer.GetChordProgression(chordToneList);

                    // Debug
                    if (chordToneList.Count() == chordToneListCountMax) {
                        string text = chordToneList[0].number.ToString() + "_" + chordToneList[1].number.ToString() + "_" + chordToneList[2].number.ToString() + "_" + chordToneList[3].number.ToString();
                        Debug.Log(text);
                        Debug.Log(chordProgression);
                    }
                    */

                    // RMS : Mean and SD
                    rmsMeanCurrent = rmsBuffer.Average();
                    rmsSDCurrent = SD(rmsBuffer.ToArray());

                    // Shoot Fireworks!!!
                    // Shoot Rising Stars
                    fireworks.shootRising(toneListBuffer, rmsMeanCurrent);

                    // Shoot Ground Stars
                    fireworks.shootGroundEffect(toneListBuffer, rmsMeanCurrent);
                }

                // RMS Analysis => Change Shoot Span
                if (rmsBuffer.Any()) {
                    // RMS Difference
                    var rmsDiff = Diff(rmsBuffer.ToArray());
                    // RMS Difference Normalize
                    float mean = rmsDiff.Average();
                    float std = SD(rmsDiff);
                    var rmsDiffNorm = rmsDiff.Select(x => (x - mean) / (std + 1e-6f)).ToArray();
                    // RMS Difference Count Over Threshold 
                    int rmsDiffCount = rmsDiffNorm.Where(x => x > 2.4).ToArray().Length;
                    // change shoot span
                    if (rmsDiffCount == 0) {
                        bpm -= bpmIncrement;
                    }
                    else if (rmsDiffCount > 1) {
                        bpm += bpmIncrement;
                    }
                    // Clamp Shoot Span with min max value
                    bpm = Mathf.Clamp(bpm, bpmMin, bpmMax);
                    // Shoot Span
                    shootSpan = bpm2Frame(bpm, fps);
                    // Debug
                    /*
                    Debug.Log("DiffMax : " + rmsDiffNorm.Max());
                    Debug.Log("Count : " + rmsDiffCount);
                    Debug.Log("bpm : " + bpm);
                    */
                }

                // Reset Buffer
                toneListBuffer.Clear();
                rmsBuffer.Clear();
                // Reset Frame Count
                frameCount = 0;
            }

            // Frame Count
            frameCount += 1;
        }

        // Update Audio Spectrum UI
        if (AudioSpectrumUI.activeSelf) {
            UpdateAudioSpectrumUI(spectrum, peakIndex);
        }
    }

    // Update AudioSpectrumUI
    private void UpdateAudioSpectrumUI(float[] spectrum, int[] peak_index){
        // Update Spectrum Bar
        for (int i = 0; i < sampleMax; i++){
            // Spectrum bar
            GameObject bar = bar_list[i];
            // Value
            float value = spectrum[i];
            // Bar Scale
            float scale_height = Mathf.Clamp(value, 0.0f, 1.0f);
            bar.transform.localScale = new Vector2(1, scale_height);
            // Bar Position
            float height = SpectrumBarPrefab.GetComponent<RectTransform>().sizeDelta.y * scale_height;
            float x = bar.transform.localPosition.x;
            float y = (float)AudioSpectrumOffsetPosition + height / 2;
            bar.transform.localPosition = new Vector2(x, y);
            // Peak
            if (peak_index.Contains(i)){
                bar.GetComponent<Image>().color = Color.red;
            }
            else{
                bar.GetComponent<Image>().color = Color.white;
            }
        }
    }

    /* Mathematics */
    // RMS : Root Mean Squared
    private float RMS(float[] array){
        int len = array.Length;
        float square = 0f;
        for (int i=0; i < len; i++){
            square += Mathf.Pow(array[i], 2);
        }
        float mean = square / (float)len;
        float rms = (float)Math.Sqrt(mean);
        return rms;
    }

    // Difference
    private float[] Diff(float[] array) {
        // Array Check
        if (!array.Any()) {
            return array;
        }
        // Difference
        float[] diff = new float[array.Length];
        diff[0] = 0;
        for (int i = 1; i < array.Length; i++) {
            diff[i] = array[i] - array[i - 1];
        }
        return diff;
    }

    // SD : Standard Deviation
    private float SD(float[] array) {
        float dst = 0.0f;
        // Array Check
        if (!array.Any()) {
            return dst;
        }
        // average
        float ave = array.Average();
        // Summation of Squared
        float sum2 = array.Select(x => x * x).Sum();
        // Variance
        float var = sum2 / array.Length - ave * ave;
        // Standard Deviation
        dst = Mathf.Sqrt(var);
        return dst;
    }

    // SMA : Simple Moving Average
    private float[] SMA(float[] array, int window_size = 3) {
        // Cehck Odd Number
        if (window_size % 2 == 0 || window_size <= 1) {
            return array;
        }
        // SMA Destination
        float[] dst = new float[array.Length];
        // 0 Padding Data by window size
        float[] pad = new float[array.Length + window_size - 1];
        int first_index = (window_size - 1) / 2;
        Array.Copy(array, 0, pad, first_index, array.Length);

        /*
        // SMA
        float[] temp = Enumerable.Range(0, pad.Length - window_size + 1)
            .Select(i => pad.Skip(i).Take(window_size).Average()).ToArray();
        // Copy to Destination
        Array.Copy(temp, 0, dst, 0, array.Length);
        */

        // SMA
        for (int i = 0; i < array.Length; i++) {
            dst[i] = pad.Skip(i).Take(window_size).Average();
        }

        return dst;
    }

    // Detect Peak Index
    private int[] PeakIndex(float[] array, float threshold = 0.1f) {
        float[] diff = new float[array.Length];
        float[] diff2 = new float[array.Length];
        List<int> index = new List<int>();
        // Difference
        diff[0] = 0;
        for(int i = 1; i < array.Length; i++) {
            diff[i] = array[i] - array[i - 1];
        }
        // Difference 
        diff2[0] = 0;
        diff2[1] = 0;
        for (int i = 2; i < array.Length; i++) {
            diff2[i] = diff[i] * diff[i - 1];
        }
        // Peak
        for (int i = 2; i < array.Length; i++) {
            // +-reverse
            if (diff2[i] < 0) {
                // 0 Cross
                if (diff[i] < 0 && diff[i - 1] > 0) {
                    // Over Threshold
                    if (array[i] > threshold) {
                        index.Add(i);
                    }
                }
            }
        }
        int[] dst = index.ToArray();
        return dst;
    }

    // Band Pass Filtering to Peak Index
    private int[] BandPassFilterToPeaks(int[] peak_index, float[]frequency, float freq_min_hz, float freq_max_hz) {
        List<int> dst = new List<int>();
        for (int i = 0; i < peak_index.Length; i++) {
            // Get Peak Frequency
            int index = peak_index[i];
            float freq = frequency[index];
            // Band Pass Filter
            if (freq >= freq_min_hz && freq <= freq_max_hz) {
                dst.Add(index);
            }
        }
        return dst.ToArray();
    }

    /* Convert Unit */
    // Frequency to Note Number
    private int Freq2NoteNum(float frequency, int a4_hz = 442) {
        return Mathf.FloorToInt(69 + 12 * Mathf.Log(frequency / a4_hz, 2));
    }

    // BPM to Frame
    private int bpm2Frame(int bpm, int fps) {
        return (int)(fps / (bpm / 60f));
    }

    /* CallBack */
    // Spectrum Threshold Changed
    public void OnSliderSpectrumThresholdChanged() {
        float value = SliderSpectrumThreshold.GetComponent<Slider>().value;
        if (value >= spectrumThresholdMin) {
            spectrumThreshold = SliderSpectrumThreshold.GetComponent<Slider>().value;
        }
        else {
            spectrumThreshold = spectrumThresholdMin;
            SliderSpectrumThreshold.GetComponent<Slider>().value = spectrumThresholdMin;
        }
        
    }

    // BandPass Changed
    public void OnSliderBandPassChanged() {
        // Get Value from Slider
        float freq01 = SliderBandPass01.GetComponent<Slider>().value * freqSampleMax;
        float freq02 = SliderBandPass02.GetComponent<Slider>().value * freqSampleMax;
        // Lower value is FreqMin, Higher value is FreqMax
        if (freq02 >= freq01) {
            freqMinHz = freq01;
            freqMaxHz = freq02;
        }
        else {
            freqMinHz = freq02;
            freqMaxHz = freq01;
        }
    }

    // Dropdown Audio Device changed
    public void OnDropdownAudioDevicesChanged() {
        // Stop Audio Clip
        audioSource.Stop();
        // Stop Microphone
        Microphone.End(device);
        // Wait for Stop
        while (Microphone.IsRecording(device)) { };

        // Get Audio Device from Dropdown list
        string newDevice = Microphone.devices[DropdownAudioDevices.value];

        // Set New device
        audioSource.clip = Microphone.Start(newDevice, true, clipLengthSec, samplingRate);
        // Wait for new Device Ready
        while (Microphone.GetPosition(newDevice) == 0) { }

        audioSource.Play();
    }

    private void OnValidate() {
        // Check sample is in sample list
        if (!sample_list.Contains(sample)) {
            sample = sample_list[sample_index];
        }
    }
}
