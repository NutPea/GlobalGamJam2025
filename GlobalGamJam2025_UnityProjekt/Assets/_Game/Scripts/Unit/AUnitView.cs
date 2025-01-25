using UnityEngine;
using static Game.Unit.UnitModel;

public abstract class AUnitView : MonoBehaviour
{
    /// <summary>
    /// This is called just initally
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void MaxHPChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called when receiving dmg/healing
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void CurrentHPChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// this is called when a unit moves, TODO define how transform change is applied
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void PositionChanged(Vector2Int oldValue, Vector2Int newValue)
    {

    }

    /// <summary>
    /// this is called when a unit rotates, TODO define how transform change is applied
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void UnitRotationChanged(UnitRotation oldValue, UnitRotation newValue)
    {

    }

    /// <summary>
    /// this is called just initially
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void MaxMovementPointsChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called when turn starts or after moving
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void CurrentMovementPointsChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called at the start of the unit move after (reset to 0) and when being buffed/debuffed
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void MaxMovementPointModifierChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called at the start of the round (when action points increase) and after ability case
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void CurrentActionPointsChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called at the start of the game -> displays basic action point increase per round without modifier
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void ActionPointChangeChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called at the start of unit turn (reset to 0) and when buffed/debuffed
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void ActionPointChangeModifierChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called at the start of the game
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void MaxActionPointsChanged(int oldValue, int newValue)
    {

    }

    /// <summary>
    /// This is called at the start of a round (false) and after a unit moved (true)
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public virtual void IsUsedThisRoundChanged(bool oldValue, bool newValue)
    {

    }
}
