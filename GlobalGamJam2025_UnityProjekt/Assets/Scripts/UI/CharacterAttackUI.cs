using Cinemachine;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GetraenkeBub
{

    public class CharacterAttackUI :MonoBehaviour, IUIState
    {
        private AAbility aAbility;
        private GameObject caster;
        private List<GameObject> opfers;

        [SerializeField] private float extraDistance = 3f;


        [SerializeField] private float timeBeforeAttack = 0.5f;
        [SerializeField] private float timeBeforeReaktion = 1f;
        [SerializeField] private float timeNeeded = 2f;

        [Header("Spawn")]

        [SerializeField]private GameObject attackMemeGameobject;
        [SerializeField]private GameObject reaktionGameobject;
        private List<GameObject> spawnedObjects = new List<GameObject>();

        public void Init()
        {
            CameraManager.Instance.attackCamera.gameObject.SetActive(false);
        }

        public void OnBeforeEnter()
        {
        
        }

        public void OnEnter()
        {
            CameraManager.Instance.attackCamera.gameObject.SetActive(true);
        }

        public void OnLeave()
        {
            CameraManager.Instance.attackCamera.gameObject.SetActive(false);
            foreach(GameObject toDestroyObject in spawnedObjects)
            {
                Destroy(toDestroyObject);
            }
        }

        public void HandleAbility(Action done, AAbility ability, GameObject caster, List<GameObject> targets)
        {
            aAbility = ability;
            this.caster = caster;
            this.opfers = targets;
            UIStateManager.Instance.ChangeUIState(EUIState.CharacterAttack);
            Vector3 cameraMovePosition = (targets[0].transform.position - caster.transform.position).normalized * extraDistance 
                + caster.transform.position 
                + caster.transform.right * extraDistance
                + Vector3.up * extraDistance;
            CameraManager.Instance.attackCamera.transform.position = cameraMovePosition;
            CameraManager.Instance.attackCamera.LookAt = targets[0].transform;
            StartCoroutine(InvokeAfterTime(timeBeforeAttack,timeBeforeReaktion,() => done.Invoke()));
        }

        private IEnumerator InvokeAfterTime(float timeBeforeAttack,float timeBeforeReaktion,Action doneAction)
        {
            yield return new WaitForSeconds(timeBeforeAttack);
            HandleBeforeAttack();
            yield return new WaitForSeconds(timeBeforeReaktion);
            HandleReaktion();
            yield return new WaitForSeconds(timeNeeded);
            UIStateManager.Instance.ReturnToLastUiState();
            doneAction?.Invoke();

        }

        private void HandleBeforeAttack()
        {
            if(aAbility.showAttackSprites.Count == 0)
            {
                return;
            }
            GameObject meme = Instantiate(attackMemeGameobject, caster.transform.position, Quaternion.identity);
            spawnedObjects.Add(meme);
            ReaktionHandler reaktionHandler = meme.GetComponent<ReaktionHandler>();
            reaktionHandler.SetImage(aAbility.showAttackSprites[UnityEngine.Random.Range(0, aAbility.showAttackSprites.Count - 1)]);
        }

        private void HandleReaktion()
        {

            if (aAbility.attackReaktions.Count == 0)
            {
                return;
            }
            foreach (GameObject opfa in opfers)
            {
                GameObject meme = Instantiate(reaktionGameobject, opfa.transform.position, Quaternion.identity);
                spawnedObjects.Add(meme);
                ReaktionHandler reaktionHandler = meme.GetComponent<ReaktionHandler>();
                reaktionHandler.SetImage(aAbility.attackReaktions[UnityEngine.Random.Range(0, aAbility.attackReaktions.Count - 1)]);
            }
        }
    }
}
