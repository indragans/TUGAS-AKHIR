using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public Text characterNameText;
    public Text dialogueText;
    public GameObject choicesPanel;
    public Button choiceButtonPrefab;

    [Header("Test Data (optional)")]
    public DialogueLine[] lines; // assign dari inspector untuk testing

    private int currentLine = 0;

    void Start()
    {
        // jika sudah ada lines di inspector, mulai dialog otomatis (opsional)
        if (lines != null && lines.Length > 0)
            StartDialogue(lines);
    }

    public void StartDialogue(DialogueLine[] newLines)
    {
        if (newLines == null || newLines.Length == 0) return;
        lines = newLines;
        currentLine = 0;
        ShowLine();
    }

    void ShowLine()
    {
        if (lines == null || lines.Length == 0) return;
        if (currentLine < 0 || currentLine >= lines.Length) {
            EndDialogue();
            return;
        }

        var line = lines[currentLine];

        // UI
        if (characterNameText != null) characterNameText.text = line.characterName ?? "";
        if (dialogueText != null) dialogueText.text = line.dialogueText ?? "";

        // Clear old choices
        if (choicesPanel != null) {
            foreach (Transform t in choicesPanel.transform) Destroy(t.gameObject);
        }

        // Show choices if ada
        if (line.choices != null && line.choices.Length > 0 && choicesPanel != null && choiceButtonPrefab != null)
        {
            choicesPanel.SetActive(true);
            for (int i = 0; i < line.choices.Length; i++)
            {
                var choice = line.choices[i];
                var btn = Instantiate(choiceButtonPrefab, choicesPanel.transform);
                var txt = btn.GetComponentInChildren<Text>();
                if (txt != null) txt.text = choice.choiceText ?? "Choice";
                int nextIndex = choice.nextLineIndex;
                btn.onClick.AddListener(() => OnChoose(nextIndex));
            }
        }
        else
        {
            // tidak ada pilihan -> tampilkan tombol lanjut (bisa pakai input keyboard)
            if (choicesPanel != null) choicesPanel.SetActive(false);
        }

        // TODO: panggil animasi/ekspresi di sini jika perlu, mis:
        // animator.SetTrigger(line.expression);
    }

    // contohnya dipanggil dari tombol "Next" di UI
    public void OnNextLine()
    {
        // hanya lanjut jika baris sekarang tidak punya pilihan
        var current = lines != null && currentLine >= 0 && currentLine < lines.Length ? lines[currentLine] : null;
        if (current != null && current.choices != null && current.choices.Length > 0) return;

        currentLine++;
        ShowLine();
    }

    void OnChoose(int nextIndex)
    {
        if (nextIndex < 0) {
            EndDialogue();
            return;
        }
        currentLine = nextIndex;
        ShowLine();
    }

    void EndDialogue()
    {
        // aksi saat dialog selesai
        Debug.Log("Dialogue ended.");
        // contoh: hide UI
        if (characterNameText != null) characterNameText.text = "";
        if (dialogueText != null) dialogueText.text = "";
        if (choicesPanel != null) choicesPanel.SetActive(false);
    }
}
