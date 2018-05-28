using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Wit.BaiduAip.Speech;
using SimpleJSON;



public class HumanState
{

}
abstract class PersonChatState : FSMState
{

    public PersonChatState()
    {
        stateId = StateID.MusicStateId;
    }

    public override void Act(GameObject wsClient, GameObject npc)
    {
		
    }
   
    public override void Reason(GameObject wsClient, string data)
    {
		WSClient wS = wsClient.GetComponent<WSClient>();
		if (!wS._audioSource.isPlaying){
			FSMSystem.Instance().PerformTransition(PersonState.Idle);
		}
    }


    public override void DoBeforeEntering()
    {
		// 进入说话状态时  麦克风图标 消失
        ControlAnim.Instance().DismissMicrophone();
    }

    public override void DoBeforeLeaving()
	{ 
		
    }
}


internal class PersonSleepState : PersonChatState
{
	public PersonSleepState()
    {
		stateId = StateID.SleepStateId;
    }

	public override void Act(GameObject wsClient, GameObject npc)
    {
        //Debug.Log("log_stateId_Act" + stateId.ToString());
    }

	public override void Reason(GameObject wsClient , string data)
    {
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
    }

	public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
    }


     
}

internal class PersonMusicState : PersonChatState
{
	private bool isPlaying = false;
	public PersonMusicState(){
		stateId = StateID.MusicStateId;
	}

	public override void Act(GameObject wsClient, GameObject npc)
	{

		 WSClient wS = wsClient.GetComponent<WSClient>();;
		Debug.Log("检测是否可以唱歌唱歌");
		if (!wS._audioSource.isPlaying)
		{
            Debug.Log("开始唱歌");
			// 语音说完  播放mp3
			wS.StartCoroutine(loadMusic(wS._audioSource));
		}

		if (!isPlaying && !wS._audioSource.isPlaying)
		{
			Debug.Log("唱歌完成");
			FSMSystem.Instance().PerformTransition(PersonState.Idle);

		}
	}

	public override void Reason(GameObject wsClient,  string data){
		
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
        WWW music = new WWW("http://m10.music.126.net/20180528174954/7b54fbd7abe9f9893ddcc805781b9f3f/ymusic/fc6d/0a5c/db75/2b16355237283456b93e133c2d8ad6ef.mp3");
        yield return music;

        AudioClip lamusic = music.GetAudioClip
                                 (false, false, AudioType.MPEG);
        _audioSource.clip = lamusic;
        _audioSource.Play();
        isPlaying = true;
    }

}

internal class PersonIdleState : FSMState
{
	public PersonIdleState()
    {
		stateId = StateID.IdleStateId;
    }

    public override void Act(GameObject wsClient, GameObject npc)
    {
    }

	public override void Reason(GameObject wsClient, string data)
    {
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
		ControlAnim.Instance().ShowMicrophone();
    }

	public override void DoBeforeLeaving()
	{
		base.DoBeforeLeaving();
	}
}


internal class PersonAwakeState : PersonChatState
{
	public PersonAwakeState()
    {
		stateId = StateID.AwakeStateId;
    }

    public override void Act(GameObject wsClient, GameObject npc)
    {
    }

    public override void Reason(GameObject wsClient, string data)
    {
		base.Reason(wsClient, data);
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
    }

    public override void DoBeforeLeaving()
    {
		base.DoBeforeLeaving();
    }
}

internal class PersonStopState : PersonChatState
{
	public PersonStopState()
    {
		stateId = StateID.StopStateId;
    }

	public override void Act(GameObject wsClient, GameObject npc)
    {
    }

	public override void Reason(GameObject wsClient, string data)
    {
		base.Reason(wsClient, data);
    }

    public override void DoBeforeEntering()
    {
		base.DoBeforeEntering();
    }

	public override void DoBeforeLeaving()
	{
		base.DoBeforeLeaving();
	}
}


internal class PersonShowWeatherState :PersonChatState{

	public PersonShowWeatherState(){
		stateId = StateID.ShowWeatherStateId;
	}

	public override void Act(GameObject wsClient, GameObject npc)
	{
		
	}

	public override void Reason(GameObject wsClient, string data)
	{
		base.Reason(wsClient, data);
	}

	public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
		ControlAnim.Instance().ShowWeather();
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
		ControlAnim.Instance().DissmissWeather();
    }
}


internal class PersonShowConstellationState :PersonChatState {
	public PersonShowConstellationState()
    {
		stateId = StateID.ShowConstellationStateId;
    }

	public override void Act(GameObject wsClient, GameObject npc)
    {
       
    }

	public override void Reason(GameObject wsClient, string data)
    {
		base.Reason(wsClient, data);
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
		ControlAnim.Instance().ShowConstellation();
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
		ControlAnim.Instance().DismissConstellation();
    }
}
