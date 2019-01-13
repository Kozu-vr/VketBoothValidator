using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// Z.公式プレハブのチェック
    /// 02. ObjectSwitchプレハブのオーバーライド用のファイル複製チェック
    /// </summary>
    public class ObjectSwitchRule : BaseRule
    {
        //ルール名
        public new string ruleName = "Z02 ObjectSwitch Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public ObjectSwitchRule(Options _options) : base(_options)
        {
        }

        //検証メソッド
        public override Result Validate()
        {
            //初期化
            base.Validate();

            //検証ロジック
            GameObject[] boothObjects = Utils.GetInstance().FindAllObjectsInBooth();
            bool dirtFlg = false;
            string originalControllerGuid = "f9b823b9c8e35064f8c88856a042e491";
            if (boothObjects != null)
            {
                foreach (GameObject obj in boothObjects)
                {
                    Animator[] cmps = obj.GetComponents<Animator>();

                    foreach (Animator cmp in cmps)
                    {
                        string controllerGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(cmp.runtimeAnimatorController));
                        if (controllerGuid == originalControllerGuid)
                        {
                            dirtFlg = true;
                            AddResultLog(string.Format("アニメーションコントローラー{0}が複製されないまま使われています。", cmp.runtimeAnimatorController.name));
                        }
                    }
                }
            }

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(!dirtFlg ? Result.SUCCESS : Result.FAIL);
        }
    }
}