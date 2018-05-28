using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonState
{
	None,
	Sleep,
	Idle,
	Music,
	Stop,
	ShowWeather,
	Awake,
    ShowConstellation
}

public enum StateID
{
	NoneStateId=0,
	SleepStateId = 1,
	IdleStateId = 2,
	MusicStateId = 3,
	StopStateId = 4,
    ShowWeatherStateId =5,
    ShowConstellationStateId=6,
    AwakeStateId,
}

public abstract class FSMState{
	protected Dictionary<PersonState, StateID> map = new Dictionary<PersonState, StateID>();
	protected StateID stateId;
    
	public StateID ID { get { return stateId; } }
    
	public void AddPersonState(PersonState personState, StateID id)
    {
        // Check if anyone of the args is invalid
		if (personState == PersonState.None)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

		if (id == StateID.NoneStateId)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
		if (map.ContainsKey(personState))
        {
			Debug.LogError("FSMState ERROR: State " + id.ToString() + " already has transition " + personState.ToString() +
                           "Impossible to assign to another state");
            return;
        }

		map.Add(personState, id);
    }

    /// <summary>
    /// This method deletes a pair transition-state from this state's map.
    /// If the transition was not inside the state's map, an ERROR message is printed.
    /// </summary>
	public void DeleteTransition(PersonState personState)
    {
        // Check for NullTransition
		if (personState == PersonState.None)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
		if (map.ContainsKey(personState))
        {
			map.Remove(personState);
            return;
        }
		Debug.LogError("FSMState ERROR: Transition " + personState.ToString() + " passed to " + stateId.ToString() +
                       " was not on the state's transition list");
    }

    /// <summary>
    /// This method returns the new state the FSM should be if
    ///    this state receives a transition and 
    /// </summary>
	public StateID GetOutputState(PersonState personState)
    {
        // Check if the map has this transition
		if (map.ContainsKey(personState))
        {
			return map[personState];
        }
		return StateID.NoneStateId;
    }

    /// <summary>
    /// This method is used to set up the State condition before entering it.
    /// It is called automatically by the FSMSystem class before assigning it
    /// to the current state.
    /// </summary>
    public virtual void DoBeforeEntering() { }

    /// <summary>
    /// This method is used to make anything necessary, as reseting variables
    /// before the FSMSystem changes to another one. It is called automatically
    /// by the FSMSystem before changing to a new state.
    /// </summary>
    public virtual void DoBeforeLeaving() { }

    /// <summary>
    /// This method decides if the state should transition to another on its list
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
	public abstract void Reason(GameObject wsClient, string data);

    /// <summary>
    /// This method controls the behavior of the NPC in the game World.
    /// Every action, movement or communication the NPC does should be placed here
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
	public abstract void Act(GameObject wsClient, GameObject npc);
}


/// <summary>
/// FSMSystem class represents the Finite State Machine class.
///  It has a List with the States the NPC has and methods to add,
///  delete a state, and to change the current state the Machine is on.
/// </summary>
public class FSMSystem
{
	private List<FSMState> fSMStates;

    // The only way one can change the state of the FSM is by performing a transition
    // Don't change the CurrentState directly
    private StateID currentStateID;
	private FSMState currentState;

    public StateID CurrentStateID { get { return currentStateID; } }
    public FSMState CurrentState { get { return currentState; } }


	private static FSMSystem _instance;
	private static object _lock = new object();
	public static FSMSystem Instance()
    {
		if (_instance == null)
        {

            lock (_lock)
            {
				if (_instance == null)
                {
					_instance = new FSMSystem();

					PersonSleepState personSleep = new PersonSleepState();
					personSleep.AddPersonState(PersonState.Awake, StateID.AwakeStateId);

                    PersonIdleState personIdleState = new PersonIdleState();
					personIdleState.AddPersonState(PersonState.Sleep, StateID.SleepStateId);
					personIdleState.AddPersonState(PersonState.Music, StateID.MusicStateId);
					personIdleState.AddPersonState(PersonState.ShowWeather, StateID.ShowWeatherStateId);
					personIdleState.AddPersonState(PersonState.ShowConstellation, StateID.ShowConstellationStateId);
					personIdleState.AddPersonState(PersonState.Stop, StateID.StopStateId);


                    PersonStopState personStopState = new PersonStopState();
					personStopState.AddPersonState(PersonState.Idle, StateID.IdleStateId);


                    
                    PersonMusicState personPlaySate = new PersonMusicState();
					personPlaySate.AddPersonState(PersonState.Stop, StateID.StopStateId);
					personPlaySate.AddPersonState(PersonState.Idle, StateID.IdleStateId);


                    PersonShowWeatherState showWeatherState = new PersonShowWeatherState();
                    showWeatherState.AddPersonState(PersonState.Idle, StateID.IdleStateId);

                    PersonShowConstellationState showConstellationState = new PersonShowConstellationState();
                    showConstellationState.AddPersonState(PersonState.Idle, StateID.IdleStateId);

					PersonAwakeState awakeState = new PersonAwakeState();
					awakeState.AddPersonState(PersonState.Idle, StateID.IdleStateId);

					_instance.AddState(personSleep);
					_instance.AddState(awakeState);
					_instance.AddState(personIdleState);
					_instance.AddState(personStopState);
					_instance.AddState(personPlaySate);
					_instance.AddState(showWeatherState);
					_instance.AddState(showConstellationState);


}
            }
        }
        return _instance;
    }



	private FSMSystem()
    {
		fSMStates = new List<FSMState>();
    }

    /// <summary>
    /// This method places new states inside the FSM,
    /// or prints an ERROR message if the state was already inside the List.
    /// First state added is also the initial state.
    /// </summary>
    public void AddState(FSMState s)
    {
        // Check for Null reference before deleting
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the Initial state,
        //   the state the machine is in when the simulation begins
		if (fSMStates.Count == 0)
        {
			fSMStates.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        // Add the state to the List if it's not inside it
		foreach (FSMState state in fSMStates)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
		fSMStates.Add(s);
    }

    /// <summary>
    /// This method delete a state from the FSM List if it exists, 
    ///   or prints an ERROR message if the state was not on the List.
    /// </summary>
    public void DeleteState(StateID id)
    {
        // Check for NullState before deleting
		if (id == StateID.NoneStateId)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        // Search the List and delete the state if it's inside it
		foreach (FSMState state in fSMStates)
        {
            if (state.ID == id)
            {
				fSMStates.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    /// <summary>
    /// This method tries to change the state the FSM is in based on
    /// the current state and the transition passed. If current state
    ///  doesn't have a target state for the transition passed, 
    /// an ERROR message is printed.
    /// </summary>
	public void PerformTransition(PersonState personState)
    {
        // Check for NullTransition before changing the current state
		if (personState == PersonState.None)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        // Check if the currentState has the transition passed as argument
		StateID id = currentState.GetOutputState(personState);
		if (id == StateID.NoneStateId)

        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
			               " for transition " + personState.ToString());
            return;
        }

        // Update the currentStateID and currentState       
        currentStateID = id;
		foreach (FSMState state in  fSMStates)
        {
            if (state.ID == currentStateID)
            {
                // Do the post processing of the state before setting the new one
                currentState.DoBeforeLeaving();

                currentState = state;

                // Reset the state to its desired condition before it can reason or act
                currentState.DoBeforeEntering();
                break;
            }
        }

    } // PerformTransition()

} //class FSMSystem

