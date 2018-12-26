using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace VketTools
{
    /// <summary>
    /// D.ブース規定
    /// 02.マテリアル数制限　10個以内
    /// ブース内に設置するアバターのサンプル等全てを含み10個以内
    /// 初期表示でアクティブなオブジェクトのみが対象
    /// ルートオブジェクトがブースに含まれないオブジェクトは無視
    /// ベースフォルダ外のマテリアルを参照している場合はエラー
    /// </summary>
    public class NumberOfMaterialsRule : BaseRule
    {
        //ルール名
        public new string ruleName = "Ð02.マテリアル数制限 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public NumberOfMaterialsRule(Options _options) : base(_options)
        {
        }

        //検証メソッド
        public override Result Validate()
        {
            //初期化
            base.Validate();
            int maxMaterialCount = 10;
            //検証ロジック
            Scene scene = SceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(options.sceneGuid));
            if (!scene.IsValid())
            {
                AddResultLog("無効なシーンです");
                return SetResult(Result.FAIL);
            }
            Transform[] allTransforms = Object.FindObjectsOfType<Transform>();
            List<Renderer> allRenderers = new List<Renderer>();
            List<Material> allMaterials = new List<Material>();
            foreach (Transform tr in allTransforms)
            {
                if (Utils.GetInstance().isBoothObject(tr.gameObject))
                {
                    allRenderers.AddRange(tr.GetComponents<Renderer>());
                }
            }
            foreach (Renderer renderer in allRenderers)
            {
                allMaterials.AddRange(renderer.sharedMaterials);
                if (renderer.sharedMaterials.Contains(null))
                {
                    AddResultLog("Missingまたは未割当のマテリアルがみつかりました。:" + renderer.gameObject.name);
                }
            }
            IEnumerable<Material> dictinctMaterials = allMaterials.Distinct();
            bool dirtFlg = false;
            AddResultLog("アクティブな使用マテリアル:");
            foreach (Material material in dictinctMaterials)
            {
                if (material == null)
                {
                    dirtFlg = true;
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(material.GetInstanceID());
                    if (assetPath.IndexOf(AssetDatabase.GetAssetPath(options.baseFolder)) == -1)
                    {
                        AddResultLog("ベースフォルダに含まれないマテリアルを参照しています。");
                        dirtFlg = true;
                    }
                    AddResultLog(" " + assetPath);
                }
            }
            AddResultLog("シーン内マテリアル数：" + dictinctMaterials.Count());
            if (dictinctMaterials.Count() > maxMaterialCount)
            {
                AddResultLog(string.Format("マテリアル数が{0}を超えています。", maxMaterialCount));
                dirtFlg = true;
            }
            //検証結果を設定して返す(正常：Result.SUCESS 異常：Result.FAIL)
            return SetResult(dirtFlg ? Result.FAIL : Result.SUCCESS);
        }
    }
}