using UnityEngine;
using System.Collections;

public class CChangeCharacterDialog : MonoBehaviour {

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
		if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.CHANGE_CHARACTER_DIALOG)
		{
			gameObject.GetComponent<Renderer>().enabled = true;
	        foreach (Transform child in transform)
	        {
	            child.GetComponent<Renderer>().enabled = true;
	        }
		}
		else
		{
			gameObject.GetComponent<Renderer>().enabled = false;
	        foreach (Transform child in transform)
	        {
	            child.GetComponent<Renderer>().enabled = false;
	        }
		}
	}

	public void OnClick()
	{
		Debug.Log("OnMouseDown");
	}
}
