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
        public string name ;
		public string date;
        public string descriptionText;
		public int type;// 星座图标  horoscopes = ["白羊", "金牛", "双子", "巨蟹", "狮子", "处女", "天秤", "天蝎", "射手", "摩羯", "水瓶", "双鱼"]
	}


	[Serializable]
	public class Weather{
		public string time ;
        public string temperature  ;
        public string weather ;
        public string wind_direction ;
        public string air_quality  ;
		public string weather_icon;
		public string weather_bg;
	}
    
}



