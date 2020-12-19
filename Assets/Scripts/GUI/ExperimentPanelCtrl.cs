using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExperimentPanelCtrl : MonoBehaviour
{


    [Header("Pointers")]
    public TimeCtrl _TimeCtrl;
    public Text gameSpeedText;
    public GameObject cameraOptionsMenu;


    [Space(5)]
    [Header("Game params")]
    public float gameSpeed = 1f;

    //screenshots
    private string screenShotName = "Screen1.jpg";



    [Space(5)]
    [Header("Experiment pointers")]
    public ExpCamCtrl experimentCamera;

    public GraphCtrl graphController;

    // experiment state handlers for next and previous buttons
    private ExperimentStateMachine currentExperimentStateMachine;


    // flags
    private bool isFullscreen = false;
    private bool isGameStopped = false;

    // Use this for initialization
    void Start () {
        currentExperimentStateMachine = ExperimentStateMachine.GetExperimentStateMachine();

    }
	

    void UpdateGameSpeedText()
    {
        gameSpeed = _TimeCtrl.GetGameSpeed();
        gameSpeedText.text = gameSpeed.ToString("F1") + "x";
    }


    #region PublicHandlesAndInterfaces


    public bool CheckForExperimentRestart()
    {
        return isGameStopped;
    }

    #region BUTTONS

    public void ButtonPreviousClick()
    {
        currentExperimentStateMachine.GoToPreviousState();
    }

    public void ButtonPauseClick()
    {
        _TimeCtrl.PauseGame();
    }

    public void ButtonPlayClick()
    {
        isGameStopped = false;
        _TimeCtrl.UnPauseGame();
    }

    public void ButtonStopClick()
    {
        _TimeCtrl.PauseGame();
        isGameStopped = true;
    }

    public void ButtonNextClick()
    {
        currentExperimentStateMachine.GoToNextState();
    }

    public void ButtonReloadClick()
    {
        _TimeCtrl.RestartGame();
    }

    public void ButtonSpeedDecreaseClick()
    {
        _TimeCtrl.DecreaseGameSpeed(0.1f);
        UpdateGameSpeedText();
    }

    public void ButtonSpeedIncreaseClick()
    {
        _TimeCtrl.IncreaseGameSpeed(0.1f);
        UpdateGameSpeedText();
    }

    public void ButtonEyeClick()
    {
        experimentCamera.GetComponent<Camera>().orthographic = !experimentCamera.GetComponent<Camera>().orthographic;
    }

    public void ButtonCameraModeClick()
    {
        cameraOptionsMenu.SetActive(!cameraOptionsMenu.activeInHierarchy);
    }

    public void ButtonGridClick()
    {
        graphController.EnableDisableGraph();
    }

    public void ButtonCaptureClick()
    {
        try
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
//        string path = "C:/PhysicsClass";
            ScreenCapture.CaptureScreenshot(path + "/Screenshot.png");
        }
        catch (UnityException e)
        {
            Debug.Log(e);
        }
    }

    public void ButtonFullScreenClick()
    {
        Screen.fullScreen = !Screen.fullScreen;
        
    }

    public void ButtonSpeedStepUp()
    {
        _TimeCtrl.IncreaseGameSpeed(1f);
        UpdateGameSpeedText();

    }

    public void ButtonSpeedStepDown()
    {
        _TimeCtrl.DecreaseGameSpeed(1f);
        UpdateGameSpeedText();

    }


    public void CamMenu_ButtonLeft()
    {
        experimentCamera.ChooseCamPos(false,false,false,true,false);
        graphController.ChangeCamView(false, false, false, true, false);
    }
    public void CamMenu_ButtonRight() {
        experimentCamera.ChooseCamPos(false, false, false, false, true);
        graphController.ChangeCamView(false, false, false, false, true);
    }
    public void CamMenu_ButtonTop() {
        experimentCamera.ChooseCamPos(false, false, true, false, false);
        graphController.ChangeCamView(false, false, true, false, false);
    }
    public void CamMenu_ButtonFront() {
        experimentCamera.ChooseCamPos(true, false, false, false, false);
        graphController.ChangeCamView(true, false, false, false, false);
    }
    public void CamMenu_ButtonBack() {
        experimentCamera.ChooseCamPos(false, true, false, false, false);
        graphController.ChangeCamView(false, true, false, false, false);
    }


    #endregion
    #endregion
}
