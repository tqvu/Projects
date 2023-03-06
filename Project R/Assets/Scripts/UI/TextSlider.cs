using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
     public TextMeshProUGUI numberText;
     public AudioMixer mixer;

     [SerializeField] private Slider slider;

     void Start() 
     {
        slider = GetComponent<Slider>();
        SetLevel(slider.value);
        SetNumberText(slider.value);
     }

     public void SetNumberText(float value)
     {
        value *= 100;
        numberText.text = value.ToString("F0");
     }

    public void SetLevel(float level)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(level) * 20);
    }
}
