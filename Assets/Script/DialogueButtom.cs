using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueButtom : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public int id;

    GameObject selectline;
    // Start is called before the first frame update
    void Start()
    {
        selectline = transform.Find("line").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueController.Instance.ButtomClick(id);
        selectline.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectline.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectline.SetActive(false);
    }
}
