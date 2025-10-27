using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EventController : MonoBehaviour
{
    public Image imgMessage;
    int idxMessage = 0;
    public Sprite[] sprMessages;

    public void GotoScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void CarouselClick()
    {
        idxMessage++;
        idxMessage = idxMessage % sprMessages.Length;
        imgMessage.sprite = sprMessages[idxMessage];
    }
}
