using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;
public class ARCamera : MonoBehaviour
{
    public static ARCamera instance;
    bool successSet;
    bool isSet;

    void Awake()
    {
        instance = this;
    }


    void Update()
    {

        if (!isSet && CameraDevice.Instance.GetCameraDirection() == CameraDevice.CameraDirection.CAMERA_BACK)
        {
            successSet = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            if (successSet)
                isSet = true;
            else
            {
                successSet = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
                if (successSet)
                    isSet = true;
                else
                {
                    successSet = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
                    if (successSet)
                        isSet = true;
                }
            }
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            // Set again autofocus mode when app is resumed
            isSet = false;
            successSet = false;
        }
    }

    private void OnApplicationFocus(bool paused)
    {
        if (paused)
        {
            // Set again autofocus mode when app is resumed
            isSet = false;
            successSet = false;
        }
    }

}
