// Thinking about turning the template into a delegate chain next time
using System;
using UnityEngine;
using System.Linq;

namespace BodySliders
{
	public static class Extensions
	{
		public static float AbsSlider(float value, string labelName = null)
		{
			GUILayout.BeginHorizontal();
			if (labelName != null)
				GUILayout.Label(labelName, GUILayout.Width(70f));
			value = GUILayout.HorizontalSlider(value, (float)LimitConfig.NewMin / 100, (float)LimitConfig.NewMax / 100);
			GUILayout.Label(value.ToString("0.00"), GUILayout.Width(30f));
			GUILayout.EndHorizontal();
			return value;
		}
		
		public static void NewCSlider(HSColorSet C, Action setter)
		{
			GUILayout.BeginHorizontal();
			var temp1 = C.specularIntensity;
			C.specularIntensity = GUILayout.HorizontalSlider(C.specularIntensity, (float)LimitConfig.NewMin / 100, (float)LimitConfig.NewMax / 100);
			if (Math.Abs(temp1 - C.specularIntensity) > 0.01F)
				setter();
			GUILayout.Label(C.specularIntensity.ToString("0.00"), GUILayout.Width(30f));
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			var temp2 = C.specularSharpness;
			C.specularSharpness = GUILayout.HorizontalSlider(C.specularSharpness, (float)LimitConfig.NewMin / 100, (float)LimitConfig.NewMax / 100);
			if (Math.Abs(temp2 - C.specularSharpness) > 0.01F)
				setter();
			GUILayout.Label(C.specularSharpness.ToString("0.00"), GUILayout.Width(30f));
			GUILayout.EndHorizontal();
		}
		
