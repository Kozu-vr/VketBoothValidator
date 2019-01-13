using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 05.JointがObjectSyncと併用されていないことを検証。
    /// </summary>
    public class JointRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G05 Joint Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public JointRule(Options _options) : base(_options)
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
                    Joint[] cmps = obj.GetComponents<UnityEngine.Joint>();

                    foreach (Joint cmp in cmps)
                    {
                        if (cmp != null)
                        {
                            if (findFlg == false)
                            {
                                findFlg = true;
                                AddResultLog("ブース内のJoint：");
                            }
                            AddResultLog(string.Format(" {0}({1})", cmp.gameObject.name, cmp.GetType().Name));
                            MonoBehaviour[] siblings = cmp.gameObject.GetComponents<MonoBehaviour>();
                            foreach (MonoBehaviour mb in siblings)
                            {
                                if (mb.GetType().FullName == "VRCSDK2.VRC_ObjectSync")
                                {
                                    dirtFlg = true;
                                    AddResultLog("  ObjectSyncと併用されているJointがあります。");
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