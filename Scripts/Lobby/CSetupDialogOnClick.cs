using UnityEngine;
using System.Collections;

public class CSetupDialogOnClick : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	 
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnMouseDown()
    {
        if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.SETUP_DIALOG)
        {
            CLobby lobby = FindObjectOfType(typeof(CLobby)) as CLobby;
            CSetupDialog setup = FindObjectOfType(typeof(CSetupDialog)) as CSetupDialog;
            lobby.GetComponent<AudioSource>().PlayOneShot(lobby.m_BomClip);
            Debug.Log("gameObject.tag = " + gameObject.tag + " gameObject.name = " + gameObject.name);
            if (gameObject.name == "Close")
            {
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
            }
            if (gameObject.name == "background_audio1")
            {
                setup.SetBackgroundAudio(gameObject.name);
            }
            if (gameObject.name == "background_audio2")
            {
                setup.SetBackgroundAudio(gameObject.name);
            }
            if (gameObject.name == "background_audio3")
            {
                setup.SetBackgroundAudio(gameObject.name);
            }
            if (gameObject.name == "background_audio4")
            {
                setup.SetBackgroundAudio(gameObject.name);
            }
            if (gameObject.name == "background_audio5")
            {
                setup.SetBackgroundAudio(gameObject.name);
            }
            if (gameObject.name == "special_audio1")
            {
                setup.SetSpecialAudio(gameObject.name);
            }
            if (gameObject.name == "special_audio2")
            {
                setup.SetSpecialAudio(gameObject.name);
            }
            if (gameObject.name == "special_audio3")
            {
                setup.SetSpecialAudio(gameObject.name);
            }
            if (gameObject.name == "special_audio4")
            {
                setup.SetSpecialAudio(gameObject.name);
            }
            if (gameObject.name == "special_audio5")
            {
                setup.SetSpecialAudio(gameObject.name);
            }
        }
    }
}
