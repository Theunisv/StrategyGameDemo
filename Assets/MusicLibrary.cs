using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] private AudioClip spellCastSound;
    [SerializeField] private AudioClip spellHitSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip selectionSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip errorSound;
    
    private AudioSource fxAudio;

    public AudioClip SpellCastSound => spellCastSound;
    public AudioClip SpellHitSound => spellHitSound;
    public AudioClip HoverSound => hoverSound;
    public AudioClip SelectionSound => selectionSound;

    private void Start()
    {
        AudioSource fxAudio = gameObject.GetComponent<AudioSource>();
    }

    public void PlayCastSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(spellCastSound);
    }
    public void PlaySpellHitSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(spellHitSound);
    }
    public void PlayHoverSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(hoverSound);
    }
    public void PlaySelectionSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(selectionSound);
    }
    public void PlayMoveSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(moveSound);
    }

    public void PlayErrorSound()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(errorSound);
    }
}
