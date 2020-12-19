using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExperimentGuideBubleCtrl : MonoBehaviour
{

    public Text myText;
    public GameObject BubbleGameObject;

    private ExperimentStateMachine currentStateMachine;
    private ExperimentStateMachine.PendulumStates pendulumState;


    public float secondsBeforeBubbleTurnOff = 30;
    private float timer = 0;


    public string stateMessage = "";
	// Use this for initialization
	void Start () {
        currentStateMachine = ExperimentStateMachine.GetExperimentStateMachine();
	    pendulumState = currentStateMachine.currentPendulumState;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (currentStateMachine.currentPendulumState != pendulumState)
	    {
	        //state changed, enable bubble, write new message
	        pendulumState = currentStateMachine.currentPendulumState;
	        EnableBubble(true);
	        SetMessage(currentStateMachine.GetStateText());
	        myText.text = stateMessage;
	        timer = 0f;
	    }

        if (timer > secondsBeforeBubbleTurnOff)
	    {
            // stop counting and disable bubble
            EnableBubble(false);
	    }
	    else
	    {
            // still counting
            timer += Time.deltaTime;
        }
	    


	}

    public void SetMessage(string msg)
    {
        stateMessage = msg;
    }

    public void EnableBubble(bool enable)
    {
        BubbleGameObject.SetActive(enable);
    } 
}
