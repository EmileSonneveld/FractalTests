using UnityEngine;
using System.Collections;

public class TriangularFractal : MonoBehaviour {

	void Start () {
        GetComponent<MeshFilter>().mesh = GeometryMaker.MakePrism();

	}
	
	void Update () {
	
	}
}
