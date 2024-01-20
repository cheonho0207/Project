using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TIME_CONTROL
{

    public class Background : MonoBehaviour
    {
        public void Awake()
        {
            Physics.IgnoreLayerCollision(8, 9);
        }

        public void OnTriggerEnter(Collider other)
        {
            Copy_Self();

        }

        public void Copy_Self()
        {
            GameObject new_obj = GameObject.Instantiate(gameObject) as GameObject;
            new_obj.transform.Translate(new Vector3((transform.localScale.y * -10.0f), 0.0f, 0.0f));
        }
        // Start is called before the first frame update

    }
}
