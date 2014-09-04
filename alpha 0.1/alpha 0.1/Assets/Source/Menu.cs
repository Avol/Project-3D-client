using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {
    private Interaction interaction;
	private GameObject _mainMenu;
	private GameObject _profileMenu;
	private GameObject _profileScreen;

	public string currentMenu = "startScreen";

	void changeMenu(string menuName) {
		switch (menuName) {
			case "StartScreen":
			startScreenCode();
				break;
			case "ProfileMenu":
			profileMenuCode();
				break;
			case "ProfileScreen":
			profileScreenCode();
				break;
		}
	}
	
	void Start () {
        this.interaction = GameObject.Find("Main Camera").GetComponent<Core>().interaction;
        this.interaction.addInteractionObject(GameObject.Find("ProfileMenuButton"), delegate()
        {
            profileMenuButton();
        });
		
		this.interaction.addInteractionObject(GameObject.Find("ProfileMenuBackButton"), delegate()
        {
            profileMenuBackButton();
        });
		
		this.interaction.addInteractionObject(GameObject.Find("StartGameButton"), delegate()
        {
            startGame();
        });

		_mainMenu = GameObject.Find("MainMenu");
		_profileMenu = GameObject.Find("ProfileMenu");
		_profileScreen = GameObject.Find("ProfileScreen");

		this.changeMenu ("StartScreen");
	}
	
	void profileMenuButton(){
		this.changeMenu ("ProfileMenu");
	}
	
	void profileMenuBackButton(){
		this.changeMenu ("StartScreen");
	}

	void startScreenCode(){
		_mainMenu.SetActive (true);
		_profileMenu.SetActive (false);
		_profileScreen.SetActive (false);
		Debug.Log ("you are now in the StartScreen");
	}

	void profileMenuCode(){
		_mainMenu.SetActive (false);
		_profileMenu.SetActive (true);
		_profileScreen.SetActive (false);
		Debug.Log ("you are now in the ProfileMenu");
	}

	void profileScreenCode(){
		_mainMenu.SetActive (false);
		_profileMenu.SetActive (false);
		_profileScreen.SetActive (true);
		Debug.Log ("you are now in the profileScreen");
	}

	void startGame(){
		Debug.Log ("Start the game");
		_mainMenu.SetActive (false);
		_profileMenu.SetActive (false);
		_profileScreen.SetActive (false);
		Application.LoadLevel("Room");
	}

	void exitGame(){
		Debug.Log ("Exit the game");
	}

}



