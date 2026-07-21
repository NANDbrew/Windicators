using UnityEngine;
using UnityEngine.SceneManagement;

namespace Windicators
{
    internal class AddShopItems
    {
        internal static void SceneLoaded(Scene scene, LoadSceneMode _)
        {
            if (scene.name == "island 15 M (Fort)")
                FortAestrin();
            if (scene.name == "island 9 E (dragon cliffs) scenery")
                FortAestrin();
        }

        internal static void FortAestrin()
        {
            var scenery = GameObject.Find("island 15 M (Fort) scenery");
            if (scenery == null)
            {
                Debug.LogError("Fort Aestrin scenery not found.");
                return;
            }

            MakeShopItem("shop item (999)", scenery.transform, new Vector3(-68f, 3.3f, 44.1f), new Vector3(2f, 142f, 359.8f), PrefabsDirectory.instance.directory[514]);
        }
        internal static void DragonCliffs()
        {
            var scenery = GameObject.Find("island 9 E (dragon cliffs) scenery");
            if (scenery == null)
            {
                Debug.LogError("scenery not found.");
                return;
            }

            MakeShopItem("shop item (999)", scenery.transform, new Vector3(-79.8f, 4.6f, -546.5f), new Vector3(75f, 45.4f, 78.5f), PrefabsDirectory.instance.directory[512]);
        }
        internal static void MakeShopItem(string name, Transform parent, Vector3 position, Vector3 rotation, GameObject go)
        {
            var shopitem = new GameObject(name);
            shopitem.transform.parent = parent;
            shopitem.transform.localPosition = position;
            shopitem.transform.localRotation = Quaternion.Euler(rotation);
            var filter = shopitem.AddComponent<MeshFilter>();
            filter.mesh = go.GetComponent<MeshFilter>().mesh;
            shopitem.AddComponent<MeshRenderer>();
            var itemSpawner = shopitem.AddComponent<ShopItemSpawner>();
            itemSpawner.itemPrefab = go;
        }

        internal static void SpawnShop(GameObject scenery)
        {
            if (AssetTools.bundle == null) AssetTools.LoadAssetBundles();
            int index = scenery.gameObject.GetComponent<IslandSceneryScene>().parentIslandIndex;
#if DEBUG
            Debug.Log("Windicators: patching shopkeeper on island " + index);
#endif
            if (AssetTools.shopKeepers.ContainsKey(index))
            {
                var shopkeeper = UnityEngine.Object.Instantiate(AssetTools.shopKeepers[index], scenery.transform);
                shopkeeper.shopPrefab.transform.parent = scenery.transform;
/*                if (index == 15)
                {
                    AddShopItems.MakeShopItem("shop item (999)", scenery.transform, new Vector3(-68f, 3.3f, 44.1f), new Vector3(2f, 142f, 359.8f), PrefabsDirectory.instance.directory[514]);

                }*/
                Debug.Log($"Windicators: Adding shopkeeper for island {index}");
            }
            else
            {
#if DEBUG
                Debug.LogWarning($"Windicators: No shopkeeper found for island {index}");
#endif
            }
        }
    }
}
