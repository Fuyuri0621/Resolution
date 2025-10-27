using UnityEngine;
using System;
using System.Collections;
using static UnityEngine.UI.Button;
using UnityEngine.Serialization;

public class CloseAftertime : MonoBehaviour
{
    [FormerlySerializedAs("onAfterTime")]
    public ButtonClickedEvent ClickAction;
    [SerializeField] float time = 3f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void open()
    {
        StartCoroutine("wait");
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(time);

        ClickAction.Invoke();
    }
}
