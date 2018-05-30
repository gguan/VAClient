using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonStopState : PersonChatState {
	public PersonStopState()
    {
        stateId = StateID.StopStateId;
    }

	public override void Act(GameObject gameObject, GameObject npc)
    {
		base.Act(gameObject,npc);
    }

	public override void Reason(GameObject gameObject, string data)
    {
		base.Reason(gameObject, data);
    }

    public override void DoBeforeEntering()
    {
		FeedBackState("state", "stop");
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
    }
}
