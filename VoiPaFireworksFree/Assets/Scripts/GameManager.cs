using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>{
    // Camera
    public GameObject MainCamera;

    // UI
    public GameObject SettingUI;
    public GameObject AudioSpectrumUI;
    public GameObject AdvancedSettingUI;
    public GameObject FireworksUI;
    public GameObject QuitUI;
    private GameObject[] allUI;
    public Dropdown dropdownSkybox;
    public Slider SliderLivelyEffect;
    public Slider SliderShootingWidth;
    public Slider SliderCameraAngle;
    public Slider SliderCameraZoom;
    public Slider SliderCameraHeight;
    
    // SkyBox
    public bool skyboxRotation = true;
    public Material skyboxColdNight;
    public Material skyboxDeepDusk;
    public Material skyboxNightMoonBurst;
    private Material[] skyboxes;
    private Material skybox;
    public float skyRotSpeed = 0.001f;
    private float skyRotVal;

    // Game Objects
    public Fireworks fireworks;

    // Status
    public bool isFireworks = true;
    private bool isScene = false;

    // Start is called before the first frame update
    void Start(){
        // All UI
        allUI = new GameObject[] { SettingUI, AudioSpectrumUI, AdvancedSettingUI, FireworksUI, QuitUI };

        // Skybox Material
        skyboxes = new Material[] { skyboxNightMoonBurst, skyboxColdNight, skyboxDeepDusk };
        // Set DropdownSkybox
        dropdownSkybox.AddOptions(skyboxes.Select(x => x.name).ToList());
        // Set Skybox
        OnDropdownSkyboxChanged();

        // Show SettingUI
        ShowSettingUI();

        // Set Prefs Default
        ChangePrefs(Constant.PREFS_01);
    }

    // Update is called once per frame
    void Update(){
        // Skybox Rotation
        if (skyboxRotation) {
            skyRotVal = Mathf.Repeat(skybox.GetFloat("_Rotation") + skyRotSpeed, 360f);
            skybox.SetFloat("_Rotation", skyRotVal);
        }

        // Key Event
        // Escape -> SettingUI
        if(Input.GetKeyDown(KeyCode.Escape)) { 
            // Stop Fireworks
            if (isScene) {
                ShowSettingUI();
                isScene = false;
            }
            //  Show Quit UI
            else {
                ShowUI(QuitUI);
            }
        }
    }

    // Show FireWorks UI
    public void ShowFireworksUI() {
        // Deactivate All UI
        foreach (GameObject UI in allUI) {
            UI.SetActive(false);
        }
        // Active Fireworks UI
        FireworksUI.SetActive(true);
        isScene = true;
    }

    // Show Advanced Setting UI
    public void ShowAdvancedSettingUI() {
        // Deactivate All UI
        foreach (GameObject UI in allUI) {
            UI.SetActive(false);
        }
        // Active Fireworks UI
        AdvancedSettingUI.SetActive(true);
        AudioSpectrumUI.SetActive(true);
        isScene = false;
    }

    // Show Setting UI
    public void ShowSettingUI() {
        // Deactivate All UI
        foreach (GameObject UI in allUI) {
            UI.SetActive(false);
        }
        // Active Setting UI
        SettingUI.SetActive(true);
        AudioSpectrumUI.SetActive(true);
        isScene = false;
    }

    // Show New UI
    void ShowUI(GameObject newUI) {
        // Deactivate All UI
        foreach(GameObject UI in allUI) {
            UI.SetActive(false);
        }
        // Activate New UI
        newUI.SetActive(true);
    }

    // Change Skybox Material
    public void OnDropdownSkyboxChanged() {
        // get new skybox from Dropdown
        skybox = skyboxes[dropdownSkybox.value];
        // Set new skybox
        RenderSettings.skybox = skybox;
    }

    // Change Camera Angle
    public void OnSliderCameraAngleChanged() {
        // Get Value
        float value = (float)SliderCameraAngle.GetComponent<Slider>().value;
        // Change Camera Angle
        MainCamera.transform.rotation = Quaternion.Euler(value, 0, 0);
    }

    // Change Camera Position
    public void OnCameraPositionChanged() {
        // Get Value
        float height = (float)SliderCameraHeight.GetComponent<Slider>().value;
        float depth = (float)SliderCameraZoom.GetComponent<Slider>().value;
        // Change Camera Position
        MainCamera.transform.position = new Vector3(0, height, depth);
    }

    // Prefs Button 01
    public void OnButtonPrefs01() {
        // Read Preferences
        Prefs pref = Constant.PREFS_01;
        // Change Prefs
        ChangePrefs(pref);
    }

    // Prefs Button 02
    public void OnButtonPrefs02() {
        // Read Preferences
        Prefs pref = Constant.PREFS_02;
        // Change Prefs
        ChangePrefs(pref);
    }

    // Change Prefs
    private void ChangePrefs(Prefs pref) {
        // Set Value
        MainCamera.transform.position = new Vector3(0, pref.cameraHeight, pref.cameraDepth);
        MainCamera.transform.rotation = Quaternion.Euler(pref.cameraAngle, 0, 0);
        fireworks.ChangeLivelyEffectThreshold(pref.livelyEffect);
        fireworks.ShootingWidth = pref.shootingWidth;
        // Set UI
        SliderCameraAngle.GetComponent<Slider>().value = pref.cameraAngle;
        SliderCameraHeight.GetComponent<Slider>().value = pref.cameraHeight;
        SliderCameraZoom.GetComponent<Slider>().value = pref.cameraDepth;
        SliderLivelyEffect.GetComponent<Slider>().value = pref.livelyEffect;
        SliderShootingWidth.GetComponent<Slider>().value = pref.shootingWidth;
    }

    public void OnQuitUIButtonOKPressed() {
        Application.Quit();
    }
}
