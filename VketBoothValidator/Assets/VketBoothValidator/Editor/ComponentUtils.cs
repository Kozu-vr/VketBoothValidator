using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VketTools
{
    /// <summary>
    ///使用可能コンポーネントの判定
    /// ALLOW：許可
    /// NEGOTIABLE：要相談
    /// DISALLOW：禁止
    /// </summary>
    public enum WhitelistClass
    {
        ALLOW,
        NEGOTIABLE,
        DISALLOW
    }

    /// <summary>
    ///コンポーネント検証のためのutilityクラス
    /// </summary>
    public class ComponentUtils
    {
        private static ComponentUtils _singleInstance = new ComponentUtils();
        public WhitelistComponetReference[] whitelistComponetReferences;

        public static ComponentUtils GetInstance()
        {
            return _singleInstance;
        }

        private ComponentUtils()
        {
            whitelistComponetReferences = new WhitelistComponetReference[]
            {
                new WhitelistComponetReference("VRC_Trigger",new string[]{"VRCSDK2.VRC_Trigger"},new string[]{"VRCSDK2.VRC_EventHandler"},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_Object Sync",new string[]{"VRCSDK2.VRC_ObjectSync"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_Pickup",new string[]{"VRCSDK2.VRC_Pickup"},new string[]{"UnityEngine.Rigidbody"},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_Tutorial Area Marker",new string[]{"VRCSDK2.VRC_TutorialAreaMarker"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_Audio Bank",new string[]{"VRCSDK2.VRC_AudioBank"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_Avatar Pedestal",new string[]{"VRCSDK2.VRC_AvatarPedestal"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Rigidbody",new string[]{"UnityEngine.Rigidbody"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Cloth",new string[]{"UnityEngine.Cloth"},new string[]{"UnityEngine.SkinnedMeshRenderer"},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Joint",new string[]{"UnityEngine.CharacterJoint","UnityEngine.ConfigurableJoint","UnityEngine.FixedJoint","UnityEngine.HingeJoint","UnityEngine.SpringJoint"},new string[] { "UnityEngine.Rigidbody" },WhitelistClass.ALLOW),
                new WhitelistComponetReference("Constant Force", new string[] { "UnityEngine.ConstantForce" }, new string[] { "UnityEngine.Rigidbody" }, WhitelistClass.ALLOW),
                new WhitelistComponetReference("Collider",new string[]{"UnityEngine.SphereCollider","UnityEngine.BoxCollider","UnityEngine.SphereCollider","UnityEngine.CapsuleCollider","UnityEngine.MeshCollider","UnityEngine.WheelCollider"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Dynamic Bone",new string[]{"DynamicBone"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Dynamic Bone Collider",new string[]{"DynamicBoneCollider"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Skinned Mesh Renderer",new string[]{"UnityEngine.SkinnedMeshRenderer"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Mesh Renderer ",new string[]{"UnityEngine.MeshRenderer"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Mesh Filter",new string[]{"UnityEngine.MeshFilter"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Particle System",new string[]{"UnityEngine.ParticleSystem"},new string[]{"UnityEngine.ParticleSystemRenderer"},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Trail Renderer",new string[]{"UnityEngine.TrailRenderer"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Line Renderer",new string[]{"UnityEngine.LineRenderer"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Light",new string[]{"UnityEngine.Light"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Animator",new string[]{"UnityEngine.Animator"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Animation",new string[]{"UnityEngine.Animation"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Audio Source",new string[]{"UnityEngine.AudioSource"},new string[]{"ONSPAudioSource"},WhitelistClass.ALLOW),
                new WhitelistComponetReference("Canvas",new string[]{"UnityEngine.Canvas"},new string[]{"UnityEngine.RectTransform","UnityEngine.UI.CanvasScaler","UnityEngine.UI.GraphicRaycaster"},WhitelistClass.ALLOW),
                new WhitelistComponetReference("uGUI+VRC_Ui Shape",new string[]{"VRCSDK2.VRC_UiShape","UnityEngine.UI.Text","UnityEngine.UI.Image","UnityEngine.UI.RawImage","UnityEngine.UI.Mask","UnityEngine.UI.RectMask2D","UnityEngine.UI.Button","UnityEngine.UI.InputField","UnityEngine.UI.Toggle","UnityEngine.UI.ToggleGroup","UnityEngine.UI.Slider","UnityEngine.UI.Scrollbar","UnityEngine.UI.Dropdown","UnityEngine.UI.ScrollRect","UnityEngine.UI.Selectable","UnityEngine.UI.Shadow","UnityEngine.UI.Outline","UnityEngine.UI.PositionAsUV1"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_Panorama",new string[]{"VRCSDK2.scripts.Scenes.VRC_Panorama"},new string[]{"VRCSDK2.VRC_DataStorage"},WhitelistClass.NEGOTIABLE),
                new WhitelistComponetReference("Camera",new string[]{"UnityEngine.Camera"},new string[]{""},WhitelistClass.NEGOTIABLE),
                new WhitelistComponetReference("Video Player",new string[]{"UnityEngine.Video.VideoPlayer"},new string[]{""},WhitelistClass.NEGOTIABLE),
                new WhitelistComponetReference("VRC_Station",new string[]{"VRCSDK2.VRC_Station"},new string[]{""},WhitelistClass.NEGOTIABLE),
                new WhitelistComponetReference("VRC_Mirror",new string[]{"VRCSDK2.VRC_MirrorReflection"},new string[]{""},WhitelistClass.NEGOTIABLE),
                new WhitelistComponetReference("Transform",new string[]{"UnityEngine.Transform"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("ParticleSystemRenderer",new string[]{"UnityEngine.ParticleSystemRenderer"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("ONSPAudioSource",new string[]{"ONSPAudioSource"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("RectTransform",new string[]{"UnityEngine.RectTransform"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_DataStorage",new string[]{"VRCSDK2.VRC_DataStorage"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_EventHandler",new string[]{"VRCSDK2.VRC_EventHandler"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("CanvasRenderer",new string[]{"UnityEngine.CanvasRenderer"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("CanvasScaler",new string[]{"UnityEngine.UI.CanvasScaler"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("GraphicRaycaster",new string[]{"UnityEngine.UI.GraphicRaycaster"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("ReflectionProbe",new string[]{"UnityEngine.ReflectionProbe"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("LightProbeGroup",new string[]{"UnityEngine.LightProbeGroup"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("EventSystem",new string[]{"UnityEngine.EventSystems.EventSystem"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("StandaloneInputModule",new string[]{"UnityEngine.EventSystems.StandaloneInputModule"},new string[]{""},WhitelistClass.ALLOW),
                new WhitelistComponetReference("VRC_SceneResetPosition",new string[]{"VRCSDK2.VRC_SceneResetPosition"},new string[]{""},WhitelistClass.ALLOW),
            };
        }
    }

    /// <summary>
    ///使用可能コンポーネントのクラス名と依存するクラスのクラス名を定義する
    /// </summary>
    public class WhitelistComponetReference
    {
        public string name;
        public string[] fullNames;

        public string[] dependencies;
        public WhitelistClass whitelistClass;

        public WhitelistComponetReference(string name, string[] fullNames, string[] dependencies, WhitelistClass whitelistClass)
        {
            this.name = name;
            this.fullNames = fullNames;
            this.dependencies = dependencies;
            this.whitelistClass = whitelistClass;
        }
        public bool Match(Component comp)
        {
            return Array.IndexOf(fullNames, comp.GetType().FullName) > -1;
        }
        public bool MatchDependedComponent(Component comp)
        {
            return Array.IndexOf(dependencies, comp.GetType().FullName) > -1;
        }
    }
}