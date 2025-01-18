using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public SoundLibary.SFX soundEffect;
    public void _PlaySoundEffect()
    {
        SoundManager.Instance.PlayLibarySound(soundEffect);
    }

    public void _PlaySoundEffectRepeat()
    {
        SoundManager.Instance.PlayContinuousLibarySound(soundEffect);
    }

    public void _PlaySoundEffectRandome()
    {
        SoundManager.Instance.PlayRandomLibarySound(soundEffect);
    }
}
