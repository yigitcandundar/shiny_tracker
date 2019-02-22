using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBehavior : MonoBehaviour {

    public Text titleText;
    public Text startDateText;
    public Text endDateText;
    public Text tryCountText;

    public Button buttonContinue;
    public Button buttonRemove;

    private TrackerInfo selfInfo;

	public void Initialize(TrackerInfo info)
    {
        selfInfo = info;

        titleText.text = info.title;

        startDateText.text = "Start Date: \n" + info.startDate;
        endDateText.text = "End Date: \n" + info.endDate;

        if (selfInfo.isFinished)
        {
            buttonContinue.gameObject.SetActive(false);
            buttonRemove.gameObject.SetActive(true);
            tryCountText.text = "Finished in " + info.tryCount.ToString() + " tries";
        }
        else
        {
            buttonContinue.gameObject.SetActive(true);
            buttonRemove.gameObject.SetActive(false);
            tryCountText.text = "Current Tries: " + info.tryCount.ToString();
        }

        buttonContinue.onClick.AddListener(Continue);
        buttonRemove.onClick.AddListener(Remove);
    }

    void Continue()
    {
        if (!selfInfo.isFinished)
        {
            SelectionManager.instance.OpenDetailPanel(this, selfInfo);
        }
        else
        {
            buttonContinue.gameObject.SetActive(false);
            buttonRemove.gameObject.SetActive(true);
        }
    }
    void Remove()
    {
        TrackerManager.RemoveTracker(selfInfo.id);
        SelectionManager.instance.InitializeSelectionMenu();
    }
}