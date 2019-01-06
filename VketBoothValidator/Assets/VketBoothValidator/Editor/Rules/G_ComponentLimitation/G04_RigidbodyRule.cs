using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 04.Rigidbody がIs Kinematic、Collision DetectionがDiscreteであることを検証。
    /// </summary>
    public class RigidbodyRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G04 Rigidbody Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public RigidbodyRule(Options _options) : base(_options)
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
                    Rigidbody[] cmps = obj.GetComponents<UnityEngine.Rigidbody>();

                    foreach (Rigidbody cmp in cmps)
                    {
                        if (cmp != null)
                        {
                            if (findFlg == false)
                            {
                                findFlg = true;
                                AddResultLog("ブース内のRigidbody：");
                            }
                            AddResultLog(string.Format(" {0}(is Kinematic:{1}, {2})", cmp.gameObject.name, cmp.isKinematic, cmp.collisionDetectionMode));
                            if (cmp.isKinematic == false)
                            {
                                AddResultLog("is KinematicではないRigidbodyです。運営に相談済みの場合はこのメッセージを無視してください。");
                                dirtFlg = true;
                            }
                            if (cmp.collisionDetectionMode != CollisionDetectionMode.Discrete)
                            {
                                AddResultLog("CollisionDetectionがDiscreteでないRigidbodyです。");
                                dirtFlg = true;
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