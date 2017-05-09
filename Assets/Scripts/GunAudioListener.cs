using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Sound;
using JLib;
using UnityEngine;
public class GunAudioListener : BaseAudioSourceListener
{
    protected override void RegisterListener()
    {
        GlobalEventQueue.RegisterListener( E_Event.Fire , PlayGunSound );
    }

    public void PlayGunSound(object p)
    {
        int randomIndex = Random.Range(0, audioClips.Count);
        PlayAudio( randomIndex );
    }
}

