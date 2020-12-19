using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;

public class GraphControl : MonoBehaviour
{
    private bool cantCallBeforeFinish = false;

    // axis titles
    private string X_AxisTitle = "X_Axis";
    private string Y_AxisTitle = "Y_Axis";


    public bool graphGrid
    {
        get { return _graphGrid; }
        set
        {
            if (_graphGrid != value)
            {
                _graphGrid = value;
                CheckGraphGrid();

            }
        }
    }

    private bool courActivated = false;

    [SerializeField]
    private bool _graphGrid;

    #region main/shared/thread vars
    private float _minXaxisThread = 0.0f;
    private float _maxXaxisThread = 0.0f;
    private float _minYaxisThread = 0.0f;
    private float _maxYaxisThread = 0.0f;

    private float _minXaxis = 0.0f;
    private float _maxXaxis = 0.0f;
    private float _minYaxis = 0.0f;
    private float _maxYaxis = 0.0f;

    private float _minXaxisShared = 0.0f;
    private float _maxXaxisShared = 0.0f;
    private float _minYaxisShared = 0.0f;
    private float _maxYaxisShared = 0.0f;


    private WMG_List<Vector2> sharedListSeries1 = new WMG_List<Vector2>();
    private WMG_List<Vector2> threadListSeries1 = new WMG_List<Vector2>();

    private object thisLock = new object();
    #endregion


    public float plotIntervalSeconds;

    public WMG_Axis_Graph graph;

    WMG_Series series1;
    WMG_Series series2;


    System.Globalization.NumberFormatInfo tooltipNumberFormatInfo = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
    System.Globalization.NumberFormatInfo yAxisNumberFormatInfo = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
    System.Globalization.NumberFormatInfo seriesDataLabelsNumberFormatInfo = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
    System.Globalization.NumberFormatInfo indicatorLabelNumberFormatInfo = new System.Globalization.CultureInfo("en-US", false).NumberFormat;


    // max number of data length
    public int MaxSeriesData = 500;


    // Use this for initialization
    void Awake()
    {

        Initialise();



    }

    void Update()
    {


        CheckGraphGrid();

        CheckMaxListCount();


    }
    public void SetSeriesData(ref WMG_Series serie, Color pointColor, Color lineColor, float lineScale, float pointWidHeight)
    {
        serie = graph.addSeries();
        serie.pointColor = pointColor;
        serie.lineColor = lineColor;
        serie.lineScale = 0.5f;
        serie.pointWidthHeight = 8;
    }
    public void SetGraph_Y_Title(string Y_title)
    {
        graph.yAxis.AxisTitleString = Y_title;
    }
    public void SetGraph_X_Title(string X_title)
    {
        graph.xAxis.AxisTitleString = X_title;
    }


    public IEnumerator calculateGraphMinMax()
    {
        threadListSeries1.Clear();
        // calculate min and max values for both axis
        lock (thisLock)
        {
            threadListSeries1.SetList(sharedListSeries1);
        }

        _minXaxisThread = Mathf.Infinity;
        _maxXaxisThread = Mathf.NegativeInfinity;
        _minYaxisThread = Mathf.Infinity;
        _maxYaxisThread = Mathf.NegativeInfinity;

        /*
        _minXaxisThread = 100f;
        _maxXaxisThread = -100f;
        _minYaxisThread = 100;
        _maxYaxisThread = -100;

        */
        for (int i = 1; i < threadListSeries1.Count; i++)
        {
            if (threadListSeries1[i - 1].x > _maxXaxisThread)
            {
                _maxXaxisThread = threadListSeries1[i - 1].x;
            }
            if (threadListSeries1[i - 1].x < _minXaxisThread)
            {
                _minXaxisThread = threadListSeries1[i - 1].x;
            }


            if (threadListSeries1[i - 1].y > _maxYaxisThread)
            {
                _maxYaxisThread = threadListSeries1[i - 1].y;
            }
            if (threadListSeries1[i - 1].y < _minYaxisThread)
            {
                _minYaxisThread = threadListSeries1[i - 1].y;
            }

        }

        yield return new WaitForSeconds(0);

        lock (thisLock)
        {

            _minXaxisShared = _minXaxisThread;
            _maxXaxisShared = _maxXaxisThread;
            _minYaxisShared = _minYaxisThread;
            _maxYaxisShared = _maxYaxisThread;
        }
        cantCallBeforeFinish = false;
    }

