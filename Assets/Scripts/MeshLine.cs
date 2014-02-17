using UnityEngine;
using System.Collections;

public class MeshLine : MonoBehaviour {

	public static void DrawLine (Vector3 start, Vector3 end, float width) {

		GameObject go = new GameObject();
		LineRenderer lr = go.AddComponent<LineRenderer>() as LineRenderer;
		go.transform.position = start;
		lr.SetWidth(width, width);
		lr.SetPosition(0, start - new Vector3(0, 1, 0));
		lr.SetPosition(1, end - new Vector3(0, 1, 0));
		Color cl = new Color(227, 227, 227, 0.5f);
		lr.SetColors(cl, cl);
		lr.material = new Material(Shader.Find("Sprites/Default"));
	}
}
