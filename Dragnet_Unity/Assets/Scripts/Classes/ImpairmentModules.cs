using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ImpairmentModules
{
    public void BlastWave(GameObject _caller)
    {
        Vector3 startPos = _caller.transform.position;
        Vector3 targetPos = Vector3.zero;
        int angle = 15;

        int startAng = (int) (-angle);
        int finishAng = (int)(angle);

        int gap = (int)(angle / 8);
        RaycastHit hit;

        for (int i = startAng; i < finishAng; i += gap)
        {
            targetPos = (Quaternion.Euler(0,i,0) * _caller.transform.TransformDirection(Vector3.forward)).normalized * 5f;
   
            if (Physics.Raycast(startPos, targetPos, out hit))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Vector3 forceDir = hit.collider.transform.position - _caller.transform.position;
                    Rigidbody _npcRigibody = hit.collider.GetComponent<Rigidbody>();
                    _npcRigibody.AddForce(forceDir * 100f, ForceMode.Force);
                }
            }

            Debug.DrawRay(startPos, targetPos, Color.green);
        }
    }

}



//CIRCLE DRAWER=======================
    //private List<Vector3> nodes = new List<Vector3>();
    //float curveamount = 45f;
    //float caluAngle;


//public void BlastWaveV2(GameObject _caller)
//{

//    nodes.Clear();
//    caluAngle = 0;

//    for (int i = 0; i < 10 + 1; i++)
//    {
//         float posX = Mathf.Cos( caluAngle * Mathf.Deg2Rad ) * 10f;
//         float posZ = Mathf.Sin( caluAngle * Mathf.Deg2Rad) * 10f;
//         nodes.Add(_caller.transform.position + (_caller.transform.right * posX) + (_caller.transform.forward * posZ));
//         caluAngle += curveamount / (float)(10);
//    }

//    for ( int i  = 0; i < nodes.Count - 1; i ++ )
//    {
//         Debug.DrawLine( nodes[i], nodes[i + 1], Color.red );
//    }
//}