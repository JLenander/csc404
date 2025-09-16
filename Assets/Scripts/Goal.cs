using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public AudioSource normal;
    public AudioSource scary;
    public GameObject redLight;
    public GameObject flashLight;
    public GameObject defaultLight;
    public GameObject alarm;
    public float interval = 1f;
    private bool emergency;
    private Coroutine toggleCoroutine;
    private HUDController hUDController;

    void Start()
    {
        emergency = false;
        scary.enabled = false;
        redLight.SetActive(false);
        flashLight.SetActive(false);
        alarm.SetActive(false);
        hUDController = FindFirstObjectByType<HUDController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            Debug.Log("Hand entered trigger: " + other.name);
            EmergencyEvent();
        }
    }

    void EmergencyEvent()
    {
        hUDController.playText();
        emergency = true;
        scary.enabled = true;
        normal.enabled = false;
        redLight.SetActive(true);
        defaultLight.SetActive(false);
        toggleCoroutine = StartCoroutine(ToggleLoop());
        StartCoroutine(ToggleSoundLoop());
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

    IEnumerator ToggleSoundLoop()
    {
        yield return new WaitForSeconds(2);
        alarm.SetActive(false);
    }

    public void StopEmergency()
    {
        hUDController.playText();
        emergency = false;
        scary.enabled = false;
        normal.enabled = true;

        defaultLight.SetActive(true);
        redLight.SetActive(false);
        flashLight.SetActive(false);

        if (toggleCoroutine != null)
            StopCoroutine(toggleCoroutine);
    }
}
