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
    /// B.ファイル&フォルダ名規定
    /// 08:「.blend」形式のファイル禁止
    /// </summary>
    public class BlenderFileRule : BaseRule
    {
        public new string ruleName = "B08:Blenderファイル rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public BlenderFileRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            string prohibitedExtentionRegex = ".blend[0-9]*";
            int expectedCount = 0;

            string[] guids = AssetDatabase.FindAssets("t:Object", new[] { AssetDatabase.GetAssetPath(options.baseFolder) });
            IEnumerable<string> dictinctGuids = guids.Distinct();
            string assetPath;
            List<string> invalidPath = new List<string>();
            Regex regex = new Regex(prohibitedExtentionRegex);
            foreach (string guid in dictinctGuids)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!AssetDatabase.IsValidFolder(assetPath) && regex.IsMatch(Path.GetExtension(assetPath)))
                {
                    invalidPath.Add(assetPath);
                }
            }
            if (invalidPath.Count > 0)
            {
                AddResultLog(string.Format("Belnder形式のモデルは使用できません。FBX形式に変換してください。"));
                foreach (string path in invalidPath.ToArray())
                {
                    AddResultLog(" " + path);
                }
            }

            return SetResult(invalidPath.Count == expectedCount ? Result.SUCCESS : Result.FAIL);
        }
    }
}