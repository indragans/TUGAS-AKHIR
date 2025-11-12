using System;
using System.Collections;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Graphs;
using TMPro;
using UnityEngine;
using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Databases;

public class DialoguePlayer : Singleton<DialoguePlayer>
{
    private DialogueController _ctrl;

    [SerializeField] private GameObject dialoguePanel1;
    [SerializeField] private GameObject dialoguePanel2;
    [SerializeField] private TextMeshProUGUI actorName1;
    [SerializeField] private TextMeshProUGUI actorName2;
    [SerializeField] private TextMeshProUGUI lines1;
    [SerializeField] private TextMeshProUGUI lines2;

    private bool dialogueIsPlaying;
    private bool waitingForInput;

    private Dictionary<string, int> actorPanelMap = new Dictionary<string, int>();
    private int currentPanelIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        var database = new DatabaseInstance();
        _ctrl = new DialogueController(database);

        dialoguePanel1.SetActive(false);
        dialoguePanel2.SetActive(false);
    }

    public void PlayDialogue(DialogueGraph dialogue, Action callback = null)
    {
        actorPanelMap.Clear();
        currentPanelIndex = 0;
        dialoguePanel1.SetActive(false);
        dialoguePanel2.SetActive(false);

        _ctrl.Events.SpeakWithAudio.AddListener((actor, text, audioClip) =>
        {
            string name = actor.DisplayName;

            int panelIndex = GetOrAssignPanelIndexForActor(name);
            ShowPanel(panelIndex);

            if (panelIndex == 1)
            {
                actorName1.text = name;
                lines1.text = text;
            }
            else
            {
                actorName2.text = name;
                lines2.text = text;
            }

            dialogueIsPlaying = true;
            waitingForInput = true;
        });

        _ctrl.Events.End.AddListener(() =>
        {
            StartCoroutine(EndDialogue(callback));
        });

        _ctrl.Play(dialogue);
    }

    private int GetOrAssignPanelIndexForActor(string actorName)
    {
        if (actorPanelMap.TryGetValue(actorName, out int existing))
        {
            return existing;
        }

        if (!actorPanelMap.ContainsValue(1))
        {
            actorPanelMap[actorName] = 1;
            return 1;
        }
        if (!actorPanelMap.ContainsValue(2))
        {
            actorPanelMap[actorName] = 2;
            return 2;
        }

        int victimPanel = (currentPanelIndex == 1) ? 2 : 1;

        string keyToRemove = null;
        foreach (var kv in actorPanelMap)
        {
            if (kv.Value == victimPanel)
            {
                keyToRemove = kv.Key;
                break;
            }
        }
        if (keyToRemove != null)
        {
            actorPanelMap.Remove(keyToRemove);
        }

        actorPanelMap[actorName] = victimPanel;
        return victimPanel;
    }

    private void ShowPanel(int panelIndex)
    {
        if (panelIndex == 1)
        {
            dialoguePanel1.SetActive(true);
            dialoguePanel2.SetActive(false);
        }
        else
        {
            dialoguePanel1.SetActive(false);
            dialoguePanel2.SetActive(true);
        }

        currentPanelIndex = panelIndex;
    }

    private void Update()
    {
        if (_ctrl != null) _ctrl.Tick();

        if (dialogueIsPlaying && waitingForInput && Input.GetKeyDown(KeyCode.Space))
        {
            waitingForInput = false;
            StartCoroutine(NextDialogue());
        }
    }

    private IEnumerator NextDialogue()
    {
        yield return null;
        _ctrl.Next();
    }

    private IEnumerator EndDialogue(Action action)
    {
        yield return null;

        dialoguePanel1.SetActive(false);
        dialoguePanel2.SetActive(false);

        Debug.Log("end dialogue");
        _ctrl.Events.SpeakWithAudio.RemoveAllListeners();
        _ctrl.Events.End.RemoveAllListeners();
        dialogueIsPlaying = false;
        waitingForInput = false;
        actorPanelMap.Clear();
        currentPanelIndex = 0;
        action?.Invoke();
    }

    public void ForceStop()
    {
        if (!dialogueIsPlaying) return;
        _ctrl.Stop();

        dialoguePanel1.SetActive(false);
        dialoguePanel2.SetActive(false);

        _ctrl.Events.SpeakWithAudio.RemoveAllListeners();
        _ctrl.Events.End.RemoveAllListeners();
        dialogueIsPlaying = false;
        waitingForInput = false;
        actorPanelMap.Clear();
        currentPanelIndex = 0;
    }
}