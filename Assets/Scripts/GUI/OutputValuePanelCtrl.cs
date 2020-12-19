using UnityEngine;
using UnityEngine.UI;

public class OutputValuePanelCtrl : MonoBehaviour
{

    public Text nameText;
    public InputField valueInputField;
    public Text unitText;

    private ExperimentOutput myExperimentOutput;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

	    if (nameText && valueInputField && unitText)
	    {
	        nameText.text = myExperimentOutput.name;
	        valueInputField.text = myExperimentOutput.value.ToString("F2");
	        unitText.text = myExperimentOutput.unit;
	    }

	
	}

    public void SetMyExperimentOutput(ExperimentOutput exOut)
    {
        if (myExperimentOutput == null)
        {
            myExperimentOutput = new ExperimentOutput();
        }
        myExperimentOutput = exOut;
    }

    public ExperimentOutput GetMyExperimentOutput()
    {
        return myExperimentOutput;
    }
}
