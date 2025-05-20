using UnityEngine;
using UnityEngine.Events;

public class EventTriggerZone : MonoBehaviour
{
    public bool hasDiffrentTrigger = true;
    [HideIf("hasDiffrentTrigger")] public UnityEvent onEnter;
    [ShowIf("hasDiffrentTrigger")] public UnityEvent onChinLiEnter;
    [ShowIf("hasDiffrentTrigger")] public UnityEvent onTanTongEnter;
    public bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {
            if (hasDiffrentTrigger)
            {
                switch (GameManager.Instance.controlingCharacter)
                {
                    case AllPlayerCharacter.ChinLi:
                        {

                            onChinLiEnter.Invoke();
                            break;
                        }
                    case AllPlayerCharacter.TanTong:
                        {
                            MissionManager.Instance.SwitchMission(19);
                            onTanTongEnter.Invoke();
                            break;
                        }


                }
            }
            else
            {
                onEnter.Invoke();
            }
                    hasTriggered = true;
            
        }
    }
}
