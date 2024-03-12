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
    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;
    
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

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    public IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;
        //handle pan for trigger
        if (!panToStartingPos)
        {
            switch (panDirection)
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

            endPos *= panDistance;
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
        while (elapsedTime < panTime)
        {
            elapsedTime += 1 / 60f;
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            framingTransposer.m_TrackedObjectOffset = panLerp;
            yield return null;
        }
        Debug.Log(elapsedTime);
       
    }

    #endregion
}
