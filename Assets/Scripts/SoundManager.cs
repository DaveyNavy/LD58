using System.IO;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Attack Sounds")]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip attack4;
    public AudioClip attack5; // Corrected from "atttack5"

    [Header("Player State Sounds")]
    public AudioClip death1;
    public AudioClip death2;
    public AudioClip death3;
    public AudioClip death4;
    public AudioClip hurt1;
    public AudioClip heartbeat1;

    [Header("Movement Sounds")]
    public AudioClip walk1;
    public AudioClip walk2;

    [Header("Gameplay Action Sounds")]
    public AudioClip feed1;
    public AudioClip upgrade1;

    [Header("Cutscene & Miscellaneous")]
    public AudioClip cutscene1;
    public AudioClip misc1;
    public AudioClip misc2;
    public AudioClip liminal1;


    private AudioSource ambienceSource;
    private AudioSource sfxSource;

    void Awake()
    {
        //var soundFiles = ListSoundFiles();
        //Debug.Log(string.Join(", ", soundFiles));

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ambienceSource = gameObject.AddComponent<AudioSource>();
        ambienceSource.loop = true;
        ambienceSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        //
        PlayAmbience(liminal1, 0.2f, 0.6f);
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip != null)
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip, volume);
            sfxSource.pitch = 1f; // Reset pitch after playing
        }
    }

    public void PlayAmbience(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = true)
    {
        if (clip != null)
        {
            ambienceSource.clip = clip;
            ambienceSource.volume = volume;
            ambienceSource.pitch = pitch;
            ambienceSource.loop = loop;
            ambienceSource.Play();
        }
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
    }

    public static AudioSource PlayOnAudioSource(Transform target, AudioClip clip, bool play = true)
    {
        if (target == null || clip == null) return null;
        AudioSource source = target.gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        if (play)
            source.Play();

        return source;
    }

    public static string[] ListSoundFiles()
    {
        string soundsPath = Path.Combine(Application.dataPath, "Sounds");
        if (!Directory.Exists(soundsPath))
            return new string[0];

        return Directory.GetFiles(soundsPath)
            .Select(Path.GetFileName)
            .ToArray();
    }
}
