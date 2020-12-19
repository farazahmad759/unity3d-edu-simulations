using UnityEngine;
using System.Collections;

public class DetectClickDown : MonoBehaviour
{

    public ExperimentPanelCtrl MyExperimentPanelCtrl;

	

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MyExperimentPanelCtrl.ButtonFullScreenClick();
        }
    }
}
