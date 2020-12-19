using UnityEngine;
using System.Collections;

public class GraphCapture : MonoBehaviour {

    public bool grab = false;
    public bool capture = false;


    public Texture2D tex;
   
     void OnPostRender()
    {
        if (grab)
        {
            capture = true;
            grab = false;

            int width = 390;
            int height = 210;
//            var startX = 4.5f;
//            var startY = 3f;

             tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();


        }
    }

}
