using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PendulumCtrl : MonoBehaviour
{
    // pendulum physical vars
    [Space(5)]
    [Header("Pointers and params")]
    [Space(5)]
    public Dropdown MaterialDropdown;
    public InputSectionCtrl _InputSectionCtrl;
    public OutputsGuiSectionCtrl _OutputsGuiSectionCtrl;
    public ExperimentPanelCtrl _ExperimentPanelCtrl;
    public Material[] materials;
    public Transform topOfTheRope;


    public GameObject pendulumMaskGO;
    public GameObject PendulumPhysGameObject;
    private SpringJoint _springJoint;
    private Rigidbody _rigidbody;
    public float dragCoefficient = 0.01f;
    private MouseDragScript _dragBob;

    
    public Transform pendulumTrans;

    // reset pendulum to start params
    private Vector3 pendulumStartPos;
    private bool doOnce = true;

    //internal calculations
    private float maxAngle = 0f;
    private float maxAngleTemp = 0f;
    private bool isCalcMaxAngle = true;
    private bool flag_PeriodCalc = true;
    private float timeFlagFirst = 0f;
    private float timeFlagSecond = 0f;
    private List<float> timePeriodHolder;
    private float meanTimePeriod = 0f;
    private float frequency = 0f;

    // Inputs
    [Space(20)]
    [Header("GUI inputs")]
    [Space(5)]
    public float massBob;
    public float diametarBob;
    public float lengthPendulum;
    public float currentGravity;
    public float friction;
    private List<InputParameter> thisExInputs;


    // Outputs
    [Space(20)]
    [Header("GUI outputs")]
    [Space(5)]

    [SerializeField]
    private List<ExperimentOutput> myCurrentOutputs;
    // local calculations
    private float angleFromCenter;
    private float maxAngleDiference = 80f;
    private float y_oncircle = 0f;
    private float lengthofPendulumBefore = 0.0f;

    private Vector3 lastBobPos = new Vector3();
    private float speedBob = 0f;

    /// <WORK> ////////////////////////////////////////////////////
    void Start ()
	{
	    _springJoint = PendulumPhysGameObject.GetComponent<SpringJoint>();
	    _rigidbody = PendulumPhysGameObject.GetComponent<Rigidbody>();
        _dragBob = pendulumMaskGO.GetComponent<MouseDragScript>();

        pendulumStartPos = PendulumPhysGameObject.transform.position;
        timePeriodHolder = new List<float>();

        InitialiseGuiInputs();
        InitialiseGuiOutputs();
     
    }

    void FixedUpdate()
    {
        CalculateBobSpeed();
    }

	void Update ()
    {
        CalculatePeriod();
	    CalculateCurrentMaxAmplitude();

        UpdateExperimentWithInputs();
        UpdateExperimentOutputs();
        UpdateExperimentGameState();

    }

    #region Geters
    public float GetLength()
    {
        float angleCalibrated = -angleFromCenter + 180;

        y_oncircle = topOfTheRope.position.y + lengthPendulum * Mathf.Cos(angleCalibrated * Mathf.PI / 180);
        return y_oncircle;
    }
    public float GetzCord()
    {
        float angleCalibrated = -angleFromCenter + 180;
        float z_oncircle = topOfTheRope.position.z + lengthPendulum * Mathf.Sin(angleCalibrated * Mathf.PI / 180);
        return z_oncircle;
    }
    #endregion

    #region initialisators
    void InitialiseGuiInputs()
    {

        thisExInputs = new List<InputParameter>();

        // Gather input list
        InputParameter tempParam = new InputParameter();
        // 1
        tempParam.paramName = "Mass of bob";
        tempParam.paramUnit = "kg";
        tempParam.paramMinValue = 0.01f;
        tempParam.paramMaxValue = 5f;
        tempParam.paramValue = 0.5f;
        thisExInputs.Add(tempParam);

        // 2
        InputParameter tempParam2 = new InputParameter();
        tempParam2.paramName = "Diameter of bob";
        tempParam2.paramUnit = "m";
        tempParam2.paramMinValue = 0.01f;
        tempParam2.paramMaxValue = 0.5f;
        tempParam2.paramValue = 0.05f;
        thisExInputs.Add(tempParam2);
        /*
                // 3
                InputParameter tempParam3 = new InputParameter();
                tempParam3.paramName = "Length of thread";
                tempParam3.paramUnit = "m";
                tempParam3.paramMinValue = 0.01f;
                tempParam3.paramMaxValue = 3;
                tempParam3.paramValue = 0.5f;
                thisExInputs.Add(tempParam3);*/


        // 3
        InputParameter tempParam3 = new InputParameter();
        tempParam3.paramName = "Pendulum length";
        tempParam3.paramUnit = "m";
        tempParam3.paramMinValue = 0.01f;
        tempParam3.paramMaxValue = 3;
        tempParam3.paramValue = 0.5f;
        thisExInputs.Add(tempParam3);

        // 3
        InputParameter tempParam4 = new InputParameter();
        tempParam4.paramName = "Gravity";
        tempParam4.paramUnit = "m/s2";
        tempParam4.paramMinValue = 0f;
        tempParam4.paramMaxValue = 20;
        tempParam4.paramValue = 9.81f;
        thisExInputs.Add(tempParam4);

        // 3
        InputParameter tempParam5 = new InputParameter();
        tempParam5.paramName = "Friction";
        tempParam5.paramUnit = "N";
        tempParam5.paramMinValue = 0f;
        tempParam5.paramMaxValue = 10;
        tempParam5.paramValue = 5f;
        thisExInputs.Add(tempParam5);


        // update GUI
        _InputSectionCtrl.AssignInputsList(thisExInputs);

        thisExInputs = _InputSectionCtrl.thisProjectParameters;

        lengthofPendulumBefore = lengthPendulum;

    }

    void InitialiseGuiOutputs()
    {
        myCurrentOutputs = new List<ExperimentOutput>();
        //0
        ExperimentOutput exOut0 = new ExperimentOutput();
        exOut0.name = "Time";
        exOut0.unit = "s";
        exOut0.value = 0f;
        exOut0.time = 0f;
        myCurrentOutputs.Add(exOut0);
        //1 
        ExperimentOutput exOut1 = new ExperimentOutput();
        exOut1.name = "Amplitude";
        exOut1.unit = "cm";
        exOut1.value = angleFromCenter;
        exOut1.time = 0f;
        myCurrentOutputs.Add(exOut1);
        //2
        ExperimentOutput exOut2 = new ExperimentOutput();
        exOut2.name = "Length of thread";
        exOut2.unit = "cm";
        exOut2.value = 0.55f;
        exOut2.time = 0f;
        myCurrentOutputs.Add(exOut2);
        //3 
        ExperimentOutput exOut3 = new ExperimentOutput();
        exOut3.name = "Inst. displacement";
        exOut3.unit = "cm";
        exOut3.value = 0f;
        exOut3.time = 0f;
        myCurrentOutputs.Add(exOut3);
        //4
        ExperimentOutput exOut4 = new ExperimentOutput();
        exOut4.name = "Time period";
        exOut4.unit = "s";
        exOut4.value = 0f;
        exOut4.time = 0f;
        myCurrentOutputs.Add(exOut4);
        //5 
        ExperimentOutput exOut5 = new ExperimentOutput();
        exOut5.name = "Frequency";
        exOut5.unit = "Hz";
        exOut5.value = 0f;
        exOut5.time = 0f;
        myCurrentOutputs.Add(exOut5);
        
        //6
        ExperimentOutput exOut6 = new ExperimentOutput();
        exOut6.name = "Inst. velocity";
        exOut6.unit = "m/s";
        exOut6.value = 0f;
        exOut6.time = 0f;
        myCurrentOutputs.Add(exOut6);
        //6
        ExperimentOutput exOut7 = new ExperimentOutput();
        exOut7.name = "Inst. acceleration";
        exOut7.unit = "m/s2";
        exOut7.value = 0f;
        exOut7.time = 0f;
        myCurrentOutputs.Add(exOut7);
        //6
        ExperimentOutput exOut8 = new ExperimentOutput();
        exOut8.name = "Kinetic en.";
        exOut8.unit = "J";
        exOut8.value = 0f;
        exOut8.time = 0f;
        myCurrentOutputs.Add(exOut8);
        //6
        ExperimentOutput exOut9 = new ExperimentOutput();
        exOut9.name = "Potential en.";
        exOut9.unit = "J";
        exOut9.value = 0f;
        exOut9.time = 0f;
        myCurrentOutputs.Add(exOut9);
        //6
        ExperimentOutput exOut10 = new ExperimentOutput();
        exOut10.name = "Total energy";
        exOut10.unit = "J";
        exOut10.value = 0f;
        exOut10.time = 0f;
        myCurrentOutputs.Add(exOut10);

        ExperimentOutput exOut11 = new ExperimentOutput();
        exOut11.name = "Angle";
        exOut11.unit = "degree";
        exOut11.value = angleFromCenter;
        exOut11.time = 0f;
        myCurrentOutputs.Add(exOut11);

        //finaly send them to outputs
        _OutputsGuiSectionCtrl.Initialization(myCurrentOutputs);
    }

    #endregion

    #region UpdateFunctions
    void CalculateCurrentMaxAmplitude()
    {
        // update max angle
        if (angleFromCenter > 0)
        {
            isCalcMaxAngle = true;
            if (angleFromCenter > maxAngleTemp)
            {
                maxAngleTemp = angleFromCenter;
            }
        }
        else
        {
            if (isCalcMaxAngle)
            {
                isCalcMaxAngle = false;
                maxAngle = maxAngleTemp;
                maxAngleTemp = 0f;
            }
        }
    }

    void CalculateBobSpeed()
    {
        float distance = Vector3.Distance(PendulumPhysGameObject.transform.position,lastBobPos);
//        speedBob = distance/Time.fixedDeltaTime;
        speedBob = _rigidbody.velocity.magnitude;
        lastBobPos = transform.position;
        Debug.Log(speedBob);
    }

    void UpdateExperimentWithInputs()
    {

        massBob = thisExInputs[0].paramValue;
        diametarBob = thisExInputs[1].paramValue;
//        lengthPendulum = thisExInputs[2].paramValue + thisExInputs[1].paramValue/2f;
        lengthPendulum = thisExInputs[2].paramValue;
        currentGravity = thisExInputs[3].paramValue;
        friction = thisExInputs[4].paramValue;

        // friction 
        _rigidbody.drag = diametarBob * friction * 0.01f /** _rigidbody.velocity.magnitude*/;

        // gravity
        Physics.gravity = new Vector3(0, -currentGravity, 0);

        // size
        pendulumMaskGO.transform.localScale = Vector3.one * diametarBob;
        PendulumPhysGameObject.transform.localScale = Vector3.one * diametarBob;

        // weight
        _rigidbody.mass = massBob;

        // lenght
        _springJoint.maxDistance = lengthPendulum;
        
        if (lengthPendulum != lengthofPendulumBefore)
        {
           SetPendulumPosition(topOfTheRope.position, pendulumTrans.position, lengthPendulum);
        }
        lengthofPendulumBefore = lengthPendulum;
    }

    void UpdateExperimentOutputs()
    {
        angleFromCenter = CalculateAngle(new Vector3(topOfTheRope.position.x, topOfTheRope.position.y, topOfTheRope.position.z), new Vector3(topOfTheRope.position.x, pendulumTrans.position.y, pendulumTrans.position.z));

        if (angleFromCenter > 180f)
        {
            angleFromCenter -= 360f;
        }

        //        angleFromCenter = Mathf.DeltaAngle(Mathf.Atan2(0, 0) * Mathf.Rad2Deg, Mathf.Atan2(pendulumTrans.position.y, pendulumTrans.position.x) * Mathf.Rad2Deg) + 90;
        // current real outputs
        myCurrentOutputs[0].value = Time.timeSinceLevelLoad; 
        myCurrentOutputs[1].value = lengthPendulum * (maxAngle * 3.14f / 180f) * 100f; // amplitude // normalized to cm
        myCurrentOutputs[2].value = (lengthPendulum - diametarBob/2f)* 100f; // normalized to cm
        myCurrentOutputs[3].value = _rigidbody.transform.localPosition.z * 100f; // Inst. displacemen // normalized to cm
        myCurrentOutputs[4].value = 2f * 3.1415f * Mathf.Sqrt(lengthPendulum/currentGravity); // time period
        myCurrentOutputs[5].value = 1f/ myCurrentOutputs[4].value; //freq
        myCurrentOutputs[6].value = Mathf.Sqrt(2f*currentGravity*lengthPendulum*(1f - Mathf.Cos(angleFromCenter*3.14f/180f)));  // inst velocity
        myCurrentOutputs[7].value = -1f * currentGravity * angleFromCenter / lengthPendulum; //inst acceleration
//        myCurrentOutputs[8].value = 0.5f * massBob * Mathf.Pow((_rigidbody.velocity.magnitude),2); // kinetic energy
        myCurrentOutputs[8].value = 0.5f * massBob * _rigidbody.velocity.sqrMagnitude; // kinetic energy
        myCurrentOutputs[9].value = massBob*currentGravity*lengthPendulum*(1f - Mathf.Cos(angleFromCenter * 3.14f / 180f)); // potential energy
        myCurrentOutputs[10].value = myCurrentOutputs[8].value + myCurrentOutputs[9].value; // total energy
        myCurrentOutputs[11].value = angleFromCenter;

        _OutputsGuiSectionCtrl.UpdateExperimentOutputs(myCurrentOutputs);
    }

    void UpdateExperimentGameState()
    {
        if (_ExperimentPanelCtrl.CheckForExperimentRestart())
        {
            ResetPendulumToStart();
            if (_rigidbody)
            {
                _rigidbody.velocity = new Vector3(0, 0, 0);
            }
        }
        else
        {
            doOnce = true;
        }
    }

    void ResetPendulumToStart()
    {
        if (doOnce)
        {
            doOnce = false;
           PendulumPhysGameObject.transform.position = new Vector3(pendulumStartPos.x, lengthPendulum, pendulumStartPos.z);
        }
    }
    

    void CalculatePeriod()
    {
        float currentZ = _rigidbody.transform.localPosition.z;
        float currentPeriod = 0f;
        if (currentZ > 0f && flag_PeriodCalc)
        {
            flag_PeriodCalc = false;
            timeFlagFirst = Time.time;

            
        }

        if (currentZ < 0f && !flag_PeriodCalc)
        {
            flag_PeriodCalc = true;
            timeFlagSecond = Time.time;
            currentPeriod = (timeFlagSecond - timeFlagFirst)*2f;

            timePeriodHolder.Add(currentPeriod);

            if (timePeriodHolder.Count == 2)
            {
                bool sanityCheck = (timePeriodHolder[1] - timePeriodHolder[0]) >
                timePeriodHolder[1]*0.3f;
                if (sanityCheck)
                {
                    timePeriodHolder.RemoveAt(0);
                }
            }

            if (timePeriodHolder.Count == 21)
            {
                timePeriodHolder.RemoveAt(0);
            }
            meanTimePeriod = 0f;
            foreach (float temp in timePeriodHolder)
            {
                meanTimePeriod += temp;
            }
            meanTimePeriod = meanTimePeriod/timePeriodHolder.Count;
        }

    }


    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.x;
    }

    #endregion

    #region Setters

    public void DropdownValueChanged(int i)
    {
         pendulumMaskGO.GetComponent<MeshRenderer>().material = materials[MaterialDropdown.value];
    }

    public void SetPendulumPosition(Vector3 from, Vector3 to, float ropeLenght)
    {
        angleFromCenter = CalculateAngle(new Vector3(from.x, from.y, from.z), new Vector3(from.x, to.y, to.z));

        if (angleFromCenter > 180f)
        {
            angleFromCenter -= 360f;
        }

        angleFromCenter = Mathf.Clamp(angleFromCenter, -maxAngleDiference, maxAngleDiference);


        float angleCalibrated = -angleFromCenter + 180;


//        float x_oncircle = topOfTheRope.position.x + ropeLenght * Mathf.Sin(angleCalibrated * Mathf.PI / 180);
        y_oncircle = topOfTheRope.position.y + ropeLenght * Mathf.Cos(angleCalibrated * Mathf.PI / 180);
        float z_oncircle = topOfTheRope.position.z + ropeLenght * Mathf.Sin(angleCalibrated * Mathf.PI / 180);

        pendulumTrans.position = new Vector3(from.x, y_oncircle, z_oncircle);
        //                pendulumTrans.position = new Vector3(x_oncircle, y_oncircle, from.z);
    }
    #endregion
    
}
