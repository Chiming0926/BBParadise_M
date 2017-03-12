using UnityEngine;
using System.Collections;

public class CLobby : MonoBehaviour
{
    AGCC m_agcc = null;

    public Texture2D[] character_textures = new Texture2D[5];
    void Start ()
    {
        m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
        if (m_agcc == null) return;

        for (int i=0; i< gameObject.transform.childCount; i++)
        {
            Debug.Log(gameObject.transform.GetChild(i).gameObject.name);
            if (gameObject.transform.GetChild(i).gameObject.name == "character")
            {
				ResetCharacter(m_agcc.m_PlayerInfo.character_num, gameObject.transform.GetChild(i).gameObject);
            }
        }
	}
	
	void Update ()
    {
	
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
