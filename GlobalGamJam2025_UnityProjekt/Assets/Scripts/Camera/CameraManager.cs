using UnityEngine;

namespace GetraenkeBub
{

    public class CameraManager : MonoBehaviour
    {

        public static CameraManager Instance;
        bool follow;
        [SerializeField] private Transform LookAtPosition;
        private Transform target;


        private void Awake()
        {
            Instance = this;
            follow = false;
        }
       
        public void LookAtPoint(Vector3 lookAtPosition)
        {

            follow = false;
            LookAtPosition.transform.position = lookAtPosition;
        }


        public void SetFollow(Transform followTransform)
        {
            follow = true;
            target = followTransform;
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
            LookAtPosition.transform.position = target.transform.position;
        }
    }
}
