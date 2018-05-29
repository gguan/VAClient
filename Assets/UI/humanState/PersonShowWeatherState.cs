using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonShowWeatherState : PersonChatState {

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
        ControlAnim.Instance().ShowWeather();
    }
}
