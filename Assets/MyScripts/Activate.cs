﻿using UnityEngine;
using VRTK;
using System.Collections;

public class Activate : MonoBehaviour {


    private GameObject objActivate;

    void Start() {
        objActivate = GetComponentInChildren<VRTK_ObjectTooltip>().gameObject;
    }

	// Update is called once per frame
	void Update () {


        if (Input.GetMouseButtonDown(0)) {
        
            objActivate.SetActive(!objActivate.activeSelf);
            
        }

    }
}
