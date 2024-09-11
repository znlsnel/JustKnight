using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicReporter : MonoBehaviour
{
	[SerializeField] List<PlayableDirector> playableDirectors;
	[SerializeField] EpisodeSO episode;
	public void PlayClip(int idx)
	{
		DialogueManager dm = UIHandler.instance._dialogue.GetComponent<DialogueManager>();
		dm.UpdateQuestDialogue(ref episode);

		if (dm.GetCurEpisode() != episode)
			return;
		 
		playableDirectors[idx].Play(); 
	}
}
