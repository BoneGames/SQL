using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Submit : MonoBehaviour {

    public InputField password, username, newPassword1, newPassword2, newUsername, email;
    public Text newAccountFeedback, loginFeedback;

    void Start () {
		
	}


    void Update () {
		
	}

    public void SubmitLoginButton()
    {
        if (LoginInputCheck())
        {
            loginFeedback.text = "Success!";
        }
    }

    public void SubmitNewButton()
    {
        if(NewInputCheck())
        {
            newAccountFeedback.text = "Success!";
        }
    }

    bool NewInputCheck()
    {
        if(newPassword1.text != "" && newPassword2.text != "" && newUsername.text != "" && email.text != "")
        {
            if(!email.text.Contains("@") || !email.text.Contains(".com") || email.text.Length < 7)
            {
                newAccountFeedback.text = "Enter Valid Email Address";
                return false;
            }
            if(newPassword1.text == newPassword2.text)
            {
                return true;
            }
            else
            {
                newAccountFeedback.text = "The Passwords Entered Do not Match";
                return false;
            }
        }
        else
        {
            newAccountFeedback.text = "Make Sure You Have Entered Your Username, Password, AND confirmed our Password";
            return false;
        }
    }

    bool LoginInputCheck()
    {
        if (password.text != "" && newUsername.text != "")
        {
            return true;
        }
        else
        {
            loginFeedback.text = "Make Sure You Have Entered Your Username AND Password";
            return false;
        }
    }
}

