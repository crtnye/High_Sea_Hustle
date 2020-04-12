using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour
{

    public AudioClip buttonClick;

    private Button button {  get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } }

    string outputMixer = "Sound Effects";

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = buttonClick;
        source.playOnAwake = false;

        AudioMixer mixer = Resources.Load("MasterMixer") as AudioMixer;
        source.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/Sound Effects")[0];

        button.onClick.AddListener(() => PlaySound());

    }

    void PlaySound()
    {
        source.PlayOneShot(buttonClick);
    }

    public void PlaySoundOneShot()
    {
        PlaySound();
    }
}
