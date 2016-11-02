﻿using UnityEngine;
using System.Collections;
using VRTK;




/*TODO: 
 * Implement proper rotation lerping
 * If object is clicked while lerping, move back. 
 * If atPlayer is true for an object and player clicks on another object, send current object back and new one forwards
 * 
 * BUGS: Name of hit gameobject prints three times
 * Line 67 (do more stuff here) doesn't print
 * 
 * 
 */

public class directPlane : VRTK_InteractableObject {

    static private bool atPlayer = false;
    string objAtPlayer = "";
    RaycastHit hit;
    bool running;

    Vector3 startPos; //Beginning position of the lerp
    Quaternion startRot;
    static public float dur = 3.0f; //duration of the lerp
    

	protected override void Start () {
        base.Start();
        Debug.Log("Begin");
        startRot = transform.rotation;
        startPos = transform.position;
    }


    protected override void Update()
    {
       if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                objAtPlayer = hit.transform.gameObject.name;
            }
        }
        


    }
    //For movement of the object on click
    IEnumerator MovePlane()
    {
        //Ending position of the lerp
        Vector3 endPos = Singleton.instance.player.transform.position + Singleton.instance.player.transform.forward;

        if (!atPlayer) //For the lerping towards the player 
        {
            running = true;
            transform.LookAt(Singleton.instance.player.transform);

            //Rotation of the plane
            Quaternion endRot = transform.rotation;
            for (float i = 0; i < dur; i += Time.deltaTime)
            {
                Quaternion newRot = Quaternion.Lerp(startRot, endRot, i / dur);
                this.transform.rotation = newRot;
                yield return null;
            }

            //Translation of the plane
            for (float j = 0; j < dur; j += Time.deltaTime)
            {
                Vector3 newPos = Vector3.Lerp(startPos, endPos, j / dur);
                this.transform.position = newPos;
                yield return null;
            }
            this.transform.position = endPos;
            atPlayer = true;
            running = false;
        }
        else //For lerping back to originial position
        {
            running = true;
            Quaternion endRot = transform.rotation;
       
            for (float i = 0; i < dur; i += Time.deltaTime)
            {
                Vector3 newPos = Vector3.Lerp(endPos, startPos, i / dur);
                this.transform.position = newPos;
                yield return null;
            }
            for (float j = 0; j < dur; j += Time.deltaTime)
            {
                Quaternion newRot = Quaternion.Lerp(endRot, startRot, j / dur);
                this.transform.rotation = newRot;
                yield return null;
            }


            this.transform.position = startPos;
            atPlayer = false;
            running = false;
        }
    }

    public void OnMouseDown()
    {
        Activate();
    }

    public void Activate()
    {
        if (!running)
        {
            StartCoroutine(MovePlane());
        }
    }

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        Activate();
    }

    //If object has this script, set a boolean to true
    //Coroutine to deactivate boolean 
}