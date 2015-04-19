using UnityEngine;
using System.Collections;

public class SyncTransformDelayed : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target.localRotation, 0.25f);
	}
}
