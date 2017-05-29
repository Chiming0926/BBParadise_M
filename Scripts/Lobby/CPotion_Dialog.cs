using UnityEngine;
using System.Collections;

public class CPotion_Dialog : MonoBehaviour {
	CLobby_OnClick.CURRENT_DIALOG dialog = CLobby_OnClick.CURRENT_DIALOG.NULL_DIALOG;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (dialog != CLobby_OnClick.m_CurrentDialog)
		{
            CLobby lobby = FindObjectOfType(typeof(CLobby)) as CLobby;
            lobby.GetComponent<AudioSource>().PlayOneShot(lobby.m_BomClip);
            if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.POTION_DIALOG)
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
					Destroy(child.gameObject.GetComponent<BoxCollider2D>());
	            }
	        }
			dialog = CLobby_OnClick.m_CurrentDialog;
		}
	}
}
