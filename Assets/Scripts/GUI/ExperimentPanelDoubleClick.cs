using UnityEngine;
using System.Collections;

public class ExperimentPanelDoubleClick : MonoBehaviour
{
    public ExperimentPanelCtrl _ExperimentPanelCtrl;
    private bool firstClick = false;
    public float timeForClick = 0.2f;
	
	// Update is called once per frame
	void OnMouseOver () {
	    if (!firstClick)
	    {
	        if (Input.GetMouseButtonUp(0))
	        {
	            firstClick = true;
	            StartCoroutine(DoubleClickCheck());
	        }
	    }
	    else
	    {
            if (Input.GetMouseButtonUp(0))
            {
                // this is second click
                _ExperimentPanelCtrl.ButtonEyeClick();
            }
        }
	
	}

    IEnumerator DoubleClickCheck()
    {
        yield return new WaitForSeconds(timeForClick);
        firstClick = false;
    }
}
