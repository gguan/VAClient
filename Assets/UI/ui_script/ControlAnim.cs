using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  单例工具类 控制部分动画
 * 
 **/


public class ControlAnim
{


	private int showWindow = Animator.StringToHash("show_weather");
	private int dismissWindow = Animator.StringToHash("dismiss_weather");
	private int dismiss6 = Animator.StringToHash("dismiss");
	private int appear6 = Animator.StringToHash("appear");
	private int suoxiao = Animator.StringToHash("suoxiao");
	private int dismissConstellation = Animator.StringToHash("dismiss_constellation");
	private int showConstellation = Animator.StringToHash("show_constellation");
	private static volatile ControlAnim _instance;
	private static object _lock = new object();

	private Boolean isShowWeather = false;
	private Boolean isShowConstellation = false;

	private Animator weatherAnimtor;
	public static ControlAnim Instance()
	{
		if (_instance == null)
		{

			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = new ControlAnim();
				}
			}
		}
		return _instance;
	}

	private ControlAnim()
	{

	}


	internal void DismissActionWindow()
	{
		ShowTips("");

		if (isShowWeather)
		{
			DissmissWeather();
			isShowWeather = false;
		}

		if (isShowConstellation)
		{
			DismissConstellation();
			isShowConstellation = false;
		}
	}


	internal void ShowActionWindow(WSClient.RequestData<SimpleJSON.JSONObject> action)
	{
		ShowTips(action.message);
		FindAnimtor("SD_unitychan_humanoid").CrossFade("StandHand", 0.2f);
		TestLog.Log("action =" +action.type+",weather");
		if("weather".Equals(action.type)){
			isShowWeather = true;
            ShowWeather();
		}else if("constellation".Equals(action.type)){
			isShowConstellation = true;
            ShowConstellation();
		}else if("chat".Equals(action.type)){
			
		}
	}


	public void ShowTips(string info)
	{
		GameObject game = GameObject.Find("voice_input_show");
		Text t = game.GetComponent<Text>();
		t.text = info;
	}

	public void ShowWeather()
	{
		if (weatherAnimtor == null)
		{
			weatherAnimtor = FindAnimtor("weather_show");
		}


		AnimatorStateInfo stateInfo = weatherAnimtor.GetCurrentAnimatorStateInfo(0);
		TestLog.Log("shortNameHash ---" + stateInfo.shortNameHash);
         
	    weatherAnimtor.SetTrigger(showWindow);
			//weatherAnimtor.CrossFade("弹窗动画", 0.2f);
		 Dismiss6();
         
	}


	public void DissmissWeather()
	{
		if (weatherAnimtor == null)
		{
			weatherAnimtor = FindAnimtor("weather_show");
		}
		weatherAnimtor.SetTrigger(dismissWindow);
		//weatherAnimtor.CrossFade("关窗动画", 0.2f);
		Show6();
	}


	public void ShowConstellation()
	{
		Animator constellationAnimtor = FindAnimtor("constellations_luck");
		constellationAnimtor.SetTrigger(showConstellation);
		//constellationAnimtor.CrossFade("星座弹出",0.2f);
		Dismiss6();
	}


	public void DismissConstellation()
	{
		Animator constellationAnimtor = FindAnimtor("constellations_luck");
		constellationAnimtor.SetTrigger(dismissConstellation);
		//constellationAnimtor.CrossFade("星座消失",0.2f);
		Show6();
	}

	private void Dismiss6()
	{
		Animator animator = FindAnimtor("plane_of_rotation_front");
		animator.SetTrigger(dismiss6);
		//animator.CrossFade("消失",0.2f);

		Animator animatorBack = FindAnimtor("plane_of_rotation_back");
		animatorBack.SetTrigger(dismiss6);
		//animatorBack.CrossFade("消失", 0.2f);
		TestLog.Log("六面体消失");
	}


	private void Show6()
	{
		Animator animator = FindAnimtor("plane_of_rotation_front");
		animator.SetTrigger(appear6);
		animator.SetTrigger(suoxiao);
		//animator.CrossFade("出现", 0.2f);
		//animator.CrossFade("suoxiao",0.2f);

		Animator animatorBack = FindAnimtor("plane_of_rotation_back");
		animatorBack.SetTrigger(appear6);
		animatorBack.SetTrigger(suoxiao);
		//animatorBack.CrossFade("出现", 0.2f);
		//animatorBack.CrossFade("suoxiao",0.2f);

		TestLog.Log("六面体出现");
	}

	private Animator FindAnimtor(string name)
	{
		TestLog.Log("GameObject =" + name);
		return FindGameObject(name).GetComponent<Animator>();
	}



	private GameObject FindGameObject(string name)
	{
		return GameObject.Find(name);
	}


	public void ShowMicrophone(){
		FindAnimtor("microphone").SetTrigger("show_microphone");
	}

	public void DismissMicrophone(){
		FindAnimtor("microphone").SetTrigger("dismiss_microphone");
	}

	public class TestLog{

		private static Boolean isTest = false;
		public static void Log(string log){
			if(isTest){
				Debug.Log("ControlAnim.cs--"+log);
			}
		}
	}
}
