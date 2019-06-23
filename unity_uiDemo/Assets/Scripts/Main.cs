using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour{
    
	public Canvas canvasPortrait;
	public Canvas canvasLandscape;


    private void Start(){
		//initPortraitCanvas(canvasPortrait);
		//initLandscapeCanvas(canvasLandscape);
		//默认进入竖屏的Cavans
		enterPortraitCanvas();
    }

	//进入竖屏Canvas
	public void enterPortraitCanvas(){
		canvasPortrait.gameObject.SetActive(true);
		canvasLandscape.gameObject.SetActive(false);

		Screen.orientation=ScreenOrientation.Portrait;
	}

	//进入横屏Canvas
	public void enterLandscapeCanvas(){
		canvasPortrait.gameObject.SetActive(false);
		canvasLandscape.gameObject.SetActive(true);

		Screen.orientation=ScreenOrientation.Landscape;
	}

	//进入竖屏游戏场景
	public void enterPortaitGameScene(){
		//吊销竖屏Canvas
		canvasPortrait.gameObject.SetActive(false);
		//加载前必须在Build Settings窗口中添加要加载的场景(把场景拖入窗口)
		SceneManager.LoadSceneAsync("gamePortrait");
	}

	//进入横屏游戏场景
	public void enterLandscapeGameScene(){
		//吊销竖屏Canvas
		canvasLandscape.gameObject.SetActive(false);
		//加载前必须在Build Settings窗口中添加要加载的场景(把场景拖入窗口)
		SceneManager.LoadSceneAsync("gameLandscape");
	}

	//初始化设置竖屏Canvas
	/*private void initPortraitCanvas(Canvas canvas){
		//ScreenSpaceOverlay:显示在所有对象的顶层
		canvas.renderMode=RenderMode.ScreenSpaceOverlay;

		var scaler=canvas.GetComponent<CanvasScaler>();
		//根据屏幕大小缩放Canvas
		scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
		//设计分辨率
		scaler.referenceResolution=new Vector2(640.0f,960.0f);
		
		scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

		// matchWidthOrHeight 0：匹配宽度 1：匹配高度
		scaler.matchWidthOrHeight=1.0f;
	}*/

	//初始化设置横屏Canvas
	/*private void initLandscapeCanvas(Canvas canvas){
		canvas.renderMode=RenderMode.ScreenSpaceOverlay;

		var scaler=canvas.GetComponent<CanvasScaler>();
		
		scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
		
		scaler.referenceResolution=new Vector2(960.0f,640.0f);
		
		scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

		scaler.matchWidthOrHeight=0.0f;
	}*/

    
}
