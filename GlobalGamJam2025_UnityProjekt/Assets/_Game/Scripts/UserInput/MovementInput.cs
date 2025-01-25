using UnityEngine;

namespace Game.Input
{
    public class MovementInput : AUserInput
    {
        public Vector2Int position;

        public MovementInput(Vector2Int position)
        {
            this.position = position;
        }
    }
}