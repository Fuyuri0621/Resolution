using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;
using static VLB.MaterialModifier;

public class MissionManager : MonoBehaviour
{

    [SerializeField] MissionTable missionTable;
    [SerializeField] TextMeshProUGUI MissionPanelText;

    [SerializeField] int nowMissionID;
    [SerializeField] List<MissionTargetUI> allWaypoints;


    DialogueController dialogueController;


    private static MissionManager _instance;
    public static MissionManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;


        Debug.Log("載入 " + missionTable.MissionList.Count + " 個任務");
    }
    void Start()
    {
        dialogueController = GameManager.Instance.gameObject.GetComponent<DialogueController>();
        UpdateMissioninfo();
        for (int i = 0; i < missionTable.MissionList[nowMissionID].Waypointsid.Count; i++)
        {
            allWaypoints.Find(x => x.waypointID == missionTable.MissionList[nowMissionID].Waypointsid[i]).gameObject.SetActive(true);
        }
    }

    void Update()
    {

    }

    virtual public bool CheackMission(string prompt)
    {
        switch (missionTable.MissionList[nowMissionID].type)
        {
            case MissionType.Interact:

                if (missionTable.MissionList[nowMissionID].Interactprompt == prompt) { AccomplishMission(); return true; }
                break;

            case MissionType.ToPosition:

                if (missionTable.MissionList[nowMissionID].Interactprompt == prompt) { AccomplishMission(); return true; }
                break;

        }
        return false;
    }

    virtual public void CheackMission()
    {
        switch (missionTable.MissionList[nowMissionID].type)
        {
            case MissionType.GetItem:

                if (GameManager.Instance.GetBackpackLocalItemById(missionTable.MissionList[nowMissionID].getItemID) != null) { AccomplishMission(); }

                break;

            case MissionType.Killememy:

                if (!GameManager.Instance.isBattling) { AccomplishMission(); }

                break;
        }
    }
    public void SwitchMission(int id)
    {
        nowMissionID = id;
        UpdateMissioninfo();
    }
    public void AccomplishMission()
    {
        switch (missionTable.MissionList[nowMissionID].accomplishBehavior)
        {
            case AccomplishBehavior.None:
                {
                    break;
                }
            case AccomplishBehavior.Notice:
                {
                    NoticeManager.Instance.SendNotice(missionTable.MissionList[nowMissionID].AccomplishNotice, 2f, 0f);
                    break;
                }
            case AccomplishBehavior.Dialogue:
                {
                    NoticeManager.Instance.SendDialogue(missionTable.MissionList[nowMissionID].DialogueSpeaker, missionTable.MissionList[nowMissionID].AccomplishDialogue, 2, 0);
                    break;
                }
            case AccomplishBehavior.Cutscene: //之後要改成過場對話
                {

                    dialogueController.Talk(missionTable.MissionList[nowMissionID].dialogue);
                    break;
                }
            case AccomplishBehavior.LoadScene: 
                {

                    SceneManager.LoadScene(missionTable.MissionList[nowMissionID].SceneName);
                    break;
                }
        }

        for (int i = 0; i < missionTable.MissionList[nowMissionID].Waypointsid.Count; i++)
        {
            allWaypoints.Find(x => x.waypointID == missionTable.MissionList[nowMissionID].Waypointsid[i]).gameObject.SetActive(false);
        }

        GameManager.Instance.UpdateCheckPoint();

        nowMissionID++;

        UpdateMissioninfo();

    }

    public int Getnowmissionid() {  return nowMissionID; }
    private void UpdateMissioninfo()
    {
        MissionPanelText.text = missionTable.MissionList[nowMissionID].shortDescription;

        for (int i = 0; i < missionTable.MissionList[nowMissionID].Waypointsid.Count; i++)
        {
            allWaypoints.Find(x => x.waypointID == missionTable.MissionList[nowMissionID].Waypointsid[i]).gameObject.SetActive(true);
        }
    }
}
