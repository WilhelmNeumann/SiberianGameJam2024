using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    [SerializeField]
    private AudioSource musicPlaylist;

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