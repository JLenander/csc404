using UnityEngine;

public class BlinkConsole : Interactable
{
    public float totalTimeBlink = 50f;
    public float totalTimePress = 10f;

    public float timeToNextBlink;
    public float pressCountdown;

    private bool timerIsRunning;

    private void Start()
    {
        DisableOutline();
        timeToNextBlink = totalTimeBlink;
        pressCountdown = totalTimePress;
        timerIsRunning = true;
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
                timeToNextBlink = 0;
                timerIsRunning = false;
                timeToNextBlink = totalTimeBlink;
                Debug.Log("time to blink");
                outline.OutlineColor = Color.red;
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
                Debug.Log("out of time you are sus");
            }
        }

    }

    public override void Interact(GameObject player)
    {
        PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
        ResetTimers();
        playerInteract.NullInteracting();
    }

    private void ResetTimers()
    {
        timerIsRunning = true;
        timeToNextBlink = totalTimeBlink;
        pressCountdown = totalTimePress;
        outline.OutlineColor = Color.white;
        //EnableOutline();
        Debug.Log("timers reset");
    }
}
