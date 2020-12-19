using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class OutputsGraphChoiceCtrl : MonoBehaviour
{
    public OutputsGuiSectionCtrl _OutputsGuiSectionCtrl;
    public GraphControl _GraphControl;

    public Dropdown xAxisDropdown;
    public Dropdown yAxisDropdown;
    public Dropdown yAxisExponentDropdown;

    private int xAxisChoiseIndex = -1;
    private int yAxisChoiceIndex = -1;


    private int xAxisChoiseIndexGraph = -1;
    private int yAxisChoiceIndexGraph = -1;

    private float yAxisExponent = 1;
    private List<ExperimentOutput> myExperimentOutputs;

    public float graphingSpeed = 0.25f;

    //flags
    private bool graphBeingDrawn = false;
    private bool okToDraw = true;
    private bool graphExpanded = false;
    private int counter = 0;

    // graph capture

    private Texture2D tex;
    public RectTransform GraphPosition;
 

    #region privateFunctions
    // Use this for initialization
    void Start () {

        xAxisChoiseIndex = xAxisDropdown.value;
        
        yAxisChoiceIndex = yAxisDropdown.value;
        xAxisDropdown.onValueChanged.AddListener(delegate {DropdownXChanged();});
        yAxisDropdown.onValueChanged.AddListener(delegate {DropdownYChanged();});
        yAxisExponentDropdown.onValueChanged.AddListener(delegate {DropDownExponentChanged();});
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (counter > (int) (60*graphingSpeed))
	    {
	        okToDraw = true;
	        counter = 0;
	    }
	    else
	    {
	        counter++;
	        okToDraw = false;
	    }


	    if (graphBeingDrawn && okToDraw && yAxisChoiceIndex >= 0 && xAxisChoiseIndex >= 0)
	    {
	        okToDraw = false;
            DrawNewPlot();
//            StartCoroutine(WaitForNextPlot());
	    }
	
	}

    private void ControlGraphSize()
    {
        if (graphExpanded)
        {
            _GraphControl.GetComponent<RectTransform>().sizeDelta = new Vector2(680, 680);
        }
        else
        {
            _GraphControl.GetComponent<RectTransform>().sizeDelta = new Vector2(390, 390);
        }
    }

    private void DrawNewPlot()
    {
        float xValue = myExperimentOutputs[xAxisChoiseIndexGraph].value;
        float yValue = myExperimentOutputs[yAxisChoiceIndexGraph].value;
        yValue = yValue>=0 ? Mathf.Pow(yValue, yAxisExponent):  -Mathf.Pow(-yValue, yAxisExponent);
        yValue = Mathf.Clamp(yValue, -100000, 100000);
        _GraphControl.AddNewNode(new Vector2(xValue, yValue));
    }

    private IEnumerator WaitForNextPlot()
    {
        yield return new WaitForSeconds(graphingSpeed);
        okToDraw = true;
    }
    #endregion



    #region Public_Interface_functions 
    public void InitializeScript(List<ExperimentOutput> wholeList)
    {
        myExperimentOutputs = new List<ExperimentOutput>(wholeList);
        InitiateDropdownValues();
    }

    private void InitiateDropdownValues()
    {
        xAxisDropdown.options = new List<Dropdown.OptionData>();
        yAxisDropdown.options = new List<Dropdown.OptionData>();
        int i = 0;
        foreach (ExperimentOutput output in myExperimentOutputs)
        {
            xAxisDropdown.options.Add(new Dropdown.OptionData(output.name));
            yAxisDropdown.options.Add(new Dropdown.OptionData(output.name));
            i++;
        }
    }
    #endregion

    #region public_handles

    public void DropdownXChanged()
    {
        xAxisChoiseIndex = xAxisDropdown.value;
    }

    public void DropdownYChanged()
    {
        yAxisChoiceIndex = yAxisDropdown.value;
    }

    public void DropDownExponentChanged()
    {
        switch (yAxisExponentDropdown.value)
        {
            case (0):
                yAxisExponent = -4;
                break;
            case (1):
                yAxisExponent = -3;
                break;
            case (2):
                yAxisExponent = -2;
                break;
            case (3):
                yAxisExponent = -1;
                break;
            case (4):
                yAxisExponent = -0.5f;
                break;
            case (5):
                yAxisExponent = 0.5f;
                break;
            case (6):
                yAxisExponent = 1;
                break;
            case (7):
                yAxisExponent = 2;
                break;
            case (8):
                yAxisExponent = 3;
                break;
            case (9):
                yAxisExponent = 4;
                break;
        }
    }

    public void GenerateGraph()
    {
        _GraphControl.ClearSearies();
        xAxisChoiseIndexGraph = xAxisChoiseIndex;
        yAxisChoiceIndexGraph = yAxisChoiceIndex;

        // 390 is predefined height and width
        _GraphControl.GetComponent<RectTransform>().sizeDelta = new Vector2(390, 390);
        _GraphControl.gameObject.SetActive(true);
        _GraphControl.SetMaxNodeCount(_GraphControl.MaxSeriesData);
        _GraphControl.SetGraph_X_Title(xAxisDropdown.options[xAxisChoiseIndexGraph].text.ToString() + "(" + myExperimentOutputs[xAxisChoiseIndex].unit + ")");
        _GraphControl.SetGraph_Y_Title(yAxisDropdown.options[yAxisChoiceIndexGraph].text.ToString()+ "(" + myExperimentOutputs[yAxisChoiceIndex].unit + ")");
        
        graphBeingDrawn = true;
    }

    public void DeletGraph()
    {
        graphBeingDrawn = false;
        _GraphControl.ClearSearies();
        _GraphControl.gameObject.SetActive(false);
    }

    public void ExpandGraph()
    {
        graphExpanded = !graphExpanded;
        ControlGraphSize();
    }
    public void CaptureGraph()
    {
        StartCoroutine(SaveGraph());
    }

    #endregion
    IEnumerator SaveGraph()
    {

            int width = (int) GraphPosition.sizeDelta.x - 1;
            int height = (int) GraphPosition.sizeDelta.y - 1;
            float startX = GraphPosition.position.x + 47;
            float startY = GraphPosition.position.y + 40;
            yield return new WaitForEndOfFrame();


        try
        {

            tex = new Texture2D(width, height, TextureFormat.RGB24, false);


//        tex.ReadPixels(new Rect(startX, startY, GraphPosition.sizeDelta.x, GraphPosition.sizeDelta.y), 0, 0);
//        tex.Apply();

            Rect tempRect = new Rect(Mathf.Abs(GraphPosition.anchoredPosition.x),
                Mathf.Abs(GraphPosition.anchoredPosition.y),
                width, height);


//                tex.ReadPixels(new Rect(50,100,390 ,390), 0, 0);
            tex.ReadPixels(tempRect, 0, 0);
            tex.Apply();


            byte[] bytes = tex.EncodeToPNG();

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
//        string path = "C:/PhysicsClass";
            FileStream file = System.IO.File.Open(path + "/GraphScreenshot.png", FileMode.Create);
            BinaryWriter binary = new BinaryWriter(file);
            binary.Write(bytes);

            file.Close();
            Destroy(tex);
        }
        catch (UnityException e)
        {

            Debug.Log(e);
        }


    }
}
