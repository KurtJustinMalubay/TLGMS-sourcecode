using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class optionsScript : MonoBehaviour
{
    public UniversalRenderPipelineAsset urpAsset;
    public Image lowImg;
    public Image mediumImg;
    public Image highImg;
    public Image mobile;
    public Image pc;
    public settings sett;
    public playerScript playerS;
    [NonSerialized] public int active;
    [NonSerialized] public int contAct;
    [NonSerialized] public string saveFilePath;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(saveFilePath))
        {
            LoadData();
        }
        else
        {
            CreateNewGameData();
        }
        if (sett == null)
        {
            return;
        }

        active = sett.act;
        contAct = sett.contAct;
        setSettings();
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(sett);
        File.WriteAllText(saveFilePath, json);
    }
    void LoadData()
    {
        string loadedJson = File.ReadAllText(saveFilePath);
        sett = JsonUtility.FromJson<settings>(loadedJson);
    }
    void CreateNewGameData()
    {
        sett = new settings
        {
            act = 2,
            contAct = 1,
            isMobile = false
        };
        SaveData();
    }

    public void low()
    {
        QualitySettings.SetQualityLevel(0);
        active = 0;
        urpAsset.renderScale = 0.3f;
        lowImg.color = new Color32(0, 181, 226, 255);
        mediumImg.color = Color.white;
        highImg.color = Color.white;
    }
    public void medium()
    {
        QualitySettings.SetQualityLevel(1);
        active = 1;
        urpAsset.renderScale = 0.6f;
        mediumImg.color = new Color32(0, 181, 226, 255);
        lowImg.color = Color.white;
        highImg.color = Color.white;
    }
    public void high()
    {
        QualitySettings.SetQualityLevel(2);
        active = 2;
        urpAsset.renderScale = 1f;
        highImg.color = new Color32(0, 181, 226, 255);
        lowImg.color = Color.white;
        mediumImg.color = Color.white;
    }

    public void setSettings()
    {
        switch (active)
        {
            case 0:
                low();
                break;
            case 1:
                medium();
                break;
            case 2:
                high();
                break;
        }
        switch (contAct) {
            case 0:
                Mobile();
                break;
            case 1:
                PC();
                break;
        }
    }

    public void Mobile()
    {
        contAct = 0;
        mobile.color = new Color32(0, 181, 226, 255);
        pc.color = Color.white;
        sett.isMobile = true;
        if (playerS != null)
        {
            playerS.mobileControls.SetActive(true);
            playerS.botUI.SetActive(false);
        }
    }

    public void PC()
    {
        contAct = 1;
        mobile.color = Color.white;
        pc.color = new Color32(0, 181, 226, 255);
        sett.isMobile = false;
        if (playerS != null)
        {
            playerS.mobileControls.SetActive(false);
            playerS.botUI.SetActive(true);
        }
    }
    public class settings
    {
        public int act;
        public int contAct;
        public bool isMobile;
    }
}