using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 03.Pickup が３つ以内であることを検証します。
    /// </summary>
    public class PickupObjectRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G03 Pickup Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public PickupObjectRule(Options _options) : base(_options)
        {
        }

        //検証メソッド
        public override Result Validate()
        {
            //初期化
            base.Validate();
            int maxCount = 3;
            //検証ロジック
            GameObject[] boothObjects = Utils.GetInstance().FindAllObjectsInBooth();
            bool findFlg = false;
            int count = 0;
            if (boothObjects != null)
            {
                foreach (GameObject obj in boothObjects)
                {
                    Component[] cmps = obj.GetComponents(typeof(MonoBehaviour));

                    foreach (Component cmp in cmps)
                    {
                        if (cmp != null && cmp.GetType().FullName.IndexOf("_Pickup") >= 0)
                        {
                            if (findFlg == false)
                            {
                                findFlg = true;
                                AddResultLog("ブース内のPickup Object：");
                            }
                            AddResultLog(cmp.gameObject.name);
                            count++;
                        }
                    }
                }
            }
            bool result = true;
            if (count > maxCount)
            {
                result = false;
                AddResultLog(string.Format("ブース内のPickup Objectが{0}を超えています。運営に相談済の場合はこのエラーを無視してください。", maxCount));
            }

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(result ? Result.SUCCESS : Result.FAIL);
        }
    }
}