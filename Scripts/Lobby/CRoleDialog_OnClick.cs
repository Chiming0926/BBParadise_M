using UnityEngine;
using System.Collections;

public class CRoleDialog_OnClick : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.ROLE_DIALOG)
        {
            CLobby lobby = FindObjectOfType(typeof(CLobby)) as CLobby;
            lobby.GetComponent<AudioSource>().PlayOneShot(lobby.m_BomClip);
            Debug.Log("gameObject.tag = " + gameObject.tag + " gameObject.name = " + gameObject.name);
            if (gameObject.name == "role_dialog_close")
            {
                CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
                lobby.TextControl(true);
            }
            else if (gameObject.name == "change_btn")
            {
                CRoleDialog role = FindObjectOfType(typeof(CRoleDialog)) as CRoleDialog;
                role.show_changing_dialog(true);
            }
            else if (gameObject.name == "change_dialog_close")
            {
                CRoleDialog role = FindObjectOfType(typeof(CRoleDialog)) as CRoleDialog;
                role.show_changing_dialog(false);
            }
            else if (gameObject.name == "change_dialog_ok")
            {

            }
        }
    }
}