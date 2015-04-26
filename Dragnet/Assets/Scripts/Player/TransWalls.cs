using UnityEngine;
using System.Collections;

public class TransWalls : MonoBehaviour {

    public Material transWall;
    public int alphaValue;
    private Material storeWallMat;
    private GameObject storeWallHit;
    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () 
    {
        RaycastToPlayer();
	}

    private void RaycastToPlayer()
    {
        RaycastHit hit;
        Vector3 rayDir = Player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, rayDir, out hit))
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                if (hit.collider.gameObject != storeWallHit)
                {
                    storeWallHit = hit.collider.gameObject;
                    MeshRenderer wallMesh = hit.collider.gameObject.GetComponent<MeshRenderer>();

                    if (wallMesh.material != transWall)
                    {
                        storeWallMat = wallMesh.material;
                        storeWallHit = hit.collider.gameObject;

                        Color currWallColor = storeWallMat.color;
                        currWallColor.a = alphaValue;
                        transWall.color = currWallColor;
                        wallMesh.material = transWall;
                    }
                }
            }
            else
            {
                if (storeWallHit == null)
                    return;

                storeWallHit.GetComponent<MeshRenderer>().material = storeWallMat;
                storeWallHit = null;
                storeWallMat = null;
            }
        }
    }
}
