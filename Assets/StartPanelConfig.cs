using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelConfig : MonoBehaviour
{

    public GameObject LoginCanvas, NewAccountCanvas, NewPasswordCanvas;
    void Start()
    {
        LoginCanvas.SetActive(true);
        NewAccountCanvas.SetActive(false);
        NewPasswordCanvas.SetActive(false);
    }
}
