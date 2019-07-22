using UnityEngine;
using System.Collections;

public class GatewayScript : MonoBehaviour {
	
	public int _gateNumber;
	public bool _isFinalGate;
    public float _rotateSpeed;

	public GameObject _nextGate;

    public static bool finishGatesWay;

    void Start ()
    {

    }

    void Update ()
    {
        transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
    }

    public void EnableFirstGate ()
    {
        if (_gateNumber == 0)
        {
            GetComponent<Animation>()["open"].speed = 4;
            this.GetComponent<Animation>().Play("open");

            this.gameObject.GetComponent<SphereCollider>().enabled = true;
        }
    }
	
	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            if (!_isFinalGate)
            {
                Animation anim = _nextGate.GetComponent<Animation>();

                anim["open"].speed = 4;
                if (!anim.isPlaying)
                    anim.Play("open");

                _nextGate.GetComponent<SphereCollider>().enabled = true;
            }

            else
            {
                Debug.Log("Fin du parcours");
                finishGatesWay = true;
            }
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Animation>()["open"].speed = -4;
            GetComponent<Animation>()["open"].time = GetComponent<Animation>()["open"].length;
            this.GetComponent<Animation>().Play("open");

            this.gameObject.GetComponent<SphereCollider>().enabled = false;
        }  
    }
}
