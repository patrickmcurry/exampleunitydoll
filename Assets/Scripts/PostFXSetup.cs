using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
public class PostFXSetup : MonoBehaviour
{
	void Start()
	{
		Camera high = GameObject.Find("Camera_high").GetComponent<Camera>();
		Camera med = GameObject.Find("Camera_med").GetComponent<Camera>();
		Camera low = GameObject.Find("Camera_low").GetComponent<Camera>();
		
		if(high && med && low)
		{
			high.enabled = med.enabled = low.enabled = false;

			int qualityLevel = QualitySettings.GetQualityLevel();
			var sm40 = (SystemInfo.graphicsShaderLevel >= 40);
			var sm30 = (SystemInfo.graphicsShaderLevel >= 30);

			if (sm40 && SystemInfo.deviceType != DeviceType.Handheld && qualityLevel >= 1)
				high.enabled = true;
			else if (sm30 && SystemInfo.deviceType != DeviceType.Handheld && qualityLevel >= 1)
				med.enabled = true;
			else
				low.enabled = true;
		}
		else
			Debug.LogError("Cameras missing! Different cameras are needed for different performance profiles.");
	}
}
