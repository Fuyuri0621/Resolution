using Micosmo.SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using VLB;

public class SecurityCam : MonoBehaviour
{
    public float ScanTime;
    public float TrackTime;

    public Light SpotLight;

    public Sensor targetSensor;

    [SerializeField] int suspiciousRate = 5;
    [SerializeField] float suspicious = 0f;
    [SerializeField] float alertness = 0;
    [SerializeField] float releaseTime = 5;


    [SerializeField] Slider suspiciousSlider;
    [SerializeField] Slider alertnessSlider;

    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = targetSensor.GetNearestDetection();

        ScanMode();
    }


    private void ScanMode()
    {
        if (target != null)
        {


            AddSuspicious();


           
            releaseTime = 3;
            releaseTime = 3;
        }
        else
        {
            if (releaseTime > 0)
                releaseTime -= Time.deltaTime;
            if (releaseTime < 0)
                releaseTime = 0;


            if (releaseTime == 0)
            {
                ReduceAlertness();


                if (releaseTime > 0)
                    releaseTime -= Time.deltaTime;
                if (releaseTime < 0)
                    releaseTime = 0;
            }

            if (alertness == 0 && releaseTime == 0) ReduceSuspicious();
        }


        

        UpdateSlider(suspiciousSlider, (int)suspicious);
        UpdateSlider(alertnessSlider, (int)alertness);
    }


    private void AddSuspicious()
    {

        if (suspicious == 100)
        {

            SpotLight.color = Color.yellow;
            

            AddAlertness();
            return;
        }
        suspicious += (Time.deltaTime * suspiciousRate);

        if (suspicious > 100)
            suspicious = 100;



    }

    private void ReduceSuspicious()
    {
        if (suspicious == 0f)
            return;

        suspicious -= Time.deltaTime * 60;
        if (suspicious < 0) { suspicious = 0; SpotLight.color = Color.blue;  }
    }

    private void AddAlertness()
    {

        if (alertness == 100)
        {
            SpotLight.color = Color.red;

            return;
        }

        alertness += (Time.deltaTime * suspiciousRate);

        if (alertness > 100)
            alertness = 100;

    }

    private void ReduceAlertness()
    {
        if (alertness == 0f)
            return;

        alertness -= Time.deltaTime * 60;
        if (alertness < 0) alertness = 0;
    }


    public void UpdateSlider(Slider targetSlider, int current)
    {

        targetSlider.value = current;
    }
}
