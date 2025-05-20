using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoticeManager : MonoBehaviour
{
    [SerializeField]GameObject canvas;
    [SerializeField] GameObject noticePanel;
    [SerializeField] GameObject dialoguePanel;
    private static NoticeManager _instance;
    float timer = 0;

    public static IEnumerator WaitForSeconds(float duration, Action action = null)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }

    private void Awake()
    {
        _instance = this;


    }

    public static NoticeManager Instance
    {
        get { return _instance; }
    }
    void Start()
    {
     //   SendDialogue("測試員A", "突然被叫來測試最新的保全系統，真是有夠雖的", 3f, 0f);
     //   SendDialogue("測試員A", "我記得隊長說測試用的ID卡放在櫃台 去找找吧", 3f, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0) { timer = 0; }

        if (timer > 0) { timer -= Time.deltaTime; }
    }

    public void SendNotice(string notice, float time, float waitTime)
    {
        if (waitTime > 0)
        {
            StartCoroutine(WaitForSeconds(waitTime, () =>
            {
                GameObject msg = Instantiate(noticePanel, canvas.transform);
                msg.GetComponentInChildren<TextMeshProUGUI>().text = notice;
                Destroy(msg, time);
            }));
        }
        else
        {
            GameObject msg = Instantiate(noticePanel, canvas.transform);
            msg.GetComponentInChildren<TextMeshProUGUI>().text = notice;
            Destroy(msg, time);
        }

    }

    public void SendDialogue(string speaker, string content, float time, float waitTime)
    {
        if (waitTime > 0)
        {
            StartCoroutine(WaitForSeconds(waitTime, () =>
            {
                GameObject msg = Instantiate(dialoguePanel, canvas.transform);
                msg.transform.Find("Speaker").GetComponent<TextMeshProUGUI>().text = speaker;
                msg.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = content;
                Destroy(msg, time);
            }));
        }
        else
        {
            GameObject msg = Instantiate(dialoguePanel, canvas.transform);
            msg.transform.Find("Speaker").GetComponent<TextMeshProUGUI>().text = speaker;
            msg.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = content;
            Destroy(msg, time);
        }
    }
}

