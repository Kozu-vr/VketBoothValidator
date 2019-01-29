using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// Z.公式プレハブのチェック
    /// 01.PickupObjectSyncPrefab（Animation版）のオーバーライド用のファイル複製チェック
    /// "Pickup"オブジェクトとその親のScaleが一致しているかチェック
    /// </summary>
    public class PickupObjectSyncPrefabRule : BaseRule
    {
        //ルール名
        public new string ruleName = "Z01 PickupObjectSyncPrefab Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public PickupObjectSyncPrefabRule(Options _options) : base(_options)
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
            string originalControllerGuid = "6893d5acfbed8d544856d8eb7cc11133";
            if (boothObjects != null)
            {
                foreach (GameObject obj in boothObjects)
                {
                    //ファイル複製チェック
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
                    //Scaleチェック
                    if (obj.name == "Pickup" && obj.transform.parent != null)
                    {
                        Component[] mbs = obj.GetComponents(typeof(MonoBehaviour));

                        foreach (Component cmp in mbs)
                        {
                            if (cmp != null && cmp.GetType().FullName.IndexOf("VRCSDK2.VRC_Pickup") >= 0)
                            {
                                Transform parentTransform = obj.transform.parent;

                                if (parentTransform.localScale != obj.transform.localScale)
                                {
                                    dirtFlg = true;
                                    AddResultLog(string.Format("公式プレハブの一番上の親オブジェクトと直下の'Pickup'はScaleが一致している必要があります。：{0}"
                                        , parentTransform.gameObject.name));
                                }
                            }
                        }
                    }
                }
            }

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(!dirtFlg ? Result.SUCCESS : Result.FAIL);
        }
    }
}