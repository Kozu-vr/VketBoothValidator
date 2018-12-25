using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using System.IO;

namespace VketTools
{
    /// <summary>
    /// A.提出形式
    /// 01:Unity 2017.4.15f1で作成すること
    /// </summary>
    public class UnityVersionRule : BaseRule
    {
        public new string ruleName = "A01:Unity version Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }

        public UnityVersionRule(Options _options) : base(_options)
        {
        }

        public override Result Validate()
        {
            base.Validate();
            Result result;
            string actualVer = Application.unityVersion;
            string expectedVer = "2017.4.15f1";
            AddResultLog("Unity version:" + actualVer);
            if (Application.unityVersion == expectedVer)
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