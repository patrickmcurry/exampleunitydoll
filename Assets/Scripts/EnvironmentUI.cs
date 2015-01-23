using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent (typeof (ToggleGroup))]
public class EnvironmentUI : MonoBehaviour {

	public Toggle environmentTogglePrefab;
	public GameObject environmentsParent;
	public float biasOffset;
	public EventSystem eventSystem;

	void Start () {
		Environment[] environments = environmentsParent.GetComponentsInChildren<Environment> ();
		ToggleGroup group = GetComponent<ToggleGroup> ();
		Canvas canvas = GetComponentInParent<Canvas> ();

		for (int i = 0; i < environments.Length; i++)
		{
			Environment environment = environments[i];

			Toggle t = Instantiate<Toggle>(environmentTogglePrefab);
			t.transform.SetParent(transform);
			t.transform.localPosition = Vector3.zero;
			t.transform.localScale = Vector3.one;
			t.transform.localRotation = Quaternion.identity;
			t.isOn = environment.activateOnAwake;
			t.onValueChanged.AddListener((b)=> {if (b) environment.Activate();});
			t.group = group;

			if (t.isOn)
			{
				eventSystem.SetSelectedGameObject(t.gameObject);
			}

			Material material = t.GetComponentInChildren<MeshRenderer>().material;
			material.SetTexture ("_Cube", environment.specCube);
			float sphereSizeInScreen = 43 * canvas.scaleFactor;
			float bias = - Mathf.Max(0, environment.specCube.height / sphereSizeInScreen);
			material.SetFloat("_Bias",bias);
		}
	}
}