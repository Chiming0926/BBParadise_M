using UnityEngine;
using System.Collections;

public class CRoleDialog : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CLobby_OnClick.show_role_dialog == 1)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    void OnMouseDown()
    {
        if (gameObject.GetComponent<Renderer>().enabled == false)
            return;
        if (gameObject.tag == "role_dialog_close")
        {
            CLobby_OnClick.show_role_dialog = 0;
        }
    }
}
