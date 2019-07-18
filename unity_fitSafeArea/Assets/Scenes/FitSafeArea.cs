using UnityEngine;

public class FitSafeArea:MonoBehaviour{
	private Rect _safeArea;
	private Rect _lastSafeArea;
	private RectTransform _panel;

	private void Awake() {
		_panel=GetComponent<RectTransform>();

		_safeArea=new Rect(0.0f,0.0f,Screen.width*0.9f,Screen.height);//测试：取屏幕宽度的0.9
		//_safeArea=Screen.safeArea;
	}

	private void Start() {
		refresh(_safeArea);
	}

	private void Update() {
		refresh(_safeArea);
	}

	private void refresh(Rect r){
		if(_lastSafeArea==r)return;
		_lastSafeArea=r;
		//
		Debug.LogFormat("safeArea.position:{0}, safeArea.size:{1}",r.position,r.size);
		Debug.LogFormat("anchorMin:{0},anchorMax:{1}",_panel.anchorMin,_panel.anchorMax);
		Vector2 anchorMin=r.position;
		Vector2 anchorMax=r.position+r.size;
		//anchorMin(左上角)、anchorMax(右下角)表示在屏幕上的百分比位置,在屏幕内的取值范围是[0,1]
		anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;
		Debug.LogFormat("anchorMin:{0},anchorMax:{1}",_panel.anchorMin,_panel.anchorMax);
		Debug.Log("=====================================================================");
	}
}
