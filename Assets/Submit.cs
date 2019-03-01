using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Submit : MonoBehaviour {

    public InputField password, username, newPassword1, newPassword2, newUsername, email, forgotPasswordEmail;
    public Text newAccountFeedback, loginFeedback;
    string toolTip;


    public void SubmitLoginButton()
    {
        if (LoginInputCheck())
        {
            StartCoroutine(Login(username.text, password.text));
        }
    }

    public void SubmitNewButton()
    {
        if(NewInputCheck())
        {
            //newAccountFeedback.text = "Success!";
            StartCoroutine(CreateUser(newUsername.text, newPassword1.text, email.text));

        }
    }

    public void RetrievePasswordButton()
    {
        StartCoroutine(RetrievePassword(forgotPasswordEmail.text));
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
            if(newPassword1.text != newPassword2.text)
            {
                newAccountFeedback.text = "Your Passwords Do not Match";
                return false;
            }
        }
        else
        {
            newAccountFeedback.text = "Make Sure You Have Entered Your Username, Email, AND your Password twice";
            return false;
        }
        return true;
    }

    bool LoginInputCheck()
    {
        if (password.text != "" && username.text != "")
        {
            return true;
        }
        else
        {
            loginFeedback.text = "Make Sure You Have Entered Your Username AND Password";
            return false;
        }
    }

    

    IEnumerator CreateUser(string _username, string _password, string _email)
    {
        // Link to PHP
        string createUserURL = "http://localhost/squealsystem/InsertUser.php";

        // Info to send to the POST variables (empty) in PHP InsertUser script
        WWWForm insertUserForm = new WWWForm();
        
        insertUserForm.AddField("usernamePost", _username);
        insertUserForm.AddField("passwordPost", _password);
        insertUserForm.AddField("emailPost", _email);
        
        WWW www = new WWW(createUserURL, insertUserForm);

        yield return www;
        toolTip = www.text;
        newAccountFeedback.text = toolTip;
    }

    IEnumerator Login(string _username, string _password)
    {
        string loginURL = "http://localhost/squealsystem/Login.php";

        // Info to send to the POST variables (empty) in PHP InsertUser script
        WWWForm loginForm = new WWWForm();

        loginForm.AddField("usernamePost", _username);
        loginForm.AddField("passwordPost", _password);

        WWW www = new WWW(loginURL, loginForm);

        yield return www;
        toolTip = www.text;
        loginFeedback.text = toolTip;
        Debug.Log(toolTip);
    }

    IEnumerator RetrievePassword(string _email)
    {
        string retrievePasswordURL = "http://localhost/squealsystem/RetrievePassword.php";
        return null;
    }
}