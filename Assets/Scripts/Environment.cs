using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[ExecuteInEditMode]
public class Environment : MonoBehaviour
{
	public bool activateOnAwake = false;
	public Cubemap skyCube = null;
	public Cubemap specCube = null;
	public float camHDRExposure = 1.0f;
	
	public static Environment main = null;
	
	//	Skybox material, allocated only if requested.
	private Material _skyboxMaterial = null;
	private Material skyboxMaterial
	{
		get
		{
			if( _skyboxMaterial == null )
			{		
				Shader shader = Shader.Find( "Skybox/Cubemap" );
				if( shader )
				{
					_skyboxMaterial = new Material( shader );
					_skyboxMaterial.name = "Skybox";
				}
				else
					Debug.LogError( "Couldn't find " + shader.name + " shader" );
			}
			return _skyboxMaterial;
		}
	}

	void Awake()
	{
		if (activateOnAwake)
			Activate();
	}
	
	private void SetLights( bool enabled )
	{
		Light[] lights = GetComponentsInChildren<Light>();
		foreach( Light light in lights )
			light.enabled = enabled;
	}
	
	public void Deactivate()
	{
		SetLights( false );
	}
	
	private Environment[] CollectAllEnvironments()
	{
		return GameObject.FindObjectsOfType<Environment>();
	}

	void Update ()
	{
		if (main != this)
			return;
	}
	
	public void Activate()
	{
		#if UNITY_EDITOR
		var selectedGO = UnityEditor.Selection.activeGameObject;
		if (selectedGO)
		{
			var selectedEnvironment = selectedGO.GetComponent<Environment>();
			if (selectedEnvironment != null && selectedEnvironment != this)
				return;
		}
		#endif
		
		var envs = CollectAllEnvironments();
		foreach (var env in envs)
			if (env != this)
				env.Deactivate();

		SetupGraphicsParameters ();
	}

	public static Camera GetActiveCamera()
	{
		Camera[] cameras = FindObjectsOfType<Camera>();
		foreach(Camera camera in cameras)
		{
			if(camera.name != "CameraUI" && camera.enabled)
			{
				return camera;
			}
		}
		return null;
	}
	private void SetupGraphicsParameters ()
	{
		Camera cam = GetActiveCamera();
		if (cam)
		{
			var toneMap = cam.GetComponent<UnityStandardAssets.ImageEffects.Tonemapping> ();
			if (toneMap && toneMap.enabled)
				toneMap.exposureAdjustment = camHDRExposure;
		}

		RenderSettings.skybox = skyboxMaterial;
		if (skyCube)
		{
			skyboxMaterial.SetTexture ("_Tex", skyCube);
		}
		
		if (specCube)
		{
			RenderSettings.customReflection = specCube;
			RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;

			RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
			DynamicGI.UpdateEnvironment ();
		}

		SetLights( true );

		main = this;
		
	}
}
