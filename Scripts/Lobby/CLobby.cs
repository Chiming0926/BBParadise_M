using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CLobby : MonoBehaviour
{
    AGCC m_agcc = null;
    public AudioClip m_BomClip;
    public Texture2D[] character_textures = new Texture2D[5];
	CLobby_OnClick.CURRENT_DIALOG dialog;

	IEnumerator GetImageAndShow() 
	{
        WWW www;
        if (m_agcc.m_TestMode == false) 
            www = new WWW("https://graph.facebook.com/" + m_agcc.m_PlayerInfo.fbUserId + "/picture?type=large");
        else
		    www = new WWW("https://graph.facebook.com/" + "938736822806986" + "/picture?type=large"); 
    	yield return www;
   		GameObject obj = GameObject.Find ("character_01");
		if (obj != null)
		{
        	Sprite sp = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector3(0.5f, 0.5f, 0));
        	obj.GetComponent<SpriteRenderer>().sprite = sp;
    	}
    }
	
    void Start ()
    {
        CLobby_OnClick.m_CurrentDialog = CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG;
        m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
        if (m_agcc == null) return;

		StartCoroutine(GetImageAndShow());
		
        for (int i=0; i< gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.name == "character")
            {
				ResetCharacter(m_agcc.m_PlayerInfo.character_num, gameObject.transform.GetChild(i).gameObject);
            }
        }

        /* update user info */
        TextControl(true);
    }
	
    internal void TextControl(bool show)
    {
        if (show)
        {
            /* update user info */
            GameObject objCanvas = GameObject.Find("Canvas");
            foreach (Transform child in objCanvas.transform)
            {
                Text tx = child.GetComponent<Text>();
                if (child.name == "TextCake")
                    tx.text = m_agcc.m_PlayerInfo.cake.ToString();
                if (child.name == "TextMoney")
                    tx.text = m_agcc.m_PlayerInfo.money.ToString();
                if (child.name == "TextGem")
                    tx.text = m_agcc.m_PlayerInfo.gem.ToString();
            }
        }
        else
        {
            /* update user info */
            GameObject objCanvas = GameObject.Find("Canvas");
            foreach (Transform child in objCanvas.transform)
            {
                Text tx = child.GetComponent<Text>();
                if (child.name == "TextCake")
                    tx.text = "";
                if (child.name == "TextMoney")
                    tx.text = "";
                if (child.name == "TextGem")
                    tx.text = "";
            }
        }
    }

	void Update ()
    {
		if (dialog != CLobby_OnClick.m_CurrentDialog)
		{
			if (CLobby_OnClick.m_CurrentDialog == CLobby_OnClick.CURRENT_DIALOG.LOBBY_DIALOG)
	        {
                foreach (Transform child in transform)
	            {
                    child.gameObject.AddComponent<BoxCollider2D>();
                }
			}
			else
			{
                foreach (Transform child in transform)
                {
               		Destroy(child.gameObject.GetComponent<BoxCollider2D>());
                }
            }
			dialog = CLobby_OnClick.m_CurrentDialog;
		}
	}

	internal void ResetCharacter(int index, GameObject obj)
	{
	    SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
        Sprite s = Sprite.Create(character_textures[index], new Rect(0, 0, character_textures[index].width, 
            character_textures[index].height), new Vector3(0.5f, 0.5f, 0));
        spr.sprite = s;
	}

    void OnMouseDown()
    {
        if (gameObject.GetComponent<Renderer>().enabled == false)
            return;
        if (gameObject.tag == "start_game_dialog_close")
        {
            CLobby_OnClick.show_start_game_dialog = 0;
        }
    }
}
