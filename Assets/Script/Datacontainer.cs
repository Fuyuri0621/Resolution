using UnityEngine;

[CreateAssetMenu(fileName = "Datacontainer", menuName = "Scriptable Objects/Datacontainer")]
public class Datacontainer : ScriptableObject
{
  public ALLSecne currentScene;

    public Vector3 lastCheckpoint;
    public int lastmissionid;
    [Header("¦^´_")]
    public int healAmout;




    public string savedate = "yyyy/mm/dd 00:00";
}

public enum ALLSecne
{
    inLobby,
    outLobby,
    City,
    B4,
    B4_small,
    B3,
    B3_small
}
