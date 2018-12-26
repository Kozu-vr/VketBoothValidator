using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

namespace VketTools
{
    /// <summary>
    /// C.Scene内階層形式規定
    /// 01.シーンルートにベースフォルダと同名のオブジェクト(ブースルートオブジェクト)がある事
    /// Occluder Static, Occludee Static, Dynamicの３つのEmptyオブジェクトを作ること
    /// すべてのオブジェクトはこのどれかの階層下に入れること
    /// その他のルートオブジェクトの情報をログ出力する（エラーにはしない）
    /// </summary>
    public class ObjectHierarchyRule : BaseRule
    {
        public new string ruleName = "C01:シーン階層構造 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public ObjectHierarchyRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            Result result;
            Scene scene = SceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(options.sceneGuid));
            if (!scene.IsValid())
            {
                AddResultLog("無効なシーンです");
                return SetResult(Result.FAIL);
            }
            GameObject[] rootObjects = scene.GetRootGameObjects();
            GameObject rootBoothObject = null;
            string baseFolderName = options.baseFolder.name;
            int rootBoothObjectCount = 0;
            int OccluderStaticCount = 0;
            int OccludeeStaticCount = 0;
            int DynamicCount = 0;
            int otherRootCount = 0;
            bool dirflg = false;
            List<string> otherRootObjectName = new List<string>();
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name == baseFolderName)
                {
                    rootBoothObject = obj;
                    rootBoothObjectCount++;
                }
                else
                {
                    otherRootCount++;
                    otherRootObjectName.Add(obj.name);
                }
            }
            if (rootBoothObjectCount == 0)
            {
                AddResultLog(string.Format("シーンのルートに{0}オブジェクトがありません。", baseFolderName));
                dirflg = true;
            }
            else
            {
                int otherChildCount = 0;
                List<string> otherChildObjectName = new List<string>();
                foreach (Transform child in rootBoothObject.transform)
                {
                    switch (child.gameObject.name)
                    {
                        case "Occluder Static":
                            OccluderStaticCount++;
                            break;
                        case "Occludee Static":
                            OccludeeStaticCount++;
                            break;
                        case "Dynamic":
                            DynamicCount++;
                            break;
                        default:
                            otherChildCount++;
                            otherChildObjectName.Add(child.gameObject.name);
                            break;
                    }
                }

                if (OccluderStaticCount == 0)
                {
                    AddResultLog(string.Format("{0}オブジェクトの直下に'Occluder Static'がありません。", baseFolderName));
                    dirflg = true;
                }
                else if (OccluderStaticCount > 1)
                {
                    AddResultLog(string.Format("{0}オブジェクトの直下に'Occluder Static'が複数あります。", baseFolderName));
                    dirflg = true;
                }
                if (OccludeeStaticCount == 0)
                {
                    AddResultLog(string.Format("{0}オブジェクトの直下に'Occludee Static'がありません。", baseFolderName));
                    dirflg = true;
                }
                else if (OccludeeStaticCount > 1)
                {
                    AddResultLog(string.Format("{0}オブジェクトの直下に'Occludee Static'が複数あります。", baseFolderName));
                    dirflg = true;
                }
                if (DynamicCount == 0)
                {
                    AddResultLog(string.Format("{0}オブジェクトの直下に'Dynamic'がありません。", baseFolderName));
                    dirflg = true;
                }
                else if (DynamicCount > 1)
                {
                    AddResultLog(string.Format("{0}オブジェクトの直下に'Dynamic'が複数あります。", baseFolderName));
                    dirflg = true;
                }
                if (otherChildCount > 0)
                {
                    AddResultLog(string.Format("{0}オブジェクトの直下に規定外のオブジェクトがあります", baseFolderName));
                    dirflg = true;
                    foreach (string name in otherChildObjectName)
                    {
                        AddResultLog(" " + name);
                    }
                }
            }
            if (otherRootCount > 0)
            {
                AddResultLog("シーン内の次のオブジェクトはブースに含まれないです。");
                foreach (string name in otherRootObjectName)
                {
                    AddResultLog(" " + name);
                }
            }
            result = dirflg ? Result.FAIL : Result.SUCCESS;
            return SetResult(result);
        }
    }
}