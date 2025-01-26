using Cinemachine;
using UnityEngine;

namespace GetraenkeBub
{

    public class CameraManager : MonoBehaviour
    {

        public static CameraManager Instance;
        bool follow;
        [SerializeField] private Transform LookAtPosition;
        public Transform Target;

       public CinemachineVirtualCamera attackCamera;


        private void Awake()
        {
            Instance = this;
            follow = false;
            attackCamera.gameObject.SetActive(false);
        }
       
        public void LookAtPoint(Vector3 lookAtPosition)
        {

            follow = false;
            LookAtPosition.transform.position = lookAtPosition;
        }


        public void SetFollow(Transform followTransform)
        {
            follow = true;
            Target = followTransform;
        }

        public void RemoveFollow()
        {
            follow = false;
        }

        private void Update()
        {
            if (!follow)
            {
                return;
            }
            LookAtPosition.transform.position = Target.transform.position;
        }
    }
}
