using System;
using Duckov.Scenes;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Duckov.MiniMaps
{
	// Token: 0x02000276 RID: 630
	public class SimplePointOfInterest : MonoBehaviour, IPointOfInterest
	{
		// Token: 0x14000081 RID: 129
		// (add) Token: 0x060013D8 RID: 5080 RVA: 0x000497E8 File Offset: 0x000479E8
		// (remove) Token: 0x060013D9 RID: 5081 RVA: 0x00049820 File Offset: 0x00047A20
		public event Action<PointerEventData> OnClicked;

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00049855 File Offset: 0x00047A55
		// (set) Token: 0x060013DB RID: 5083 RVA: 0x0004985D File Offset: 0x00047A5D
		public float ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
			set
			{
				this.scaleFactor = value;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00049866 File Offset: 0x00047A66
		// (set) Token: 0x060013DD RID: 5085 RVA: 0x0004986E File Offset: 0x00047A6E
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00049877 File Offset: 0x00047A77
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x0004987F File Offset: 0x00047A7F
		public Color ShadowColor
		{
			get
			{
				return this.shadowColor;
			}
			set
			{
				this.shadowColor = value;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00049888 File Offset: 0x00047A88
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x00049890 File Offset: 0x00047A90
		public float ShadowDistance
		{
			get
			{
				return this.shadowDistance;
			}
			set
			{
				this.shadowDistance = value;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x00049899 File Offset: 0x00047A99
		public string DisplayName
		{
			get
			{
				return this.displayName.ToPlainText();
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x000498A6 File Offset: 0x00047AA6
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x000498B0 File Offset: 0x00047AB0
		public int OverrideScene
		{
			get
			{
				if (this.followActiveScene && MultiSceneCore.ActiveSubScene != null)
				{
					return MultiSceneCore.ActiveSubScene.Value.buildIndex;
				}
				if (!string.IsNullOrEmpty(this.overrideSceneID))
				{
					return SceneInfoCollection.GetBuildIndex(this.overrideSceneID);
				}
				return -1;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00049904 File Offset: 0x00047B04
		// (set) Token: 0x060013E6 RID: 5094 RVA: 0x0004990C File Offset: 0x00047B0C
		public bool IsArea
		{
			get
			{
				return this.isArea;
			}
			set
			{
				this.isArea = value;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x00049915 File Offset: 0x00047B15
		// (set) Token: 0x060013E8 RID: 5096 RVA: 0x0004991D File Offset: 0x00047B1D
		public float AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
			set
			{
				this.areaRadius = value;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00049926 File Offset: 0x00047B26
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x0004992E File Offset: 0x00047B2E
		public bool HideIcon
		{
			get
			{
				return this.hideIcon;
			}
			set
			{
				this.hideIcon = value;
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00049937 File Offset: 0x00047B37
		private void OnEnable()
		{
			PointsOfInterests.Register(this);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0004993F File Offset: 0x00047B3F
		private void OnDisable()
		{
			PointsOfInterests.Unregister(this);
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00049947 File Offset: 0x00047B47
		public void Setup(Sprite icon = null, string displayName = null, bool followActiveScene = false, string overrideSceneID = null)
		{
			if (icon != null)
			{
				this.icon = icon;
			}
			this.displayName = displayName;
			this.followActiveScene = followActiveScene;
			this.overrideSceneID = overrideSceneID;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0004997B File Offset: 0x00047B7B
		public void SetColor(Color color)
		{
			this.color = color;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00049984 File Offset: 0x00047B84
		public bool SetupMultiSceneLocation(MultiSceneLocation location, bool moveToMainScene = true)
		{
			Vector3 position;
			if (!location.TryGetLocationPosition(out position))
			{
				return false;
			}
			base.transform.position = position;
			this.overrideSceneID = location.SceneID;
			if (moveToMainScene && MultiSceneCore.MainScene != null)
			{
				SceneManager.MoveGameObjectToScene(base.gameObject, MultiSceneCore.MainScene.Value);
			}
			return true;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000499E4 File Offset: 0x00047BE4
		public static SimplePointOfInterest Create(Vector3 position, string sceneID, string displayName, Sprite icon = null, bool hideIcon = false)
		{
			GameObject gameObject = new GameObject("POI_" + displayName);
			gameObject.transform.position = position;
			SimplePointOfInterest simplePointOfInterest = gameObject.AddComponent<SimplePointOfInterest>();
			simplePointOfInterest.overrideSceneID = sceneID;
			simplePointOfInterest.displayName = displayName;
			simplePointOfInterest.hideIcon = hideIcon;
			simplePointOfInterest.icon = icon;
			SceneManager.MoveGameObjectToScene(gameObject, MultiSceneCore.MainScene.Value);
			return simplePointOfInterest;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00049A44 File Offset: 0x00047C44
		public void NotifyClicked(PointerEventData pointerEventData)
		{
			Action<PointerEventData> onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked(pointerEventData);
		}

		// Token: 0x04000E9E RID: 3742
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000E9F RID: 3743
		[SerializeField]
		private Color color = Color.white;

		// Token: 0x04000EA0 RID: 3744
		[SerializeField]
		private Color shadowColor = Color.white;

		// Token: 0x04000EA1 RID: 3745
		[SerializeField]
		private float shadowDistance;

		// Token: 0x04000EA2 RID: 3746
		[LocalizationKey("Default")]
		[SerializeField]
		private string displayName = "";

		// Token: 0x04000EA3 RID: 3747
		[SerializeField]
		private bool followActiveScene;

		// Token: 0x04000EA4 RID: 3748
		[SceneID]
		[SerializeField]
		private string overrideSceneID;

		// Token: 0x04000EA5 RID: 3749
		[SerializeField]
		private bool isArea;

		// Token: 0x04000EA6 RID: 3750
		[SerializeField]
		private float areaRadius;

		// Token: 0x04000EA7 RID: 3751
		[SerializeField]
		private float scaleFactor = 1f;

		// Token: 0x04000EA9 RID: 3753
		[SerializeField]
		private bool hideIcon;
	}
}
