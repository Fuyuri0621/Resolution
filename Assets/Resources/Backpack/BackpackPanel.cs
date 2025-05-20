using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BackpackLocalData;

public class BackpackPanel : MonoBehaviour
{


    Transform UIMenu;

    Transform UIScrolView;

    Transform UIDetailpanel;

    public GameObject BackpackUIItemPrefab;

    private string _chooseUid;
    public string chooseUid
    {
        get
        {
            return _chooseUid;
        } 
        set
        {
            _chooseUid = value;
            RefreshDetail();
        }
    }

    private void Awake()
    {
        UIScrolView = transform.Find("Center/Scroll View");
        UIDetailpanel = transform.Find("Center/DetailPanel");
    }

    private void Start()
    {
        RefreshUI();
    }

    private void Update()
    {

    }

    private void RefreshUI()
    {
        RefreshScroll();
    }
    private void RefreshDetail()
    {
        BackpackLocalItem localItem = GameManager.Instance.GetBackpackItemByUid(chooseUid);

        if (localItem.isNew)
        { localItem.isNew = false; }
        RefreshScroll();

        UIDetailpanel.GetComponent<BackpackDetail>().Refresh(localItem, this);



    }
    private void RefreshScroll()
    {
        RectTransform scrollContent = UIScrolView.GetComponent<ScrollRect>().content;
        for(int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        foreach(BackpackLocalItem localItem in GameManager.Instance.GetSortBackPackLocalData())
        {
            Transform BackpackUIItem = Instantiate(BackpackUIItemPrefab.transform,scrollContent) as Transform;
            BackpackCell backpackCell = BackpackUIItem.GetComponent<BackpackCell>();
            backpackCell.Refresh(localItem, this);
        }
    }
    public void ClosePanel()
    {
        GameManager.Instance.PauseGame();

        Destroy(gameObject);
    }
}
