using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public AudioClip menuSelect;
    public AudioClip menuMove;
    public AudioClip menuBack;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMenuSelect()
    {
        audioSource.PlayOneShot(menuSelect);
    }

    public void PlayMenuMove()
    {
        audioSource.PlayOneShot(menuMove);
    }

    public void PlayMenuBack()
    {
        audioSource.PlayOneShot(menuBack);
    }
}
