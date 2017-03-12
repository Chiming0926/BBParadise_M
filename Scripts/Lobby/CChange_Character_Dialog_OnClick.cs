using UnityEngine;
using System.Collections;

public class CChange_Character_Dialog_OnClick : MonoBehaviour {

	 AGCC m_agcc = null;
	// Use this for initialization
	void Start () 
	{
		m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
        if (m_agcc == null) return;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
    {
        if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.CHANGE_CHARACTER_DIALOG)
        {
            Debug.Log("gameObject.tag = " + gameObject.tag + " gameObject.name = " + gameObject.name);
            GameObject obj = GameObject.Find("character");
            CLobby lobby = null;
            if (obj)
            {
                lobby = FindObjectOfType(typeof(CLobby)) as CLobby;
                if (lobby == null) return;
            }
            if (gameObject.tag == "change_character_01")
            {
                m_agcc.m_PlayerInfo.SetCharaterNum(0);
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
                lobby.ResetCharacter(0, obj);

            }
            else if (gameObject.tag == "change_character_02")
            {
                m_agcc.m_PlayerInfo.SetCharaterNum(1);
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
                lobby.ResetCharacter(1, obj);
            }
            else if (gameObject.name == "Character_03")
            {
                m_agcc.m_PlayerInfo.SetCharaterNum(2);
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
                lobby.ResetCharacter(2, obj);
            }
            else if (gameObject.name == "Character_04")
            {
                m_agcc.m_PlayerInfo.SetCharaterNum(3);
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
                lobby.ResetCharacter(3, obj);
            }
            else if (gameObject.name == "Character_05")
            {
                m_agcc.m_PlayerInfo.SetCharaterNum(4);
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
                lobby.ResetCharacter(4, obj);
            }
        }
    }
}
