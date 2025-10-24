using System;
using UnityEngine;

namespace Debugging
{
	// Token: 0x02000220 RID: 544
	public class InstantiateTiming : MonoBehaviour
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x0003F4D7 File Offset: 0x0003D6D7
		public void InstantiatePrefab()
		{
			Debug.Log("Start Instantiate");
			UnityEngine.Object.Instantiate<GameObject>(this.prefab);
			Debug.Log("Instantiated");
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0003F4F9 File Offset: 0x0003D6F9
		private void Awake()
		{
			Debug.Log("Awake");
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0003F505 File Offset: 0x0003D705
		private void Start()
		{
			Debug.Log("Start");
		}

		// Token: 0x04000D04 RID: 3332
		public GameObject prefab;
	}
}
