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
        /// ignoriert Kosten -> überprüft ob im Grid Target im AOE Bereich verfügbar ist
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTargetConditionSatisfied()
        {
            return true;
        }
        public virtual void Cast(Action callbackCastFinished)
        {
            //TODO hier was überlegen für Rückgabe

            callbackCastFinished();
        }
    }
}