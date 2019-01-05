using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace VketTools
{
    /// <summary>
    ///ユーティリティクラス
    /// </summary>
    public sealed class Utils
    {
        private static Utils _singleInstance = new Utils();
        public Options options { set; get; }

        public static Utils GetInstance()
        {
            return _singleInstance;
        }

        private Utils()
        {
        }

        public void setOptons(Options opt)
        {
            options = opt;
        }

        /// <summary>
        ///引数のTransformオブジェクトがブース内のものか調べる
        ///ブース内のものはbaseFolderと同じ名前のルートオブジェクトを持ち、シーンにロードされている。
        /// </summary>
        public bool isBoothObject(GameObject go)
        {
            if (options == null)
            {
                return false;
            }
            string rootName = go.transform.root.gameObject.name;
            bool isSameName = rootName == options.baseFolder.name;
            bool isInScence = AssetDatabase.GUIDToAssetPath(options.sceneGuid) == go.scene.path;
            return (isSameName && isInScence);
        }

        /// <summary>
        /// シーン内のルートオブジェクトのうち、ブースに含まれるオブジェクトを返す
        /// </summary>
        public GameObject GetRootBoothObject()
        {
            if (options == null)
            {
                return null;
            }
            string sceneGuid = options.sceneGuid;
            string baseFolderName = options.baseFolder.name;
            Scene scene = SceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(sceneGuid));
            if (!scene.IsValid())
            {
                return null;
            }

            GameObject[] rootObjects = scene.GetRootGameObjects();
            GameObject boothRootObject = null;
            foreach (GameObject go in rootObjects)
            {
                if (go.name == baseFolderName)
                {
                    boothRootObject = go;
                }
            }
            return boothRootObject;
        }

        /// <summary>
        /// ブースに含まれるすべてのオブジェクトを検索して返す
        /// </summary>
        public GameObject[] FindAllObjectsInBooth()
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