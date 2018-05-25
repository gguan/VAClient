﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddlePositionScript : MonoBehaviour {

	Text mText;
	// Use this for initialization
	void Start () {
		mText = gameObject.GetComponent<Text>();


		transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width,Screen.height/2);
		float x = transform.parent.position.x;
        float y = transform.parent.position.y + (Screen.height / 2 - 150);
		transform.SetPositionAndRotation(new Vector3(x, y, 0), new Quaternion());
        

	}
  

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			ControlAnim.Instance().ShowTips("dsjhkfjskjk");
		}
	}
}