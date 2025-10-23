using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractSlider : MonoBehaviour
{
    // Start is called before the first frame update
    float v;
    [SerializeField] Slider slider;
   [SerializeField] Text text;
    [SerializeField] Playercontrol1 playercontrol1;

    float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) { timer -= Time.deltaTime;v += Time.deltaTime; text.text = timer.ToString("0.0");slider.value = v; }
        if (timer < 0) { timer = 0;  gameObject.SetActive(false); GameManager.Instance.IsControlling = true; }
    }

    public void StartInteract(float t)
    {

        timer = t;
        slider.maxValue = t;
        v = 0;
    }
}
