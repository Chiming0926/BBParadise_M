using UnityEngine;
using System;
using System.Collections;

public partial class AGCC : MonoBehaviour {

    /*******************************************************************
	*	Copyright  2013 arcalet. All Rights Reserved
	*	 
	*******************************************************************/
    public static int BBDEBUG_INFO = 1;
    public static int BBDEBUG_WARNING = 2;
    public static int BBDEBUG_ERROR = 4;
    private static int CHANGE_NAME_COST = 80;
    private static int bbDebug = 7; /* debug level info, warning, error */
    internal bool m_TestMode = false;

	#region Variables	
	string gguid = "7245577f-4961-7642-a64c-ba5bb008892c";
	string sguid = "52a06444-ff13-654b-bfa1-29da9f7124dd";

	string iguid_server = "";
	string iguid_player = "2a0ccd1d-f564-f74f-b1c6-d2a6157b8b59";

    string sguid_game = "c4345a29-310f-d241-b95c-77928bf819c6";
    string lguid = "";

	byte[] certificate = {0x43, 0x13, 0xbd, 0x25, 0x4e, 0x7b, 0xa3, 0x4b, 0x86, 0x13, 0xa8, 0xa5, 0x49,
                            0xcf, 0xd1, 0x5e, 0x58, 0x34, 0x2c, 0xda, 0xb, 0x9f, 0xc, 0x41, 0x8a, 0x3e,
                            0xd4, 0x71, 0x5a, 0xb, 0x3d, 0x41, 0x57, 0x1c, 0x53, 0xed, 0xf, 0x52, 0x70,
                            0x47, 0x93, 0xb5, 0x9f, 0x27, 0xe3, 0xa0, 0x66, 0x2d, 0x86, 0x55, 0x5b, 0x9c,
                            0x4b, 0x33, 0xd8, 0x40, 0xbb, 0x66, 0xf1, 0x8c, 0x67, 0x19, 0x8a, 0x4, 0x1a,
                            0x14, 0x28, 0xa7, 0x67, 0x95, 0x72, 0x4c, 0x8c, 0xfb, 0xa, 0xf5, 0x4a, 0x12,
                            0x49, 0x53, 0x6d, 0xf7, 0xa0, 0xd4, 0x73, 0x96, 0x52, 0x43, 0x8e, 0x54, 0x79,
                            0xfa, 0x48, 0xeb, 0x5b, 0xaf, 0xd1, 0x64, 0x20, 0x3d, 0x49, 0xa5, 0xbc, 0x40,
                            0x89, 0xa2, 0xb6, 0xc5, 0x6f, 0xd6, 0xac, 0xfe, 0x2f, 0x92, 0x4d, 0xbc, 0x3f,
                            0xbd, 0x4b, 0x4d, 0x90, 0xf8, 0x50, 0xf2, 0x2, 0x16, 0x75, 0xd4};

    internal ArcaletGame ag = null;
	public ServerSettings serverSettings = new ServerSettings();
	
	#endregion

