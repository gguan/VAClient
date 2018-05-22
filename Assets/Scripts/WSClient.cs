using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using Wit.BaiduAip.Speech;
using SimpleJSON;

public class WSClient : MonoBehaviour {

	[Serializable]
    public class ActionData
    {
		public int type;
        public string message;
        public string action;
		public string emotion;
		public JSONObject data;
    }

	[Serializable]
    public class ResponseData
    {
        public int type;
        public string message;
        public JSONObject data;
    }
    
	WebSocket ws;  

	private string ip = "127.0.0.1";

	private bool _connected = false;

	bool _onCmd = false;
    string _cmd = "";
	public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

	// Baidu AIP
    const string APIKey = "NgT1OZT4jTdTZizBgPvWkVnB";
    const string SecretKey = "e7766ab06a495cf0ddba6598efb376af";
    private Tts _asr;

	public AudioSource _audioSource;
    public Animator _animator;
    public AnimationClip[] _animClips;
	bool _startPlaying;
	GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start () {
          
		SetupServer();

		_startPlaying = false;
		// 初始化百度TTS
        _asr = new Tts(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

		guiStyle.fontSize = 20;
        guiStyle.wordWrap = true;

	}
	
	// Update is called once per frame
	void Update () {
		if (_startPlaying)
        {
            if (!_audioSource.isPlaying)
            {
                _startPlaying = false;
                _animator.CrossFade("Standing@loop", 0.25f);
                _animator.CrossFade("default@sd_hmd", 0.1f);
				//ws.Send(System.Text.Encoding.UTF8.GetBytes("Duration: " + _audioSource.clip.length + "s"));
				var response = new ResponseData();
				response.message = "Duration: " + _audioSource.clip.length + "s";
				response.type = 0;
				ws.Send(JsonUtility.ToJson(response));
                Debug.Log("播放完毕");
            }
        }

		while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }
              
	}

	void ProcessCmd(string data)
    {
		try
		{
			ActionData action = JsonUtility.FromJson<ActionData>(data);
			Debug.Log(action.message);         
			ExecuteOnMainThread.Enqueue(() =>
			{
				StartCoroutine(_asr.Synthesis(action.message.Trim(), s =>
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
						Debug.Log("合成成功，正在播放，共" + _audioSource.clip.length + "s");
						var response = new ResponseData();
                        response.message = "Data received";
                        response.type = 1;
                        ws.Send(JsonUtility.ToJson(response));
					}
					else
					{
						Debug.LogError(s.err_msg);
						var response = new ResponseData();
						response.message = s.err_msg;
                        response.type = 0;
                        ws.Send(JsonUtility.ToJson(response));
					}
				}));
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
