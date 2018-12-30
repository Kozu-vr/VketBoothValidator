using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace VketTools
{
    /// <summary>
    /// D.ブース規定
    /// 01.ブース寸法は幅4m×奥行き3m×高さ5m
    /// 初期状態でアクティブなオブジェクトのRenderer.boundsが(X,Y,Z)=(4,5,3)以内にあることを検証する
    /// ルートオブジェクトがブースに含まれないオブジェクトは無視
    /// </summary>
    public class BoothSizeRule : BaseRule
    {
        //ルール名
        public new string ruleName = "Ð01.ブースサイズ Rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public BoothSizeRule(Options _options) : base(_options)
        {
        }

        //検証メソッド
        public override Result Validate()
        {
            //初期化
            base.Validate();
            Vector3 maxBoundsSize = new Vector3(4, 5, 3);
            //検証ロジック

            Scene scene = SceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(options.sceneGuid));
            if (!scene.IsValid())
            {
                AddResultLog("無効なシーンです");
                return SetResult(Result.FAIL);
            }
            Transform[] allTransforms = Object.FindObjectsOfType<Transform>();
            List<Renderer> allRenderers = new List<Renderer>();
            foreach (Transform tr in allTransforms)
            {
                if (Utils.GetInstance().isBoothObject(tr.gameObject))
                {
                    allRenderers.AddRange(tr.GetComponents<Renderer>());
                }
            }
            Bounds boothBounds = new Bounds();
            if (allRenderers.Count > 0)
            {
                boothBounds = allRenderers[0].bounds;
            }
            foreach (Renderer renderer in allRenderers)
            {
                Bounds child_bounds = renderer.bounds;
                boothBounds.Encapsulate(child_bounds);
            }

            AddResultLog("ブースのサイズ:" + boothBounds.size.ToString("f3"));
            bool dirtFlg = false;
            if (boothBounds.size.x > maxBoundsSize.x)
            {
                AddResultLog(string.Format("幅(X)が{0}を{1:0.######}超えています。", maxBoundsSize.x, boothBounds.size.x - maxBoundsSize.x));
                dirtFlg = true;
            }
            if (boothBounds.size.y > maxBoundsSize.y)
            {
                AddResultLog(string.Format("高さ(Y)が{0}を{1:0.######}超えています。", maxBoundsSize.y, boothBounds.size.y - maxBoundsSize.y));
                dirtFlg = true;
            }
            if (boothBounds.size.z > maxBoundsSize.z)
            {
                AddResultLog(string.Format("奥行(Z)が{0}を{1:0.######}超えています。", maxBoundsSize.z, boothBounds.size.z - maxBoundsSize.z));
                dirtFlg = true;
            }

            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(dirtFlg ? Result.FAIL : Result.SUCCESS);
        }
    }
}