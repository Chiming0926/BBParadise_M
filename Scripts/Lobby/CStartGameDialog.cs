using UnityEngine;
using System.Collections;

public class CStartGameDialog : MonoBehaviour {
    CLobby_OnClick.CURRENT_DIALOG dialog = CLobby_OnClick.CURRENT_DIALOG.NULL_DIALOG;
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
		if (dialog != CLobby_OnClick.m_CurrentDialog)
		{
	        if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.START_GAME_DIALOG)
	        {
				gameObject.GetComponent<Renderer>().enabled = true;
	            foreach (Transform child in transform)
	            {
	                child.GetComponent<Renderer>().enabled = true;
					child.gameObject.AddComponent<BoxCollider2D>();
	            }
	        }
	        else
	        {
				gameObject.GetComponent<Renderer>().enabled = false;
	            foreach (Transform child in transform)
	            {
	                child.GetComponent<Renderer>().enabled = false;
					Destroy(child.GetComponent<BoxCollider2D>());
	            }
	        }
			dialog = CLobby_OnClick.m_CurrentDialog;
		}
    }

    void OnMouseDown()
    {
    }
}
