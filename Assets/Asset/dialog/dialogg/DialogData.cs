using System;
using UnityEngine;

[Serializable]
public class DialogueChoice {
    public string choiceText;
    public int nextLineIndex = -1; // -1 = akhir / tidak ada next
}

[Serializable]
public class DialogueLine {
    public string characterName;
    [TextArea(2,5)]
    public string dialogueText;
    public string expression; // optional, mis. "happy","angry"
    public DialogueChoice[] choices = new DialogueChoice[0];
}
