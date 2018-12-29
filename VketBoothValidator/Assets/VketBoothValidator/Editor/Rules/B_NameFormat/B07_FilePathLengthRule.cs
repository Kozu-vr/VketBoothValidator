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
    /// 07:ファイルパス長制限
    /// Assetsフォルダ以下のパス長が180文字以下であることをチェック
    /// </summary>
    public class FilePathLengthRule : BaseRule
    {
        public new string ruleName = "B07:ファイルパス長制限 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public FilePathLengthRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            Result result;
            int expectedMaxLength = 180;
            int expectedCount = 0;

            string[] guids1 = AssetDatabase.FindAssets("t:Object", new[] { AssetDatabase.GetAssetPath(options.baseFolder) });
            IEnumerable<string> dictinctGuids = guids1.Distinct();
            string assetPath;
            List<string> invalidPath = new List<string>();
            foreach (string guid in dictinctGuids)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.Length > expectedMaxLength)
                {
                    invalidPath.Add(assetPath);
                }
            }

            AddResultLog("180文字を超える長いパス：" + invalidPath.Count);
            foreach (string path in invalidPath.ToArray())
            {
                ResultLog += System.Environment.NewLine+ path;
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