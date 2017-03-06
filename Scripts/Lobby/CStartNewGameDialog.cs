using UnityEngine;
using System.Collections;

public class CStartNewGameDialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Renderer>().enabled = false;
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (CStartGameDialog.show_new_game_dialog == 1)
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
    /*    if (CStartGameDialog.show_new_game_dialog == 1)
        {
            if (gameObject.tag == "start_game_dialog_close")
            {
                CStartGameDialog.show_new_game_dialog = 0;
            }
            return;
        }
        if (CStartGameDialog.show_new_game_dialog == 1)
        {
            if (gameObject.tag == "start_game_dialog_close")
            {
                CLobby_OnClick.show_start_game_dialog = 0;
            }
        }*/
    }
}
