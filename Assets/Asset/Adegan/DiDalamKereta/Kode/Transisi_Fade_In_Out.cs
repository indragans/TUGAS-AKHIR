using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Transisi_Fade_In_Out : MonoBehaviour
{
    public string nextSceneName;              // nama scene tujuan
    [SerializeField] private GameObject fadeObj; // objek panel fade / animasi

    // Fungsi yang bisa dipanggil dari script lain
    public void StartTransition()
    {
        StartCoroutine(DoTransition());
    }

    private IEnumerator DoTransition()
    {
        if (fadeObj != null)
            fadeObj.SetActive(true);

        // Durasi fade (sesuaikan)
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(nextSceneName);
    }
}
