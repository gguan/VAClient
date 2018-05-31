using System;
using UnityEngine;
using VirtualAssistant;

public class PersonShowWeatherState : PersonPlayingState
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
	public class Main{
		public int temp;
		public int pressure;
		public int humidity;
		public int temp_min;
		public int temp_max;
	}


	[Serializable]
	public class Wind{
		public int speed;
		public int deg;
	}

	[Serializable]
	public class Clouds{
		public int all;
	}

	[Serializable]
	public class Sys{
		public int type;
		public long id;
		public float message;
		public string country;
		public long sunrise;
		public long sunset;
	}


	public PersonShowWeatherState()
    {
        stateId = StateID.ShowWeatherStateId;
    }

	public override void Act(GameObject gameObject, GameObject npc)
    {
		base.Act(gameObject, npc);
    }

	public override void Reason(GameObject gameObject, string data)
    {
		base.Reason(gameObject, data);
    }

    public override void DoBeforeEntering()
    {
		base.DoBeforeEntering();
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
        ControlAnim.Instance().DissmissWeather();
    }

    public override void DoAnimtor()
    {
		//展示天气信息
		GameObject weatherGameObject = GameObject.Find("ui/weather_show");
		WeatherEdit weather=weatherGameObject.GetComponent<WeatherEdit>();
        
		try{
			 RequestData<WeatherData> weatherData = JsonUtility.FromJson< RequestData<WeatherData>>(actionData);
			//weather.wind_direction = weatherData.data.wind.deg;

		}catch(Exception e)
		{
			FeedBackError(e.Message,"error");
		}

        ControlAnim.Instance().ShowWeather();
    }
 
}


