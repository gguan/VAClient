using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  HumanState : MonoBehaviour {


	private FSMSystem fSM;
	private Animator _animtator;

	public void SetState(PersonState state){
		fSM.PerformTransition(state);
	}

	// Use this for initialization
	void Start () {
		MakeFSM();

		_animtator = GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>();
	}

	private void MakeFSM()
	{
		fSM = new FSMSystem();

		PersonSleepSatte personSleep = new PersonSleepSatte();
		personSleep.AddPersonState(PersonState.Idle, StateID.IdleStateId);
		personSleep.AddPersonState(PersonState.Stop, StateID.StopStateId);
		personSleep.AddPersonState(PersonState.Play, StateID.PlayStateId);

		PersonIdleSatte personIdleSatte = new PersonIdleSatte(); 
		personIdleSatte.AddPersonState(PersonState.Sleep, StateID.SleepStateId);
		personIdleSatte.AddPersonState(PersonState.Stop, StateID.StopStateId);
		personIdleSatte.AddPersonState(PersonState.Play, StateID.PlayStateId);

		PersonStopSatte personStopSatte = new PersonStopSatte();
		personStopSatte.AddPersonState(PersonState.Sleep, StateID.SleepStateId);
		personStopSatte.AddPersonState(PersonState.Idle, StateID.IdleStateId);
		personStopSatte.AddPersonState(PersonState.Play, StateID.PlayStateId);

		PersonPlaySatte personPlaySatte = new PersonPlaySatte();
		personPlaySatte.AddPersonState(PersonState.Sleep, StateID.SleepStateId);
		personPlaySatte.AddPersonState(PersonState.Stop, StateID.StopStateId);
		personPlaySatte.AddPersonState(PersonState.Idle, StateID.IdleStateId);

		fSM.AddState(personSleep);
		fSM.AddState(personIdleSatte);
		fSM.AddState(personStopSatte);
		fSM.AddState(personPlaySatte);
   
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.A)){
			SetState(PersonState.Idle);
		}
		if (Input.GetKeyDown(KeyCode.B))
        {
			SetState(PersonState.Play);
        }
		if (Input.GetKeyDown(KeyCode.C))
        {
			SetState(PersonState.Sleep);
        }
		if (Input.GetKeyDown(KeyCode.D))
        {
			SetState(PersonState.Stop);
        }
	}



}

internal class PersonSleepSatte : FSMState
{
	public PersonSleepSatte()
    {
		stateId = StateID.SleepStateId;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        //Debug.Log("log_stateId_Act" + stateId.ToString());
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        Debug.Log("log_stateId_Reason" + stateId.ToString());
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
		Debug.Log("--------- sleep");

    }
}
internal class PersonPlaySatte : FSMState
{
	public PersonPlaySatte(){
		stateId = StateID.PlayStateId;
	}

	public override void Act(GameObject player, GameObject npc)
	{
		//Debug.Log("log_stateId_Act" + stateId.ToString());
	}

	public override void Reason(GameObject player, GameObject npc)
	{
		Debug.Log("log_stateId_Reason" + stateId.ToString());
	}

	public override void DoBeforeEntering()
	{
		base.DoBeforeEntering();
		Debug.Log("--------- play");
		GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>().CrossFade("sad", 0.2f);
	}

	public override void DoBeforeLeaving()
	{
		base.DoBeforeLeaving();
		GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>().CrossFade("default@sd_hmd", 0.2f);
	}

}

internal class PersonIdleSatte : FSMState
{
	public PersonIdleSatte()
    {
		stateId = StateID.IdleStateId;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        //Debug.Log("log_stateId_Act" + stateId.ToString());
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        Debug.Log("log_stateId_Reason" + stateId.ToString());
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
		Debug.Log("---------idle");
		Debug.Log("---------进入IDLE状态时执行 微笑");
		GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>().CrossFade("smile", 0.2f);
    }

	public override void DoBeforeLeaving()
	{
		base.DoBeforeLeaving();
		Debug.Log("---------离开IDLE状态时执行 生气");
		GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>().CrossFade("angry", 0.2f);
	}
}

internal class PersonStopSatte : FSMState
{
	public PersonStopSatte()
    {
		stateId = StateID.StopStateId;
    }

    public override void Act(GameObject player, GameObject npc)
    {
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        Debug.Log("log_stateId_Reason" + stateId.ToString());
    }

    public override void DoBeforeEntering()
    {
        base.DoBeforeEntering();
		Debug.Log("---------进入停止状态时执行 微笑");
		GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>().CrossFade("smile", 0.2f);
    }

	public override void DoBeforeLeaving()
	{
		base.DoBeforeLeaving();
		Debug.Log("---------离开停止状态时执行 生气");
		GameObject.Find("SD_unitychan_humanoid").GetComponent<Animator>().CrossFade("angry", 0.2f);
	}
}
