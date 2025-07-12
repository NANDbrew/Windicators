using System.Collections;
using UnityEngine;

namespace Windicators.Scripts
{
    public class AutoLevel : MonoBehaviour
    {
        ItemRigidbody itemRigidbody;
        ShipItem shipItem;
        bool locked;
        bool locking;

        private void Start()
        {
            shipItem = transform.parent.GetComponent<ShipItem>();
            //itemRigidbody = shipItem.GetItemRigidbody();
        }

        private void LateUpdate()
        {
/*            if (itemRigidbody == null)
            {
                Start();
                return;
            }*/
            if (shipItem.held)
            {
                locked = false;
            }
            else if (!locking && !locked)
            {
                StartCoroutine(LockRoutine());
            }
            if (!locked)
            {
                /*                Vector3 eulerAngles = base.transform.eulerAngles;
                                eulerAngles.x = 0f;
                                eulerAngles.z = 0f;
                                base.transform.eulerAngles = eulerAngles;*/
                //Vector3 eulerAngles = base.transform.eulerAngles;
                if (shipItem.currentActualBoat != null)
                {
                    /*eulerAngles.x = shipItem.currentActualBoat.eulerAngles.x;
                    eulerAngles.z = shipItem.currentActualBoat.eulerAngles.z;*/
                    //transform.rotation = shipItem.currentActualBoat.rotation;
                    //base.transform.LookAt(base.transform.forward, shipItem.currentActualBoat.up * 1000);
                    base.transform.up = shipItem.currentActualBoat.up;
                }
                else
                {
                    base.transform.up = Vector3.up;
/*
                    eulerAngles.x = 0f;
                    eulerAngles.z = 0f;*/
                    //transform.rotation = Quaternion.identity;
                }
                //base.transform.eulerAngles = eulerAngles;
            }

/*            else if (!locking)
            {
                if (!shipItem.held)
                {
                    if (!locked)
                    {
                        StartCoroutine(LockRoutine());
                    }
                }
                else
                {
                    locked = false;
                }
            }*/
        }

        public IEnumerator LockRoutine()
        {
            locking = true;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            locked = true;
            locking = false;
        }
    }
}
