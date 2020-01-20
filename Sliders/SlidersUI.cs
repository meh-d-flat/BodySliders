using Studio;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace BodySliders
{
	public class SlidersUI : MonoBehaviour
	{
		static float X = ((float)Screen.width / 5), Y = ((float)Screen.height / 2);
		
		public static Rect windowMain = new Rect(10, 10, X, Y);
		
		public static bool onlySliderValues;

		int R1ID, R2ID;
		
		string presetName = String.Empty, charaNewName = String.Empty;
	
		float wideSliderMin = (float)LimitConfig.NewMin / 100, wideSliderMax = (float)LimitConfig.NewMax / 100;//WideSlider

		SubType subType;
		
		OCIChar studioChar;
		
		CharFemale charaFemale;
		
		Vector2 scrollView, scrollViewSub;
		
		Studio.Studio studio = Studio.Studio.Instance;
		
		CustomMenu.SmCustomLoad.FileInfo[] presetsList;
		
		Rect windowSub = new Rect((int)(Screen.width / 3.35F), 0, (int)(Screen.width / 2.485F) / 2, Screen.height / 2);

		CharacterPart somePart;
		
		CharaList listFemale, listMale;
		
		CharaFileSort charaFiles;
		
		Transform mainCanvas;
		
		public enum SubType
		{
			none,
			FacePresets,
			BodyPresets
		}
		
		void Awake()
		{
			R1ID = new System.Random().Next();
			R2ID = R1ID - 1;
			windowMain.position = SlidersPlugin.windowPosition;
			subType = SubType.none;
			onlySliderValues = SlidersPlugin.onlyBodyValues;
			somePart = AllParts.nullPart;
		}
		
		void Start()
		{
			mainCanvas = studio.gameObject.transform.Find("Canvas Main Menu");
			listFemale = mainCanvas.Find("01_Add/00_Female").gameObject.GetComponent<CharaList>();
			listMale = mainCanvas.Find("01_Add/01_Male").gameObject.GetComponent<CharaList>();
		}
		
		void LateUpdate()
		{
			var previousChara = CharacterPart.referenceChara;
			studio.cameraCtrl.enabled = !(windowMain.Contains(Event.current.mousePosition) || windowSub.Contains(Event.current.mousePosition));
			studioChar = GetDamnChara();
			CharacterPart.referenceChara = charaFemale = (studioChar != null) ? studioChar.charInfo as CharFemale : null;
			if (CharacterPart.referenceChara != previousChara)
			{
				somePart.ClearTheChain();
				somePart.Init();
			}
		}
		
		void OnGUI()
		{
			windowMain = GUI.Window(R1ID, windowMain, SlidersWindow, "Body Sliders Plugin");
			if ((subType != SubType.none || somePart != AllParts.nullPart) && Check())
				windowSub = GUI.Window(R2ID, windowSub, PartsWindow, somePart.partName);
		}
		
		void PartsWindow(int windowID)
		{
			scrollViewSub = GUILayout.BeginScrollView(scrollViewSub);
			
			if (somePart == AllParts.nullPart && subType == SubType.BodyPresets)
				Presets(presetsList, true);
			else if (somePart == AllParts.nullPart && subType == SubType.FacePresets)
				Presets(presetsList, false);
			else
				somePart.ActTheChain();
			
			GUILayout.EndScrollView();
			GUI.DragWindow();
		}
    
		void SlidersWindow(int windowID)
		{			
			if (Check())
			{
				scrollView = GUILayout.BeginScrollView(scrollView, GUILayout.Width(X - 15), GUILayout.Height(Y - (Y / 10)));
				
				GUILayout.Label("New name for chara:");
				charaNewName = GUILayout.TextField(charaNewName, 30);
			
				GUILayout.Label("Body|Face preset name:");
				presetName = GUILayout.TextField(presetName, 30);
			
				if (GUILayout.Button("SAVE BODY")) {
					SavePreset(true);
					presetsList = PresetList(true);
				}
				if (GUILayout.Button("SAVE FACE")) {
					SavePreset(false);
					presetsList = PresetList(false);
				}
				
				onlySliderValues = GUILayout.Toggle(onlySliderValues, "Load the sliders only");
				if (GUILayout.Button("LOAD BODY")) {
					presetsList = PresetList(true);
					subType = SubType.BodyPresets;
					somePart = AllParts.nullPart;
				}
				if (GUILayout.Button("LOAD FACE")) {
					presetsList = PresetList(false);
					subType = SubType.FacePresets;
					somePart = AllParts.nullPart;
				}
				
				GUILayout.Label("§>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
				somePart = AllParts.GetSlider(somePart);
				
				GUILayout.Label("§>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
				somePart = AllParts.GetPart(somePart);
				
				GUILayout.Label("§ Load selected char's:");
				if (GUILayout.Button("Hair"))
					LoadHair();
					
				if (GUILayout.Button("Face"))
					LoadFace();
				
				if (GUILayout.Button("Body"))
					LoadBody();
				
				DeleteButton();
				
				GUILayout.EndScrollView();				
				if (GUILayout.Button("SAVE CHAR"))
					Save();
			}
			else if (studioChar.charInfo.Sex == 0)
			{
				GUILayout.Label("Select a female character!");
				GUILayout.Label("New name for char:");
				charaNewName = GUILayout.TextField(charaNewName, 30);
				if (GUILayout.Button("SAVE MALE CHAR"))
					Save();	
				
				DeleteButton();
			}
			else
			{
				GUILayout.Label("Select a character!");
				DeleteButton();
			}
			
			GUI.DragWindow();
		}

		void DeleteButton()
		{
			GUILayout.Label("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			if (GUILayout.Button("DELETE CHARACTER SELECTED IN CHAR LIST"))
				Delete();
			GUILayout.Label("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		}
		
		bool Check()
		{
			return studioChar != null && studioChar.charInfo.Sex != 0;
		}
		
		void Presets(CustomMenu.SmCustomLoad.FileInfo[] presetList, bool body)
		{
			for (int i = 0; i < presetList.Length; i++) {
				if (GUILayout.Button("" + presetList[i].comment))
					LoadPreset(body, presetList[i].fullPath);
			}
		}
	
		CustomMenu.SmCustomLoad.FileInfo [] PresetList(bool body)
		{
			var presetList = new List<CustomMenu.SmCustomLoad.FileInfo>(32);
			var folderAssist = new FolderAssist();
			string folder = UserData.Path + "custom/female/";
			string[] searchPattern = body ? new string []{ "*.body" } : new string [] { "*.face" };
			folderAssist.CreateFolderInfoEx(folder, searchPattern, true);			
			var type = (byte)(body ? 0 : 1);// 0 for body 1 for face
			CustomMenu.SmCustomLoad.CustomData customData = new CustomMenu.SmCustomLoad.CustomDataFemale(type);

			for (int i = 0; i < folderAssist.lstFile.Count; i++) {
				customData.Load(folderAssist.lstFile[i].FullPath, null);
				var fileInfo = new CustomMenu.SmCustomLoad.FileInfo();
				fileInfo.no = i;
				fileInfo.fullPath = folderAssist.lstFile[i].FullPath;
				fileInfo.fileName = folderAssist.lstFile[i].FileName;
				fileInfo.time = folderAssist.lstFile[i].time;
				fileInfo.type = (int)type;
				fileInfo.comment = customData.comment;
				presetList.Add(fileInfo);
			}
			return presetList.ToArray();
		}
		
		void SavePreset(bool body)
		{
			var setName = (presetName == String.Empty) ? "unnamed" : presetName;
			var type = (byte)(body ? 0 : 1);
			string extension = body ? ".body" : ".face";	
			CustomMenu.SmCustomLoad.CustomData customData = new CustomMenu.SmCustomLoad.CustomDataFemale(type);
			string filePath = UserData.Path + "custom/female/f_" + setName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
			customData.comment = setName;
			customData.Save(filePath, charaFemale.customInfo);
			presetName = String.Empty;
		}
		
		void LoadPreset(bool body, string path)
		{
			var mouth = charaFemale.chaBody.mouthCtrl.FixedRate;
			var eyes = charaFemale.chaBody.eyesCtrl.OpenMax;
			var type = (byte)(body ? 0 : 1);
			CustomMenu.SmCustomLoad.CustomData customData = new CustomMenu.SmCustomLoad.CustomDataFemale(type);
			if (onlySliderValues) {
				var temporaryPizda = new CharFileInfoCustomFemale();
				customData.Load(path, temporaryPizda);
				if (body) {
					for (int i = 0; i < charaFemale.customInfo.shapeValueBody.Length; i++)
						charaFemale.customInfo.shapeValueBody[i] = temporaryPizda.shapeValueBody[i];
					charaFemale.femaleCustomInfo.bustWeight = temporaryPizda.bustWeight;
					charaFemale.femaleCustomInfo.bustSoftness = temporaryPizda.bustSoftness;
				} else
					for (int i = 0; i < charaFemale.customInfo.shapeValueFace.Length; i++)
						charaFemale.customInfo.shapeValueFace[i] = temporaryPizda.shapeValueFace[i];
			} else
				customData.Load(path, charaFemale.customInfo);
			
			charaFemale.Reload(true, false, true);
			charaFemale.UpdateBustSoftnessAndGravity();
			charaFemale.chaBody.mouthCtrl.FixedRate = mouth;
			charaFemale.chaBody.eyesCtrl.OpenMax = eyes;
		}
		
		OCIChar GetDamnChara()
		{
			if (studio.treeNodeCtrl.selectNode != null) {
				ObjectCtrlInfo OCI = null;
				if (studio.dicInfo.TryGetValue(studio.treeNodeCtrl.selectNode, out OCI) && OCI.kind == 0)
					return OCI as OCIChar;
			}
			return null;
		}

		void Save()
		{
			var chara = GetDamnChara().charInfo.chaFile;
			GetDamnChara().charInfo.customInfo.name = (charaNewName != String.Empty) ? charaNewName : GetDamnChara().charInfo.customInfo.name;
			string path2 = chara.ConvertCharaFilePath(String.Empty, true);
			string directoryName = System.IO.Path.GetDirectoryName(path2);
			using (var fileStream = new System.IO.FileStream(path2, System.IO.FileMode.Create, System.IO.FileAccess.Write))
				chara.Save(fileStream);
			var rt = new RenderTexture(Screen.width, Screen.height, 24);
			studio.cameraCtrl.mainCmaera.targetTexture = rt;
			var screenShot = new Texture2D((int)(Screen.width / 2.485F), Screen.height, TextureFormat.RGB24, false);
			studio.cameraCtrl.mainCmaera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect((int)(Screen.width / 3.35F), 0, (int)(Screen.width / 2.485F), Screen.height), 0, 0);
			studio.cameraCtrl.mainCmaera.targetTexture = null;
			RenderTexture.active = null;
			Destroy(rt);
			byte[] bytes = screenShot.EncodeToPNG();
			chara.ChangeSavePng(path2, bytes); 
			charaNewName = String.Empty;
			mainCanvas.gameObject.SetActive(false);
			listFemale.InitCharaList(true);
			listMale.InitCharaList(true);
			mainCanvas.gameObject.SetActive(true);
		}
		
		CharFileInfoCustomFemale GetCharCustom()
		{
			CharFileInfoCustomFemale charCustomFile = null;
			CharaList operatingList = mainCanvas.Find("01_Add/00_Female").gameObject.activeInHierarchy ? listFemale : null;
			if (operatingList != null)
			{
				charaFiles = (CharaFileSort)operatingList.GetType()
				.GetField("charaFileSort", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(operatingList);
				if (System.IO.File.Exists(charaFiles.selectPath) && charaFiles.selectPath != null)
				{
					CharFemaleFile charPNG = new CharFemaleFile();
					charPNG.Load(charaFiles.selectPath, true, true);
					charCustomFile = charPNG.femaleCustomInfo;
				}
				return charCustomFile;
			}
			return charCustomFile;
		}
		
		void Delete()
		{
			CharaList operatingList = mainCanvas.Find("01_Add/00_Female").gameObject.activeInHierarchy ? listFemale : mainCanvas.Find("01_Add/01_Male").gameObject.activeInHierarchy ? listMale : null;
			if (operatingList != null)
			{
				charaFiles = (CharaFileSort)operatingList.GetType()
				.GetField("charaFileSort", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(operatingList);
				if (System.IO.File.Exists(charaFiles.selectPath) && charaFiles.selectPath != null)
				{
					var sortType = charaFiles.sortKind;
					mainCanvas.gameObject.SetActive(false);
					System.IO.File.Delete(charaFiles.selectPath);
					operatingList.InitCharaList(true);
					operatingList.OnSort(sortType);
					mainCanvas.gameObject.SetActive(true);
				}
			}
		}
		
		void LoadBody()
		{
			CharFileInfoCustomFemale cfic = GetCharCustom();
			if (cfic != null)
			{
				charaFemale.femaleCustomInfo.shapeValueBody = cfic.shapeValueBody;
				charaFemale.femaleCustomInfo.areolaSize = cfic.areolaSize;
				charaFemale.femaleCustomInfo.bodyDetailWeight = cfic.bodyDetailWeight;
//				charaFemale.femaleCustomInfo.bustSoftness = cfic.bustSoftness;
//				charaFemale.femaleCustomInfo.bustWeight = cfic.bustWeight;
				charaFemale.femaleCustomInfo.matNipId = cfic.matNipId;
				charaFemale.femaleCustomInfo.matUnderhairId = cfic.matUnderhairId;
				charaFemale.femaleCustomInfo.nailColor = cfic.nailColor;
				charaFemale.femaleCustomInfo.nipColor = cfic.nipColor;
				charaFemale.femaleCustomInfo.skinColor = cfic.skinColor;
				charaFemale.femaleCustomInfo.sunburnColor = cfic.sunburnColor;
				charaFemale.femaleCustomInfo.tattoo_bColor = cfic.tattoo_bColor;
				charaFemale.femaleCustomInfo.texBodyDetailId = cfic.texBodyDetailId;
				charaFemale.femaleCustomInfo.texBodyId = cfic.texBodyId;
				charaFemale.femaleCustomInfo.texSunburnId = cfic.texSunburnId;
				charaFemale.femaleCustomInfo.texTattoo_bId = cfic.texTattoo_bId;
				charaFemale.femaleCustomInfo.underhairColor = cfic.underhairColor;
				
				charaFemale.femaleCustom.UpdateShapeBodyValueFromCustomInfo();
				charaFemale.femaleCustom.UpdateShapeBody();
				charaFemale.ChangeBustGravity(cfic.bustWeight);
				charaFemale.ChangeBustSoftness(cfic.bustSoftness);
				charaFemale.ReSetupDynamicBone();
				charaFemale.UpdateBustSoftnessAndGravity();
				charaFemale.femaleBody.updateBustSize = true;
				charaFemale.femaleCustom.ChangeCustomBodyWithoutCustomTexture();
				Functionality.UpdateBody(charaFemale, true, true, true);
				charaFemale.UpdateFace();
			}
		}
		
		void LoadFace()
		{
			CharFileInfoCustomFemale cfic = GetCharCustom();
			if (cfic != null)
			{					
				charaFemale.femaleCustomInfo.cheekColor = cfic.cheekColor;
				charaFemale.femaleCustomInfo.eyebrowColor = cfic.eyebrowColor;
				charaFemale.femaleCustomInfo.eyeHiColor = cfic.eyeHiColor;
				charaFemale.femaleCustomInfo.eyelashesColor = cfic.eyelashesColor;
				charaFemale.femaleCustomInfo.eyeLColor = cfic.eyeLColor;
				charaFemale.femaleCustomInfo.eyeRColor = cfic.eyeRColor;
				charaFemale.femaleCustomInfo.eyeshadowColor = cfic.eyeshadowColor;
				charaFemale.femaleCustomInfo.eyeWColor = cfic.eyeWColor;
				charaFemale.femaleCustomInfo.faceDetailWeight = cfic.faceDetailWeight;
				charaFemale.femaleCustomInfo.headId = cfic.headId;
				charaFemale.femaleCustomInfo.lipColor = cfic.lipColor;
				charaFemale.femaleCustomInfo.matEyebrowId = cfic.matEyebrowId;
				charaFemale.femaleCustomInfo.matEyeHiId = cfic.matEyeHiId;
				charaFemale.femaleCustomInfo.matEyelashesId = cfic.matEyelashesId;
				charaFemale.femaleCustomInfo.matEyeLId = cfic.matEyeLId;
				charaFemale.femaleCustomInfo.matEyeRId = cfic.matEyeRId;
				charaFemale.femaleCustomInfo.matNipId = cfic.matNipId;
				charaFemale.femaleCustomInfo.matUnderhairId = cfic.matUnderhairId;
				charaFemale.femaleCustomInfo.moleColor = cfic.moleColor;
				charaFemale.femaleCustomInfo.shapeValueFace = cfic.shapeValueFace;
				charaFemale.femaleCustomInfo.tattoo_fColor = cfic.tattoo_fColor;
				charaFemale.femaleCustomInfo.texCheekId = cfic.texCheekId;
				charaFemale.femaleCustomInfo.texEyeshadowId = cfic.texEyeshadowId;
				charaFemale.femaleCustomInfo.texFaceDetailId = cfic.texFaceDetailId;
				charaFemale.femaleCustomInfo.texFaceId = cfic.texFaceId;
				charaFemale.femaleCustomInfo.texLipId = cfic.texLipId;
				charaFemale.femaleCustomInfo.texMoleId = cfic.texMoleId;
				charaFemale.femaleCustomInfo.texTattoo_fId = cfic.texTattoo_fId;
			
				charaFemale.femaleBody.ChangeHeadNew();
				charaFemale.femaleCustom.UpdateShapeFaceValueFromCustomInfo();
				charaFemale.femaleCustom.ChangeCustomFaceWithoutCustomTexture();
				charaFemale.UpdateFace();
			}
		}
		
		void LoadHair()
		{
			CharFileInfoCustomFemale cfic = GetCharCustom();
			if (cfic != null)
			{			
				charaFemale.femaleCustomInfo.hairAcsColor = cfic.hairAcsColor;
				charaFemale.femaleCustomInfo.hairColor = cfic.hairColor;
				charaFemale.femaleCustomInfo.hairId = cfic.hairId;
				charaFemale.femaleCustomInfo.hairType = cfic.hairType;
					
				charaFemale.femaleBody.ChangeHair(true);
				for (int i = 0; i < charaFemale.femaleCustomInfo.hairId.Length; i++)
					charaFemale.femaleCustom.ChangeHairColor(i);
			}
		}
	}
}
