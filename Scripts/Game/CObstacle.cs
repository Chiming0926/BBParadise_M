using UnityEngine;
using System.Collections;

public class CObstacle : MonoBehaviour
{
    public int m_WoodenType;
    private bool m_StartDectection;
    public Texture2D[] texture = new Texture2D[5];
	// Use this for initialization
	void Start () 
    {
        m_StartDectection = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
   
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("col.tag = " + col.tag);
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<CPlayer>().AddProps(m_WoodenType);
            Destroy(gameObject);
        }
    }

    internal void WoodenBom()
    {
        if (m_WoodenType != 0)
        {
            SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
            Sprite s;
            s = Sprite.Create(texture[m_WoodenType - 1], new Rect(0, 0, texture[m_WoodenType - 1].width, texture[m_WoodenType - 1].height), new Vector3(0.5f, 0.5f, 0));
            spr.sprite = s;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }    

    internal void SetWoodenType(int type)
    {
        m_WoodenType = (int)((type & 0xffff0000) >> 16);
    }
}
