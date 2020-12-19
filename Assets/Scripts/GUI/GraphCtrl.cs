using UnityEngine;
using System.Collections;

public class GraphCtrl : MonoBehaviour
{

    public GameObject graphLeft;
    public GameObject graphRight;
    public GameObject graphUp;
    public GameObject graphFront;
    public GameObject graphBack;

    private bool left = false;
    private bool right = false;
    private bool up = false;
    private bool front = false;
    private bool back = false;

    public Transform startPosition;

    // Use this for initialization
    void Start () {
       Vector3 positionOfGraph = transform.position;
        transform.position = new Vector3(positionOfGraph.x, -2+startPosition.position.y, positionOfGraph.z);
    }
	

    public void ChangeCamView(bool forward, bool backward, bool top, bool leftV, bool rightV)
    {
        left = leftV;
        right = rightV;
        up = top;
        front = forward;
        back = backward;

        graphFront.SetActive(front);
        graphBack.SetActive(back);
        graphLeft.SetActive(left);
        graphRight.SetActive(right);
        graphUp.SetActive(up);
    }

    public void EnableDisableGraph()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
    
}
