using UnityEngine;
using System.Collections;

public class CObstacle : MonoBehaviour
{
    public int m_WoodenType;
    public bool m_Prop;
    public Texture2D[] texture = new Texture2D[5];
	// Use this for initialization
	void Start () 
    {
        m_Prop = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
   
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       
    }

    internal void WoodenBom()
    {
        if (m_WoodenType != 0)
        {
            SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
            Sprite s;
            s = Sprite.Create(texture[m_WoodenType - 1], new Rect(0, 0, texture[m_WoodenType - 1].width, texture[m_WoodenType - 1].height), new Vector3(0.5f, 0.5f, 0));
            spr.sprite = s;
            m_Prop = true;
            BoxCollider2D b2d = gameObject.GetComponent<BoxCollider2D>();
            b2d.isTrigger = true;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            foreach (Transform child in gameObject.transform)
                Destroy(child.GetComponent<SpriteRenderer>());
        }
        else
        {
            Destroy(gameObject);
        }
    }    

    internal void SetObstacleResource(int type)
    {
        m_WoodenType = type;
		WoodenBom();
    }
}
