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


	[Serializable]
	public class Constellation{
		public string title;
        public int loveLuckStar;
        public int moneyLuckStar;
        public int careerLuckStar;
        public int totalLuckStar;
        public int luckNumber ;
        public string luckColor ;
		public string starConstellation;
        public string dateConstellation ;
        public string descriptionText;
		public int type;// 星座图标  horoscopes = ["白羊", "金牛", "双子", "巨蟹", "狮子", "处女", "天秤", "天蝎", "射手", "摩羯", "水瓶", "双鱼"]
	}



	namespace Weather
    {

        [Serializable]
        public class WeatherData
        {
            public Coord coord;
            public Weather[] weather;
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



