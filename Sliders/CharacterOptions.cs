using System;
using Studio;
using UnityEngine;
using System.Reflection;

namespace BodySliders
{
	public class LoadAndFuse : CharacterPart
	{
		Transform listFemales;
		CharFileInfoCustomFemale chaFileOne, chaFileTwo;
//		string nameOne, nameTwo;
		
		public override void Init()
		{
			listFemales = Studio.Studio.Instance.gameObject.transform.Find("Canvas Main Menu").Find("01_Add/00_Female");
			
			partName = "Load / Fuse";
			
			AddStep(GUI);
		}
		
		void GUI()
		{
			GUILayout.Label("Load selected char's: ");
			if (GUILayout.Button("Hair"))
				LoadHair();
			if (GUILayout.Button("Face"))
				LoadFace();
			if (GUILayout.Button("Body"))
				LoadBody();
			
			GUILayout.Label("Chara fusion: ");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("First"))
				chaFileOne = GetCharCustom();
			if (GUILayout.Button("Second"))
				chaFileTwo = GetCharCustom();
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label(chaFileOne != null ? chaFileOne.name : null);
			GUILayout.Label(chaFileTwo != null ? chaFileTwo.name : null);
			GUILayout.EndHorizontal();
			
			if (GUILayout.Button("Fuse face"))
				FuseFace();
		}
		
