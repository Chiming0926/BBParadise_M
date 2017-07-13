using UnityEngine;
using System.Collections;

public class CWBall : MonoBehaviour
{
    public GameObject ws_up;
    public GameObject ws_down;
    public GameObject ws_left;
    public GameObject ws_right;
    public Texture2D[] texture = new Texture2D[2];
    public Texture2D textureboom;
    public AudioClip  m_BomClip;

    private const int reset_factor = 5;
    private int pic = 0;
    private int cnt = 0;
    public int power = 4;

    private int m_BomCnt = 300;

    // Use this for initialization
    void Start ()
    {
    }
	
    bool create_specific_ws(GameObject obj, Vector2 pos)
    {
        var sp = obj.GetComponent<SpriteRenderer>();
        sp.sortingOrder = 18;
        GameObject ins = Instantiate(obj, new Vector3(pos.x, pos.y, 0), gameObject.transform.rotation) as GameObject;
        ins.name = "ws"; 
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, new Vector2(0.1f, 0.1f), 0.0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.name == "ws")
                continue;
            return true;
        }
        return false;
    }

    void create_ws()
    {
        /*	var up   = ws_up.GetComponent<SpriteRenderer>();
            var down = ws_down.GetComponent<SpriteRenderer>();
            for (int i = 0; i < power; i++)
            {
                up.sortingOrder = 18;
                down.sortingOrder = 18;
                Instantiate(ws_up, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (i + 1), 0), gameObject.transform.rotation);
                Instantiate(ws_down, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (-1-i), 0), gameObject.transform.rotation);
                Instantiate(ws_right, new Vector3(gameObject.transform.position.x + 1.0f * (i + 1), gameObject.transform.position.y, 0), gameObject.transform.rotation);
                Instantiate(ws_left, new Vector3(gameObject.transform.position.x + 1.0f * (-1 - i), gameObject.transform.position.y, 0), gameObject.transform.rotation);
            }
            */
        for (int i = 0; i < power; i++)
        {
            if (create_specific_ws(ws_up, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (i + 1))))
                break;  
        }
        for (int i = 0; i < power; i++)
        {
            if (create_specific_ws(ws_down, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (-1 - i))))
                break;
        }
        for (int i = 0; i < power; i++)
        {
            if (create_specific_ws(ws_right, new Vector2(gameObject.transform.position.x + 1.0f * (i + 1), gameObject.transform.position.y)))
                break;
        }
        for (int i = 0; i < power; i++)
        {
            if (create_specific_ws(ws_left, new Vector2(gameObject.transform.position.x + 1.0f * (-1 - i), gameObject.transform.position.y)))
                break;
        }
    }

    void destroy_ws()
    {
        destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f));
        destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));

        for (int i=0; i<power; i++)
        {
            if (destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (i + 1))))
                break;
        }
        for (int i = 0; i < power; i++)
        {
            if (destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (-1 - i))))
                break;
        }
        for (int i = 0; i < power; i++)
        {
            if (destroy_obstacle(new Vector2(gameObject.transform.position.x + 1.0f * (i + 1), gameObject.transform.position.y)))
                break;
        }
        for (int i = 0; i < power; i++)
        {
            if (destroy_obstacle(new Vector2(gameObject.transform.position.x + 1.0f * (-1 - i), gameObject.transform.position.y)))
                break;
        }
    }

    bool destroy_obstacle(Vector2 pos)
    {
        bool touch = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, new Vector2(0.1f, 0.1f), 0.0f);
        foreach (Collider2D collider in colliders)
        {
            touch = true;
            if (collider.tag == "Player")
            {
				/* Send death message */
				CGameManager gameManager = FindObjectOfType(typeof(CGameManager)) as CGameManager;
				if (gameManager != null)
				{
					gameManager.check_death_people(collider.gameObject.name);
				}
				continue;
            }
			if (collider.name == "Flag")
            {
				/* don't destroy */
				continue;
            }
			if (collider.tag == "block")
			{
				continue;
			}
			if (collider.tag == "wball")
			{
                Debug.Log("@@@@@@@@@@@@ wball");
				collider.gameObject.GetComponent<CWBall>().Bom();
                continue;

            }
            if (collider.tag == "wooden")
            {
                collider.gameObject.GetComponent<CObstacle>().WoodenBom();
                continue;
            }
            if (collider.gameObject.name == "ws")
            {
                touch = false;
            }
            Destroy(collider.gameObject);
        }
        return touch;
    }
    
    internal void SetPower(int pw)
    {
        power = pw;
    }

	internal void Bom()
	{
		cnt = m_BomCnt + 7;
	}

	void DestroyWBall()
	{
		destroy_ws();
		Debug.Log("wball name = " + gameObject.name);
		GameObject obj = GameObject.Find(gameObject.name);
		obj.GetComponent<CPlayer>().wball_destroy();
        Destroy(gameObject);
	}

    // Update is called once per frame
    void Update ()
    {
        bool changePic = false;
        if (cnt < m_BomCnt)
        {
            if ((cnt / reset_factor) >= 1)
            {
                if ((cnt % reset_factor) == 0)
                    changePic = true;
            }

            if (changePic)
            {
                pic++;
                SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
                Sprite s = Sprite.Create(texture[pic % 2], new Rect(0, 0, texture[pic % 2].width, texture[pic % 2].height), new Vector3(0.5f, 0.5f, 0));
                spr.sprite = s;
            }
        }
        else if (cnt >= m_BomCnt && cnt < (m_BomCnt+10))
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(m_BomClip);
            create_ws();
            SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
            Sprite s = Sprite.Create(textureboom, new Rect(0, 0, textureboom.width, textureboom.height), new Vector3(0.5f, 0.5f, 0));
            spr.sprite = s;
        }
        else
        {
        	DestroyWBall();
        }
        cnt++;

        /* check collider status */
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.5f, 0.5f), 0.0f);
        if (colliders.Length == 0)
        {
            Collider2D c2d = gameObject.GetComponent<Collider2D>();
            if (c2d == null)
            {
                gameObject.AddComponent<BoxCollider2D>();
            }
        }

    }
}
