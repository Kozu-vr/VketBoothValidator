using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace VketTools
{

    public class Utils
    {
        /// <summary>
        ///引数のTransformオブジェクトがブース内のものか調べる
        ///ブース内のものはrootがOccluder Static,Occludee Static,Dynamicのいずれかに属する
        /// </summary>
        public static bool isBoothObject(Transform tr)
        {
            string rootName = tr.root.gameObject.name;
            return (rootName == "Occluder Static" || rootName == "Occludee Static" || rootName == "Dynamic");
        }

        /// <summary>
        ///引数のTransformオブジェクトがブース内のものか調べる
        ///ブース内のものはrootがOccluder Static,Occludee Static,Dynamicのいずれかに属する
        /// </summary>
        public static bool isBoothObject(GameObject go)
        {
            string rootName = go.transform.root.gameObject.name;
            return (rootName == "Occluder Static" || rootName == "Occludee Static" || rootName == "Dynamic");
        }

        /// <summary>
        /// シーン内のルートオブジェクトのうち、ブースに含まれるオブジェクトの配列を返す
        /// </summary>
        public static GameObject[] getRootBoothObjects(string sceneGuid)
        {
            Scene scene = SceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(sceneGuid));
            if (!scene.IsValid())
            {
                return null;
            }

            GameObject[] rootObjects = scene.GetRootGameObjects();
            List<GameObject> boothRootObject = new List<GameObject>();
            foreach (GameObject go in rootObjects)
            {
                if (go.name == "Occluder Static" || go.name == "Occludee Static" || go.name == "Dynamic")
                {
                    boothRootObject.Add(go);
                }
            }
            return boothRootObject.ToArray();
        }

        /// <summary>
        /// ブースに含まれるすべてのオブジェクトを検索して返す
        /// </summary>
        public static GameObject[] FindAllObjectsInBooth()
        {
            GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            List<GameObject> boothObjects = new List<GameObject>();
            foreach (GameObject go in objects)
            {
                if (isBoothObject(go) && (go.hideFlags & HideFlags.HideAndDontSave) == 0)
                {
                    boothObjects.Add(go);
                }
            }
            return boothObjects.ToArray();
        }

    }

}