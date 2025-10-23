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
        Triggered(other.tag);
    }

    public void Triggered(string tag)
    {
        if (!GameManager.Instance.IsControlling) return;
        if (hasTriggered && triggerOnce) return;

        if (tag=="Player")
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
