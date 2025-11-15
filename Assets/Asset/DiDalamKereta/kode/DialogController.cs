using System.Collections;
using UnityEngine;
using TMPro;

public class DialogFancy3D : MonoBehaviour
{
    public TextMeshPro dialogText;      // TMP 3D
    public string[] sentences;
    public float typingSpeed = 0.03f;

    public AudioSource audioSource;     
    public AudioClip typeSound;

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // Wave effect
    public float amplitude = 0.1f;
    public float frequency = 6f;

    void Start()
    {
        ShowSentence();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
                FinishTyping();
            else
                NextSentence();
        }

        ApplyWaveEffect(); // terus jalan, baik typing maupun selesai
    }

    void ShowSentence()
    {
        if (index >= sentences.Length)
        {
            Debug.Log("Dialog selesai");
            return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogText.text = "";
        typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        for (int i = 0; i < sentence.Length; i++)
        {
            dialogText.text += sentence[i];

            if (typeSound != null && audioSource != null)
                audioSource.PlayOneShot(typeSound);

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void FinishTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogText.text = sentences[index];
        isTyping = false;
    }

    void NextSentence()
    {
        index++;
        ShowSentence();
    }

    void ApplyWaveEffect()
    {
        dialogText.ForceMeshUpdate();
        TMP_TextInfo textInfo = dialogText.textInfo;
        float time = Time.time;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;

            float wave = Mathf.Sin(time * frequency + i * 0.3f) * amplitude;

            vertices[vertexIndex + 0].y += wave;
            vertices[vertexIndex + 1].y += wave;
            vertices[vertexIndex + 2].y += wave;
            vertices[vertexIndex + 3].y += wave;

            textInfo.meshInfo[meshIndex].vertices = vertices;
        }

        // Apply ke TMP 3D
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            dialogText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
