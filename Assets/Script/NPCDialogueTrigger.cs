using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.Dialogues.Graphs;

public class NPCDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] private DialogueGraph dialogue;
    [SerializeField] private GameObject popupUI;

    private bool playerInRange = false;
    private bool dialogueStarted = false;

    private void Start()
    {
        if (popupUI != null)
            popupUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !dialogueStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        dialogueStarted = true;
        if (popupUI != null)
            popupUI.SetActive(false);

        DialoguePlayer.Instance.PlayDialogue(dialogue, () =>
        {
            dialogueStarted = false;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (!dialogueStarted && popupUI != null)
                popupUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (popupUI != null)
                popupUI.SetActive(false);
        }
    }
}