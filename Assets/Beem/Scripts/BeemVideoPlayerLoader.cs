using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class BeemVideoPlayerLoader : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private VideoClip[] _clips;
    [SerializeField] private string[] keys;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("VideoName"))
        {
            var key = PlayerPrefs.GetString("VideoName");
            var index = Array.FindIndex(keys, x => x.Contains(key));

            if (index >= 0)
            {
                _videoPlayer.clip = _clips[index];
                _videoPlayer.Play();
            }
        }
    }
}
