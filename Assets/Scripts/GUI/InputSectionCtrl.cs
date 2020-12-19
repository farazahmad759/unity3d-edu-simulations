using UnityEngine;
using System.Collections.Generic;

public class InputParameter
{
    public string paramName = "Bla";
    public string paramUnit = "unit";
    public float paramValue = 0.5f;
    public float paramMinValue = 0f;
    public float paramMaxValue = 1f;
}

public class InputSectionCtrl : MonoBehaviour
{

    public GameObject InputFieldPrefab;
    public Transform PrefabsParentGameObject;

    public List<InputParameter> thisProjectParameters;

    [Space(20)]
    [Header("TESTER")]
    public bool testMe = false;

    
    

    

    void AddParameter(InputParameter newParam)
    {
        if (thisProjectParameters == null)
        {
            thisProjectParameters = new List<InputParameter>();
        }
        thisProjectParameters.Add(newParam);

        GameObject newInputField =
            Instantiate(InputFieldPrefab, new Vector3(), new Quaternion()) as GameObject;
        newInputField.transform.SetParent(PrefabsParentGameObject, false);

        newInputField.GetComponent<InputPanelCtrl>().SetParamData(newParam, this);
    }
    
    public float GetParamValue(string soughtParamName)
    {
        float returnValue = -1f;
        foreach (InputParameter thisParam in thisProjectParameters)
        {
            if (thisParam.paramName == soughtParamName)
            {
                returnValue = thisParam.paramValue;
            }
        }
        return returnValue;
    }

    public void UpdateParameter(InputParameter newParam)
    {
        foreach (InputParameter currParam in thisProjectParameters)
        {
            if (currParam.paramName == newParam.paramName)
            {
                currParam.paramValue = newParam.paramValue;
            }
        }
    }

    public void AssignInputsList(List<InputParameter> list)
    {
        foreach (InputParameter tempParam in list)
        {
            AddParameter(tempParam);
        }
    }
    


    #region TESTERS
    void Test()
    {
        InputParameter newParameter = new InputParameter();

        //obavezno setovati parametar pre upotrebe

        for (int i = 0; i < 3; i++)
        {
            AddParameter(newParameter);

        }
    }

    #endregion
}
