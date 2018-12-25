using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// Z.テンプレート用
    /// 99.ルールの概要
    /// </summary>
    public class TamplateRule : BaseRule
    {
        //ルール名
        public new string ruleName = "Z99:テンプレート用 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public TamplateRule(Options _options) : base(_options)
        {
        }

        //検証メソッド
        public override Result Validate()
        {
            //初期化
            base.Validate();

            //検証ロジック
            AddResultLog("テンプレートルールの実行");

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(Result.FAIL);
        }
    }
}