using UnityEngine;
using UnityEngine.UI;

public class PhoneUIController : MonoBehaviour
{
    public Image screenImage; // drag UI Image here
    public Sprite faceIDSprite;
    public Sprite homeSprite;
    public Sprite swipeSprite;
    public Sprite matchSprite;
  
    public void ShowFaceID() => screenImage.sprite = faceIDSprite;
    public void ShowHome() => screenImage.sprite = homeSprite;
    public void ShowSwipe() => screenImage.sprite = swipeSprite;
    public void ShowMatch() => screenImage.sprite = matchSprite;
}


