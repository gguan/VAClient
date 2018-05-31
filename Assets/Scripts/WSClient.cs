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
    private FSMSystem fSM;

    private string ip = "127.0.0.1";
	//private string ip = "192.168.31.10";
	private int port = 9000;
    private bool _connected = false;

    bool _onCmd = false;
    public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    // Baidu AIP
    const string APIKey = "NgT1OZT4jTdTZizBgPvWkVnB";
    const string SecretKey = "e7766ab06a495cf0ddba6598efb376af";
    public Tts _asr;


    GUIStyle guiStyle = new GUIStyle();

    // Use this for initialization
    void Start()
    {

        SetupServer();

        // 初始化百度TTS
        _asr = new Tts(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        guiStyle.fontSize = 20;
        guiStyle.wordWrap = true;

        MakeFSM();
    }


    private void MakeFSM()
    {
        fSM = FSMSystem.Instance();
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
				if (fSM.CurrentStateID == StateID.SleepStateId && reqData.type != "awake")
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
							fSM.PerformTransition(PersonState.ShowWeather);
							break;
						case "horoscope":
							fSM.PerformTransition(PersonState.ShowConstellation);
							break;
						case "awake":
							fSM.PerformTransition(PersonState.Awake);
							break;
						case "chat":
							fSM.PerformTransition(PersonState.Chat);
							break;
						case "music":
							fSM.PerformTransition(PersonState.Music);
							break;
						case "stop":
							fSM.PerformTransition(PersonState.Stop);
							break;
						case "sleep":
							fSM.PerformTransition(PersonState.Sleep);
							break;
					}
					fSM.CurrentState.Reason(gameObject, data);
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
