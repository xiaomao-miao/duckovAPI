using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.MiniMaps
{
	// Token: 0x02000271 RID: 625
	public class MapMarkerPOI : MonoBehaviour, IPointOfInterest
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x0004901D File Offset: 0x0004721D
		public MapMarkerPOI.RuntimeData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00049025 File Offset: 0x00047225
		public Sprite Icon
		{
			get
			{
				return MapMarkerManager.GetIcon(this.data.iconName);
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x00049037 File Offset: 0x00047237
		public int OverrideScene
		{
			get
			{
				return SceneInfoCollection.GetBuildIndex(this.data.overrideSceneKey);
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x00049049 File Offset: 0x00047249
		public Color Color
		{
			get
			{
				return this.data.color;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x00049056 File Offset: 0x00047256
		public Color ShadowColor
		{
			get
			{
				return Color.black;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x0004905D File Offset: 0x0004725D
		public float ScaleFactor
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00049064 File Offset: 0x00047264
		public void Setup(Vector3 worldPosition, string iconName = "", string overrideScene = "", Color? color = null)
		{
			this.data = new MapMarkerPOI.RuntimeData
			{
				worldPosition = worldPosition,
				iconName = iconName,
				overrideSceneKey = overrideScene,
				color = ((color == null) ? Color.white : color.Value)
			};
			base.transform.position = worldPosition;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x000490CE File Offset: 0x000472CE
		public void Setup(MapMarkerPOI.RuntimeData data)
		{
			this.data = data;
			base.transform.position = data.worldPosition;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000490F4 File Offset: 0x000472F4
		public void NotifyClicked(PointerEventData eventData)
		{
			MapMarkerManager.Release(this);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x000490FC File Offset: 0x000472FC
		private void OnDestroy()
		{
			PointsOfInterests.Unregister(this);
		}

		// Token: 0x04000E92 RID: 3730
		[SerializeField]
		private MapMarkerPOI.RuntimeData data;

		// Token: 0x02000543 RID: 1347
		[Serializable]
		public struct RuntimeData
		{
			// Token: 0x04001EAD RID: 7853
			public Vector3 worldPosition;

			// Token: 0x04001EAE RID: 7854
			public string iconName;

			// Token: 0x04001EAF RID: 7855
			public string overrideSceneKey;

			// Token: 0x04001EB0 RID: 7856
			public Color color;
		}
	}
}
