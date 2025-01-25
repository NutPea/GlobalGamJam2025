using Game.Grid;
using Game.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Collections;

namespace Game
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private GridPresenter[] levels;
        private List<UnitPresenter> units;

        private IEnumerator gameFlow;

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
                    //if input is ability cast -> continue
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