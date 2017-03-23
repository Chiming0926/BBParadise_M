using UnityEngine;
using System.Collections;

public partial class CGameManager : MonoBehaviour
{
    private static int MAX_LEVEL_NUM = 2;
    private static bool DEBUG = false;


    internal void BBDebug(string msg)
    {
        if (DEBUG)
            Debug.Log(msg);
    }

    // Use this for initialization
    void Start ()
    {
        InitMessageModule();
        CreateMap(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
