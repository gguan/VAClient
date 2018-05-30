using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSleepState : PersonChatState {

	public PersonSleepState()
    {
        stateId = StateID.SleepStateId;
    }

    public override void DoBeforeEntering()
    {
		FeedBackState("state", "sleep");
    }

	protected override void AfterVoiceEnd()
	{
		
	}

}
