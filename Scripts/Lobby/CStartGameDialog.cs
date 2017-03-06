using UnityEngine;
using System.Collections;

public class CStartGameDialog : MonoBehaviour {
    public static int show_new_game_dialog = 0;
    // Use this for initialization
    void Start () 
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (CLobby_OnClick.show_start_game_dialog == 1)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().enabled = true;
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().enabled = false;
                gameObject.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    void OnMouseDown()
    {
    }
}
