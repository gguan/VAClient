using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMusicState : PersonPlayingState {

	private bool isPlaying = false;
	private bool isStartMusic = false;
	String musicUrl = "http://www.llss.bz/mp3/onj001.mp3";

    public PersonMusicState()
    {
		isStartMusic = false;
        stateId = StateID.MusicStateId;
    }

	public override void Act(GameObject gameObject, GameObject npc)
    {
		wSClient = gameObject.GetComponent<WSClient>();
		base.Act(gameObject,npc);
		if (isStartMusic && ! _audioSource.isPlaying)
        {
            isStartMusic = false;
            Debug.Log("结束唱歌");
            FSMSystem.Instance().PerformTransition(PersonState.Idle);
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
    }

    IEnumerator loadMusic()
    {
		try{
			if (wSClient != null && actionData != null)
			{
				WSClient.RequestData <WSClient.MusicData> musicActionData = JsonUtility.FromJson<WSClient.RequestData<WSClient.MusicData>>(actionData);
				if ("music".Equals(musicActionData.type)){
					musicUrl = musicActionData.data.url;
				}
			
			}
		}catch (Exception ex){
			isStartMusic = false;
			Debug.LogError(ex.Message);
		}
		Debug.Log("mp3url="+ actionData +"---" + musicUrl);
		WWW music = new WWW(musicUrl);
		yield return music;
		try{
            AudioClip lamusic = music.GetAudioClip
                                     (false, false, AudioType.MPEG);
            _audioSource.clip = lamusic;
            _audioSource.Play();
			isStartMusic = true;
		}catch (Exception exc){
			isStartMusic = false;
			Debug.LogError(exc.Message);
		}
        

    }

	protected override void AfterVoiceEnd()
    {
		if (!isStartMusic && wSClient!=null)
        {
            Debug.Log("开始唱歌");
            // 语音说完  播放mp3
            
			wSClient.StartCoroutine(loadMusic());
        }
    }
}
