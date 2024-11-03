using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    [SerializeField]
    private List<AudioClip> musicPlaylist;

    [Header("SFX")]

    [Header("UI")]
    [SerializeField]
    private AudioClip buttonHover;

    [SerializeField]
    private AudioClip buttonClick;

    [Header("Dialogs")]
    [SerializeField]
    private AudioClip dialog;

    private void Awake()
    {
        instance = this;
    }
}