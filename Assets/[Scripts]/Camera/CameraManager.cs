using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private Vector2 _startingTrackedOffset;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;
    private Coroutine panCameraCoroutine;
    public static CameraManager instance;
    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras = default;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i <allVirtualCameras.Length; i++)
        {
            //set the current active camera
            currentCamera = allVirtualCameras[i];
            //set the framing transposer
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        //set the starting position of the tracked object offset
        _startingTrackedOffset = framingTransposer.m_TrackedObjectOffset;
    }
    
    #region PanCamera

    public void PanCameraOnContact(float _panDistance, float _panTime, PanDirection _panDirection, bool _panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(_panDistance, _panTime, _panDirection, _panToStartingPos));
    }

    public IEnumerator PanCamera(float _panDistance, float _panTime, PanDirection _panDirection, bool _panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;
        //handle pan for trigger
        if (!_panToStartingPos)
        {
            switch (_panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;
            }

            endPos *= _panDistance;
            startingPos = _startingTrackedOffset;
            endPos += startingPos;
        }
        //handle the pan back to startng position
        else
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedOffset;
        }

        float elapsedTime = 0;
        while (elapsedTime < _panTime)
        {
            elapsedTime += 1 / 60f;
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / _panTime));
            framingTransposer.m_TrackedObjectOffset = panLerp;
            yield return null;
        }
    }

    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineVirtualCamera _cameraFromLeft, CinemachineVirtualCamera _cameraFromRight, Vector2 _triggerExitDirection)
    {
        Debug.Log(_triggerExitDirection);
        Debug.Log(_cameraFromLeft);
        Debug.Log(currentCamera + "camara actual");
        //if the current Camera is the camera on the left and our trigger exit direction was on the right
        if (currentCamera == _cameraFromLeft && _triggerExitDirection.x > 0f)
        {
            Debug.Log("cambio de camara a derecha");
            //activate the new camera
            _cameraFromRight.enabled = true;
            //deactivate the other camera
            _cameraFromLeft.enabled = false;
            //set the new camera at the current camera
            currentCamera = _cameraFromRight;
            //update our composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else if(currentCamera == _cameraFromRight && _triggerExitDirection.x < 0f)
        {
            Debug.Log("cambio de camara a izquierda");
            _cameraFromLeft.enabled = true;
            //deactivate the other camera
            _cameraFromRight.enabled = false;
            //set the new camera at the current camera
            currentCamera = _cameraFromLeft;
            //update our composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    #endregion
}
