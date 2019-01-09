using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace VketTools
{
    /// <summary>
    /// A.提出形式
    /// 06:提出フォルダ内に、フォルダ名と同名のブースのPrefabを作成している
    /// </summary>
    public class BoothPrefabRule : BaseRule
    {
        public new string ruleName = "A06:ブースPrefab化 rule";
        Result result;
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public BoothPrefabRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            string[] guids = AssetDatabase.FindAssets("t:prefab", new[] { AssetDatabase.GetAssetPath(options.baseFolder) });
            string prefabPath = "";
            int boothPrefabCount = 0;
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(assetPath) == options.baseFolder.name)
                {
                    prefabPath = assetPath;
                    boothPrefabCount++;
                }
            }
            if (boothPrefabCount == 0)
            {
                AddResultLog(string.Format("ベースフォルダ内にPrefab'{0}'がみつかりません。", options.baseFolder.name));
                result = Result.FAIL;
            }
            else if (boothPrefabCount == 1)
            {
                GameObject scencebooth = Utils.GetInstance().GetRootBoothObject();
                PropertyModification[] modification = PrefabUtility.GetPropertyModifications(scencebooth);
                if (scencebooth != null)
                {
                    GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
                    if (hasModification(scencebooth, modification) || hasAttachedChild(scencebooth) ||
                        hasComponentChange(scencebooth, prefab))
                    {
                        AddResultLog("シーン内のブースはプレハブから構成が変更されています。プレハブを最新に作り直してください。");
                        result = Result.FAIL;
                    }
                    else
                    {
                        result = Result.SUCCESS;
                    }
                }
            }
            else
            {
                AddResultLog(string.Format("ベースフォルダ内にPrefab'{0}'が複数あります。", options.baseFolder.name));
                result = Result.FAIL;
            }
            return SetResult(result);
        }
        /// <summary>
        /// GameObject間でコンポーネントの増減をチェックする。
        /// 子オブジェクトに対して再帰的にチェックする。
        /// true:コンポーネントに増減がある false:増減がない
        /// </summary>
        private bool hasComponentChange(GameObject scencebooth, GameObject prefab)
        {
            if (compareComponents(scencebooth, prefab))
            {
                return true;
            }
            else
            {
                //全ゲームオブジェクトについて
                for (int i = 0; i < scencebooth.transform.childCount; i++)
                {
                    if (hasComponentChange(scencebooth.transform.GetChild(i).gameObject, prefab.transform.GetChild(i).gameObject))
                    {
                        return true;
                    }
                }
                //なければfalseを返す。
                return false;
            }
        }

        private bool compareComponents(GameObject go1, GameObject go2)
        {
            //両方のコンポーネントをリスト化
            List<Component> go1Component = new List<Component>(go1.GetComponents<Component>());
            List<Component> go2Component = new List<Component>(go2.GetComponents<Component>());
            //シーン内オブジェクト側の全コンポーネントについてプレハブ側を探索
            foreach (Component cmp1 in go1Component)
            {
                //同名のコンポーネントを検索
                Component[] cmps = go2Component.Where(data => data.GetType() == cmp1.GetType()).ToArray<Component>();
                if (cmps.Length == 0)
                {
                    AddResultLog(string.Format("プレハブから追加されたコンポーネント:{0}", cmp1.GetType().Name));
                    //見つからなければ追加コンポーネント検知としてtrueを返す
                    return true;
                }
            }
            foreach (Component cmp2 in go2Component)
            {
                //同名のコンポーネントを検索
                Component[] cmps = go1Component.Where(data => data.GetType() == cmp2.GetType()).ToArray<Component>();
                if (cmps.Length == 0)
                {
                    AddResultLog(string.Format("プレハブから削除されたコンポーネント:{0}", cmp2.GetType().Name));
                    //見つからなければ削除コンポーネント検知としてtrueを返す
                    return true;
                }
            }
            //何も検知しなかったらfalse
            return false;
        }


        //ブースにprefabの構成物以外の子オブジェクトが追加されているかチェックする。
        private bool hasAttachedChild(GameObject scencebooth)
        {
            GameObject[] boothObjects = Utils.GetInstance().FindAllObjectsInBooth();
            foreach (GameObject gm in boothObjects)
            {
                if (PrefabUtility.FindPrefabRoot(gm) != PrefabUtility.FindPrefabRoot(scencebooth))
                {
                    AddResultLog("プレハブに含まれないオブジェクト:" + gm.name);
                    return true;
                }
            }
            return false;
        }

        //prefabの構成オブジェクトに変更が加えられているかチェックする。
        private bool hasModification(GameObject booth, PropertyModification[] modification)
        {
            List<string> propertyString = new List<string> { "m_LocalPosition.x", "m_LocalPosition.y", "m_LocalPosition.z", "m_LocalRotation.x", "m_LocalRotation.y", "m_LocalRotation.z", "m_LocalRotation.w", "m_RootOrder" };
            bool dirtFlg = false;
            foreach (PropertyModification pm in modification)
            {
                if (pm.target.name == booth.name)
                {
                    //必ず含まれるプロパティか調べる
                    if (propertyString.IndexOf(pm.propertyPath) == -1)
                    {
                        dirtFlg = true;
                    }
                }
                else
                {
                    dirtFlg = true;
                }
            }
            return dirtFlg;
        }
    }
}