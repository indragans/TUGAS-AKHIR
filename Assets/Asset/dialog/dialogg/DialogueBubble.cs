using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueBubble : MonoBehaviour
{
    public TMP_Text dialogueText;
    public float showDuration = 2.5f; // berapa lama bubble ditampilkan

    public void Show(string text)
    {
        dialogueText.text = text;
        StartCoroutine(AutoHide());
    }

    IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(showDuration);
        Destroy(gameObject);
    }
}
