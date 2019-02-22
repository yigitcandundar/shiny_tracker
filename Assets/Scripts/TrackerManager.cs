using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerManager : MonoBehaviour
{
    private static TrackerManager instance = null;

    private static TrackerData trackerData;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(gameObject);
        }

        LoadTrackerProgress();
    }

    public static void SaveTrackerProgress()
    {
        string dataString = "";

        if (trackerData != null)
        {
            dataString = JsonUtility.ToJson(trackerData);

            PlayerPrefs.SetString("trackerData", dataString);

            Debug.Log("Successfully saved data!");
        }
        else
        {
            Debug.Log("Tracker data is null! Couldn't save progress!");
        }
    }
    public static void LoadTrackerProgress()
    {
        string dataString = "";

        dataString = PlayerPrefs.GetString("trackerData");

        if (!String.IsNullOrEmpty(dataString))
        {
            trackerData = JsonUtility.FromJson<TrackerData>(dataString);
        }
        else
        {
            trackerData = new TrackerData();
            Debug.Log("Couldnt load data!");
        }
    }

    public static void AddTracker(string trackerTitle)
    {
        if (!string.IsNullOrEmpty(trackerTitle))
        {
            TrackerInfo newInfo = new TrackerInfo(trackerTitle);

            trackerData.info.Add(newInfo);

            SaveTrackerProgress();
        }
        else
        {
            Debug.Log("Title cannot be empty!");
        }
    }
    public static void RemoveTracker(string trackerID)
    {
        if (trackerData != null && trackerData.info != null)
        {
            bool objDeleted = false;

            for (int i = 0; i < trackerData.info.Count; i++)
            {
                if (trackerData.info[i].id == trackerID)
                {
                    trackerData.info.Remove(trackerData.info[i]);
                    objDeleted = true;
                    break;
                }
            }

            if (objDeleted)
            {
                SaveTrackerProgress();
                Debug.Log("Deletion successful!");
            }
            else
            {
                Debug.Log("Tracker not found!");
            }
        }
        else
        {
            Debug.Log("Error occured while removing tracker!");
        }
    }
    public static void CloseTracker(string trackerID)
    {
        if (trackerData != null && trackerData.info != null)
        {
            bool objUpdated = false;

            for (int i = 0; i < trackerData.info.Count; i++)
            {
                if (trackerData.info[i].id == trackerID)
                {
                    if (!trackerData.info[i].isFinished)
                    {
                        trackerData.info[i].FinishTracker();
                        objUpdated = true;
                    }
                    else
                    {
                        Debug.Log("Tracker already closed!");
                    }
                    break;
                }
            }

            if (objUpdated)
            {
                SaveTrackerProgress();
                Debug.Log("Deletion successful!");
            }
            else
            {
                Debug.Log("Tracker not found!");
            }
        }
        else
        {
            Debug.Log("Error occured while removing tracker!");
        }
    }

    public static TrackerData GetTrackerData()
    {
        return trackerData;
    }

    public void OnApplicationQuit()
    {
        SaveTrackerProgress();
    }
}

[Serializable]
public class TrackerData
{
    public TrackerData()
    {
        info = new List<TrackerInfo>();
    }

    public List<TrackerInfo> info;
}

[Serializable]
public class TrackerInfo
{
    public TrackerInfo(string _title)
    {
        id = GenerateID();
        title = _title;
        tryCount = 0;
        startDate = DateTime.Now.ToShortDateString();
        endDate = "In Progress";
        isFinished = false;
    }
    public void FinishTracker()
    {
        isFinished = true;
        endDate = DateTime.Now.ToShortDateString();
    }

    public string id;
    public string title;
    public int tryCount;
    public string startDate;
    public string endDate;
    public bool isFinished = false;

    public string GenerateID()
    {
        return Guid.NewGuid().ToString("N");
    }
}