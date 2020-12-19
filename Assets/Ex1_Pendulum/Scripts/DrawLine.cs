using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
    public GameObject lineOrigin;
    public GameObject lineTarget;
    public LineRenderer _LineRenderer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        _LineRenderer.SetPosition(0, lineOrigin.transform.position);
        _LineRenderer.SetPosition(1,lineTarget.transform.position);
	
	}
}
