using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using Wit.BaiduAip.Speech;

public class KalliopeClient : MonoBehaviour
{

	private Socket _clientSocket;
	private byte[] _receiveBuffer = new byte[1024];
	private bool _connected = false;

	private string ip = "127.0.0.1";
	// TODO
	bool _onCmd = false;
	string _cmd = "";

	// Baidu AIP
	const string APIKey = "NgT1OZT4jTdTZizBgPvWkVnB";
	const string SecretKey = "e7766ab06a495cf0ddba6598efb376af";
	private Tts _asr;

	public AudioSource _audioSource;
	bool _startPlaying;

	GUIStyle guiStyle = new GUIStyle();

	// Use this for initialization
	void Start()
	{
		_startPlaying = false;

		// 初始化百度TTS
		_asr = new Tts(APIKey, SecretKey);
		StartCoroutine(_asr.GetAccessToken());
		// 连接Kalliope
		SetupServer();
		// Check connection every 10 seconds
		StartCoroutine("CheckSocket");


		guiStyle.fontSize = 20;
		guiStyle.wordWrap = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (_onCmd)
		{
			ProcessCmd(_cmd);
			_onCmd = false;
		}

		if (_startPlaying)
		{
			if (!_audioSource.isPlaying)
			{
				_startPlaying = false;
				SendData(System.Text.Encoding.UTF8.GetBytes("Duration: " + _audioSource.clip.length + "s"));
				Debug.Log("播放完毕");
			}
		}

	}

	void ProcessCmd(string data)
	{
		Debug.Log(data);
		StartCoroutine(_asr.Synthesis(data.Trim(), s =>
		{
			if (s.Success)
			{
				_audioSource.clip = s.clip;
				_audioSource.Play();
				_startPlaying = true;
				Debug.Log("合成成功，正在播放，共" + _audioSource.clip.length + "s");
			}
			else
			{
				Debug.LogError(s.err_msg);
			}
		}));
	}

	void OnApplicationQuit()
	{
		_clientSocket.Close();
	}

	IEnumerator CheckSocket()
	{
		for (; ; )
		{
			if (_connected)
			{
				// execute block of code here
				if ((_clientSocket.Poll(1000, SelectMode.SelectRead) && _clientSocket.Available == 0) || !_clientSocket.Connected)
				{
					_connected = false;
					Debug.LogError("Socket connection lost!");
				}
			}
			yield return new WaitForSeconds(10.0f);
		}
	}

	void SetupServer()
	{
		try
		{
			_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), 9000));
			_connected = true;
			Debug.Log("Kalliope socket connected!");
			_clientSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
		}
		catch (SocketException ex)
		{
			Debug.LogError("连接socket失败: " + ex.Message);
		}

	}

	private void ReceiveCallback(IAsyncResult AR)
	{
		//Check how much bytes are recieved and call EndRecieve to finalize handshake
		int recieved = _clientSocket.EndReceive(AR);

		if (recieved <= 0)
			return;

		//Copy the recieved data into new buffer , to avoid null bytes
		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_receiveBuffer, 0, recData, 0, recieved);

		//Process data here the way you want , all your bytes will be stored in recData
		//Debug.Log(System.Text.Encoding.UTF8.GetString(_receiveBuffer));
		//SendData(System.Text.Encoding.UTF8.GetBytes("ping"));

		// Save command and process it in Update() function
		_cmd = System.Text.Encoding.UTF8.GetString(recData).Trim();
		_onCmd = true;
		Debug.Log("收到: " + _cmd + " 长度: " + recieved);

		//Start receiving again
		_clientSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
	}

	private void SendData(byte[] data)
	{
		SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
		socketAsyncData.SetBuffer(data, 0, data.Length);
		_clientSocket.SendAsync(socketAsyncData);
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
