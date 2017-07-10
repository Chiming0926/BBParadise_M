//#define MOVEDETECT
#define USING_VELOCITY
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class CPlayer : MonoBehaviour
{
    enum PLAYER_DIRECTION
    {
        UP     = 0,
        DOWN   = 1,
        RIGHT  = 2,
        LEFT   = 3,
        NULL   = 4,
        UP_STOP= 5,
        DOWN_STOP = 6,
        RIGHT_STOP = 7,
        LEFT_STOP = 8,
    };

    enum PLAYER_MOUNTS
    {
        NULL = 0,
        TORTOISE = 3, 
        UFO = 4,
    };


#if USING_VELOCITY
    public float moveSpeed = 2.5f;
    public float m_StaticMoveSpeed = 2.5f;
#endif

    private const float PLAYER_SIZE = 0.8f;

    private const int KEYBOARD_TEST = 1;
    public GameObject water_ball;
    private float   speed = 0.03f;
    public  int     wball_cnt = 0;
    public static GameObject static_wball;
    public Texture2D []texture_DOWN = new Texture2D[4];
	public Texture2D []texture_UP = new Texture2D[4];
	public Texture2D []texture_RIGHT = new Texture2D[8];
	public Texture2D []texture_LEFT = new Texture2D[8];

    public Texture2D[] m_TortoiseUP = new Texture2D[2];
    public Texture2D[] m_TortoiseDOWN = new Texture2D[2];
    public Texture2D[] m_TortoiseLEFT = new Texture2D[2];
    public Texture2D[] m_TortoiseRIGHT = new Texture2D[2];

    public Texture2D[] m_UfoUP = new Texture2D[2];
    public Texture2D[] m_UfoDOWN = new Texture2D[2];
    public Texture2D[] m_UfoLEFT = new Texture2D[2];
    public Texture2D[] m_UfoRIGHT = new Texture2D[2];

    public Texture2D[] m_MountUP = new Texture2D[2];
    public Texture2D[] m_MountDOWN = new Texture2D[2];
    public Texture2D[] m_MountLEFT = new Texture2D[2];
    public Texture2D[] m_MountRIGHT = new Texture2D[2];


    private Vector2 m_OriBoxCollierOffset;

    private PLAYER_MOUNTS m_Mounts = 0;
    private GameObject m_MountObj;
	private float m_MountsPositionY = -0.6f;
	private float m_MountsPositionX = -0.1f;

    private const int reset_factor = 6;
    private PLAYER_DIRECTION direct = 0;
    private PLAYER_DIRECTION m_PreDirect = 0;
    private PLAYER_DIRECTION pic_direct = 0;
    private int pic = 0;
    private int cnt = 0;
    private static int cpos_x = 0;
    private static int cpos_y = 0;
    public static GameObject player;

	public int m_MaxWball = 0;
    public int m_MaxPower = 0;
    private bool m_Controller = false;

    public AudioClip m_PutBallClip;

    private GameObject m_MoveDetectObj;

    public Texture2D[] m_PlayerBom = new Texture2D[2];
    private int  m_PlayerDeadCnt = 0;

    private Vector3 m_PrePosition;
    private int m_Poid;
    private AGCC m_agcc;
    private bool m_RemotePlayer = true;

    internal class UnHandleMessage
    {
        internal int x;
        internal int y;
        internal int dir;
    }

    List<UnHandleMessage> m_UnhandleMessage = new List<UnHandleMessage>();

    void Start ()
    {
        direct = PLAYER_DIRECTION.NULL;
        player = gameObject;
        static_wball = water_ball;
		wball_cnt = 0;
		m_MaxWball = 1;
        m_MaxPower = 1;

        m_Mounts = PLAYER_MOUNTS.NULL;

        /* Load resource */
        int i = 0;
        for (i=1; i<=2; i++)
        {
            m_TortoiseUP[i-1] = Resources.Load("Textures/Game/Mounts/Tortoise/Up/"+i.ToString("00")) as Texture2D;
            m_TortoiseDOWN[i-1] = Resources.Load("Textures/Game/Mounts/Tortoise/Down/" + i.ToString("00")) as Texture2D;
            m_TortoiseLEFT[i-1] = Resources.Load("Textures/Game/Mounts/Tortoise/Left/" + i.ToString("00")) as Texture2D;
            m_TortoiseRIGHT[i-1] = Resources.Load("Textures/Game/Mounts/Tortoise/Right/" + i.ToString("00")) as Texture2D;

            m_UfoUP[i-1] = Resources.Load("Textures/Game/Mounts/Ufo/Up/" + i.ToString("00")) as Texture2D;
            m_UfoDOWN[i-1] = Resources.Load("Textures/Game/Mounts/Ufo/Down/" + i.ToString("00")) as Texture2D;
            m_UfoLEFT[i-1] = Resources.Load("Textures/Game/Mounts/Ufo/Left/" + i.ToString("00")) as Texture2D;
            m_UfoRIGHT[i-1] = Resources.Load("Textures/Game/Mounts/Ufo/Right/" + i.ToString("00")) as Texture2D;
        }

        /* init Mounts obj */
        m_MountObj = new GameObject();
        m_MountObj.transform.parent = gameObject.transform;
        m_MountObj.name = "Mount";
        SpriteRenderer spr = m_MountObj.AddComponent<SpriteRenderer>();

        /* init audio */
        m_PutBallClip = Resources.Load("Audio/Game/wball/PutBall") as AudioClip;

        /* init Move Detect Obj */
        m_MoveDetectObj = new GameObject();
        m_MoveDetectObj.name = "MoveDetectObj";
      //  BoxCollider2D c2d = m_MoveDetectObj.AddComponent<BoxCollider2D>();
     //   m_MoveDetectObj.transform.position = gameObject.transform.position;
        SpriteRenderer sprMove = m_MoveDetectObj.AddComponent < SpriteRenderer >() as SpriteRenderer;
        //   c2d.offset = new Vector2(0.001998782f, -0.365293f);
        //   c2d.size = new Vector2(0.7680514f, 0.6568841f);

        /* save origin box collier offset */
        BoxCollider2D b2d = gameObject.GetComponent<BoxCollider2D>();
        m_OriBoxCollierOffset = b2d.offset;

        m_PrePosition = transform.position;

        m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
    }

    internal void SetPlayerInfo(string name, int poid, bool remotePlayer)
    {
        foreach(Transform child in transform)
        {
            if (child.name == "Canvas")
            {
                foreach (Transform grandson in child)
                {
                    if (grandson.name == "Text")
                    {
                        Text txt = grandson.GetComponent<Text>();
                        txt.text = name;
                    }
                }
            }
        }
        m_Poid = poid;
        m_RemotePlayer = remotePlayer;
    }

	internal void SetControllerStatus(bool controlStatus)
	{
		m_Controller = controlStatus;
	}

    internal void SetMountStatus(int mt)
    {
        PLAYER_MOUNTS pm = (PLAYER_MOUNTS)mt;

        for (int i=0; i<2; i++)
        {
            switch (pm)
            {
                case PLAYER_MOUNTS.TORTOISE:
                    m_MountUP[i] = m_TortoiseUP[i];
                    m_MountDOWN[i] = m_TortoiseDOWN[i];
                    m_MountLEFT[i] = m_TortoiseLEFT[i];
                    m_MountRIGHT[i] = m_TortoiseRIGHT[i];
                    m_MountObj.transform.localScale = new Vector3(0.75f, 0.8f, 1);
                    m_MountObj.transform.position = new Vector3(0.2f, -0.671f, 1);
                    if (m_RemotePlayer == false)
                    {
                        BoxCollider2D b2d = gameObject.GetComponent<BoxCollider2D>();
                        b2d.offset = new Vector2(-0.023f, -0.6f);
                    }
                    break;
                case PLAYER_MOUNTS.UFO:
                    m_MountUP[i] = m_UfoUP[i];
                    m_MountDOWN[i] = m_UfoDOWN[i];
                    m_MountLEFT[i] = m_UfoLEFT[i];
                    m_MountRIGHT[i] = m_UfoRIGHT[i];
                    break;
            }
        }

        Sprite sMount = Sprite.Create(m_MountUP[0], new Rect(0, 0, m_MountUP[0].width, m_MountUP[0].height), new Vector3(0.5f, 0.5f, 0));
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        SpriteRenderer srMount = m_MountObj.GetComponent<SpriteRenderer>();
        srMount.sortingOrder = sr.sortingOrder + 1;
        srMount.sprite = sMount;
        m_Mounts = (PLAYER_MOUNTS)mt;
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

            SpriteRenderer sprMount = m_MountObj.GetComponent<SpriteRenderer>();
            Sprite sMount; 

            if (m_Mounts == PLAYER_MOUNTS.NULL)
            {
                sMount = null;
                switch (direct)
                {
                    case PLAYER_DIRECTION.UP:
                    case PLAYER_DIRECTION.UP_STOP:
                        s = Sprite.Create(texture_UP[pic], new Rect(0, 0, texture_UP[pic].width, texture_UP[pic].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 4)
                            pic = 0;
                        break;
                    case PLAYER_DIRECTION.DOWN:
                    case PLAYER_DIRECTION.DOWN_STOP:
                        s = Sprite.Create(texture_DOWN[pic], new Rect(0, 0, texture_DOWN[pic].width, texture_DOWN[pic].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 4)
                            pic = 0;
                        break;
                    case PLAYER_DIRECTION.RIGHT:
                    case PLAYER_DIRECTION.RIGHT_STOP:
                        s = Sprite.Create(texture_RIGHT[pic], new Rect(0, 0, texture_RIGHT[pic].width, texture_RIGHT[pic].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 8)
                            pic = 0;
                        break;
                    case PLAYER_DIRECTION.LEFT:
                    case PLAYER_DIRECTION.LEFT_STOP:
                        s = Sprite.Create(texture_LEFT[pic], new Rect(0, 0, texture_LEFT[pic].width, texture_LEFT[pic].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 8)
                            pic = 0;
                        break;
                    default:
                        s = Sprite.Create(texture_UP[pic], new Rect(0, 0, texture_UP[pic].width, texture_UP[pic].height), new Vector3(0.5f, 0.5f, 0));
                        break;
                }
            }
            else
            {
                Debug.Log("@@@@@@@@@@@@@@@@123 hahaha");
                sMount = null;
                switch (direct)
                {
                    case PLAYER_DIRECTION.UP:
                        s = Sprite.Create(texture_UP[0], new Rect(0, 0, texture_UP[0].width, texture_UP[0].height), new Vector3(0.5f, 0.5f, 0));
                        sMount = Sprite.Create(m_MountUP[pic], new Rect(0, 0, m_MountUP[0].width, m_MountUP[0].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 2)
                            pic = 0;
                        break;
                    case PLAYER_DIRECTION.DOWN:
                        s = Sprite.Create(texture_DOWN[0], new Rect(0, 0, texture_DOWN[0].width, texture_DOWN[0].height), new Vector3(0.5f, 0.5f, 0));
                        sMount = Sprite.Create(m_MountDOWN[pic], new Rect(0, 0, m_MountDOWN[pic].width, m_MountDOWN[pic].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 2)
                            pic = 0;
                        break;
                    case PLAYER_DIRECTION.RIGHT:
                        s = Sprite.Create(texture_RIGHT[0], new Rect(0, 0, texture_RIGHT[0].width, texture_RIGHT[0].height), new Vector3(0.5f, 0.5f, 0));
                        sMount = Sprite.Create(m_MountRIGHT[pic], new Rect(0, 0, m_MountRIGHT[pic].width, m_MountRIGHT[pic].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 2)
                            pic = 0;
                        break;
                    case PLAYER_DIRECTION.LEFT:
                        s = Sprite.Create(texture_LEFT[0], new Rect(0, 0, texture_LEFT[0].width, texture_LEFT[0].height), new Vector3(0.5f, 0.5f, 0));
                        sMount = Sprite.Create(m_MountLEFT[0], new Rect(0, 0, m_MountLEFT[0].width, m_MountLEFT[0].height), new Vector3(0.5f, 0.5f, 0));
                        pic++;
                        if (pic >= 2)
                            pic = 0;
                        break;
                    default:
                        s = Sprite.Create(texture_UP[pic], new Rect(0, 0, texture_UP[0].width, texture_UP[0].height), new Vector3(0.5f, 0.5f, 0));
                        break;
                }
            } 

            spr.sprite = s;
            if (sMount != null)
                sprMount.sprite = sMount;
        }
        cnt++;
    }

    internal void AddProps(int type)
    {
        Debug.Log("AddProps type = " + type);
        switch (type)
        {
            case 1:
                m_MaxWball++;
                break;
            case 2:
                if (m_MaxPower < 10)
                    m_MaxPower++;
                break;
            case 3:
				SetMountStatus(3);
                break;
            default:
                break;
        }
    }
    
    public void create_wball(float x, float y)
    {
		GameObject obj = Instantiate(static_wball, new Vector3(Mathf.RoundToInt(x) , Mathf.RoundToInt(y)), player.transform.rotation) as GameObject;
		obj.name = gameObject.name;
        obj.GetComponent<CWBall>().SetPower(m_MaxPower);
        gameObject.GetComponent<AudioSource>().PlayOneShot(m_PutBallClip);
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

#if REMOVE
    void player_move(PLAYER_DIRECTION dir)
    {
		bool sendMsg = false;
        set_player_pic(dir);
        if (dir == PLAYER_DIRECTION.RIGHT)
        {
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.RIGHT) == false)
            {
#if MOVEDETECT
                m_MoveDetectObj.transform.position += new Vector3(speed, 0, 0);
                sendMsg = true;
#elif USING_VELOCITY
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);

#else
               if (gameObject.transform.position.x <= 17.3f)
                   gameObject.transform.position += new Vector3(speed, 0, 0);
#endif
                sendMsg = true;
            }
        }
        else if (dir == PLAYER_DIRECTION.LEFT)
        {
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.LEFT) == false)
            {
#if MOVEDETECT

                if (m_MoveDetectObj.transform.position.x >= 0.0f)
                    m_MoveDetectObj.transform.position += new Vector3(-speed, 0, 0);
#elif USING_VELOCITY
                GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, 0);
#else

                if (gameObject.transform.position.x >= 0.0f)
                    gameObject.transform.position += new Vector3(-speed, 0, 0);
#endif
                sendMsg = true;
            }
        }
        else if (dir == PLAYER_DIRECTION.UP)
        {
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.UP) == false)
            {
#if MOVEDETECT
                 if (m_MoveDetectObj.transform.position.y <= 5.0f)
                    m_MoveDetectObj.transform.position += new Vector3(0, speed, 0);
#elif USING_VELOCITY
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveSpeed);
#else
                if (gameObject.transform.position.y <= 10.6)
                    gameObject.transform.position += new Vector3(0, speed, 0);
#endif
                var sr = this.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
                sendMsg = true;
            }
        }
        else if (dir == PLAYER_DIRECTION.DOWN)
        {
            if (restricted_area(gameObject.transform.position.x, gameObject.transform.position.y, PLAYER_DIRECTION.DOWN) == false)
            {
#if MOVEDETECT
                if (m_MoveDetectObj.transform.position.y >= -4.5f)
                    m_MoveDetectObj.transform.position += new Vector3(0, -speed, 0);
#elif USING_VELOCITY
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -moveSpeed);
#else
                if (gameObject.transform.position.y >= 0)
                    gameObject.transform.position += new Vector3(0, -speed, 0);
#endif
                var sr = this.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
                sendMsg = true;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

		if (sendMsg)
		{
            /* check collider status */
            bool touch = false;
#if MOVEDETECT
            Collider2D[] colliders = Physics2D.OverlapBoxAll(m_MoveDetectObj.transform.position, new Vector2(0.58f, 0.6f), 0.0f);
#else
            Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.5f, 0.5f), 0.0f);
