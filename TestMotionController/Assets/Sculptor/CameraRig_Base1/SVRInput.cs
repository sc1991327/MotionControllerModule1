using UnityEngine;
using System.Collections;
using Valve.VR;
using System.Collections.Generic;

public class SButton
{
    public bool Down;
    public bool Hold;
    public bool Up;

    public SButton()
    {
        Down = false;
        Hold = false;
        Up = false;
    }

    public SButton(bool i_down, bool i_hold, bool i_up)
    {
        Down = i_down;
        Hold = i_hold;
        Up = i_up;
    }
}

public class SteamInputState
{
    public SButton Button_L_Trigger;
    public SButton Button_L_Grip;
    public SButton Button_L_Menu;
    public SButton Button_L_Touchpad;
    public SButton Button_R_Trigger;
    public SButton Button_R_Grip;
    public SButton Button_R_Menu;
    public SButton Button_R_Touchpad;

    public SButton Touch_L;
    public SButton Touch_R;

    public Vector2 Axis2D_L_Touch;
    public Vector2 Axis2D_R_Touch;

    public SteamInputState()
    {
        Button_L_Trigger = new SButton();
        Button_L_Grip = new SButton();
        Button_L_Menu = new SButton();
        Button_L_Touchpad = new SButton();
        Button_R_Trigger = new SButton();
        Button_R_Grip = new SButton();
        Button_R_Menu = new SButton();
        Button_R_Touchpad = new SButton();

        Touch_L = new SButton();
        Touch_R = new SButton();

        Axis2D_L_Touch = new Vector2();
        Axis2D_R_Touch = new Vector2();
    }
}

public enum Handle { none, left, right }

public class SVRInput : MonoBehaviour {

    public enum Button
    {
        L_Trigger,
        L_Grip,
        L_Menu,
        L_Touchpad,
        R_Trigger,
        R_Grip,
        R_Menu,
        R_Touchpad
    }

    public enum Touch
    {
        L_Touch,
        R_Touch
    }

    public enum Axis2D
    {
        L_TouchPos,
        R_TouchPos
    }

    static SteamInputState svrinput;

    // cached roles - may or may not be connected
    public GameObject SteamLeftHandController;
    public GameObject SteamRightHandController;

    private SteamVR_TrackedObject leftTrackObj;
    private SteamVR_TrackedObject rightTrackObj;

    List<int> controllerIndices = new List<int>();

    // Use this for initialization
    void Awake()
    {
        leftTrackObj = SteamLeftHandController.GetComponent<SteamVR_TrackedObject>();
        rightTrackObj = SteamRightHandController.GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        svrinput = new SteamInputState();
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
    void Update()
    {
        SteamInputState tempinput = new SteamInputState();

        foreach (var index in controllerIndices)
        {
            var deviceHand = SteamVR_Controller.Input(index);

            if (index == (int)leftTrackObj.index)
            {
                //Debug.Log("Left Index: " + index);
                // leftHand

                tempinput.Button_L_Trigger = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger), deviceHand.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger), deviceHand.GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger));
                tempinput.Button_L_Grip = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_Grip), deviceHand.GetPress(EVRButtonId.k_EButton_Grip), deviceHand.GetPressUp(EVRButtonId.k_EButton_Grip));
                tempinput.Button_L_Menu = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu), deviceHand.GetPress(EVRButtonId.k_EButton_ApplicationMenu), deviceHand.GetPressUp(EVRButtonId.k_EButton_ApplicationMenu));
                tempinput.Button_L_Touchpad = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_Axis0), deviceHand.GetPress(EVRButtonId.k_EButton_Axis0), deviceHand.GetPressUp(EVRButtonId.k_EButton_Axis0));

                tempinput.Touch_L = new SButton(deviceHand.GetTouchDown(EVRButtonId.k_EButton_Axis0), deviceHand.GetTouch(EVRButtonId.k_EButton_Axis0), deviceHand.GetTouchUp(EVRButtonId.k_EButton_Axis0));

                tempinput.Axis2D_L_Touch = deviceHand.GetAxis(EVRButtonId.k_EButton_Axis0);

            }
            else if (index == (int)rightTrackObj.index)
            {
                //Debug.Log("Right Index: " + index);
                // rightHand

                tempinput.Button_R_Trigger = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger), deviceHand.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger), deviceHand.GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger));
                tempinput.Button_R_Grip = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_Grip), deviceHand.GetPress(EVRButtonId.k_EButton_Grip), deviceHand.GetPressUp(EVRButtonId.k_EButton_Grip));
                tempinput.Button_R_Menu = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu), deviceHand.GetPress(EVRButtonId.k_EButton_ApplicationMenu), deviceHand.GetPressUp(EVRButtonId.k_EButton_ApplicationMenu));
                tempinput.Button_R_Touchpad = new SButton(deviceHand.GetPressDown(EVRButtonId.k_EButton_Axis0), deviceHand.GetPress(EVRButtonId.k_EButton_Axis0), deviceHand.GetPressUp(EVRButtonId.k_EButton_Axis0));

                tempinput.Touch_R = new SButton(deviceHand.GetTouchDown(EVRButtonId.k_EButton_Axis0), deviceHand.GetTouch(EVRButtonId.k_EButton_Axis0), deviceHand.GetTouchUp(EVRButtonId.k_EButton_Axis0));

                tempinput.Axis2D_R_Touch = deviceHand.GetAxis(EVRButtonId.k_EButton_Axis0);
            }
            else
            {
                //Debug.Log("Input Index: " + index);
            }
        }

        svrinput = tempinput;
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

    public static bool GetPress(Button virtualButton)
    {
        bool revalue = false;

        switch (virtualButton)
        {
            case Button.L_Grip:
                revalue = svrinput.Button_L_Grip.Hold;
                break;
            case Button.L_Menu:
                revalue = svrinput.Button_L_Menu.Hold;
                break;
            case Button.L_Touchpad:
                revalue = svrinput.Button_L_Touchpad.Hold;
                break;
            case Button.L_Trigger:
                revalue = svrinput.Button_L_Trigger.Hold;
                break;
            case Button.R_Grip:
                revalue = svrinput.Button_R_Grip.Hold;
                break;
            case Button.R_Menu:
                revalue = svrinput.Button_R_Menu.Hold;
                break;
            case Button.R_Touchpad:
                revalue = svrinput.Button_R_Touchpad.Hold;
                break;
            case Button.R_Trigger:
                revalue = svrinput.Button_R_Trigger.Hold;
                break;
        }
        return revalue;
    }

}
