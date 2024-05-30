using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
	public Sound[] sounds;

	public static SoundManager instance;


	void Awake()
	{
		if(instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		
		foreach (Sound s in sounds) 
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
		}
	}

	void Start()
	{
		// Play("Theme");
	}

	public void Play(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.LogWarning("Sound " + name + " not Found");
			return;
		}
		s.source.Play();

	}
}

/*	Sound Class  */

[System.Serializable]
public class Sound
{
	public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch = 1f;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
