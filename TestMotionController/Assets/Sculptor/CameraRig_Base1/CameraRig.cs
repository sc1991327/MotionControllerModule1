using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using System.Collections.Generic;
using Valve.VR;

public enum CameraRigPlatform
{
    None,
    SteamVR,
    OculusVR,
}

public class CameraRig : MonoBehaviour {

    public GameObject OculusCamera;
    public GameObject SteamCamera;

    public static CameraRigPlatform cameraRigPlatform = CameraRigPlatform.None;

    List<int> controllerIndices = new List<int>();

    // Use this for initialization
    void Awake () {

        // Here Supported Unity 5.3 and need to rewrite for unity 5.4
        UnityEngine.VR.VRSettings.enabled = true;
        if (UnityEngine.VR.VRDevice.isPresent && UnityEngine.VR.VRSettings.loadedDevice == UnityEngine.VR.VRDeviceType.Oculus)
        {
            cameraRigPlatform = CameraRigPlatform.OculusVR;
            SteamVR.enabled = false;//disable steam vr
            SteamCamera.SetActive(false);
        }
        else
        {
            if (UnityEngine.VR.VRDevice.isPresent)
            {
                UnityEngine.VR.VRSettings.loadedDevice = UnityEngine.VR.VRDeviceType.None;
                cameraRigPlatform = CameraRigPlatform.None;
            }
            else
            {
                UnityEngine.VR.VRSettings.enabled = false;
                SteamVR.enabled = true;
                if (SteamVR.enabled)
                {
                    cameraRigPlatform = CameraRigPlatform.SteamVR;
                }
                OculusCamera.SetActive(false);
            }

        }

    }

    private void OnDeviceConnected(params object[] args)
    {
        SteamDeviceConnected(args);
    }

    void OnEnable()
    {
        SteamVR_Utils.Event.Listen("device_connected", OnDeviceConnected);
    }

    void OnDisable()
    {
        SteamVR_Utils.Event.Remove("device_connected", OnDeviceConnected);
    }

    // Update is called once per frame
    void Update () {

    }

    private void SteamDeviceConnected(params object[] args)
    {
        var index = (int)args[0];

        var system = OpenVR.System;
        if (system == null || system.GetTrackedDeviceClass((uint)index) != ETrackedDeviceClass.Controller)
            return;

        var connected = (bool)args[1];
        if (connected)
        {
            Debug.Log(string.Format("Controller {0} connected.", index));
            controllerIndices.Add(index);
        }
        else
        {
            Debug.Log(string.Format("Controller {0} disconnected.", index));
            controllerIndices.Remove(index);
        }
    }

}
