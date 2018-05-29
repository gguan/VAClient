using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using Wit.BaiduAip.Speech;
using SimpleJSON;

public class WSClient : MonoBehaviour
{

	[Serializable]
	public class ActionData<T>
	{
		public string type;
		public string message;
		public string action;
		public string emotion;
		public T data;
		public string[] options;
	}

	[Serializable]
	public class ResponseData
	{
		public int type;
		public string message;
		public JSONObject data;
	}


	[Serializable]
	public class ServerStateData{
		public static string Order_Listening ="order_listening";
		public string state;
	}

	[Serializable]
    public class STTData
    {
        public string text ;
    }

	[Serializable]
	public class MusicData{
		public string id;
		public string name;
		public string url;
	}


	public WebSocket ws;  
	private FSMSystem fSM;
 
	private string ip = "127.0.0.1";

	private bool _connected = false;

	bool _onCmd = false;
    string _cmd = "";
	public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

	// Baidu AIP
    const string APIKey = "NgT1OZT4jTdTZizBgPvWkVnB";
    const string SecretKey = "e7766ab06a495cf0ddba6598efb376af";
	public Tts _asr;
    
	public AudioSource _audioSource;
    public Animator _animator;
    public AnimationClip[] _animClips;
     
	GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start () {
          
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
	void Update () {

		FSMSystem.Instance().CurrentState.Act(gameObject,gameObject);
		while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }
	}

	void ProcessCmd(string data)
    {
		try
		{
			ActionData<JSONObject> action = JsonUtility.FromJson<ActionData<JSONObject>>(data);

			ExecuteOnMainThread.Enqueue(() => {
				switch (action.type)
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
                    case "server_state":
                        ActionData<ServerStateData> mServerStateData = JsonUtility.FromJson<ActionData<ServerStateData>>(data);
						if(ServerStateData.Order_Listening.Equals(mServerStateData.data.state)){
                          ControlAnim.Instance().ShowMicrophone();
                        }else{
                          ControlAnim.Instance().DismissMicrophone();
                        }
                        break;
                    case "stt":
						ActionData<STTData> mStt = JsonUtility.FromJson<ActionData<STTData>>(data);
						ControlAnim.Instance().ShowTips(mStt.data.text);
                        break;
                }
				fSM.CurrentState.Reason(gameObject, data);
            });
		
		}
        catch (Exception e)
        {
            Debug.LogError(e.Message);
			var response = new ResponseData();
			response.message = e.Message;
            response.type = 0;
            ws.Send(JsonUtility.ToJson(response));
        }      
    }


	void SetupServer()
    {
		Debug.Log("连接websocket");
		ws = new WebSocket("ws://"+ip+":9000");
        // Event at connection start.
        ws.OnOpen += (sender, e) =>
        {
			_connected = true;
            Debug.Log("Opended");
        };
        // Event when receiving a message.
        ws.OnMessage += (sender, e) =>
        {
			Debug.Log("Received: " + e.Data);
            ProcessCmd(e.Data);         
        };

		ws.OnClose += (sender, e) => {
			_connected = false;
        };

		try
		{
			// Connection.
			ws.Connect();         
		}
		catch (Exception ex)
        {
            Debug.LogError("连接websocket失败: " + ex.Message);
        }

		Debug.Log("结果" + _connected);

    }

	void OnApplicationQuit()
    {
        ws.Close();
    }

	void OnGUI()
    {
        showText(); //语音文字显示
        showOptionText(); //右下角 操作说明

        if (_connected)
        {
            return;
        }

        showConnectEdit();

        if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 60, 150, 30), "Connect Again"))
        {
            SetupServer();
        }
    }

    void showText()
    {
        if (!"".Equals(_cmd))
        {


            GUI.Label(new Rect(Screen.width / 2 - 240, Screen.height - 100, 480, 50), " ", guiStyle);
        }
    }

    void showConnectEdit()
    {
        //获取输入框输入的内容  
        ip = GUI.TextField(new Rect(Screen.width / 2 - 75, Screen.height - 100, 150, 30), ip, 15);
    }

	void showOptionText()
    {

        Rect rect = new Rect(Screen.width - 170, Screen.height - 100, 150, 70);

        GUI.Box(rect, "", "button");
        GUI.Label(new Rect(Screen.width - 160, Screen.height - 90, 150, 30), "显示日志 :X");
        GUI.Label(new Rect(Screen.width - 160, Screen.height - 60, 150, 30), "关闭声音 :L");

    }

     
}
