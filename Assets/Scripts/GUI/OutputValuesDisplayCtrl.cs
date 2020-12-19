using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutputValuesDisplayCtrl : MonoBehaviour
{

    public GameObject outputValuePanelPrefab;
    public GameObject prefabParent;

    private List<GameObject> prefabList;
    
    public void InitiateList(List<ExperimentOutput> exOutputs, List<bool> selectedValues)
    {
        if (prefabList == null)
        {
            prefabList = new List<GameObject>();
        }
        else
        {
            foreach (GameObject prefab in prefabList)
            {
                GameObject.Destroy(prefab);
            }
            prefabList = new List<GameObject>();
        }

        for (int i = 0; i < selectedValues.Count; i++)
        {
            if (selectedValues[i])
            {
                // this one is selected to show him, do this now
                GameObject tempPrefab =
                    Instantiate(outputValuePanelPrefab, outputValuePanelPrefab.transform.position, outputValuePanelPrefab.transform.rotation) as GameObject;
                tempPrefab.transform.SetParent(prefabParent.transform);
                tempPrefab.transform.localScale = new Vector3(1,1,1);
                tempPrefab.GetComponent<RectTransform>().anchoredPosition3D = new Vector3();

                prefabList.Add(tempPrefab);
                
                if (tempPrefab.GetComponent<OutputValuePanelCtrl>())
                {
                    //set his values
                    tempPrefab.GetComponent<OutputValuePanelCtrl>().SetMyExperimentOutput(exOutputs[i]);
                }
            }
        }
    }
    
    public void UpdateList(List<ExperimentOutput> exOutputs)
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            OutputValuePanelCtrl tempScript = prefabList[i].GetComponent<OutputValuePanelCtrl>();
            for (int j = 0; j < exOutputs.Count; j++)
            {
                if (tempScript.GetMyExperimentOutput().name == exOutputs[j].name)
                {
                    //found a match, update his values
                    tempScript.GetMyExperimentOutput().value = exOutputs[j].value;
                }
            }
        }
    }


}
