// Controls phone UI, switch between Face ID, Home, Swipe (profiles to choose), and Match screens.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUIController : MonoBehaviour
{
    public Image screenImage; // drag UI Image here
    public Image nextImage; // drag next UI Image
    // Drag and drop sprites here
    public Sprite faceIDSprite;
    public Sprite homeSprite;
    public List<Sprite> profiles;
    public List<Sprite> matchProfiles;

    public float flySpeed = 1000f;  // pixels per second
    public float tiltAngle = 30f;   // degrees

    public void ShowFaceID() => screenImage.sprite = faceIDSprite;
    public void ShowHome() => screenImage.sprite = homeSprite;

    private int index = 0;
    private int count = 0;
    private int swipeDirection = 0; // -1 = left, 1 = right
    private RectTransform rt;
    private bool swiping = false;

    private void Awake()
    {
        rt = screenImage.rectTransform;
    }

    // Toggle between two swipe screens, for now
    // CHANGE when more profiles added
    public bool ClickApp()
    {
        if (screenImage.sprite == homeSprite)
        {
            screenImage.sprite = profiles[index];
            index = (index + 1) % profiles.Count;
            nextImage.sprite = profiles[index];
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SwipeNope()
    {
        // flick sceen image left
        StartSwipe(-1);
        count++; // increase count
    }

    public void SwipeLike()
    {
        if (count < 4) // if count not surpassed, keep the swiping
        {
            StartSwipe(1);
            count++; // increase count
        }
        else
        {
            screenImage.sprite = matchProfiles[index]; // match them
        }
    }

    private void StartSwipe(int direction)
    {
        swipeDirection = direction;
        swiping = true;
    }

    private void Update()
    {
        if (!swiping) return;

        // Move horizontally based on direction
        rt.anchoredPosition += Vector2.right * swipeDirection * flySpeed * Time.deltaTime;

        // Tilt while flying (left/right)
        rt.localRotation = Quaternion.Euler(0, 0, 180 - tiltAngle * swipeDirection);

        // Off-screen check
        if (Mathf.Abs(rt.anchoredPosition.x) > Screen.width)
        {
            swiping = false;
            rt.anchoredPosition = Vector2.zero;
            rt.localRotation = Quaternion.Euler(0, 0, 180);

            // afterwards swap screen to next and next to next next
            screenImage.sprite = profiles[index];
            index = (index + 1) % profiles.Count; // loop around if past the list
            nextImage.sprite = profiles[index];
        }
    }
}


