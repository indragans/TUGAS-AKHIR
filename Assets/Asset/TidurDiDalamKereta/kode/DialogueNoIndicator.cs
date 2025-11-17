using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueNoIndicator : MonoBehaviour
{
    [Header("References")]
    public GameObject window;               // Panel UI (DialogueWindow)
    public TMP_Text dialogueText;           // TextMeshPro component

    [Header("Dialogue")]
    public List<string> dialogues;
    public float writingSpeed = 0.03f;

    [Header("Floating Effect (optional)")]
    public bool useFloating = true;
    public float floatSpeed = 2f;
    public float floatAmount = 5f;

    [Header("Behavior")]
    public bool autoStart = true; // kalau mau langsung muncul di Start

    private int index = 0;
    private bool started = false;
    private bool waitForNext = false;
    private Vector2 originalTextPos;
    private Coroutine writingCoroutine;

    private void Awake()
    {
        // jangan langsung mematikan kalau mau debug; tetap set inactive sesuai desain
        if (window != null)
            window.SetActive(false);
    }

    private void Start()
    {
        if (autoStart)
        {
            StartDialogue();
        }
    }

    // Panggil untuk mulai dialog
    public void StartDialogue()
    {
        Debug.Log("[Dialogue] StartDialogue() called");
        // basic checks
        if (window == null)
        {
            Debug.LogError("[Dialogue] Window reference is NULL. Drag your DialogueWindow (Panel) into the script in Inspector.");
            return;
        }
        if (dialogueText == null)
        {
            Debug.LogError("[Dialogue] dialogueText (TMP) reference is NULL. Drag your TMP Text into the script in Inspector.");
            return;
        }
        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogError("[Dialogue] Dialogues list is empty. Add at least 1 element in Inspector.");
            return;
        }

        started = true;
        window.SetActive(true); // pastikan nyala
        dialogueText.gameObject.SetActive(true);
        originalTextPos = dialogueText.rectTransform.anchoredPosition;

        index = 0;
        StartNextDialogue();
    }

    private void StartNextDialogue()
    {
        waitForNext = false;

        if (writingCoroutine != null)
            StopCoroutine(writingCoroutine);

        if (index >= dialogues.Count)
        {
            Debug.Log("[Dialogue] Reached end of dialogues.");
            EndDialogue();
            return;
        }

        dialogueText.text = string.Empty;
        writingCoroutine = StartCoroutine(WritingCoroutine(dialogues[index]));
        Debug.Log("[Dialogue] WritingCoroutine started for index " + index);
    }

    IEnumerator WritingCoroutine(string line)
    {
        Debug.Log("[Dialogue] Writing start: \"" + (line.Length > 30 ? line.Substring(0, 30) + "..." : line) + "\"");
        int charIndex = 0;

        while (charIndex < line.Length)
        {
            // jika pemain tekan E saat typing, langsung tampilkan sisa kalimat
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueText.text = line;
                Debug.Log("[Dialogue] Typing skipped by E");
                break;
            }

            dialogueText.text += line[charIndex];
            charIndex++;
            yield return new WaitForSeconds(writingSpeed);
        }

        // pastikan seluruh teks tampil
        dialogueText.text = line;
        waitForNext = true;
        Debug.Log("[Dialogue] Sentence finished. waiting for next (press E).");
    }

    public void EndDialogue()
    {
        Debug.Log("[Dialogue] EndDialogue() called");
        started = false;
        waitForNext = false;

        if (writingCoroutine != null)
            StopCoroutine(writingCoroutine);

        // reset posisi teks
        dialogueText.rectTransform.anchoredPosition = originalTextPos;

        if (window != null)
            window.SetActive(false);
    }

    private void Update()
    {
        if (!started)
            return;

        // Floating effect (opsional)
        if (useFloating && dialogueText != null)
        {
            Vector2 newPos = originalTextPos;
            newPos.y += Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            dialogueText.rectTransform.anchoredPosition = newPos;
        }

        // Debug: lihat apakah E terdeteksi (akan muncul di Console)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[Dialogue] E pressed. started=" + started + " waitForNext=" + waitForNext + " index=" + index);
        }

        // Jika kalimat selesai dan pemain tekan E -> lanjutkan
        if (waitForNext && Input.GetKeyDown(KeyCode.E))
        {
            waitForNext = false;
            index++;
            StartNextDialogue();
        }
    }
}
