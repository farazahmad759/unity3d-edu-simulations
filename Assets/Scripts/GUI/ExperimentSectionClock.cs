using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExperimentSectionClock : MonoBehaviour
{

    public Text ClockText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{

	    int secondsFromLoad = (int)Time.timeSinceLevelLoad;
	    int hours = secondsFromLoad/3600;
	    int minutes = (secondsFromLoad - hours*3600)/60;
	    int seconds = (secondsFromLoad - hours*3600 - minutes*60);
        ClockText.text = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");

	}
}
