using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour {

    public static SelectionManager instance = null;

    public GameObject selectionPanel;
    public GameObject detailPanel;
    public GameObject entryPanel;
    public GameObject correctionPanel;

    public GameObject selectionObjectPrefab;
    public RectTransform selectionObjectParent;

    public GameObject noDataFoundObject;
    
    public Text detailTitle;
    public Text detailCounter;

    public InputField newEntryField;

    public InputField correctionField;

    private List<GameObject> currentObjs = new List<GameObject>();
    private SelectionBehavior currentSelection;
    private TrackerInfo currentInfo;

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
    }


    private void Start()
    {
        InitializeSelectionMenu();
    }
         
    public void InitializeSelectionMenu()
    {
        UpdateSelectionMenu(TrackerManager.GetTrackerData());
    }
    public void UpdateSelectionMenu(TrackerData data)
    {
        OpenSelectionPanel();

        foreach (GameObject obj in currentObjs)
        {
            Destroy(obj);
        }

        currentObjs.Clear();

        if (data != null && data.info != null && data.info.Count > 0)
        {
            noDataFoundObject.SetActive(false);
            
            GameObject current;

            foreach (TrackerInfo info in data.info)
            {
                current = Instantiate(selectionObjectPrefab, selectionObjectParent);
                current.GetComponent<SelectionBehavior>().Initialize(info);
                currentObjs.Add(current);
            }
        }
        else
        {
            noDataFoundObject.SetActive(true);
        }
    }

    public void OpenSelectionPanel()
    {
        selectionPanel.SetActive(true);
        detailPanel.SetActive(false);
        entryPanel.SetActive(false);
    }
    public void OpenDetailPanel(SelectionBehavior selection, TrackerInfo trackerInfo)
    {
        currentSelection = selection;
        currentInfo = trackerInfo;

        selectionPanel.SetActive(false);
        detailPanel.SetActive(true);
        entryPanel.SetActive(false);

        detailTitle.text = currentInfo.title;
        detailCounter.text = currentInfo.tryCount.ToString();
    }
    public void CloseDetailPanel()
    {
        TrackerManager.SaveTrackerProgress();
        InitializeSelectionMenu();
    }

    public void OpenEntryPanel()
    {
        entryPanel.SetActive(true);

        newEntryField.text = "";
    }
    public void SubmitEntry()
    {
        if (!string.IsNullOrEmpty(newEntryField.text))
        {
            TrackerManager.AddTracker(newEntryField.text);

            InitializeSelectionMenu();
        }
        else
        {
            Debug.Log("Entry title cannot be empty");
        }
    }

    public void IncrementCounter()
    {
        if (currentInfo != null && !currentInfo.isFinished)
        {
            currentInfo.tryCount++;
            detailCounter.text = currentInfo.tryCount.ToString();
        }
    }
    public void CloseTracker()
    {
        if (currentInfo != null)
        {
            TrackerManager.CloseTracker(currentInfo.id);
            InitializeSelectionMenu();
        }
    }
    public void RemoveTracker()
    {
        if (currentInfo != null)
        {
            TrackerManager.RemoveTracker(currentInfo.id);
            InitializeSelectionMenu();
        }
    }

    public void OpenCorrectionPanel()
    {
        correctionPanel.SetActive(true);

        if (currentInfo != null)
        {
            correctionField.text = currentInfo.tryCount.ToString();
        }
        else
        {
            correctionField.text = "0";
        }
    }
    public void CloseCorrectionPanel(bool doCorrection)
    {
        correctionPanel.SetActive(false);

        if (doCorrection)
        {
            if (currentInfo != null)
            {
                int current= System.Convert.ToInt32(correctionField.text);

                if (current > 0)
                {
                    currentInfo.tryCount = current;
                }
                else
                {
                    currentInfo.tryCount = 0;
                }

                detailCounter.text = currentInfo.tryCount.ToString();
            }
        }
    }
}