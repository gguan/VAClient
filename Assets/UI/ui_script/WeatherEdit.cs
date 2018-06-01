using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherEdit : MonoBehaviour {

	// Use this for initialization
	public string time = "周一 5月28日农历四月十四";
	public string temperature = "17 ~ 32°C";
	public string weather = "晴";
	public string wind_direction = "东南风3级";
	public string air_quality = "空气质量 : 42优";
	public Texture weather_icon ;
	public Texture weather_bg;

	GameObject timeGameObject;
	GameObject weatherIconObject;
	GameObject temperatureGameObject;
	GameObject weatherGameObject;
	GameObject windDirectionGameObject;
	GameObject airQualityGameObject;
	void Start () {
		weather_icon = Resources.Load("weather/cloudy") as Texture;
		weather_bg = Resources.Load("weather/Rain") as Texture;

		timeGameObject = transform.Find("time").gameObject;
		weatherIconObject = transform.Find("weather_icon").gameObject;
		temperatureGameObject = transform.Find("temperature").gameObject;
		weatherGameObject = transform.Find("weather").gameObject;
		windDirectionGameObject = transform.Find("wind_direction").gameObject;
		airQualityGameObject = transform.Find("air_quality").gameObject;


	}
	
	// Update is called once per frame
	void Update () {

	
	}

	private void OnGUI()
	{
		timeGameObject.GetComponent<Text>().text = time;
        weatherIconObject.GetComponent<RawImage>().texture = weather_icon;
        temperatureGameObject.GetComponent<Text>().text = temperature;
        weatherGameObject.GetComponent<Text>().text = weather;
        windDirectionGameObject.GetComponent<Text>().text = wind_direction;
        airQualityGameObject.GetComponent<Text>().text = air_quality;
        gameObject.GetComponent<RawImage>().texture = weather_bg;
		weatherIconObject.GetComponent<RawImage>().texture = weather_icon;
	}
}
