using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Amplitude : MonoBehaviour {

    public Rigidbody bob;
    public Transform bobHolder;
    public Slider slider;
    

    public float amp;
	

    

    public void SetBobValue()
    {
        bobHolder.eulerAngles = new Vector3(0,0,slider.value);
    }
}
