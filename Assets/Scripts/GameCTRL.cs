using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCTRL : MonoBehaviour {

   
	// Update is called once per frame
	void Update () {

	    if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("waterWorld");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
