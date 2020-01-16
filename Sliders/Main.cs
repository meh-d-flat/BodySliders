using IllusionPlugin;
using UnityEngine;

namespace BodySliders
{
	public class SlidersPlugin : IEnhancedPlugin
	{
		bool pluginEnabled;
		
		public static Vector2 windowPosition = new Vector2(10, 10);
		
		public static bool onlyBodyValues;
		
		public string Name {
			get {
				return "BodySliders";
			}
		}

		public string Version {
			get {
				return "2.0";
			}
		}

		public string[] Filter {
			get {
				return new string[2] {
					"StudioNEO_32",
					"StudioNEO_64"
				};
			}
		}

		public void OnApplicationStart()
		{
			ModPrefs.SetString("BodySliders", "Unity3D_KeyCodes", "https://docs.unity3d.com/ScriptReference/KeyCode.html");
			ModPrefs.GetString("BodySliders", "enable|disable", "KeypadPeriod", true);
		}

		public void OnLateUpdate()
		{
			if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), ModPrefs.GetString("BodySliders", "enable|disable"))) && Manager.Scene.Instance.ActiveScene.name == "Studio")
				Switch();
			
			windowPosition = SlidersUI.windowMain.position;
			onlyBodyValues = SlidersUI.onlySliderValues;
		}
		
		public void OnUpdate() {}
		public void OnLevelWasLoaded(int level)	{}
		public void OnApplicationQuit()	{}
		public void OnLevelWasInitialized(int level) {}
		public void OnFixedUpdate()	{}
		
		//TODO: come up with something better for enable / disable
		void Switch()
		{
			if (pluginEnabled)
				UnityEngine.Object.DestroyImmediate(UnityEngine.Object.FindObjectOfType<SlidersUI>(), true);
			else
				Object.FindObjectOfType<StudioScene>().gameObject.AddComponent<SlidersUI>();
			
			Studio.Studio.Instance.cameraCtrl.enabled = true;
			UnityEngine.Object.FindObjectOfType<StudioScene>().cameraInfo.cameraCtrl.enabled = true;
			Studio.Studio.Instance.colorPaletteCtrl.visible = false;
			pluginEnabled = !pluginEnabled;
		}
	}
}
