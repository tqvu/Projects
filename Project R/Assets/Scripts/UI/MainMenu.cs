using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public LevelLoader loader;
    public PlayerData playerdata;
    public GameObject playerPrefab;
    public GameObject[] dependencies;
    private int height;
    private int width;
    public string dataJSON;
    public void PlayGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "player.data");//path for save file
        if (File.Exists(path))
        {
            StartCoroutine(LoadGame());//make a player, add variables into it, then throw it into hub
        }
        else
        {
            NewGame();
        }
       
    }

    public void NewGame()
    {
        Debug.Log("New Game");
        StartCoroutine(loader.LoadingLevel("New Game"));
    }
    public IEnumerator LoadGame()
    {
        Debug.Log("Load Game");
        yield return StartCoroutine(loader.LoadingLevel("Hub"));
        GameObject player = Instantiate(playerPrefab);
        foreach (GameObject go in dependencies)
        {
            Instantiate(go);
            //go.SetActive(true);
        }
        player.GetComponent<PlayerStats>().Load();

    }

    public void QuitGame()
    {
        StartCoroutine(QuitRoutine()); 
    }
    
    List<int> widths = new List<int>() {800, 1024, 1280, 1366, 1920, 2560};
    List<int> heights = new List<int>() {600, 768, 720, 768, 1080, 1440};

    public void SetScreenSize (int index) 
    {
        bool fullscreen = Screen.fullScreen;
        width = widths[index];
        height = heights[index];
        Screen.SetResolution(width, height, fullscreen);
    }

    public void SetFullscreen (bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }

    public IEnumerator QuitRoutine()
    {
        loader.animator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        Debug.Log("QUIT");
        Application.Quit();
    }

    public void SaveSettings()
    {
        SettingsData data = new SettingsData(height, width, Screen.fullScreen, FindObjectOfType<Slider>().value);
        Debug.Log(Application.persistentDataPath);
        string savedata = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/PlayerSettings.json", savedata);
    }

    public void LoadSettings() 
    {
        if(File.Exists(Application.persistentDataPath + "/PlayerSettings.json"))
        {
            dataJSON = File.ReadAllText(Application.persistentDataPath + "/PlayerSettings.json");
            JsonUtility.FromJson<SettingsData>(dataJSON);
            Debug.Log(JsonUtility.FromJson<SettingsData>(dataJSON));
        }
    }

}
