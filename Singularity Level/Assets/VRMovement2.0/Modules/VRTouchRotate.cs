using UnityEngine;
using System.Collections;
using DG.Tweening;

public class VRTouchRotate : MonoBehaviour {

    public bool canRotate = true;
    public enum eRotationMode { ButtonPointAndShoot, QuickStick, SlowStick,ButtonRotateLR, None };
    [Header("-Rotate Modes-")]
    public eRotationMode RotationMode = eRotationMode.QuickStick;
    [Header("-Rotate Controls-")]
    public OVRInput.Button RotateButton;
    [Header("-Rotate Settings-")]
    public bool fadeRotate = false;
    [Range(0,180)]
    public float rotateDegrees = 45;
    public float rotateTime = .2f;        //ButtonRotation
    public float slowRotateSpeed = .7f;   //Speed for Stick Rotate
    VRTouchMove2 refSystem;
    void Start()
    {
        refSystem = GetComponent<VRTouchMove2>();
    }

void Update()
    {
        if(canRotate)
        {
            if (RotationMode == eRotationMode.ButtonPointAndShoot)
            {
                PointShootPressed();
            }
            if (RotationMode == eRotationMode.QuickStick)
            {
                QuickStickRotate();
            }
            if (RotationMode == eRotationMode.SlowStick)
            {
                SlowStickRotate();
            }
            if(RotationMode == eRotationMode.ButtonRotateLR)
            {
                ButtonRotateLR();
            }
        }
    }


    void ButtonRotateLR()
    {
        if (OVRInput.GetDown(RotateButton, OVRInput.Controller.LTouch))
        {
            RotateByDegrees(-rotateDegrees);
        }
        if (OVRInput.GetDown(RotateButton, OVRInput.Controller.RTouch))
        {
            RotateByDegrees(rotateDegrees);
        }
    }

    //Slowly Rotates the Charactor Controller
    void SlowStickRotate()
    {
        float h = 0;
        switch (refSystem.ControlsOn)
        {
            case VRTouchMove2.eControllerType.Left:
                h = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).x;
                break;
            case VRTouchMove2.eControllerType.Right:
                h = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
                break;
            case VRTouchMove2.eControllerType.Both:
                h = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch).x;
                h += OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
                h = Mathf.Clamp(h, -1, 1);
                break;

        }
        if (Mathf.Abs(h) > .1f)
        {
            RotateByDegrees(h * slowRotateSpeed);
        }
    }
    //Quick 45 Degree Rotations for the Charactor Controller
    void QuickStickRotate()
    {
        switch (refSystem.ControlsOn)
        {
            case VRTouchMove2.eControllerType.Left:
                if (OVRInput.GetDown(OVRInput.Button.Left, OVRInput.Controller.LTouch))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (OVRInput.GetDown(OVRInput.Button.Right, OVRInput.Controller.LTouch))
                {
                    RotateByDegrees(rotateDegrees);
                }
                break;
            case VRTouchMove2.eControllerType.Right:
                if (OVRInput.GetDown(OVRInput.Button.Left, OVRInput.Controller.RTouch))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (OVRInput.GetDown(OVRInput.Button.Right, OVRInput.Controller.RTouch))
                {
                    RotateByDegrees(rotateDegrees);
                }
                break;
            case VRTouchMove2.eControllerType.Both:
                if (OVRInput.GetDown(OVRInput.Button.Left, OVRInput.Controller.LTouch))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (OVRInput.GetDown(OVRInput.Button.Right, OVRInput.Controller.LTouch))
                {
                    RotateByDegrees(rotateDegrees);
                }
                if (OVRInput.GetDown(OVRInput.Button.Left, OVRInput.Controller.RTouch))
                {
                    RotateByDegrees(-rotateDegrees);
                }
                if (OVRInput.GetDown(OVRInput.Button.Right, OVRInput.Controller.RTouch))
                {
                    RotateByDegrees(rotateDegrees);
                }
                break;

        }
    }
    //Point Shoot Pressed Event
    void PointShootPressed()
    {
        VRTouchMove2.InputData InputHolder = refSystem.InputReturnDown(RotateButton);
       if(InputHolder.isRight == true)
        {
            PointAndShootRotation(refSystem.rightController);
        }
        if (InputHolder.isLeft == true)
        {
            PointAndShootRotation(refSystem.leftController);
        }
    }

    /// <summary>
    /// Point and Shoot Rotation ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    ///  Changes the Rotation based on where the Controller is pointing and where you are looking only only in one direction. This allows someone to quickly look behind them.
    /// </summary>

    void PointAndShootRotation(Transform selectedController)
    {
            if (fadeRotate)
            {
                refSystem.myFade.StartFadeIn(refSystem.fadeTime);
            }
            //Get Position in front of Object
            Vector3 holder = new Ray(selectedController.position, selectedController.forward).GetPoint(1);
            //Get Rotational Direction
            Vector3 lookPos = holder - refSystem.headRig.transform.position;
            //Remove Y Component
            lookPos.y = 0;
            //Transform rotation
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            //Get Rotational Direction
            Vector3 rotPosition = rotation.eulerAngles - refSystem.transform.localRotation.eulerAngles;
            //Flatten Rotational Return
            rotPosition.x = 0;
            rotPosition.z = 0;
            //Apply Rotation
            refSystem.yourRig.transform.DORotate(rotPosition, rotateTime);
    }

    //Helper Function To rotate by set Degrees
    public void RotateByDegrees(float degrees)
    {
        RotateByDegrees2(degrees);
        //if (refSystem.myFade && fadeRotate)
        //{
        //    refSystem.myFade.StartFadeIn(refSystem.fadeTime);
        //}
        //Vector3 holder1;
        //holder1 = refSystem.yourRig.transform.rotation.eulerAngles;
        //holder1.y += degrees;
        //Vector3 rotPosition = holder1;
        //refSystem.yourRig.transform.DORotate(rotPosition, rotateTime);
    }

    void RotateByDegrees2(float degrees)
    {
        if (refSystem.myFade && fadeRotate)
        {
            refSystem.myFade.StartFadeIn(refSystem.fadeTime);
        }
        //Transform mc =//LogsUtil.getMainCamera();
        Vector3 cameraPos = transform.TransformPoint(refSystem.headRig.position);
        Vector3 holder1;
        holder1 = refSystem.yourRig.transform.rotation.eulerAngles;
        holder1.y += degrees;
        Vector3 rotPosition = holder1;
        refSystem.yourRig.transform.DORotate(holder1, rotateTime);
        //refSystem.yourRig.transform.eulerAngles = holder1;
        Vector3 newCameraPos = transform.TransformPoint((refSystem.headRig.position));
        Vector3 cameraDifference = newCameraPos - cameraPos;
        refSystem.yourRig.transform.position = refSystem.yourRig.transform.position - cameraDifference;
        //Debug.Log("now cameraPos=" + cameraPos);
    }
}

