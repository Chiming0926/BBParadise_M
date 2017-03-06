using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;
using Facebook.Unity;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

public class CLogin : MonoBehaviour {

	/* messages for fb login */
	string Status;
	string LastResponse;

	/* arcalet object */
	AGCC m_agcc = null;


	// Use this for initialization
	void Start () 
	{
		FB.Init(this.OnInitComplete, this.OnHideUnity);
		m_agcc = FindObjectOfType(typeof(AGCC)) as AGCC;
		if(m_agcc == null) return;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void OnInitComplete()
    {
        this.Status = "Success - Check log for details";
        this.LastResponse = "Success Response: OnInitComplete Called\n";
        string logMessage = string.Format(
            "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
            FB.IsLoggedIn,
            FB.IsInitialized);
        Debug.Log("OnInitComplete");
    }

    private void OnHideUnity(bool isGameShown)
    {
        this.Status = "Success - Check log for details";
        this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
		Debug.Log("Is game shown: " + isGameShown);
    }

	void OnMouseDown()
    {
        if (gameObject.tag == "fb_login")
        {
     		Debug.Log("start fb login process");
			FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.FbLoginCallback);
        }
    }

	void FbLoginCallback(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("login result = " + result.RawResult);
            Debug.Log("FB login is successful, token = " +  AccessToken.CurrentAccessToken.TokenString);
            FB.API("/me?fields=id,name,email", HttpMethod.GET, user_callback);
        }
        else
        {
            Debug.Log("FB login is not successful");
        } 
    }

	string user_account;
    string user_password;
    string user_mail;

    void user_callback(IResult result)
    {
        string md5 = getMd5Method(result.ResultDictionary["email"].ToString());
        if (md5 != null)
        {
            user_mail = result.ResultDictionary["email"].ToString();
            user_password = md5.Substring(16, 16);
            user_account = md5.Substring(16, 10);
            Debug.Log("user_account = " + user_account + ", user_password = " + user_password);
            m_agcc.ArcaletLaunch(user_account, user_password, user_mail);
			m_agcc.setFBUserId(result.ResultDictionary["id"].ToString());
        }
    }

    private string getMd5Method(string input)
    {
        try
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            byte[] myData = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < myData.Length; i++)
            {
                sBuilder.Append(myData[i].ToString("x"));
            }

            return string.Format("ComputeHash(16)：{0}", sBuilder.ToString());
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Can't get user's email");
        }
        return null;
    }
}
