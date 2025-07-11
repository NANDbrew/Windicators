using HarmonyLib;

namespace Windicators
{
    [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
    internal static class ItemTest
    {
        internal static string[] prefabNames = { "telltale", "telltale 1", "telltale 2", "item wind vane", "wind compass E", "telltale 3" };
        internal static void Prefix()
        {
            Array.Resize(ref PrefabsDirectory.instance.directory, 512);
            AssetTools.LoadAssetBundles();
            foreach (var name in prefabNames)
            {
                var prefab = AssetTools.bundle.LoadAsset("Assets/ShipyardExpansion/" + name + ".prefab") as GameObject;
                PrefabsDirectory.instance.directory[prefab.GetComponent<SaveablePrefab>().prefabIndex] = prefab;
                if (prefab.GetComponentInChildren<WindDirectionDisplay>() is WindDirectionDisplay disp)
                {
                    var newComp = disp.gameObject.AddComponent<Weathervane>();
                    if (name == "telltale 2" || name == "telltale 3") newComp.pivotLocal = false;
                    Component.Destroy(disp);
                }
                if (prefab.transform.GetChild(0).GetComponent<WindClothSimple>() is WindClothSimple wcs)
                {
                    wcs.gameObject.AddComponent<WindFlag>().multiplier = wcs.staticMultiplier;
                    Component.Destroy(wcs);
                }
                //prefab.AddComponent<WindToggler>();
            }
        }
    }
    [HarmonyPatch(typeof(ShipItem))]
    internal static class HangableItemPatches
    {
        [HarmonyPatch("OnEnterInventory")]
        [HarmonyPostfix]
        public static void EnterPatch(ShipItem __instance)
        {
            Debug.Log("into the pocket");
            __instance.GetComponentInChildren<WindFlag>(true)?.gameObject.SetActive(false);
        }
        [HarmonyPatch("OnLeaveInventory")]
        [HarmonyPostfix]
        internal static void ExitPatch(ShipItem __instance)
        {
            Debug.Log("out of the pocket");
            __instance.GetComponentInChildren<WindFlag>(true)?.gameObject.SetActive(true);
        }
    }
    [RequireComponent(typeof(Collider))]
    public class Weathervane : MonoBehaviour
    {
        private Vector3 previousPosition;
        public Vector3 worldVelocity;
        public Transform shadowedBy;
        public float multiplier = 0.1f;

        float maxTilt = 75;
        Vector3 startAngles;
        public bool pivotLocal = true;

        public virtual void Awake()
        {
            base.GetComponent<Collider>().isTrigger = true;
            startAngles = base.transform.localEulerAngles;
        }
        public void Update()
        {
            if (shadowedBy != null)
            {
                Vector3 localPos = transform.position - shadowedBy.position;
                worldVelocity = (localPos - previousPosition) / Time.deltaTime;
                previousPosition = localPos;
            }
            else
            {
                worldVelocity = (transform.position - previousPosition) / Time.deltaTime;
                previousPosition = transform.position;
            }
        }
        public virtual void LateUpdate()
        {
            if (GameState.wasInSettingsMenu || !GameState.playing) return;
            //if (!pivotLocal && Vector3.Angle(-transform.parent.forward, Vector3.up) > maxTilt) return;

            if (!shadowedBy)
            {
                base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation((Wind.currentWind - worldVelocity), Vector3.up), Time.deltaTime * (Wind.currentWind.magnitude - worldVelocity.magnitude) * multiplier);
            }
            else
            {
                base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(-worldVelocity, Vector3.up), Time.deltaTime * worldVelocity.magnitude * multiplier);
            }

            if (pivotLocal) transform.localEulerAngles = new Vector3(startAngles.x, transform.localEulerAngles.y, startAngles.z);
            else transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<InteriorEffectsTrigger>() != null)
            {
                shadowedBy = other.transform;
                previousPosition = transform.position - shadowedBy.position;
                Debug.Log("flag entered interior");
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<InteriorEffectsTrigger>() != null)
            {
                shadowedBy = null;
                previousPosition = transform.position;
                Debug.Log("flag exited interior");
            }
        }

    }

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

    [HarmonyPatch(typeof(PlayerCrouching))]
    internal static class NewSpawner
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePatch()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                //Debug.Log("trying to instantiate object");
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(389, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Refs.ovrCameraRig.rotation);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(388, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Refs.ovrCameraRig.rotation);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(387, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Refs.ovrCameraRig.rotation);
                }
            }
        }
        public static GameObject SpawnItem(int itemIndex, Vector3 position, Quaternion rotation)
        {

            GameObject obj = UnityEngine.Object.Instantiate(SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory[itemIndex], position, rotation);
            obj.GetComponent<ShipItem>().sold = true;
            obj.GetComponent<SaveablePrefab>().RegisterToSave();
            //obj.GetComponent<Good>()?.RegisterAsMissionless();
            return obj;
        }

    }


}
