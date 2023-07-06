using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;

public class SceneTeleporter : MonoBehaviour
{

    public string sceneToLoad;
    public GameObject loadingScreen;
    public Slider loadingBar;
    public PlayerControls controls;
    public GameObject spawn;
    bool isDuplicate = true;
    System.Random rand = new System.Random();

    public LevelLoader loader;

    public void Awake()
    {
        isDuplicate = true;
        loadingScreen = GameObject.FindGameObjectWithTag("Loading Screen").transform.GetChild(0).gameObject;
        loadingBar = loadingScreen.GetComponentInChildren<Slider>(true);
        spawn = GameObject.FindGameObjectWithTag("Spawn");
        controls = FindObjectOfType<PlayerControls>();
        OnLevelFinishedLoading();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (controls.gameObject.GetComponent<PlayerStats>().smallPowerups[3].enabled)
        {
            controls.gameObject.GetComponent<PlayerStats>().Healing(controls.gameObject.GetComponent<PlayerStats>().smallPowerups[3].level * 2);
        }

        if (other.CompareTag("Player") && !other.isTrigger)
        {

            controls.canMove = false;
            if(SceneManager.GetActiveScene().name == "Hub")
            {
                FindObjectOfType<PlayerStats>().Save();
            }

            if (controls.usedScenes.Count == 4 && controls.floor_number == 1)
            {
                sceneToLoad = "F1_BossRoom";
                isDuplicate = false;
                controls.floor_number++;
                
                controls.usedScenes.RemoveAll(s => s.Length != 0);
            }
            else if (controls.usedScenes.Count == 6 && controls.floor_number == 2)
            {
                sceneToLoad = "F2_BossRoom";
                isDuplicate = false;
                controls.floor_number++;

                controls.usedScenes.RemoveAll(s => s.Length != 0);
            }

            if (sceneToLoad.Length != 0)//if there is already a thing in the fill bar
            {
                isDuplicate = false;
                
            }
            else
            {
                while (isDuplicate)
                {
                    switch (controls.floor_number)
                    {
                        case 1:
                            switch (rand.Next(1, 5))
                            {
                                case 1:
                                    sceneToLoad = "F1_Zone1";
                                    break;
                                case 2:
                                    sceneToLoad = "F1_Zone2";
                                    break;
                                case 3:
                                    sceneToLoad = "F1_Zone3";
                                    break;
                                case 4:
                                    sceneToLoad = "F1_Zone4";
                                    break;
                                default:
                                    //Debug.Log("No room exists for this number : " + num.ToString());
                                    break;
                            }
                            break;
                        case 2:
                            switch (rand.Next(1, 7))
                            {
                                case 1:
                                    sceneToLoad = "F2_Zone1";
                                    break;
                                case 2:
                                    sceneToLoad = "F2_Zone2";
                                    break;
                                case 3:
                                    sceneToLoad = "F2_Zone3";
                                    break;
                                case 4:
                                    sceneToLoad = "F2_Zone4";
                                    break;
                                case 5:
                                    sceneToLoad = "F2_Zone5";
                                    break;
                                case 6:
                                    sceneToLoad = "F2_Zone6";
                                    break;
                                default:
                                    //Debug.Log("No room exists for this number : " + num.ToString());
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    

                    if (!(controls.usedScenes.Contains(sceneToLoad)))
                    {
                        controls.usedScenes.Add(sceneToLoad);
                        isDuplicate = false;
                    }

                    
                }
                
            }
            if (loader != null)
            {
                StartCoroutine(loader.LoadingLevel(sceneToLoad));
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
                controls.canMove = true;
            }
            
            



            //playerStorage.initialValue = playerPosition;
            //SceneManager.LoadScene(sceneToLoad);



        }
    }

    public void StartLoad(string scene)
    {
        StartCoroutine(LoadSceneAsynchronously(scene));
    }

    IEnumerator LoadSceneAsynchronously(string sceneToLoad)
    {
        
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1);

        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;

            yield return new WaitForSeconds(1);
        }

    }

    private void OnLevelFinishedLoading()
    {
        //Debug.Log(controls.usedScenes.Count);
        controls.canMove = true;
        controls.gameObject.transform.position = spawn.transform.position;
        loadingScreen.SetActive(false);
        foreach (Sound s in FindObjectOfType<AudioManager>().sounds)
        {
            if (s.source != null)
            {
                s.source.volume = s.volume;
            }
            
        }
        if (SceneManager.GetActiveScene().name == "Hub")//load first then save, to check if there is data
        {
            FindObjectOfType<PlayerStats>().Save();
        }
    }

    //return to hub function
}
