using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CLogo : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("Login");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
