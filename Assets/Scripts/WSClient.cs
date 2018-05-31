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

    //private string ip = "127.0.0.1";
	private string ip = "192.168.31.10";
    private int port = 9000;
    private bool connected = false;

    public WebSocket ws;

    private FSMSystem fsm;

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
            ExecuteOnMainThread.Enqueue(() =>
            {

             
			var reqType = JSON.Parse(data)["type"].Value;
			var jSONArray =JSON.Parse(data)["options"].AsArray;

			string[] options = new string[jSONArray.Count];
			for (int i = 0; i < jSONArray.Count;i++){
				options[i] = jSONArray[i].Value;
			}
		  
            {        // 该代码 不属于任何 状态 有就执行
                GameObject gameUI = GameObject.Find("ui");
                AddBubbleList add = gameUI.GetComponent<AddBubbleList>();
			    if(add!=null){
				if (options != null && options.Length > 0)
                    {
                        add.createNewBubble( options);
                    }
                    else
                    {
                        add.createNewBubble(new string[0] { });
                    }
			    }
            }

                //如果当前状态 是Sleep的话 只能接受awake 状态
			if (FSMSystem.Instance().CurrentStateID == StateID.SleepStateId &&  reqType != "awake")
            {
                var response = new ResponseData();
				response.message = "can not do " + reqType + " when unity sleep";
                response.type = "sleep";
                ws.Send(JsonUtility.ToJson(response));
                return;
            }
			else if ("server_state".Equals(reqType)) // microphone 控制指令
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
            }
			else if ("stt".Equals(reqType)) // 人说的话
            {
                RequestData<STTData> mStt = JsonUtility.FromJson<RequestData<STTData>>(data);
                ControlAnim.Instance().ShowTips(mStt.data.text);
            }
            else //状态改变
            {
				switch (reqType)
                {
                    case "weather":
                        FSMSystem.Instance().PerformTransition(PersonState.ShowWeather, data);
                        break;
                    case "horoscope":
                        FSMSystem.Instance().PerformTransition(PersonState.ShowConstellation, data);
                        break;
                    case "awake":
                        FSMSystem.Instance().PerformTransition(PersonState.Awake, data);
                        break;
                    case "chat":
                        FSMSystem.Instance().PerformTransition(PersonState.Chat, data);
                        break;
                    case "music":
                        FSMSystem.Instance().PerformTransition(PersonState.Music, data);
                        break;
                    case "stop":
                        FSMSystem.Instance().PerformTransition(PersonState.Stop, data);
                        break;
                    case "sleep":
                        FSMSystem.Instance().PerformTransition(PersonState.Sleep, data);
                        break;
                    default:
                        FSMSystem.Instance().PerformTransition(PersonState.Idle, data);
					    var response = new ResponseData();
					    response.message = "can not parse state";
						response.type = "error";
                        ws.Send(JsonUtility.ToJson(response));
                        break;
                }
            }
        });
    }


    void SetupServer()
    {
        ws = new WebSocket("ws://" + ip + ":"+ port);

        ws.OnOpen += (sender, e) =>
        {
            connected = true;
            Debug.Log("Websocket connected");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Received: " + e.Data);
            ProcessRequest(e.Data);
        };

        ws.OnClose += (sender, e) =>
        {
            connected = false;
            Debug.Log("Websocket closed");
        };

        try
        {
            Debug.Log("Connect websocket server...");
            ws.Connect();
        }
        catch (Exception ex)
        {
            Debug.LogError("Websocket failed: " + ex.Message);
        }
    }

    void OnApplicationQuit()
    {
        ws.Close();
    }

    void OnGUI()
    {

        if (connected)
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
