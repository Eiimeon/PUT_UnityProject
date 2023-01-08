using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAndData_Manager : MonoBehaviour
{

    // ----------------------------------------------------------
    //                         SINGLETON
    // ----------------------------------------------------------

    private static MusicAndData_Manager _instance;

    public static MusicAndData_Manager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }   
        else
        {
            _instance = this;
        }
    }


    // ----------------------------------------------------------
    // ----------------------------------------------------------

    // Variables Audio
    [SerializeField] public GameObject advisors;
    [SerializeField] public GameObject emissary;
    [SerializeField] public AudioClip[] emissaries;

    //Variables DATA
    public bool isFrench;

    public IEnumerator FadeAndStopAudio(AudioSource _audioSource)
    {
        float startingVolume = _audioSource.volume;
        while (_audioSource.volume > 0.01)
        {
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, 0, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _audioSource.Stop();
        _audioSource.volume = startingVolume;
    }

    public void SetEmissary(Emissary _emissary)
    {
        emissary.GetComponent<AudioSource>().clip = _emissary.audioClip;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