	internal PlayerInfo m_PlayerInfo = new PlayerInfo();
	
	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);
		ArcaletSystem.UnityEnvironment();
    }
	
    public static void BBDebug(int debugLevel, string msg)
    {
        int numOfDebugLevel = 3;
        for (int i=0; i< numOfDebugLevel; i++)
        {
            if ((debugLevel & (1 << i)) == 1)
            {
                Debug.Log(msg);
                break;
            }
        }
    }

	void MainMessageIn(string msg, int delay, ArcaletGame game)
	{
		try 
		{
            Debug.Log("MainMessageIn, msg = " + msg);
        }
        catch (Exception e) { Debug.LogWarning("MainMessageIn Exception:\r\n" + e.ToString()); }
	}
	
	void PrivateMessageIn(string msg, int delay, ArcaletGame game)
	{
		try {
			Debug.Log("@ PrivaMsg>> " + msg);
			string[] cmds = msg.Split(':');			
            switch (cmds[0])
            {
				case "dp_new": GetPlayerInfos(cmds[1]); break;
				case "dp_room": DP_Room(cmds[1]); break;
				case "dp_rematch": ReMatch(cmds[1]); break;
				case "bb_match_data": UpdateMatchData(cmds[1]); break;
            }
        }
        catch (Exception e) { Debug.LogWarning("PrivateMessageIn Exception:\r\n" + e.ToString()); }
	}
	
	void GameMessageIn(string msg, int delay, ArcaletScene scene)
	{
		try {
			Debug.Log("@ GameMsg>> " + msg);
			string[] cmds = msg.Split(':');
			CGameManager game = FindObjectOfType(typeof(CGameManager)) as CGameManager;
			if (game == null) 
				return ;
			
            switch (cmds[0])
            {
				case "bb_move":
					game.player_move(cmds[1]); 
					break;
				case "bb_stop": 
					game.player_stop(cmds[1]); 
					break;
                case "bb_new_move":
                    game.player_move(cmds[1]);
                    break;
                case "bb_new_stop":
                    game.player_stop(cmds[1]);
                    break;
                case "bb_player":
					game.add_player(cmds[1]);
					break;
				case "bb_wball":
					game.bb_wball(cmds[1]);
					break;
                case "bb_going_to_death":
                    game.handle_going_to_death_message(cmds[1]);
                    break;
				case "bb_move_Controlled_player":
                    game.controlled_player_move(cmds[1]);
					break;
				case "bb_over":
					game.handle_game_over(cmds[1]);
					break;
                case "bb_props":
                    game.HandlePropsMessage(cmds[1]);
                    break;
                case "bb_mount_dead":
                    game.HandleMountDead(cmds[1]);
                    break;
                case "client_bb_wball":
                    game.HandleClientWBall(cmds[1]);
                    break;
                default:
					break;
            }
        }
        catch (Exception e) { Debug.LogWarning("GameMessageIn Exception:\r\n" + e.ToString()); }
	}
	
	void OnApplicationQuit()
	{
		if(ag==null) return;
		ag.Dispose();
	}

	internal void setFBUserId(string id)
	{
		m_PlayerInfo.fbUserId = id;
	}

    internal void setFBUserName(string name)
    {
        m_PlayerInfo.fbUserName = name;
    }

    internal bool ChangeNickName(string name)
    {
        if (m_PlayerInfo.money < CHANGE_NAME_COST)
            return false;
        m_PlayerInfo.money -= CHANGE_NAME_COST;
        m_PlayerInfo.SetPlayerInfo(m_PlayerInfo.money.ToString(), PlayerInfo.SET_PLAYERINFO.SET_MONEY);
		m_PlayerInfo.SetPlayerInfo(name, PlayerInfo.SET_PLAYERINFO.SET_NICKNAME);
        return true;
    }
}

[System.Serializable]
public class ServerSettings 
{
	public bool passageGate = false;
	public string announcement = "";
	public int dpPoid = 0;
}

[System.Serializable]
public class PlayerInfo
{

    internal enum SET_PLAYERINFO
    {
        SET_CHARACTER,
        SET_NICKNAME,
        SET_CAKE,
        SET_MONEY,
        SET_GEM,
        SET_BACKGROUND_AUDIO,
        SET_SPECIAL_AUDIO,
    };

	/* communicate with arcalet */
	internal	ArcaletGame ag = null;
	AGCC		agcc = null;
	string 		iguid_player = "2a0ccd1d-f564-f74f-b1c6-d2a6157b8b59";
	int 		itemId = 0;
	
    public string nickname = "NickName";
    public string account = "Account";
	
    public int win = 0;
    public int lose = 0;
    public int draw = 0;
    public string winRate = "0%";
	public string fbUserId = "";
    public string fbUserName = "";
    public int gem = 0;
    public int money = 0;
    public int cake = 0;
    public int backgroundAudio = 0;
    public int specialAudio = 0;

	public int character_num = 0;

	internal void SetArcalet(ArcaletGame arg, AGCC agc)
	{
		ag = arg;
		agcc = agc;
	}

	internal void SetItemId(int id)
	{
		itemId = id;
		Debug.Log("itemId = " + itemId);
	}
	
    internal void SetWinRate()
    {
        if (win == 0) winRate = "0%";
        else
        {
            float rate_f = (float)win / (win + lose);
            int rate_100 = Mathf.CeilToInt(rate_f * 100);
            winRate = rate_100 + "%";
        }
    }

    internal void SetPlayerInfo(string value, PlayerInfo.SET_PLAYERINFO cmd)
    {
        string[] Params = { "p_character_num", "p_nickname", "p_cake", "p_money", "p_gem",
                            "p_background_audio", "p_special_audio"};
        ArcaletItem.SetItemInstanceAttribute(ag, iguid_player, itemId, Params[(int)cmd], value,
                    CB_SetItemAttribute, Params[(int)cmd]);
    }

	void CB_SetItemAttribute(int code, object token) 
	{
		if(code == 0) 
		{
			string attr = token.ToString();
			Debug.Log("SetItemAttribute : " + attr + " Successed");
			agcc.GetPlayerInfos("test");
		}
		else 
		{
			Debug.LogWarning("SetItemAttribute Failed - Error:" + code);
		}
	}
}


