using Game;
using GetraenkeBub;
using UnityEngine;
using System;

public class SpamMail : AAbility
{
    override public void Cast(Action callbackCastFinished)
    {
        Instantiate(spawnedEffect, transform.parent.transform);



        if (UIStateManager.Instance == null)
        {
            callbackCastFinished();
        }
        else
        {
            UIStateManager.Instance.HandleAbility(() => callbackCastFinished());
        }
    }
}
