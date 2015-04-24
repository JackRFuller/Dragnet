﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TargetLockV2 : MonoBehaviour {

    public List<GameObject> npcsInView = new List<GameObject>();
    public List<GameObject> npcLocalPositions = new List<GameObject>();
    public static GameObject Target;
    [SerializeField] GameObject lockOnObj;


    private bool flickRight = false;
    private bool flickLeft = false;
    private bool stickReset;

    public enum LockSystem
    {
        Auto,
        Select
    }

    public LockSystem LockSelection;


    void Update()
    {
        if (npcsInView.Count > 0)
        {
            Target = npcsInView[0];

            if (LockSelection == LockSystem.Select)
                ManageInput();
            else
                npcsInView.Sort(SortListByDistance);

            if (lockOnObj != null)
                lockOnObj.transform.position = Target.transform.position;
        }
        else
        {
            Target = null;

            if (lockOnObj != null)
                lockOnObj.transform.position = new Vector3(1000f, 1000f, 1000f);
        }
    }

    private void ManageInput()
    {
        float rStickInput = Input.GetAxis("R_Horizontal");

        if (rStickInput > -0.1f && rStickInput < 0.1f)
        {
            flickRight = false;
            flickLeft = false;
            stickReset = true;
        }
        if (rStickInput > 0.3f && !flickRight)
        {
            flickRight = true;
            flickLeft = false;
        }
        if (rStickInput < -0.3f && !flickLeft)
        {
            flickLeft = true;
            flickRight = false;
        }

        AimAtTarget();
    }

    private void AimAtTarget()
    {
        if (flickRight && stickReset)
        {
            SortRight();
            //NextTarget(true);
            stickReset = false;
        }
        else if (flickLeft && stickReset)
        {
            SortLeft();
            // NextTarget(false);
            stickReset = false;
        }
    }

    private void SortRight()
    {
        npcsInView.Sort(SortToRight);
    }

    private void SortLeft()
    {
        npcsInView.Sort(SortToLeft);
    }

    int SortListByDistance(GameObject obj1, GameObject obj2)
    {
        if (obj1 != null && obj2 != null)
        {
            float obj1Distance = Vector3.Distance(obj1.transform.position, transform.position);
            float obj2Distance = Vector3.Distance(obj2.transform.position, transform.position);

            if (obj1Distance > obj2Distance)
                return 1;
            else if (obj2Distance > obj1Distance)
                return -1;
            else
                return 0;
        }
        else
            return 0;
    }


    int SortToRight(GameObject obj1, GameObject obj2)
    {
        if (obj1 != null && obj2 != null)
        {
            float xDist1 = TargetLockV2.Target.transform.InverseTransformPoint(obj1.transform.position).x;
            float xDist2 = TargetLockV2.Target.transform.InverseTransformPoint(obj2.transform.position).x;

            if (xDist1 > xDist2)
                return 1;
            else if (xDist1 < xDist2)
                return -1;
            else
                return 0;

        }
        else
            return 0;
    }

    int SortToLeft(GameObject obj1, GameObject obj2)
    {
        if (obj1 != null && obj2 != null)
        {
            float xDist1 = TargetLockV2.Target.transform.InverseTransformPoint(obj1.transform.position).x;
            float xDist2 = TargetLockV2.Target.transform.InverseTransformPoint(obj2.transform.position).x;

            if (xDist1 < xDist2)
                return 1;
            else if (xDist1 > xDist2)
                return -1;
            else
                return 0;

        }
        else
            return 0;
    }
}
