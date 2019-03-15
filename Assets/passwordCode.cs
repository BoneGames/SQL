using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class passwordCode : MonoBehaviour {

    public InputField passwordResetCode;


    public void PasswordCode()
    {
        if(passwordResetCode.text == DataToPass.randomCode)
        {
            Debug.Log("Success");
        }
    }
}
