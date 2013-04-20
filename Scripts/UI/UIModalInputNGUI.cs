﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Allow InputManager to handle input for NGUI.  Put this component along with ui modal manager. Use in conjuction with UIButtonKeys for each item.
/// </summary>
[AddComponentMenu("M8/UI/ModalInputNGUI")]
public class UIModalInputNGUI : MonoBehaviour {
    public int player = 0;

    public int axisX = InputManager.ActionInvalid;
    public int axisY = InputManager.ActionInvalid;

    public float axisDelay = 0.25f;
    public float axisThreshold = 0.75f;

    public int enter = InputManager.ActionInvalid;
    public int cancel = InputManager.ActionInvalid;

    private UICamera.MouseOrTouch mController = new UICamera.MouseOrTouch();
    private bool mInputActive = false;

    void OnEnable() {
        if(mInputActive) {
            StartCoroutine(AxisCheck());
        }
    }

    void OnDestroy() {
        OnUIModalInactive();
    }

    void OnInputEnter(InputManager.Info data) {
        bool pressed = data.state == InputManager.State.Pressed;
        bool released = data.state == InputManager.State.Released;

        if(pressed || released) {
            UICamera.currentTouchID = -666;
            UICamera.currentTouch = mController;
            UICamera.currentTouch.current = UICamera.selectedObject;
            UICamera.eventHandler.ProcessTouch(pressed, released);
            UICamera.currentTouch.current = null;

            UICamera.currentTouch = null;
        }

        //UICamera.current
        //mController
       /* if(data.state == InputManager.State.Pressed && UICamera.selectedObject != null) {
            UICamera.Notify(UICamera.selectedObject, "OnClick", null);
        }*/
    }

    void OnInputCancel(InputManager.Info data) {
        if(data.state == InputManager.State.Pressed && UICamera.selectedObject != null) {
            UICamera.Notify(UICamera.selectedObject, "OnKey", KeyCode.Escape);
        }
    }

    IEnumerator AxisCheck() {
        float nextTime = 0.0f;

        while(mInputActive) {
            if(UICamera.selectedObject != null) {
                float time = Time.realtimeSinceStartup;
                if(nextTime < time) {
                    InputManager input = Main.instance.input;

                    if(axisX != InputManager.ActionInvalid) {
                        float x = input.GetAxis(player, axisX);
                        if(x < -axisThreshold) {
                            nextTime = time + axisDelay;
                            UICamera.Notify(UICamera.selectedObject, "OnKey", KeyCode.LeftArrow);
                        }
                        else if(x > axisThreshold) {
                            nextTime = time + axisDelay;
                            UICamera.Notify(UICamera.selectedObject, "OnKey", KeyCode.RightArrow);
                        }
                    }

                    if(axisY != InputManager.ActionInvalid) {
                        float y = input.GetAxis(player, axisY);
                        if(y < -axisThreshold) {
                            nextTime = time + axisDelay;
                            UICamera.Notify(UICamera.selectedObject, "OnKey", KeyCode.DownArrow);
                        }
                        else if(y > axisThreshold) {
                            nextTime = time + axisDelay;
                            UICamera.Notify(UICamera.selectedObject, "OnKey", KeyCode.UpArrow);
                        }
                    }
                }
            }

            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    void OnUIModalActive() {
        if(!mInputActive) {
            //bind callbacks
            InputManager input = Main.instance.input;

            if(enter != InputManager.ActionInvalid)
                input.AddButtonCall(player, enter, OnInputEnter);

            if(cancel != InputManager.ActionInvalid)
                input.AddButtonCall(player, cancel, OnInputCancel);

            mInputActive = true;

            if(gameObject.activeInHierarchy) {
                StartCoroutine(AxisCheck());
            }
        }
    }

    void OnUIModalInactive() {
        if(mInputActive) {
            //unbind callbacks
            InputManager input = Main.instance != null ? Main.instance.input : null;

            if(input != null) {
                if(enter != InputManager.ActionInvalid)
                    input.RemoveButtonCall(player, enter, OnInputEnter);

                if(cancel != InputManager.ActionInvalid)
                    input.RemoveButtonCall(player, cancel, OnInputCancel);
            }

            mInputActive = false;
        }
    }
}