		CharFileInfoCustomFemale GetCharCustom()
		{
			CharFileInfoCustomFemale charCustomFile = null;
			CharaList operatingList = listFemales.gameObject.activeInHierarchy ? listFemales.gameObject.GetComponent<CharaList>() : null;
			if (operatingList != null)
			{
				CharaFileSort charaFiles = (CharaFileSort)operatingList.GetType()
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
		
		void LoadBody()
		{
			CharFileInfoCustomFemale cfic = GetCharCustom();
			if (cfic != null)
			{
				referenceChara.femaleCustomInfo.shapeValueBody = cfic.shapeValueBody;
				referenceChara.femaleCustomInfo.areolaSize = cfic.areolaSize;
				referenceChara.femaleCustomInfo.bodyDetailWeight = cfic.bodyDetailWeight;
				referenceChara.femaleCustomInfo.bustSoftness = cfic.bustSoftness;
				referenceChara.femaleCustomInfo.bustWeight = cfic.bustWeight;
				referenceChara.femaleCustomInfo.matNipId = cfic.matNipId;
				referenceChara.femaleCustomInfo.matUnderhairId = cfic.matUnderhairId;
				referenceChara.femaleCustomInfo.nailColor = cfic.nailColor;
				referenceChara.femaleCustomInfo.nipColor = cfic.nipColor;
				referenceChara.femaleCustomInfo.skinColor = cfic.skinColor;
				referenceChara.femaleCustomInfo.sunburnColor = cfic.sunburnColor;
				referenceChara.femaleCustomInfo.tattoo_bColor = cfic.tattoo_bColor;
				referenceChara.femaleCustomInfo.texBodyDetailId = cfic.texBodyDetailId;
				referenceChara.femaleCustomInfo.texBodyId = cfic.texBodyId;
				referenceChara.femaleCustomInfo.texSunburnId = cfic.texSunburnId;
				referenceChara.femaleCustomInfo.texTattoo_bId = cfic.texTattoo_bId;
				referenceChara.femaleCustomInfo.underhairColor = cfic.underhairColor;
				
				referenceChara.Reload(true, true, true);
				referenceChara.UpdateFace();
			}
		}
		
		void LoadFace()
		{
			CharFileInfoCustomFemale cfic = GetCharCustom();
			if (cfic != null)
			{					
				referenceChara.femaleCustomInfo.cheekColor = cfic.cheekColor;
				referenceChara.femaleCustomInfo.eyebrowColor = cfic.eyebrowColor;
				referenceChara.femaleCustomInfo.eyeHiColor = cfic.eyeHiColor;
				referenceChara.femaleCustomInfo.eyelashesColor = cfic.eyelashesColor;
				referenceChara.femaleCustomInfo.eyeLColor = cfic.eyeLColor;
				referenceChara.femaleCustomInfo.eyeRColor = cfic.eyeRColor;
				referenceChara.femaleCustomInfo.eyeshadowColor = cfic.eyeshadowColor;
				referenceChara.femaleCustomInfo.eyeWColor = cfic.eyeWColor;
				referenceChara.femaleCustomInfo.faceDetailWeight = cfic.faceDetailWeight;
				referenceChara.femaleCustomInfo.headId = cfic.headId;
				referenceChara.femaleCustomInfo.lipColor = cfic.lipColor;
				referenceChara.femaleCustomInfo.matEyebrowId = cfic.matEyebrowId;
				referenceChara.femaleCustomInfo.matEyeHiId = cfic.matEyeHiId;
				referenceChara.femaleCustomInfo.matEyelashesId = cfic.matEyelashesId;
				referenceChara.femaleCustomInfo.matEyeLId = cfic.matEyeLId;
				referenceChara.femaleCustomInfo.matEyeRId = cfic.matEyeRId;
				referenceChara.femaleCustomInfo.moleColor = cfic.moleColor;
				referenceChara.femaleCustomInfo.shapeValueFace = cfic.shapeValueFace;
				referenceChara.femaleCustomInfo.tattoo_fColor = cfic.tattoo_fColor;
				referenceChara.femaleCustomInfo.texCheekId = cfic.texCheekId;
				referenceChara.femaleCustomInfo.texEyeshadowId = cfic.texEyeshadowId;
				referenceChara.femaleCustomInfo.texFaceDetailId = cfic.texFaceDetailId;
				referenceChara.femaleCustomInfo.texFaceId = cfic.texFaceId;
				referenceChara.femaleCustomInfo.texLipId = cfic.texLipId;
				referenceChara.femaleCustomInfo.texMoleId = cfic.texMoleId;
				referenceChara.femaleCustomInfo.texTattoo_fId = cfic.texTattoo_fId;
			
				referenceChara.femaleBody.ChangeHeadNew();
				referenceChara.femaleCustom.UpdateShapeFaceValueFromCustomInfo();
				referenceChara.femaleCustom.ChangeCustomFaceWithoutCustomTexture();
				referenceChara.UpdateFace();
			}
		}
		
		void LoadHair()
		{
			CharFileInfoCustomFemale cfic = GetCharCustom();
			if (cfic != null)
			{			
				referenceChara.femaleCustomInfo.hairAcsColor = cfic.hairAcsColor;
				referenceChara.femaleCustomInfo.hairColor = cfic.hairColor;
				referenceChara.femaleCustomInfo.hairId = cfic.hairId;
				referenceChara.femaleCustomInfo.hairType = cfic.hairType;
					
				referenceChara.femaleBody.ChangeHair(true);
				for (int i = 0; i < referenceChara.femaleCustomInfo.hairId.Length; i++)
					referenceChara.femaleCustom.ChangeHairColor(i);
			}
		}
		
		void FuseFace()
		{
			if (chaFileOne != null && chaFileTwo != null)
			{
				float blendMin = 0.25f, blendMax = 0.75f;
				float blendColorMin = 0f, blendColorMax = 0.9f;
//				float blend = UnityEngine.Random.Range(blendMin, blendMax); //0.3f, 0.7f
			
				for (int i = 0; i < referenceChara.femaleCustomInfo.shapeValueFace.Length; i++)
					referenceChara.femaleCustomInfo.shapeValueFace[i] = Mathf.Lerp(chaFileOne.shapeValueFace[i], chaFileTwo.shapeValueFace[i], /*blend*/UnityEngine.Random.Range(blendMin, blendMax));
				referenceChara.femaleCustom.UpdateShapeFaceValueFromCustomInfo();
			
				referenceChara.femaleCustomInfo.faceDetailWeight = Mathf.Lerp(chaFileOne.faceDetailWeight, chaFileTwo.faceDetailWeight, UnityEngine.Random.Range(blendMin, blendMax));
				referenceChara.femaleCustomInfo.headId = BoolR() ? chaFileOne.headId : chaFileTwo.headId;
				
				referenceChara.femaleCustomInfo.matEyebrowId = BoolR() ? chaFileOne.matEyebrowId : chaFileTwo.matEyebrowId;
				referenceChara.femaleCustomInfo.matEyeHiId = BoolR() ? chaFileOne.matEyeHiId : chaFileTwo.matEyeHiId;
				referenceChara.femaleCustomInfo.matEyelashesId = BoolR() ? chaFileOne.matEyelashesId : chaFileTwo.matEyelashesId;
				
				bool eyes = BoolR();
				referenceChara.femaleCustomInfo.matEyeLId = eyes ? chaFileOne.matEyeLId : chaFileTwo.matEyeLId;
				referenceChara.femaleCustomInfo.matEyeRId = eyes ? chaFileOne.matEyeRId : chaFileTwo.matEyeRId;
				
				referenceChara.femaleCustomInfo.texCheekId = BoolR() ? chaFileOne.texCheekId : chaFileTwo.texCheekId;
				referenceChara.femaleCustomInfo.texEyeshadowId = BoolR() ? chaFileOne.texEyeshadowId : chaFileTwo.texEyeshadowId;
				referenceChara.femaleCustomInfo.texFaceDetailId = BoolR() ? chaFileOne.texFaceDetailId : chaFileTwo.texFaceDetailId;
				referenceChara.femaleCustomInfo.texFaceId = BoolR() ? chaFileOne.texFaceId : chaFileTwo.texFaceId;
				referenceChara.femaleCustomInfo.texLipId = BoolR() ? chaFileOne.texLipId : chaFileTwo.texLipId;
				referenceChara.femaleCustomInfo.texMoleId = BoolR() ? chaFileOne.texMoleId : chaFileTwo.texMoleId;
				referenceChara.femaleCustomInfo.texTattoo_fId = BoolR() ? chaFileOne.texTattoo_fId : chaFileTwo.texTattoo_fId;
				
				referenceChara.femaleCustomInfo.eyebrowColor.BlendRGB(chaFileOne.eyebrowColor, chaFileTwo.eyebrowColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				referenceChara.femaleCustomInfo.eyeHiColor.BlendRGB(chaFileOne.eyeHiColor, chaFileTwo.eyeHiColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				referenceChara.femaleCustomInfo.eyelashesColor.BlendRGB(chaFileOne.eyelashesColor, chaFileTwo.eyelashesColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				float blendColor = UnityEngine.Random.Range(blendColorMin, blendColorMax);
				referenceChara.femaleCustomInfo.eyeLColor.BlendRGB(chaFileOne.eyeLColor, chaFileTwo.eyeLColor, blendColor);
				referenceChara.femaleCustomInfo.eyeRColor.BlendRGB(chaFileOne.eyeRColor, chaFileTwo.eyeRColor, blendColor);
				referenceChara.femaleCustomInfo.eyeWColor.BlendRGB(chaFileOne.eyeWColor, chaFileTwo.eyeWColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
			
				referenceChara.femaleCustomInfo.cheekColor.BlendRGB(chaFileOne.cheekColor, chaFileTwo.cheekColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				referenceChara.femaleCustomInfo.eyeshadowColor.BlendRGB(chaFileOne.eyeshadowColor, chaFileTwo.eyeshadowColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				referenceChara.femaleCustomInfo.lipColor.BlendRGB(chaFileOne.lipColor, chaFileTwo.lipColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				referenceChara.femaleCustomInfo.moleColor.BlendRGB(chaFileOne.moleColor, chaFileTwo.moleColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				referenceChara.femaleCustomInfo.tattoo_fColor.BlendRGB(chaFileOne.tattoo_fColor, chaFileTwo.tattoo_fColor, UnityEngine.Random.Range(blendColorMin, blendColorMax));
				
				referenceChara.femaleBody.ChangeHeadNew();
				referenceChara.femaleCustom.ChangeCustomFaceWithoutCustomTexture();
				referenceChara.UpdateFace();
			}
		}
		
		bool BoolR()
		{
			return new System.Random().Next() % 2 == 0;
		}		
	}
	
	public class DeleteChara : CharacterPart
	{
		Transform mainCanvas;
		CharaList listFemale, listMale;
		
		public override void Init()
		{
			partName = "Delete Character";
			
			PickUp();
			AddStep(DeleteButton);
		}
		
		void DeleteButton()
		{
			GUILayout.Label("This action is irreversible!");
			if (GUILayout.Button("DELETE CHARACTER SELECTED IN CHAR LIST"))
				Delete();
		}

		void PickUp()
		{
			mainCanvas = Studio.Studio.Instance.gameObject.transform.Find("Canvas Main Menu");
			listFemale = mainCanvas.Find("01_Add/00_Female").gameObject.GetComponent<CharaList>();
			listMale = mainCanvas.Find("01_Add/01_Male").gameObject.GetComponent<CharaList>();
		}
		
		void Delete()
		{
			CharaList operatingList = mainCanvas.Find("01_Add/00_Female").gameObject.activeInHierarchy ? listFemale : mainCanvas.Find("01_Add/01_Male").gameObject.activeInHierarchy ? listMale : null;
			if (operatingList != null)
			{
				CharaFileSort charaFiles = (CharaFileSort)operatingList.GetType()
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
	}
}
