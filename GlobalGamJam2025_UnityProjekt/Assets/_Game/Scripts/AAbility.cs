using System;
using UnityEngine;

namespace Game
{
    public abstract class AAbility
    {
        public int actionPointCost;
        //add configuration possibilty for effect
        //add definition for AOE area
        //add configuration possibility for targetType

        /// <summary>
        /// ignoriert Kosten -> �berpr�ft ob im Grid Target im AOE Bereich verf�gbar ist
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTargetConditionSatisfied()
        {
            return true;
        }
        public virtual void Cast(Action callbackCastFinished)
        {
            //TODO hier was �berlegen f�r R�ckgabe

            callbackCastFinished();
        }
    }
}