using UnityEngine;
using System;
using System.Collections;

public class QualitySetup : MonoBehaviour
{
	void Awake()
	{

	#if UNITY_IPHONE
		if(	(UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad2Gen) ||
			(UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPad3Gen) ||
			(UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone4)	 ||
			(UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone3GS)	)
		{
			var index = Array.IndexOf(QualitySettings.names, "Fastest");
			if(index >= 0)
				QualitySettings.SetQualityLevel(index, true);
		}
	#endif

	#if UNITY_ANDROID
		if (SystemInfo.graphicsShaderLevel < 30)
		{
			var index = Array.IndexOf(QualitySettings.names, "Fastest");
			if(index >= 0)
				QualitySettings.SetQualityLevel(index, true);
		}
		// only way to keep animations smooth
		QualitySettings.vSyncCount = 1;

		// drop to lowres textures if we don't have enough vram to fit all textures
		QualitySettings.masterTextureLimit = 256 / SystemInfo.graphicsMemorySize;

		Console.WriteLine("===========================");
		Console.WriteLine("Quality setting : " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
		Console.WriteLine("Master texture limit: " + QualitySettings.masterTextureLimit);
		Console.WriteLine("===========================");
	#endif

		// Ensure screen stays on
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// Okay, all should be alright now.. Go ahead and load the first scene.
		Application.LoadLevel("Doll");
	}
}
