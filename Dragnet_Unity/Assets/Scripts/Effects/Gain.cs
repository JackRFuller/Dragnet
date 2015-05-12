using UnityEngine;
using System.Collections;

public class Gain : MonoBehaviour {

	public float gain;

	void OnAudioFilterRead(float[] data, int channels) 
    {
		for (int i = 0; i < data.Length; ++i)
			data[i] = data[i] * gain;			
	}
}
