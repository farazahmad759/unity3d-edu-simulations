using System;
using UnityEngine;
using UnityEngine.UI;

public class InputPanelCtrl : MonoBehaviour
{

    public InputSectionCtrl _InputSectionCtrl;

    private InputParameter parameter;

    public Text _text;
    public Slider _Slider;
    public InputField _InputField;

    private float changeAmount = 0f;

    

    public void Initialisation()
    {



        //local
        changeAmount = (parameter.paramMaxValue - parameter.paramMinValue) / 20f;
        float minVal = parameter.paramMinValue;
        float maxVal = parameter.paramMaxValue;
        float currVal = parameter.paramValue;

        // slider
        _Slider.minValue = minVal;
        _Slider.maxValue = maxVal;
        _Slider.value = currVal;

        // text box
        _text.text = parameter.paramName + " (" + parameter.paramUnit + ")";

        //input field
        _InputField.text = parameter.paramValue.ToString("F2");
    }

    void UpdateControllerValues()
    {
        _InputSectionCtrl.UpdateParameter(parameter);
    }

    public void InputFieldChanged(string s)
    {
        parameter.paramValue = Convert.ToSingle(_InputField.text);
        UpdateGuiElements();
        UpdateControllerValues();
    }

    public void SliderChanged(float f)
    {
        parameter.paramValue = (_Slider.value);
        UpdateGuiElements();
        UpdateControllerValues();
    }

    void UpdateGuiElements()
    {
        _Slider.value = parameter.paramValue;
        _InputField.text = parameter.paramValue.ToString("F2");
    }

    public void IncreaseParamValue()
    {
        parameter.paramValue += changeAmount;
        parameter.paramValue = Mathf.Clamp(parameter.paramValue, parameter.paramMinValue, parameter.paramMaxValue);
        UpdateGuiElements();
        UpdateControllerValues();
    }

    public void DecreseParamValue()
    {
        parameter.paramValue -= changeAmount;
        parameter.paramValue = Mathf.Clamp(parameter.paramValue, parameter.paramMinValue, parameter.paramMaxValue);
        UpdateGuiElements();
        UpdateControllerValues();
    }

    public void SetParamData(InputParameter newParam, InputSectionCtrl ctrl)
    {
        parameter = new InputParameter();
        parameter = newParam;
//        paramValue = newParam.paramValue;
//        paramName = newParam.paramName;
//        paramUnit = newParam.paramUnit;
//        paramMin = newParam.paramMinValue;
//        paramMax = newParam.paramMaxValue;

        _InputSectionCtrl = ctrl;

        Initialisation();
        UpdateGuiElements();
    }

}
