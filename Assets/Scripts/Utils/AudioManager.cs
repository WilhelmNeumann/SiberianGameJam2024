using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    [SerializeField]
    private AudioClip[] musicPlaylist;

    [SerializeField]
    private AudioSource audioSource;

    [Header("SFX")]

    [Header("UI")]
    [SerializeField]
    private AudioSource buttonHover;

    [SerializeField]
    private AudioSource buttonClick;

    [Header("Dialogs")]
    [SerializeField]
    private AudioClip dialog;

    private void Awake()
    {
        instance = this;

        try
        {
            var r = Random.Range(0, musicPlaylist.Length);

            audioSource.clip = musicPlaylist[r];
            audioSource.loop = true;
            audioSource.Play();
        }
        catch(System.Exception i)
        {
            audioSource.clip = musicPlaylist[0];
        }
    }

    public void PlayClickSound()
    {
        buttonClick.Play();
    }

    public void PlayHoverSound()
    {
        buttonHover.Play();
    }
}