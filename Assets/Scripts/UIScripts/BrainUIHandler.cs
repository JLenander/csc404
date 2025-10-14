using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BrainUIHandler : OverlayUIHandler
{
    public static BrainUIHandler Instance;

    private VisualElement doorUI;
    private VisualElement taskUI;
    private Label leftDoorText, rightDoorText, l2, r2;
    private Color neutralColour;
    private Color redColour;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Instance = this;
        doorUI = root.Query<VisualElement>("DoorWindow").First();
        taskUI = root.Query<VisualElement>("TaskWindow").First();
        leftDoorText = root.Query<Label>("LeftDoorDesc").First();
        rightDoorText = root.Query<Label>("RightDoorDesc").First();
        l2 = root.Query<Label>("L2").First();
        r2 = root.Query<Label>("R2").First();

        ColorUtility.TryParseHtmlString("#2BD575", out neutralColour);
        ColorUtility.TryParseHtmlString("#D52B30", out redColour);

        doorUI.visible = false;
    }

    // switch between door and task UI
    public void SwitchScreen()
    {
        doorUI.visible = !doorUI.visible;
        taskUI.visible = !taskUI.visible;
    }

    // lock one of the doors
    public void LockDoor(bool left, int seconds)
    {
        StartCoroutine(DoorCountdownRoutine(left, seconds));
    }

    IEnumerator DoorCountdownRoutine(bool left, int seconds)
    {
        int currSeconds = seconds;
        if (left)
        {
            leftDoorText.style.color = neutralColour;
            l2.visible = false;
        }
        else
        {
            rightDoorText.style.color = neutralColour;
            r2.visible = false;
        }

        while (currSeconds >= 0)
        {
            string content = "UNLOCKED\n---\nTIME 0:0" + currSeconds;

            if (left)
            {
                leftDoorText.text = content;
            }
            else
            {
                rightDoorText.text = content;
            }

            currSeconds -= 1;

            yield return new WaitForSeconds(1);
        }

        if (left)
        {
            leftDoorText.text = "LOCKED";
            leftDoorText.style.color = redColour;
            l2.visible = true;
        }
        else
        {
            rightDoorText.text = "LOCKED";
            rightDoorText.style.color = redColour;
            r2.visible = true;
        }
    }
}
