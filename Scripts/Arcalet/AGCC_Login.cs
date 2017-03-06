using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public partial class AGCC {

	string m_username;
	string m_password;
	string m_email;
	
	//get a system account
	internal void GetSystemAccount()
	{
		Debug.Log("GetSystemAccount");
		ArcaletSystem.ApplyNewUser(gguid, certificate, CB_GetSysAccount, null);
	}
	
	//callback function
	void CB_GetSysAccount(int code, object data, object token)
	{
		ag = null;
	/*	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
		if(menu == null) 
			return;
		
		if(code == 0) {
			Debug.Log("GetSystemAccount Successed");
			Hashtable ht = data as Hashtable;
			string system_acc = ht["userid"].ToString();
			string system_pw = ht["passwd"].ToString();
			menu.SystemAccInfo(system_acc, system_pw);
		
		else 
		{
			Debug.Log("GetSystemAccount Failed");
			menu.LoginError("CB_GetSysAccount Failed: " + code);
		}*/
	}
	
	//login arcalet server
	internal void ArcaletLaunch( string username, string password, string email)
	{
		Debug.Log("ArcaletLaunch");
		if(ag!=null)
			ag.Dispose();
		Debug.Log("ArcaletLaunch1");
		m_username  = username;
		m_password  = password;
		m_email	    = email;
		ag = new ArcaletGame(username, password, gguid, sguid, certificate );
		Debug.Log("ArcaletLaunch 2");
	//	ag = new ArcaletGame("bbhappy010", "12345678", gguid, sguid, certificate );
		ag.onMessageIn += MainMessageIn;
		ag.onPrivateMessageIn += PrivateMessageIn;
		ag.onCompletion += CB_ArcaletLaunch;		
		ag.Launch();
	}
	
	//callback function
	void CB_ArcaletLaunch(int code, ArcaletGame game)
	{	
		Debug.Log("CB_ArcaletLaunch");
		if(code==0) {
			Debug.Log("ArcaletLaunch Successed");
            SceneManager.LoadScene("lobby");
            LoginCheck();
        }
		else {
			Debug.Log("ArcaletLaunch Failed code = " + code);
            /* Login Failed, regist this user automatically */
            Regist(m_username, m_password, m_email);
        }
	}

	void Regist(string username, string password, string mail)
    {
        string[] registToken = new string[] { username, password, mail };
        ArcaletSystem.ApplyNewUser(gguid, certificate, username, password,
         mail, CB_Regist, registToken);
    }	    

	void CB_Regist(int code, object token)
    {

        if (code == 0)
        {
            /* regist sucessful */
            string[] reg = token as string[]; 
            string acc = reg[0];
            string pw = reg[1];
            string mail = reg[2];
            Debug.Log("Regist Successed - Account:" + acc + " / Password:" +
             pw + " E-Mail" + mail);
			ArcaletLaunch(acc, pw, mail);
        }
        else
        {
            Debug.LogWarning("Regist Failed - Error:" + code);
        }
    }

	//get item class - server settings
	void GetServerSettings()
	{
		ArcaletItem.GetItemClass(ag, iguid_server, CB_GetServerSettings, null);
	}
	
	//callback function
	void CB_GetServerSettings(int code, object data, object token)
	{
		if(code == 0) {
			Debug.Log("CB_GetServerSettings Successed");
			List<Hashtable>	list = data as List<Hashtable>;
			List<Hashtable> attr_ht = list[0]["attr"] as List<Hashtable>;			
			foreach (Hashtable attr in attr_ht) {				
				switch(attr["name"].ToString()) {
					case "PassageGate":
						if(attr["value"].ToString() == "1") 
							serverSettings.passageGate = true;
						break;

					case "Announcement":
						serverSettings.announcement = attr["value"].ToString();
						break;			
				}
          	}
			LoginCheck();
		}
		else {
			Debug.Log("CB_GetServerSettings Failed");
		//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
		//	if(menu != null) 
		//		menu.LoginError("CB_GetServerSettings Failed: " + code);
		}
	}
	
	//link into dp
	void LoginCheck()
	{	
		Debug.Log("LoginCheck");
	//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
	//	if(menu == null) 
	//		return;
		
	//	if(!serverSettings.passageGate) {
	//		menu.LoginError("Server is closed.");
	//		return;
	//	}
	//	StartCoroutine("DPLinkTimer");
		ag.SendOnClose("quit:" + ag.gameUserid + "/" + ag.poid);
		ag.Send("new:" + ag.gameUserid + "/" + ag.poid);
	}
	
	IEnumerator DPLinkTimer()
	{
		yield return new  WaitForSeconds(10);
		Debug.Log("DPLink TimeOut");
	//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
	//	if(menu != null) 
	//		menu.LoginError("DPLink TimeOut");
	}
	
	//get item instance - player informations
	void GetPlayerInfos(string msg)
	{
		Debug.Log("GetPlayerInfos");
		serverSettings.dpPoid = int.Parse(msg);
		ArcaletItem.GetItemInstance(ag, iguid_player, CB_GetPlayerInfos, null);
	}
	
	//callback function
	void CB_GetPlayerInfos(int code, object data, object token)
	{	
	//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
	//	if(menu == null) 
	//		return;
	/*	
		if(code == 0) {
			Debug.Log("GetPlayerInfos Successed");
			List<Hashtable> list = data as List<Hashtable>;
			List<Hashtable> attr_ht = list[0]["attr"] as List<Hashtable>;
			
			foreach (Hashtable attr in attr_ht) {
	            if(attr["name"].ToString() == "Win")
	            	OXGame.playerInfo.win = int.Parse(attr["value"].ToString());

	           	if(attr["name"].ToString() == "Lose")
	           		OXGame.playerInfo.lose = int.Parse(attr["value"].ToString());
				
				if(attr["name"].ToString() == "Draw")
	           		OXGame.playerInfo.draw = int.Parse(attr["value"].ToString());
				
				if(attr["name"].ToString() == "GoodOX") {					
					if(attr["value"].ToString() != "0")
						OXGame.shopInfo.goodOX = true;
				}
				
				if(attr["name"].ToString() == "GoodBoard") {					
					if(attr["value"].ToString() != "0")
						OXGame.shopInfo.goodBoard = true;
				}
	      	}
			OXGame.playerInfo.SetWinRate();
			StopCoroutine("DPLinkTimer");
			menu.LoginSuccessed();
		}
		else {
			Debug.Log("GetPlayerInfos Failed: " + code);
			menu.LoginError("CB_GetPlayerInfos Failed: " + code);
		}*/
	}
	
	//set nickname
	internal void SetNickName(string nick)
	{
		ag.SetPlayerNickname(nick, CB_SetNickName, null);
	}
	
	//callback function
	void CB_SetNickName(int code, object token)
	{
		if(code == 0)
			Debug.Log("SetNickName Successed");
		else 
			Debug.Log("SetNickName Failed: " + code);
	/*	OXGame.playerInfo.nickname = ag.nickname;
		MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
		if(menu!=null)
			menu.uiState = MenuType.Main;
		*/
	}
	
}
