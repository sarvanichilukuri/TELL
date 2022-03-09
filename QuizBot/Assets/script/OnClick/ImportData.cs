using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

// Responsible for importing data into RedCap
public class ImportData  : MonoBehaviour
{
    public SaveLoad saveLoad;
    public Button clickedButton; //Button clicked

    void Start()
    {
        saveLoad = new SaveLoad();
        clickedButton.onClick.AddListener(() => ImportActions());
    }


    // Function that executes on button click and is responsible for importing data.
    // The aim is to develop this function for each scene (if import required)
    void ImportActions()
    {
        // If classroomId was not entered in the first scene, import is not allowed.
        if (DataManager.classroomId == String.Empty)
            Debug.Log("Classroom ID is missing, cannot import");
        else
        {
            // Prepare request for import.
            RedCapRequest redCapRequest = new RedCapRequest();
            redCapRequest.token = "B345C5E9AFB7556F4627986E305D4F81"; // This is Akshay's creds, to be replaced!
            redCapRequest.content = "record";
            redCapRequest.action = "export";
            redCapRequest.format = "json";
            redCapRequest.type = "flat";
            redCapRequest.returnFormat = "json";
            redCapRequest.fields_0 = "record_id";
            redCapRequest.form_0 = "credentials";
            redCapRequest.filterLogic = "[classroom_id]=" + "\"" + DataManager.classroomId + "\"";

            // Execute import request
            StartCoroutine(RedCapService.Instance.ImportAllData(usersDetails => GetAndSaveUserDetails(usersDetails),
                                                                        redCapRequest));
        }
    }

    // Function is responsible for preprocessing data obtained from RedCap and storing it locally.
    // 1. Records obtained from RedCap should contain classroomId and childId. If any of these are missing, record
    // is not stored locally.
    // 2. Once eligible records are obtained, they are stored locally.
    void GetAndSaveUserDetails(UsersDetails usersDetails)
    {
        for (int index = usersDetails.users.Count - 1; index >= 0; index--)
        {
            Credential credential = usersDetails.users[index];
            if (credential.classroom_id == String.Empty || credential.child_id == String.Empty)
            {
                usersDetails.users.RemoveAt(index);
                Debug.Log("Record obtained from RedCap is corrupted, missing classroom_id/child_id");
            }
        }
        
        if (usersDetails.users.Count > 0)
            saveLoad.SaveAll(usersDetails);
        else
            Debug.Log("No data to import in RedCap Database");
    }
}