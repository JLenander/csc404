using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaLevel1Manager : MonoBehaviour
{
    public static NovaLevel1Manager Instance;
    public Animator novaAnimator;

    public List<DialogueScriptableObj> dialogues = new List<DialogueScriptableObj>();

    public List<GameObject> cakeSlices = new List<GameObject>();

    public EvidenceSpawner evidenceSpawner;

    public bool grabbed = false;
    private float switchInterval = 10f;
    private float timer = 0f;
    private bool talking = false;
    private int cakeIndex = 0;
    public bool ate = false;

    void Start()
    {
        Instance = this;
    }

    IEnumerator EatCake()
    {
        talking = false;
        novaAnimator.SetTrigger("Eat");
        cakeSlices[cakeIndex].SetActive(false);
        cakeIndex++;
        yield return new WaitForSeconds(4f);
        talking = true;
    }

    public void PlayLevelRoutine()
    {
        StartCoroutine(LevelStart());
    }

    public IEnumerator LevelStart()
    {
        int index = 0;
        // seat nova at seat, intro dialogue
        transform.position = new Vector3(254.8f, -26.8f, 9.8f);
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        talking = true;

        // blurb about herself
        GlobalPlayerUIManager.Instance.LoadText(dialogues[index]);
        index++;
        yield return new WaitForSeconds(15f);

        // blurb about food
        GlobalPlayerUIManager.Instance.LoadText(dialogues[index]);
        index++;
        yield return new WaitForSeconds(10f);
        yield return new WaitUntil(() => ate);

        // evidence falls out
        talking = false;
        novaAnimator.SetTrigger("Bag");
        yield return new WaitForSeconds(2f);
        evidenceSpawner.SpawnTempSpecial();

        // blurb about having to get the evidence
        GlobalPlayerUIManager.Instance.LoadText(dialogues[index]);
        index++;
        talking = true;

        // she eats a slice
        yield return new WaitUntil(() => grabbed);

        GlobalPlayerUIManager.Instance.LoadText(dialogues[index]);
        index++;
        yield return new WaitForSeconds(15f);

        yield return StartCoroutine(EatCake());

        // talk about having to do it before she finishes
        GlobalPlayerUIManager.Instance.LoadText(dialogues[index]);
        index++;

        yield return new WaitForSeconds(60f);

        // after a while she eats another slice
        yield return StartCoroutine(EatCake());

        yield return new WaitForSeconds(5f);

        // // drink coffee
        // talking = false;
        // novaAnimator.SetTrigger("Eat");
        // yield return new WaitForSeconds(4f);
        // talking = true;

        // // prompt to refill the drink
        // GlobalPlayerUIManager.Instance.LoadText(dialogues[index]);
        // index++;

        // eat last slice
        yield return new WaitForSeconds(60f);

        yield return StartCoroutine(EatCake());

        GlobalPlayerUIManager.Instance.LoadText(dialogues[index]); // times up!!
        index++;

        yield return new WaitForSeconds(15f);

        GlobalPlayerUIManager.Instance.LoadText(dialogues[index]); // times up!!
        index++;
        yield return new WaitForSeconds(15f);

        ScoreboardUIHandler.Instance.ShowScoreboard();
    }

    // Update is called once per frame
    void Update()
    {
        // randomly triggers one, switches every 10 seconds
        timer += Time.deltaTime;

        if (timer >= switchInterval && talking)
        {
            timer = 0f;

            // Pick a random trigger
            int variant = Random.Range(0, 3); // 0,1,2

            // Reset all triggers first (optional, prevents overlap)
            novaAnimator.ResetTrigger("Talk 1");
            novaAnimator.ResetTrigger("Talk 2");
            novaAnimator.ResetTrigger("Talk 3");

            // Set the chosen trigger
            switch (variant)
            {
                case 0:
                    novaAnimator.SetTrigger("Talk 1");
                    break;
                case 1:
                    novaAnimator.SetTrigger("Talk 2");
                    break;
                case 2:
                    novaAnimator.SetTrigger("Talk 3");
                    break;
            }
        }
    }
}
