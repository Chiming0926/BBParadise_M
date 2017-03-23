using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class CPlayer : MonoBehaviour
{
    enum PLAYER_DIRECTION
    {
        UP     = 0,
        DOWN   = 1,
        RIGHT  = 2,
        LEFT   = 3,
        NULL   = 4
    };

	private const float PLAYER_SIZE = 0.8f;

    private const int KEYBOARD_TEST = 1;
    public GameObject water_ball;
    private float   speed = 0.08f;
    public  int     wball_cnt = 0;
    public static GameObject static_wball;
    public Texture2D []texture_DOWN = new Texture2D[4];
	public Texture2D []texture_UP = new Texture2D[4];
	public Texture2D []texture_RIGHT = new Texture2D[8];
	public Texture2D []texture_LEFT = new Texture2D[8];
    private const int reset_factor = 6;
    private PLAYER_DIRECTION direct = 0;
    private PLAYER_DIRECTION pic_direct = 0;
    private int pic = 0;
    private int cnt = 0;
    private static int cpos_x = 0;
    private static int cpos_y = 0;
    public static GameObject player;

	public int m_MaxWball = 0;
    public int m_MaxPower = 0;
    private bool m_Controller = false;

    private const float start_point_y = 3.6f; 
	void Start ()
    {
        direct = PLAYER_DIRECTION.NULL;
        player = gameObject;
        static_wball = water_ball;
		wball_cnt = 0;
		m_MaxWball = 1;
        m_MaxPower = 1;
    }

	internal void SetControllerStatus(bool controlStatus)
	{
		m_Controller = controlStatus;
	}

    void set_player_pic(PLAYER_DIRECTION dir)
    {
        if (dir == PLAYER_DIRECTION.NULL)
            return;
        if (pic_direct != dir)
        {
            pic = 0;
            cnt = 0;
        }
        pic_direct = dir;
        bool changePic = false;
        if ((cnt / reset_factor) >= 1)
        {
            if ((cnt % reset_factor) == 0)
                changePic = true;
        } 
        if (changePic)
        {
            SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
            Sprite s;
            switch (direct)
            {
                case PLAYER_DIRECTION.UP:
                    s = Sprite.Create(texture_UP[pic], new Rect(0, 0, texture_UP[pic].width, texture_UP[pic].height), new Vector3(0.5f, 0.5f, 0));
                    pic++;
                    if (pic >= 4)
                        pic = 0;
                    break;
                case PLAYER_DIRECTION.DOWN:
                    s = Sprite.Create(texture_DOWN[pic], new Rect(0, 0, texture_DOWN[pic].width, texture_DOWN[pic].height), new Vector3(0.5f, 0.5f, 0));
                    pic++;
                    if (pic >= 4)
                        pic = 0;
                    break;
                case PLAYER_DIRECTION.RIGHT:
                    s = Sprite.Create(texture_RIGHT[pic], new Rect(0, 0, texture_RIGHT[pic].width, texture_RIGHT[pic].height), new Vector3(0.5f, 0.5f, 0));
                    pic++;
                    if (pic >= 8)
                        pic = 0;
                    break;
                case PLAYER_DIRECTION.LEFT:
                    s = Sprite.Create(texture_LEFT[pic], new Rect(0, 0, texture_LEFT[pic].width, texture_LEFT[pic].height), new Vector3(0.5f, 0.5f, 0));
                    pic++;
                    if (pic >= 8)
                        pic = 0;
                    break;
                default:
                    s = Sprite.Create(texture_UP[pic], new Rect(0, 0, texture_UP[pic].width, texture_UP[pic].height), new Vector3(0.5f, 0.5f, 0));
                    break;
            }
            spr.sprite = s;
        }
        cnt++;
    }
    // 3.4 
    void reset_y(Vector3 pos)
    {
        for (int i=0; i< CGameManager.map_height; i++)
        {
            if (pos.y <= 4.5f - i*1.0f
                && pos.y > 4.5f - (i+1) * 1.0f)
            {
                if (4.5f - i * 1.0f - pos.y > pos.y - 4.5f + (i + 1) * 1.0f)
                {
                    pos = new Vector3(pos.x, 4.5f - (i + 1) * 1.0f, -1);
                }
                else
                {
                    pos = new Vector3(pos.x, 4.5f - i * 1.0f, -1);
                }
                return;
            }
        }
    }

    void reset_x(Vector3 pos)
    {
        for (int i = 0; i < CGameManager.map_width; i++)
        {
            if (pos.x >= -4.5f + i * 1.0f
                && pos.x < -4.5f + (i + 1) * 1.0f)
            {
                if (pos.x + 4.5f - i * 1.0f > 4.5f + (i + 1) * 1.0f - pos.x)
                {
                }
                else
                {
                }
                return;
            }
        }
    }

    internal void AddProps(int type)
    {
        switch (type)
        {
            case 1:
                m_MaxWball++;
                break;
            case 2:
                if (m_MaxPower < 10)
                    m_MaxPower++;
                break;
            default:
                break;
        }
    }

	bool map_out_of_range(int x, int y)
	{
		if (x < 0 || y < 0 || x > 15 || y > 9)
		{
			Debug.Log("out of ramge x = " + x + " y = " + y);
			return true;
		}
		return false;
	}
	
    public void create_wball()
    {
        Vector3 pos = this.transform.position;
    /*    for (int i = 0; i < CBackground.map_height; i++)
        {
            if (pos.y <= 4.5f - i * 1.0f
                && pos.y > 4.5f - (i + 1) * 1.0f)
            {
                if (4.5f - i * 1.0f - pos.y > pos.y - 4.5f + (i + 1) * 1.0f)
                {
                    pos = new Vector3(pos.x, 4.5f - (i + 1) * 1.0f, -1);
                    cpos_y = i - 1;
                }
                else
                {
                    cpos_y = i;
                    pos = new Vector3(pos.x, 4.5f - i * 1.0f, -1);
                }
                break;
            }
        }

        for (int i = 0; i < CBackground.map_width; i++)
        {
            if (pos.x >= -7.5f + i * 1.0f
                && pos.x < -7.5f + (i + 1) * 1.0f)
            {
                if (pos.x + 7.5f - i * 1.0f > 7.5f + (i + 1) * 1.0f - pos.x)
                {
                    cpos_x = i;
                    pos = new Vector3(-7.5f + (i + 1) * 1.0f, pos.y, -1);
                }
                else
                {
                    cpos_x = i+1;
                    pos = new Vector3(-7.5f + i * 1.0f , pos.y, -1);
                }
                break;
            }
        }
*/
		int array_y = (int)(4.5f - pos.y + PLAYER_SIZE / 1.5f);
        int array_x = (int)(pos.x + 7.5f + PLAYER_SIZE / 2);
		
		if (map_out_of_range(array_x, array_y ))
		{
			wball_cnt--;
			return;
		}
        //CGameManager.map[array_y, array_x] = 2;

		GameObject obj = Instantiate(static_wball, new Vector3(-7.5f + array_x , 4.5f - array_y), player.transform.rotation) as GameObject;
		obj.name = gameObject.name;
        obj.GetComponent<CWBall>().SetPower(m_MaxPower);
    }

    public void BeginMove()
    {
    }

    public void EndMove()
    {
		direct = PLAYER_DIRECTION.NULL;
    }

    public void UpdateDirection(int direction)
    {
        direct = (PLAYER_DIRECTION)direction;
    }
/*
	public void UpdateDirection(Vector3 direction)
    {
        if (Math.Abs(direction.x) == Math.Abs(direction.y))
        {
            // don't change 
			return;
        }
        else if (Math.Abs(direction.x) > Math.Abs(direction.y))
        {
            if (direction.x > 0)
                direct = PLAYER_DIRECTION.RIGHT;
            else
                direct = PLAYER_DIRECTION.LEFT;
        }
        else
        {
            if (direction.y > 0)
				direct = PLAYER_DIRECTION.UP;
            else
                direct = PLAYER_DIRECTION.DOWN;
        }
    }
   */
   
    void player_move(PLAYER_DIRECTION dir)
    {
        var r = this.GetComponent<Rigidbody2D>();
		bool sendMsg = false;
		if (gameObject.name == "Player01")
		{
			if (dir != PLAYER_DIRECTION.NULL)
				Debug.Log("player_move dir = " + dir);
		}
        if (dir == PLAYER_DIRECTION.RIGHT)
        {
            set_player_pic(PLAYER_DIRECTION.RIGHT);
			if (m_Controller == false)
				return;
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.RIGHT) == false)
            {
                if (gameObject.transform.position.x <= 7.5f)
                {
                    gameObject.transform.position += new Vector3(speed, 0, 0);
					sendMsg = true;
                }
            }
        }
        else if (dir == PLAYER_DIRECTION.LEFT)
        {
            set_player_pic(PLAYER_DIRECTION.LEFT);
			if (m_Controller == false)
				return;
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.LEFT) == false)
            {
                if (gameObject.transform.position.x >= -7.5)
                {
                    gameObject.transform.position += new Vector3(-speed, 0, 0);
					sendMsg = true;
                }
            }
        }
        else if (dir == PLAYER_DIRECTION.UP)
        {
            set_player_pic(PLAYER_DIRECTION.UP);
			if (m_Controller == false)
				return;
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.UP) == false)
            {
                if (gameObject.transform.position.y <= 4.5f)
                {
                    var sr = this.GetComponent<SpriteRenderer>();
                    gameObject.transform.position += new Vector3(0, speed, 0);
                    sr.sortingOrder = (int)((4.5f - gameObject.transform.position.y)) + 7;
                    if (4.5f - gameObject.transform.position.y != 0.0f)
                        sr.sortingOrder++;
					sendMsg = true;
                }
            }
        }
        else if (dir == PLAYER_DIRECTION.DOWN)
        {
            Debug.Log("@@@@@@@@@@@@1 m_Controller = " + m_Controller);
            set_player_pic(PLAYER_DIRECTION.DOWN);
			if (m_Controller == false)
				return;
            Debug.Log("@@@@@@@@@@@@2");
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.DOWN) == false)
            {
                if (gameObject.transform.position.y >= -4.5f)
                {
                    var sr = this.GetComponent<SpriteRenderer>();
                    gameObject.transform.position += new Vector3(0, -speed, 0);
                    Debug.Log("@@@@@@@@@@@@3");
                    sr.sortingOrder = (int)((4.5f - gameObject.transform.position.y)) + 7;
                    if (4.5f - gameObject.transform.position.y != 0.0f)
                        sr.sortingOrder++;
					sendMsg = true;
                }
            }
        }
        else
        {
            r.velocity = new Vector2(0.0f, 0.0f);
        }
		if (sendMsg)
		{
            CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;
            gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
        }
        direct = dir;
    }

    internal void SetPosition(float x, float y)
    {
        var r = this.GetComponent<Rigidbody2D>();
        var sr = this.GetComponent<SpriteRenderer>();
        gameObject.transform.position = new Vector3(x, y, 0);
        sr.sortingOrder = (int)((4.5f - gameObject.transform.position.y)) + 7;
        if (4.5f - gameObject.transform.position.y != 0.0f)
            sr.sortingOrder++;
    }

	void changeDirect(PLAYER_DIRECTION dir)
	{
		if (dir != direct)
		{
			direct = dir;
			if (direct == PLAYER_DIRECTION.RIGHT || direct == PLAYER_DIRECTION.LEFT)
			{
				reset_y(gameObject.transform.position);
            }
			else if (direct == PLAYER_DIRECTION.UP || direct == PLAYER_DIRECTION.DOWN)
			{
	//			reset_x(Vector3 pos);
			}
		}
	}

    void Update ()
    {
        // var r = this.GetComponent<Rigidbody2D>();
       // set_player_pic(direct);
		//Debug.Log("@@@@@@ player move");
        player_move(direct);
       
        if (Input.GetKey(KeyCode.RightArrow))
        {
            set_player_pic(direct);
            player_move(PLAYER_DIRECTION.RIGHT);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            set_player_pic(PLAYER_DIRECTION.LEFT);
            player_move(PLAYER_DIRECTION.LEFT);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            set_player_pic(PLAYER_DIRECTION.UP);
            player_move(PLAYER_DIRECTION.UP);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            set_player_pic(PLAYER_DIRECTION.DOWN);
            player_move(PLAYER_DIRECTION.DOWN);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            if (wball_cnt < 1)
            {
                create_wball();
                wball_cnt++;
            }
        }
    }
    public void OnClick()
    {
        if (wball_cnt < m_MaxWball)
        {
            create_wball();
            wball_cnt++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    /*
        Debug.Log("@@@@@ OnCollisionEnter2D");
        switch (direct)
        {
            case PLAYER_DIRECTION.UP:
                gameObject.transform.position += new Vector3(0, -0.01f, 0);
                break;
            case PLAYER_DIRECTION.DOWN:
                gameObject.transform.position += new Vector3(0, 0.01f, 0);
                break;
            case PLAYER_DIRECTION.RIGHT:
                gameObject.transform.position += new Vector3(-0.01f, 0, 0);
                break;
            case PLAYER_DIRECTION.LEFT:
                gameObject.transform.position += new Vector3(0.01f, 0, 0);
                break;
            default:
                break;
        }
    */
    }

    void OnCollisionStay(Collision collision)
    {
    }
    public static void create_wball_from()
    {

    }

    public void wball_destroy()
    {
        wball_cnt--;
        if (wball_cnt < 0)
            wball_cnt = 0;
    }

    bool restricted_area(float x, float y, PLAYER_DIRECTION dir)
    {
		return false;
		/*
		int array_x = (int)x, array_y = (int)y;
        switch (direct)
        {
            case PLAYER_DIRECTION.UP:
                y+=speed; 
				array_y = (int)(4.5f-y+PLAYER_SIZE);
                break;
            case PLAYER_DIRECTION.DOWN:
                y-=speed;
				array_y = (int)(4.5f-y+PLAYER_SIZE);
                break;
            case PLAYER_DIRECTION.RIGHT:
                x+=speed;
                break;
            case PLAYER_DIRECTION.LEFT:
                x-=speed;
                break;
            default:
                break;
        }
		array_y = (int)(4.5f-y+PLAYER_SIZE);
        array_x = (int)(x+7.5f+PLAYER_SIZE);
		if (map_out_of_range(array_x, array_y))
			return true;
		if (CBackground.map[(int)array_y, (int)array_x] != 0)
		{
			return true;
		}
        return false;
        */
    }

    public class PlayerData
    {
        public static string    user_account;
        public static string    user_password;
        public static string    user_mail;
        public static string    user_name;
        public static bool      fb_login;
        public static int       character_num;
    }
}
