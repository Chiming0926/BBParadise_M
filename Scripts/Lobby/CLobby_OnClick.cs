using UnityEngine;
using System.Collections;

public class CLobby_OnClick : MonoBehaviour {
    public static int show_role_dialog = 0;
    public static int show_start_game_dialog = 0;
	public static int show_change_character_dialog = 0;

    /* counter for scaling up the button */
    private static int m_ScaleTimer = -1;
    private static GameObject m_ScaleObj;
    private static CURRENT_DIALOG m_NextDialog;
    private static Vector3 m_OriScale;

    internal enum CURRENT_DIALOG 
	{ 
		NULL_DIALOG,
		LOBBY_DIALOG,
		ROLE_DIALOG, 
		START_GAME_DIALOG,
		START_NEW_GAME_DIALOG,
		CHANGE_CHARACTER_DIALOG, 
		PROPS_DIALOG,
		POTION_DIALOG,
        SETUP_DIALOG,
        AUCTION_DIALOG,
        WBALL_DIALOG,
        STORE_DIALOG,
	};

	internal static CURRENT_DIALOG m_CurrentDialog = CURRENT_DIALOG.LOBBY_DIALOG;
    // Use this for initialization
    void Start ()
    {
		
    }

    static void ScalupButton(GameObject obj, CURRENT_DIALOG dialog)
    {
        /* Scale up the button */
        m_OriScale = obj.transform.localScale;
        obj.transform.localScale = new Vector3(1.05f, 1.05f , 1.0f);

        m_ScaleObj = obj;
        m_NextDialog = dialog;
        m_ScaleTimer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_ScaleTimer != -1)
        {
            m_ScaleTimer++;
            if (m_ScaleTimer > 90)
            {
                //Debug.Log(m_OriTransform.localScal.x);
                m_ScaleObj.transform.localScale = m_OriScale;
                m_CurrentDialog = m_NextDialog;
                m_ScaleTimer = -1;
            }
        }
    }

    void OnMouseDown()
    {
        if (m_CurrentDialog != CURRENT_DIALOG.LOBBY_DIALOG)
			return;
        CLobby lobby = FindObjectOfType(typeof(CLobby)) as CLobby;
        lobby.GetComponent<AudioSource>().PlayOneShot(lobby.m_BomClip);
        Debug.Log("OnMouseDown gameObject.tag = " +  gameObject.tag);
		if (gameObject.tag == "role")
        {
            //m_CurrentDialog = CURRENT_DIALOG.ROLE_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.ROLE_DIALOG);
            Debug.Log("role down");
            lobby.TextControl(false);
        }
        else if (gameObject.tag == "props")
        {
            //m_CurrentDialog = CURRENT_DIALOG.PROPS_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.PROPS_DIALOG);
            Debug.Log("props down");
        }
        else if (gameObject.tag == "potion")
        {
            Debug.Log("potion down");
			//m_CurrentDialog = CURRENT_DIALOG.POTION_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.POTION_DIALOG);
        }
        else if (gameObject.tag == "auction")
        {
            Debug.Log("auction down");
            //m_CurrentDialog = CURRENT_DIALOG.AUCTION_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.AUCTION_DIALOG);
        }
        else if (gameObject.tag == "setup")
        {
            Debug.Log("setup down");
            //m_CurrentDialog = CURRENT_DIALOG.SETUP_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.SETUP_DIALOG);
        }
        else if (gameObject.tag == "start_game")
        {
            Debug.Log("start game");
            //m_CurrentDialog = CURRENT_DIALOG.START_GAME_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.START_GAME_DIALOG);
            lobby.TextControl(false);
        }
		else if (gameObject.name == "role_background")
        {
            Debug.Log("change character");
            //m_CurrentDialog = CURRENT_DIALOG.CHANGE_CHARACTER_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.CHANGE_CHARACTER_DIALOG);
        }
        else if (gameObject.name == "water_ball")
        {
            Debug.Log("water_ball");
            //m_CurrentDialog = CURRENT_DIALOG.WBALL_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.WBALL_DIALOG);
        }
        else if (gameObject.name == "store")
        {
            //m_CurrentDialog = CURRENT_DIALOG.STORE_DIALOG;
            ScalupButton(gameObject, CURRENT_DIALOG.STORE_DIALOG);
        }
    }
}
