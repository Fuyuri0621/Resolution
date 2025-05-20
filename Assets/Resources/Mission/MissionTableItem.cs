using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Mission/MissionTableItem", fileName = "MissionTableItem")]

public class MissionTableItem : ScriptableObject
{
    public int id;

    public MissionType type;

    public int getItemID;

    public string Interactprompt;

    public string MissionName;

    public string shortDescription;

    public string description;

    public List<int> Waypointsid;

    public AccomplishBehavior accomplishBehavior;

    [ShowIfEnum("accomplishBehavior", AccomplishBehavior.Notice)] public string AccomplishNotice;
    //一句話
    [ShowIfEnum("accomplishBehavior", AccomplishBehavior.Dialogue)] public string AccomplishDialogue;
    [ShowIfEnum("accomplishBehavior", AccomplishBehavior.Dialogue)] public string DialogueSpeaker;

    //過場動畫
    [ShowIfEnum("accomplishBehavior", AccomplishBehavior.Cutscene)] public Dialogues dialogue;

    [ShowIfEnum("accomplishBehavior", AccomplishBehavior.LoadScene)] public string SceneName;

}

public enum AccomplishBehavior
{
    None,
    Notice,
    Dialogue,
    Cutscene,
    LoadScene,
}