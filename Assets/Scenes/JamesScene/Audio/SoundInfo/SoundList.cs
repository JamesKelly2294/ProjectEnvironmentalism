using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundList : MonoBehaviour
{
    public SoundInfo[] sounds;

    AudioManager _am;

    public void Awake()
    {
        _am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        foreach(var soundInfo in sounds)
        {
            Debug.Log("Registering " + soundInfo.id);
            _am.RegisterSoundInfo(soundInfo);
        }
    }
}
