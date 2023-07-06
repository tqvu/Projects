using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData 
{
    public int height;
    public int width;
    public bool fullscreen;
    public float volume;

    public SettingsData(int height, int width, bool fullscreen, float volume)
    {
        this.height = height;
        this.width = width;
        this.fullscreen = fullscreen;
        this.volume = volume;
    }
}
