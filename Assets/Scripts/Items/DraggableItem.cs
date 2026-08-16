using UnityEngine;

public class DraggableItem : MonoBehaviour, iItemInteraction {

    Rigidbody rb;
    public float spring = 75f;
    public float dampening = 5f;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void click() {}

    public void release() {}

    public void drag(Vector3 snapPosition) {
        Vector3 vect = snapPosition - transform.position;
        float dist = vect.magnitude;

        Vector3 direction = vect.normalized;
        Vector3 force = (direction * dist * spring) - (rb.linearVelocity * dampening);
   
        rb.AddForce(force, ForceMode.Acceleration);

        Vector3 torque = rb.angularVelocity - (rb.angularVelocity * dampening);
        rb.AddTorque(torque, ForceMode.Acceleration);
    }
}
