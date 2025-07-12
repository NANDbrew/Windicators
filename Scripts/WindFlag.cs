using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Windicators.Scripts
{
    [RequireComponent(typeof(Cloth))]
    public class WindFlag : Weathervane
    {
        private Cloth cloth;
        private GameObject rolledCloth;

        public override void Awake()
        {
            base.Awake();
            cloth = GetComponent<Cloth>();
            rolledCloth = base.transform.parent.Find("rolled_cloth").gameObject;
            rolledCloth.SetActive(false);
            multiplier = 1f;
        }

        public override void LateUpdate()
        {
            if (!shadowedBy)
            {
                cloth.externalAcceleration = (Wind.currentWind - worldVelocity) * multiplier;
            }
            else
            {
                cloth.externalAcceleration = -worldVelocity * multiplier;
            }
        }
        public void OnEnable()
        {
            rolledCloth?.SetActive(false);
            this.StartCoroutine(ResetCloth());
        }
        public void OnDisable()
        {
            rolledCloth?.SetActive(true);
        }

        private IEnumerator ResetCloth()
        {
            cloth.enabled = false;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            cloth.enabled = true;
        }


    }
}
