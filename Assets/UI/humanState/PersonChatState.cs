using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using VirtualAssistant;


public class PersonChatState :FSMState {

    protected bool _startPlaying;
	protected AudioSource _audioSource;
	protected Animator _animator;
	protected WSClient client;

    public PersonChatState()
    {
        stateId = StateID.ChatStateId;
        _startPlaying = false;
		_animator = GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>();
		_audioSource = GameObject.Find("SD_unitychan_humanoid").GetComponent<AudioSource>();
		client = GameObject.Find("Network").GetComponent<WSClient>();
    }

    public override void Act(GameObject gameObject, GameObject npc)
    {

        if (_startPlaying)
        {
            if (! _audioSource.isPlaying)
            {
                _startPlaying = false;
				_animator.CrossFade("stand", 0.25f);
				_animator.CrossFade("default", 0.15f);
                //ws.Send(System.Text.Encoding.UTF8.GetBytes("Duration: " + _audioSource.clip.length + "s"));
               
				Debug.Log("播放完毕");
                ControlAnim.Instance().ShowTips("");
                AfterVoiceEnd();
            }
        }
    }

 
    public override void Reason(GameObject gameObject, string data)
    {

    }

    // message 需要合成的message emotion表情 action 动作 
	protected void SpeechSynthesis(string message,string emotion ,string action){
		client.StartCoroutine(client.tts.Synthesis(message.Trim(), s =>
        {
            if (s.Success)
            {
                _audioSource.clip = s.clip;
                _audioSource.Play();
                _animator.SetLayerWeight(1, 1);
                //_animator.CrossFade("angry@sd_hmd", 0.1f);
                //_animator.CrossFade("Walking@loop", 0.2f);
                //_animator.SetBool("thinking_00", true);
				if (emotion != null)
                {
					_animator.CrossFade(emotion, 0.1f);
                }
				if (action != null)
                {
					_animator.CrossFade(action, 0.25f);
                }

                _startPlaying = true;
                Debug.Log("合成成功，正在播放，共" + _audioSource.clip.length + "s");

                DoAnimtor(); // 执行ui 伴随动画
				ControlAnim.Instance().ShowTips(message); // 显示顶端字幕

            }
            else
            {
                FeedBackError(s.err_msg, GetErrorStateType());
            }
        }));
	}

    // 已经进入该状态 处理数据
	public override void HandleData(){
		base.HandleData();
		try
        {
			if(data!=null && !"".Equals(data) && !"idle".Equals(data)){
				RequestData<JSONObject> action = JsonUtility.FromJson<RequestData<JSONObject>>(data);
                if (action.message != null && !action.message.Equals(""))
                {
					SpeechSynthesis(action.message,action.emotion,action.action ); //合成语音并播放
                }
			}
        }
        catch (Exception e)
        {
            FeedBackError(e.Message, GetErrorStateType());
        }
	}

	public virtual string GetErrorStateType(){
		return "error";
	}
       

    public virtual void DoAnimtor()
    {
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
		FeedBackState("state", "playing");
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
    }

	protected virtual void AfterVoiceEnd()
    {
        FSMSystem.Instance().PerformTransition(PersonState.Idle, "idle");
    }

	protected virtual void FeedBackError(string error,string errorType ){
		Debug.LogError(error);
        var response = new  ResponseData();
		response.message = error;
		response.type = errorType;
		client.ws.Send(JsonUtility.ToJson(response));
	}

	protected virtual void FeedBackState(string stateMessage, string stateType )
    {
		var response = new  ResponseData();
		response.message = stateMessage;
		response.type = stateType;
		GameObject.Find("Network").GetComponent<WSClient>().ws.Send(JsonUtility.ToJson(response));
    }
}
