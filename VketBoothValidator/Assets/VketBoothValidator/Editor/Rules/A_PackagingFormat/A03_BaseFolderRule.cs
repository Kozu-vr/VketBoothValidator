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
    /// 03:ベースフォルダの形式
    /// ベースフォルダがAssets直下であることと、「aaa_bbb」の形式になっていることを確認
    /// </summary>
    public class BaseFolderRule : BaseRule
    {
        public new string ruleName = "A03:ベースフォルダの形式 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public BaseFolderRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            Result result;
            int expectedCount = 0;
            string assetPath;
            List<string> invalidPath = new List<string>();
            string folderName = options.baseFolder.name;
            assetPath = AssetDatabase.GetAssetPath(options.baseFolder);
            //Check folder path
            string expectedPath = Path.Combine("Assets/", folderName);
            if (assetPath.IndexOf(expectedPath) != 0)
            {
                invalidPath.Add(assetPath);
                AddResultLog("ベースフォルダがAssets直下にありません。");
            }
            //Chck folder name fomart
            Regex regName = new Regex(@"[^_]+_[^_]+");
            if (!regName.IsMatch(folderName))
            {
                invalidPath.Add(assetPath);
                AddResultLog("フォルダー名が「サークル名_サークル主」になっていません。");
            }


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