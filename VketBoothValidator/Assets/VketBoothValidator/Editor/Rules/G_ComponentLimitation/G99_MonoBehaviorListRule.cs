using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 99.ブース内で使用中のMonoBehaviorを一覧出力する
    /// Missingのものもログに出力する。
    /// </summary>
    public class MonoBehaviorListRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G99:スクリプト一覧出力Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public MonoBehaviorListRule(Options _options) : base(_options)
        {
        }

        //検証メソッド
        public override Result Validate()
        {
            //初期化
            base.Validate();

            //検証ロジック
            GameObject[] boothObjects = Utils.GetInstance().FindAllObjectsInBooth();
            bool findFlg = false;
            bool dirtFlg = false;
            if (boothObjects != null)
            {
                foreach (GameObject obj in boothObjects)
                {
                    Component[] cmps = obj.GetComponents(typeof(MonoBehaviour));

                    foreach (Component cmp in cmps)
                    {
                        if (findFlg == false)
                        {
                            findFlg = true;
                            AddResultLog("ブース内のMonoBehavior：");
                        }
                        if (cmp != null)
                        {
                            string cmpInfo = string.Format(" {0} ({1})", cmp.gameObject.name, cmp.GetType().FullName);
                            AddResultLog(cmpInfo);
                        }
                        else
                        {
                            dirtFlg = true;
                            AddResultLog("MissingのScript（MonoBehaviour）が含まれています");
                        }
                    }
                }
            }

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(dirtFlg ? Result.FAIL : Result.SUCCESS);
        }
    }
}