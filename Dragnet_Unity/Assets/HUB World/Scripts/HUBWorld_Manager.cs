using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUBWorld_Manager : MonoBehaviour
{

	public enum MenuMode
	{
		Login,
		MainMenu,
	};

	public MenuMode CurrentMenuMode;

    #region LogIn

	[Header("Login Variables")]
	[SerializeField] bool UsernameGenerated = false;
	[SerializeField] Text Instructions;
    [SerializeField] InputField Username;
	[SerializeField] string User_Name;
    [SerializeField] Text ReturnText;
	[SerializeField] string Password;
	[SerializeField] Text WelcomeMessage;
    

    #endregion

    #region MainHub
    [Header("Main Menu Variables")]
    bool buttonPressed = false;
	[SerializeField] int Button_ID = 0;
	[SerializeField] GameObject[] Buttons;

    [SerializeField] string[] Weather_Type;
    [SerializeField] Text Time_Date;
    [SerializeField] Text Weather;  

    #endregion

    // Use this for initialization
	void Start () {		

        Buttons[Button_ID].GetComponent<Animation>().Play("BigButton_Grow");
        Buttons[Button_ID].transform.FindChild("Highlight").GetComponent<Image>().enabled = true;

        int WeatherType = Random.Range(0, Weather_Type.Length);
        Weather.text = Weather_Type[WeatherType];

        Time_Date.text = System.DateTime.Now.ToString();
	
	}
	
	// Update is called once per frame
	void Update () {

        //UpdateTime
        Time_Date.text = System.DateTime.Now.ToString();

		if(CurrentMenuMode == MenuMode.Login)
		{
			Login();
		}

		if(CurrentMenuMode == MenuMode.MainMenu)
		{
			MainMenu();
		}
	}

	#region MainMenu

	void MainMenu()
	{

        if (!buttonPressed)
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                MainMenu_Right();
            }

            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                MainMenu_Left();
            }

            if (Input.GetButtonUp("Submit") || Input.GetButtonUp("A"))
            {
                DetermineMenu();
            }
        }        
	}

    void DetermineMenu()
    {
        switch (Button_ID)
        {
            case 0:
                Debug.Log("Gallery");
                break;
            case 1:
                Debug.Log("Message Board");
                break;
            case 2:
                Debug.Log("Abilties");
                break;
            case 3:
                Debug.Log("Options");
                break;
            case 4:
                Debug.Log("Logout");
                break;
        }
    }

    void MainMenu_Right()
    {
        Buttons[Button_ID].GetComponent<Animation>().Play("BigButton_Shrink");
        Buttons[Button_ID].transform.FindChild("Highlight").GetComponent<Image>().enabled = false;  

        Button_ID++;
        if (Button_ID == Buttons.Length)
        {
            Button_ID = 0;
        }

        MainMenuChangeButton();
    }

    void MainMenu_Left()
    {
        Buttons[Button_ID].GetComponent<Animation>().Play("BigButton_Shrink");
        Buttons[Button_ID].transform.FindChild("Highlight").GetComponent<Image>().enabled = false;

        Button_ID--;
        if (Button_ID == -1)
        {
            Button_ID = Buttons.Length -1;
        }

        MainMenuChangeButton();
    }



    void MainMenuChangeButton()
    {                   

        Buttons[Button_ID].GetComponent<Animation>().Play("BigButton_Grow");
        Buttons[Button_ID].transform.FindChild("Highlight").GetComponent<Image>().enabled = true;

        buttonPressed = true;
        StartCoroutine(PauseBetweenPress());
            
    }

    IEnumerator PauseBetweenPress()
    {
        yield return new WaitForSeconds(0.1F);
        buttonPressed = false;
    }

	#endregion

	#region LoginProcedure

	void Login()
	{
		if(!UsernameGenerated)
		{
			if (Username.text.Length > 0)
			{
				ReturnText.enabled = true;
				
				if(Input.GetButtonUp("Submit"))
				{
					User_Name = Username.text;
					LogInProcedure();
				}
			}
			else
			{
				ReturnText.enabled = false;
			}
		}
	}

	void LogInProcedure()
	{
		UsernameGenerated = true;
		Instructions.text = "Generating Automated Password";
		StartCoroutine (FlashLoginText ());
		StartCoroutine (TypeInPassWord ());
		ReturnText.enabled = false;
	}

	IEnumerator TypeInPassWord()
	{
		Username.text = "";
		foreach(char letter in Password.ToCharArray())
		{
			Username.text += letter;
			yield return 0;
			yield return new WaitForSeconds(0.1F);
		}
	}

	IEnumerator FlashLoginText()
	{

		for(int i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds (0.75F);
			switch(Instructions.enabled)
			{
				case true:
				Instructions.enabled = false;
				break;

				case false:
				Instructions.enabled = true;
				break;
			}
		}

		Instructions.text = "Password Successfully Generated";
		StartCoroutine (LoadMainMenu ());

	}

	IEnumerator LoadMainMenu()
	{
		yield return new WaitForSeconds(0.75F);
		Instructions.enabled = false;
		Username.gameObject.SetActive (false);
		WelcomeMessage.text += " " + User_Name;
		WelcomeMessage.enabled = true;

		yield return new WaitForSeconds (1.5F);
		
		

		GameObject.Find ("LogInObjects").GetComponent<Animation> ().Play ();
		CurrentMenuMode = MenuMode.MainMenu;
	}

	#endregion
}
