using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogController : MonoBehaviour
{
    public bool dialogFinished = false;

    [Header("Text & Sentences")]
    public TextMeshPro dialogText;         // TextMeshPro (3D)
    public string[] sentences;
    public float typingSpeed = 0.03f;

    [Header("Sound (optional)")]
    public AudioSource audioSource;
    public AudioClip typeSound;

    [Header("Bounce (per-char)")]
    public float bounceAmplitude = 0.06f;  // tinggi loncatan (meter/units)
    public float bounceFrequency = 3f;     // frekuensi loncatan (cycles per second)

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // track waktu mulai muncul tiap karakter (index berdasarkan posisi karakter)
    private float[] charStartTimes;

    void Start()
    {
        /*audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.clip = null;*/

        // pre-alloc buffer untuk charStartTimes (maks 1000 default, akan expand jika perlu)
        charStartTimes = new float[1024];
        for (int i = 0; i < charStartTimes.Length; i++) charStartTimes[i] = -1f;

        ShowSentence();
    }

    void Update()
    {
        // Input handling
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
                FinishTyping();
            else
                NextSentence();
        }

        // Always update per-character bounce (whether typing or already finished)
        ApplyPerCharBounce();
    }

    void EnsureCharBufferCapacity(int needed)
    {
        if (needed <= charStartTimes.Length) return;
        int newSize = Mathf.NextPowerOfTwo(needed);
        float[] newArr = new float[newSize];
        for (int i = 0; i < newArr.Length; i++) newArr[i] = -1f;
        for (int i = 0; i < charStartTimes.Length; i++) newArr[i] = charStartTimes[i];
        charStartTimes = newArr;
    }

    void ShowSentence()
    {
        if (index >= sentences.Length)
        {
            Debug.Log("Dialog selesai");
            return;
        }

        // reset start times for upcoming text length
        int maxLen = Mathf.Max( (dialogText.text != null ? dialogText.text.Length : 0), sentences[index].Length );
        EnsureCharBufferCapacity(maxLen);
        for (int i = 0; i < maxLen; i++) charStartTimes[i] = -1f;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogText.text = "";
        typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
    }

   IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogText.text = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            dialogText.text += sentence[i];

            charStartTimes[i] = Time.time;

            // sound only when typing
            if (audioSource != null && typeSound != null)
            {
                audioSource.PlayOneShot(typeSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }


    void FinishTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // reveal full sentence and set start times for any characters not yet timestamped
        string s = sentences[index];
        dialogText.text = s;

        EnsureCharBufferCapacity(s.Length);
        for (int i = 0; i < s.Length; i++)
        {
            if (charStartTimes[i] < 0f) // not set yet
                charStartTimes[i] = Time.time;
        }

        isTyping = false;
    }

    void NextSentence()
    {
        index++;
        if (index < sentences.Length)
        {
            ShowSentence();
        }
        else
        {
            dialogFinished = true;
        }
    }

    void ApplyPerCharBounce()
    {
        dialogText.ForceMeshUpdate();

        TMP_TextInfo textInfo = dialogText.textInfo;
        int charCount = textInfo.characterCount;
        float time = Time.time;

        for (int i = 0; i < charCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int mIndex = charInfo.materialReferenceIndex;
            int vIndex = charInfo.vertexIndex;

            Vector3[] verts = textInfo.meshInfo[mIndex].vertices;

            float startTime = charStartTimes[i];
            if (startTime < 0f) continue;

            float elapsed = time - startTime;

            // Kaku: pingpong (atas-bawah)
            float yOffset = Mathf.PingPong(elapsed * bounceFrequency, bounceAmplitude * 2) - bounceAmplitude;

            verts[vIndex + 0].y += yOffset;
            verts[vIndex + 1].y += yOffset;
            verts[vIndex + 2].y += yOffset;
            verts[vIndex + 3].y += yOffset;
        }

        // Apply modified mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            dialogText.UpdateGeometry(meshInfo.mesh, i);
        }
    }

}
