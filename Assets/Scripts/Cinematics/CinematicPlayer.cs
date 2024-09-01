using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CinematicPlayer : Singleton<CinematicPlayer>
{
	public void PlayClip(PlayableDirector playableDirector)
        {
		playableDirector.Play();  
	}
}
