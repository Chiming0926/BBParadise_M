using UnityEngine;
using System.Collections;

public class CLobby : MonoBehaviour
{
	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    void OnMouseDown()
    {
        if (gameObject.GetComponent<Renderer>().enabled == false)
            return;
        if (gameObject.tag == "start_game_dialog_close")
        {
            CLobby_OnClick.show_start_game_dialog = 0;
        }
    }
}
