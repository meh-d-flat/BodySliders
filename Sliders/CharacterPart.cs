using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace BodySliders
{
	/// I found this solution to be a better approach to template pattern in my case
	/// Because i can just add a reference to some outside method, instead of creating a new one inside of an object 
	public abstract class NuTemplate
	{
		Action actionChain;
		
		public void AddStep(Action newStep)
		{
			actionChain += new Action(newStep);
		}
		
		public void ActTheChain()
		{
			if (actionChain != null)
				actionChain();
		}
		
		public void ClearTheChain()
		{
			actionChain = null;
		}
		
		public Delegate[] GetActionListCopy()
		{
			Delegate[] list = new Delegate[actionChain.GetInvocationList().Length];
			list = actionChain.GetInvocationList();
			return list;		
		}
	}
	
	public class CharacterPart : NuTemplate
	{
		public CharacterPart()
		{
			Init();
		}
		
		public static CharFemale referenceChara;
		public string partName = "No Part";
		protected HSColorSet referenceColor = null;
		protected ListInfoBase[] listAllParts = new ListInfoBase[0];
		protected Vector2 listScrollView;
		
		public virtual void Init()
		{
		}
		
		~CharacterPart()
		{
			Resources.UnloadUnusedAssets();
		}
	}
	
	public class AllParts
	{
		public static readonly CharacterPart nullPart = new CharacterPart();
		
		static Dictionary<string, Type> allSliders = new Dictionary<string, Type>() {
			{ "Body Sliders", typeof(AllParts.GroupBody) },
			{ "Tits Sliders", typeof(AllParts.GroupTits) },
			{ "Face Sliders", typeof(AllParts.GroupFace) },
			{ "Jaw and Chin Sliders", typeof(AllParts.GroupJawChin) },
			{ "Cheeks Sliders", typeof(AllParts.GroupCheeks) },
			{ "Eyebrows Sliders", typeof(AllParts.GroupEyebrows) },
			{ "Eyes Sliders", typeof(AllParts.GroupEyes) },
			{ "Nose Sliders", typeof(AllParts.GroupNose) },
			{ "Mouth Sliders", typeof(AllParts.GroupMouth) },
			{ "Ears Sliders", typeof(AllParts.GroupEars) }
		};
		
		static Dictionary<string, Type> allParts = new Dictionary<string, Type>() {
			{ "Skin", typeof(AllParts.Skin) },
			{ "Body Detail", typeof(AllParts.DetailBody) },
			{ "Sunburn", typeof(AllParts.Sunburn) },
			{ "Head Mesh", typeof(AllParts.HeadMesh) },
			{ "Face Texture", typeof(AllParts.Face) },
			{ "Face Detail", typeof(AllParts.DetailFace) },
			{ "Nipples", typeof(AllParts.Nipples) },
			{ "Pubic Hair", typeof(AllParts.PubicHair) },
			{ "Nails", typeof(AllParts.Nails) },
			{ "Eyebrows", typeof(AllParts.Eyebrows) },
			{ "Eyelashes", typeof(AllParts.Eyelashes) },
			{ "Eye whites", typeof(AllParts.EyeWhites) },
			{ "Eye Left", typeof(AllParts.EyeLeft) },
			{ "Eye Right", typeof(AllParts.EyeRight) },
			{ "Both Eyes", typeof(AllParts.BothEyes) },
			{ "Eyes Highlights", typeof(AllParts.EyesHighlights) },
			{ "Eyeshadow", typeof(AllParts.Eyeshadow) },
			{ "Cheeks", typeof(AllParts.Cheeks) },
			{ "Lips", typeof(AllParts.Lips) },
			{ "Mole", typeof(AllParts.Mole) },
			{ "Hair Front", typeof(AllParts.HairFront) },
			{ "Hair Back", typeof(AllParts.HairBack) },
			{ "Hair Side", typeof(AllParts.HairSide) },
			{ "Hair Accessory", typeof(AllParts.HairAcs) },
			{ "Load / Fuse", typeof(LoadAndFuse)}
		};
		
		static Dictionary<string, Type> allOptions = new Dictionary<string, Type>() {
			{ "Delete selected char file", typeof(DeleteChara)}
		};
		
		public static CharacterPart GetSlider(CharacterPart prt)
		{
			foreach (var entry in allSliders)
			{
				if (GUILayout.Button(entry.Key))
				{
					ReSet();
					prt = (CharacterPart)Activator.CreateInstance(entry.Value);
				}
			}
			return prt;
		}
		
		public static CharacterPart GetPart(CharacterPart prt)
		{
			foreach (var entry in allParts)
			{
				if (GUILayout.Button(entry.Key))
				{
					ReSet();
					prt = (CharacterPart)Activator.CreateInstance(entry.Value);
				}
			}
			return prt;
		}
		
		public static CharacterPart GetOptions(CharacterPart prt)
		{
			foreach (var entry in allOptions)
			{
				if (GUILayout.Button(entry.Key))
				{
					ReSet();
					prt = (CharacterPart)Activator.CreateInstance(entry.Value);
				}
			}
			return prt;
		}
		
		static void ReSet()
		{
			Studio.Studio.Instance.colorPaletteCtrl.visible = false;
			Studio.Studio.Instance.colorMenu.updateColorFunc = null;
		}
		
		class GroupBody : CharacterPart
		{
			public override void Init()
			{
				partName = "Body Sliders";
				AddStep(AdditionalGUI);
			}
			
			void AdditionalGUI()
			{
				Functionality.BodySlider(referenceChara, SliderNames.body, 0);
				for (int i = 9; i < 32; i++)
					Functionality.BodySlider(referenceChara, SliderNames.body, i);
			}
		}
		
		class GroupTits : CharacterPart
		{
			public override void Init()
			{
				partName = "Tits Sliders";
				AddStep(AdditionalGUI);
			}
			
			void AdditionalGUI()
			{
				for (int i = 1; i < 9; i++)
					Functionality.BodySlider(referenceChara, SliderNames.body, i);
				float newTitWeight = Functionality.AbsSlider(referenceChara.femaleCustomInfo.bustWeight, "Tits Weight");
				float newTitSoft = Functionality.AbsSlider(referenceChara.femaleCustomInfo.bustSoftness, "Tits Soft");
				if (Math.Abs(newTitWeight - referenceChara.femaleCustomInfo.bustWeight) > 0.01F || Math.Abs(newTitSoft - referenceChara.femaleCustomInfo.bustSoftness) > 0.01F) {
					referenceChara.ChangeBustGravity(newTitWeight);
					referenceChara.ChangeBustSoftness(newTitSoft);
				}
				referenceChara.ReSetupDynamicBone();
				referenceChara.UpdateBustSoftnessAndGravity();
				referenceChara.femaleBody.updateBustSize = true;
			}
		}
		
		class GroupFace : CharacterPart
		{
			public override void Init()
			{
				partName = "Face Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 0; i < 5; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class GroupJawChin : CharacterPart
		{
			public override void Init()
			{
				partName = "Jaw and Chin Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 5; i < 13; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class GroupCheeks : CharacterPart
		{
			public override void Init()
			{
				partName = "Cheeks Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 13; i < 19; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class GroupEyebrows : CharacterPart
		{
			public override void Init()
			{
				partName = "Eyes Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 19; i < 24; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class GroupEyes : CharacterPart
		{
			public override void Init()
			{
				partName = "Eyes Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 24; i < 40; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class GroupNose : CharacterPart
		{
			public override void Init()
			{
				partName = "Nose Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 40; i < 55; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class GroupMouth : CharacterPart
		{
			public override void Init()
			{
				partName = "Mouth Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 55; i < 62; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class GroupEars : CharacterPart
		{
			public override void Init()
			{
				partName = "Ears Sliders";
				AddStep(AdditionalGUI);
			}
			void AdditionalGUI()
			{
				for (int i = 62; i < 67; i++)
					Functionality.FaceSlider(referenceChara, SliderNames.face, i);
			}
		}
		
		class Eyebrows : CharacterPart
		{
			public override void Init()
			{
				partName = "Eyebrows";
				referenceColor = referenceChara.femaleCustomInfo.eyebrowColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyebrow).Values.ToArray();
			
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeEyebrowColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matEyebrowId, ref listScrollView, listAllParts, referenceChara.femaleCustom.ChangeEyebrow));		
			}
		}
		
		class BothEyes : CharacterPart
		{
			public override void Init()
			{
				partName = "Both Eyes";
				referenceColor = referenceChara.femaleCustomInfo.eyeRColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyeball).Values.ToArray();
				
				AddStep(Diffuse);
				AddStep(Specular);
				AddStep(() => Functionality.NewCSlider(referenceColor, ColorChanger));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matEyeRId, ref listScrollView, listAllParts, () => {
					referenceChara.femaleCustomInfo.matEyeLId = referenceChara.femaleCustomInfo.matEyeRId;
					referenceChara.femaleCustom.ChangeEyeR();
					referenceChara.femaleCustom.ChangeEyeL();
				}));
			}
			
			void ColorChanger()
			{
				referenceChara.femaleCustomInfo.eyeLColor = referenceChara.femaleCustomInfo.eyeRColor;
				referenceChara.femaleCustom.ChangeEyeLColor();
				referenceChara.femaleCustom.ChangeEyeRColor();
			}
			
			void Diffuse()
			{
				if (GUILayout.Button("Diffuse Color")) {
					Studio.Studio.Instance.colorPaletteCtrl.visible = false;
					Studio.Studio.Instance.colorMenu.SetColor(referenceColor.rgbaDiffuse, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
					    referenceChara.femaleCustomInfo.eyeRColor.SetDiffuseRGBA(c);
					    ColorChanger();
					});
					Studio.Studio.Instance.colorPaletteCtrl.visible = true;
				}
			}
			void Specular()
			{
				if (GUILayout.Button("Specular Color")) {
					Studio.Studio.Instance.colorPaletteCtrl.visible = false;
					Studio.Studio.Instance.colorMenu.SetColor(referenceColor.rgbSpecular, UI_ColorInfo.ControlType.PresetsSample);
					Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
					    referenceChara.femaleCustomInfo.eyeRColor.SetSpecularRGB(c);
					    ColorChanger();
					});
					Studio.Studio.Instance.colorPaletteCtrl.visible = true;
				}
			}
		}
		
		class EyeRight : CharacterPart
		{
			public override void Init()
			{
				partName = "Eye Right";
				referenceColor = referenceChara.femaleCustomInfo.eyeRColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyeball).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeEyeRColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matEyeRId, ref listScrollView, listAllParts, referenceChara.femaleCustom.ChangeEyeR));
			}
		}
		
		class EyeLeft : CharacterPart
		{
			public override void Init()
			{
				partName = "Eye Left";
				referenceColor = referenceChara.femaleCustomInfo.eyeLColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyeball).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeEyeLColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matEyeLId, ref listScrollView, listAllParts, referenceChara.femaleCustom.ChangeEyeL));
			}
		}
		
		class EyesHighlights : CharacterPart
		{
			public override void Init()
			{
				partName = "Eyes Highlights";
				referenceColor = referenceChara.femaleCustomInfo.eyeHiColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyehi).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeEyeHiColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matEyeHiId, ref listScrollView, listAllParts, referenceChara.femaleCustom.ChangeEyeHi));
			}
		}
		
		class Eyelashes : CharacterPart
		{
			public override void Init()
			{
				partName = "Eyelashes";
				referenceColor = referenceChara.femaleCustomInfo.eyelashesColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_eyelashes).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeEyelashesColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matEyelashesId, ref listScrollView, listAllParts, referenceChara.femaleCustom.ChangeEyelashes));
			}
		}
		
		class Nipples : CharacterPart
		{
			public override void Init()
			{
				partName = "Nipples";
				referenceColor = referenceChara.femaleCustomInfo.nipColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_nip).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeNipColor));
				AddStep(AdditionalGUI);
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matNipId, ref listScrollView, listAllParts, referenceChara.femaleCustom.ChangeNip));
			}
			void AdditionalGUI()
			{
				GUILayout.Label("Areola size: ");
				referenceChara.femaleCustomInfo.areolaSize = Functionality.AbsSlider(referenceChara.femaleCustomInfo.areolaSize);
				referenceChara.femaleBody.areola.Blend(referenceChara.femaleCustomInfo.areolaSize);
			}
		}
		
		class PubicHair : CharacterPart
		{
			public override void Init()
			{
				partName = "Pubic Hair";
				referenceColor = referenceChara.femaleCustomInfo.underhairColor;
				listAllParts = referenceChara.ListInfo.GetFemaleMaterialList(CharaListInfo.TypeFemaleMaterial.cf_m_underhair).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeUnderHairColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.matUnderhairId, ref listScrollView, listAllParts, referenceChara.femaleCustom.ChangeUnderHair));
			}
		}
		
		class Nails : CharacterPart
		{
			public override void Init()
			{
				partName = "Nails";
				referenceColor = referenceChara.femaleCustomInfo.nailColor;
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeNailColor));
			}
		}
		
		class EyeWhites : CharacterPart
		{
			public override void Init()
			{
				partName = "Eye Whites";
				referenceColor = referenceChara.femaleCustomInfo.eyeWColor;
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, referenceChara.femaleCustom.ChangeEyeWColor));
			}
		}
		
		class HeadMesh : CharacterPart
		{
			public override void Init()
			{
				partName = "Head Mesh";
				listAllParts = referenceChara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_head).Values.ToArray();
				
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.headId, ref listScrollView, listAllParts, referenceChara.femaleBody.ChangeHeadNew));
			}
		}
		
		class HairFront : CharacterPart
		{
			public override void Init()
			{
				partName = "Hair Front";
				referenceColor = referenceChara.femaleCustomInfo.hairColor[1];
				listAllParts = referenceChara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairF).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, () => referenceChara.femaleCustom.ChangeHairColor(1)));
				AddStep(() => Functionality.SetAllHair(referenceChara, referenceColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.hairId[1], ref listScrollView, listAllParts, () => referenceChara.femaleBody.ChangeHair(true)));
			}
		}
		
		class HairBack : CharacterPart
		{
			public override void Init()
			{
				partName = "Hair Back";
				referenceColor = referenceChara.femaleCustomInfo.hairColor[0];
				listAllParts = referenceChara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairB).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, () => referenceChara.femaleCustom.ChangeHairColor(0)));
				AddStep(() => Functionality.SetAllHair(referenceChara, referenceColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.hairId[0], ref listScrollView, listAllParts, () => referenceChara.femaleBody.ChangeHair(true)));
			}
		}
		
		class HairSide : CharacterPart
		{
			public override void Init()
			{
				partName = "Hair Side";
				referenceColor = referenceChara.femaleCustomInfo.hairColor[2];
				listAllParts = referenceChara.ListInfo.GetFemaleFbxList(CharaListInfo.TypeFemaleFbx.cf_f_hairS).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, () => referenceChara.femaleCustom.ChangeHairColor(2)));
				AddStep(() => Functionality.SetAllHair(referenceChara, referenceColor));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.hairId[2], ref listScrollView, listAllParts, () => referenceChara.femaleBody.ChangeHair(true)));
			}
		}
		
		class HairAcs : CharacterPart
		{
			public override void Init()
			{
				partName = "Hair Accessory";
				referenceColor = referenceChara.femaleCustomInfo.hairAcsColor[0];
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, UpdatePart));
			}
			
			void UpdatePart()
			{
				for (int i = 0; i < referenceChara.femaleCustomInfo.hairAcsColor.Length; i++)
					referenceChara.femaleCustom.ChangeHairAcsColor(i);
			}
		}
		
		class Face : CharacterPart
		{
			public override void Init()
			{
				partName = "Face Texture";
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_face).Values.ToArray();
				
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texFaceId, ref listScrollView, listAllParts, referenceChara.femaleBody.ChangeHeadNew));
			}
		}
		
		class DetailFace : CharacterPart
		{
			public override void Init()
			{
				partName = "Face Detail";
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_detail_f).Values.ToArray();
				
				AddStep(AdditionalGUI);
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texFaceDetailId, ref listScrollView, listAllParts, () => referenceChara.femaleCustom.ChangeFaceDetailTex()));
			}
			
			void AdditionalGUI()
			{
				GUILayout.Label("Texture power: ");
				referenceChara.femaleCustomInfo.faceDetailWeight = Functionality.AbsSlider(referenceChara.femaleCustomInfo.faceDetailWeight);
				referenceChara.femaleCustom.ChangeFaceDetailWeight();
			}
		}
		
		class DetailBody : CharacterPart
		{
			public override void Init()
			{
				partName = "Body Detail";
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_detail_b).Values.ToArray();
				
				AddStep(AdditionalGUI);
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texBodyDetailId, ref listScrollView, listAllParts, () => referenceChara.femaleCustom.ChangeBodyDetailTex()));
			}
			
			void AdditionalGUI()
			{
				GUILayout.Label("Texture power: ");
				referenceChara.femaleCustomInfo.bodyDetailWeight = Functionality.AbsSlider(referenceChara.femaleCustomInfo.bodyDetailWeight);
				referenceChara.femaleCustom.ChangeBodyDetailWeight();
			}
		}
		
		class Skin : CharacterPart
		{
			public override void Init()
			{
				partName = "Skin";
				referenceColor = referenceChara.femaleCustomInfo.skinColor;
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_body).Values.ToArray();
				
				AddStep(() => Functionality.ColorControlGroup(referenceColor, ColorChanger));
				AddStep(AdditionalGUI);
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texBodyId, ref listScrollView, listAllParts, ColorChanger));
			}
			
			void ColorChanger()
			{
				Functionality.UpdateBody(referenceChara, true, true, false);
				referenceChara.UpdateFace();
			}
			
			void AdditionalGUI()
			{
				if (GUILayout.Button("Specular to nips and pubes"))
				{
					referenceChara.femaleCustomInfo.nipColor.SetSpecularRGB(referenceColor.rgbSpecular);
					referenceChara.femaleCustomInfo.nipColor.specularIntensity = referenceColor.specularIntensity;
					referenceChara.femaleCustomInfo.nipColor.specularSharpness = referenceColor.specularSharpness;
					referenceChara.femaleCustomInfo.underhairColor.SetSpecularRGB(referenceColor.rgbSpecular);
					referenceChara.femaleCustomInfo.underhairColor.specularIntensity = referenceColor.specularIntensity;
					referenceChara.femaleCustomInfo.underhairColor.specularSharpness = referenceColor.specularSharpness;
					referenceChara.femaleCustom.ChangeUnderHairColor();
					referenceChara.femaleCustom.ChangeNipColor();
				}
			}
		}
		
		class Lips : CharacterPart
		{
			public override void Init()
			{
				partName = "Lips";
				referenceColor = referenceChara.femaleCustomInfo.lipColor;
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_lip).Values.ToArray();
				
				AddStep(() => Functionality.ButtonTypical(referenceColor, new Action(() => referenceChara.UpdateFace())));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texLipId, ref listScrollView, listAllParts, referenceChara.femaleBody.ChangeHeadNew));
			}
		}
		
		class Mole : CharacterPart
		{
			public override void Init()
			{
				partName = "Mole";
				referenceColor = referenceChara.femaleCustomInfo.moleColor;
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_mole).Values.ToArray();
				
				AddStep(() => Functionality.ButtonTypical(referenceColor, new Action(() => referenceChara.UpdateFace())));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texMoleId, ref listScrollView, listAllParts, referenceChara.femaleBody.ChangeHeadNew));
			}
		}
		
		class Sunburn : CharacterPart
		{
			public override void Init()
			{
				partName = "Sunburn";
				referenceColor = referenceChara.femaleCustomInfo.sunburnColor;
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_sunburn).Values.ToArray();
				
				AddStep(() => Functionality.ButtonTypical(referenceColor, new Action(() => Functionality.UpdateBody(referenceChara, false, true, true))));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texSunburnId, ref listScrollView, listAllParts, () => Functionality.UpdateBody(referenceChara, false, true, true)));
			}
		}
		
		class Cheeks : CharacterPart
		{
			public override void Init()
			{
				partName = "Cheeks";
				referenceColor = referenceChara.femaleCustomInfo.cheekColor;
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_cheek).Values.ToArray();
				
				AddStep(() => Functionality.ButtonTypical(referenceColor, new Action(() => referenceChara.UpdateFace())));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texCheekId, ref listScrollView, listAllParts, referenceChara.femaleBody.ChangeHeadNew));
			}
		}
		
		class Eyeshadow : CharacterPart
		{
			public override void Init()
			{
				partName = "Eyeshadow";
				referenceColor = referenceChara.femaleCustomInfo.eyeshadowColor;
				listAllParts = referenceChara.ListInfo.GetFemaleTextureList(CharaListInfo.TypeFemaleTexture.cf_t_eyeshadow).Values.ToArray();
				
				AddStep(() => Functionality.ButtonTypical(referenceColor, new Action(() => referenceChara.UpdateFace())));
				AddStep(() => Functionality.GUIListDraw(ref referenceChara.femaleCustomInfo.texEyeshadowId, ref listScrollView, listAllParts, referenceChara.femaleBody.ChangeHeadNew));
			}
		}
		
		class PresetsTab : CharacterPart
		{
			public override void Init()
			{
				partName = "Load and Fuse";

			}
			
		}
	}
	
	public static class Functionality
	{
		static float wsMin = (float)LimitConfig.NewMin, wsMax = (float)LimitConfig.NewMax;
		
		public static void ChangeHeadNew(this CharFemaleBody fBody)
		{
			var mouth = fBody.mouthCtrl.FixedRate;
			var eyes = fBody.eyesCtrl.OpenMax;
			fBody.ChangeHead(true);
			fBody.mouthCtrl.FixedRate = mouth;
			fBody.eyesCtrl.OpenMax = eyes;
		}
		
		public static void ColorControlGroup(HSColorSet color, Action colorSetter, bool drawSpecularButton = true, bool drawSlider = true)
		{
			GUILayout.BeginHorizontal();
			ButtonTypical(color, new Action(colorSetter));
			RandomColor(color, new Action(colorSetter));
			GUILayout.EndHorizontal();
			
			if (drawSpecularButton)
				ButtonTypical(color, new Action(colorSetter), false);
			if (drawSlider)
				NewCSlider(color, new Action(colorSetter));
		}
		
		public static float AbsSlider(float value, string labelName = null)
		{
			value = value * 100;
			GUILayout.BeginHorizontal();
			if (labelName != null)
				GUILayout.Label(labelName, GUILayout.Width(70f));
			value = GUILayout.HorizontalSlider(value, wsMin, wsMax);
			value = Single.Parse(GUILayout.TextField(value.ToString("0"), GUILayout.Width(30f)));
			GUILayout.EndHorizontal();
			value = Mathf.Clamp(value, wsMin, wsMax);
			return value / 100;
		}
		
		public static void FaceSlider(CharFemale chara, string[] partNames, int partIndex)
		{
			float newValue = AbsSlider(chara.customInfo.shapeValueFace[partIndex], partNames[partIndex]);
			if (Math.Abs(newValue - chara.customInfo.shapeValueFace[partIndex]) > 0.01F)
				chara.chaCustom.SetShapeFaceValue(partIndex, newValue);
		}
		
		//TODO: figure out a way to do height while char's FK/IK is influenced by StudioNEOPlugin
		public static void BodySlider(CharFemale chara, string[] partNames, int partIndex)
		{
			float newValue = AbsSlider(chara.customInfo.shapeValueBody[partIndex], partNames[partIndex]);
			if (Math.Abs(newValue - chara.customInfo.shapeValueBody[partIndex]) > 0.01F)
			{
				chara.chaCustom.SetShapeBodyValue(partIndex, newValue);
				chara.femaleCustom.UpdateShapeBodyValueFromCustomInfo();
				chara.femaleCustom.UpdateShapeBody();
			}
		}
		
		public static void SetAllHair(CharFemale chara, HSColorSet color)
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
		
		public static void ButtonTypical(HSColorSet color, Action changer, bool isDiffuse = true)
		{
			string name = isDiffuse ? "Diffuse Color" : "Specular Color";
			if (GUILayout.Button(name)) {
				Studio.Studio.Instance.colorPaletteCtrl.visible = false;
				Studio.Studio.Instance.colorMenu.SetColor((isDiffuse) ? color.rgbaDiffuse : color.rgbSpecular, UI_ColorInfo.ControlType.PresetsSample);
				Studio.Studio.Instance.colorMenu.updateColorFunc = new UI_ColorInfo.UpdateColor(c => {
					if (isDiffuse)
						color.SetDiffuseRGBA(c);
					else
						color.SetSpecularRGB(c);
					changer();
				});
				Studio.Studio.Instance.colorPaletteCtrl.visible = true;
			}
		}
		
		public static void NewCSlider(HSColorSet colorSet, Action setter)
		{
			float newIntensity = AbsSlider(colorSet.specularIntensity);
			if (Math.Abs(colorSet.specularIntensity - newIntensity) > 0.01F) {
				colorSet.specularIntensity = newIntensity;
				setter();
			}
			float newSharpness = AbsSlider(colorSet.specularSharpness);
			if (Math.Abs(colorSet.specularSharpness - newSharpness) > 0.01F) {
				colorSet.specularSharpness = newSharpness;
				setter();
			}
		}
		
		public static void GUIListDraw(ref int currentPartID, ref Vector2 position, ListInfoBase[] list, Action onChange)
		{
			var style = new GUIStyle(GUI.skin.button);
			style.normal.textColor = Color.green;
			position = GUILayout.BeginScrollView(position);
			for (int i = 0; i < list.Length; i++)
			{
				var buttonStyle = (list[i].Id != currentPartID) ? new GUIStyle(GUI.skin.button) : style;
				if (GUILayout.Button("" + list[i].Name, buttonStyle))
				{
					if (currentPartID != list[i].Id)
					{
						currentPartID = list[i].Id;
						onChange();
					}
				}
			}
			if (GUILayout.Button("RANDOM"))
			{
				currentPartID = list.RandomMember().Id;
				onChange();
			}
			GUILayout.EndScrollView();
		}
		
		public static void RandomColor(HSColorSet hcs, Action changer)
		{
			if (GUILayout.Button("R", GUILayout.Width(SlidersUI.windowMain.width / 10 - 10)))
			{
				Color color = UnityEngine.Random.ColorHSV();
				hcs.SetDiffuseRGB(color);
				changer();
				Studio.Studio.Instance.colorMenu.SetColor(color, UI_ColorInfo.ControlType.PresetsSample);
			}
		}
		
		public static void UpdateBody(CharFemale chara, bool body, bool bodyTatoo, bool bodySunburn)
		{	
			Texture2D texture2D = null;
			Texture texture = null, texture2 = null;
			
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
			Texture texture = null, texture2 = null;
			
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
	}
	
	public static class Extension
	{
		public static T RandomMember<T>(this T[] array) {
			return array[new System.Random().Next(array.Length)];
		}
		
		public static T RandomMember<T>(this List<T> list) {
			return list[new System.Random().Next(list.Count)];
		}
	}
}
