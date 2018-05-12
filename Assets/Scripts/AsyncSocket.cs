using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using Wit.BaiduAip.Speech;

public class AsyncSocket : MonoBehaviour {

	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _receiveBuffer = new byte[1024];

	// TODO: Make it outside 
	bool _hasTask = false;
	string _taskData = "";

    // Baidu AIP
	const string APIKey = "NgT1OZT4jTdTZizBgPvWkVnB";
    const string SecretKey = "e7766ab06a495cf0ddba6598efb376af";
	private Tts _asr;

	AudioSource _audioSource;
	bool _startPlaying;

	// Use this for initialization
	void Start () {

		_audioSource = GetComponent<AudioSource>();
		_startPlaying = false;

		_asr = new Tts(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());
		SetupServer();
	}
	
	// Update is called once per frame
	void Update () {
		if (_hasTask) 
		{
			ProcessData(_taskData);
			_hasTask = false;
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

    void ProcessData(string data)
	{
		Debug.Log(data);
		StartCoroutine(_asr.Synthesis(data.Trim(), s =>
            {
                if (s.Success)
                {
                    Debug.Log("合成成功，正在播放");
				    _audioSource.clip = s.clip;
				    _audioSource.Play();

                    _startPlaying = true;
                }
                else
                {
                    Debug.Log(s.err_msg);
                }
            })); 
	}

	void OnApplicationQuit()
    {
        _clientSocket.Close();
    }

	private void SetupServer()
    {
        try
        {
            _clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 9000));
			Debug.Log("Socket connected!");
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }

        _clientSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);      
    }

	private void ReceiveCallback(IAsyncResult AR)
    {
        //Check how much bytes are recieved and call EndRecieve to finalize handshake
        int recieved = _clientSocket.EndReceive(AR);
		Debug.Log(recieved);

        if (recieved <= 0)
            return;

        //Copy the recieved data into new buffer , to avoid null bytes
        byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_receiveBuffer, 0, recData, 0, recieved);

		//Process data here the way you want , all your bytes will be stored in recData
		//Debug.Log(System.Text.Encoding.UTF8.GetString(_receiveBuffer));
		//SendData(System.Text.Encoding.UTF8.GetBytes("ping"));

		// Store data in dataContainer and process it in Update() function
		_taskData = System.Text.Encoding.UTF8.GetString(recData).Trim();      
		_hasTask = true;

		Debug.Log("收到: " + _taskData + " 长度: " + _taskData.Length);

        //Start receiving again
		_clientSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    private void SendData(byte[] data)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data, 0, data.Length);
        _clientSocket.SendAsync(socketAsyncData);
    }

}
