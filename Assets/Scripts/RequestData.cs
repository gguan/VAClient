using System;

namespace VirtualAssistant
{
    [Serializable]
    public class RequestData<T>
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
        public string type;
        public string message;
        public SimpleJSON.JSONObject data;
    }

    [Serializable]
    public class ServerStateData
    {
        public static string Order_Listening = "order_listening";
        public string state;
    }

    [Serializable]
    public class STTData
    {
        public string text;
    }

    [Serializable]
    public class MusicData
    {
        public string id;
        public string name;
        public string url;
    }

}
