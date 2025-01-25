using UnityEngine;

namespace GetraenkeBub
{

    public class LookAtMainCamera : MonoBehaviour
    {
        private Transform lookAtCameraTransform;

        private void Awake()
        {
            lookAtCameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.forward = -lookAtCameraTransform.forward;
        }
    }
}