    public void CheckGraphGrid()
    {

        if (!graphGrid)
        {
            graph.xAxis.hideGrid = true;
            graph.yAxis.hideGrid = true;
        }
        else
        {
            graph.xAxis.hideGrid = false;
            graph.yAxis.hideGrid = false;
        }
    }




    public void Initialise()
    {


        graph.legend.hideLegend = true;


        graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks;

        graph.autoAnimationsEnabled = false;
        graph.xAxis.hideLabels = false;
        graph.xAxis.hideTicks = true;
        graph.xAxis.hideGrid = false;
        graph.yAxis.AxisNumTicks = 5;

        graph.yAxis.hideTicks = true;
        graph.axisWidth = 1;
        //     graph.yAxis.MaxAutoGrow = true; // auto increase yAxis max if a point value exceeds max
        //     graph.yAxis.MinAutoGrow = true; // auto decrease yAxis min if a point value exceeds min

        CheckGraphGrid();

        SetGraph_X_Title(X_AxisTitle);
        SetGraph_Y_Title(Y_AxisTitle);


        SetSeriesData(ref series1, Color.red, Color.green, 0.5f, 8f);
        SetSeriesData(ref series2, Color.blue, Color.cyan, 0.5f, 8f);


        graph.tooltipDisplaySeriesName = true;



        graph.yAxis.axisLabelLabeler = customYAxisLabelLabeler;
        graph.xAxis.axisLabelLabeler = customXAxisLabelLabeler;

    }
    string customYAxisLabelLabeler(WMG_Axis axis, int labelIndex)
    {
        float num = axis.AxisMinValue + labelIndex * (axis.AxisMaxValue - axis.AxisMinValue) / (axis.axisLabels.Count - 1);
        yAxisNumberFormatInfo.CurrencyDecimalDigits = axis.numDecimalsAxisLabels;
        return num.ToString("F1", yAxisNumberFormatInfo);
    }
    string customXAxisLabelLabeler(WMG_Axis axis, int labelIndex)
    {
        float num = axis.AxisMinValue + labelIndex * (axis.AxisMaxValue - axis.AxisMinValue) / (axis.axisLabels.Count - 1);
        yAxisNumberFormatInfo.CurrencyDecimalDigits = axis.numDecimalsAxisLabels;

        return num.ToString("F1", yAxisNumberFormatInfo);
    }


    public void CheckMaxListCount()
    {
        if (series1.pointValues.Count >= MaxSeriesData)
        {
            int deleteAbove = series1.pointValues.Count - MaxSeriesData;
            series1.pointValues.RemoveRange(0, deleteAbove);

            //  series1.pointValues.RemoveAt(0);


        }
        if (series2.pointValues.Count >= MaxSeriesData)
        {
            int deleteAbove = series2.pointValues.Count - MaxSeriesData;
            series2.pointValues.RemoveRange(0, deleteAbove);

         //   series2.pointValues.RemoveAt(0);
        }
    }


    #region InterfaceFunctions
    public void ClearSearies()
    {
        // to clear all searies
        CleaSeries1();
        CleaSeries2();
    }
    public void CleaSeries1()
    {
        if (series1)
            series1.pointValues.Clear();
    }
    public void CleaSeries2()
    {
        if (series2)
            series2.pointValues.Clear();
    }

    public void AddNewNode(Vector2 node)
    {

        updateGraphData(node);
        if (series1.pointValues.Count > 1)
        {
            cantCallBeforeFinish = true;
            StartCoroutine(calculateGraphMinMax());
        }


    }

