using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WelcomeMessage : MonoBehaviour {

    public Text welcomeMessage;
	void Start () {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "LoggedIn")
        {
            welcomeMessage.text = "Well Done, " + DataToPass.username + ", You Have Successfully Logged In";
        }
        if (SceneManager.GetActiveScene().name == "AccountCreated")
        {
            welcomeMessage.text = "Welcome, " + DataToPass.username + DataToPass.extraInfo;
        }
    }
}
