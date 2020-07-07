using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>{
    // UI
    public GameObject SettingUI;
    public GameObject AudioSpectrumUI;
    public GameObject FireworksUI;
    public GameObject QuitUI;
    public Dropdown dropdownSkybox;

    // SkayBox
    public bool skyboxRotation = true;
    public Material skyboxColdNight;
    public Material skyboxDeepDusk;
    public Material skyboxNightMoonBurst;
    private Material[] skyboxes;
    private Material skybox;
    public float skyRotSpeed = 0.001f;
    private float skyRotVal;

    // Status
    public bool IsFireworks = false;

    // Start is called before the first frame update
    void Start(){
        // Skybox Material
        skyboxes = new Material[] { skyboxNightMoonBurst, skyboxColdNight, skyboxDeepDusk };
        // Set DropdownSkybox
        dropdownSkybox.AddOptions(skyboxes.Select(x => x.name).ToList());
        // Set Skybox
        OnDropdownSkyboxChanged();

        // Show SettingUI
        ShowSettingUI();
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
            if (IsFireworks) {
                ShowSettingUI();
                IsFireworks = false;
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
        GameObject[] allUI = { SettingUI, AudioSpectrumUI, FireworksUI, QuitUI };
        foreach (GameObject UI in allUI) {
            UI.SetActive(false);
        }
        // Active Fireworks UI
        FireworksUI.SetActive(true);
        IsFireworks = true;
    }

    // Show Setting UI
    public void ShowSettingUI() {
        // Deactivate All UI
        GameObject[] allUI = { SettingUI, AudioSpectrumUI, FireworksUI, QuitUI };
        foreach (GameObject UI in allUI) {
            UI.SetActive(false);
        }
        // Active Setting UI
        SettingUI.SetActive(true);
        AudioSpectrumUI.SetActive(true);
        IsFireworks = false;
    }

    // Show New UI
    void ShowUI(GameObject newUI) {
        // Deactivate All UI
        GameObject[] allUI = { SettingUI, AudioSpectrumUI, FireworksUI, QuitUI };
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

    public void OnQuitUIButtonOKPressed() {
        Application.Quit();
    }
}
