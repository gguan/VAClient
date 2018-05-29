using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMusicState : PersonChatState {

	private bool isPlaying = false;
	private bool isStartMusic = false;
	private WSClient wSClient;
    public PersonMusicState()
    {
		isStartMusic = false;
        stateId = StateID.MusicStateId;
    }

	public override void Act(GameObject gameObject, GameObject npc)
    {
		wSClient = gameObject.GetComponent<WSClient>();
		base.Act(gameObject,npc);
		if (isStartMusic && !wSClient._audioSource.isPlaying)
        {
            isStartMusic = false;
            Debug.Log("结束唱歌");
            FSMSystem.Instance().PerformTransition(PersonState.Idle);
        }
    }

	public override void Reason(GameObject gameObject, string data)
    {
		wSClient = gameObject.GetComponent<WSClient>();
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

    IEnumerator loadMusic(AudioSource _audioSource)
    {
		String musicUrl = "http://www.llss.bz/mp3/onj001.mp3";
		try{
			if (wSClient != null && actionData != null)
			{
				WSClient.ActionData < WSClient.MusicData> musicActionData = JsonUtility.FromJson<WSClient.ActionData<WSClient.MusicData>>(actionData);
				musicUrl =	musicActionData.data.url;
			}
		}catch (Exception ex){
			isStartMusic = false;
			Debug.LogError(ex.Message);
		}
		WWW music = new WWW(musicUrl);
		yield return music;
		try{
            AudioClip lamusic = music.GetAudioClip
                                     (false, false, AudioType.MPEG);
            _audioSource.clip = lamusic;
            _audioSource.Play();
		}catch (Exception exc){
			isStartMusic = false;
			Debug.LogError(exc.Message);
		}
        

    }

	public override void AfterVoiceEnd()
    {
		if (!isStartMusic && wSClient!=null)
        {
            Debug.Log("开始唱歌");
            // 语音说完  播放mp3
            isStartMusic = true;
            WSClient.ExecuteOnMainThread.Enqueue(() => {
				wSClient.StartCoroutine(loadMusic(wSClient._audioSource));
            });
        }
    }
}
