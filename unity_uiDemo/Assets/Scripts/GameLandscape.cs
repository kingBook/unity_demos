using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLandscape:MonoBehaviour{
	
    //点右上角返回按钮
	public void onClickBack(){
		SceneManager.LoadSceneAsync("main");
	}
}
