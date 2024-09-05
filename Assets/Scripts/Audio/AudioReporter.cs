using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReporter : MonoBehaviour
{
        // Start is called before the first frame update
        [SerializeField] AudioClip[] _audios;
        [SerializeField] AudioClip[] _randomAudios; 

	public void ReportSound(int soundIndex)
	{
		if (soundIndex >= 0 && soundIndex < _audios.Length)
		{
			AudioManager.instance.PlaySound(_audios[soundIndex]);
		}
	}

	// Animation Event���� ȣ���� �Լ�
	public void PlayAnimationSound(int soundIndex)
	{
		ReportSound(soundIndex);
	}

	public void PlayRandom()
	{
		int idx = Random.Range(0, _audios.Length - 1);
		AudioManager.instance.PlaySound(_randomAudios[idx]);
	} 
}
