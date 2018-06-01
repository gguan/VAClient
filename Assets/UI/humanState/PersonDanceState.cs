using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonDanceState : PersonPlayingState {

	public bool isPlayDance = false;
	public PersonDanceState(){
		isPlayDance = false;
		stateId = StateID.DanceStateId;
	}

	public override void Act(GameObject gameObject, GameObject npc)
	{
		base.Act(gameObject, npc);
		if (isPlayDance && !_audioSource.isPlaying)
        {
			AnimatorStateInfo animatorStateInfo =	_animator.GetCurrentAnimatorStateInfo(0);
			if ((animatorStateInfo.normalizedTime > 1.0f) && (animatorStateInfo.IsName("tiantianquan_vmd"))){  
				isPlayDance = false;
                Debug.Log("结束舞蹈");
                FSMSystem.Instance().PerformTransition(PersonState.Idle, "idle"); }  
        }
	}

	public override void Reason(GameObject gameObject, string data)
	{
		base.Reason(gameObject, data);
	}

	public override void DoAnimtor()
	{
		base.DoAnimtor();

	}

	public override void DoBeforeLeaving()
	{
		base.DoBeforeLeaving();
		ControlAnim.Instance().Show6();

		isPlayDance = false;
	

        _animator.CrossFade("stand", 0.1f);
        _animator.CrossFade("default", 0.15f);
		//GameObject.Find("SD_unitychan_humanoid").transform.position = new Vector3(0,0,0);
		//GameObject.Find("SD_unitychan_humanoid").transform.localScale = new Vector3(1,1,1);
		//GameObject.Find("SD_unitychan_humanoid").transform.rotation  =Quaternion.Euler(0, 180, 0);
	}

	public override void DoBeforeEntering()
	{
		base.DoBeforeEntering();
	}

    //说完message 话之后执行
	protected override void AfterVoiceEnd()
	{
        // 说完话之后 如果舞蹈还没执行
		if(!isPlayDance){
			AudioClip lamusic =  Resources.Load<AudioClip>("ttq");
            _audioSource.clip = lamusic;
            _audioSource.Play();
     
            ControlAnim.Instance().Dismiss6();
            Debug.Log("开始跳舞");
            _animator.CrossFade("ttq_vmd", 0.2f);
            isPlayDance = true;
		}
	}
}
