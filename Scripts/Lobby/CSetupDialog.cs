using UnityEngine;
using System.Collections;

public class CSetupDialog : MonoBehaviour
{
    CLobby_OnClick.CURRENT_DIALOG dialog = CLobby_OnClick.CURRENT_DIALOG.NULL_DIALOG;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (dialog != CLobby_OnClick.m_CurrentDialog)
        {
            if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.SETUP_DIALOG)
            {
                gameObject.GetComponent<Renderer>().enabled = true;
                foreach (Transform child in transform)
                {
                    child.GetComponent<Renderer>().enabled = true;
                    child.gameObject.AddComponent<BoxCollider2D>();
                }
                SetSpecialAudio("special_audio1");
                SetBackgroundAudio("background_audio1");
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

    internal void SetSpecialAudio(string name)
    {
        for (int i=0; i<5; i++)
        {
            string objName = "special_audio" + (i + 1).ToString("0");
            GameObject obj = GameObject.Find(objName);
            if (obj != null)
            {
                obj.GetComponent<SpriteRenderer>().enabled = name != objName ? false : true;
            }
        }
    }

    internal void SetBackgroundAudio(string name)
    {
        for (int i = 0; i < 5; i++)
        {
            string objName = "background_audio" + (i + 1).ToString("0");
            GameObject obj = GameObject.Find(objName);
            if (obj != null)
            {
                obj.GetComponent<SpriteRenderer>().enabled = name != objName ? false : true;
            }
        }
    }
}
