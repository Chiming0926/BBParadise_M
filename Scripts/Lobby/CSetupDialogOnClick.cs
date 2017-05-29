using UnityEngine;
using System.Collections;

public class CSetupDialogOnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.SETUP_DIALOG)
        {
            CLobby lobby = FindObjectOfType(typeof(CLobby)) as CLobby;
            lobby.GetComponent<AudioSource>().PlayOneShot(lobby.m_BomClip);
            Debug.Log("gameObject.tag = " + gameObject.tag + " gameObject.name = " + gameObject.name);
            if (gameObject.name == "Close")
            {
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;

            }
        }
    }
}
