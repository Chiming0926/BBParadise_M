using UnityEngine;
using System.Collections;

public class CRoleDialog : MonoBehaviour
{
    CLobby_OnClick.CURRENT_DIALOG dialog = CLobby_OnClick.CURRENT_DIALOG.NULL_DIALOG;
    bool m_ShowDialog = false;
    private GUIStyle m_GuiStyle;
    AGCC m_agcc;
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        show_changing_dialog(false);
        m_GuiStyle = new GUIStyle();
        m_GuiStyle.fontSize = 36;
        m_GuiStyle.font = Resources.Load("fonts/GenJyuuGothicX-Monospace-Bold") as Font;
        m_GuiStyle.normal.textColor = Color.white;
    }

    internal void show_changing_dialog(bool show)
    {
        GameObject obj = GameObject.Find("role_change_dialog");
        if (show)
        {
            if (obj.GetComponent<Renderer>().enabled == false)
            {
                obj.GetComponent<Renderer>().enabled = true;
                foreach (Transform child in obj.transform)
                {
                    child.GetComponent<Renderer>().enabled = true;
                    child.gameObject.AddComponent<BoxCollider2D>();
                }
            }
        }
        else
        {
            if (obj.GetComponent<Renderer>().enabled == true)
            {
                obj.GetComponent<Renderer>().enabled = false;
                foreach (Transform child in obj.transform)
                {
                    Debug.Log("child = " + child.name);
                    child.GetComponent<Renderer>().enabled = false;
                    Destroy(child.gameObject.GetComponent<BoxCollider2D>());
                }
            }
        }
        m_ShowDialog = show;
        m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
        if (m_agcc == null) return;
    }

    // Update is called once per frame
    void Update()
    {
		if (dialog != CLobby_OnClick.m_CurrentDialog)
		{
	        if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.ROLE_DIALOG)
	        {
                gameObject.GetComponent<Renderer>().enabled = true;
	            foreach (Transform child in transform)
	            {
	                child.GetComponent<Renderer>().enabled = true;
					child.gameObject.AddComponent<BoxCollider2D>();
	            }
                show_changing_dialog(false);
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

    void OnGUI()
    {

        if (m_ShowDialog)
        {
            GUI.Label(new Rect(730, 250, 200, 50), m_agcc.m_PlayerInfo.nickname, m_GuiStyle);
        }
    }
}
