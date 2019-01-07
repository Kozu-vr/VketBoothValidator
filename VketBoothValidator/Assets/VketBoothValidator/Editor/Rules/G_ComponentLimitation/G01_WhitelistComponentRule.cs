using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VketTools
{
    /// <summary>
    /// G.Component規定
    /// 01.コンポーネントが使用可能リストにあるか検証する。
    /// </summary>
    public class WhitelistComponentRule : BaseRule
    {
        //ルール名
        public new string ruleName = "G01 使用可能コンポーネント Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public WhitelistComponentRule(Options _options) : base(_options)
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
                    Component[] cmps = obj.GetComponents(typeof(Component));

                    foreach (Component cmp in cmps)
                    {
                        if (cmp != null)
                        {
                            bool findFlg = false;
                            foreach (WhitelistComponetReference reference in ComponentUtils.GetInstance().whitelistComponetReferences)
                            {
                                if (reference.Match(cmp))
                                {
                                    findFlg = true;
                                    if (reference.whitelistClass == WhitelistClass.ALLOW)
                                    {
                                        //AddResultLog(cmp.name + "(" + reference.name + ") は使用できるコンポーネントです。");
                                    }
                                    else if (reference.whitelistClass == WhitelistClass.NEGOTIABLE)
                                    {
                                        dirtFlg = true;
                                        AddResultLog(cmp.name + "(" + reference.name + ") は要相談コンポーネントです。運営に相談済みの場合はこのメッセージを無視してください。");
                                    }
                                    else
                                    {
                                        dirtFlg = true;
                                        AddResultLog(cmp.name + "(" + reference.name + ") は使用が許可されていないコンポーネントです。");
                                    }
                                }
                                else if (reference.MatchDependedComponent(cmp))
                                {
                                    findFlg = true;
                                    //AddResultLog(cmp.name + "(" + cmp.GetType().FullName + ") は" + reference.name + "の依存コンポーネントです。");
                                }
                            }
                            if (!findFlg)
                            {
                                dirtFlg = true;
                                AddResultLog(cmp.name + "(" + cmp.GetType().FullName + ") は使用可能コンポーネント一覧に入っていません。");
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