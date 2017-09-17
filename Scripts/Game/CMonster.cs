using UnityEngine;
using System.Collections;
using System.Threading;

public class CMonster : MonoBehaviour
{
    private const int GAME_FRAME = 30;
    private const int GAME_UPDATE_RATE = 30;
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
    private int     m_Direct = 0;  /* 0: Stop, 1:Up, 2:Down, 3:Left, 4:Right */
    private int     m_UpdateCnt = 0;
    private float   m_MoveSpeed = 1.5f;
  //  private float   m_MoveSpeed = 0.04f;
    private Vector2 m_Goal;
    private int     m_IdleWait = 0;
    private bool    m_Moving = false;

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
        Vector2 pos = transform.position;

        if (m_Moving)
        {
            if (pos == m_Goal)
            {
                // Reached goal. Back to Idle.
                m_Moving = false;
                m_IdleWait = 0;// Random.Range(1, 30);
            }

            else
            {
                // Walk a bit towards goal
                Vector2 v = Vector2.MoveTowards(pos,
                                                m_Goal,
                                                m_MoveSpeed);
                transform.position = v;
            }
        }
        else
        {
            // Idle
            if (m_IdleWait > 0)
            {
                // Update the animation parameter
                //anim.SetInteger("Direction", 4);

                // Wait a bit so it doesn't look nervous
                --m_IdleWait;
            }
            else
            {
                UpdateNewTarget();
            }
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, new Vector2(1.0f, 1.5f), 0.0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                collider.gameObject.GetComponent<CPlayer>().PlayerKilledByMonster();
            }
        }
        SetSortingOrder();

    }

    void UpdateNewTarget()
    {
        m_Direct = (int)Random.Range(0, 5.0f);
        if (Moving())
            return;
        /* Monster can't move, we need to change direct */
      //  for ()
      //  int new_direct = 
    }

    bool Moving()
    {
        
        switch (m_Direct)
        {
            case 0:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                break;
            case 1:
                GetComponent<Rigidbody2D>().velocity = new Vector2(m_MoveSpeed, 0);
                break;
            case 2:
                GetComponent<Rigidbody2D>().velocity = new Vector2(-m_MoveSpeed, 0);
                break;
            case 3:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, m_MoveSpeed);
                break;
            case 4:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -m_MoveSpeed);
                break;
        }
        m_IdleWait = 30;
        if (m_Direct == 0)
            m_IdleWait = 5;
        return true;
        /*
        Vector2 pos = transform.position;
        switch (m_Direct)
        {
            case 0:
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); return true;
            case 1:
                m_Goal = pos + new Vector2(0, 1); break;
            case 2:
                m_Goal = pos + new Vector2(0, -1); break;
            case 3:
                m_Goal = pos + new Vector2(1, 0); break;
            case 4:
                m_Goal = pos + new Vector2(-1, 0); break;
        }

        int posx = (int)(m_Goal.x + 0.5f);
        int posy = (int)(m_Goal.y + 0.5f);
        m_Goal.x = posx;
        m_Goal.y = posy;
        if (m_Goal.x < 0 || m_Goal.x > 17)
            return false;
        if (m_Goal.y < 0 || m_Goal.y > 10)
            return false;
        string objName = "obstacle" + posx + "/" + posy;
        GameObject obj = GameObject.Find(objName);
        Debug.Log("@@@ objName = " + objName);
        if (obj != null)
            return false;
        m_Goal.x = posx;
        m_Goal.y = posy;
        m_Moving = true;
        return true;*/

    }

    void SetSortingOrder()
    {
        var sr = this.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 10 - Mathf.RoundToInt(gameObject.transform.position.y) + 8;
    }
}
