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
    public Sprite anyOneSprite;
    public Sprite matchOneSprite;
    public Sprite swipeTwoSprite;
    public Sprite anyTwoSprite;
    public Sprite matchTwoSprite;
    // public Sprite swipeThreeSprite;      // for now no option to choose on third
    // either swiped out fro first time (to one) or auto match on swipe
    public Sprite anyThreeSprite;
    public Sprite matchThreeSprite;
  
    public void ShowFaceID() => screenImage.sprite = faceIDSprite;
    public void ShowHome() => screenImage.sprite = homeSprite;

    // Toggle between two swipe screens, for now
    // CHANGE when more profiles added
    public void ShowSwipe()
    {
        if (screenImage.sprite == homeSprite)
            screenImage.sprite = anyOneSprite;
        else if (screenImage.sprite ==anyOneSprite)
            screenImage.sprite = anyTwoSprite;
        else if (screenImage.sprite == anyTwoSprite)
                screenImage.sprite = anyThreeSprite;
        else if (screenImage.sprite == anyThreeSprite)
                screenImage.sprite = swipeOneSprite; 
        else if (screenImage.sprite == swipeOneSprite)
            screenImage.sprite = swipeTwoSprite;
        else if (screenImage.sprite == swipeTwoSprite)
            screenImage.sprite = matchThreeSprite;
            // auto match to third if second swiped Second profile
    }
    
    // Show match screen corresponding to current swipe screen
    public void ShowMatch()
    {
        if (screenImage.sprite ==anyOneSprite)
            screenImage.sprite = anyTwoSprite;
        else if (screenImage.sprite == anyTwoSprite)
            screenImage.sprite = anyThreeSprite;
        else if (screenImage.sprite == anyThreeSprite)
            screenImage.sprite = swipeOneSprite; 
        else if (screenImage.sprite == swipeOneSprite)
            screenImage.sprite = matchOneSprite;
        else if (screenImage.sprite == swipeTwoSprite)
            screenImage.sprite = matchTwoSprite;
        // none for three because if two was rejected three was auto matched
    }
}


