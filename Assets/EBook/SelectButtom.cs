using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class SelectButtom : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{




    private Transform UIMouseOverAni;

    [FormerlySerializedAs("onClick")]
    public ButtonClickedEvent ClickAction;



    void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {




        UIMouseOverAni = transform.Find("MouseOverAni");


        UIMouseOverAni.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        UIMouseOverAni.gameObject.SetActive(true);
        UIMouseOverAni.GetComponent<Animator>().SetTrigger("click");
        ClickAction.Invoke();


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIMouseOverAni.gameObject.SetActive(true);
        UIMouseOverAni.GetComponent<Animator>().SetTrigger("in");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIMouseOverAni.gameObject.SetActive(true);
        UIMouseOverAni.GetComponent<Animator>().SetTrigger("out");
    }

}
