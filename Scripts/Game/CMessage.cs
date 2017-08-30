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
    private GameObject m_MyPlayer;
	
    internal void InitMessageModule()
    {
        m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
        cur_direct = PLAYER_DIRECTION.NULL;
        if (NONETWORK)
        {
            PlayerData playerData = new PlayerData();
            playerData.player_account = "test520";
            playerData.player_ins = Instantiate(m_PlayerPrefabs[1], new Vector3(17, 10.26f, -1), gameObject.transform.rotation) as GameObject;
            player_list.Add(playerData);
            playerData.player_ins.name = "Player02";
            playerData.player_ins.GetComponent<CPlayer>().SetControllerStatus(true);
            playerData.player_ins.GetComponent<CPlayer>().SetPlayerInfo(playerData.player_account, 555, false);

            SpriteRenderer sp = playerData.player_ins.GetComponent<SpriteRenderer>();
            m_Controller = true;
            m_MyPlayer = playerData.player_ins;

            CreateMap(2);
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
            m_MyPlayer.GetComponent<CPlayer>().EndMove();
            string msg = "bb_stop:" + m_agcc.ag.gameUserid + "/" + m_MyPlayer.transform.position.x + "/" + m_MyPlayer.transform.position.y;
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
                player_move_local(msg);
            }
            else
            {
                m_MyPlayer.GetComponent<CPlayer>().UpdateDirection((int)direct);
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
                    string msg = "bb_move_Controlled_player:" + data.player_account + "/" + direct + "/" + obj.transform.position.x + "/" + obj.transform.position.y + "/" + DateTime.Now.Ticks;
                    BBSendMessage(msg);
                }
            }
        }
	}

    internal void SendMountMessage(GameObject obj, int props)
    {
        foreach (PlayerData data in player_list)
        {
            if (data.player_ins.name == obj.name)
            {
                string msg = "bb_props:" + data.player_account + "/" + props;
                if (NONETWORK == false)
                    BBSendMessage(msg);
                else
                {
                    string[] cmds = msg.Split(':');
                    HandlePropsMessage(cmds[1]);
                }
            }
        }
    }

    internal void HandlePropsMessage(string msg)
    {
        string[] m = msg.Split('/');
        Debug.Log("m[0] =" + m[0] + " m[1] = " + m[1]);
        foreach(PlayerData data in player_list)
        {
            if (data.player_account == m[0])
            {
                data.player_ins.GetComponent<CPlayer>().AddProps(int.Parse(m[1]));
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
            if (m_Controller)
            {
                msg = "bb_wball:" + m_agcc.ag.gameUserid + "/" + m_MyPlayer.transform.position.x + "/" + m_MyPlayer.transform.position.y;
                BBSendMessage(msg);
            }
            else
            {
                msg = "client_bb_wball:" + m_agcc.ag.gameUserid;
                BBSendMessage(msg);
            }
        }
    }

    internal void HandleClientWBall(string msg)
    {
        if (m_Controller)
        {
            foreach (PlayerData data in player_list)
            {
                if (data.player_account == msg)
                {
                    msg = "bb_wball:" + data.player_account + "/" + data.player_ins.transform.position.x + "/" + data.player_ins.transform.position.y;
                    BBSendMessage(msg);
                    return;
                }
            }
        }
    }

    internal void HandleCreateMap(string msg)
    {
        CreateMap(0);
    }

    internal void player_move_local(string msg)
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
                {
                    data.player_ins.GetComponent<CPlayer>().UpdateDirection(int.Parse(m[1]));
                }
            }
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
                if (data.player_account == m[0])
                {
                    if (data.me == false)
				    	data.player_ins.GetComponent<CPlayer>().UpdateDirection(int.Parse(m[1]));
                }
            }
        }
    }

    internal void player_new_move(string msg)
    {
        string[] m = msg.Split('/');
        int poid = int.Parse(m[0]);
        if (poid != m_agcc.ag.poid)
        {

        }
    }

    internal void player_new_stop(string msg)
    {
        string[] m = msg.Split('/');
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
                {
                    data.player_ins.GetComponent<CPlayer>().SetPosition(float.Parse(m[2]), float.Parse(m[3]), int.Parse(m[1]));
                }
            }
        }
	}

    internal void Sync_Player_Position()
    {
        if (NONETWORK)
        {

        }
        else
        {
        //    string msg = "bb";
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
                {
                    if (data.me == false)
                        data.player_ins.GetComponent<CPlayer>().SetPosition(float.Parse(m[1]), float.Parse(m[2]), 4);
                }
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
        Vector3[] positionArray = new[] { new Vector3(17, 10.3f, -1), new Vector3(0, 10.3f, -1), new Vector3(0.0f, 4.8f, -1),
                                            new Vector3(8.5f, -4.5f, -1), new Vector3(-8.5f, -4.5f, -1), new Vector3(0.0f, -4.5f, -1)};
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
        if (playerData.me)
        {
            m_MyPlayer = playerData.player_ins;
            playerData.player_ins.GetComponent<CPlayer>().SetPlayerInfo(playerData.player_account, m_agcc.ag.poid, false);
        }
        else
        {
            playerData.player_ins.GetComponent<CPlayer>().SetPlayerInfo(playerData.player_account, m_agcc.ag.poid, true);
            Destroy(playerData.player_ins.GetComponent<BoxCollider2D>());
        }    
        Debug.Log("playerData.player_account");
        
        player_list.Add(playerData);
    }

    internal void bb_wball(string msg)
    {
        BBDebug("bb_wball");
        if (NONETWORK)
        {
            player_list[0].player_ins.GetComponent<CPlayer>().OnClick(m_MyPlayer.transform.position.x, m_MyPlayer.transform.position.y);
        }
        else
        {
            string[] m = msg.Split('/');
            foreach (PlayerData data in player_list)
            {
                if (data.player_account == m[0])
                    data.player_ins.GetComponent<CPlayer>().OnClick(float.Parse(m[1]), float.Parse(m[2]));
            }
        }
    }

    internal void check_death_people(string name)
    {
        foreach (PlayerData data in player_list)
        {
            if (data.player_ins.name == name)
            {
                if (data.me)
                {
                    if (data.player_ins.GetComponent<CPlayer>().PlayerDead() == true)
                    {
                        string msg = "bb_going_to_death:" + data.player_account;
                        if (NONETWORK)
                        {
                            string[] cmds = msg.Split(':');
                            handle_going_to_death_message(cmds[1]);
                        }
                        else
                        {
                            BBSendMessage(msg);
                        }
                    }
                    else
                    {
                        string msg = "bb_mount_dead:" + data.player_account;
                        if (NONETWORK)
                        {
                            string[] cmds = msg.Split(':');
                            handle_going_to_death_message(cmds[1]);
                        }
                        else
                        {
                            BBSendMessage(msg);
                        }
                    }
                }
            }
        }
    }

    internal void BBSendMessage(string msg)
    {
        Debug.Log("Send Message : " + msg);
        m_agcc.sn.Send(msg);
    }

    internal void HandleMountDead(string msg)
    {
        string[] m = msg.Split('/');
        foreach (PlayerData data in player_list)
        {
            if (data.player_account == m[0])
            {
                data.player_ins.GetComponent<CPlayer>().MountDead();
            }
        }
    }

    internal void handle_going_to_death_message(string msg)
    {
        BBDebug("handle_death_message");
        string[] m = msg.Split('/');
        foreach (PlayerData data in player_list)
        {
            if (data.player_account == m[0])
            {
                data.player_ins.GetComponent<CPlayer>().PlayerDeadProcess();
            }
        }
    }

    internal void SendDeathMessage(GameObject obj)
    {
        if (NONETWORK == false)
        {
            foreach (PlayerData data in player_list)
            {
                if (data.player_ins.name == obj.name)
                {
                    string msg = "bb_death:" + data.player_account;
                    BBSendMessage(msg);
                }
            }
        }
    }

    internal void handle_game_over(string msg)
    {
        m_agcc.SceneGameOver();
        m_agcc.SetAudio(true);
        SceneManager.LoadScene("lobby");
    }
}
