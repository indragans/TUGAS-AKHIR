using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transisi : MonoBehaviour
{
    public string nextSceneName;          // ← isi di Inspector
    public DialogController dialog;       // ← drag script dialog

    [SerializeField] private GameObject awalTransisi;
    [SerializeField] private GameObject akhirTransisi;

    private void Start()
    {
        awalTransisi.SetActive(true);
        StartCoroutine(HideAwalTransisi());
    }

    IEnumerator HideAwalTransisi()
    {
        yield return new WaitForSeconds(5f);
        awalTransisi.SetActive(false);
    }

    private void Update()
    {
        // Tunggu sampai dialog selesai
        if (!dialog.dialogFinished)
            return;

        // Baru bisa exit
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ExitTransisi());
        }
    }

    IEnumerator ExitTransisi()
    {
        akhirTransisi.SetActive(true);
        yield return new WaitForSeconds(1f);

        // Load scene berdasarkan input inspector
        SceneManager.LoadScene(nextSceneName);
    }
}
