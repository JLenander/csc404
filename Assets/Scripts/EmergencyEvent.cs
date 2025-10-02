using System.Collections;
using UnityEngine;

public class EmergencyEvent : MonoBehaviour
{
    public static EmergencyEvent Instance;
    public AudioSource normal; // normal music
    public AudioSource scary; // emergency music

    // alternates between these two lights during event
    public GameObject redLight; // a red light
    public GameObject flashLight; // a darker light

    public GameObject defaultLight; // default scene light
    public float interval = 1f; // light intervals


    [SerializeField] private HandConsole leftArmTerminal; // reference arm console to disable
    [SerializeField] private HandConsole rightArmTerminal; // reference arm console to disable

    public int safeUses = 5; // number of uses that are basically safe

    [SerializeField] private DialogueScriptableObj brokenDialogue;

    private int leftArmShotCount;
    private int rightArmShotCount;
    private bool leftShutdown; // bool used to show left arm is shutdown
    private bool rightShutdown; // bool used to show left arm is shutdown
    private bool emergency;
    private Coroutine toggleCoroutine;

    void Start()
    {
        leftShutdown = false;
        rightShutdown = false;
        emergency = false;

        scary.enabled = false;
        redLight.SetActive(false);
        flashLight.SetActive(false);

        leftArmShotCount = 0;
        rightArmShotCount = 0;

        Instance = this;
    }

    void StartEvent()
    {
        GlobalPlayerUIManager.Instance.LoadText(brokenDialogue);
        emergency = true;
        scary.enabled = true;
        normal.enabled = false;
        redLight.SetActive(true);
        defaultLight.SetActive(false);
        toggleCoroutine = StartCoroutine(ToggleLoop());
    }

    void StopEvent()
    {
        // if another arm still broken, dont cancel
        if (leftShutdown || rightShutdown)
            return;

        emergency = false;

        scary.enabled = false;
        normal.enabled = true;

        defaultLight.SetActive(true);
        redLight.SetActive(false);
        flashLight.SetActive(false);

        if (toggleCoroutine != null)
            StopCoroutine(toggleCoroutine);
    }

    IEnumerator ToggleLoop()
    {
        while (true)
        {
            // Toggle active state
            flashLight.SetActive(!flashLight.activeSelf);
            redLight.SetActive(!redLight.activeSelf);

            // Wait for interval
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// Used to increment arm shot count
    /// </summary>
    public void IncrementCount(bool left)
    {
        if (left)
        {
            leftArmShotCount++;
        }
        else
        {
            rightArmShotCount++;
        }

        TestBreak(left);
    }

    /// <summary>
    /// Test to see if the arm will break
    /// </summary>
    /// <param name="left"></param>
    /// <returns></returns>
    void TestBreak(bool left)
    {
        int useCount;
        HandConsole handConsole;
        if (left)
        {
            useCount = leftArmShotCount;
            handConsole = leftArmTerminal;
            leftShutdown = true;
        }
        else
        {
            useCount = rightArmShotCount;
            handConsole = rightArmTerminal;
            rightShutdown = true;
        }

        int riskyUses = Mathf.Max(0, useCount - safeUses);
        float breakChance = 1f - Mathf.Exp(-0.25f * riskyUses);

        if (Random.value < breakChance) // random is [0,1], chance gets higher with more breaks
        {
            // time to break!
            handConsole.DisableInteract();
            if (!emergency)
            {
                StartEvent();
            }
        }
    }

    /// <summary>
    /// Fix left arm, seperate functions since interactables can only call a function with no params
    /// </summary>
    public void FixLeftArm()
    {

        leftShutdown = false;
        // turn hand console back on
        leftArmTerminal.EnableInteract();
        leftArmShotCount = 0;
        StopEvent();
    }

    /// <summary>
    /// Fix right arm, seperate functions since interactables can only call a function with no params
    /// </summary>
    public void FixRightArm()
    {

        rightShutdown = false;
        // turn hand console back on
        rightArmTerminal.EnableInteract();
        rightArmShotCount = 0;
        StopEvent();
    }
}
