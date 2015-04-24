using UnityEngine;
using System.Collections;

public class ConstructEffect : MonoBehaviour {

    public float range;
    public float speed;
    private Vector3 hiddenPos;
    private Vector3 shownPos;
    private GameObject PC;

	// Use this for initialization
	void Start () {

        PC = GameObject.FindGameObjectWithTag("Player");
        shownPos = transform.position;
        hiddenPos = shownPos;
        hiddenPos.y -= 5f;
        transform.position = hiddenPos;

	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, PC.transform.position) < range)
        {
            //move to shown
            if (transform.position.y != shownPos.y)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position = shownPos;
            }
        }
        else
        {
            //move to hidden

            if (transform.position.y != hiddenPos.y)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
            }
            else
            {
                transform.position = hiddenPos;
            }
        }
	
	}
}
