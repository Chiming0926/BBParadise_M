using UnityEngine;
using System.Collections;

public partial class AGCC
{
    internal void SetAudio(bool turnOn)
    {
        if (turnOn)
            gameObject.GetComponent<AudioSource>().Play();
        else
            gameObject.GetComponent<AudioSource>().Stop();
    }

    internal void SetBackgroundAudioLevel(float level)
    {
        gameObject.GetComponent<AudioSource>().volume = level;
    }
}