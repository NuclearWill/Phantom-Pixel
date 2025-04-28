using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSettings : MonoBehaviour
{
    public AudioMixer MasterMixer;
    public Slider VolumeSlider;

    public void Start()
    {
        SetVolume();
    }
    public void SetVolume()
    {
        float volume = VolumeSlider.value;
        MasterMixer.SetFloat("MasterV", Mathf.Log10(volume)*20);
    }
}
