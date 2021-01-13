using Cinemachine;
using UnityEngine;

namespace Managers.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        public InputReader inputReader;
        public Camera mainCamera;
        public CinemachineFreeLook freeLookVCam;
        private bool _isRMBPressed;

        [SerializeField, Range(1f, 5f)]
        private float _speed = 2.5f;

        private void OnEnable()
        {
            inputReader.CameraMoveEvent += OnCameraMove;
            inputReader.EnableMouseControlCameraEvent += OnEnableMouseControlCamera;
            inputReader.DisableMouseControlCameraEvent += OnDisableMouseControlCamera;
        }

        private void OnDisable()
        {
            inputReader.CameraMoveEvent -= OnCameraMove;
            inputReader.EnableMouseControlCameraEvent -= OnEnableMouseControlCamera;
            inputReader.DisableMouseControlCameraEvent -= OnDisableMouseControlCamera;
        }

        public void SetupProtagonistVirtualCamera(Transform target)
        {
            freeLookVCam.Follow = target;
            freeLookVCam.LookAt = target;
        }

        private void OnEnableMouseControlCamera()
        {
            _isRMBPressed = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisableMouseControlCamera()
        {
            _isRMBPressed = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // when mouse control is disabled, the input needs to be cleared
            // or the last frame's input will 'stick' until the action is invoked again
            freeLookVCam.m_XAxis.m_InputAxisValue = 0;
            freeLookVCam.m_YAxis.m_InputAxisValue = 0;
        }

        private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (isDeviceMouse && !_isRMBPressed) return;

            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.smoothDeltaTime * _speed;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.smoothDeltaTime * _speed;
        }
    }
}

