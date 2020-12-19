using UnityEngine;
using System.Collections;

public class ExpCamCtrl : MonoBehaviour
{


    public Transform camForwardPos;
    public Transform camBackwardPos;
    public Transform camTopPos;
    public Transform camLeftPos;
    public Transform camRightPos;

    private bool flagDoOnce = true;

    void Start()
    {
        transform.position = camForwardPos.position;
        transform.rotation = camForwardPos.rotation;
    }
    
    public void ChooseCamPos(bool forward, bool backward, bool top, bool left, bool right)
    {
        if (forward)
        {
            transform.position = camForwardPos.position;
            transform.rotation = camForwardPos.rotation;
        }
        else if (backward)
        {
            transform.position = camBackwardPos.position;
            transform.rotation = camBackwardPos.rotation;
        }
        else if (top)
        {
            transform.position = camTopPos.position;
            transform.rotation = camTopPos.rotation;
        }
        else if (left)
        {
            transform.position = camLeftPos.position;
            transform.rotation = camLeftPos.rotation;
        }
        else if (right)
        {
            transform.position = camRightPos.position;
            transform.rotation = camRightPos.rotation;
        }

    }
}
