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
                result = Result.SUCCESS;
            }
            else
            {
                AddResultLog(string.Format("ベースフォルダ内にPrefab'{0}'が複数あります。", options.baseFolder.name));
                result = Result.FAIL;
            }
            return SetResult(result);
        }
    }
}