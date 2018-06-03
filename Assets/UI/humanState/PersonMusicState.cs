using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAssistant;

public class PersonMusicState : PersonPlayingState {

	private bool isPlaying = false;
	private bool isStartMusic = false;
	String musicUrl = "http://www.llss.bz/mp3/onj001.mp3";

	public AudioSource _musicAudioSource;
    public PersonMusicState()
    {
		isStartMusic = false;
        stateId = StateID.MusicStateId;
		_musicAudioSource = GameObject.Find("Network").GetComponent<AudioSource>();
    }

	public override void Act(GameObject gameObject, GameObject npc)
    {
		client = gameObject.GetComponent<WSClient>();
		base.Act(gameObject,npc);
		if (isStartMusic && ! _audioSource.isPlaying)
        {
			if(!_musicAudioSource.isPlaying){
				isStartMusic = false;
                Debug.Log("结束唱歌");
                FSMSystem.Instance().PerformTransition(PersonState.Idle, "idle");
			}
        }
    }

	public override void Reason(GameObject gameObject, string data)
    {
		base.Reason(gameObject,data);
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();

    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();

		isStartMusic = false;
		_musicAudioSource.Stop();
    }

    IEnumerator loadMusic()
    {
		try{
			if (client != null && data != null)
			{
				RequestData < MusicData> musicActionData = JsonUtility.FromJson<RequestData< MusicData>>(data);
				if ("music".Equals(musicActionData.type)){
					musicUrl = musicActionData.data.url;
				}
			
			}
		}catch (Exception ex){
			isStartMusic = false;
			Debug.LogError(ex.Message);
		}
		Debug.Log("mp3url="+ data +"---" + musicUrl);
		WWW music = new WWW(musicUrl);
		yield return music;
		try{
            AudioClip lamusic = music.GetAudioClip
                                     (false, false, AudioType.MPEG);
			_musicAudioSource.clip = lamusic;
			_musicAudioSource.Play();
			isStartMusic = true;
		}catch (Exception exc){
			isStartMusic = false;
			Debug.LogError(exc.Message);
		}
        

    }

	protected override void AfterVoiceEnd()
    {
		if (!isStartMusic && client!=null)
        {
            Debug.Log("开始唱歌");
            // 语音说完  播放mp3
            
			client.StartCoroutine(loadMusic());
        }
    }
}