#endif
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Player")
                {
                    continue;
                }

                if (collider.gameObject.name == "MoveDetectObj")
                {
                    continue;
                }

                if (collider.gameObject.tag == "wooden")
                {
                 //   Debug.Log("collider.gameObject.name = " + collider.gameObject.name);
                    if (collider.gameObject.GetComponent<CObstacle>().m_WoodenType != 0 &&
                        collider.gameObject.GetComponent<CObstacle>().m_Prop == true)
                    {
                        CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;
                        gameMgr.SendMountMessage(gameObject, collider.gameObject.GetComponent<CObstacle>().m_WoodenType);
                        Destroy(collider.gameObject);
                        continue;
                    }
                }
                touch = true;
            }

            if(m_Controller == false)
                return;
#if MOVEDETECT
            if (touch == false)
            {
                gameObject.transform.position = m_MoveDetectObj.transform.position;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = m_MoveDetectObj.GetComponent<SpriteRenderer>().sortingOrder;
#endif
				{
	                CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;
                    if (gameObject.transform.position != m_PrePosition)
                    {
                        if (direct == dir)
                        {
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                            m_PrePosition = gameObject.transform.position;
                        }
                    }
                    else
                        Debug.Log("don't move");
	                if (m_Mounts != PLAYER_MOUNTS.NULL)
	                {
	                    m_MountObj.transform.position = gameObject.transform.position + new Vector3(m_MountsPositionX, m_MountsPositionY, 0);
	                    SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
	                    SpriteRenderer srMount = m_MountObj.GetComponent<SpriteRenderer>();
	                    srMount.sortingOrder = sr.sortingOrder + 1;
	                }
	                set_player_pic(dir);
				}
#if MOVEDETECT
            }
            else
            {
                m_MoveDetectObj.transform.position = gameObject.transform.position;
                m_MoveDetectObj.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
            }
