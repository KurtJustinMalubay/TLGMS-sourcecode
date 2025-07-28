using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public GameObject options;
    public GameObject mainUI;
    public optionsScript save;
    public PlayerSets sets;
    public int score;
    private string playerProf;

    private void Start()
    {
        playerProf = Application.persistentDataPath + "/playerProf.json";
        if (File.Exists(playerProf))
        {
            LoadData();
        }
        else
        {
            CreateNewGameData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(sets);
        File.WriteAllText(playerProf, json);
    }
    void LoadData()
    {
        string loadedJson = File.ReadAllText(playerProf);
        sets = JsonUtility.FromJson<PlayerSets>(loadedJson);
    }
    void CreateNewGameData()
    {
        sets = new PlayerSets
        {
            maxScore = 0
        };
        SaveData();
    }
    public void LoadSceneStart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void openOptions()
    {
        options.SetActive(true);
        mainUI.SetActive(false);
        save.setSettings();
    }

    public void closeOptions()
    {
        options.SetActive(false);
        mainUI.SetActive(true);
        save.sett.act = save.active;
        save.sett.contAct = save.contAct;
        save.SaveData();
    }

    public void openYT()
    {
        Application.OpenURL("https://www.youtube.com/@SashimiOffice");
    }
    public void openTiktok()
    {
        Application.OpenURL("https://www.tiktok.com/@sashimi_21");
    }
    public void BMAC()
    {
        Application.OpenURL("https://buymeacoffee.com/justinkurtu");
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public class PlayerSets
    {
        public int maxScore;
    } 
}
