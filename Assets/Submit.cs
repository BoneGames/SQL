using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Net;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.SceneManagement;

public class Submit : MonoBehaviour {

    public InputField password, username, newPassword1, newPassword2, newUsername, email, forgotPasswordEmail, randomCode, resetPassword1, resetPassword2;
    public Text newAccountFeedback, loginFeedback, recoverPasswordFeedback, resetPasswordFeedback;
    public GameObject randomCodeObj, resetPasswordsObj;
    public InputField[] passwords = new InputField[5];
    public string toolTip;

    public void LogOut()
    {
        SceneManager.LoadScene(0);
    }

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
        toolTip = "";
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
        if(toolTip == "Success! You're the first user account to have been created!")
        {
            Debug.Log("hello");
            DataToPass.extraInfo = ", you're our first Logger";
            DataToPass.username = _username;
            SceneManager.LoadScene(1);
        }
        if (toolTip == "Success! Your user account has been created.")
        {
            Debug.Log("hello");
            DataToPass.extraInfo = ", login' account created!";
            DataToPass.username = _username;
            SceneManager.LoadScene(1);
        }
        newAccountFeedback.text = toolTip;
    }

    IEnumerator Login(string _username, string _password)
    {
        toolTip = "";
        string loginURL = "http://localhost/squealsystem/Login.php";

        // Info to send to the POST variables (empty) in PHP InsertUser script
        WWWForm loginForm = new WWWForm();

        loginForm.AddField("usernamePost", _username);
        loginForm.AddField("passwordPost", _password);

        WWW www = new WWW(loginURL, loginForm);

        yield return www;
        toolTip = www.text;
        loginFeedback.text = toolTip;
        if(toolTip == "Login Successful")
        {
            DataToPass.username = _username;
            SceneManager.LoadScene(1);
        }
        Debug.Log(toolTip);
    }

    IEnumerator RetrievePassword(string _email)
    {
        Debug.Log(_email);
        toolTip = "";

        string retrievePasswordURL = "http://localhost/squealsystem/passwordRecovery.php";
        WWWForm emailForm = new WWWForm();
        emailForm.AddField("emailPost", _email);
        WWW www = new WWW(retrievePasswordURL, emailForm);
        yield return www;
        Debug.Log(_email);


        if (www.text == "email")
        {
            SendEmail(_email);
            toolTip = "An email with password reset instructions has been sent to " + _email;
            DataToPass.resetPasswordEmail = _email;
        } else {
            toolTip = www.text;
            Debug.Log(toolTip);
        }
        recoverPasswordFeedback.text = toolTip;
    }

    public void SendEmail(string email)
    {
        string code = RandomString();

        MailMessage mail = new MailMessage();
        mail.To.Add(email);
        mail.Subject = "SQueaL Games User Reset";
        mail.Body = "Hello" + username + 
            "\nReset your Account using this code: " + code;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 25;

        smtpServer.Credentials = new System.Net.NetworkCredential
            ("benjaminjgbarnes@gmail.com", "benoisdabomb1") 
            as ICredentialsByHost;

        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback
            = delegate (object s, X509Certificate cert, X509Chain chain
            , SslPolicyErrors policyErrors)
            { return true; };

        smtpServer.Send(mail);
        StartCoroutine("DelayLoadScene");
        
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(3);
    }

    string RandomString()
    {
        int seed = Random.Range(0,System.DateTime.Now.Millisecond);
        int number = seed * System.DateTime.Now.Second;
        string randomCode = number.ToString();
        DataToPass.randomCode = randomCode;
        return randomCode;
    }

    public void CheckCode()
    {
        if(randomCode.text == DataToPass.randomCode)
        {
            randomCodeObj.SetActive(false);
            resetPasswordsObj.SetActive(true);
        }
    }

    public void CheckResetPasswords()
    {
        resetPasswordFeedback.text = "";
        if(resetPassword1.text == resetPassword2.text)
        {
            StartCoroutine(ChangePassword(resetPassword1.text));
        } 
        else
        {
            resetPasswordFeedback.text = "Your passwords don't match.";
        }
    }

    IEnumerator ChangePassword(string _password)
    {
        toolTip = "";
        // Link to PHP
        string changePasswordURL = "http://localhost/squealsystem/ResetPassword.php";

        // Info to send to the POST variables (empty) in PHP InsertUser script
        WWWForm changePasswordForm = new WWWForm();
        
        changePasswordForm.AddField("passwordPost", _password);
        changePasswordForm.AddField("emailPost", DataToPass.resetPasswordEmail);
        
        WWW www = new WWW(changePasswordURL, changePasswordForm);
        
        yield return www;
        toolTip = www.text;
        Debug.Log(toolTip);
        if(toolTip == "Success")
        {
            resetPasswordFeedback.text = "Well Done, you have reset your password. You can now Login";
            resetPasswordsObj.SetActive(false);
        } else {
            resetPasswordFeedback.text = "There was a problem resetting your password. The internet must be broken...";
        }
        // if (toolTip == "Success! Your user account has been created.")
        // {
        //     Debug.Log("hello");
        //     DataToPass.extraInfo = ", login' account created!";
        //     DataToPass.username = _username;
        //     SceneManager.LoadScene(1);
        // }
        // newAccountFeedback.text = toolTip;
    }
}