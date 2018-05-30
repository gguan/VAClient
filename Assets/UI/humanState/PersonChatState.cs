using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class PersonChatState :FSMState {

    protected bool _startPlaying;
    protected string actionData;
	protected WSClient wSClient;
	protected AudioSource _audioSource;
	protected Animator _animator;
    public PersonChatState()
    {
        stateId = StateID.ChatStateId;
        _startPlaying = false;
		_animator = GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>();
		_audioSource = GameObject.Find("SD_unitychan_humanoid").GetComponent<AudioSource>();
    }

    public override void Act(GameObject gameObject, GameObject npc)
    {

        WSClient wS = gameObject.GetComponent<WSClient>();

        if (_startPlaying)
        {
            if (! _audioSource.isPlaying)
            {
                _startPlaying = false;
				_animator.CrossFade("stand", 0.25f);
				_animator.CrossFade("default", 0.15f);
                //ws.Send(System.Text.Encoding.UTF8.GetBytes("Duration: " + _audioSource.clip.length + "s"));
                var response = new WSClient.ResponseData();
                response.message = "Duration: " +  _audioSource.clip.length + "s";
                response.type = "info";
                wS.ws.Send(JsonUtility.ToJson(response));
				Debug.Log("播放完毕");
                ControlAnim.Instance().ShowTips("");
                AfterVoiceEnd();
            }
        }
    }

    public override void Reason(GameObject gameObject, string data)
    {
		wSClient = gameObject.GetComponent<WSClient>();
        actionData = data;
        try
        {
			WSClient.RequestData<JSONObject> action = JsonUtility.FromJson<WSClient.RequestData<JSONObject>>(data);

			{
                GameObject gameUI = GameObject.Find("ui");
                AddBubbleList add = gameUI.GetComponent<AddBubbleList>();
                if (action.options != null && action.options.Length > 0)
                {
                    add.createNewBubble(action.options);
                }
                else
                {
                    add.createNewBubble(new string[0] { });
                }
            }

            if (action.message != null && !action.message.Equals(""))
            {
					wSClient.StartCoroutine(wSClient._asr.Synthesis(action.message.Trim(), s =>
                    {

                        if (s.Success)
                        {

							_audioSource.clip = s.clip;
							_audioSource.Play();
							_animator.SetLayerWeight(1, 1);
                            //_animator.CrossFade("angry@sd_hmd", 0.1f);
                            //_animator.CrossFade("Walking@loop", 0.2f);
                            //_animator.SetBool("thinking_00", true);
                            if (action.emotion != null)
                            {
								 _animator.CrossFade(action.emotion, 0.1f);
                            }
                            if (action.action != null)
                            {
								 _animator.CrossFade(action.action, 0.25f);
                            }

                            _startPlaying = true;
							Debug.Log("合成成功，正在播放，共" +  _audioSource.clip.length + "s");

                            DoAnimtor();
                            ControlAnim.Instance().ShowTips(action.message);

                        }
                        else
                        {
							FeedBackError(s.err_msg,GetErrorStateType());
                        }
                    }));
                }
        }
        catch (Exception e)
        {
			FeedBackError(e.Message,GetErrorStateType());
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
        FSMSystem.Instance().PerformTransition(PersonState.Idle);
    }

	protected virtual void FeedBackError(string error,string errorType ){
		Debug.LogError(error);
        var response = new WSClient.ResponseData();
		response.message = error;
		response.type = errorType;
		wSClient.ws.Send(JsonUtility.ToJson(response));
	}

	protected virtual void FeedBackState(string stateMessage, string stateType )
    {
		
		var response = new WSClient.ResponseData();
		response.message = stateMessage;
		response.type = stateType;
		GameObject.Find("Network").GetComponent<WSClient>().ws.Send(JsonUtility.ToJson(response));
    }
}
