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





	namespace Weather
    {

        [Serializable]
        public class WeatherData
        {
            public Coord coord;
            public Weather weather;
            //public string base;//TODO 
            public Main main;
            public long visibility;
            public Wind wind;
            public Clouds clouds;
            public long dt;
            public Sys sys;
            public long id;
            public string name;
            public int cod;
        }

        [Serializable]
        public class Coord
        {
            public float lon;
            public float lat;
        }

        [Serializable]
        public class Weather
        {

            public long id;
            public string main;
            public string description;
            public string icon;
        }

        [Serializable]
        public class Main
        {
            public int temp;
            public int pressure;
            public int humidity;
            public int temp_min;
            public int temp_max;
        }


        [Serializable]
        public class Wind
        {
            public int speed;
            public int deg;
        }

        [Serializable]
        public class Clouds
        {
            public int all;
        }

        [Serializable]
        public class Sys
        {
            public int type;
            public long id;
            public float message;
            public string country;
            public long sunrise;
            public long sunset;
        }
    }
}



