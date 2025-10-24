using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.MiniMaps
{
	// Token: 0x02000273 RID: 627
	public class MiniMapSettings : MonoBehaviour, IMiniMapDataProvider
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x000493BD File Offset: 0x000475BD
		public Sprite CombinedSprite
		{
			get
			{
				return this.combinedSprite;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x000493C5 File Offset: 0x000475C5
		public Vector3 CombinedCenter
		{
			get
			{
				return this.combinedCenter;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x000493CD File Offset: 0x000475CD
		public List<IMiniMapEntry> Maps
		{
			get
			{
				return this.maps.ToList<IMiniMapEntry>();
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x000493DA File Offset: 0x000475DA
		// (set) Token: 0x060013BC RID: 5052 RVA: 0x000493E1 File Offset: 0x000475E1
		public static MiniMapSettings Instance { get; private set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x000493EC File Offset: 0x000475EC
		public float PixelSize
		{
			get
			{
				int width = this.combinedSprite.texture.width;
				if (width > 0 && this.combinedSize > 0f)
				{
					return this.combinedSize / (float)width;
				}
				return -1f;
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0004942C File Offset: 0x0004762C
		private void Awake()
		{
			foreach (MiniMapSettings.MapEntry mapEntry in this.maps)
			{
				SpriteRenderer offsetReference = mapEntry.offsetReference;
				if (offsetReference != null)
				{
					offsetReference.gameObject.SetActive(false);
				}
			}
			if (MiniMapSettings.Instance == null)
			{
				MiniMapSettings.Instance = this;
			}
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x000494A8 File Offset: 0x000476A8
		public static bool TryGetMinimapPosition(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = worldPosition;
			if (MiniMapSettings.Instance == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(sceneID))
			{
				return false;
			}
			MiniMapSettings.MapEntry mapEntry = MiniMapSettings.Instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e != null && e.sceneID == sceneID);
			if (mapEntry == null)
			{
				return false;
			}
			Vector3 a = worldPosition - mapEntry.mapWorldCenter;
			Vector3 b = mapEntry.mapWorldCenter - MiniMapSettings.Instance.combinedCenter;
			a + b;
			return true;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00049534 File Offset: 0x00047734
		public static bool TryGetWorldPosition(Vector3 minimapPosition, string sceneID, out Vector3 result)
		{
			result = minimapPosition;
			if (MiniMapSettings.Instance == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(sceneID))
			{
				return false;
			}
			MiniMapSettings.MapEntry mapEntry = MiniMapSettings.Instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e != null && e.sceneID == sceneID);
			if (mapEntry == null)
			{
				return false;
			}
			result = mapEntry.mapWorldCenter + minimapPosition;
			return true;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000495A8 File Offset: 0x000477A8
		public static bool TryGetMinimapPosition(Vector3 worldPosition, out Vector3 result)
		{
			result = worldPosition;
			Scene activeScene = SceneManager.GetActiveScene();
			if (!activeScene.isLoaded)
			{
				return false;
			}
			string sceneID = SceneInfoCollection.GetSceneID(activeScene.buildIndex);
			return MiniMapSettings.TryGetMinimapPosition(worldPosition, sceneID, out result);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x000495E4 File Offset: 0x000477E4
		internal void Cache(MiniMapCenter miniMapCenter)
		{
			int scene = miniMapCenter.gameObject.scene.buildIndex;
			MiniMapSettings.MapEntry mapEntry = this.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e.SceneReference != null && e.SceneReference.UnsafeReason == SceneReferenceUnsafeReason.None && e.SceneReference.BuildIndex == scene);
			if (mapEntry == null)
			{
				return;
			}
			mapEntry.mapWorldCenter = miniMapCenter.transform.position;
		}

		// Token: 0x04000E95 RID: 3733
		public List<MiniMapSettings.MapEntry> maps;

		// Token: 0x04000E96 RID: 3734
		public Vector3 combinedCenter;

		// Token: 0x04000E97 RID: 3735
		public float combinedSize;

		// Token: 0x04000E98 RID: 3736
		public Sprite combinedSprite;

		// Token: 0x02000546 RID: 1350
		[Serializable]
		public class MapEntry : IMiniMapEntry
		{
			// Token: 0x1700075A RID: 1882
			// (get) Token: 0x060027EB RID: 10219 RVA: 0x000925D0 File Offset: 0x000907D0
			public SceneReference SceneReference
			{
				get
				{
					SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
					if (sceneInfo == null)
					{
						return null;
					}
					return sceneInfo.SceneReference;
				}
			}

			// Token: 0x1700075B RID: 1883
			// (get) Token: 0x060027EC RID: 10220 RVA: 0x000925F4 File Offset: 0x000907F4
			public string SceneID
			{
				get
				{
					return this.sceneID;
				}
			}

			// Token: 0x1700075C RID: 1884
			// (get) Token: 0x060027ED RID: 10221 RVA: 0x000925FC File Offset: 0x000907FC
			public Sprite Sprite
			{
				get
				{
					return this.sprite;
				}
			}

			// Token: 0x1700075D RID: 1885
			// (get) Token: 0x060027EE RID: 10222 RVA: 0x00092604 File Offset: 0x00090804
			public bool Hide
			{
				get
				{
					return this.hide;
				}
			}

			// Token: 0x1700075E RID: 1886
			// (get) Token: 0x060027EF RID: 10223 RVA: 0x0009260C File Offset: 0x0009080C
			public bool NoSignal
			{
				get
				{
					return this.noSignal;
				}
			}

			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x060027F0 RID: 10224 RVA: 0x00092614 File Offset: 0x00090814
			public float PixelSize
			{
				get
				{
					int width = this.sprite.texture.width;
					if (width > 0 && this.imageWorldSize > 0f)
					{
						return this.imageWorldSize / (float)width;
					}
					return -1f;
				}
			}

			// Token: 0x17000760 RID: 1888
			// (get) Token: 0x060027F1 RID: 10225 RVA: 0x00092652 File Offset: 0x00090852
			public Vector2 Offset
			{
				get
				{
					if (this.offsetReference == null)
					{
						return Vector2.zero;
					}
					return this.offsetReference.transform.localPosition;
				}
			}

			// Token: 0x060027F2 RID: 10226 RVA: 0x0009267D File Offset: 0x0009087D
			public MapEntry()
			{
			}

			// Token: 0x060027F3 RID: 10227 RVA: 0x00092685 File Offset: 0x00090885
			public MapEntry(MiniMapSettings.MapEntry copyFrom)
			{
				this.imageWorldSize = copyFrom.imageWorldSize;
				this.sceneID = copyFrom.sceneID;
				this.sprite = copyFrom.sprite;
			}

			// Token: 0x04001EB3 RID: 7859
			public float imageWorldSize;

			// Token: 0x04001EB4 RID: 7860
			[SceneID]
			public string sceneID;

			// Token: 0x04001EB5 RID: 7861
			public Sprite sprite;

			// Token: 0x04001EB6 RID: 7862
			public SpriteRenderer offsetReference;

			// Token: 0x04001EB7 RID: 7863
			public Vector3 mapWorldCenter;

			// Token: 0x04001EB8 RID: 7864
			public bool hide;

			// Token: 0x04001EB9 RID: 7865
			public bool noSignal;
		}

		// Token: 0x02000547 RID: 1351
		public struct Data
		{
		}
	}
}
