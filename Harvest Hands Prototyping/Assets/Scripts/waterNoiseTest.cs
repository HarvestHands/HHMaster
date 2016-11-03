using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class waterNoiseTest : MonoBehaviour 

	{
		public float perlinScale = 4.56f;
		public float waveSpeed = 1f;
		public float waveHeight = 2f;

		private Mesh mesh;


		void Update()
		{
			AnimateMesh();
		}

		void AnimateMesh()
		{
			if ( !mesh )
				mesh = GetComponent< MeshFilter >().sharedMesh;

			Vector3[] vertices = mesh.vertices;

			for (int i = 0; i < vertices.Length; i++)
			{
				float pX = ( vertices[i].x * perlinScale ) + ( Time.timeSinceLevelLoad * waveSpeed );
				float pZ = ( vertices[i].z * perlinScale ) + ( Time.timeSinceLevelLoad * waveSpeed );

				vertices[i].y = ( Mathf.PerlinNoise( pX, pZ ) - 0.5f ) * waveHeight;
			}

			mesh.vertices = vertices;
		}

	void OnCollisionStay(Collision terrain){
		foreach (ContactPoint contact in terrain.contacts) {
			print (contact.thisCollider.name + "hit" + contact.otherCollider.name);
			Debug.DrawRay (contact.point, contact.normal, Color.white);
		}
	}
	}