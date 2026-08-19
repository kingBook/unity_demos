using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePortrait:MonoBehaviour{

	private void Start() {
		//Rect safeArea=Screen.safeArea;
		//Screen.SetResolution((int)safeArea.width,(int)safeArea.height,false);
	}

	//点右上角返回按钮
	public void onClickBack(){
		SceneManager.LoadSceneAsync("main");
	}

}
