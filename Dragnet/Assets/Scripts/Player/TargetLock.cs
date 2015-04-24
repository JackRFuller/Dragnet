using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetLock : MonoBehaviour {

    public float lockRange;
    [SerializeField] GameObject lockOnObject;
    [SerializeField] List<GameObject> npcList = new List<GameObject>();
    public List<GameObject> npcsInView = new List<GameObject>();
    [SerializeField] GameObject pcTarget;
    public static GameObject staticPCStarget;
    [SerializeField] List<GameObject> npcLocalPositions = new List<GameObject>();

    private int index = 0;
    public int listIndex
    {
        get
        {
            if (index < 0)
                index = npcsInView.Count - 1;

            if (index > npcsInView.Count - 1)
                index = 0;

            return index;
        }
        set
        {
            index = value;
        }
    }

    private bool flickRight = false;
    private bool flickLeft = false;
    private bool stickReset;


	// Use this for initialization
	void Start () 
    {
        npcList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
	}
	
	// Update is called once per frame
	void Update ()
    {

        DetectInput();
        AimAtTarget();
        TargetLock.staticPCStarget = pcTarget;


	}

    private void DetectInput()
    {
        float rStickInput = Input.GetAxis("R_Horizontal");

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextTarget(true);
        }

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
    }

    private void AimAtTarget()
    {
        if (flickRight && stickReset)
        {
            ToRightOfTarget();
            //NextTarget(true);
            stickReset = false;
        }
        else if (flickLeft && stickReset)
        {
            ToLeftOfTarget();
           // NextTarget(false);
            stickReset = false;
        }
    }


    private void NextTarget(bool _increase)
    {
        if (npcsInView.Count > 0)
        {
            SortByDistance();

            if (_increase)
                listIndex++;
            else
                listIndex--;

            
            pcTarget = npcsInView[listIndex].gameObject;
            lockOnObject.transform.position = npcsInView[listIndex].transform.position;
        }
    }

    private void SortByDistance()
    {
        // bubble-sort transforms
        if (npcsInView.Count > 0)
        {
            for (int i = 0; i < npcsInView.Count; i++)
            {
                float sqrMag1 = (npcsInView[i + 0].transform.position - transform.position).sqrMagnitude;

                if ((i + 1) < npcsInView.Count-1)
                {
                    float sqrMag2 = (npcsInView[i + 1].transform.position - transform.position).sqrMagnitude;

                    if (sqrMag2 < sqrMag1)
                    {
                        GameObject tempStore = npcsInView[i];
                        npcsInView[i] = npcsInView[i + 1];
                        npcsInView[i + 1] = tempStore;
                        i = 0;
                    }
                }
            }            
        }
    }

    private void ToLeftOfTarget()
    {
        if (pcTarget == null)
        {
            SortByDistance();
            pcTarget = npcsInView[0].gameObject;
            return;
        }

        npcLocalPositions.Clear();

        for (int i = 0; i < npcsInView.Count; i++)
        {
            if(npcsInView[i] != pcTarget.gameObject)
            {
                Vector3 npcRelative = pcTarget.transform.InverseTransformPoint(npcsInView[i].transform.position);
                //Debug.Log("LEFT:  " + npcsInView[i].name + " Relative to " + pcTarget.name + "    x = " + npcRelative.x);
                if (npcRelative.x < 0)
                {
                    npcLocalPositions.Add(npcsInView[i].gameObject);
                }
            }
        }

        SortListLeft();
    }
    
    private void ToRightOfTarget()
    {
        if (pcTarget == null)
        {
            SortByDistance();
            pcTarget = npcsInView[0].gameObject;
            return;
        }

        npcLocalPositions.Clear();

        for (int i = 0; i < npcsInView.Count; i++)
        {
            if(npcsInView[i] != pcTarget.gameObject)
            {
                Vector3 npcRelative = pcTarget.transform.InverseTransformPoint(npcsInView[i].transform.position);
               // Debug.Log("RIGHT:  " + npcsInView[i].name + " Relative to " + pcTarget.name + "    x = " + npcRelative.x);
                if (npcRelative.x > 0)
                {
                    npcLocalPositions.Add(npcsInView[i].gameObject);
                }
            }
        }

        SortListRight();
    }
    
    private void SortListRight()
    {
        for (int i = 0; i < npcLocalPositions.Count; i++)
        {
            float xDist1 = pcTarget.transform.InverseTransformPoint( npcLocalPositions[i + 0].transform.position ).x;

            if ( (i + 1) < npcLocalPositions.Count)
            {
                float xDist2 = pcTarget.transform.InverseTransformPoint(npcLocalPositions[i + 1].transform.position).x;

                if (xDist2 < xDist1)
                {
                    GameObject tempStore = npcLocalPositions[i];
                    npcLocalPositions[i] = npcLocalPositions[i + 1];
                    npcLocalPositions[i + 1] = tempStore;
                    i = 0;
                }
            }
			
        }

        if (npcLocalPositions.Count > 0)
        {
            pcTarget = npcLocalPositions[0].gameObject;
            lockOnObject.transform.position = pcTarget.transform.position;
        }

    }

    private void SortListLeft()
    {
        for (int i = 0; i < npcLocalPositions.Count; i++)
        {
            float xDist1 = pcTarget.transform.InverseTransformPoint(npcLocalPositions[i + 0].transform.position).x;

            if ((i + 1) < npcLocalPositions.Count)
            {
                float xDist2 = pcTarget.transform.InverseTransformPoint(npcLocalPositions[i + 1].transform.position).x;

                if (xDist2 > xDist1)
                {
                    GameObject tempStore = npcLocalPositions[i];
                    npcLocalPositions[i] = npcLocalPositions[i + 1];
                    npcLocalPositions[i + 1] = tempStore;
                    i = 0;
                }
            }
        }

        if (npcLocalPositions.Count > 0)
        {
            pcTarget = npcLocalPositions[0].gameObject;
            lockOnObject.transform.position = pcTarget.transform.position;
        }

    }

}


   //private void CheckNPCInRange()
   // {
   //     bool _inRange = false;

   //     for (int i = 0; i < npcList.Count; i++)
   //     {
   //         _inRange = npcInRange(npcList[i]);
   //         bool _addNPC = true;

   //         if (_inRange)
   //         {
   //             for (int j = 0; j < npcsInView.Count; j++)
   //             {
   //                 if (npcsInView[j].gameObject == npcList[i].gameObject)
   //                 {
   //                     _addNPC = false;
   //                     break;
   //                 }

   //             }
   //             if (Vector3.Distance(transform.position, npcList[i].transform.position) < lockRange && _addNPC)
   //             {
   //                 npcsInView.Add(npcList[i]);
   //             }
   //         }
   //         else
   //         {
   //             if(npcsInView.Count > 0)
   //                  npcsInView.Remove(npcList[i]);
   //         }
   //     }
   // }

   // private bool npcInRange(GameObject _npc)
   // {
   //     Vector3 dir = (_npc.transform.position - transform.position).normalized;
   //     float direction = Vector3.Dot(dir, transform.forward);

   //     if (direction > 0)
   //         return true;
   //     else
   //         return false;
   // }
