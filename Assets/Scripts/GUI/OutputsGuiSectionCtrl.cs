using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ExperimentOutput
{
    public string name = "Output1";
    public string unit = "smt";
    public float value = 0f;
    public float time = 0f;

    public ExperimentOutput()
    {
        value = 0f;
        time = 0f;
    }
}

public class OutputsGuiSectionCtrl : MonoBehaviour
{
    //pointers
    public OutputQuantatiesSelector _OutputQuantatiesSelector;
    public OutputValuesDisplayCtrl _OutputValuesDisplayCtrl;
    public OutputsGraphChoiceCtrl _OutputsGraphChoiceCtrl;

    private List<ExperimentOutput> currentExperimentOutput;

    // tracked values for graph
    private List<Vector2> firstTrackedValue;
    private int firstTrackedValueIndex = -1;
    private List<Vector2> secondTrackedValue;
    private int secondTrackedValueIndex = -1;

    private int numberOfOutputs = 0;

    // members for output display section
    private List<bool> toggleActivityList;
    private bool listChanged = false;


	
	void Update () {
        // update is on while game is on, so we take value updates here
        LoggTrackedValues();
    }


    // UNDER THE HOOD
    private void LoggTrackedValues()
    {
        if (firstTrackedValueIndex != -1)
        {
            firstTrackedValue.Add(new Vector2(currentExperimentOutput[firstTrackedValueIndex].value, currentExperimentOutput[firstTrackedValueIndex].time));
        }
        if (secondTrackedValueIndex != -1)
        {
            secondTrackedValue.Add(new Vector2(currentExperimentOutput[secondTrackedValueIndex].value, currentExperimentOutput[secondTrackedValueIndex].time));
        }
    }

    #region PublicFunctions
    // interface from experiment towards this script
    public void Initialization(List<ExperimentOutput> startingOutputs)
    {
        currentExperimentOutput = new List<ExperimentOutput>(startingOutputs);
//        numberOfOutputs = startingOutputs.Count;
//        foreach (ExperimentOutput currOutput in startingOutputs)
//        {
//            currentExperimentOutput.Add(currOutput);
//        }

        // initialize others
        if(_OutputQuantatiesSelector)
            _OutputQuantatiesSelector.Initialization(currentExperimentOutput);

        if(_OutputsGraphChoiceCtrl)
            _OutputsGraphChoiceCtrl.InitializeScript(currentExperimentOutput);

    }
    public void UpdateExperimentOutputs(List<ExperimentOutput> newOutputs)
    {
        for (int i = 0; i < newOutputs.Count; i++)
        {
            // update outputs
            currentExperimentOutput[i].value = newOutputs[i].value;
            currentExperimentOutput[i].time = newOutputs[i].time;
        }

        //notify displaying script that we have new values, if there is a list
        if(listChanged)
            _OutputValuesDisplayCtrl.UpdateList(currentExperimentOutput);
    }

    // interface for graphs 
    public void SetTrackedValue(int first, int second)
    {
        firstTrackedValueIndex = first;
        secondTrackedValueIndex = second;
    }

    //interface for outputs display
    public void SetToggleList(List<bool> list)
    {
        toggleActivityList = new List<bool>(list);
        listChanged = true;
        // list changed, initiate change in display as well
        _OutputValuesDisplayCtrl.InitiateList(currentExperimentOutput, toggleActivityList);
    }


    public ExperimentOutput GetOutput(int outputIndex)
    {
        return currentExperimentOutput[outputIndex];
    }
    public List<ExperimentOutput> GetOutputsList()
    {
        return currentExperimentOutput;
    }
    #endregion
}
