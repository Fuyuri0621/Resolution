using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BackpackLocalData;

public class BackpackDetail : MonoBehaviour
{
    [SerializeField] private Transform UIDescription;
    [SerializeField] private Transform UIShortDescription;
    [SerializeField] private Transform UIIcon;
    [SerializeField]private Transform UITitle;

    private BackpackLocalItem backpackLocalData;
    private BackpackPanel uiParent;
    private BackpackTableItem backpackTableItem;


    void Awake()
    {
        InitUIName();
        Refresh(GameManager.Instance.GetBackpackLocalData()[0],null);
    }

    private void InitUIName()
    {
        UIIcon = transform.Find("Center/Icon");
        UIDescription = transform.Find("Bottom/Description");
        UIShortDescription = transform.Find("Center/Description");
        UITitle = transform.Find("Top/Title");

    }

    public void Refresh(BackpackLocalItem backpackLocalData, BackpackPanel uiparent)
    {
        this.uiParent = uiparent;
        this.backpackLocalData = backpackLocalData;
        this.backpackTableItem = GameManager.Instance.GetBackpackItemById(backpackLocalData.id);

        UIDescription.GetComponent<TextMeshProUGUI>().text = backpackTableItem.description;

        UIShortDescription.GetComponent<TextMeshProUGUI>().text = backpackTableItem.shortDescription;

        UITitle.GetComponent<TextMeshProUGUI>().text = backpackTableItem.name;

        Texture2D t = (Texture2D)Resources.Load(this.backpackTableItem.iconPath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;
    }
}
