using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUBWorld_Manager : MonoBehaviour
{

    #region LogIn

    [SerializeField] InputField Username;
    [SerializeField] Text ReturnText;
    

    #endregion

    #region MainHub
    [SerializeField] string[] Weather_Type;
    [SerializeField] Text Time_Date;
    [SerializeField] Text Weather;  

    #endregion

    // Use this for initialization
	void Start () {

        int WeatherType = Random.Range(0, Weather_Type.Length);
        Weather.text = Weather_Type[WeatherType];

        Time_Date.text = System.DateTime.Now.ToString();
	
	}
	
	// Update is called once per frame
	void Update () {

        //UpdateTime
        Time_Date.text = System.DateTime.Now.ToString();

        if (Username.text.Length > 0)
        {
            ReturnText.enabled = true;
        }
        else
        {
            ReturnText.enabled = false;
        }
	
	}
}
