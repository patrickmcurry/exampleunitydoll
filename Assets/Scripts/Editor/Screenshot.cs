using UnityEngine;
using UnityEditor;
using System.IO;

public class Screenshot : EditorWindow
{
	[MenuItem("Tools/Screenshot")]
	public static void TakeShot()
	{

		System.IO.Directory.CreateDirectory( "Screenshots" );
		var path = EditorUtility.SaveFilePanel(
			"Save screenshot as PNG",
			"Screenshots",
			"",
			"png");
		Application.CaptureScreenshot( path, 0 );
	}
 }
	
