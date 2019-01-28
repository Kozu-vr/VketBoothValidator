using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// Y.よくある入稿ミス
    /// 01.Animatorで動かすオブジェクトはDynamic以外に配置しないこと
    /// </summary>
    public class AnimationObjectHierarchyRule : BaseRule
    {
        //ルール名
        public new string ruleName = "Y01 AnimationObjectHierarchy Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public AnimationObjectHierarchyRule(Options _options) : base(_options)
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
            GameObject occluderStatic = null;
            GameObject occludeeStatic = null;

            GameObject boothRoot = Utils.GetInstance().GetRootBoothObject();
            if (boothRoot != null)
            {
                if (boothRoot.transform.Find("Occluder Static"))
                {
                    occluderStatic = boothRoot.transform.Find("Occluder Static").gameObject;
                }
                if (boothRoot.transform.Find("Occludee Static"))
                {
                    occludeeStatic = boothRoot.transform.Find("Occludee Static").gameObject;
                }

                foreach (GameObject obj in boothObjects)
                {
                    AnimationClip[] clips = AnimationUtility.GetAnimationClips(obj);
                    if (clips.Length > 0)
                    {
                        //boothRootの場合
                        if (obj.transform.parent == null)
                        {
                            foreach (var clip in clips)
                            {
                                foreach (var binding in AnimationUtility.GetCurveBindings(clip))
                                {
                                    if (binding.path.StartsWith("Occluder Static/") || binding.path.StartsWith("Occludee Static/"))
                                    {
                                        dirtFlg = true;
                                        AddResultLog(string.Format(string.Format("アニメーションではDynamic以下のオブジェクト以外は動かせません:{0}", obj.name)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            GameObject go = obj;
                            while (go.transform.parent != null)
                            {
                                if ((occluderStatic != null && go == occluderStatic) || (occludeeStatic != null && go == occludeeStatic))
                                {
                                    dirtFlg = true;
                                    AddResultLog(string.Format("アニメーションは{0}以下では使用できません。Dynamicに移動してください。:{1}", go.name, obj.name));
                                }
                                go = go.transform.parent.gameObject;
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
