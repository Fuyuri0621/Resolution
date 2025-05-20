using UnityEngine;
using UnityEngine.Playables;

public class CutTrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    
    public void PlayCut()
    {
        timeline.Play();
        //°±¤î±±¨î
    }
}
