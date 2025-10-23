using UnityEngine;

public class Hackend : MonoBehaviour
{
    StageEventManager stageManager;
    void Start()
    {
        stageManager = gameObject.GetComponentInParent<StageEventManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HackPlayer")
        {
            stageManager.AddPoint();
            stageManager.Winstage();
        }
    }
}
