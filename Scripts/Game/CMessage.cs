using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public partial class CGameManager
{
    public GameObject[] m_PlayerPrefabs = new GameObject[6];
    public GameObject   m_Joystick;
    public GameObject   m_Button;

    enum PLAYER_DIRECTION
    {
        UP      = 0,
        DOWN    = 1,
        RIGHT   = 2,
        LEFT    = 3,
        NULL    = 4
    };

    internal class PlayerData
    {
        internal GameObject player_ins;
        internal string     player_account;
        internal string     nickname;
        internal int        player_poid;
        internal bool       me;
    }

    List<PlayerData> player_list = new List<PlayerData>();

    private static bool NONETWORK = true;

    PLAYER_DIRECTION cur_direct;
    AGCC m_agcc = null;
	bool m_Controller;
	
    internal void InitMessageModule()
    {
        m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
        cur_direct = PLAYER_DIRECTION.NULL;
        if (NONETWORK)
        {
            PlayerData playerData = new PlayerData();
            playerData.player_account = "test520";
            playerData.player_ins = Instantiate(m_PlayerPrefabs[1], new Vector3(7.5f, 4.5f, -1), gameObject.transform.rotation) as GameObject;
            player_list.Add(playerData);
            playerData.player_ins.GetComponent<CPlayer>().SetControllerStatus(true);
        }
    }

    public void BeginMove()
    {
    }

    public void EndMove()
    {
        if (cur_direct == PLAYER_DIRECTION.NULL)
            return;
       
        cur_direct = PLAYER_DIRECTION.NULL;
        if (NONETWORK)
        {
            string msg = "bb_stop:" + "test520";
            player_stop(msg);
        }
        else
        {
            string msg = "bb_stop:" + m_agcc.ag.gameUserid;
            m_agcc.sn.Send(msg);
        }
    }

    public void UpdateDirection(Vector3 direction)
    {
        PLAYER_DIRECTION direct;
        if (Math.Abs(direction.x) == Math.Abs(direction.y))
        {
            /* don't change */
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
        if (direct != cur_direct)
        {
            cur_direct = direct;
            string msg;

            if (NONETWORK)
            {
                msg = "bb_move:" + "test520" + "/" + (int)direct;
                player_move(msg);
            }
            else
            {
                msg = "bb_move:" + m_agcc.ag.gameUserid + "/" + (int)direct;
                m_agcc.sn.Send(msg);
            }
        }
    }

	internal void SendMoveMsgforControlled(GameObject obj, int direct)
	{
        if (NONETWORK == false)
        {
            foreach (PlayerData data in player_list)
            {
                if (data.player_ins.name == obj.name)
                {
                    string msg = "bb_move_Controlled_player:" + data.player_account + "/" + direct + "/" + obj.transform.position.x + "/" + obj.transform.position.y;
                    m_agcc.sn.Send(msg);
                }
            }
        }
	}

    public void OnBtnClick()
    {
        BBDebug("OnBtnClick");
        string msg;
        
        if (NONETWORK)
        {
            msg = "bb_wball:" + "test520";
            bb_wball(msg);
        }
        else
        {
            msg = "bb_wball:" + m_agcc.ag.gameUserid;
            m_agcc.sn.Send(msg);
        }
    }

    internal void player_move(string msg)
    {
        string[] m = msg.Split('/');
        if (NONETWORK)
        {
            player_list[0].player_ins.GetComponent<CPlayer>().UpdateDirection(int.Parse(m[1]));
        }
        else
        {
            foreach (PlayerData data in player_list)
            {
				Debug.Log("@@@@@@@@@@@@@ player_move 1");
                if (data.player_account == m[0])
                {
					Debug.Log("@@@@@@@@@@@@@ player_move 2");
					data.player_ins.GetComponent<CPlayer>().UpdateDirection(int.Parse(m[1]));
                }
            }
        }
    }

	internal void controlled_player_move(string msg)
	{
		string[] m = msg.Split('/');
        if (NONETWORK)
        {
            player_list[0].player_ins.GetComponent<CPlayer>().UpdateDirection(int.Parse(m[1]));
        }
        else
        {
            foreach (PlayerData data in player_list)
            {
                if (data.player_account == m[0])
                	data.player_ins.GetComponent<CPlayer>().SetPosition(float.Parse(m[2]), float.Parse(m[3]));
            }
        }
	}

    internal void player_stop(string msg)
    {
        BBDebug("player_stop");
        string[] m = msg.Split('/');
        if (NONETWORK)
        {
            player_list[0].player_ins.GetComponent<CPlayer>().EndMove();
        }
        else
        {
            foreach (PlayerData data in player_list)
            {
                if (data.player_account == m[0])
                    data.player_ins.GetComponent<CPlayer>().EndMove();
            }
        }
    }

    internal void add_player(string msg)
    {
        if (player_list.Count >= 6)
        {
            Debug.Log("Gameroom is full");
            return;
        }
        Vector3[] positionArray = new[] { new Vector3(7.5f, 4.5f, -1), new Vector3(-7.5f, 4.5f, -1), new Vector3(0.0f, 4.5f, -1),
                                            new Vector3(7.5f, -4.5f, -1), new Vector3(-7.5f, -4.5f, -1), new Vector3(0.0f, -4.5f, -1)};
        string[] m = msg.Split('/');
        PlayerData playerData = new PlayerData();

        playerData.player_poid = int.Parse(m[0]);

        if (m[1] == m_agcc.ag.gameUserid)
            playerData.me = true;
        else
            playerData.me = false;
		Debug.Log("m_agcc.ag.gameUserid = " + m_agcc.ag.gameUserid + " m[3] = " + m[3]);
		if (m[3] == m_agcc.ag.gameUserid)
            m_Controller = true;
        else
            m_Controller = false;
        Debug.Log("m_Controller = " + m_Controller);
        playerData.player_account = m[1];
        playerData.nickname = m[2];
        playerData.player_ins = Instantiate(m_PlayerPrefabs[player_list.Count], positionArray[player_list.Count], gameObject.transform.rotation) as GameObject;
        playerData.player_ins.name = "Player" + player_list.Count.ToString("00");
        playerData.player_ins.GetComponent<CPlayer>().SetControllerStatus(m_Controller);
		player_list.Add(playerData);
    }

    internal void bb_wball(string msg)
    {
        BBDebug("bb_wball");
        if (NONETWORK)
        {
            player_list[0].player_ins.GetComponent<CPlayer>().OnClick();
        }
        else
        {
            string[] m = msg.Split('/');
            foreach (PlayerData data in player_list)
            {
                if (data.player_account == m[0])
                    data.player_ins.GetComponent<CPlayer>().OnClick();
            }
        }
    }

    internal void check_death_people(string name)
    {
        foreach (PlayerData data in player_list)
        {
            if (data.player_ins.name == name)
            {
                BBDebug("bb_death");
                string msg = "bb_death:" + m_agcc.ag.gameUserid;
                m_agcc.sn.Send(msg);
            }
        }
    }

    internal void handle_death_message(string msg)
    {
        BBDebug("handle_death_message");
        string[] m = msg.Split('/');
        foreach (PlayerData data in player_list)
        {
            if (data.player_account == m[0])
            {
                Destroy(data.player_ins);
            }
        }
    }

    internal void handle_game_over(string msg)
    {
        m_agcc.SceneGameOver();
        SceneManager.LoadScene("lobby");
    }
}