    public void SetMaxNodeCount(int maxNode)
    {
        MaxSeriesData = maxNode;
    }
    #endregion
    public void updateGraphData(Vector2 node)
    {
        _minXaxis = 0;
        _maxXaxis = 0;
        _minYaxis = 0;
        _maxYaxis = 0;

        series1.pointValues.Add(node);
        lock (thisLock)
        {

            _minXaxis = _minXaxisShared;
            _maxXaxis = _maxXaxisShared;
            _minYaxis = _minYaxisShared;
            _maxYaxis = _maxYaxisShared;

            sharedListSeries1.Clear();
            sharedListSeries1.SetList(series1.pointValues);

        }

        if (series1.pointValues.Count > 2)
        {
 
            // add a litle more value to max value, so border should be bigger then max value
            float addLilteMoreToMax = Mathf.Abs(_maxXaxis) > 5f ? 0.5f : 0.05f * Mathf.Abs(_maxXaxis);
            float maxXaxis = _maxXaxis + addLilteMoreToMax;

            maxXaxis = Mathf.Clamp(maxXaxis, 1, maxXaxis);
            float minXaxis = _minXaxis;
            #region check simetric
            if (maxXaxis != minXaxis)
            {
               // check should x values be simetric, and get max value ( max) and range should be -max...max
                if (minXaxis < 0)
                {
                    // gets the max abs value, for simetric borders
                    float maxValueAbs = Mathf.Abs(maxXaxis) > Mathf.Abs(minXaxis) ? Mathf.Abs(maxXaxis) : Mathf.Abs(minXaxis);
                    minXaxis = -maxValueAbs;
                    maxXaxis = maxValueAbs;
                }


            }

            // y axis
            int maxYaxis = Mathf.CeilToInt(_maxYaxis);
            maxYaxis = Mathf.Clamp(maxYaxis, 1, maxYaxis);

            int minYaxis = Mathf.FloorToInt(_minYaxis);
            if (maxYaxis > minYaxis)
            {

                if (_minYaxis < 0)
                {
                    int maxYvalue = Mathf.Abs(maxYaxis) > Mathf.Abs(minYaxis) ? Mathf.Abs(maxYaxis) : Mathf.Abs(minYaxis);
                    maxYaxis = maxYvalue;
                    minYaxis = -maxYvalue;


                }
            }
            #endregion
            #region ticks and min/max logic

            if (minXaxis != maxXaxis)
            {
                graph.xAxis.AxisMinValue = minXaxis;
                graph.xAxis.AxisMaxValue = maxXaxis;
            }

            // old logic for Xaxis, with rounded values, but with strange moving along axis
            /*
            if (maxXaxis * minXaxis > 0)
            {
                
                if (Mathf.Abs(maxXaxis) > 3)
                {
                    graph.xAxis.AxisNumTicks = 5;
                    graph.xAxis.AxisMinValue = minXaxis - 2 + Mathf.Abs(minXaxis % 2);
                    graph.xAxis.AxisMaxValue = maxXaxis + 2 - Mathf.Abs(maxXaxis % 2);
                }
                else
                {
                    graph.xAxis.AxisMinValue = minXaxis;
                    graph.xAxis.AxisMaxValue = maxXaxis;
                }
            }
            else
            {
                if (Mathf.Abs(maxXaxis) > 3)
                {
                    graph.xAxis.AxisNumTicks = 5;
                    graph.xAxis.AxisMinValue = minXaxis - 2 + Mathf.Abs(minXaxis % 2);
                    graph.xAxis.AxisMaxValue = -(minXaxis - 2 + Mathf.Abs(minXaxis % 2));
                }
                else
                {
                   graph.xAxis.AxisMinValue = minXaxis;
                   graph.xAxis.AxisMaxValue = maxXaxis;
                }
            }
         */
            if (maxYaxis * minYaxis > 0)
            {
                if (Mathf.Abs(maxYaxis) > 3)
                {
                    graph.yAxis.AxisNumTicks = 5;

                    graph.yAxis.AxisMinValue = minYaxis - 2 + Mathf.Abs(minYaxis % 2);
                    graph.yAxis.AxisMaxValue = maxYaxis + 2 - Mathf.Abs(maxYaxis % 2);
                }
                else
                {
                    graph.yAxis.AxisMinValue = minYaxis;
                    graph.yAxis.AxisMaxValue = maxYaxis;
                }
            }

            else
            {
                if (Mathf.Abs(maxYaxis) > 3)
                {

                    graph.yAxis.AxisNumTicks = 5;

                    graph.yAxis.AxisMinValue = minYaxis - 2 + Mathf.Abs(minYaxis % 2);
                    graph.yAxis.AxisMaxValue = -(minYaxis - 2 + Mathf.Abs(minYaxis % 2));


                }
                else
                {
                    graph.yAxis.AxisMinValue = minYaxis;
                    graph.yAxis.AxisMaxValue = maxYaxis;
                }
            }

            #endregion



        }
    }



}

