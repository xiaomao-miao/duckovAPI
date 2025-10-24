using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Scenes
{
	// Token: 0x02000330 RID: 816
	[ExecuteAlways]
	public class SceneLocationsProvider : MonoBehaviour
	{
		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x00064535 File Offset: 0x00062735
		public static ReadOnlyCollection<SceneLocationsProvider> ActiveProviders
		{
			get
			{
				if (SceneLocationsProvider._activeProviders_ReadOnly == null)
				{
					SceneLocationsProvider._activeProviders_ReadOnly = new ReadOnlyCollection<SceneLocationsProvider>(SceneLocationsProvider.activeProviders);
				}
				return SceneLocationsProvider._activeProviders_ReadOnly;
			}
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x00064554 File Offset: 0x00062754
		public static SceneLocationsProvider GetProviderOfScene(SceneReference sceneReference)
		{
			if (sceneReference == null)
			{
				return null;
			}
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == sceneReference.BuildIndex);
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x00064590 File Offset: 0x00062790
		public static SceneLocationsProvider GetProviderOfScene(Scene scene)
		{
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == scene.buildIndex);
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000645C0 File Offset: 0x000627C0
		internal static SceneLocationsProvider GetProviderOfScene(int sceneBuildIndex)
		{
			return SceneLocationsProvider.ActiveProviders.FirstOrDefault((SceneLocationsProvider e) => e != null && e.gameObject.scene.buildIndex == sceneBuildIndex);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000645F0 File Offset: 0x000627F0
		public static Transform GetLocation(SceneReference scene, string name)
		{
			if (scene.UnsafeReason != SceneReferenceUnsafeReason.None)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(scene.BuildIndex, name);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x00064608 File Offset: 0x00062808
		public static Transform GetLocation(int sceneBuildIndex, string name)
		{
			SceneLocationsProvider providerOfScene = SceneLocationsProvider.GetProviderOfScene(sceneBuildIndex);
			if (providerOfScene == null)
			{
				return null;
			}
			return providerOfScene.GetLocation(name);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x00064630 File Offset: 0x00062830
		public static Transform GetLocation(string sceneID, string name)
		{
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(sceneID);
			if (sceneInfo == null)
			{
				return null;
			}
			return SceneLocationsProvider.GetLocation(sceneInfo.BuildIndex, name);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x00064655 File Offset: 0x00062855
		private void Awake()
		{
			SceneLocationsProvider.activeProviders.Add(this);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x00064662 File Offset: 0x00062862
		private void OnDestroy()
		{
			SceneLocationsProvider.activeProviders.Remove(this);
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x00064670 File Offset: 0x00062870
		public Transform GetLocation(string path)
		{
			string[] array = path.Split('/', StringSplitOptions.None);
			Transform transform = base.transform;
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					transform = transform.Find(text);
					if (transform == null)
					{
						return null;
					}
				}
			}
			return transform;
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000646BC File Offset: 0x000628BC
		public bool TryGetPath(Transform value, out string path)
		{
			path = "";
			Transform transform = value;
			List<Transform> list = new List<Transform>();
			while (transform != null && transform != base.transform)
			{
				list.Insert(0, transform);
				transform = transform.parent;
			}
			if (transform != base.transform)
			{
				return false;
			}
			this.sb.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				if (i > 0)
				{
					this.sb.Append('/');
				}
				this.sb.Append(list[i].name);
			}
			path = this.sb.ToString();
			return true;
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x00064768 File Offset: 0x00062968
		[return: TupleElementNames(new string[]
		{
			"path",
			"worldPosition",
			"gameObject"
		})]
		public List<ValueTuple<string, Vector3, GameObject>> GetAllPathsAndItsPosition()
		{
			List<ValueTuple<string, Vector3, GameObject>> list = new List<ValueTuple<string, Vector3, GameObject>>();
			Stack<Transform> stack = new Stack<Transform>();
			stack.Push(base.transform);
			while (stack.Count > 0)
			{
				Transform transform = stack.Pop();
				int childCount = transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = transform.GetChild(i);
					string item;
					if (this.TryGetPath(child, out item))
					{
						list.Add(new ValueTuple<string, Vector3, GameObject>(item, child.transform.position, child.gameObject));
						stack.Push(child);
					}
				}
			}
			return list;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000647F8 File Offset: 0x000629F8
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			foreach (Transform transform in base.transform.GetComponentsInChildren<Transform>())
			{
				if (transform.childCount == 0)
				{
					Gizmos.DrawSphere(transform.position, 1.5f);
				}
			}
		}

		// Token: 0x04001393 RID: 5011
		private static List<SceneLocationsProvider> activeProviders = new List<SceneLocationsProvider>();

		// Token: 0x04001394 RID: 5012
		private static ReadOnlyCollection<SceneLocationsProvider> _activeProviders_ReadOnly;

		// Token: 0x04001395 RID: 5013
		private StringBuilder sb = new StringBuilder();
	}
}
