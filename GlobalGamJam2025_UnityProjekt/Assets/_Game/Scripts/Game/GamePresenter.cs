using Game.Grid;
using Game.Input;
using Game.Unit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private GridPresenter[] levels;
        private List<UnitPresenter> units;
        private AUserInput lastUserInput;

        private IEnumerator gameFlow;

        private void Start()
        {
            gameFlow = PlayLevel(0);
            gameFlow.MoveNext();
        }
        private IEnumerator PlayLevel(int index)
        {
            LoadLevel(index);
            GridPresenter currentLevel = GridPresenter.Instance;
            units = currentLevel.FindGetUnits().OrderByDescending(p => p.GetInitiative()).ToList();

            for(int i = 0; i < currentLevel.GetRoundCount(); i++)
            {
                units.ForEach(p => p.ApplyOverallRoundStart());
                foreach(UnitPresenter unit in units)
                {
                    List<AAbility> abilities = unit.GetAbilityOptions();
                    //check which are possible

                    bool unitIsFinished = false;
                    while(!unitIsFinished)
                    {
                        //check which abilities are possible
                        //give abilities to UI -> Enum: Castable, OutOfEnergy, NoTarget
                        //register callbacks? -> Nein, schon initial subscribed

                        if (unit.GetCurrentMovementPoints() > 0)
                        {
                            HashSet<Vector2Int> movementOptions = unit.GetMovementOptions();
                            //display movement options
                            //subscribe movement events?
                        }

                        yield return null;

                        switch (lastUserInput)
                        {
                            case MovementInput movementInput:
                                //do movement
                                break;
                            case AbilityInput actionInput:
                                //cast

                                break;
                        }
                    }

                    //while movement points > 0 -> show movement points and handle input
                }
            }

            //InitUnits();
            yield return null;
        }
        #region PlayLevel Helper Functions
        private void LoadLevel(int index)
        {
            for(int i=0; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(i == index);
            }
        }
        #endregion

        //Spiel Start
        //Runde Start, iteriere über Runden bis Runden rum sind
        //iteriere über Units
        //option für movement solange MP
        //option für Fähigkeit -> weiter
        //wenn Unit AI Zug bestimmen und machen
        //Communities geben Punkte

    }
}