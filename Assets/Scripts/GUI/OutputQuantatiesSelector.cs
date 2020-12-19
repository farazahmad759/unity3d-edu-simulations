using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class OutputQuantatiesSelector : MonoBehaviour
{

    public OutputsGuiSectionCtrl _OutputsGuiSectionCtrl;
    public GameObject quantatiesObject;
    public GameObject prefabParentObject;

    public GameObject quanatatyTogglePrefab;

    private List<Toggle> allExperimentToggles;
    private List<bool> toggleActivityList;



    public void Initialization(List<ExperimentOutput> allOutputs)
    {
        allExperimentToggles = new List<Toggle>();
        toggleActivityList = new List<bool>();
        int i = 0;
        foreach (ExperimentOutput currOutput in allOutputs)
        {

            GameObject newQuantatyToggle =
            Instantiate(quanatatyTogglePrefab, new Vector3(), new Quaternion()) as GameObject;
            newQuantatyToggle.transform.SetParent(prefabParentObject.transform, false);
            newQuantatyToggle.GetComponentInChildren<Text>().text = currOutput.name;

            // populate lists
            allExperimentToggles.Add(newQuantatyToggle.GetComponent<Toggle>());
            if (i < 3)
            {
                toggleActivityList.Add(true);
                newQuantatyToggle.GetComponent<Toggle>().isOn = true;
            }
            else {
                toggleActivityList.Add(false);
                newQuantatyToggle.GetComponent<Toggle>().isOn = false;
            }

            //activate listener
            newQuantatyToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SomeToggleChanged();});

            i++;
        }
        SomeToggleChanged();
    }

    public void OnOffQuantaties()
    {
        quantatiesObject.SetActive(!quantatiesObject.activeInHierarchy);
    }

    public void SomeToggleChanged()
    {
//        Debug.Log("Toggle clicked");
        // check which toogle is on and update list
        int i = 0;
        foreach (Toggle currToggle in allExperimentToggles)
        {
            toggleActivityList[i] = currToggle.isOn;
            i++;
        }
        _OutputsGuiSectionCtrl.SetToggleList(toggleActivityList);
    }
}
