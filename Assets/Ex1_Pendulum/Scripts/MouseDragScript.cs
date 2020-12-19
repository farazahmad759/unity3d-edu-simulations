using UnityEngine;
using System.Collections;

public class MouseDragScript : MonoBehaviour {

    public float angleFromCenter = 0.0f;

    // top of the rope ,cords
    public Transform topOfTheRope;
    public PendulumCtrl pendulumCtrl;

    private Vector3 screenPoint;
    private Vector3 mousePosition;


    public TimeCtrl _TimeCtrl;
    private bool pauseModeClicked = false;

    private bool canDragMouse = false;

    public Transform pendulumPhys;

    void Update()
    {
        if (Time.timeScale <= 0.001f)
        {
            canDragMouse = true;
        }
        else
        {
            canDragMouse = true;
        }
        if (Time.timeScale < 0.001) // time is stopped
        {
            // its only used in pause mode/drag mode to move pendulum
            float height = pendulumCtrl.GetLength();
            float zCord = pendulumCtrl.GetzCord();
            transform.position = new Vector3(topOfTheRope.position.x, height, zCord);
        }
    }

    void FixedUpdate()
    {
        SimulateGraphicPendulum();   
    }

    void SimulateGraphicPendulum()
    {
        //gets cordinates on circle and update position   
        float height = pendulumCtrl.GetLength();
        float zCord = pendulumCtrl.GetzCord();
        transform.position = new Vector3(topOfTheRope.position.x, height, zCord);
      
    }

    void OnMouseDown()
    {
        screenPoint = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(screenPoint);
        pauseModeClicked = _TimeCtrl.IsItPaused();
    }

    void OnMouseDrag(){
        
        // mouse drag on pendulum
       
        if (topOfTheRope && pendulumCtrl && canDragMouse)
        {
            
            _TimeCtrl.PauseGame();
           
            screenPoint = Input.mousePosition;

            screenPoint.z = Vector3.Distance(Camera.main.transform.position, transform.position);
            
            mousePosition = Camera.main.ScreenToWorldPoint(screenPoint);
           
            Vector3 curMousePos= new Vector3(topOfTheRope.position.x, mousePosition.y, mousePosition.z);

            pendulumCtrl.SetPendulumPosition(topOfTheRope.position, curMousePos,pendulumCtrl.lengthPendulum);
        
        }
    }


    void OnMouseUp()
    {
        Cursor.visible = true;
        if (!pauseModeClicked)
        {
            _TimeCtrl.UnPauseGame();
        }
    }
   
}

