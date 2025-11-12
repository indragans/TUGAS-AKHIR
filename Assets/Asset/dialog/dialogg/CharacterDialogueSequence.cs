using UnityEngine;
using System.Collections;

public class CharacterDialogueSequence : MonoBehaviour
{
    public GameObject dialogueBubblePrefab;
    public Transform bubbleSpawnPoint;

    [TextArea(2, 5)]
    public string[] dialogueLines; // teks per kalimat

    public float delayBetweenBubbles = 0.5f; // jeda antar bubble

    void Start()
    {
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        foreach (string line in dialogueLines)
        {
            GameObject bubble = Instantiate(dialogueBubblePrefab, bubbleSpawnPoint.position, Quaternion.identity);
            bubble.GetComponent<DialogueBubble>().Show(line);

            // tunggu sampai bubble hampir selesai sebelum lanjut
            yield return new WaitForSeconds(bubble.GetComponent<DialogueBubble>().showDuration + delayBetweenBubbles);
        }
    }
}
