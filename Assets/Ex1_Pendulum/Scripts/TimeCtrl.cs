using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimeCtrl : MonoBehaviour
{
    [SerializeField]
    private bool Paused = false;
    [SerializeField]
    private float gameSpeed = 1f;
    [SerializeField]
    private float gameSpeedStep = 0.1f;
    private float gameSpeedMin = 0.1f;
    private float gameSpeedMax = 10f;
	// Use this for initialization
	void Start () {

        Paused = false;
    }
	
	// Update is called once per frame
	void Update () {

	    if (Paused)
	    {
	        Time.timeScale = 0;
	    }
	    else
	    {
	        Time.timeScale = gameSpeed;
	    }

	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        Application.Quit();
	    }
	
	}

    public void PauseGame()
    {
        Paused = true;

    }

    public void UnPauseGame()
    {
        Paused = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IncreaseGameSpeed(float increase)
    {
        gameSpeed += increase;
        gameSpeed = Mathf.Clamp(gameSpeed, gameSpeedMin, gameSpeedMax);
    }

    public void DecreaseGameSpeed(float increase)
    {
        gameSpeed -= increase;
        gameSpeed = Mathf.Clamp(gameSpeed, gameSpeedMin, gameSpeedMax);
    }

    public float GetGameSpeed()
    {
        return gameSpeed;
    }
    public bool IsItPaused()
    {
        return Paused;
    }

}