		public static void ButtonTypical(HSColorSet color, Action changer, bool isDiffuse = true)
		{
			string name = isDiffuse ? "Diffuse Color" : "Specular Color";
			if (GUILayout.Button(name)) {
				Studio.Studio.Instance.colorPaletteCtrl.visible = false;
				if (isDiffuse) {
					Studio.Studio.Instance.colorMenu.SetColor(color.rgbaDiffuse, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
						color.SetDiffuseRGBA(c);
						changer();
					});
				} else {
					Studio.Studio.Instance.colorMenu.SetColor(color.rgbSpecular, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
						color.SetSpecularRGB(c);
						changer();
					});
				}
				Studio.Studio.Instance.colorPaletteCtrl.visible = true;
			}
		}
		
		public static void UpdateBody(this CharFemale chara, bool body, bool bodyTatoo, bool bodySunburn)
		{	
			Texture2D texture2D = null;
			Texture texture = null;
			Texture texture2 = null;
			
			if (body) {
				chara.femaleBody.customTexCtrlBody.SetColor(Manager.Character.Instance._Color, chara.femaleCustomInfo.skinColor.rgbaDiffuse);
				chara.femaleBody.customMatBody.SetColor(Manager.Character.Instance._SpecColor, chara.femaleCustomInfo.skinColor.rgbSpecular);
				chara.femaleBody.customMatBody.SetFloat(Manager.Character.Instance._Metallic, chara.femaleCustomInfo.skinColor.specularIntensity);
				chara.femaleBody.customMatBody.SetFloat(Manager.Character.Instance._Smoothness, chara.femaleCustomInfo.skinColor.specularSharpness);
				chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_body, ref chara.femaleCustomInfo.texBodyId, ref texture2D, ref texture, ref texture2);
				chara.femaleBody.customTexCtrlBody.SetMainTexture(texture2D);
			}
			if (bodyTatoo) {
				chara.femaleBody.customTexCtrlBody.SetColor(Manager.Character.Instance._Color3, chara.femaleCustomInfo.tattoo_bColor.rgbaDiffuse);
				chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_tattoo_b, ref chara.femaleCustomInfo.texTattoo_bId, ref texture2D, ref texture, ref texture2);
				chara.femaleBody.customTexCtrlBody.SetTexture(Manager.Character.Instance._Texture3, texture2D);
			}
			if (bodySunburn) {
				chara.femaleBody.customTexCtrlBody.SetColor(Manager.Character.Instance._Color2, chara.femaleCustomInfo.sunburnColor.rgbaDiffuse);
				chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_sunburn, ref chara.femaleCustomInfo.texSunburnId, ref texture2D, ref texture, ref texture2);
				chara.femaleBody.customTexCtrlBody.SetTexture(Manager.Character.Instance._Texture2, texture2D);
			}
			chara.femaleBody.customTexCtrlBody.RebuildTextureAndSetMaterial();
		}
		
		public static void UpdateFace(this CharFemale chara)
		{
			Texture2D texture2D = null;
			Texture texture = null;
			Texture texture2 = null;
			
			chara.femaleBody.customTexCtrlFace.SetColor(Manager.Character.Instance._Color, chara.femaleCustomInfo.skinColor.rgbaDiffuse);
			chara.femaleBody.customMatFace.SetFloat(Manager.Character.Instance._Metallic, chara.femaleCustomInfo.skinColor.specularIntensity);
			chara.femaleBody.customMatFace.SetFloat(Manager.Character.Instance._Smoothness, chara.femaleCustomInfo.skinColor.specularSharpness);
			chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_face, ref chara.femaleCustomInfo.texFaceId, ref texture2D, ref texture, ref texture2);
			chara.femaleBody.customTexCtrlFace.SetMainTexture(texture2D);
			
			chara.femaleBody.customTexCtrlFace.SetColor(Manager.Character.Instance._Color3, chara.femaleCustomInfo.tattoo_fColor.rgbaDiffuse);
			chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_tattoo_f, ref chara.femaleCustomInfo.texTattoo_fId, ref texture2D, ref texture, ref texture2);
			chara.femaleBody.customTexCtrlFace.SetTexture(Manager.Character.Instance._Texture3, texture2D);
			
			chara.femaleBody.customTexCtrlFace.SetColor(Manager.Character.Instance._Color4, chara.femaleCustomInfo.cheekColor.rgbaDiffuse);
			chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_cheek, ref chara.femaleCustomInfo.texCheekId, ref texture2D, ref texture, ref texture2);
			chara.femaleBody.customTexCtrlFace.SetTexture(Manager.Character.Instance._Texture4, texture2D);
			
			chara.femaleBody.customTexCtrlFace.SetColor(Manager.Character.Instance._Color5, chara.femaleCustomInfo.eyeshadowColor.rgbaDiffuse);
			chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_eyeshadow, ref chara.femaleCustomInfo.texEyeshadowId, ref texture2D, ref texture, ref texture2);
			chara.femaleBody.customTexCtrlFace.SetTexture(Manager.Character.Instance._Texture5, texture2D);
			
			chara.femaleBody.customTexCtrlFace.SetColor(Manager.Character.Instance._Color6, chara.femaleCustomInfo.lipColor.rgbaDiffuse);
			chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_lip, ref chara.femaleCustomInfo.texLipId, ref texture2D, ref texture, ref texture2);
			chara.femaleBody.customTexCtrlFace.SetTexture(Manager.Character.Instance._Texture6, texture2D);
			
			chara.femaleBody.customTexCtrlFace.SetColor(Manager.Character.Instance._Color7, chara.femaleCustomInfo.moleColor.rgbaDiffuse);
			chara.femaleCustom.LoatTextureInfo(CharaListInfo.TypeFemaleTexture.cf_t_mole, ref chara.femaleCustomInfo.texMoleId, ref texture2D, ref texture, ref texture2);
			chara.femaleBody.customTexCtrlFace.SetTexture(Manager.Character.Instance._Texture7, texture2D);
			
			chara.femaleBody.customTexCtrlFace.RebuildTextureAndSetMaterial();
		}
		
		public static void FaceSlider(this CharFemale chara, string[] partNames, int partIndex)
		{
			float newValue = AbsSlider(chara.customInfo.shapeValueFace[partIndex], partNames[partIndex]);
			if (Math.Abs(newValue - chara.customInfo.shapeValueFace[partIndex]) > 0.01F)
				chara.chaCustom.SetShapeFaceValue(partIndex, newValue);
		}
		
		public static void BodySlider(this CharFemale chara, string[] partNames, int partIndex)
		{
			float newValue = AbsSlider(chara.customInfo.shapeValueBody[partIndex], partNames[partIndex]);
			if (Math.Abs(newValue - chara.customInfo.shapeValueBody[partIndex]) > 0.01F)
				chara.chaCustom.SetShapeBodyValue(partIndex, newValue);
		}
	}
	
	public class PartSliders : CharaParts
	{
		protected override void ButtonDiffuse()
		{
		}
		protected override void ButtonSpecular()
		{
		}
		public override void UpdatePart()
		{
		}
		protected override void ListButtonLogic(int partID)
		{
		}
		
		void SetGroup(string name)
		{
			partName = name;
			color = null;
			hasSlider = hasPartList = false;
			listAllParts = new ListInfoBase[0];
		}
		
		public class GroupBody : PartSliders
		{
			public GroupBody()
			{
				SetGroup("Body Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				chara.BodySlider(SliderNames.body, 0);
				for (int i = 9; i < 32; i++)
					chara.BodySlider(SliderNames.body, i);
			}
		}
		
		public class GroupTits : PartSliders
		{
			public GroupTits()
			{
				SetGroup("Tits Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 1; i < 9; i++)
					chara.BodySlider(SliderNames.body, i);
				float newTitWeight = Extensions.AbsSlider(chara.femaleCustomInfo.bustWeight, "Tits Weight");
				float newTitSoft = Extensions.AbsSlider(chara.femaleCustomInfo.bustSoftness, "Tits Soft");
				if (Math.Abs(newTitWeight - chara.femaleCustomInfo.bustWeight) > 0.01F || Math.Abs(newTitSoft - chara.femaleCustomInfo.bustSoftness) > 0.01F) {
					chara.ChangeBustGravity(newTitWeight);
					chara.ChangeBustSoftness(newTitSoft);
				}
				chara.ReSetupDynamicBone();
				chara.UpdateBustSoftnessAndGravity();
				chara.femaleBody.updateBustSize = true;
			}
		}
		
		public class GroupFace : PartSliders
		{
			public GroupFace()
			{
				SetGroup("Face Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 0; i < 5; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
		
		public class GroupJawChin : PartSliders
		{
			public GroupJawChin()
			{
				SetGroup("Jaw and Chin Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 5; i < 13; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
		
		public class GroupCheek : PartSliders
		{
			public GroupCheek()
			{
				SetGroup("Cheeks Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 13; i < 19; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
		
		public class GroupEyebrows : PartSliders
		{
			public GroupEyebrows()
			{
				SetGroup("Eyebrows Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 19; i < 24; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
		
		public class GroupEyes : PartSliders
		{
			public GroupEyes()
			{
				SetGroup("Eyes Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 24; i < 40; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
		
		public class GroupNose : PartSliders
		{
			public GroupNose()
			{
				SetGroup("Nose Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 40; i < 55; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
		
		public class GroupMouth : PartSliders
		{
			public GroupMouth()
			{
				SetGroup("Mouth Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 55; i < 62; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
		
		public class GroupEars : PartSliders
		{
			public GroupEars()
			{
				SetGroup("Ears Sliders");
			}
			
			protected override void AdditionalGUI()
			{
				for (int i = 62; i < 67; i++)
					chara.FaceSlider(SliderNames.face, i);
			}
		}
	}
	
	public abstract class CharaParts
	{
		public static CharFemale chara;
		
		public static readonly CharaParts nullPart = new CharaParts.Null();
		
		public string partName;
		
		protected HSColorSet color;
		
		protected ListInfoBase[] listAllParts;
		
		protected bool hasSlider = true, hasPartList = true;
		
		Vector2 listScrollView;
		
		protected abstract void ButtonDiffuse();
		
		protected abstract void ButtonSpecular();
		
		public abstract void UpdatePart();
		
		protected virtual void AdditionalGUI()
		{
		}
		
		protected abstract void ListButtonLogic(int partID);
		
		void GUIListDraw()
		{
			listScrollView = GUILayout.BeginScrollView(listScrollView);
			for (int i = 0; i < listAllParts.Length; i++)
				if (GUILayout.Button("" + listAllParts[i].Name))
					ListButtonLogic(listAllParts[i].Id);
			GUILayout.EndScrollView();
		}
		
		public void DoGUIAndLogic()
		{
			ButtonDiffuse();
			ButtonSpecular();
			if (hasSlider)
				Extensions.NewCSlider(color, UpdatePart);
			AdditionalGUI();
			if (hasPartList)
				GUIListDraw();
		}
		
		class Null : CharaParts
		{
			public Null()
			{
				partName = "No Part Selected";
				color = null;
				hasSlider = hasPartList = false;
			}
			protected override void ButtonDiffuse()
			{
			}
			protected override void ButtonSpecular()
			{
			}
			public override void UpdatePart()
			{
			}
			protected override void AdditionalGUI()
			{
			}
			protected override void ListButtonLogic(int partID)
			{
			}
		}
		
		public class EyeR : CharaParts
		{
			public EyeR()
			{
				partName = "Eye Right";
				color = chara.femaleCustomInfo.eyeRColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyeball).Values.ToArray();
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyeRColor);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyeRColor, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeEyeR();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matEyeRId = partID;
				UpdatePart();
			}
		}
		
		public class EyeL : CharaParts
		{
			public EyeL()
			{
				partName = "Eye Left";
				color = chara.femaleCustomInfo.eyeLColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyeball).Values.ToArray();		
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyeLColor);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyeLColor, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeEyeL();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matEyeLId = partID;
				UpdatePart();
			}
		}
		
		public class Eyes : CharaParts
		{
			public Eyes()
			{
				partName = "Both Eyes";
				color = chara.femaleCustomInfo.eyeLColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyeball).Values.ToArray();			
			}
			public override void UpdatePart()
			{
				chara.femaleCustomInfo.eyeRColor = chara.femaleCustomInfo.eyeLColor;
				chara.femaleCustom.ChangeEyeL();
				chara.femaleCustom.ChangeEyeR();
			}
			protected override void ButtonDiffuse()
			{
				if (GUILayout.Button("Diffuse Color")) {
					Studio.Studio.Instance.colorMenu.SetColor(color.rgbaDiffuse, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
						chara.femaleCustomInfo.eyeLColor.SetDiffuseRGBA(c);
						chara.femaleCustomInfo.eyeRColor.SetDiffuseRGBA(c);
						chara.femaleCustom.ChangeEyeLColor();
						chara.femaleCustom.ChangeEyeRColor();
					});
					Studio.Studio.Instance.colorPaletteCtrl.visible = true;
				}
			}
			protected override void ButtonSpecular()
			{
				if (GUILayout.Button("Specular Color")) {
					Studio.Studio.Instance.colorMenu.SetColor(color.rgbSpecular, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
						chara.femaleCustomInfo.eyeLColor.SetSpecularRGB(c);
						chara.femaleCustomInfo.eyeRColor.SetSpecularRGB(c);
						chara.femaleCustom.ChangeEyeLColor();
						chara.femaleCustom.ChangeEyeRColor();
					});
					Studio.Studio.Instance.colorPaletteCtrl.visible = true;
				}
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matEyeLId = partID;
				chara.femaleCustomInfo.matEyeRId = partID;
				UpdatePart();
			}
		}
		
		public class Eyebrows : CharaParts
		{
			public Eyebrows()
			{
				partName = "Eyebrows";
				color = chara.femaleCustomInfo.eyebrowColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyebrow).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyebrowColor);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyebrowColor, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeEyebrow();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matEyebrowId = partID;
				UpdatePart();
			}
		}
		
		public class EyeHighlights : CharaParts
		{
			public EyeHighlights()
			{
				partName = "Eyes Highlights";
				color = chara.femaleCustomInfo.eyeHiColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyehi).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyeHiColor);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyeHiColor, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeEyeHi();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matEyeHiId = partID;
				UpdatePart();
			}
		}
		public class Eyelashes : CharaParts
		{
			public Eyelashes()
			{
				partName = "Eyelashes";
				color = chara.femaleCustomInfo.eyelashesColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyelashes).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyelashesColor);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeEyelashesColor, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeEyelashes();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matEyelashesId = partID;
				UpdatePart();
			}
		}
		
		public class Nipples : CharaParts
		{
			public Nipples()
			{
				partName = "Nipples";
				color = chara.femaleCustomInfo.nipColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_nip).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeNipColor);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeNipColor, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeNip();
			}
			protected override void AdditionalGUI()
			{
				GUILayout.Label("Areola Size: ");
				chara.femaleCustomInfo.areolaSize = Extensions.AbsSlider(chara.femaleCustomInfo.areolaSize);
				chara.femaleBody.areola.Blend(chara.femaleCustomInfo.areolaSize);
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matNipId = partID;
				UpdatePart();
			}
		}
		
		public class Underhair : CharaParts
		{
			public Underhair()
			{
				partName = "Underhair";
				color = chara.femaleCustomInfo.underhairColor;
				listAllParts = chara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_underhair).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeUnderHairColor);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, chara.femaleCustom.ChangeUnderHairColor, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeUnderHair();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.matUnderhairId = partID;
				UpdatePart();
			}
		}
		
		public class HeadMesh : CharaParts
		{
			public HeadMesh()
			{
				partName = "Head Mesh";
				hasSlider = false;
				color = null;
				listAllParts = chara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_head).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
			}
			protected override void ButtonSpecular()
			{
			}
			public override void UpdatePart()
			{
			}
			
			protected override void ListButtonLogic(int partID)
			{
				var mouth = chara.femaleBody.mouthCtrl.FixedRate;
				var eyes = chara.femaleBody.eyesCtrl.OpenMax;
				chara.femaleBody.ChangeHead(partID, true);
				chara.femaleBody.mouthCtrl.FixedRate = mouth;
				chara.femaleBody.eyesCtrl.OpenMax = eyes;
			}
		}
		
		public class HairBack : CharaParts
		{
			public HairBack()
			{
				partName = "Hair Back";
				color = chara.femaleCustomInfo.hairColor[0];
				listAllParts = chara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairB).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, () => chara.femaleCustom.ChangeHairColor(0));
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, () => chara.femaleCustom.ChangeHairColor(0), false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeHairColor(0);
			}
			protected override void AdditionalGUI()
			{
				if (GUILayout.Button("Apply color to all hair")) {
					for (int i = 0; i < chara.femaleCustomInfo.hairColor.Length; i++) { 
						chara.femaleCustomInfo.hairColor[i].SetDiffuseRGBA(color.rgbaDiffuse);
						chara.femaleCustomInfo.hairColor[i].SetSpecularRGB(color.rgbSpecular);
						chara.femaleCustomInfo.hairColor[i].specularIntensity = color.specularIntensity;
						chara.femaleCustomInfo.hairColor[i].specularSharpness = color.specularSharpness;
						chara.femaleCustom.ChangeHairColor(i);
					}
				}
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleBody.ChangeHair(partID, -1, -1, -1, true);
			}
		}
		
		public class HairFront : CharaParts
		{
			public HairFront()
			{
				partName = "Hair Front";
				color = chara.femaleCustomInfo.hairColor[1];
				listAllParts = chara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairF).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, () => chara.femaleCustom.ChangeHairColor(1));
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, () => chara.femaleCustom.ChangeHairColor(1), false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeHairColor(1);
			}
			protected override void AdditionalGUI()
			{
				if (GUILayout.Button("Apply color to all hair")) {
					for (int i = 0; i < chara.femaleCustomInfo.hairColor.Length; i++) { 
						chara.femaleCustomInfo.hairColor[i].SetDiffuseRGBA(color.rgbaDiffuse);
						chara.femaleCustomInfo.hairColor[i].SetSpecularRGB(color.rgbSpecular);
						chara.femaleCustomInfo.hairColor[i].specularIntensity = color.specularIntensity;
						chara.femaleCustomInfo.hairColor[i].specularSharpness = color.specularSharpness;
						chara.femaleCustom.ChangeHairColor(i);
					}
				}
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleBody.ChangeHair(-1, partID, -1, -1, true);
			}
		}
		
		public class HairSide : CharaParts
		{
			public HairSide()
			{
				partName = "Hair Side";
				color = chara.femaleCustomInfo.hairColor[2];
				listAllParts = chara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairS).Values.ToArray();	
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, () => chara.femaleCustom.ChangeHairColor(2));
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, () => chara.femaleCustom.ChangeHairColor(2), false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeHairColor(2);
			}
			protected override void AdditionalGUI()
			{
				if (GUILayout.Button("Apply color to all hair")) {
					for (int i = 0; i < chara.femaleCustomInfo.hairColor.Length; i++) { 
						chara.femaleCustomInfo.hairColor[i].SetDiffuseRGBA(color.rgbaDiffuse);
						chara.femaleCustomInfo.hairColor[i].SetSpecularRGB(color.rgbSpecular);
						chara.femaleCustomInfo.hairColor[i].specularIntensity = color.specularIntensity;
						chara.femaleCustomInfo.hairColor[i].specularSharpness = color.specularSharpness;
						chara.femaleCustom.ChangeHairColor(i);
					}
				}
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleBody.ChangeHair(-1, -1, partID, -1, true);
			}
		}
		
		public class Face : CharaParts
		{
			public Face()
			{
				partName = "Face Texture";
				hasSlider = false;
				color = null;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_face).Values.ToArray();
			}

			protected override void ButtonDiffuse()
			{
			}

			protected override void ButtonSpecular()
			{
			}

			public override void UpdatePart()
			{
				chara.UpdateFace();
			}

			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texFaceId = partID;
				UpdatePart();
			}
		}
		public class DetailFace : CharaParts
		{
			public DetailFace()
			{
				partName = "Face Detail";
				hasSlider = false;
				color = null;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_detail_f).Values.ToArray();
			}

			protected override void ButtonDiffuse()
			{
			}

			protected override void ButtonSpecular()
			{
			}

			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeFaceDetailTex();
			}
			
			protected override void AdditionalGUI()
			{
				chara.femaleCustomInfo.faceDetailWeight = Extensions.AbsSlider(chara.femaleCustomInfo.faceDetailWeight);
				chara.femaleCustom.ChangeFaceDetailWeight();
			}

			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texFaceDetailId = partID;
				UpdatePart();
			}
		}
		public class DetailBody : CharaParts
		{
			public DetailBody()
			{
				partName = "Body Detail";
				hasSlider = false;
				color = null;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_detail_b).Values.ToArray();
			}

			protected override void ButtonDiffuse()
			{
			}

			protected override void ButtonSpecular()
			{
			}

			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeBodyDetailTex();
			}
			
			protected override void AdditionalGUI()
			{
				chara.femaleCustomInfo.bodyDetailWeight = Extensions.AbsSlider(chara.femaleCustomInfo.bodyDetailWeight);
				chara.femaleCustom.ChangeBodyDetailWeight();
			}

			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texBodyDetailId = partID;
				UpdatePart();
			}
		}
		public class Lips : CharaParts
		{
			public Lips()
			{
				partName = "Lips";
				hasSlider = false;
				color = chara.femaleCustomInfo.lipColor;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_lip).Values.ToArray();
			}

			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, UpdatePart);
			}

			protected override void ButtonSpecular()
			{
			}

			public override void UpdatePart()
			{
				chara.UpdateFace();
			}

			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texLipId = partID;
				UpdatePart();
			}
		}
		
		public class Nails : CharaParts
		{
			public Nails()
			{
				partName = "Nails";
				hasPartList = false;
				color = chara.femaleCustomInfo.nailColor;
				listAllParts = new ListInfoBase[0];
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, UpdatePart);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, UpdatePart, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeNailColor();
			}
			protected override void ListButtonLogic(int partID)
			{
			}
		}
		
		public class EyeWhites : CharaParts
		{
			public EyeWhites()
			{
				partName = "Eye Whites";
				hasPartList = false;
				color = chara.femaleCustomInfo.eyeWColor;
				listAllParts = new ListInfoBase[0];
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, UpdatePart);
			}
			
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, UpdatePart, false);
			}
			public override void UpdatePart()
			{
				chara.femaleCustom.ChangeEyeWColor();
			}
			protected override void ListButtonLogic(int partID)
			{
			}
		}
		
		public class Mole : CharaParts
		{
			public Mole()
			{
				partName = "Mole";
				hasSlider = false;
				color = chara.femaleCustomInfo.moleColor;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_mole).Values.ToArray();
			}
			
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, UpdatePart);
			}
			
			protected override void ButtonSpecular()
			{
			}
			
			public override void UpdatePart()
			{
				chara.UpdateFace();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texMoleId = partID;
				UpdatePart();
			}
		}
		
		public class Sunburn : CharaParts
		{
			public Sunburn()
			{
				partName = "Sunburn";
				color = chara.femaleCustomInfo.sunburnColor;
				hasSlider = false;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_sunburn).Values.ToArray();
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, UpdatePart);
			}
			
			protected override void ButtonSpecular()
			{
			}
			
			public override void UpdatePart()
			{
				chara.UpdateBody(false, false, true);
			}
			
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texSunburnId = partID;
				UpdatePart();
			}
		}
		
		public class Cheeks : CharaParts
		{
			public Cheeks()
			{
				partName = "Cheeks";
				hasSlider = false;
				color = chara.femaleCustomInfo.cheekColor;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_cheek).Values.ToArray();
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, UpdatePart);
			}
			
			protected override void ButtonSpecular()
			{
			}
			
			public override void UpdatePart()
			{
				chara.UpdateFace();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texCheekId = partID;
				UpdatePart();
			}
		}
		
		public class Skin : CharaParts
		{
			public Skin()
			{
				partName = "Skin";
				color = chara.femaleCustomInfo.skinColor;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_body).Values.ToArray();
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, UpdatePart);
			}
			protected override void ButtonSpecular()
			{
				Extensions.ButtonTypical(color, UpdatePart, false);
			}
			public override void UpdatePart()
			{
				chara.UpdateBody(true, false, false);
				chara.UpdateFace();
			}
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texBodyId = partID;
				UpdatePart();				
			}
		}
		
		public class Eyeshadow : CharaParts
		{
			public Eyeshadow()
			{
				partName = "Eyeshadow";
				color = chara.femaleCustomInfo.eyeshadowColor;
				hasSlider = false;
				listAllParts = chara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_eyeshadow).Values.ToArray();
			}
			protected override void ButtonDiffuse()
			{
				Extensions.ButtonTypical(color, chara.UpdateFace);
			}
			
			protected override void ButtonSpecular()
			{
			}
			
			public override void UpdatePart()
			{
			}
			
			protected override void ListButtonLogic(int partID)
			{
				chara.femaleCustomInfo.texEyeshadowId = partID;
				chara.UpdateFace();
			}
		}
		
		public class HairAcs : CharaParts
		{
			public HairAcs()
			{
				partName = "Hair Accessory";
				hasPartList = false;
				color = chara.femaleCustomInfo.hairAcsColor[0];
				listAllParts = new ListInfoBase[0];
			}

			protected override void ButtonDiffuse()
			{
				if (GUILayout.Button("Diffuse Color")) {
					Studio.Studio.Instance.colorMenu.SetColor(color.rgbaDiffuse, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
						for (int i = 0; i < chara.femaleCustomInfo.hairAcsColor.Length; i++) {
							chara.femaleCustomInfo.hairAcsColor[i].SetDiffuseRGBA(c);
						}
						UpdatePart();
					});
					Studio.Studio.Instance.colorPaletteCtrl.visible = true;
				}
			}

			protected override void ButtonSpecular()
			{
				if (GUILayout.Button("Specular Color")) {
					Studio.Studio.Instance.colorMenu.SetColor(color.rgbSpecular, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
						for (int i = 0; i < chara.femaleCustomInfo.hairAcsColor.Length; i++) {
							chara.femaleCustomInfo.hairAcsColor[i].SetSpecularRGB(c);
						}
						UpdatePart();
					});
					Studio.Studio.Instance.colorPaletteCtrl.visible = true;
				}	
			}

			public override void UpdatePart()
			{
				for (int i = 0; i < chara.femaleCustomInfo.hairAcsColor.Length; i++)
					chara.femaleCustom.ChangeHairAcsColor(i);
			}

			protected override void ListButtonLogic(int partID)
			{
			}
		}
	}
}