using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 06.Light がBakedであることを検証。
    /// </summary>
    public class LightRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G06 Light Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public LightRule(Options _options) : base(_options)
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
                    Light[] cmps = obj.GetComponents<UnityEngine.Light>();

                    foreach (Light cmp in cmps)
                    {
                        if (cmp != null)
                        {
                            if (findFlg == false)
                            {
                                findFlg = true;
                                AddResultLog("ブース内のLight：");
                            }
                            AddResultLog(string.Format(" {0}({1}, {2})", cmp.gameObject.name, cmp.type, cmp.lightmapBakeType));
                            if(cmp.lightmapBakeType != LightmapBakeType.Baked)
                            {
                                dirtFlg = true;
                            }
                        }
                    }
                }
            }
            bool result = true;
            if (dirtFlg)
            {
                result = false;
                AddResultLog("BakedではないLightがあります。運営に相談済みの場合はこのメッセージを無視してください。");
            }

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(result ? Result.SUCCESS : Result.FAIL);
        }
    }
}