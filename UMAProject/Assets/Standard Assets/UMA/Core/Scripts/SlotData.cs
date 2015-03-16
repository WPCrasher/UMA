using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace UMA
{
	[System.Serializable]
	public partial class SlotData
	{
		public SlotDataAsset asset;
		[System.Obsolete("SlotData.slotName is obsolete use asset.slotName!", false)]
		public string slotName;
		[System.Obsolete("SlotData.listID is obsolete.", false)]
		public int listID = -1;

		[System.Obsolete("SlotData.meshRenderer is obsolete.", true)]
		public SkinnedMeshRenderer meshRenderer;
		[System.Obsolete("SlotData.boneNameHashes is obsolete.", true)]
		public int[] boneNameHashes;
		[System.Obsolete("SlotData.boneWeights is obsolete.", true)]
		public BoneWeight[] boneWeights;
		[System.Obsolete("SlotData.umaBoneData is obsolete.", true)]
		public Transform[] umaBoneData;

		public Material materialSample;
		public float overlayScale = 1.0f;
		[System.Obsolete("SlotData.animatedBones is obsolete, use SlotDataAsset.animatedBones.", true)]
		public Transform[] animatedBones = new Transform[0];
		[System.Obsolete("SlotData.textureNameList is obsolete, use SlotDataAsset.textureNameList.", true)]
		public string[] textureNameList;
		[System.Obsolete("SlotData.slotDNA is obsolete, use SlotDataAsset.slotDNA.", true)]
		public DnaConverterBehaviour slotDNA;
		[System.Obsolete("SlotData.subMeshIndex is obsolete, use SlotDataAsset.subMeshIndex.", true)]
		public int subMeshIndex;
		/// <summary>
		/// Use this to identify slots that serves the same purpose
		/// Eg. ChestArmor, Helmet, etc.
		/// </summary>
		[System.Obsolete("SlotData.slotGroup is obsolete, use SlotDataAsset.slotGroup.", false)]
		public string slotGroup;
		/// <summary>
		/// Use this to identify what kind of overlays fit this slotData
		/// Eg. BaseMeshSkin, BaseMeshOverlays, GenericPlateArmor01
		/// </summary>
		[System.Obsolete("SlotData.tags is obsolete, use SlotDataAsset.tags.", false)]
		public string[] tags;

		private List<OverlayData> overlayList = new List<OverlayData>();

		public SlotData(SlotDataAsset asset)
		{
			this.asset = asset;
			slotName = asset.slotName;
			materialSample = asset.materialSample;
			overlayScale = asset.overlayScale;
		}	
        
		public SlotData()
		{
            
		}

		public int GetTextureChannelCount(UMAGeneratorBase generator)
		{
			return asset.GetTextureChannelCount(generator);
		}
        
		public bool RemoveOverlay(params string[] names)
		{
			bool changed = false;
			foreach (var name in names)
			{
				for (int i = 0; i < overlayList.Count; i++)
				{
					if (overlayList[i].asset.overlayName == name)
					{
						overlayList.RemoveAt(i);
						changed = true;
						break;
					}
				}
			}
			return changed;
		}
        
		public bool SetOverlayColor(Color32 color, params string[] names)
		{
			bool changed = false;
			foreach (var name in names)
			{
				foreach (var overlay in overlayList)
				{
					if (overlay.asset.overlayName == name)
					{
						overlay.color = color;
						changed = true;
					}
				}
			}
			return changed;
		}
        
		public OverlayData GetOverlay(params string[] names)
		{
			foreach (var name in names)
			{
				foreach (var overlay in overlayList)
				{
					if (overlay.asset.overlayName == name)
					{
						return overlay;
					}
				}
			}
			return null;
		}
        
		public void SetOverlay(int index, OverlayData overlay)
		{
			if (index >= overlayList.Count)
			{
				overlayList.Capacity = index + 1;
				while (index >= overlayList.Count)
				{
					overlayList.Add(null);
				}
			}
			overlayList[index] = overlay;
		}
        
		public OverlayData GetOverlay(int index)
		{
			if (index < 0 || index >= overlayList.Count)
				return null;
            return overlayList[index];
		}
        
		public int OverlayCount { get { return overlayList.Count; } }
        
		public void SetOverlayList(List<OverlayData> overlayList)
		{
			this.overlayList = overlayList;
		}
        
		public void AddOverlay(OverlayData overlayData)
		{
			overlayList.Add(overlayData);
		}
        
		public List<OverlayData> GetOverlayList()
		{
			return overlayList;
		}
        
		internal bool Validate(UMAGeneratorBase generator)
		{
			bool valid = true;
			if (asset.meshData != null)
			{
				string[] activeList;
				if (asset.textureNameList == null || asset.textureNameList.Length == 0)
				{
					activeList = generator.textureNameList;
				} else
				{
					activeList = asset.textureNameList;
				}
				int count = activeList.Length;
				while (count > 0 && string.IsNullOrEmpty(activeList[count - 1]))
				{
					count--;
				}
				for (int i = 0; i < overlayList.Count; i++)
				{
                    var overlayData = overlayList[i];
					if (overlayData != null)
					{
						if (overlayData.asset.textureList.Length != count && count != 0)
						{
							Debug.LogError(string.Format("Overlay '{0}' only have {1} textures, but it is added to SlotData '{2}' which requires {3} textures.", overlayData.asset.overlayName, overlayData.asset.textureList.Length, slotName, count));
							valid = false;
						}
					}
				}
			}
			return valid;
		}
        
		public override string ToString()
		{
			return "SlotData: " + asset.slotName;
		}

		public static implicit operator bool(SlotData obj) { return obj != null; }

	}
}