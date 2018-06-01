using System;
using UnityEngine;
using VirtualAssistant;

public class PersonShowWeatherState : PersonPlayingState
{
   
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
			Weather weatherData = JsonUtility.FromJson< RequestData<Weather>>(data).data;

			weather.time = weatherData.time;
			weather.wind_direction = weatherData.wind_direction;
			weather.temperature = weatherData.temperature;
			weather.weather = weatherData.weather;
			weather.air_quality = weatherData.air_quality;
			string bgName = weatherData.weather_bg;
			weather.weather_bg = Resources.Load("weather/bg/"+bgName) as Texture;
			string iconName = weatherData.weather_icon;
			weather.weather_icon = Resources.Load("weather/icon/"+ iconName) as Texture;
			//weather.wind_direction = weatherData.data.wind.deg;

		}catch(Exception e)
		{
			FeedBackError(e.Message,"parse weather error");
		}

        ControlAnim.Instance().ShowWeather();
    }
 
}


