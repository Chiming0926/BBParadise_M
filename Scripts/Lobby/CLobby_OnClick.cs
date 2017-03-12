using UnityEngine;
using System.Collections;

public class CLobby_OnClick : MonoBehaviour {
    public static int show_role_dialog = 0;
    public static int show_start_game_dialog = 0;
	public static int show_change_character_dialog = 0;

    internal enum CURRENT_DIALOG 
	{ 
		LOBBY_DIALOG,
		ROLE_DIALOG, 
		START_GAME_DIALOG, 
		CHANGE_CHARACTER_DIALOG, 
		PROPS_DIALOG, 
		Thu, 
		Fri, 
		Sat
	};

	internal static CURRENT_DIALOG m_CurrentDialog = CURRENT_DIALOG.LOBBY_DIALOG;
    // Use this for initialization
    void Start ()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
		if (m_CurrentDialog != CURRENT_DIALOG.LOBBY_DIALOG)
			return;
		if (gameObject.tag == "role")
        {
			m_CurrentDialog = CURRENT_DIALOG.ROLE_DIALOG;
            Debug.Log("role down");
        }
        else if (gameObject.tag == "props")
        {
            m_CurrentDialog = CURRENT_DIALOG.PROPS_DIALOG;
            Debug.Log("props down");
        }
        else if (gameObject.tag == "potion")
        {
            Debug.Log("potion down");
        }
        else if (gameObject.tag == "auction")
        {
            Debug.Log("auction down");
        }
        else if (gameObject.tag == "setup")
        {
            Debug.Log("setup down");
        }
        else if (gameObject.tag == "start_game")
        {
            Debug.Log("start game");
            show_start_game_dialog = 1;
        }
		else if (gameObject.tag == "lobby_character")
        {
            Debug.Log("change character");
            m_CurrentDialog = CURRENT_DIALOG.CHANGE_CHARACTER_DIALOG;
        }
    }
}
