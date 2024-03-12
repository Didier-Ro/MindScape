using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float elapsedTime = 0;
    public static CameraManager instance;
    private Coroutine _panCameraCoroutine;
    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;
    private Vector2 _startingTrackedOffset;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i <_allVirtualCameras.Length; i++)
        {
            //set the current active camera
            _currentCamera = _allVirtualCameras[i];
            //set the framing transposer
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        //set the starting position of the tracked object offset
        _startingTrackedOffset = _framingTransposer.m_TrackedObjectOffset;
    }
    


    #region PanCamera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
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
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedOffset;
        }

        
        while (elapsedTime < panTime * 60)
        {
            elapsedTime += 1 / 60f;
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _framingTransposer.m_TrackedObjectOffset = panLerp;
            yield return null;
        }
        elapsedTime = 0;
    }

    #endregion
}
