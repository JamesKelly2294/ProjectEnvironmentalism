using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField]
    private string ButtonHighlightSoundID;
    [SerializeField]
    private string ButtonPressedSoundID;

    private AudioManager _am;

    public void Start()
    {
        var amGO = GameObject.FindGameObjectWithTag("AudioManager");
        if(amGO)
        {
            _am = amGO.GetComponent<AudioManager>(); 
        }
    }

    public void PlayButtonHighlightedSound()
    {
        if (_am)
        {
            _am.Play(ButtonHighlightSoundID);
        }
    }

    public void PlayButtonPressedSound()
    {
        if(_am)
        {
            _am.Play(ButtonPressedSoundID);
        }
    }
}
