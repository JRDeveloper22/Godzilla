﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeiMusicManager : MonoBehaviour
{

    public AudioClip mainTheme;
    public AudioClip menuTheme;

    void Start()
    {
        WeiAudioManager.instance.PlayMusic(menuTheme, 2);
        Invoke("PlayMusic1", 3.0f);
    }

    void PlayMusic1()
    {
        WeiAudioManager.instance.PlayMusic(mainTheme, 1);
    }
    void Update()
    {


    }

}

