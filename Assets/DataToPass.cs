﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataToPass : MonoBehaviour {

    public static string username, extraInfo, randomCode, resetPasswordEmail;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
