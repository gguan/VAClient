using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonIdleState : PersonChatState
{
	float speed = 1;
	float step;
	GameObject game;
	public PersonIdleState()
    {
        stateId = StateID.IdleStateId;
		step = speed * Time.deltaTime;
		game = GameObject.Find("SD_unitychan_humanoid");
    }

	public override void Act(GameObject gameObject, GameObject npc)
    {
		if(Mathf.Abs(game.transform.position.x - 0) >0.001 || Mathf.Abs(game.transform.position.y - 0) > 0.001){
			
            
            game.transform.localPosition = new Vector3(Mathf.Lerp(game.transform.localPosition.x, 0, step),
                                                       Mathf.Lerp(game.transform.localPosition.y, 0, step),
                                                       Mathf.Lerp(game.transform.localPosition.z, 0, step));
            game.transform.localScale = new Vector3(1, 1, 1);
            game.transform.rotation = Quaternion.Euler(0, 180, 0);
		}
    }

	public override void Reason(GameObject gameObject, string data)
    {
		
    }

	public override void HandleData()
	{
		
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
