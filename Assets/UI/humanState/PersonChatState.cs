using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class PersonChatState :FSMState {

    protected bool _startPlaying;
    protected string actionData;


    public PersonChatState()
    {
        stateId = StateID.ChatStateId;
        _startPlaying = false;
    }

    public override void Act(GameObject gameObject, GameObject npc)
    {

        WSClient wS = gameObject.GetComponent<WSClient>();
        if (_startPlaying)
        {
            if (!wS._audioSource.isPlaying)
            {
                _startPlaying = false;
                wS._animator.CrossFade("stand", 0.25f);
                wS._animator.CrossFade("default", 0.15f);
                //ws.Send(System.Text.Encoding.UTF8.GetBytes("Duration: " + _audioSource.clip.length + "s"));
                var response = new WSClient.ResponseData();
                response.message = "Duration: " + wS._audioSource.clip.length + "s";
                response.type = 0;
                wS.ws.Send(JsonUtility.ToJson(response));
                Debug.Log("播放完毕");
                ControlAnim.Instance().ShowTips("");
                AfterVoiceEnd();
            }
        }
    }

    public override void Reason(GameObject gameObject, string data)
    {
        WSClient wS = gameObject.GetComponent<WSClient>();
        actionData = data;
        try
        {
            WSClient.ActionData<JSONObject> action = JsonUtility.FromJson<WSClient.ActionData<JSONObject>>(data);
            if (action.message != null && !action.message.Equals(""))
            {
                WSClient.ExecuteOnMainThread.Enqueue(() => {
                    wS.StartCoroutine(wS._asr.Synthesis(action.message.Trim(), s =>
                    {

                        if (s.Success)
                        {

                            wS._audioSource.clip = s.clip;
                            wS._audioSource.Play();
                            wS._animator.SetLayerWeight(1, 1);
                            //_animator.CrossFade("angry@sd_hmd", 0.1f);
                            //_animator.CrossFade("Walking@loop", 0.2f);
                            //_animator.SetBool("thinking_00", true);
                            if (action.emotion != null)
                            {
                                wS._animator.CrossFade(action.emotion, 0.1f);
                            }
                            if (action.action != null)
                            {
                                wS._animator.CrossFade(action.action, 0.25f);
                            }

                            _startPlaying = true;
                            Debug.Log("合成成功，正在播放，共" + wS._audioSource.clip.length + "s");

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

                            DoAnimtor();

                            ControlAnim.Instance().ShowTips(action.message);

                            var response = new WSClient.ResponseData();
                            response.message = "Data received";
                            response.type = 1;
                            wS.ws.Send(JsonUtility.ToJson(response));
                        }
                        else
                        {
                            Debug.LogError(s.err_msg);
                            var response = new WSClient.ResponseData();
                            response.message = s.err_msg;
                            response.type = 0;
                            wS.ws.Send(JsonUtility.ToJson(response));
                        }
                    }));
                });
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            var response = new WSClient.ResponseData();
            response.message = e.Message;
            response.type = 0;
            wS.ws.Send(JsonUtility.ToJson(response));
        }
    }

    public virtual void DoAnimtor()
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

    public virtual void AfterVoiceEnd()
    {
        FSMSystem.Instance().PerformTransition(PersonState.Idle);
    }
}
