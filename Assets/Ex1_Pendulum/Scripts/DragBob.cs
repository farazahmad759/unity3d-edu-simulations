using UnityEngine;
using System.Collections;

public class DragBob : MonoBehaviour {

    public float distance =0f;
    public float angle = 0;
    public float finalAngle = 0;
    public Vector3 mousePosition;

    public bool dragIsOn=false;

    public Transform pendulum;

    public Transform top;

    public Vector3 objPosition;

 

    public Vector3 _lockedYPosition;
    public Vector3 offset;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        
        if (dragIsOn)
        {

                 pendulum.transform.eulerAngles = new Vector3(0, 0, angle  );



        }

        if (!dragIsOn)
        {

        //    pendulum.transform.eulerAngles = new Vector3(0, 0, this.transform.localEulerAngles.z);
        }


    }
    public void OnMouseDrag()
    {
        
        dragIsOn = true;

        offset = pendulum.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance)-offset;
         objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        objPosition = new Vector3(objPosition.x, objPosition.y, transform.position.z);

    

         angle = Mathf.DeltaAngle(Mathf.Atan2(0, 0) * Mathf.Rad2Deg, Mathf.Atan2(objPosition.y, objPosition.x) * Mathf.Rad2Deg)+90;





    }

    void OnMouseDown()
    {
       
        



    }
    

    void OnMouseUp()
    {
        dragIsOn = false;
    }



}
