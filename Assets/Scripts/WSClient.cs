using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using Wit.BaiduAip.Speech;
using SimpleJSON;
using VirtualAssistant;

public class WSClient : MonoBehaviour
{

    public WebSocket ws;
    private FSMSystem fsm;

    private string ip = "127.0.0.1";
    private int port = 9000;
    private bool _connected = false;

    // Action queue pushed from websocket client
    public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    // Baidu TTS
    public Tts tts;

    GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        SetupServer();

        // 初始化百度TTS
        tts = new Tts();
        StartCoroutine(tts.GetAccessToken());

        guiStyle.fontSize = 20;
        guiStyle.wordWrap = true;

        MakeFSM();
    }


    private void MakeFSM()
    {
        fsm = FSMSystem.Instance();
    }

    // Update is called once per frame
    void Update()
    {
        FSMSystem.Instance().CurrentState.Act(gameObject, gameObject);
        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }
    }

    void ProcessRequest(string data)
    {
        try
        {
            RequestData<JSONObject> reqData = JsonUtility.FromJson<RequestData<JSONObject>>(data);

            ExecuteOnMainThread.Enqueue(() =>
            {
				if (fsm.CurrentStateID == StateID.SleepStateId && reqData.type != "awake")
				{
					var response = new ResponseData();
					response.message = "can not do " + reqData.type + " when unity sleep";
					response.type = "sleep";
					ws.Send(JsonUtility.ToJson(response));
					return;
				}
				else if ("server_state".Equals(reqData.type))
                {
					RequestData<ServerStateData> mSerStateData = JsonUtility.FromJson<RequestData<ServerStateData>>(data);
                    if (ServerStateData.Order_Listening.Equals(mSerStateData.data.state))
                    {
                        ControlAnim.Instance().ShowMicrophone();
                    }
                    else
                    {
                        ControlAnim.Instance().DismissMicrophone();
                    }
				}else if("stt".Equals(reqData.type)){
					RequestData<STTData> mStt = JsonUtility.FromJson<RequestData<STTData>>(data);
                    ControlAnim.Instance().ShowTips(mStt.data.text);
				}else
				{
					switch (reqData.type)
					{
						case "weather":
							fsm.PerformTransition(PersonState.ShowWeather);
							break;
						case "horoscope":
							fsm.PerformTransition(PersonState.ShowConstellation);
							break;
						case "awake":
							fsm.PerformTransition(PersonState.Awake);
							break;
						case "chat":
							fsm.PerformTransition(PersonState.Chat);
							break;
						case "music":
							fsm.PerformTransition(PersonState.Music);
							break;
						case "stop":
							fsm.PerformTransition(PersonState.Stop);
							break;
						case "sleep":
							fsm.PerformTransition(PersonState.Sleep);
							break;
					}
					fsm.CurrentState.Reason(gameObject, data);
				}
            });

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            var response = new ResponseData();
            response.message = e.Message;
            response.type = "error";
            ws.Send(JsonUtility.ToJson(response));
        }
    }


    void SetupServer()
    {
        ws = new WebSocket("ws://" + ip + ":"+ port);

        ws.OnOpen += (sender, e) =>
        {
            _connected = true;
            Debug.Log("Opended");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Received: " + e.Data);
            ProcessRequest(e.Data);
        };

        ws.OnClose += (sender, e) =>
        {
            _connected = false;
        };

        try
        {
            Debug.Log("连接websocket...");
            ws.Connect();
        }
        catch (Exception ex)
        {
            Debug.LogError("连接websocket失败: " + ex.Message);
        }

        Debug.Log("连接结果: " + _connected);

    }

    void OnApplicationQuit()
    {
        ws.Close();
    }

    void OnGUI()
    {

        if (_connected)
        {
            return;
        }

        showConnectButton();

        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 60, 150, 30), "Connect Again"))
        {
            SetupServer();
        }
    }

    void showConnectButton()
    {
        //获取输入框输入的内容  
        ip = GUI.TextField(new Rect(Screen.width / 2 - 75, Screen.height - 100, 150, 30), ip, 15);
    }
}
