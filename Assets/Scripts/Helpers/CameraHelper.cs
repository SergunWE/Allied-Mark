using UnityEngine;

public static class CameraHelper
{
    private static Camera _camera;

    public static Camera GetPlayerCamera
    {
        get
        {
            _camera ??= Camera.main;
            return _camera;
        }
    }
}