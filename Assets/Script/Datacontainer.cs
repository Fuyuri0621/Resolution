using UnityEngine;

[CreateAssetMenu(fileName = "Datacontainer", menuName = "Scriptable Objects/Datacontainer")]
public class Datacontainer : ScriptableObject
{
  [SerializeField]  ALLSecne lsatScene;

    public Vector3 lastCheckpoint;
    public int lastmissionid;
}

public enum ALLSecne
{
    Lobby,
    City,
    B4,
    B3
}
