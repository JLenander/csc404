using System.Collections;
using UnityEngine;

public class BlinkConsole : Interactable
{
    public float totalTimeBlink = 50f;
    public float totalTimePress = 10f;

    public float timeToNextBlink;
    public float pressCountdown;
    public HeadConsole headConsole;
    public GameObject blinkOverlay; // a black overlap for camera

    private bool timerIsRunning;
    private bool warning = false;
    private bool danger = false;

    private void Start()
    {
        DisableOutline();
        timeToNextBlink = totalTimeBlink;
        pressCountdown = totalTimePress;
        timerIsRunning = true;

        blinkOverlay.SetActive(false);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeToNextBlink > 0)
            {
                timeToNextBlink -= Time.deltaTime;
            }
            else
            {
                if (!warning)// so it doesnt trigger every frame
                {
                    warning = true;
                    timeToNextBlink = 0;
                    timerIsRunning = false;
                    timeToNextBlink = totalTimeBlink;
                    Debug.Log("time to blink");
                    outlineColour = Color.red;

                    // start camera fade
                    GlobalPlayerUIManager.Instance.PixelateView(pressCountdown);
                }

                EnableOutline();
            }
        }
        else
        {
            if (pressCountdown > 0)
            {
                pressCountdown -= Time.deltaTime;
            }
            else
            {
                if (!danger) // so it doesnt trigger every frame
                {
                    danger = true;
                    Debug.Log("out of time you are sus");
                    // disable head console
                    headConsole.disableInteract();
                }
            }
        }

    }

    public override void Interact(GameObject player)
    {
        PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
        ResetTimers();
        playerInteract.NullInteracting();

        if (blinkOverlay != null)
            StartCoroutine(BlinkRoutine());
    }

    private void ResetTimers()
    {
        DisableOutline();
        timerIsRunning = true;
        timeToNextBlink = totalTimeBlink;
        pressCountdown = totalTimePress;
        outlineColour = Color.white;
        //EnableOutline();
        Debug.Log("timers reset");

        headConsole.enableInteract(); // reenable head
        danger = false; // remove flags
        warning = false; // remove flags
        GlobalPlayerUIManager.Instance.DisablePixelate(); // undo pixelate
    }

    private IEnumerator BlinkRoutine()
    {
        blinkOverlay.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        blinkOverlay.SetActive(false);
    }
}
