using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Variables publicas
    public static AudioManager Instance;
    // Variables privadas
    [SerializeField]
    GameObject m_audioSourcePrefab;
    [SerializeField]
    AudioClip m_music;
    [SerializeField]
    List<PairAudioClipString> m_audioClips;
    private GameObject m_currentAudioSource;
    private GameObject m_currentAmbientalSource;

    //------------------------------------------------AWAKE--------------------------------------
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    //------------------------------------------------START-------------------------------------
    void Start()
    {
        m_currentAmbientalSource = Instantiate(m_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        m_currentAmbientalSource.transform.parent = this.transform;
        m_currentAmbientalSource.GetComponent<AudioSource>().clip = m_music;
        m_currentAmbientalSource.GetComponent<AudioSource>().loop = true;
        m_currentAmbientalSource.GetComponent<AudioSource>().Play();
    }

    //------------------------------------------------METHODS------------------------------------
    public void Play(string clip)
    {
        m_currentAudioSource = Instantiate(m_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        m_currentAudioSource.transform.parent = this.transform;
        m_currentAudioSource.GetComponent<AudioSource>().clip = m_audioClips.Find(e => e.StringKey == clip).AudioClip;
        m_currentAudioSource.GetComponent<AudioSource>().Play();
        m_currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }

    public void PlayHalfVolumen(string clip)
    {
        m_currentAudioSource = Instantiate(m_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        m_currentAudioSource.transform.parent = this.transform;
        m_currentAudioSource.GetComponent<AudioSource>().clip = m_audioClips.Find(e => e.StringKey == clip).AudioClip;
        m_currentAudioSource.GetComponent<AudioSource>().volume = 0.5f;
        m_currentAudioSource.GetComponent<AudioSource>().Play();
        m_currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }

    public void PlayThreeQuartersVolumen(string clip)
    {
        m_currentAudioSource = Instantiate(m_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        m_currentAudioSource.transform.parent = this.transform;
        m_currentAudioSource.GetComponent<AudioSource>().clip = m_audioClips.Find(e => e.StringKey == clip).AudioClip;
        m_currentAudioSource.GetComponent<AudioSource>().volume = 0.75f;
        m_currentAudioSource.GetComponent<AudioSource>().Play();
        m_currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }

    public void PlayCustomVolumen(string clip, float volume)
    {
        m_currentAudioSource = Instantiate(m_audioSourcePrefab, transform.position, Quaternion.identity) as GameObject;
        m_currentAudioSource.transform.parent = this.transform;
        m_currentAudioSource.GetComponent<AudioSource>().clip = m_audioClips.Find(e => e.StringKey == clip).AudioClip;
        m_currentAudioSource.GetComponent<AudioSource>().volume = volume;
        m_currentAudioSource.GetComponent<AudioSource>().Play();
        m_currentAudioSource.GetComponent<AudioElement>().CheckActivity = true;
    }
}

[Serializable]
public class PairAudioClipString
{
    [SerializeField]
    public AudioClip AudioClip
    {
        get
        {
            //Some other code
            return _audioClip;
        }
        set
        {
            //Some other code
            _audioClip = value;
        }
    }

    [SerializeField]
    public string StringKey
    {
        get
        {
            //Some other code
            return _stringKey;
        }
        set
        {
            //Some other code
            _stringKey = value;
        }
    }
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private string _stringKey;
}

