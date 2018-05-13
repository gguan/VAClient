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

    // TODO
	bool _onCmd = false;
	string _cmd = "";

    // Baidu AIP
    const string APIKey = "NgT1OZT4jTdTZizBgPvWkVnB";
    const string SecretKey = "e7766ab06a495cf0ddba6598efb376af";
    private Tts _asr;

	public AudioSource _audioSource;
    bool _startPlaying;

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
            _clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 9000));
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
		if (_connected) {
			return;
		}    

		if (GUI.Button(new Rect(Screen.width / 2, Screen.height - 60, 200, 40), "Connect Again"))
        {
            SetupServer();
        }
    }

}
