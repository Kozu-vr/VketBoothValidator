using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 02.ObjectSyncが設定されているオブジェクトが初期表示されているか検証します。
    /// </summary>
    public class ObjectSyncRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G02 ObjectSync Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public ObjectSyncRule(Options _options) : base(_options)
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
            if (boothObjects != null)
            {
                foreach (GameObject obj in boothObjects)
                {
                    Component[] cmps = obj.GetComponents(typeof(MonoBehaviour));

                    foreach (Component cmp in cmps)
                    {
                        if (cmp != null && cmp.GetType().FullName.IndexOf("_ObjectSync") >= 0 && cmp.gameObject.activeInHierarchy == false)
                        {
                            dirtFlg = true;
                            AddResultLog("ブース内に非表示のObjetSyncがあります");
                            AddResultLog(cmp.gameObject.name);
                        }
                        if(cmp!= null && cmp.GetType().FullName == "VRCSDK2.VRC_ObjectSync" && cmp.gameObject.name != "Pickup")
                        {
                            dirtFlg = true;
                            AddResultLog("公式プレハブ以外でObjectSyncを使用している可能性があります。運営に相談済みの場合はこのメッセージを無視してください。");
                        }
                    }
                }
            }

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(dirtFlg ? Result.FAIL : Result.SUCCESS);
        }
    }
}