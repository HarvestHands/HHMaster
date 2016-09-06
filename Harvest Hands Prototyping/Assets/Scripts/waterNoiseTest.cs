using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class waterNoiseTest : MonoBehaviour 

{
	public float perlinScale;
	public float waveSpeed;
	public float waveHeight;

	private Mesh mesh;
		
	public GameObject splash;

	void Update()
	{
		AnimateMesh();
	}

	void OnCollision(Collider col){

		if (col.gameObject.GetComponent<Pickupable> () != null) {

			Instantiate (splash, col.transform.position, col.transform.rotation );
			Debug.Log ("Splash");
		}

	}

	void AnimateMesh()
	{
		if ( !mesh )
			mesh = GetComponent< MeshFilter >().mesh;
	
		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i < vertices.Length; i++)
		{
			float pX = ( vertices[i].x * perlinScale ) + ( Time.timeSinceLevelLoad * waveSpeed );
			float pZ = ( vertices[i].z * perlinScale ) + ( Time.timeSinceLevelLoad * waveSpeed );

			vertices[i].y = ( Mathf.PerlinNoise( pX, pZ ) - 0.5f ) * waveHeight;
		}

		mesh.vertices = vertices;
	}
}