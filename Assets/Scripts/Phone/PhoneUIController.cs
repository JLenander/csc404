// Controls phone UI, switch between Face ID, Home, Swipe (profiles to choose), and Match screens.

using UnityEngine;
using UnityEngine.UI;

public class PhoneUIController : MonoBehaviour
{
    public Image screenImage; // drag UI Image here
    // Drag and drop sprites here
    public Sprite faceIDSprite;
    public Sprite homeSprite;
    public Sprite swipeOneSprite;
    public Sprite matchOneSprite;
    public Sprite swipeTwoSprite;
    public Sprite matchTwoSprite;
  
    public void ShowFaceID() => screenImage.sprite = faceIDSprite;
    public void ShowHome() => screenImage.sprite = homeSprite;

    // Toggle between two swipe screens, for now
    // CHANGE when more profiles added
    public void ShowSwipe()
    {
        if (screenImage.sprite == homeSprite)
            screenImage.sprite = swipeOneSprite;
        else if (screenImage.sprite == swipeOneSprite)
            screenImage.sprite = swipeTwoSprite;
        else if (screenImage.sprite == swipeTwoSprite)
            screenImage.sprite = swipeOneSprite;
    }
    
    // Show match screen corresponding to current swipe screen
    public void ShowMatch()
    {
        if (screenImage.sprite == swipeOneSprite)
            screenImage.sprite = matchOneSprite;
        else if (screenImage.sprite == swipeTwoSprite)
            screenImage.sprite = matchTwoSprite;
    }
}


