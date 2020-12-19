using UnityEngine;
using System.Collections;

public class ExperimentStateMachine : MonoBehaviour {

    public enum PendulumStates
    {
        Initial = 0,
        MeasureBob,
        ChangeThreadLength,
        MeasureThreadLength,
        CalculatePendulumLength,
        TieBobToPendulum,
        MarkDistancesWithChalk,
        MoveBobTo5cm,
        WaitForStableOscilation,
        Measure20Oscilations,
        CalculateAndRepeatFor10And20cm
    }

    public PendulumStates currentPendulumState = PendulumStates.Initial;
    public static ExperimentStateMachine thisObject;

    public static ExperimentStateMachine GetExperimentStateMachine()
    {
        return thisObject;
    }


	// Use this for initialization
	void Awake () {
        currentPendulumState = PendulumStates.Initial;
	    thisObject = this;
	}
	

    public void SwitchToState(PendulumStates state)
    {
        currentPendulumState = state;
    }

    public void GoToNextState()
    {
        int stateNum = (int)currentPendulumState + 1;
        stateNum = Mathf.Clamp(stateNum, 0, 10);
        currentPendulumState = (PendulumStates)(stateNum);
    }


    public void GoToPreviousState()
    {
        int stateNum = (int)currentPendulumState - 1;
        stateNum = Mathf.Clamp(stateNum, 0, 10);
        currentPendulumState = (PendulumStates)(stateNum);
    }

    public string GetStateText()
    {
        string text = "";
        switch (currentPendulumState)
        {
            case PendulumStates.Initial:
                text = "Start the experiment by familiarizing yourself with environment.";
                break;
            case PendulumStates.MeasureBob:
                text = "Double click the bob to measure it's length.";
                break;
            case PendulumStates.ChangeThreadLength:
                text = "Choose lenght of the thread from menu below.";
                break;
            case PendulumStates.MeasureThreadLength:
                text = "Double click the ruler to measure thread length.";
                break;
            case PendulumStates.CalculatePendulumLength:
                text = "Add thread lenght to the half of bob diameter to calculate pendulum length.";
                break;
            case PendulumStates.TieBobToPendulum:
                text = "Double click the bob to tie it to thread.";
                break;
            case PendulumStates.MarkDistancesWithChalk:
                text = "Double click the chalk to mark distances of 5cm, 10cm and 20cm.";
                break;
            case PendulumStates.MoveBobTo5cm:
                text = "Click and hold bob to move it to 5cm position.";
                break;
            case PendulumStates.WaitForStableOscilation:
                text = "Wait until oscilations are stable.";
                break;
            case PendulumStates.Measure20Oscilations:
                text = "Measure the time of 20 oscilations.";
                break;
            case PendulumStates.CalculateAndRepeatFor10And20cm:
                text = "Calculate g using time period and pendulum length and then repeat for 10cm and 20cm offset.";
                break;
        }

        return text;
    }

}
