using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicReporter : MonoBehaviour
{
	[SerializeField] List<PlayableDirector> playableDirectors;

	public void PlayClip(int idx)
	{ 
		playableDirectors[idx].Play(); 
	}
}