#endif
        }
        
        direct = dir;
    }
#endif
    float m_SyncTime = 0;
    float m_MomentSyncTimer = 0;

#if TEST
    void player_move(PLAYER_DIRECTION dir)
    {
        set_player_pic(dir);
        if (m_RemotePlayer) return;
        CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;
        
        if (dir == PLAYER_DIRECTION.RIGHT)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
        }
        else if (dir == PLAYER_DIRECTION.LEFT)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, 0);
        }
        else if (dir == PLAYER_DIRECTION.UP)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveSpeed);
            var sr = this.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
        }
        else if (dir == PLAYER_DIRECTION.DOWN)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -moveSpeed);
            var sr = this.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        /* check collider status */
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.5f, 0.5f), 0.0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                continue;
            }

            if (collider.gameObject.name == "MoveDetectObj")
            {
                continue;
            }

            if (collider.gameObject.tag == "wooden")
            {
                if (collider.gameObject.GetComponent<CObstacle>().m_WoodenType != 0 &&
                    collider.gameObject.GetComponent<CObstacle>().m_Prop == true)
                {
                    gameMgr.SendMountMessage(gameObject, collider.gameObject.GetComponent<CObstacle>().m_WoodenType);
                    Destroy(collider.gameObject);
                    continue;
                }
            }
        }
        if (m_RemotePlayer == false)
        {
            if (gameObject.transform.position != m_PrePosition)
            {
                if (direct == dir)
                {
                    gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
              //      m_MomentSyncTimer = Mathf.MoveTowards(m_MomentSyncTimer, 0.075f, Time.deltaTime);
              //      if (m_MomentSyncTimer >= m_SyncTime)
              //      {
              //          if (direct != PLAYER_DIRECTION.NULL)
              //              gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
              //          m_MomentSyncTimer = 0;
              //      }
                }
                else
                {
               //     m_MomentSyncTimer = Mathf.MoveTowards(m_MomentSyncTimer, 0.075f, Time.deltaTime);
                //    if (m_MomentSyncTimer >= m_SyncTime)
                //    {
                //        if (direct != PLAYER_DIRECTION.NULL)
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                //        m_MomentSyncTimer = 0;
                //    }
                }
                m_PrePosition = gameObject.transform.position;
            }
            else
            {
                if (direct != PLAYER_DIRECTION.NULL)
                {
                    switch(direct)
                    {
                        case PLAYER_DIRECTION.LEFT:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.LEFT_STOP);
                            break;
                        case PLAYER_DIRECTION.RIGHT:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.RIGHT_STOP);
                            break;
                        case PLAYER_DIRECTION.UP:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.UP_STOP);
                            break;
                        case PLAYER_DIRECTION.DOWN:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.DOWN_STOP);
                            break;
                    }
                }
            }
        }
        
        if (m_Mounts != PLAYER_MOUNTS.NULL)
        {
            m_MountObj.transform.position = gameObject.transform.position + new Vector3(m_MountsPositionX, m_MountsPositionY, 0);
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            SpriteRenderer srMount = m_MountObj.GetComponent<SpriteRenderer>();
            srMount.sortingOrder = sr.sortingOrder + 1;
        }
        set_player_pic(dir);
    }
    void player_move(PLAYER_DIRECTION dir)
    {
        set_player_pic(dir);
        direct = dir;
        if (m_RemotePlayer) return;
        CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;

        if (dir == PLAYER_DIRECTION.RIGHT)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
        }
        else if (dir == PLAYER_DIRECTION.LEFT)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, 0);
        }
        else if (dir == PLAYER_DIRECTION.UP)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveSpeed);
            var sr = this.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
        }
        else if (dir == PLAYER_DIRECTION.DOWN)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -moveSpeed);
            var sr = this.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        /* check collider status */
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.5f, 0.5f), 0.0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                continue;
            }

            if (collider.gameObject.name == "MoveDetectObj")
            {
                continue;
            }

            if (collider.gameObject.tag == "wooden")
            {
                if (collider.gameObject.GetComponent<CObstacle>().m_WoodenType != 0 &&
                    collider.gameObject.GetComponent<CObstacle>().m_Prop == true)
                {
                    gameMgr.SendMountMessage(gameObject, collider.gameObject.GetComponent<CObstacle>().m_WoodenType);
                    Destroy(collider.gameObject);
                    continue;
                }
            }
        }
        if (m_RemotePlayer == false)
        {
            if (gameObject.transform.position != m_PrePosition)
            {
                if (direct == dir)
                {
                    gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                    //      m_MomentSyncTimer = Mathf.MoveTowards(m_MomentSyncTimer, 0.075f, Time.deltaTime);
                    //      if (m_MomentSyncTimer >= m_SyncTime)
                    //      {
                    //          if (direct != PLAYER_DIRECTION.NULL)
                    //              gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                    //          m_MomentSyncTimer = 0;
                    //      }
                }
                else
                {
                    //     m_MomentSyncTimer = Mathf.MoveTowards(m_MomentSyncTimer, 0.075f, Time.deltaTime);
                    //    if (m_MomentSyncTimer >= m_SyncTime)
                    //    {
                    //        if (direct != PLAYER_DIRECTION.NULL)
                    gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                    //        m_MomentSyncTimer = 0;
                    //    }
                }
                m_PrePosition = gameObject.transform.position;
            }
            else
            {
                if (direct != PLAYER_DIRECTION.NULL)
                {
                    switch (direct)
                    {
                        case PLAYER_DIRECTION.LEFT:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.LEFT_STOP);
                            break;
                        case PLAYER_DIRECTION.RIGHT:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.RIGHT_STOP);
                            break;
                        case PLAYER_DIRECTION.UP:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.UP_STOP);
                            break;
                        case PLAYER_DIRECTION.DOWN:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.DOWN_STOP);
                            break;
                    }
                }
            }
        }

        if (m_Mounts != PLAYER_MOUNTS.NULL)
        {
            m_MountObj.transform.position = gameObject.transform.position + new Vector3(m_MountsPositionX, m_MountsPositionY, 0);
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            SpriteRenderer srMount = m_MountObj.GetComponent<SpriteRenderer>();
            srMount.sortingOrder = sr.sortingOrder + 1;
        }
    }
