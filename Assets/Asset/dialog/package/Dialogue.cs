using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject window;
    public GameObject indicator;
    public TMP_Text dialogueText;

    [Header("Dialogues")]
    public List<string> dialogues;
    public float writingSpeed = 0.02f;

    private int index = 0;          // current dialogue index
    private bool started = false;   
    private bool waitForNext = false;

    // Freeze player
    private Rigidbody2D playerRB;
    private Vector2 savedVelocity;

    public void AssignPlayer(Rigidbody2D rb)
    {
        playerRB = rb;
    }

    private void Awake()
    {
        ToggleIndicator(false);
        ToggleWindow(false);
    }

    private void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    public void StartDialogue()
    {
        if (started || dialogues.Count == 0)
            return;

        started = true;
        ToggleWindow(true);
        ToggleIndicator(false);

        // Freeze Player
        if (playerRB != null)
        {
            savedVelocity = playerRB.velocity;
            playerRB.velocity = Vector2.zero;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // jangan reset index setiap kali kecuali ingin restart
        if (index >= dialogues.Count) index = 0;

        StartCoroutine(Writing(dialogues[index]));
    }

    private IEnumerator Writing(string sentence)
    {
        dialogueText.text = "";

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(writingSpeed);
        }

        waitForNext = true; // selesai menulis kalimat
    }

    private void Update()
    {
        if (!started) return;

        if (waitForNext && Input.GetKeyDown(KeyCode.E))
        {
            waitForNext = false;
            index++;

            if (index < dialogues.Count)
            {
                StartCoroutine(Writing(dialogues[index]));
            }
            else
            {
                EndDialogue();
                // Jangan reset index disini supaya object berikutnya mulai dari awal
            }
        }
    }

    public void EndDialogue(bool showIndicator = true)
    {
        started = false;
        waitForNext = false;
        StopAllCoroutines();
        ToggleWindow(false);
        ToggleIndicator(showIndicator);

        // Unfreeze player
        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRB.velocity = savedVelocity;
        }

        // Hanya reset index kalau object ingin dipakai lagi nanti
        // index = 0; 
    }
}
