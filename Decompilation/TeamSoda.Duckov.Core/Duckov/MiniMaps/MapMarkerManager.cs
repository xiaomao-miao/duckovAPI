using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Saves;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000270 RID: 624
	public class MapMarkerManager : MonoBehaviour
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00048C3F File Offset: 0x00046E3F
		// (set) Token: 0x0600138D RID: 5005 RVA: 0x00048C46 File Offset: 0x00046E46
		public static MapMarkerManager Instance { get; private set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x00048C4E File Offset: 0x00046E4E
		public static int SelectedIconIndex
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return 0;
				}
				return MapMarkerManager.Instance.selectedIconIndex;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00048C69 File Offset: 0x00046E69
		public static Color SelectedColor
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return Color.white;
				}
				return MapMarkerManager.Instance.selectedColor;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x00048C88 File Offset: 0x00046E88
		public static Sprite SelectedIcon
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				if (MapMarkerManager.Instance.icons.Count <= MapMarkerManager.SelectedIconIndex)
				{
					return null;
				}
				return MapMarkerManager.Instance.icons[MapMarkerManager.SelectedIconIndex];
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x00048CC8 File Offset: 0x00046EC8
		public static string SelectedIconName
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				Sprite selectedIcon = MapMarkerManager.SelectedIcon;
				if (selectedIcon == null)
				{
					return null;
				}
				return selectedIcon.name;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x00048CFB File Offset: 0x00046EFB
		public static List<Sprite> Icons
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				return MapMarkerManager.Instance.icons;
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00048D16 File Offset: 0x00046F16
		private void Awake()
		{
			MapMarkerManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00048D2F File Offset: 0x00046F2F
		private void Start()
		{
			this.Load();
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00048D37 File Offset: 0x00046F37
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x00048D4A File Offset: 0x00046F4A
		private string SaveKey
		{
			get
			{
				return "MapMarkerManager_" + MultiSceneCore.MainSceneID;
			}
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00048D5C File Offset: 0x00046F5C
		private void Load()
		{
			this.loaded = true;
			MapMarkerManager.SaveData saveData = SavesSystem.Load<MapMarkerManager.SaveData>(this.SaveKey);
			if (saveData.pois != null)
			{
				foreach (MapMarkerPOI.RuntimeData data in saveData.pois)
				{
					MapMarkerManager.Request(data);
				}
			}
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00048DC8 File Offset: 0x00046FC8
		private void OnCollectSaveData()
		{
			if (!this.loaded)
			{
				return;
			}
			MapMarkerManager.SaveData saveData = new MapMarkerManager.SaveData
			{
				pois = new List<MapMarkerPOI.RuntimeData>()
			};
			foreach (MapMarkerPOI mapMarkerPOI in this.pois)
			{
				if (!(mapMarkerPOI == null))
				{
					saveData.pois.Add(mapMarkerPOI.Data);
				}
			}
			SavesSystem.Save<MapMarkerManager.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00048E5C File Offset: 0x0004705C
		public static void Request(MapMarkerPOI.RuntimeData data)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerPOI mapMarkerPOI = UnityEngine.Object.Instantiate<MapMarkerPOI>(MapMarkerManager.Instance.markerPrefab);
			mapMarkerPOI.Setup(data);
			MapMarkerManager.Instance.pois.Add(mapMarkerPOI);
			MultiSceneCore.MoveToMainScene(mapMarkerPOI.gameObject);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00048EAC File Offset: 0x000470AC
		public static void Request(Vector3 worldPos)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerPOI mapMarkerPOI = UnityEngine.Object.Instantiate<MapMarkerPOI>(MapMarkerManager.Instance.markerPrefab);
			mapMarkerPOI.Setup(worldPos, MapMarkerManager.SelectedIconName, MultiSceneCore.ActiveSubSceneID, new Color?(MapMarkerManager.SelectedColor));
			MapMarkerManager.Instance.pois.Add(mapMarkerPOI);
			MultiSceneCore.MoveToMainScene(mapMarkerPOI.gameObject);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00048F0D File Offset: 0x0004710D
		public static void Release(MapMarkerPOI entry)
		{
			if (entry == null)
			{
				return;
			}
			if (MapMarkerManager.Instance != null)
			{
				MapMarkerManager.Instance.pois.Remove(entry);
			}
			if (entry != null)
			{
				UnityEngine.Object.Destroy(entry.gameObject);
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00048F4C File Offset: 0x0004714C
		internal static Sprite GetIcon(string iconName)
		{
			if (MapMarkerManager.Instance == null)
			{
				return null;
			}
			if (MapMarkerManager.Instance.icons == null)
			{
				return null;
			}
			return MapMarkerManager.Instance.icons.Find((Sprite e) => e != null && e.name == iconName);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00048F9E File Offset: 0x0004719E
		internal static void SelectColor(Color color)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerManager.Instance.selectedColor = color;
			Action<Color> onColorChanged = MapMarkerManager.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(color);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00048FC9 File Offset: 0x000471C9
		internal static void SelectIcon(int index)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerManager.Instance.selectedIconIndex = index;
			Action<int> onIconChanged = MapMarkerManager.OnIconChanged;
			if (onIconChanged == null)
			{
				return;
			}
			onIconChanged(index);
		}

		// Token: 0x04000E8A RID: 3722
		[SerializeField]
		private List<Sprite> icons = new List<Sprite>();

		// Token: 0x04000E8B RID: 3723
		[SerializeField]
		private MapMarkerPOI markerPrefab;

		// Token: 0x04000E8C RID: 3724
		[SerializeField]
		private int selectedIconIndex;

		// Token: 0x04000E8D RID: 3725
		[SerializeField]
		private Color selectedColor = Color.white;

		// Token: 0x04000E8E RID: 3726
		public static Action<int> OnIconChanged;

		// Token: 0x04000E8F RID: 3727
		public static Action<Color> OnColorChanged;

		// Token: 0x04000E90 RID: 3728
		private bool loaded;

		// Token: 0x04000E91 RID: 3729
		private List<MapMarkerPOI> pois = new List<MapMarkerPOI>();

		// Token: 0x02000541 RID: 1345
		[Serializable]
		private struct SaveData
		{
			// Token: 0x04001EAA RID: 7850
			public string mainSceneName;

			// Token: 0x04001EAB RID: 7851
			public List<MapMarkerPOI.RuntimeData> pois;
		}
	}
}
