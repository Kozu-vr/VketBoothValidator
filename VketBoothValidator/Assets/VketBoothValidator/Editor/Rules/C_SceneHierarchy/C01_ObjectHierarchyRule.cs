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
    /// 01.Occluder Static, Occludee Static, Dynamicの３つのEmptyオブジェクトを作ること
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
            int OccluderStaticCount = 0;
            int OccludeeStaticCount = 0;
            int DynamicCount = 0;
            int otherCount = 0;
            List<string> otherObjectName = new List<string>();
            foreach (GameObject obj in rootObjects)
            {
                switch (obj.name)
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
                        otherCount++;
                        otherObjectName.Add(obj.name);
                        break;
                }
            }
            bool dirflg = false;
            if (OccluderStaticCount == 0)
            {
                AddResultLog("シーンに'Occluder Static'がありません。");
                dirflg = true;
            }
            else if (OccluderStaticCount > 1)
            {
                AddResultLog("シーンに'Occluder Static'が複数あります。");
                dirflg = true;
            }
            if (OccludeeStaticCount == 0)
            {
                AddResultLog("シーンに'Occludee Static'がありません。");
                dirflg = true;
            }
            else if (OccludeeStaticCount > 1)
            {
                AddResultLog("シーンに'Occludee Static'が複数あります。");
                dirflg = true;
            }
            if (DynamicCount == 0)
            {
                AddResultLog("シーンに'Dynamic'がありません。");
                dirflg = true;
            }
            else if (DynamicCount > 1)
            {
                AddResultLog("シーンに'Dynamic'が複数あります。");
                dirflg = true;
            }
            if(otherCount > 0)
            {
                AddResultLog("シーン内の次のオブジェクトはブースに含まれないです。");
                foreach(string name in otherObjectName)
                {
                    AddResultLog(" "+name);
                }
            }

            result = dirflg? Result.FAIL:Result.SUCCESS;
            return SetResult(result);
        }
    }
}