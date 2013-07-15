using UnityEngine;
using System.Collections;

public class TransLookAtUpLock : MonoBehaviour {
    public string targetTag = "MainCamera"; //if target is null
    public Transform target;

    public bool visibleCheck = true; //if true, only compute if source's renderer is visible

    public Transform source; //if null, use this transform

    void Awake() {
        if(target == null) {
            GameObject go = GameObject.FindGameObjectWithTag(targetTag);
            if(go != null)
                target = go.transform;
        }

        if(source == null)
            source = transform;
    }
    
    // Update is called once per frame
    void Update() {
        if(!visibleCheck || source.renderer.isVisible) {
            Vector3 v = target.position - source.position;
            v.y = 0.0f;
            Vector3 f = source.forward;
            f.y = 0.0f;

            float s = M8.MathUtil.CheckSideSign(new Vector2(v.x, v.z), new Vector2(f.x, f.z));

            float angle = Vector3.Angle(f, v);

            source.rotation *= Quaternion.AngleAxis(s*angle, Vector3.up);
        }
    }
}
