#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{

    [SerializeField] Camera mainCamera;
    [SerializeField] Camera minimapCamera;
    [SerializeField] CameraShake shakeCamera;

    public void SetMainCamera(PacMan parent)
    {
        mainCamera.transform.parent = parent.transform;
        Vector3 position = mainCamera.transform.localPosition;
        position.x = 0f;
        position.y = 0f;
        mainCamera.transform.localPosition = position;
    }

    public void SetMinimapCamera(Stage currentStage)
    {
        minimapCamera.transform.position = currentStage.MinimapCameraPosition;
        minimapCamera.orthographicSize = currentStage.MinimapCameraSize;
    }

    public void CameraShaking(float time)
    {
        shakeCamera.StartShaking(time);
    }

}
