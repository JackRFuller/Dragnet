using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUBWorld_Manager : MonoBehaviour
{

	public enum MenuMode
	{
		Login,
		MainMenu,
		LevelSelect,
        Abilities,
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
	
	#region LevelSelect
	
	[Header("Level Select Variables")]
	[SerializeField] GameObject LevelSelectItems;
	
	
	#endregion

    #region Abilities
    [Header("Ability Menu Variables")]
    [SerializeField] bool WedgeActive;
    [SerializeField] GameObject AbilityMenuItems;
    [SerializeField] int AbilityWedgeID;
    [SerializeField] GameObject[] AbilityWedge;

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
		
		if(CurrentMenuMode == MenuMode.LevelSelect)
		{
			LevelSelect();
		}

        if (CurrentMenuMode == MenuMode.Abilities)
        {
            ManageAbilityMenu();
        }
	}

    #region Abilties

    void ManageAbilityMenu()
    {
        if (WedgeActive)
        {
            if (Input.GetButtonUp("B") || Input.GetButtonUp("Fire1"))
            {
                DeactivateWedge();
            }
        }

        if (Input.GetButtonUp("A") || Input.GetButtonUp("Submit"))
        {
            if (!WedgeActive)
            {
                ActivateWedge();
                WedgeActive = true;
            }
        }

        if (!WedgeActive)
        {
            if (Input.GetButtonUp("B") || Input.GetButtonUp("Fire1"))
            {
                AbilityMenuItems.GetComponent<Animation>().Play("AbilityOut");
                CurrentMenuMode = MenuMode.MainMenu;
            }

            if (!buttonPressed)
            {
                if (Input.GetAxis("Horizontal") > 0.1)
                {
                    int previousID = AbilityWedgeID;
                    AbilityWedgeID += 1;
                    if (AbilityWedgeID == AbilityWedge.Length)
                    {
                        AbilityWedgeID = 0;
                    }
                    MoveToSelector(previousID);
                    buttonPressed = true;
                    StartCoroutine(PauseBetweenPress());
                }

                if (Input.GetAxis("Horizontal") < -0.1)
                {
                    int previousID = AbilityWedgeID;
                    AbilityWedgeID -= 1;
                    if (AbilityWedgeID == -1)
                    {
                        AbilityWedgeID = AbilityWedge.Length - 1;
                    }
                    MoveToSelector(previousID);
                    buttonPressed = true;
                    StartCoroutine(PauseBetweenPress());
                }
            }        
        }       
    }

    void DeactivateWedge()
    {

    }

    void ActivateWedge()
    {
        
    }

    void MoveToSelector(int PreviousID)
    {
        AbilityWedge[PreviousID].transform.FindChild("Selector").GetComponent<Image>().enabled = false;
        AbilityWedge[AbilityWedgeID].transform.FindChild("Selector").GetComponent<Image>().enabled = true;
    }

    


    #endregion

    #region LevelSelect

    void LevelSelect()
	{
		if(Input.GetButtonUp("B") || Input.GetButtonUp("Fire1"))
		{	
			LevelSelectItems.GetComponent<Animation>().Play("LevelSelectOut");
			CurrentMenuMode = MenuMode.MainMenu;
		}	
	}
	
	
	#endregion

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
				LevelSelectItems.GetComponent<Animation>().Play("LevelSelectIn");
				CurrentMenuMode = MenuMode.LevelSelect;
                break;
            case 1:
                Debug.Log("Message Board");
                break;
            case 2:
                Debug.Log("Abilties");
                AbilityMenuItems.GetComponent<Animation>().Play("AbilityIn");
                CurrentMenuMode = MenuMode.Abilities;
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
