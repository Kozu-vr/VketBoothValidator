using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

namespace VketTools
{
    /// <summary>
    /// C.Scene内階層形式規定
    /// 02.Occluder Static, Occludee Static, Dynamicの３つのオブジェクトにStaticが適切に設定されている。
    /// 'Occluder Static'以下のオブジェクト設定が'Occluder Static'
    /// 'Occludee Static'以下のオブジェクト設定が'Occludee Static'に設定されている。
    /// 'Dynamic'以下ではどちらも設定されていない
    /// </summary>
    public class StaticObjectRule : BaseRule
    {
        public new string ruleName = "C01:Static設定 rule";
        public override string RuleName
        {
            get
            {
                return ruleName;
            }
        }
        public StaticObjectRule(Options _options) : base(_options)
        {
        }
        public override Result Validate()
        {
            base.Validate();
            Result result;
            Scene scene = SceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(options.sceneGuid));
            if (!scene.IsValid())
            {
                AddResultLog("無効なシーンです");
                return SetResult(Result.FAIL);
            }
            bool dirtflg = false;
            List<string> inValidObjectName = new List<string>();

            GameObject rootBoothObject = Utils.GetInstance().GetRootBoothObject();
            GameObject occluderStatic = null;
            GameObject occludeeStatic = null;
            GameObject dynamic = null;
            if (rootBoothObject == null)
            {
                AddResultLog("ルートオブジェクトがありません。");
                return SetResult(Result.FAIL);
            }
            foreach (Transform child in rootBoothObject.transform)
            {
                GameObject go = child.gameObject;
                switch (go.name)
                {
                    case "Occluder Static":
                        occluderStatic = go;
                        break;
                    case "Occludee Static":
                        occludeeStatic = go;
                        break;
                    case "Dynamic":
                        dynamic = go;
                        break;
                    default:
                        break;
                }
            }

            //'Occluder Static'以下のオブジェクト設定がすべて'Occluder Static'
            if (occluderStatic != null)
            {
                Transform[] childTransforms = occluderStatic.GetComponentsInChildren<Transform>();
                foreach (Transform transform in childTransforms)
                {
                    StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(transform.gameObject);
                    if ((flags & StaticEditorFlags.OccluderStatic) == 0)
                    {
                        dirtflg = true;
                        inValidObjectName.Add(transform.name);
                    }
                }
            }
            else
            {
                AddResultLog("Occluder Staticオブジェクトがありません。");
                dirtflg = true;
            }
            if (inValidObjectName.Count() > 0)
            {
                AddResultLog("以下のOccluder Staticの設定ができていません");
                foreach (string name in inValidObjectName)
                {
                    AddResultLog(" " + name);
                }
            }

            //'Occludee Static'以下のオブジェクト設定がすべて'Occludee Static'
            inValidObjectName = new List<string>();
            if (occludeeStatic != null)
            {
                Transform[] childTransforms = occludeeStatic.GetComponentsInChildren<Transform>();
                foreach (Transform transform in childTransforms)
                {
                    StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(transform.gameObject);
                    if ((flags & StaticEditorFlags.OccludeeStatic) == 0)
                    {
                        dirtflg = true;
                        inValidObjectName.Add(transform.name);
                    }
                }
            }
            else
            {
                AddResultLog("Occludee Staticオブジェクトがありません。");
                dirtflg = true;
            }
            if (inValidObjectName.Count() > 0)
            {
                AddResultLog("以下のOccludee Staticの設定ができていません");
                foreach (string name in inValidObjectName)
                {
                    AddResultLog(" " + name);
                }
            }
            //'Dynamic'以下のオブジェクト設定がすべて'Occluder Static','Occludee Static'ではない
            inValidObjectName = new List<string>();
            if (dynamic != null)
            {
                Transform[] childTransforms = dynamic.GetComponentsInChildren<Transform>();
                foreach (Transform transform in childTransforms)
                {
                    StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(transform.gameObject);
                    if ((flags & StaticEditorFlags.OccluderStatic) != 0 | (flags & StaticEditorFlags.OccludeeStatic) != 0)
                    {
                        dirtflg = true;
                        inValidObjectName.Add(transform.name);
                    }
                }
            }
            else
            {
                AddResultLog("Dynamicオブジェクトがありません。");
                dirtflg = true;
            }
            if (inValidObjectName.Count() > 0)
            {
                AddResultLog("Dynamic以下にはOccluder static,Occludee Staticを設定できません");
                foreach (string name in inValidObjectName)
                {
                    AddResultLog(" " + name);
                }
            }
            result = dirtflg ? Result.FAIL : Result.SUCCESS;
            return SetResult(result);
        }
    }
}


