using UnityEngine;
using System.Collections;

public class CMonster : MonoBehaviour
{
    private const int GAME_FRAME = 30;
    private const int GAME_UPDATE_RATE = 10;
    internal enum MONSTER_NUMBER
    {
        LITTRE_STONE,
    };

    /* resource for monster */
    private Texture2D[] m_MonsterUp = new Texture2D[8];
    private Texture2D[] m_MonsterDown = new Texture2D[8];
    private Texture2D[] m_MonsterLeft = new Texture2D[8];
    private Texture2D m_MonsterWBom;
    private Texture2D m_MonsterBom;

    /* status */
    private int m_Direct = 0;  /* 0: Stop, 1:Up, 2:Down, 3:Left, 4:Right */
    private int m_UpdateCnt = 0;
    private float m_MoveSpeed = 2.5f;


    internal void SetupMonster(MONSTER_NUMBER id)
    {
        /* setup texture by monster id */
        switch (id)
        {
            case MONSTER_NUMBER.LITTRE_STONE:
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        m_MonsterUp[i - 1] = Resources.Load("Textures/Game/Enemy/soldier/Little_Stone_Monster/up/" + i.ToString("00")) as Texture2D;
                        m_MonsterDown[i - 1] = Resources.Load("Textures/Game/Enemy/soldier/Little_Stone_Monster/down/" + i.ToString("00")) as Texture2D;
                        m_MonsterLeft[i - 1] = Resources.Load("Textures/Game/Enemy/soldier/Little_Stone_Monster/left/" + i.ToString("00")) as Texture2D;
                    }
                    m_MonsterBom = Resources.Load("Textures/Game/Enemy/soldier/Little_Stone_Monster/bob") as Texture2D;
                    m_MonsterWBom = Resources.Load("Textures/Game/Enemy/soldier/Little_Stone_Monster/03") as Texture2D;
                    break;
                }
        }
    }


    void Start ()
    {
        SetupMonster(MONSTER_NUMBER.LITTRE_STONE); /* test */ 
        SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
        Sprite s;
        s = Sprite.Create(m_MonsterDown[0], new Rect(0, 0, m_MonsterDown[0].width, m_MonsterDown[0].height), new Vector3(0.5f, 0.5f, 0));
        spr.sprite = s;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (m_UpdateCnt >= GAME_UPDATE_RATE)
        {
            UpdateDirection();
            m_UpdateCnt = 0;
        }
        else
        {
            m_UpdateCnt++;
        }
	}

    void UpdateDirection()
    {
        int new_direct;
        /* try 100 times */
        for (int i =0; i<100; i++)
        {
            new_direct = (int)Random.Range(0, 5.0f);
            if (new_direct != m_Direct)
            {
                m_Direct = new_direct;
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
                return;
            }
        }
    }
}
