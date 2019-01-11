using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 07.AnimatorがVRC_PickupおよびVRC_ObjectSyncと併用されていないこと、使用Animationで「../」が使用されていないかとを検証。
    /// </summary>
    public class AnimatorRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G07 Animator Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public AnimatorRule(Options _options) : base(_options)
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
                    //AnimationClipの「../」使用検証
                    AnimationClip[] clips = AnimationUtility.GetAnimationClips(obj);
                    if (clips.Length > 0)
                    {
                        if (findFlg == false)
                        {
                            findFlg = true;
                            AddResultLog("ブース内のAnimator,Animation：");
                        }
                        AddResultLog(string.Format(" {0}", obj.name));
                        foreach (var clip in clips)
                        {
                            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
                            {
                                if (binding.path.StartsWith("../"))
                                {
                                    dirtFlg = true;
                                    AddResultLog("  Animationのパスに「../」は使用できません。");
                                }
                            }
                        }
                    }

                    Animator[] animators = obj.GetComponents<UnityEngine.Animator>();
                    if (animators.Length > 0)
                    {
                        //併用コンポーネントの検証
                        Component[] cmps = obj.GetComponents(typeof(MonoBehaviour));
                        foreach (Component cmp in cmps)
                        {
                            if (cmp != null && cmp.GetType().FullName == "VRCSDK2.VRC_Pickup")
                            {
                                dirtFlg = true;
                                AddResultLog("  AnimatorとVRC_Pickupは同一オブジェクトで併用できません。");
                            }
                            if (cmp != null && cmp.GetType().FullName == "VRCSDK2.VRC_ObjectSync")
                            {
                                dirtFlg = true;
                                AddResultLog("  AnimatorとVRC_ObjectSyncpは同一オブジェクトで併用できません。");
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
