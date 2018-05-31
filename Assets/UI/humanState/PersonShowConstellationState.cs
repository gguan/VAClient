using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAssistant;

public class PersonShowConstellationState : PersonPlayingState {

 


	public PersonShowConstellationState()
    {
        stateId = StateID.ShowConstellationStateId;
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
        ControlAnim.Instance().DismissConstellation();
    }

    public override void DoAnimtor()
    {
        base.DoAnimtor();

		//展示天气信息
        GameObject constellationGameObject = GameObject.Find("ui/constellation_luck");
		if(constellationGameObject!=null){
			ConstellationEdit constellationEdit = constellationGameObject.GetComponent<ConstellationEdit>();

            try
            {
				 
				Constellation constellation = JsonUtility.FromJson<RequestData<Constellation>>(data).data;
				constellationEdit.loveLuckStar = constellation.loveLuckStar;
				constellationEdit.careerLuckStar = constellation.careerLuckStar;
				constellationEdit.moneyLuckStar = constellation.loveLuckStar;
				constellationEdit.totalLuckStar = constellation.totalLuckStar;
				constellationEdit.title = constellation.title;
				constellationEdit.type = constellation.type;

            }
            catch (Exception e)
            {
                FeedBackError(e.Message, "error");
            }

            ControlAnim.Instance().ShowConstellation();
		}
    }
}
