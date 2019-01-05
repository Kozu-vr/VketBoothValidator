using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;

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
            int boothPrefabCount = 0;
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(assetPath) == options.baseFolder.name)
                {
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
                    if (hasModification(scencebooth, modification) || hasAttachedChild(scencebooth) ||
                        hasAddedComponent(scencebooth) || hasRemovedComponent(scencebooth))
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

        private bool hasRemovedComponent(GameObject scencebooth)
        {
            throw new NotImplementedException();
        }

        private bool hasAddedComponent(GameObject scencebooth)
        {
            throw new NotImplementedException();
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