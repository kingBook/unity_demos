using System.Collections;
using System.Collections.Generic;
using NAudio.Wave;
using UnityEngine;

public class Main : MonoBehaviour{
	
	private IWavePlayer _player;
	private AudioFileReader _audioFileReader;
	
	private void Start() {
		/*_player=new WaveOut();
		_audioFileReader=new AudioFileReader("http://sc1.111ttt.cn/2018/1/03/13/396131226156.mp3");
		_player.Init(_audioFileReader);
		_player.Play();*/
		
	}

	private void dispose(){
		if(_player!=null){
			_player.Stop();
			_player.Dispose();
			_player=null;
		}
		if(_audioFileReader!=null){
			_audioFileReader.Dispose();
			_audioFileReader=null;
		}
	}

	private void OnDestroy() {
		dispose();
	}

	

	private void OnApplicationQuit() {
		dispose();
	}
}
