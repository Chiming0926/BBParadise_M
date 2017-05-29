using UnityEngine;
using System.Collections;

public class CStattGameDilalogOnClick : MonoBehaviour {
    AGCC agcc = null;
    // Use this for initialization
    void Start () {
        agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
        if (agcc == null) return;
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnMouseDown()
    {
        if (gameObject.GetComponent<Renderer>().enabled == false)
            return;
        CLobby lobby = FindObjectOfType(typeof(CLobby)) as CLobby;
        lobby.GetComponent<AudioSource>().PlayOneShot(lobby.m_BomClip);
        if (gameObject.tag == "start_game_dialog_close")
        {
            if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.START_GAME_DIALOG)
            {
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
                lobby.TextControl(true);
            }
			if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.START_NEW_GAME_DIALOG)
				CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.START_GAME_DIALOG;
        }
        else if (gameObject.tag == "start_new_game_player_fight")
        {
			CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.START_NEW_GAME_DIALOG;
            agcc.Match();
        }
        else if (gameObject.tag == "start_new_game_challeage")
        {

        }
    }

}
