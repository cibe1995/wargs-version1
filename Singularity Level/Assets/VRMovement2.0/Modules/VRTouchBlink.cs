using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VRTouchBlink : MonoBehaviour
{
    [Header("-Blink Controls-")]
    public OVRInput.Button BlinkButton;       //Blink Foward
    public enum eBlinkMode { Instant, HoldRelease };
    public bool canBlink = true;
    [Header("-Blink Settings-")]
    public bool fadeBlink;
    public eBlinkMode BlinkMode;
    public float blinkDistance = 10;          //Max Blink Distance
    public float blinkMoveTime = .4f;            // Blink and Teleport Speed, 0 Is Instant
    [Header("-Required HookUps-")]
    public Transform blinkPoint;          //Your Teleport Object




    VRTouchMove2 refSystem;
    //   // Use this for initialization
    bool inBlink;
    Transform storedTransformBlink;

    // Use this for initialization
    void Start()
    {
        refSystem = GetComponent<VRTouchMove2>();
        if (!refSystem)
        {
            Debug.Log("VRTouchRotate is not on VRMove object disabling");
            this.enabled = false;
            return;
        }
    }
    void Update()
    {
        if (canBlink)
        {
            BlinkInput();
        }
    }

    public void BlinkInput()
    {
        if (BlinkMode == eBlinkMode.Instant)
        {
            FowardBlinkNormal();
        }
        if (BlinkMode == eBlinkMode.HoldRelease)
        {
            FowardBlinkHold();
        }
    }

    /// <summary>
    /// Foward Blink System  ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    ///     Blink Foward Set Direction.
    /// </summary>
    /// 
    public void FowardBlinkNormal()
    {
        VRTouchMove2.InputData InputHolderDown = refSystem.InputReturnDown(BlinkButton);
        if (InputHolderDown.pressed)
        {
            //Cast Ray Foward
            Ray ray = new Ray(InputHolderDown.selectedController.position, InputHolderDown.selectedController.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, blinkDistance))
            {
                //Blink to Point
                Vector3 holder = ray.GetPoint(hit.distance - (refSystem.yourRig.radius * 2));
                holder.y += refSystem.GetHeight();
                refSystem.yourRig.transform.DOMove(holder, blinkMoveTime);
                Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            }
            else
            {
                //Blink to Max Distance
                Vector3 holder = ray.GetPoint(blinkDistance);
                holder.y += refSystem.GetHeight();
                refSystem.yourRig.transform.DOMove(holder, blinkMoveTime);
                Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            }
        }
    }

    void BumpMe()
    {
        refSystem.yourRig.Move(Vector3.up * .01f);
    }

    public void FowardBlinkHold()
    {
        VRTouchMove2.InputData InputHolderDown = refSystem.InputReturnDown(BlinkButton);
        VRTouchMove2.InputData InputHolderUp = refSystem.InputReturnUp(BlinkButton);
        if (InputHolderDown.pressed && !inBlink)
        {
            inBlink = true;
            storedTransformBlink = InputHolderDown.selectedController;
        }
        if (inBlink)
        {
            Ray ray = new Ray(storedTransformBlink.position, storedTransformBlink.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, blinkDistance))
            {
                blinkPoint.transform.DOMove(ray.GetPoint((hit.distance-(refSystem.yourRig.radius*2))), .05f);
                blinkPoint.gameObject.SetActive(true);
                ////Blink to Point
            }
            else
            {
                blinkPoint.transform.DOMove(ray.GetPoint(blinkDistance), .05f);
                blinkPoint.gameObject.SetActive(true);
                ////Blink to Max Distance
            }
        }
        if (InputHolderUp.pressed)
        {
            inBlink = false;
            blinkPoint.gameObject.SetActive(false);
            //Cast Ray Foward
            Vector3 holder = blinkPoint.position;
            holder.y += refSystem.GetHeight();
            refSystem.yourRig.transform.DOMove(holder, blinkMoveTime);
            Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            //Ray ray = new Ray(storedTransformBlink.position, storedTransformBlink.forward);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, blinkDistance))
            //{
            //    //Blink to Point
            //    refSystem.yourRig.transform.DOMove(hit.point, blinkMoveTime);
            //    Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
            //}
            //else
            //{
            //    //Blink to Max Distance
            //    refSystem.yourRig.transform.DOMove(ray.GetPoint(blinkDistance - .2f), blinkMoveTime);
            //    Invoke("BumpMe", blinkMoveTime + Time.deltaTime);
        //}
        }
    }
}
