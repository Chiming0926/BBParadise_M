using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class AGCC {
		
	int rankingState = 0;
	
	internal void RankingBoard()
	{		
		rankingState = 0;		
		ArcaletScore.GetLeaderBoard(ag, lguid, 1, 0, CB_RankingBoard, 0, 20);
		ArcaletScore.GetLeaderBoard(ag, lguid, 2, 0, CB_RankingBoard, 1, 20);
		ArcaletScore.GetLeaderBoard(ag, lguid, 3, 0, CB_RankingBoard, 2, 20);
		ArcaletScore.GetLeaderBoard(ag, lguid, 0, 0, CB_RankingBoard, 3, 20);
	}
	
	void CB_RankingBoard(int code, object data, object token)
	{
		
		if(code == 0)
		{						
			List<Hashtable> list = data as List<Hashtable>;	
						
			int length = list.Count;
			length = Mathf.Clamp(length, 0, 20);
			
		//	int type = (int)token;
			for(int i=0; i < length; i++) {
		//		OXGame.leaderBoard[type, i].account = list[i]["userid"].ToString();
		//		OXGame.leaderBoard[type, i].nickname = list[i]["nickname"].ToString();
		//		OXGame.leaderBoard[type, i].score = list[i]["score"].ToString();			
			}
		}
		else {
			Debug.Log("CB_RankingBoard Failed: " + code + " ,Type: " + (int)token);
		}
		
		rankingState ++;
		if(rankingState == 4) {
		//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
		//	if(menu == null) 
		//		return;
			
		//	menu.ShowRankingBoard();
		}		
	}	
	
	internal void GetMallURL()
	{
		if(ag == null)
			return;

	//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
	//	if(menu == null) 
			return;
	//	menu.store.gameObject.SetActive(false);
	//	menu.uiState = MenuType.Loading;
		
	//	ArcaletItem.GetMallURL(ag, 0, "", CB_GetMallURL, null);			
	}
	
	void CB_GetMallURL(int code, object data, object token)
	{
		if(code == 0) {
			string url = data.ToString();
			Debug.Log("MallURL: " + url);
			Application.OpenURL(url);
		}
		else {
			Debug.Log("GetMallURL Failed: " + code);
		}
			
	//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
	//	if(menu == null) 
	//		return;
	//	
	//	menu.store.gameObject.SetActive(true);
	//	menu.store.refreshButton = true;
	//	menu.uiState = MenuType.None;
	}
	
	internal void RefreshPlayerInfos()
	{
	//	MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
	//	if(menu == null) 
	//		return;
	//	menu.store.gameObject.SetActive(false);
	//	menu.uiState = MenuType.Loading;
		
		ArcaletItem.GetItemInstance(ag, iguid_player, CB_RefreshPlayerInfos, null);
	}
	
	void CB_RefreshPlayerInfos(int code, object data, object token)
	{
		if(code == 0) {
			List<Hashtable> list = data as List<Hashtable>;
			List<Hashtable> attr_ht = list[0]["attr"] as List<Hashtable>;
			
			foreach (Hashtable attr in attr_ht)
			{		
				if(attr["name"].ToString() == "GoodOX") {
					Debug.Log("GoodOX: " + attr["value"].ToString());
		//			if(attr["value"].ToString() != "0")
		//				OXGame.shopInfo.goodOX = true;
				}
				
				if(attr["name"].ToString() == "GoodBoard") {
					Debug.Log("GoodBoard: " + attr["value"].ToString());
			//		if(attr["value"].ToString() != "0")
			//			OXGame.shopInfo.goodBoard = true;
				}
	      	}
		}
		else {
			Debug.Log("RefreshPlayerInfos Failed: " + code);
		}
		
		//MainManager menu = GameObject.FindObjectOfType(typeof(MainManager)) as MainManager;
	//	if(menu == null) 
			return;
		
	//	menu.store.gameObject.SetActive(true);
	//	menu.uiState = MenuType.None;
	}
	
}
