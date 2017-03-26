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

    // Use this for initialization
    void Start ()
    {
    }
	
    void create_ws()
    {
		var up   = ws_up.GetComponent<SpriteRenderer>();
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
    }

    void destroy_ws()
    {
        destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f));
        destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));

        for (int i=0; i<power; i++)
        {
            destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (i + 1)));
            destroy_obstacle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.0f * (-1-i)));
            destroy_obstacle(new Vector2(gameObject.transform.position.x + 1.0f * (i + 1), gameObject.transform.position.y));
            destroy_obstacle(new Vector2(gameObject.transform.position.x + 1.0f * (-1-i), gameObject.transform.position.y));
        }
    }

	bool map_out_of_range(int x, int y)
	{
		if (x < 0 || y < 0 || x > 15 || y > 9)
		{
			Debug.Log("wball out of ramge x = " + x + " y = " + y);
			return true;
		}
		return false;
	}

    void destroy_obstacle(Vector2 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, new Vector2(0.1f, 0.1f), 0.0f);
        foreach (Collider2D collider in colliders)
        {
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
			if (collider.tag == "wball")
			{
				collider.gameObject.GetComponent<CWBall>().Bom();
			}
            if (collider.tag == "wooden")
            {
                collider.gameObject.GetComponent<CObstacle>().WoodenBom();
                continue;
            }
            Destroy(collider.gameObject);
        }
    }
    
    internal void SetPower(int pw)
    {
        power = pw;
    }

	internal void Bom()
	{
		cnt = 120;
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
        if (cnt < 120)
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
        else if (cnt >= 120 && cnt <130)
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
    }
}
