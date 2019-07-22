using UnityEngine;
using System.Collections;

public class FreeFly_Avatar : ScenarioItem
{

    protected Vector3 translation = Vector3.zero;
    protected float rotationUp = 0f;
    protected float rotationBack = 0f;

    public JoystickController joystickController;

    // Use this for initialization
    override public void Start()
    {
        base.Start();
        if (joystickController)
        {
            joystickController.isFlying = true;
        }
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
    }


    public virtual void LateUpdate()
    {

        Vector3 vf = controller.transform.forward;

        float directionScaled = controller.direction * controller.flightMaxAngle;

        vf.y = 0f; vf.Normalize();
        translation = Time.deltaTime * controller.speed * vf;

        rotationUp = Time.deltaTime * directionScaled;
        rotationBack = 2 * Time.deltaTime * directionScaled;

        self.Translate(translation, Space.World);
        self.Rotate(Vector3.up, rotationUp, Space.Self);
        self.Rotate(Vector3.back, rotationBack, Space.Self);

        controller.direction = Mathf.Lerp(controller.direction, 0f, Time.deltaTime);

        if (self.eulerAngles.z > 45 && self.eulerAngles.z < 315)
        {
            float fix = self.eulerAngles.z < 180 ? 45 : 315;
            self.eulerAngles = new Vector3(self.eulerAngles.x, self.eulerAngles.y, fix);
        }

        Vector3 e = self.eulerAngles;
        if (e.z < 180f)
        {
            e.z = Mathf.Lerp(e.z, 0f, Time.deltaTime);
        }
        else
        {
            e.z = Mathf.Lerp(e.z, 360f, Time.deltaTime);
        }

        e.x = Mathf.Lerp(e.x, controller.maxBascule, Time.deltaTime);

        self.eulerAngles = e;

        // Altitude;
        Vector3 pos = controller.transform.position;
        pos.y = Mathf.Lerp(pos.y, controller.altitude, Time.deltaTime);
        controller.transform.position = pos;
    }
}
