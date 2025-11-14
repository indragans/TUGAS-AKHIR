using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource bgmSource;      // drag Audio Source kamu di sini

    [Header("Slider (opsional)")]
    public Slider volumeSlider;        // kalau mau pakai UI Slider

    private void Start()
    {
        // Set default volume
        if (bgmSource != null)
            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 100f);

        if (volumeSlider != null)
        {
            volumeSlider.value = bgmSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;

        PlayerPrefs.SetFloat("BGMVolume", value); // simpan volume
    }
}
