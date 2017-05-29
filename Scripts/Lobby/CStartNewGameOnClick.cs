using UnityEngine;
using System.Collections;

public class CStartNewGameOnClick : MonoBehaviour {

    AGCC agcc = null;
    // Use this for initialization
    void Start()
    {
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
        if (gameObject.name == "return")
        {
            agcc.CancelMatch();
            CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.START_GAME_DIALOG;
        }
    }
}
