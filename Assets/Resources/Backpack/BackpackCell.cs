using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BackpackLocalData;

public class BackpackCell : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler, IPointerExitHandler
{
     private Transform UIIcon;

    private Transform UINew;
    private Transform UISelect;

    private Transform UISelectAni;
    private Transform UIMouseOverAni;

    private Transform UIName;
    private BackpackLocalItem backpackLocalData;

    private BackpackTableItem backpackTableitem;
    private BackpackPanel uiParent;

    void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIIcon = transform.Find("Top/Icon");
        UINew = transform.Find("New");
        UISelect = transform.Find("Select");
        UIName = transform.Find("Bottom/ItemName");
        UISelectAni = transform.Find("SelectAni");
        UIMouseOverAni = transform.Find("MouseOverAni");

        UISelectAni.gameObject.SetActive(false);
        UIMouseOverAni.gameObject.SetActive(false);
    }
 

    public void Refresh(BackpackLocalItem backpackLocalData,BackpackPanel uiParent)
    {
        this.uiParent = uiParent;
        this.backpackLocalData = backpackLocalData;
        this.backpackTableitem = GameManager.Instance.GetBackpackItemById(backpackLocalData.id);

        UINew.gameObject.SetActive(this.backpackLocalData.isNew);
      
        Texture2D t= (Texture2D)Resources.Load(this.backpackTableitem.iconPath);

        UIName.GetComponent<TextMeshProUGUI>().text = this.backpackTableitem.name;

        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));

        UIIcon.GetComponent<Image>().sprite = temp;
    }
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.uiParent.chooseUid == this.backpackLocalData.uid)
            return;
        this.uiParent.chooseUid = this.backpackLocalData.uid;



        UISelectAni.gameObject.SetActive(true);
        UISelectAni.GetComponent<Animator>().SetTrigger("in");

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
