using UnityEngine;
using System.Collections;

public partial class CGameManager : MonoBehaviour
{
    private static int MAX_LEVEL_NUM = 3;
    private static bool DEBUG = true;

    private float oldTime; 
    private int frame = 0; 
    private float frameRate = 0f; 
    private const float INTERVAL = 0.5f; 

    internal void BBDebug(string msg)
    {
        if (DEBUG)
            Debug.Log(msg);
    }
    private void Awake()
    {
        Time.captureFramerate = 30;
    }

    // Use this for initialization
    void Start()
    {
        Time.captureFramerate = 30;
        QualitySettings.vSyncCount = 2;
        InitMessageModule();
        oldTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        float time = Time.realtimeSinceStartup - oldTime; 
        if (time >= INTERVAL) 
        {
            frameRate = frame / time; 
            oldTime = Time.realtimeSinceStartup; 
            frame = 0; 
        }
    }

    void OnGUI()
    {

//        GUI.Box(new Rect(200, 150, 600, 300), "");
 //       GUI.Label(new Rect(250, 200, 200, 30), "FPS : " + frameRate);

    }
}
