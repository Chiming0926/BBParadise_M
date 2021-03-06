﻿using UnityEngine;
using System.Collections;

public class CChangeCharacterDialog : MonoBehaviour {

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
			if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.CHANGE_CHARACTER_DIALOG)
			{
				gameObject.GetComponent<Renderer>().enabled = true;
		        foreach (Transform child in transform)
		        {
                    Debug.Log("child name = " + child.gameObject.name);
		            child.GetComponent<Renderer>().enabled = true;
                    if (child.gameObject.name.Contains("Character"))
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

	public void OnClick()
	{
		Debug.Log("OnMouseDown");
	}
}
