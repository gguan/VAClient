using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonPlayingState : PersonChatState {


	public override void DoBeforeEntering()
	{
		base.DoBeforeEntering();
		FeedBackState("state", "playing");
	}
}
