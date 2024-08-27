using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    // Start is called before the first frame update\-8
    AudioSource _audioSource;

	[Range(0.0f, 1.0f)]
	public float _sfxVolum = 1.0f;
	public override void Awake()
	{
		base.Awake();

		_audioSource = gameObject.AddComponent<AudioSource>();
	}
	public void PlaySound(AudioClip audio)
	{ 
		_audioSource.PlayOneShot(audio, _sfxVolum);
	}
}
