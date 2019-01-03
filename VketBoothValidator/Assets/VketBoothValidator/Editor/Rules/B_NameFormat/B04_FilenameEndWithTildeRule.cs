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
    /// 04:ファイル、フォルダ名の終わりの「~」禁止
    /// </summary>
    public class FilenameEndWithTildeRule : BaseRule
    {
        public new string ruleName = "B04:ファイル、フォルダ名の終わりの「~」禁止 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public FilenameEndWithTildeRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            Result result;
            string prohibitedCharacter = "~";
            int expectedCount = 0;

            string[] guids = AssetDatabase.FindAssets("t:Object", new[] { AssetDatabase.GetAssetPath(options.baseFolder) });
            IEnumerable<string> dictinctGuids = guids.Distinct();
            string assetPath;
            List<string> invalidPath = new List<string>();
            foreach (string guid in dictinctGuids)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileName(assetPath).EndsWith(prohibitedCharacter) ||
                    Path.GetFileNameWithoutExtension(assetPath).EndsWith(prohibitedCharacter))
                {
                    invalidPath.Add(assetPath);
                }
            }
            AddResultLog(string.Format("名前の末尾に'{0}'が含まれるアセット：{1}", prohibitedCharacter, invalidPath.Count));
            foreach (string path in invalidPath.ToArray())
            {
                AddResultLog(path);
            }

            if (invalidPath.Count == expectedCount)
            {
                result = Result.SUCCESS;
            }
            else
            {
                result = Result.FAIL;
            }
            return SetResult(result);
        }
    }
}