﻿using System;
using UnityEngine;
using VirtualAssistant;
using VirtualAssistant.Weather;

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
			RequestData<WeatherData> weatherData = JsonUtility.FromJson< RequestData<WeatherData>>(actionData);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
			DateTime dt = startTime.AddMilliseconds(weatherData.data.dt);

			weather.time = dt.ToString("yyyy年MM月dd日 HH:mm:ss");
			//weather.wind_direction = weatherData.data.wind.deg;

		}catch(Exception e)
		{
			FeedBackError(e.Message,"error");
		}

        ControlAnim.Instance().ShowWeather();
    }
 
}


