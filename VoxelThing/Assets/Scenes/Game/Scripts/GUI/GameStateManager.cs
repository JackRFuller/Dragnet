using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {
	
	private static GameStateManager _manager;
	private static GameStateManager manager {
		get {
			if(_manager == null) _manager = (GameStateManager) GameObject.FindObjectOfType( typeof(GameStateManager) );
			return _manager;
		}
	}
	
	public static bool IsPause {
		set {
			if(value) Time.timeScale = 1f/10000f;
			if(!value) Time.timeScale = 1;
			Cursor.visible = value;
			if(value) manager.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);
			if(!value) manager.SendMessage("OnResume", SendMessageOptions.DontRequireReceiver);
		}
		get {
			return Time.timeScale <= 0.0001f;
		}
	}
	public static bool IsPlaying {
		set {
			IsPause = !value;
		}
		get {
			return !IsPause;
		}
	}

	
	void Start() {
		Cursor.visible = false;
	}
	
	void Update() {
		Screen.lockCursor = !Cursor.visible;
		
		if(Input.GetKeyDown(KeyCode.Tab)) {
			Cursor.visible = true;
			Screen.lockCursor = false;
		}
		
		if(Input.GetKeyDown(KeyCode.Escape)) {
			IsPause = !IsPause;
		}
	}
	
}
