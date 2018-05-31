using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonIdleState : PersonChatState
{

	public PersonIdleState()
    {
        stateId = StateID.IdleStateId;
    }

	public override void Act(GameObject gameObject, GameObject npc)
    {
		
    }

	public override void Reason(GameObject gameObject, string data)
    {
		
    }

	public override void HandleData()
	{
		base.HandleData();
	}

	public override void DoBeforeEntering()
    {
		FeedBackState("state", "idle");
    }

    public override void DoBeforeLeaving()
    {
        base.DoBeforeLeaving();
    }
}