#endif

    void player_move(PLAYER_DIRECTION dir)
    {
        set_player_pic(dir);
        direct = dir;
        CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;
        if (m_RemotePlayer == false)
        {
            if (dir == PLAYER_DIRECTION.RIGHT)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
            }
            else if (dir == PLAYER_DIRECTION.LEFT)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, 0);
            }
            else if (dir == PLAYER_DIRECTION.UP)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveSpeed);
                var sr = this.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
            }
            else if (dir == PLAYER_DIRECTION.DOWN)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -moveSpeed);
                var sr = this.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }

        /* check collider status */
        Collider2D[] colliders = Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(0.5f, 0.5f), 0.0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                continue;
            }

            if (collider.gameObject.name == "MoveDetectObj")
            {
                continue;
            }

            if (collider.gameObject.tag == "wooden")
            {
                if (collider.gameObject.GetComponent<CObstacle>().m_WoodenType != 0 &&
                    collider.gameObject.GetComponent<CObstacle>().m_Prop == true)
                {
                    if (m_RemotePlayer == false)
                        gameMgr.SendMountMessage(gameObject, collider.gameObject.GetComponent<CObstacle>().m_WoodenType);
                    Destroy(collider.gameObject);
                    continue;
                }
            }
        }
        if (m_RemotePlayer == false)
        {
            if (gameObject.transform.position != m_PrePosition)
            {
                if (direct == dir)
                {
                    gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                    //      m_MomentSyncTimer = Mathf.MoveTowards(m_MomentSyncTimer, 0.075f, Time.deltaTime);
                    //      if (m_MomentSyncTimer >= m_SyncTime)
                    //      {
                    //          if (direct != PLAYER_DIRECTION.NULL)
                    //              gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                    //          m_MomentSyncTimer = 0;
                    //      }
                }
                else
                {
                    //     m_MomentSyncTimer = Mathf.MoveTowards(m_MomentSyncTimer, 0.075f, Time.deltaTime);
                    //    if (m_MomentSyncTimer >= m_SyncTime)
                    //    {
                    //        if (direct != PLAYER_DIRECTION.NULL)
                    gameMgr.SendMoveMsgforControlled(gameObject, (int)dir);
                    //        m_MomentSyncTimer = 0;
                    //    }
                }
                m_PrePosition = gameObject.transform.position;
            }
            else
            {
                if (direct != PLAYER_DIRECTION.NULL)
                {
                    switch (direct)
                    {
                        case PLAYER_DIRECTION.LEFT:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.LEFT_STOP);
                            break;
                        case PLAYER_DIRECTION.RIGHT:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.RIGHT_STOP);
                            break;
                        case PLAYER_DIRECTION.UP:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.UP_STOP);
                            break;
                        case PLAYER_DIRECTION.DOWN:
                            gameMgr.SendMoveMsgforControlled(gameObject, (int)PLAYER_DIRECTION.DOWN_STOP);
                            break;
                    }
                }
            }
        }

        if (m_Mounts != PLAYER_MOUNTS.NULL)
        {
            m_MountObj.transform.position = gameObject.transform.position + new Vector3(m_MountsPositionX, m_MountsPositionY, 0);
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            SpriteRenderer srMount = m_MountObj.GetComponent<SpriteRenderer>();
            srMount.sortingOrder = sr.sortingOrder + 1;
        }
    }


    void MovementDetector()
    {
        if (m_PreDirect != direct)
        {
            m_PreDirect = direct;
        }
        else
        {
            if (m_PreDirect == PLAYER_DIRECTION.NULL)
                return;
            m_MomentSyncTimer = Mathf.MoveTowards(m_MomentSyncTimer, m_SyncTime, Time.deltaTime);
            if (m_MomentSyncTimer >= m_SyncTime)
            {
                if (direct != PLAYER_DIRECTION.NULL)
                    CharacterSync(direct);
            }
        }
    }

    void CharacterSync(PLAYER_DIRECTION dir)
    {
        string msg = "";
        CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;
        if (dir == PLAYER_DIRECTION.NULL)
        {
            msg = string.Format("bb_new_stop:{0}/{1}/{2}/{3}", m_Poid, DateTime.Now.Ticks, transform.position.x.ToString("f1"), transform.position.y.ToString("f1"));
        }
        else
        {
            msg = string.Format("bb_new_move:{0}/{1}/{2}/{3}/{3}/{3}/{3}", 
                m_Poid, DateTime.Now.Ticks, transform.position.x.ToString("f1"), transform.position.y.ToString("f1"), moveSpeed, m_SyncTime, m_agcc.ag.GetDelayMilliseconds());
        }
        gameMgr.SendMessage(msg);
    }

    internal void SetPosition(float x, float y, int dir)
    {
        if (m_RemotePlayer == true)
        {
            player_move((PLAYER_DIRECTION)dir);
            gameObject.transform.position = new Vector3(x, y, 0);
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
        }
#if TEST
        Debug.Log("@@@@@@@@@@@@@@ SetPosition");
        if (m_RemotePlayer == true)
        {
            Debug.Log("@@@@@@@@@@@@@@ SetPosition1");
            int ori_x = Mathf.RoundToInt(transform.position.x);
            int ori_y = Mathf.RoundToInt(transform.position.y);
            int remote_x = Mathf.RoundToInt(x);
            int remote_y = Mathf.RoundToInt(y);

            if ((PLAYER_DIRECTION)dir != direct)
            {
                UnHandleMessage msg = new UnHandleMessage();
                msg.x = remote_x;
                msg.y = remote_y;
                msg.dir = dir;
                switch ((PLAYER_DIRECTION)dir)
                {
                    case PLAYER_DIRECTION.UP:
                        if (ori_y < remote_y)
                        {
                            moveSpeed = 2.8f;
                            m_UnhandleMessage.Add(msg);
                        }
                        else
                        {
                            direct = (PLAYER_DIRECTION)dir;
                            transform.position = new Vector3(transform.position.x, remote_y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.DOWN:
                        if (ori_y > remote_y)
                        {
                            moveSpeed = 2.8f;
                            m_UnhandleMessage.Add(msg);
                        }
                        else
                        {
                            direct = (PLAYER_DIRECTION)dir;
                            transform.position = new Vector3(transform.position.x, remote_y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.LEFT:
                        if (ori_x > remote_x)
                        {
                            moveSpeed = 2.8f;
                            m_UnhandleMessage.Add(msg);
                        }
                        else
                        {
                            direct = (PLAYER_DIRECTION)dir;
                            transform.position = new Vector3(remote_x, transform.position.y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.RIGHT:
                        if (ori_x < remote_x)
                        {
                            moveSpeed = 2.8f;
                            m_UnhandleMessage.Add(msg);
                        }
                        else
                        {
                            direct = (PLAYER_DIRECTION)dir;
                            transform.position = new Vector3(remote_x, transform.position.y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.NULL:
                    case PLAYER_DIRECTION.DOWN_STOP:
                    case PLAYER_DIRECTION.UP_STOP:
                    case PLAYER_DIRECTION.LEFT_STOP:
                    case PLAYER_DIRECTION.RIGHT_STOP:
                        m_UnhandleMessage.Add(msg);
                        break;
                    default:
                        return;
                }
            }
            else
            {
                /* speed down if local character is faster than remote side */
                switch ((PLAYER_DIRECTION)dir)
                {
                    case PLAYER_DIRECTION.UP:
                        if (ori_y > remote_y)
                            moveSpeed = 2.2f;
                        break;
                    case PLAYER_DIRECTION.DOWN:
                        if (ori_y < remote_y)
                            moveSpeed = 2.2f;
                        break;
                    case PLAYER_DIRECTION.LEFT:
                        if (ori_x < remote_x)
                            moveSpeed = 2.2f;
                        break;
                    case PLAYER_DIRECTION.RIGHT:
                        if (ori_x > remote_x)
                            moveSpeed = 2.2f;
                            break;
                    default:
                        return;
                }
            }
        }
#endif

        //    sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
    }

    internal void MountDead()
    {
        if (m_Mounts != PLAYER_MOUNTS.NULL)
        {
            Destroy(m_MountObj);
            m_MountObj = new GameObject();
            m_Mounts = PLAYER_MOUNTS.NULL;
            SpriteRenderer spr = m_MountObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        }
    }

    internal bool PlayerDead()
    {
        if (m_Mounts != PLAYER_MOUNTS.NULL)
            return false;
        return true;
    }

    void UpdatePlayerInfo()
    {
        Vector3 roboScreenPos = Camera.main.WorldToViewportPoint(gameObject.transform.TransformPoint(gameObject.transform.position));
        Debug.Log(roboScreenPos);

    }

    void Update()
    {
        if (m_PlayerDeadCnt > 0)
        {
            m_PlayerDeadCnt++;
            if (m_PlayerDeadCnt > 80)
            {
                Sprite spr = Sprite.Create(m_PlayerBom[1], new Rect(0, 0, m_PlayerBom[1].width, m_PlayerBom[1].height), new Vector3(0.5f, 0.5f, 0));
                SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
                sr.sprite = spr;
            }

            if (m_PlayerDeadCnt > 100)
            {
                CGameManager gameMgr = FindObjectOfType(typeof(CGameManager)) as CGameManager;
                m_PlayerDeadCnt = 0;
                gameMgr.SendDeathMessage(gameObject);
                Destroy(gameObject);
            }
            return;
        }

        player_move(direct);
#if TEST
        if (m_RemotePlayer)
        {
            if (m_UnhandleMessage.Count == 0)
            {
                moveSpeed = 2.5f;
            }
            else
            {
                /* only retain the lastest message if the number of message_list is more than 3 */
                if (m_UnhandleMessage.Count > 3)
                {
                    int count = m_UnhandleMessage.Count - 1;
                    for (int i=0; i< count; i++)
                    {
                        UnHandleMessage messages = m_UnhandleMessage[0];
                        m_UnhandleMessage.Remove(messages);
                    }
                }
                int ori_x = Mathf.RoundToInt(transform.position.x);
                int ori_y = Mathf.RoundToInt(transform.position.y);
                UnHandleMessage msg = m_UnhandleMessage[0];
                switch (direct)
                {
                    case PLAYER_DIRECTION.UP:
                        if (msg.y == ori_y)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                        }
                        if (msg.y < ori_y)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                            transform.position = new Vector3(transform.position.x, msg.y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.DOWN:
                        if (msg.y == ori_y)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                        }
                        if (msg.y > ori_y)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                            transform.position = new Vector3(transform.position.x, msg.y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.LEFT:
                        if (msg.x == ori_x)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                        }
                        if (msg.x > ori_x)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                            transform.position = new Vector3(msg.x, transform.position.y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.RIGHT:
                        if (msg.x == ori_x)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                        }
                        if (msg.x < ori_x)
                        {
                            direct = (PLAYER_DIRECTION)msg.dir;
                            m_UnhandleMessage.Remove(msg);
                            transform.position = new Vector3(msg.x, transform.position.y, transform.position.z);
                        }
                        break;
                    case PLAYER_DIRECTION.NULL:
                    case PLAYER_DIRECTION.LEFT_STOP:
                    case PLAYER_DIRECTION.RIGHT_STOP:
                    case PLAYER_DIRECTION.UP_STOP:
                    case PLAYER_DIRECTION.DOWN_STOP:
                        moveSpeed = 0;
                        break;
                    default:
                        m_UnhandleMessage.Remove(msg);
                        return;
                }
            }
        }
#endif
#if KEYBOARD
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
            Debug.Log("@@@space");
            if (wball_cnt < 5)
            {
                create_wball();
                wball_cnt++;
            }
        }
        else
        {
            player_move(PLAYER_DIRECTION.NULL);
        }
#endif
        }
    public void OnClick(float x, float y)
    {
        if (wball_cnt < m_MaxWball)
        {
            create_wball(x, y);
            wball_cnt++;
        }
    }

    public void wball_destroy()
    {
        wball_cnt--;
        if (wball_cnt < 0)
            wball_cnt = 0;
    }

    internal void PlayerDeadProcess()
    {
        Debug.Log("PlayerDeadProcess 1");
        Sprite spr = Sprite.Create(m_PlayerBom[0], new Rect(0, 0, m_PlayerBom[0].width, m_PlayerBom[0].height), new Vector3(0.5f, 0.5f, 0));
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = spr;
        sr.transform.localScale = new Vector3(0.6f, 0.6f, 1.0f);
        m_PlayerDeadCnt = 1;
    }

    bool restricted_area(float x, float y, PLAYER_DIRECTION dir)
    {
		return false;
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
