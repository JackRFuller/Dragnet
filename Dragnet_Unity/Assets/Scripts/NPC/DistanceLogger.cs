using UnityEngine;
using System.Collections;

public class DistanceLogger : MonoBehaviour {

    public static float range;
    [SerializeField] GameObject PC;
    TargetLockV2 targetLock;
    bool added = false;
	// Use this for initialization
	void Start () {

        PC = GameObject.FindGameObjectWithTag("Player");
        targetLock = PC.GetComponent<TargetLockV2>();	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Vector3.Distance(transform.position, PC.transform.position) < range)
        {
            if (!added)
            {
                for (int i = 0; i < targetLock.npcsInView.Count; i++)
                {
                    if (targetLock.npcsInView[i] == gameObject)
                        return;
                }

                bool canAdd = isInRange();

                if (canAdd)
                {
                    added = true;
                    targetLock.npcsInView.Add(gameObject);
                }
            }
            else
            {
                bool canAdd = isInRange();

                if (!canAdd)
                {
                    targetLock.npcsInView.Remove(gameObject);
                    added = false;
                }
            }
        }
        else
        {
            if (added)
            {
                targetLock.npcsInView.Remove(gameObject);
                added = false;
            }
        }     
	}
    
    //in Range
    bool isInRange()
    {
        Vector3 dir = (transform.position - PC.transform.position).normalized;
        float direction = Vector3.Dot(dir, PC.transform.forward);

        if (direction > 0)
            return true;
        else
            return false;
    }
}
