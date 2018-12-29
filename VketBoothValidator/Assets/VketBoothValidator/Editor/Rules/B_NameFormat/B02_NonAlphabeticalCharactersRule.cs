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
    /// 02:全角禁止
    /// Ascii文字以外が入っていると違反とみなします。
    /// </summary>
    public class NonAlphabeticalCharactersRule : BaseRule
    {
        public new string ruleName = "B02:全角禁止 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public NonAlphabeticalCharactersRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            Result result;
            int expectedCount = 0;

            string[] guids1 = AssetDatabase.FindAssets("t:Object", new[] { AssetDatabase.GetAssetPath(options.baseFolder) });
            IEnumerable<string> dictinctGuids = guids1.Distinct();
            Regex reg = new Regex(@"^[\x21-\x7e ]+$");
            string assetPath;
            List<string> invalidPath = new List<string>();
            foreach (string guid in dictinctGuids)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!reg.IsMatch(assetPath))
                {
                    invalidPath.Add(assetPath);
                }
            }

            AddResultLog("全角文字使用アセット数：" + invalidPath.Count);
            foreach (string path in invalidPath.ToArray())
            {
                ResultLog += System.Environment.NewLine + path;
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