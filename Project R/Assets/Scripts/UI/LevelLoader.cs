using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;
    public float transitionTime;
    public SceneTeleporter teleporter;


    public IEnumerator LoadingLevel(string scene)
    {
        animator.SetTrigger("Start");
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (!source.clip.ToString().Contains("BGM"))
            {
                StartCoroutine(StartFade(source, transitionTime, 0f));
            }
            
        }
        

        yield return new WaitForSeconds(transitionTime);

        if(scene != null)
        {
            
            SceneManager.LoadScene(scene);

            if (teleporter != null)
            {
                teleporter.StartLoad(scene);
            }
        }
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.Stop();

        yield break;
    }

}
