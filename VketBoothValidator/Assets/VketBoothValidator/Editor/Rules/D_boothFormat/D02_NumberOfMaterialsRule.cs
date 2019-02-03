using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

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
            bool dirtFlg = false;
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

                foreach (Material material in renderer.sharedMaterials)
                {
                    if (renderer.GetType() != typeof(ParticleSystemRenderer) && material == null)
                    {
                        AddResultLog("次のオブジェクトにMissingまたは未割当のマテリアルがみつかりました。:" + renderer.gameObject.name);
                        dirtFlg = true;
                    }
                    else if (renderer.GetType() == typeof(ParticleSystemRenderer))
                    {
                        ParticleSystemRenderer psr = renderer as ParticleSystemRenderer;
                        //Materialの判定
                        if (psr.sharedMaterials[0] != null)
                        {
                            allMaterials.Add(psr.sharedMaterials[0]);
                        }
                        else
                        {
                            AddResultLog("次のオブジェクトにMissingまたは未割当のマテリアルがみつかりました。:" + renderer.gameObject.name);
                            dirtFlg = true;
                        }
                        //Trail Materialの判定
                        if (psr.sharedMaterials[1] != null)
                        {
                            allMaterials.Add(psr.sharedMaterials[1]);
                        }
                        //まとめて処理したので抜ける
                        break;
                    }
                    else if (material.shader.name == "Hidden/InternalErrorShader")
                    {
                        AddResultLog("エラーのあるシェーダーがマテリアルに設定されています。:" + renderer.gameObject.name);
                        dirtFlg = true;
                    }
                    else
                    {
                        allMaterials.Add(material);
                    }
                }
            }
            IEnumerable<Material> dictinctMaterials = allMaterials.Distinct();

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
                    if (assetPath == "Resources/unity_builtin_extra")
                    {
                        AddResultLog("Default-Materialが使用されています。意図した設定ですか？");
                    }
                    else
                    {
                        if (assetPath.IndexOf(AssetDatabase.GetAssetPath(options.baseFolder) + "/") == -1)
                        {
                            AddResultLog("ベースフォルダに含まれないマテリアルを参照しています。");
                            dirtFlg = true;
                        }
                        string log = string.Format("{0} ({1})", assetPath, material.shader.name);
                        if (Path.GetExtension(assetPath).ToLower() == ".fbx")
                        {
                            log += string.Format(" ({0})", material.name);
                        }
                        AddResultLog(" " + log);
                    }
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