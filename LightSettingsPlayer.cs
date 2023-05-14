using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LightSettingsPlayer : MonoBehaviour
{
    [SerializeField] PostProcessVolume MyVolume;
    [SerializeField] PostProcessProfile Standard;
    [SerializeField] PostProcessProfile NightVision;
    [SerializeField] GameObject NightVisionOverlay;
    [SerializeField] GameObject Flashlight;
    [SerializeField] GameObject EnemyFlashlight;

    private bool NightVisionActive = false;
    private bool FlashlightActive = false;

    void Start()
    {
        NightVisionOverlay.gameObject.SetActive(false);
        Flashlight.gameObject.SetActive(false);
        EnemyFlashlight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.BatteryPower > 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (NightVisionActive == false)
                {
                    MyVolume.profile = NightVision;
                    NightVisionActive = true;
                    NightVisionOverlay.gameObject.SetActive(true);
                    SaveScript.NVLightOn = true;
                }
                else
                {
                    MyVolume.profile = Standard;
                    NightVisionActive = false;
                    NightVisionOverlay.gameObject.SetActive(false);
                    SaveScript.NVLightOn = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (FlashlightActive == false)
                {
                    FlashlightActive = true;
                    Flashlight.gameObject.SetActive(true);
                    EnemyFlashlight.gameObject.SetActive(true);
                    SaveScript.FlashLightOn = true;
                }
                else
                {
                    FlashlightActive = false;
                    Flashlight.gameObject.SetActive(false);
                    EnemyFlashlight.gameObject.SetActive(false);
                    SaveScript.FlashLightOn = false;
                }
            }
        }

        if(SaveScript.BatteryPower <= 0.0f)
        {
            MyVolume.profile = Standard;
            NightVisionActive = false;
            NightVisionOverlay.gameObject.SetActive(false);
            SaveScript.NVLightOn = false;
            FlashlightActive = false;
            Flashlight.gameObject.SetActive(false);
            EnemyFlashlight.gameObject.SetActive(false);
            SaveScript.FlashLightOn = false;
        }
    }
    
}
