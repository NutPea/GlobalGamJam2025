using UnityEngine;

namespace Game
{
    public class GameModel : MonoBehaviour
    {
        public enum AbilityUsability { Castable, NotEnoughEnergy, TargetNotAvailable}

        public ModelEntry<int>Points = new();


    }
}