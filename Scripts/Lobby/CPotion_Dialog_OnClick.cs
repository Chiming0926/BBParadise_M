using UnityEngine;
using System.Collections;

public class CPotion_Dialog_OnClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnMouseDown()
    {
        if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.POTION_DIALOG)
        {
            Debug.Log("gameObject.tag = " + gameObject.tag + " gameObject.name = " + gameObject.name);
            if (gameObject.name == "potion_close")
            {
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
            }
            else if (gameObject.name == "potion_over_view")
            {
            
            }
        }
    }
}
