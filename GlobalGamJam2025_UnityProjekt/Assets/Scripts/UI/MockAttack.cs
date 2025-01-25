using Game;
using System.Collections.Generic;
using UnityEngine;

namespace GetraenkeBub
{

    public class MockAttack : MonoBehaviour
    {
        [SerializeField] private bool mockAttack;
        [SerializeField] private AAbility aAbility;

        [SerializeField] private GameObject target;
        [SerializeField] private List<GameObject> opfer;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (mockAttack)
            {
                UIStateManager.Instance.HandleAbility(() => Debug.Log("Done"), aAbility,target, opfer);
                mockAttack = false;
            }
        }
    }
}
