using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PowerBackTimer : MonoBehaviour
{
    public float timer = 60f;
    TextMeshProUGUI timeui;
    public UnityEvent timesup;
    void Start()
    {
        timeui =GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timeui.text = timer.ToString("0.00");
        if (timer < 0f) { timesup.Invoke(); Destroy(gameObject); }

    }
}
