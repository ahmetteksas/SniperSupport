﻿using FIMSpace.FEditor;
using FIMSpace.FTools;
using FIMSpace.Generating;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace FIMSpace.AnimationTools
{

    [System.Serializable]
    public class ADClipSettings_IK : IADSettings
    {

        public AnimationClip settingsForClip;
        public List<IKSet> LimbIKSetups = new List<IKSet>();

        public ADClipSettings_IK() { }

        /// <summary> Used for identifying different variants when modifying the same AnimationClip multiple times </summary>
        [SerializeField, HideInInspector] private string setId = "";
        [SerializeField, HideInInspector] private int setIdHash = 0;
        public void AssignID(string name) { setId = name; setIdHash = setId.GetHashCode(); }
        public string SetID { get { return setId; } }
        public int SetIDHash { get { return setIdHash; } }
        public AnimationClip SettingsForClip { get { return settingsForClip; } }
        public void OnConstructed(AnimationClip clip, int hash) { settingsForClip = clip; setIdHash = hash; }


        public ADClipSettings_IK Copy(ADClipSettings_IK to)
        {
            ADClipSettings_IK cpy = (ADClipSettings_IK)MemberwiseClone();

            cpy.LimbIKSetups = new List<IKSet>();

            for (int i = 0; i < LimbIKSetups.Count; i++)
            {
                IKSet nIk = LimbIKSetups[i].Copy();
                cpy.LimbIKSetups.Add(nIk);
            }

            cpy.setId = to.setId;
            cpy.setIdHash = to.setIdHash;
            PasteValuesTo(this, cpy);

            return cpy;
        }


        #region Settings container Methods

        internal static void PasteValuesTo(ADClipSettings_IK from, ADClipSettings_IK to)
        {
            for (int i = 0; i < from.LimbIKSetups.Count; i++)
                if (from.LimbIKSetups[i].Index == to.LimbIKSetups[i].Index)
                    IKSet.PasteValuesTo(from.LimbIKSetups[i], to.LimbIKSetups[i]);
        }


        public IKSet GetIKSettingsForLimb(ADArmatureLimb selectedLimb, AnimationDesignerSave setup)
        {
            for (int i = 0; i < LimbIKSetups.Count; i++)
            {
                var ls = LimbIKSetups[i];

                if (ls.Index == selectedLimb.GetIndex)
                {
                    return ls;
                }
            }

            IKSet set = new IKSet(false, selectedLimb.GetName, selectedLimb.GetIndex);
            set.PrepareFor(selectedLimb);
            LimbIKSetups.Add(set);

            if (setup != null) setup._SetDirty();

            return set;
        }

        internal void ResetState()
        {
            for (int i = 0; i < LimbIKSetups.Count; i++)
            {
                var lSet = LimbIKSetups[i];
                if (lSet.IKType == IKSet.EIKType.FootIK)
                {
                    if (lSet.IsLegGroundingMode)
                    {
                        lSet.LegGrounding = IKSet.ELegGroundingView.Processing;
                    }
                }
            }
        }


        #endregion



        #region IK Set


        [System.Serializable]
        public class IKSet : INameAndIndex
        {
            [NonSerialized] ADClipSettings_Main refToMainSet = null;

            public int Index;
            public string ID;

            public bool Enabled = false;
            public float Blend = 1f;
            public AnimationCurve BlendEvaluation = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);

            public enum EIKType { ArmIK, FootIK, ChainIK }
            public EIKType IKType = EIKType.ChainIK;

            public enum EOrder { OverrideElasticness, InheritElasticness }
            public EOrder UpdateOrder = EOrder.OverrideElasticness;


            public IKSet(bool enabled = false, string id = "", int index = -1, float blend = 1f)
            {
                Enabled = enabled;
                Index = index;
                ID = id;
                Blend = blend;
            }


            public IKSet Copy()
            {
                IKSet cpy = (IKSet)MemberwiseClone();
                return cpy;
            }


            #region Basic IK Params


            public float IKPosOffMul = 1f;
            public AnimationCurve IKPosOffEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKPositionOffset = Vector3.zero;

            public float IKPosStillMul = 0f;
            public AnimationCurve IKPosStillEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKStillPosition = Vector3.zero;
            public float IKPosYOriginal = 0f;


            public Vector3 IKHintOffset = Vector3.zero;


            public float IKRotationBlend = 1f;
            public float ShoulderBlend = 0.3f;


            public float IKRotOffMul = 1f;
            public AnimationCurve IKRotOffEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKRotationOffset = Vector3.zero;

            public float IKRotStillMul = 0f;
            public AnimationCurve IKRotStillEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKStillRotation = Vector3.zero;

            public float ChainSmoothing = 0f;


            #endregion



            #region Leg Grounding Params

            public enum ELegMode { Grounding, BasicIK }
            public ELegMode LegMode = ELegMode.BasicIK;

            public enum ELegGroundingView { Analyze, Processing }
            public ELegGroundingView LegGrounding = ELegGroundingView.Analyze;

            public float HeelGroundedTreshold = 1f;

            public float ToesGroundedTreshold = 1f;
            public float HoldOnGround = 0f;

            public float FootGroundBlendDuration = 0.175f;
            public AnimationCurve MaxLegStretchEvaluation = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public float MaxLegStretch = 1.1f;
            public float GroundLater = 0f;
            public float UnGroundLater = 0f;

            public float FootSwingsMultiply = 1f;
            public float FootRaiseMultiply = 0f;
            public float FootMildMotionX = 0f;
            public float FootMildMotionZ = 0f;
            public float FootLinearize = 0f;
            public float FootLinearizeSpeedMul = 1f;

            bool displayProcessingGroundingCurve = false;

            #endregion



            #region Extra Leg Grounding Params

            public float HoldXPosWhenGrounded = 0f;
            public float HoldZPosWhenGrounded = 0f;

            public float IKWhenGroundedPosOffMul = 1f;
            public AnimationCurve IKWhenGroundedPosOffEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKWhenGroundedPositionOffset = Vector3.zero;

            public float IKWhenUngroundedPosOffMul = 1f;
            public AnimationCurve IKWhenUngroundedPosOffEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKWhenUngroundedPositionOffset = Vector3.zero;

            public float IKWhenGroundedRotOffMul = 1f;
            public AnimationCurve IKWhenGroundedRotOffEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKWhenGroundedRotationOffset = Vector3.zero;

            public float IKWhenUngroundedRotOffMul = 1f;
            public AnimationCurve IKWhenUngroundedRotOffEvaluate = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
            public Vector3 IKWhenUngroundedRotationOffset = Vector3.zero;

            #endregion



            #region Extra Params - Curves Gen - align to

            public bool UseFootRotationMapping = true;
            public string LatestLimbName = "";
            public bool GenerateFootGroundCurve = false;
            public string GenerateFootStepEvents = "";
            public float GeneratedStepEventsTimeOffset = 0.0f;

            public string alignToBoneName = "";
            public Transform alignTo = null;
            public float AlignToBlend = 1f;

            #endregion



            #region Utility Helper Properties


            public static IKSet CopyingFrom = null;

            public string GetName { get { return ID; } }
            public int GetIndex { get { return Index; } }
            public float GUIAlpha { get { if (Enabled == false) return 0.1f; if (Blend < 0.5f) return 0.2f + Blend; return 1f; } }



            public bool IsLegGroundingMode
            {
                get
                {
                    if (IKType == ADClipSettings_IK.IKSet.EIKType.FootIK)
                        if (LegMode == ADClipSettings_IK.IKSet.ELegMode.Grounding)
                            return true;

                    return false;
                }
            }


            internal void DrawHellFootDetectionMask(ADClipSettings_Main mainSet, ADArmatureLimb limb, float heelGroundedTreshold, float toesGroundedTreshold, float a1 = 1f, float a2 = 1f)
            {
                Color preC = Handles.color;
                Transform animT = mainSet.LatestAnimator.transform;
                Transform footT = limb.LastBone.T;


                Vector3 lowestYPos = animT.position + animT.up * FootDataAnalyze.LowestFootCoords.y;

                // Lowest guide
                Handles.color = new Color(preC.r, preC.g, preC.b, a1);

                Vector3 heelP = GetAnalysisHeelPoint(footT);
                Vector3 toesP = GetAnalysisToesPoint(footT, heelP);
                //Vector3 hell_toesPoint = Vector3.Lerp(heelP, toesP, 0.6f);

                // Toes and heel to lowest
                Handles.color = new Color(preC.r, preC.g, preC.b, a2 * 0.3f);
                Vector3 dPoint = heelP; dPoint.y = lowestYPos.y;
                //Handles.DrawDottedLine(dPoint, heelP, 2f);
                dPoint = toesP; dPoint.y = lowestYPos.y;

                Vector3 heelPT = GetAnalysisTreshHeelPoint(footT, heelGroundedTreshold);
                Vector3 toesPT = GetAnalysisTreshToesPoint(footT, toesGroundedTreshold);

                Handles.DrawDottedLine(heelPT, ChangeY(heelPT, lowestYPos.y), 2f);
                Handles.DrawDottedLine(toesPT, ChangeY(toesPT, lowestYPos.y), 2f);


                Handles.color = new Color(preC.r - 0.3f, preC.g, preC.b - 0.3f, a2 * 0.75f);

                // Guides for heel
                Handles.DrawLine(heelPT, footT.position);
                Handles.DrawLine(heelPT, heelP);
                // Guides for toes
                Handles.color = new Color(preC.r - 0.3f, preC.g, preC.b - 0.3f, a2 * 0.75f);
                Handles.DrawLine(toesPT, footT.position);
                Handles.DrawLine(toesPT, toesP);

                Handles.color = new Color(preC.r, preC.g, preC.b, a2 * 0.3f);

                // Toes and heel tresh points
                float refLen = FootDataAnalyze.GetRefScale(limb.Bones[1].T, footT);
                float treshLen = FootDataAnalyze.GetHeightTresholdScale(limb.Bones[1].T, footT, refLen);

                Handles.color = new Color(Handles.color.r, Handles.color.g, preC.b * 0.2f, a1 * 0.3f);
                dPoint = heelPT;

                // Define Debug Y Pos for foot grounding level
                dPoint.y = animT.position.y + (treshLen * animT.lossyScale.y * 1f) * heelGroundedTreshold;
                Handles.SphereHandleCap(0, dPoint, Quaternion.identity, treshLen * 0.75f, EventType.Repaint);
                Handles.DrawLine(dPoint + Vector3.forward * refLen * 2f, dPoint - Vector3.forward * refLen * 2f);


                dPoint = toesPT;
                dPoint.y = animT.position.y + (treshLen * 1.0f) * toesGroundedTreshold;
                Handles.SphereHandleCap(0, dPoint, Quaternion.identity, treshLen * 1f, EventType.Repaint);
                Handles.DrawLine(dPoint + Vector3.forward * refLen * 2f, dPoint - Vector3.forward * refLen * 2f);

                Handles.color = preC;

                #region Backup

                //Vector3 heelP = GetAnalysisHeelPoint(footT);
                ////heelP = footT.position;

                //// Heel
                //Handles.color = new Color(Handles.color.r, Handles.color.g, Handles.color.b, a1);
                //float treshLen = FootDataAnalyze.GetTresholdLength(limb.Bones[1].T, footT, heelGroundedTreshold);
                //Vector3 toesP = GetAnalysisToesPoint(footT, heelP);
                ////toesP = limb.LastBone.TrPoint(FootDataAnalyze._footToToes);

                //float refLimbLen = limb.GetCurrentLimbLength() * 0.22f;
                //Vector3 off = mainSet.LatestAnimator.transform.right * refLimbLen;

                //_b_hf_detect[0] = (heelP) + off;
                //_b_hf_detect[1] = (toesP) + off;
                //_b_hf_detect[2] = off + (toesP - footT.TransformVector(FootDataAnalyze._footLocalToGround) * treshLen);

                //// Toes
                //treshLen = FootDataAnalyze.GetTresholdLength(limb.Bones[1].T, footT, toesGroundedTreshold);
                //_b_hf_detect[3] = off + heelP - footT.TransformVector(FootDataAnalyze._footLocalToGround) * treshLen;
                //_b_hf_detect[4] = heelP + off;

                //Handles.color = new Color(Handles.color.r, Handles.color.g, Handles.color.b, a2);

                //Vector3 hell_toesPoint = Vector3.Lerp(_b_hf_detect[0], _b_hf_detect[1], 0.4f);
                //Vector3 hell_toesPoint2 = Vector3.Lerp(_b_hf_detect[2], _b_hf_detect[3], 0.4f);
                //Handles.DrawLine(hell_toesPoint, hell_toesPoint2);

                //_b_hf_detect[1] = hell_toesPoint;
                //_b_hf_detect[2] = hell_toesPoint2;
                //Handles.DrawAAPolyLine(2f, _b_hf_detect);

                #endregion

            }


            public bool WasAnalyzed
            {
                get
                {
                    return (FootDataAnalyze != null && FootDataAnalyze.analyzed);
                }
            }


            float clipLength = 1f;
            float clipFramerate = 30f;
            float clipKeyStep = 0.04f;

            #endregion


            #region Utility / Refresh Methods

            Vector3 ChangeY(Vector3 pos, float y)
            {
                Vector3 p = pos;
                pos.y = y;
                return pos;
            }


            public static void PasteValuesTo(IKSet from, IKSet to)
            {
                to.BlendEvaluation = AnimationDesignerWindow.CopyCurve(from.BlendEvaluation);
                to.MaxLegStretchEvaluation = AnimationDesignerWindow.CopyCurve(from.MaxLegStretchEvaluation);
                to.Blend = from.Blend;
                to.Enabled = from.Enabled;

                to.IKRotationBlend = from.IKRotationBlend;

                //to.IKType = from.IKType;
                to.UpdateOrder = from.UpdateOrder;

                to.IKPosOffMul = from.IKPosOffMul;
                to.IKPosOffEvaluate = from.IKPosOffEvaluate;
                to.IKPositionOffset = from.IKPositionOffset;
                to.IKPosStillMul = from.IKPosStillMul;
                to.IKPosStillEvaluate = from.IKPosStillEvaluate;
                to.IKStillPosition = from.IKStillPosition;
                to.IKPosYOriginal = from.IKPosYOriginal;
                to.IKRotOffMul = from.IKRotOffMul;
                to.IKRotOffEvaluate = from.IKRotOffEvaluate;
                to.IKRotationOffset = from.IKRotationOffset;
                to.IKRotStillMul = from.IKRotStillMul;
                to.IKRotStillEvaluate = from.IKRotStillEvaluate;
                to.IKStillRotation = from.IKStillRotation;
                to.IKHintOffset = from.IKHintOffset;

                to.GenerateFootGroundCurve = from.GenerateFootGroundCurve;

                to.ChainSmoothing = from.ChainSmoothing;

                PasteGroundingValuesTo(from, to);

            }

            public static void PasteGroundingValuesTo(IKSet from, IKSet to)
            {
                to.IKRotationOffset = from.IKRotationOffset;

                to.LegMode = from.LegMode;
                to.LegGrounding = from.LegGrounding;
                to.HeelGroundedTreshold = from.HeelGroundedTreshold;
                to.ToesGroundedTreshold = from.ToesGroundedTreshold;
                to.HoldOnGround = from.HoldOnGround;
                to.FootGroundBlendDuration = from.FootGroundBlendDuration;
                to.MaxLegStretch = from.MaxLegStretch;
                to.GroundLater = from.GroundLater;
                to.UnGroundLater = from.UnGroundLater;
                to.LegGrounding = from.LegGrounding;

                to.FootSwingsMultiply = from.FootSwingsMultiply;
                to.FootRaiseMultiply = from.FootRaiseMultiply;
                to.FootMildMotionX = from.FootMildMotionX;
                to.FootMildMotionZ = from.FootMildMotionZ;
                to.FootLinearize = from.FootLinearize;
                to.FootLinearizeSpeedMul = from.FootLinearizeSpeedMul;
                to.HoldXPosWhenGrounded = from.HoldXPosWhenGrounded;
                to.HoldZPosWhenGrounded = from.HoldZPosWhenGrounded;
            }

            public void RefreshMod(AnimationDesignerSave save, ADClipSettings_Main main)
            {
                clipLength = main.settingsForClip.length;
                clipFramerate = main.settingsForClip.frameRate;
                clipKeyStep = 1f / clipFramerate;

                if (alignToBoneName != "")
                {
                    if (alignTo == null || alignTo.name != alignToBoneName)
                    {
                        alignTo = save.GetBoneByName(alignToBoneName);
                    }
                }

                refToMainSet = main;
            }

            public float SecToProgr(float sec)
            {
                return ADClipSettings_Main.SecondsToProgress(sec, clipLength);
            }


            #endregion



            #region GUI Related


            bool ikSetupFoldout = false;
            bool ikCoordsFoldout = false;
            bool ikRotsFoldout = false;
            bool ikAnalyzeExtraFoldot = false;
            static bool mainSetFoldout = false;
            static bool mainSetFoldoutExtra = false;
            bool displayGrInfo = false;

            static bool ikMainGroundFoldout = false;

            string selectorHelperId = "";

            bool grIkSwingsFoldout = true;
            bool grIkOffsFoldout = false;

            bool grIkOffsConstFoldout = false;
            bool grIkOffsWhenGrFoldout = false;
            bool grIkOffsWhenUngrFoldout = false;

            bool syncGrLegIKSettingsToOtherLegs = false;

            internal bool requestReinitialize = false;


            internal void DrawTopGUI(string title, float progr)
            {
                EditorGUILayout.BeginHorizontal();

                Enabled = EditorGUILayout.Toggle(Enabled, GUILayout.Width(24));

                if (!string.IsNullOrEmpty(title))
                {
                    EditorGUILayout.LabelField(title + " : Inverse Kinematics (IK)", FGUI_Resources.HeaderStyle);
                }

                #region Copy Paste Buttons

                if (CopyingFrom != null)
                {
                    if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("Clipboard").image, "Paste copied values"), FGUI_Resources.ButtonStyle, GUILayout.Width(22), GUILayout.Height(19)))
                    {
                        PasteValuesTo(CopyingFrom, this);
                    }
                }

                if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("TreeEditor.Duplicate").image, "Copy ik parameters values below to paste them into other limb"), FGUI_Resources.ButtonStyle, GUILayout.Width(22), GUILayout.Height(19)))
                {
                    CopyingFrom = this;
                }

                #endregion

                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (!Enabled) GUI.enabled = false;

                AnimationDesignerWindow.GUIDrawFloatPercentage(ref Blend, new GUIContent("IK Blend:"));
                AnimationDesignerWindow.DrawSliderProgress(Blend * BlendEvaluation.Evaluate(progr), 52);
                AnimationDesignerWindow.DrawCurve(ref BlendEvaluation, "Blend Along Clip Time");
                AnimationDesignerWindow.DrawCurveProgress(progr);


                GUI.enabled = true;
            }


            internal void DrawParamsGUI(float animProgr, ADArmatureLimb limb, ADClipSettings_Main clipMain, AnimationDesignerSave save, ADClipSettings_IK ikSets)
            {
                if (refToMainSet == null) refToMainSet = clipMain;

                if (!Enabled) GUI.enabled = false;
                Color preC = GUI.color;
                Color preBC = GUI.backgroundColor;

                FGUI_Inspector.DrawUILine(0.3f, 0.5f, 1, 15, 0.975f);


                #region IK Setup Mode


                Texture2D ikIcon = AnimationDesignerWindow.Tex_Leg;
                if (IKType == EIKType.ArmIK) ikIcon = AnimationDesignerWindow.Tex_Arm;
                else if (IKType == EIKType.ChainIK) ikIcon = AnimationDesignerWindow.Tex_Chain;


                GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("  " + FGUI_Resources.GetFoldSimbol(ikSetupFoldout, 10, "►") + "  IK Setup", ikIcon), FGUI_Resources.FoldStyle, GUILayout.Height(22))) ikSetupFoldout = !ikSetupFoldout;

                IKType = (EIKType)EditorGUILayout.EnumPopup(IKType);

                if (IKType == EIKType.FootIK)
                {
                    EditorGUIUtility.labelWidth = 80;
                    LegMode = (ELegMode)EditorGUILayout.EnumPopup("Foot IK Mode:", LegMode, GUILayout.Width(160));
                    EditorGUIUtility.labelWidth = 0;
                }

                EditorGUILayout.EndHorizontal();

                if (ikSetupFoldout)
                {

                    GUILayout.Space(6);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 84;


                    //IKType = (EIKType)EditorGUILayout.EnumPopup(new GUIContent("   IK Algorithm:", "Algorithm used for computing IK.\nChain IK is dedicated for limbs with more than 3 bones."), IKType);
                    //GUILayout.Space(6);
                    //UpdateOrder = (EOrder)EditorGUILayout.EnumPopup(new GUIContent("Update Order:", "Overriding will make IK position more stiff and in-place, executing before elasticness will allow elasticness applying it's effect over IK position"), UpdateOrder);

                    EditorGUIUtility.labelWidth = 0;
                    EditorGUILayout.EndHorizontal();


                    #region Check Limb Count Correctness

                    if (IKType == EIKType.ArmIK)
                    {
                        if (limb.Bones.Count != 4)
                        {
                            GUILayout.Space(4);
                            EditorGUILayout.HelpBox("Arm IK requires limb with 4 bones to work!", MessageType.Warning);
                            GUILayout.Space(4);
                        }
                    }
                    else if (IKType == EIKType.FootIK)
                    {
                        if (limb.Bones.Count != 3)
                        {
                            GUILayout.Space(4);
                            EditorGUILayout.HelpBox("Foot IK requires limb with 3 bones to work!", MessageType.Warning);
                            GUILayout.Space(4);
                        }
                    }
                    else if (IKType == EIKType.ChainIK)
                    {
                        if (limb.Bones.Count <= 2)
                        {
                            GUILayout.Space(4);
                            EditorGUILayout.HelpBox("Chain IK requires limb with more than 2 bones to work!", MessageType.Warning);
                            GUILayout.Space(4);
                        }
                    }

                    #endregion


                    if (IKType == EIKType.ChainIK || (IKType == EIKType.FootIK && LegMode == ELegMode.Grounding))
                    {
                        GUILayout.Space(8);
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("Foot/Chain IK Requires correctly setted T-Pose!\nMake sure it's setted correctly under the Top Tab (click on the displayed object name)", EditorStyles.centeredGreyMiniLabel, GUILayout.Height(26));
                        EditorGUILayout.EndVertical();

                        if (IKType == EIKType.ChainIK)
                        {
                            GUILayout.Space(4);
                            AnimationDesignerWindow.GUIDrawFloatPercentage(ref ChainSmoothing, new GUIContent("Chain Smoothing: "));
                        }
                    }

                    GUILayout.Space(4);
                    if (requestReinitialize) GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
                    if (GUILayout.Button(new GUIContent("Re-Initialize IK Processor", "Should be called after correcting T-Pose of the model or after changing Limb bones hierarchy"))) { requestReinitialize = true; }
                    GUI.color = preC;

                    GUILayout.Space(6);
                }

                EditorGUILayout.EndVertical();

                #endregion


                #region Leg Grounding GUI

                if (IKType == EIKType.FootIK && LegMode == ELegMode.Grounding)
                {
                    if (limb != null) LatestLimbName = limb.GetName;

                    GUILayout.Space(10);
                    EditorGUIUtility.labelWidth = 160;
                    EditorGUILayout.BeginHorizontal();
                    LegGrounding = (ELegGroundingView)EditorGUILayout.EnumPopup("View IK Grounding Stage:", LegGrounding);

                    if (displayGrInfo) GUI.color = Color.white * 0.6f;
                    if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Info), FGUI_Resources.ButtonStyle, GUILayout.Width(24), GUILayout.Height(18))) displayGrInfo = !displayGrInfo;
                    if (displayGrInfo) GUI.color = preC;

                    EditorGUILayout.EndHorizontal();
                    EditorGUIUtility.labelWidth = 0;


                    #region Info Display

                    if (displayGrInfo)
                    {
                        if (LegGrounding == ELegGroundingView.Analyze)
                        {
                            EditorGUILayout.HelpBox("Prepare settings for algorithm which will analyze source animation in order to collect useful data and detect foot touching ground moments, then use it for foot animation correction in the 'Processing' stage.", MessageType.Info);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("Using data gathered during 'Analyze' stage for applying active foot animation correction with parameters below.", MessageType.Info);
                        }
                    }

                    #endregion


                    GUILayout.Space(7);


                    #region Grounding GUI Panels

                    EditorGUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);
                    EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);

                    if (LegGrounding == ELegGroundingView.Analyze)
                    {

                        #region Analyze View

                        if (SceneView.lastActiveSceneView != null)
                            if (SceneView.lastActiveSceneView.camera != null)
                            {
                                GUILayout.Space(3);
                                if (SceneView.lastActiveSceneView.camera.orthographic)
                                {
                                    if (AnimationDesignerWindow.ContainsProjectFileSave())
                                        if (GUILayout.Button(new GUIContent("  Switch Camera Back to Perspective", EditorGUIUtility.IconContent("Camera Icon").image), GUILayout.Height(22)))
                                        {
                                            SceneView.lastActiveSceneView.orthographic = false;
                                        }
                                }
                                else
                                {
                                    EditorGUILayout.HelpBox("It's easier to analyze with Scene Isometric Camera", MessageType.None);

                                    if (AnimationDesignerWindow.ContainsProjectFileSave())
                                        if (GUILayout.Button(new GUIContent(" Switch Scene Camera to Isometric", EditorGUIUtility.IconContent("Camera Icon").image), GUILayout.Height(22)))
                                        {
                                            SceneView.lastActiveSceneView.orthographic = true;
                                            Transform refT = refToMainSet.LatestAnimator.transform;
                                            float refScale = save.ScaleRef;
                                            SceneView.lastActiveSceneView.pivot = refT.TransformPoint(Vector3.right * 0.5f + Vector3.up * refScale * 0.2f) * (refScale) * 0.5f;
                                            SceneView.lastActiveSceneView.rotation = Quaternion.LookRotation(-refT.TransformPoint(Vector3.right));
                                            if (AnimationDesignerWindow.Get) AnimationDesignerWindow.Get.FrameTarget(refToMainSet.LatestAnimator.gameObject);
                                        }

                                }
                            }


                        GUILayout.Space(5);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("During 'Analyze' stage, the IK is disabled for debugging purpose, switch to 'Processing' to see IK effect", MessageType.Info);
                        if (GUILayout.Button("Go to Processing", GUILayout.Height(26))) { LegGrounding = ELegGroundingView.Processing; }
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(5);

                        SyncFootIKToggle(preC);
                        GUILayout.Space(3);

                        HeelGroundedTreshold = EditorGUILayout.Slider("Heel Grounding Threshold", HeelGroundedTreshold, -2f, 1f);
                        ToesGroundedTreshold = EditorGUILayout.Slider("Toes Grounding Threshold ", ToesGroundedTreshold, -2f, 2f);
                        //GUILayout.Space(4);
                        //HoldOnGround = EditorGUILayout.Slider("Hold On Ground", HoldOnGround, 0f, 0.25f);
                        GUILayout.Space(8);


                        bool changed = false;
                        if (HeelGroundedTreshold != FootDataAnalyze._latestHeelTesh) changed = true;
                        if (ToesGroundedTreshold != FootDataAnalyze._latestToesTesh) changed = true;
                        if (HoldOnGround != FootDataAnalyze._latestHoldOnGround) changed = true;


                        if (FootDataAnalyze == null || FootDataAnalyze.analyzed == false)
                        {
                            GUI.backgroundColor = Color.green;
                            if (GUILayout.Button(new GUIContent("  Analyze animation clip using settings above", FGUI_Resources.Tex_Refresh))) { GenerateFootAnalyzeData(save, limb, clipMain.settingsForClip, ikSets, HeelGroundedTreshold, ToesGroundedTreshold, HoldOnGround); }
                            GUI.backgroundColor = preBC;
                        }
                        else
                        {

                            EditorGUIUtility.labelWidth = 70;
                            AnimationDesignerWindow.DrawCurve(ref FootDataAnalyze.GroundingCurve, "Grounding:");
                            AnimationDesignerWindow.DrawCurveProgress(animProgr, 74);
                            EditorGUIUtility.labelWidth = 0;


                            if (changed)
                            {
                                GUI.backgroundColor = new Color(1f, 0.9f, 0.6f, 1f);
                                if (GUILayout.Button(new GUIContent("  Re-Analyze animation clip using settings above", FGUI_Resources.Tex_Refresh))) { GenerateFootAnalyzeData(save, limb, clipMain.settingsForClip, ikSets, HeelGroundedTreshold, ToesGroundedTreshold, HoldOnGround); }
                                GUI.backgroundColor = preBC;
                                EditorGUILayout.HelpBox("Analysis parameters changed: to see difference, hit button above", MessageType.None);
                            }
                        }

                        GUILayout.Space(4);

                        if (FootDataAnalyze == null || FootDataAnalyze.analyzed == false)
                        {
                            EditorGUILayout.HelpBox("Not analyzed yet!", MessageType.Warning);
                        }
                        else
                        {
                            if (changed == false)
                            {
                                GUILayout.Space(4);
                                EditorGUILayout.HelpBox("Analyze Report: Average Foot Forward Velocity On Ground = " + (-FootDataAnalyze.approximateFootPushDirection.z) + "  Sides: " + (FootDataAnalyze.approximateFootPushDirection.x) + "  Dominant Axis: " + (-FootDataAnalyze.approximateFootPushDirectionDominant), MessageType.None);
                                GUILayout.Space(4);
                            }

                            FGUI_Inspector.FoldHeaderStart(ref ikAnalyzeExtraFoldot, "Extra Operations", FGUI_Resources.BGInBoxStyle);

                            if (ikAnalyzeExtraFoldot)
                            {

                                if (AnimationDesignerWindow.Get)
                                    if (AnimationDesignerWindow.Get.TargetClip)
                                    {
                                        GUILayout.Space(3);

                                        if (GUILayout.Button(new GUIContent("Grounding Curves to Pelvis Motion", "Extracting all analyzed legs grounding curves to pelvis motion which can be modified under 'Main & Hips IK Settings for a Current Clip' foldout below")))
                                        {
                                            AnimationCurve syncCurves = new AnimationCurve();

                                            int samples = Mathf.RoundToInt(refToMainSet.SettingsForClip.length * refToMainSet.SettingsForClip.frameRate) + 1;
                                            float stepProgr = 1f / (float)(samples);

                                            for (int f = 0; f <= samples; f++)
                                            {
                                                float averageValue = 0f;
                                                float progr = f * stepProgr;

                                                for (int i = 0; i < save.Limbs.Count; i++)
                                                {
                                                    var set = ikSets.GetIKSettingsForLimb(save.Limbs[i], save);
                                                    if (set == null) continue;
                                                    if (set.IsLegGroundingMode == false) continue;
                                                    //set.syncGrLegIKSettingsToOtherLegs = false;
                                                    averageValue = Mathf.Lerp(averageValue, set.FootDataAnalyze.GroundingCurve.Evaluate(progr), 0.5f);
                                                }

                                                syncCurves.AddKey(progr, 1f - averageValue);
                                            }

                                            syncCurves = AnimationGenerateUtils.ReduceKeyframes(syncCurves, 0.05f);
                                            refToMainSet.PelvisOffsetYEvaluate = syncCurves;
                                        }

                                        GUILayout.Space(2);

                                        if (GUILayout.Button(new GUIContent("Export Grounding Curve in Animation Clip", "It can be useful for developers programming procedural leg grounding algorithms")))
                                        {
                                            if (ExportGroundingCurveFor(AnimationDesignerWindow.Get.TargetClip))
                                                EditorUtility.DisplayDialog("Grounding Curve Saved", "Saved grounding curve '" + LatestLimbName + "-Grounding' to model file.\nIt can be useful for developers programming procedural leg grounding algorithms", "Ok");
                                            else
                                                EditorUtility.DisplayDialog("Grounding Curve NOT Saved", "Couldn't save grounding curve in file", "Ok");
                                        }

                                        GUILayout.Space(2);
                                    }
                            }

                            GUILayout.EndVertical();
                            GUILayout.Space(4);
                        }


                        GUILayout.Space(6);
                        if (AnimationDesignerWindow.Get) AnimationDesignerWindow.Get.DrawPlaybackPanel();

                        #endregion


                        SyncSettingsIfSwitched(save, limb, ikSets);

                    }
                    else if (LegGrounding == ELegGroundingView.Processing)
                    {

                        for (int i = 0; i < ikSets.LimbIKSetups.Count; i++)
                        {
                            if (ikSets.LimbIKSetups[i] == this) continue;
                            ikSets.LimbIKSetups[i].LegGrounding = ELegGroundingView.Processing;
                        }

                        #region Processing View

                        GUILayout.Space(3);
                        if (syncGrLegIKSettingsToOtherLegs) GUI.color = new Color(0.7f, 1f, 0.7f);

                        AnimationDesignerWindow.GUIDrawFloatSeconds("Ground IK Blend Duration", ref FootGroundBlendDuration, 0f, 0.3f, "sec", "How long should be transition from ungrounded to grounded state of the leg animation basing on the prepared grounding curve during analyze stage");

                        EditorGUILayout.BeginHorizontal();
                        MaxLegStretch = EditorGUILayout.Slider(new GUIContent("Max Leg Stretch", "Limiting maximum leg limb stretch, so it will never snap to totally straight pose."), MaxLegStretch, 0.5f, 1.1f);
                        AnimationDesignerWindow.DrawCurve(ref MaxLegStretchEvaluation, "", 50);
                        EditorGUILayout.EndHorizontal();
                        Rect r = AnimationDesignerWindow.DrawSliderProgress(Mathf.InverseLerp(0.5f, 1.1f, MaxLegStretch) * MaxLegStretchEvaluation.Evaluate(animProgr), 148f, 108f);
                        AnimationDesignerWindow.DrawCurveProgressOnR(animProgr, 136f, 56f, r);

                        SyncFootIKToggle(preC);

                        FGUI_Inspector.DrawUILine(0.4f, 0.4f, 1, 10, 0.975f);

                        if (syncGrLegIKSettingsToOtherLegs) GUI.color = new Color(0.7f, 1f, 0.7f);
                        AnimationDesignerWindow.GUIDrawFloatPercentage(ref HoldXPosWhenGrounded, new GUIContent("Hold X When Grounded", "Holding static, animation average X Position when foot is beign grounded, to remove foot sliding to sides when putting steps.\nIT SHOULD BE SET TO ZERO WHEN DESIGNING STRAFE WALK/RUN ANIMATIONS!"));
                        AnimationDesignerWindow.GUIDrawFloatPercentage(ref HoldZPosWhenGrounded, new GUIContent("Hold Z When Grounded", "Holding static, animation average Z Position when foot is beign grounded, to remove foot sliding to sides when putting steps.\nIT SHOULD BE SET TO ZERO WHEN DESIGNING WALK/RUN FORWARD/BACKWARD ANIMATIONS!"));

                        if (FootLinearize <= 0f) GUI.color = new Color(1f, 1f, 1f, preC.a * 0.6f);
                        AnimationDesignerWindow.GUIDrawFloatPercentage(ref FootLinearize, new GUIContent("Linear Z Movement", "Trying to set constant foot movement speed when grounded to make possible character movement translation look perfectly synced with legs.\n\nSetting HoldX or HoldZ values higher helps blending this feature!"));

                        if (FootLinearize > 0f)
                        {
                            //FootLinearizeSpeedMul = EditorGUILayout.FloatField("Linear Speed Multiplier:", FootLinearizeSpeedMul);
                            if (FootLinearizeSpeedMul < 0.1f) FootLinearizeSpeedMul = 1f; else if (FootLinearizeSpeedMul > 2f) FootLinearizeSpeedMul = 2f;
                        }

                        GUI.color = preC;

                        GUILayout.Space(6);

                        if (syncGrLegIKSettingsToOtherLegs) GUI.color = new Color(0.7f, 1f, 0.7f);
                        FGUI_Inspector.FoldHeaderStart(ref grIkSwingsFoldout, " Leg Swing Animation Modify", FGUI_Resources.BGInBoxStyle);

                        if (grIkSwingsFoldout)
                        {
                            GUILayout.Space(4);
                            AnimationDesignerWindow.GUIDrawFloatPercentage(ref FootSwingsMultiply, new GUIContent("Swings Blend", "Blend amount for all of three parameters below"));
                            GUILayout.Space(4);
                            FootRaiseMultiply = EditorGUILayout.Slider(new GUIContent("Foot Raise Multiply", "Increasing / decreasing slightly height of foots when legs is ungrounded"), FootRaiseMultiply, -1f, 2f);
                            FootMildMotionZ = EditorGUILayout.Slider(new GUIContent("Foot Mild Forward Swings", "Increasing / decreasing slightly forward/backward foot motion"), FootMildMotionZ, -0.5f, 1f);
                            FootMildMotionX = EditorGUILayout.Slider(new GUIContent("Foot Mild Sides Swings", "Increasing / decreasing slightly left/right side foot motion"), FootMildMotionX, -0.5f, 1f);
                            GUILayout.Space(4);
                        }

                        EditorGUILayout.EndVertical();

                        if (syncGrLegIKSettingsToOtherLegs) GUI.color = preC;

                        GUILayout.Space(6);


                        FGUI_Inspector.FoldHeaderStart(ref grIkOffsFoldout, " Extra IK Position Offsets", FGUI_Resources.BGInBoxStyle);

                        if (grIkOffsFoldout)
                        {
                            GUILayout.Space(6);
                            FGUI_Inspector.FoldHeaderStart(ref grIkOffsConstFoldout, " Constant IK Offsets", FGUI_Resources.BGInBoxStyle);
                            if (grIkOffsConstFoldout)
                            {
                                GUILayout.Space(4);
                                DrawIkBasicPositionOffsetsGUI(animProgr, ref IKPositionOffset, ref IKPosOffMul, ref IKPosOffEvaluate, "Knee", 24f);

                                DrawIkBasicRotationOffsetsGUI(animProgr, ref IKRotationOffset, ref IKRotOffMul, ref IKRotOffEvaluate);

                                GUILayout.Space(-3);
                                UseFootRotationMapping = EditorGUILayout.Toggle(new GUIContent("Foot Rotation Mapping:", "Automatically mapping foot rotation to be aligned with ground, if it fails, you can disable it, but in 90% situations it should work correctly"), UseFootRotationMapping);
                                GUILayout.Space(6);
                            }
                            EditorGUILayout.EndVertical();



                            GUILayout.Space(8);
                            FGUI_Inspector.FoldHeaderStart(ref grIkOffsWhenGrFoldout, " When Grounded IK Offsets", FGUI_Resources.BGInBoxStyle);
                            if (grIkOffsWhenGrFoldout)
                            {
                                GUILayout.Space(4);
                                DrawIkBasicPositionOffsetsGUI(animProgr, ref IKWhenGroundedPositionOffset, ref IKWhenGroundedPosOffMul, ref IKWhenGroundedPosOffEvaluate, "", 24f);
                                //DrawIkBasicRotationOffsetsGUI(ref IKWhenGroundedRotationOffset, ref IKWhenGroundedRotOffMul, ref IKWhenUngroundedRotOffEvaluate);
                            }
                            EditorGUILayout.EndVertical();



                            GUILayout.Space(8);
                            FGUI_Inspector.FoldHeaderStart(ref grIkOffsWhenUngrFoldout, " When Ungrounded IK Offsets", FGUI_Resources.BGInBoxStyle);
                            if (grIkOffsWhenUngrFoldout)
                            {
                                GUILayout.Space(4);
                                DrawIkBasicPositionOffsetsGUI(animProgr, ref IKWhenUngroundedPositionOffset, ref IKWhenUngroundedPosOffMul, ref IKWhenUngroundedPosOffEvaluate, "", 24f);
                                //DrawIkBasicRotationOffsetsGUI(ref IKWhenUngroundedRotationOffset, ref IKWhenUngroundedRotOffMul, ref IKWhenUngroundedRotOffEvaluate);
                            }

                            EditorGUILayout.EndVertical();

                        }

                        EditorGUILayout.EndVertical();


                        if (FootDataAnalyze != null)
                        {
                            GUILayout.Space(6);

                            if (displayProcessingGroundingCurve == false)
                            {
                                if (GUILayout.Button("^ Quick View for Grounding Curve ^", EditorStyles.centeredGreyMiniLabel)) displayProcessingGroundingCurve = !displayProcessingGroundingCurve;
                            }
                            else
                            {
                                if (GUILayout.Button(" Quick View for Grounding Curve ", EditorStyles.centeredGreyMiniLabel)) displayProcessingGroundingCurve = !displayProcessingGroundingCurve;
                                AnimationDesignerWindow.DrawCurve(ref FootDataAnalyze.GroundingCurve, "");
                                AnimationDesignerWindow.DrawCurveProgress(animProgr, 2, 60);

                            }
                        }



                        //GUILayout.Space(9);
                        //EditorGUILayout.LabelField("TODO: Impulse for Spring On Grounded", EditorStyles.centeredGreyMiniLabel);


                        if (AnimationDesignerWindow.Get)
                        {
                            FGUI_Inspector.DrawUILine(0.4f, 0.4f, 1, 18, 0.975f);
                            AnimationDesignerWindow.Get.DrawPlaybackPanel();
                        }


                        #endregion

                        SyncSettingsIfSwitched(save, limb, ikSets);

                    }


                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndVertical();


                    #endregion



                    GUILayout.Space(9);

                    EditorGUIUtility.labelWidth = 220;
                    //GenerateFootGroundCurve = EditorGUILayout.Toggle(new GUIContent("Export Foot Grounding Height Curve:", "Including foot height animation curve (analyze tab - Grounding Curve) during generating clip file with name of the limb. ('" + limb.GetName + "')"), GenerateFootGroundCurve);

                    GenerateFootStepEvents = EditorGUILayout.TextField(new GUIContent("Generate foot-step events:", "[Left empty to not use this feature]\nGenerating footstep events in animation clip basing on the 'Grounding' curve from the Analyze Tab.\nYou can check this events when you double-click on the generated AnimationClip file in the project browser."), GenerateFootStepEvents);
                    EditorGUIUtility.labelWidth = 0;

                    if (string.IsNullOrEmpty(GenerateFootStepEvents) == false)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(16);
                        EditorGUILayout.LabelField("Events Time Offset:", GUILayout.Width(124));
                        GeneratedStepEventsTimeOffset = GUILayout.HorizontalSlider(GeneratedStepEventsTimeOffset, -0.2f, 0.2f);
                        GUILayout.Space(6);
                        EditorGUILayout.LabelField(System.Math.Round(GeneratedStepEventsTimeOffset, 2) + " sec", GUILayout.Width(60));
                        EditorGUILayout.EndHorizontal();
                    }

                    GUILayout.Space(4);


                }

                #endregion

                else
                {

                    #region Basic IK Position / Rotation Settings


                    if (IKType == EIKType.ArmIK)
                    {
                        GUILayout.Space(10);
                        AnimationDesignerWindow.GUIDrawFloatPercentage(ref ShoulderBlend, new GUIContent("   IK Shoulders Blend:", FGUI_Resources.Tex_Default, "When ik target position is too far, shoulders rotation can be triggered to ease reaching far elements animation."));
                        GUILayout.Space(4);

                        EditorGUILayout.BeginHorizontal();
                        MaxLegStretch = EditorGUILayout.Slider(new GUIContent("Max Arm Stretch", "Limiting maximum arm limb stretch, so it will never snap to totally straight pose."), MaxLegStretch, 0.5f, 1.1f);
                        AnimationDesignerWindow.DrawCurve(ref MaxLegStretchEvaluation, "", 50);
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(4);
                    }
                    else if (IKType == EIKType.FootIK)
                    {
                        GUILayout.Space(4);
                        EditorGUILayout.BeginHorizontal();
                        MaxLegStretch = EditorGUILayout.Slider("Max Leg Stretch", MaxLegStretch, 0.5f, 1.1f);
                        AnimationDesignerWindow.DrawCurve(ref MaxLegStretchEvaluation, "", 50);
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(4);
                    }

                    GUILayout.Space(6);

                    FGUI_Inspector.FoldHeaderStart(ref ikCoordsFoldout, "  IK Positions Modify", FGUI_Resources.BGInBoxStyle);

                    if (ikCoordsFoldout)
                    {
                        GUILayout.Space(4);

                        string hintTitle = "";
                        if (IKType == EIKType.ArmIK) hintTitle = "Elbow"; else if (IKType == EIKType.FootIK) hintTitle = "Knee";
                        DrawIkBasicPositionOffsetsGUI(animProgr, ref IKPositionOffset, ref IKPosOffMul, ref IKPosOffEvaluate, hintTitle);

                        EditorGUILayout.BeginHorizontal();
                        IKStillPosition = EditorGUILayout.Vector3Field(new GUIContent("  IK Still Position:", FGUI_Resources.Tex_Movement), IKStillPosition);
                        if (IKPosStillMul < 0.1f) { if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Refresh, "Copy current animator hand position and paste into stil position"), EditorStyles.label, GUILayout.Width(20), GUILayout.Height(17))) { IKStillPosition = refToMainSet.LatestAnimator.transform.InverseTransformPoint(limb.LastBone.pos); IKPosStillMul = 1f; } }
                        EditorGUILayout.EndHorizontal();


                        GUILayout.Space(4);
                        EditorGUILayout.BeginHorizontal();
                        AnimationDesignerWindow.GUIDrawFloatPercentage(ref IKPosStillMul, new GUIContent("Still Blend:", "How much still ik target position should be applied."));
                        AnimationDesignerWindow.DrawCurve(ref IKPosStillEvaluate, "", 70);
                        EditorGUILayout.EndHorizontal();

                        Rect r = AnimationDesignerWindow.DrawSliderProgress(IKPosStillMul * IKPosStillEvaluate.Evaluate(animProgr), 68f, 127f);
                        AnimationDesignerWindow.DrawCurveProgressOnR(animProgr, 140f, 60f, r);

                        GUILayout.Space(9);
                        AnimationDesignerWindow.GUIDrawFloatPercentage(ref IKPosYOriginal, new GUIContent("Still Y Original:", "Keeping Y position of original animation wrist position for the ik target position. It can be useful for carrying animations. If switching it offsets your hand ik position too low or too high, try adjusting it with 'Position Offset' parameter above."));
                        GUILayout.Space(3);

                        FGUI_Inspector.DrawUILine(0.3f, 0.5f, 1, 15, 0.975f);


                        EditorGUILayout.BeginHorizontal();
                        GUI.color = new Color(1f, 1f, 1f, 0.7f);
                        alignTo = (Transform)EditorGUILayout.ObjectField(new GUIContent("Align IK With:", "You can assign some custom transform reference to for ik target to follow. Can be useful for carrying animation - then you can put here counter arm bone and adjust positioning with ik position offset parameter above."), alignTo, typeof(Transform), true);
                        GUI.color = preC;
                        GUILayout.Space(6);

                        if (Searchable.IsSetted)
                            if (selectorHelperId != "")
                                if (selectorHelperId == "algn" + GetIndex)
                                {
                                    object g = Searchable.Get();

                                    if (g == null) alignTo = null; else alignTo = (Transform)g;

                                    if (alignTo)
                                        alignToBoneName = alignTo.name;
                                    else
                                        alignToBoneName = "";

                                    selectorHelperId = "";
                                }


                        if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_DownFold), EditorStyles.label, GUILayout.Width(20), GUILayout.Height(18)))
                        {
                            selectorHelperId = "algn" + GetIndex;
                            AnimationDesignerWindow.ShowBonesSelector("Choose Your Character Model Bone", save.GetAllArmatureBonesList, AnimationDesignerWindow.GetMenuDropdownRect(), true);
                        }

                        EditorGUILayout.EndHorizontal();
                        AnimationDesignerWindow.GUIDrawFloatPercentage(ref AlignToBlend, new GUIContent("Align To Blend:"));
                        if (alignTo != null) if (IKPosStillMul > 0.6f) if (AlignToBlend > 0.1f)
                                {
                                    EditorGUILayout.HelpBox("IK Still Position Blend is overriding 'Aling To' feature!", MessageType.None);
                                }

                        GUILayout.Space(3);

                        if (IKType == EIKType.ArmIK)
                        {
                            if (refToMainSet != null)
                                if (refToMainSet.OffsetHandsIKBlend < 0.5f) { EditorGUILayout.HelpBox("Offset Y Pos With Pelvis is multiplied by Offset Hands Blend in the bottom tab of this IK settings!", MessageType.None); }
                            AnimationDesignerWindow.GUIDrawFloatPercentage(ref OffsetWithPelvisBlend, new GUIContent("Offset Y Pos With Pelvis", "Offsetting hand ik position with additional pelvis motion applied under 'Main & Hips IK Settings for a Current Clip' tab.\n\n!!! This value is multiplied by 'Offset Hands Blend' value in the mentioned tab."));
                            GUILayout.Space(3);
                        }
                    }

                    EditorGUILayout.EndVertical();

                    GUILayout.Space(8);

                    if (IKType != EIKType.ChainIK)
                    {

                        FGUI_Inspector.FoldHeaderStart(ref ikRotsFoldout, "  IK Rotations Modify", FGUI_Resources.BGInBoxStyle);

                        if (ikRotsFoldout)
                        {

                            DrawIkBasicRotationOffsetsGUI(animProgr, ref IKRotationOffset, ref IKRotOffMul, ref IKRotOffEvaluate);


                            EditorGUILayout.BeginHorizontal();
                            IKStillRotation = EditorGUILayout.Vector3Field(new GUIContent("  IK Still Rotation:", FGUI_Resources.Tex_Rotation), IKStillRotation);
                            if (IKRotStillMul < 0.1f) { if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Refresh, "Copy current animation wrist rotation and paste into still rotation"), EditorStyles.label, GUILayout.Width(20), GUILayout.Height(17))) { IKStillRotation = limb.LastBone.rot.eulerAngles; IKRotStillMul = 1f; } }
                            EditorGUILayout.EndHorizontal();
                            GUILayout.Space(6);
                            EditorGUILayout.BeginHorizontal();
                            AnimationDesignerWindow.GUIDrawFloatPercentage(ref IKRotStillMul, new GUIContent("Still Blend:"));
                            AnimationDesignerWindow.DrawCurve(ref IKRotStillEvaluate, "", 70);
                            EditorGUILayout.EndHorizontal();


                            Rect r = AnimationDesignerWindow.DrawSliderProgress(IKRotStillMul * IKRotStillEvaluate.Evaluate(animProgr), 68, 127);
                            AnimationDesignerWindow.DrawCurveProgressOnR(animProgr, 140f, 60f, r);

                            GUILayout.Space(8);

                            if (IKType != EIKType.ChainIK)
                                AnimationDesignerWindow.GUIDrawFloatPercentage(ref IKRotationBlend, new GUIContent("Overall IK Rotation Blend", "Blend IK Foot/Hand rotation to choosed target rotation %"));
                            GUILayout.Space(4);
                        }

                        EditorGUILayout.EndVertical();

                    }


                    #endregion

                }


                GUILayout.Space(3);
                GUI.enabled = true;

            }


            #region Additional GUI Panels


            public static void DrawIKHipsParameters(float progr, ADClipSettings_Main clipMain, AnimationDesignerSave save)
            {
                Color preguiC = GUI.color;

                string ff = FGUI_Resources.GetFoldSimbol(mainSetFoldout, false);

                if (GUILayout.Button(ff + "  Main & Hips IK Settings for a Current Clip  " + ff, FGUI_Resources.HeaderStyle))
                {
                    mainSetFoldout = !mainSetFoldout;
                }

                if (mainSetFoldout)
                {
                    int displays = clipMain.GUI_OnDisplay(true, 0, save);

                    GUILayout.Space(8);
                    EditorGUILayout.HelpBox("Settings below are affecting all Foot IK limbs with Grounding mode", MessageType.None);
                    GUILayout.Space(9);

                    if (displays < 3) GUI.color = new Color(1f, 1f, 0.7f, 1f);
                    clipMain.Pelvis = (Transform)EditorGUILayout.ObjectField("Pelvis Transform:", clipMain.GetPelvis(save, save.ReferencePelvis.parent), typeof(Transform), true);

                    if (displays < 4)
                    {
                        EditorGUILayout.HelpBox("Make sure 'Pelvis' have assigned correct PELVIS bone", displays < 2 ? MessageType.Info : MessageType.None);
                        Rect rr = GUILayoutUtility.GetLastRect();
                        if (GUI.Button(rr, GUIContent.none, GUIStyle.none)) { clipMain.GUI_OnDisplay(true, 5); }
                    }
                    GUI.color = preguiC;

                    GUILayout.Space(6);


                    #region Axis curves for Pelvis

                    EditorGUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);
                    EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);

                    AnimationDesignerWindow.GUIDrawFloatPercentage(ref clipMain.PelvisOffsetsBlend, new GUIContent("Pelvis Offsets Blend:"));
                    GUILayout.Space(4);

                    EditorGUIUtility.labelWidth = 180;
                    clipMain.PelvisConstantYOffset = EditorGUILayout.FloatField("Pelvis Constant Y Offset:", clipMain.PelvisConstantYOffset);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 150;
                    clipMain.PelvisYOffset = EditorGUILayout.FloatField("Pelvis Y Offset:", clipMain.PelvisYOffset);
                    EditorGUIUtility.labelWidth = 0;
                    AnimationDesignerWindow.DrawCurve(ref clipMain.PelvisOffsetYEvaluate, "", 50, 0f, -1f, 1f, 1f);
                    EditorGUILayout.EndHorizontal();

                    float rrOffset = 94f;

                    AnimationDesignerWindow.DrawCurveProgressOnR(progr, rrOffset);


                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 150;
                    clipMain.PelvisXOffset = EditorGUILayout.FloatField("Pelvis Sides (x) Offset:", clipMain.PelvisXOffset);
                    EditorGUIUtility.labelWidth = 0;
                    AnimationDesignerWindow.DrawCurve(ref clipMain.PelvisOffsetXEvaluate, "", 50, 0f, -1f, 1f, 1f);
                    EditorGUILayout.EndHorizontal();

                    AnimationDesignerWindow.DrawCurveProgressOnR(progr, rrOffset);


                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 150;
                    clipMain.PelvisZOffset = EditorGUILayout.FloatField("Pelvis Forward (z) Offset:", clipMain.PelvisZOffset);
                    EditorGUIUtility.labelWidth = 0;
                    AnimationDesignerWindow.DrawCurve(ref clipMain.PelvisOffsetZEvaluate, "", 50, 0f, -1f, 1f, 1f);
                    EditorGUILayout.EndHorizontal();

                    AnimationDesignerWindow.DrawCurveProgressOnR(progr, rrOffset);


                    EditorGUILayout.EndVertical();

                    if (AnimationDesignerWindow.Get)
                    {
                        EditorGUILayout.BeginHorizontal();
                        AnimationDesignerWindow.Get.DrawPlaybackButton();
                        GUILayout.Space(5);
                        AnimationDesignerWindow.Get.DrawPlaybackTimeSlider();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();

                    #endregion


                    GUILayout.Space(6);

                    EditorGUILayout.BeginHorizontal();

                    //???
                    EditorGUILayout.BeginVertical(GUILayout.Width(18)); GUILayout.Space(4);
                    if (GUILayout.Button(FGUI_Resources.GetFoldSimbol(ikMainGroundFoldout, true), EditorStyles.label, GUILayout.Width(18))) ikMainGroundFoldout = !ikMainGroundFoldout;
                    EditorGUILayout.EndVertical();

                    clipMain.IKGroundLevel = EditorGUILayout.FloatField("Floor Y Height Level:", clipMain.IKGroundLevel);
                    EditorGUILayout.EndHorizontal();

                    if (ikMainGroundFoldout)
                    {
                        EditorGUI.indentLevel++;
                        clipMain.IKGroundNormal = EditorGUILayout.Vector3Field("Ground Plane Normal:", clipMain.IKGroundNormal).normalized;
                        EditorGUI.indentLevel--;
                    }

                    GUILayout.Space(6);
                    clipMain.OffsetHandsIKBlend = EditorGUILayout.Slider(new GUIContent("Offset Hands Blend", "Add pelvis position offset to hand ik"), clipMain.OffsetHandsIKBlend, 0f, 1f);


                    GUILayout.Space(5);
                    FGUI_Inspector.FoldHeaderStart(ref mainSetFoldoutExtra, "Extra Settings", FGUI_Resources.BGInBoxStyle);

                    if (mainSetFoldoutExtra)
                    {
                        clipMain.ElasticnesSettings.Enabled = EditorGUILayout.Toggle("Use Hips Elasticness", clipMain.ElasticnesSettings.Enabled);

                        if (clipMain.ElasticnesSettings.Enabled)
                        {
                            GUILayout.Space(6);
                            clipMain.ElasticnesSettings.DrawParamsGUI(1f, false, true, false);
                        }

                        GUILayout.Space(3);

                    }

                    EditorGUILayout.EndVertical();



                    GUILayout.Space(-4);
                }
                else
                {
                    clipMain.GUI_OnDisplay(false);
                }


                GUILayout.Space(10);
            }


            void DrawIkBasicPositionOffsetsGUI(float progr, ref Vector3 IKPositionOffset, ref float IKPosOffMul, ref AnimationCurve IKPosOffEvaluate, string hintName = "Elbow", float xOff = 0f)
            {
                IKPositionOffset = EditorGUILayout.Vector3Field(new GUIContent("  IK Position Offset:", FGUI_Resources.Tex_Movement), IKPositionOffset);
                GUILayout.Space(4);

                EditorGUILayout.BeginHorizontal();
                AnimationDesignerWindow.GUIDrawFloatPercentage(ref IKPosOffMul, new GUIContent("Offset Blend:"));
                AnimationDesignerWindow.DrawCurve(ref IKPosOffEvaluate, "", 70);
                EditorGUILayout.EndHorizontal();

                Rect r = AnimationDesignerWindow.DrawSliderProgress(IKPosOffMul * IKPosOffEvaluate.Evaluate(progr), 68, 127);
                AnimationDesignerWindow.DrawCurveProgressOnR(progr, 140f + xOff, 60f, r);

                if (hintName != "")
                {
                    GUILayout.Space(12);
                    IKHintOffset = EditorGUILayout.Vector3Field(new GUIContent("   " + hintName + " Hint Offset:", FGUI_Resources.TexTargetingIcon, "Hint offset which drives 'knee'/'elbow' positioning for ik.\n\nIf correction required, for elbow in most cases you will set Z value to negative + some X offsets depending if it's right or left limb. For knees you will set Z value positive + some X axis offsets."), IKHintOffset);
                }

                GUILayout.Space(12);
            }


            void DrawIkBasicRotationOffsetsGUI(float progr, ref Vector3 IKRotationOffset, ref float IKRotOffMul, ref AnimationCurve IKRotOffEvaluate)
            {
                GUILayout.Space(4);
                IKRotationOffset = EditorGUILayout.Vector3Field(new GUIContent("  IK Rotation Offset:", FGUI_Resources.Tex_Rotation), IKRotationOffset);
                GUILayout.Space(6);
                EditorGUILayout.BeginHorizontal();
                AnimationDesignerWindow.GUIDrawFloatPercentage(ref IKRotOffMul, new GUIContent("Offset Blend:"));
                AnimationDesignerWindow.DrawCurve(ref IKRotOffEvaluate, "", 70);
                EditorGUILayout.EndHorizontal();

                Rect r = AnimationDesignerWindow.DrawSliderProgress(IKRotOffMul * IKRotOffEvaluate.Evaluate(progr), 68, 127);
                AnimationDesignerWindow.DrawCurveProgressOnR(progr, 140f, 60f, r);

                GUILayout.Space(12);
            }


            void SyncFootIKToggle(Color preCol)
            {
                EditorGUIUtility.labelWidth = 198;
                if (syncGrLegIKSettingsToOtherLegs) GUI.color = new Color(0.5f, 1f, 0.2f, 1f);
                syncGrLegIKSettingsToOtherLegs = EditorGUILayout.Toggle("Sync other legs settings with this", syncGrLegIKSettingsToOtherLegs);
                if (syncGrLegIKSettingsToOtherLegs) GUI.color = preCol;
                EditorGUIUtility.labelWidth = 0;
            }


            #endregion



            #endregion


            public float OffsetWithPelvisBlend = 1f;

            internal Vector3 GetHintPosition(Vector3 defHintPos, Transform root, float progr)
            {
                if (refToMainSet != null)
                {
                    if (IKType == EIKType.ArmIK)
                        defHintPos += refToMainSet.LatestAnimator.transform.TransformVector(refToMainSet.GetHipsOffset(progr) * (refToMainSet.OffsetHandsIKBlend * OffsetWithPelvisBlend));
                }

                return defHintPos + root.TransformVector(IKHintOffset);
            }


            bool ComputeGroundedIn(float animationProgress)
            {
                bool grounded = FootDataAnalyze.GroundedIn(animationProgress);

                #region Backup

                //if (LegGrounding == ELegGroundingView.Processing)
                //{
                //    if (!grounded) // if not yet grounded
                //    {
                //        if (GroundLater < 0f) // if we want to ground sooner
                //        {
                //            grounded = FootDataAnalyze.GroundedIn(animationProgress - (GroundLater));
                //        }

                //        if (UnGroundLater > 0f) // Keep grounded a bit longer
                //        {
                //            grounded = FootDataAnalyze.GroundedIn(animationProgress - (UnGroundLater));
                //        }
                //    }
                //    else // if grounded
                //    {
                //        if (GroundLater > 0f) // if we want to ground later
                //        {
                //            grounded = FootDataAnalyze.GroundedIn(animationProgress - (GroundLater));
                //        }

                //        if (UnGroundLater > 0f) // Unground sooner
                //        {
                //            grounded = FootDataAnalyze.GroundedIn(animationProgress - (UnGroundLater));
                //        }
                //    }
                //}
                #endregion

                return grounded;
            }


            float ComputeGroundedBlend(float animationProgress)
            {
                bool grounded = ComputeGroundedIn(animationProgress);
                if (grounded) return 1f;


                // Predict next grounding with sampling
                float nextGroundingInProgr = -1f;
                float blendDurInProgr = SecToProgr(FootGroundBlendDuration);

                int toSampleCount = Mathf.CeilToInt(blendDurInProgr * clipFramerate);

                if (toSampleCount > 0)
                {
                    //UnityEngine.Debug.Log( "sec=" + FootGroundBlendDuration + " cliplen=" + clipLength +  "  durToProgr= " + SecToProgr(FootGroundBlendDuration) + "  clipKeyStep = " + clipKeyStep + "   /  = " + (blendDurInProgr * clipFramerate) + "   /  = " + (blendDurInProgr * clipKeyStep) + "   toSample = " + toSample);
                    float progrStep = blendDurInProgr / (float)toSampleCount;
                    float boostDensity = 32f;

                    for (int i = 1; i <= toSampleCount * boostDensity; i++)
                    {
                        float progr = animationProgress + progrStep * ((float)i / boostDensity);
                        float cycProgr = progr;
                        if (cycProgr > 1f) cycProgr -= 1f;

                        if (ComputeGroundedIn(cycProgr))
                        {
                            nextGroundingInProgr = progr;
                            break;
                        }
                    }

                    if (nextGroundingInProgr != -1f)
                    {
                        float diff = nextGroundingInProgr - animationProgress;
                        //UnityEngine.Debug.Log("diff = " + diff + "    diff / dur = " + diff / blendDurInProgr);
                        return 1f - (diff / blendDurInProgr);
                    }
                    else // Postdicting if was grounded in previous frames
                    {
                        float wasGroundingInPreviousProgr = -1f;

                        for (int i = 1; i <= toSampleCount * boostDensity; i++)
                        {
                            float progr = animationProgress - progrStep * ((float)i / boostDensity);
                            float cycProgr = progr;
                            if (cycProgr < 0f) cycProgr += 1f;
                            if (ComputeGroundedIn(cycProgr))
                            {
                                wasGroundingInPreviousProgr = progr;
                                break;
                            }
                        }

                        if (wasGroundingInPreviousProgr != -1f)
                        {
                            float diff = animationProgress - wasGroundingInPreviousProgr;
                            //UnityEngine.Debug.Log("diff = " + diff + "    diff / dur = " + diff / blendDurInProgr);
                            return 1f - (diff / blendDurInProgr);
                        }

                    }
                }


                return 0f;
            }


            internal void ComputeFootIKGrounding(ref Vector3 newIKPos, ref Quaternion newIKRot, Transform root, float progr, FIK_IKProcessor processor)
            {
                if (LegGrounding == ELegGroundingView.Analyze) return;

                float groundingBlendIn = ComputeGroundedBlend(progr);

                Vector3 ikMotionEdit = newIKPos;

                float raise = FootRaiseMultiply * FootSwingsMultiply;

                if (raise != 0f || FootMildMotionZ * FootSwingsMultiply != 0f || FootMildMotionX * FootSwingsMultiply != 0f)
                {
                    Vector3 motionEditLocal = root.InverseTransformPoint(ikMotionEdit);

                    if (raise < 0f) motionEditLocal.y = Mathf.LerpUnclamped(motionEditLocal.y, FootDataAnalyze._groundToFootHeight, -raise * 0.5f);
                    else if (raise > 0f) motionEditLocal.y = Mathf.LerpUnclamped(FootDataAnalyze._groundToFootHeight, motionEditLocal.y, 1f + raise * 0.5f);

                    motionEditLocal.z = Mathf.LerpUnclamped(motionEditLocal.z, 0f, FootMildMotionZ * FootSwingsMultiply);
                    motionEditLocal.x = Mathf.LerpUnclamped(motionEditLocal.x, 0f, FootMildMotionX * FootSwingsMultiply);

                    ikMotionEdit = root.TransformPoint(motionEditLocal);
                }

                Vector3 constOff = root.TransformVector(IKPositionOffset) * IKPosOffMul * IKPosOffEvaluate.Evaluate(progr);

                if (IKWhenUngroundedPosOffMul > 0f) ikMotionEdit = Vector3.LerpUnclamped(ikMotionEdit, ikMotionEdit + root.TransformVector(IKWhenUngroundedPositionOffset), IKWhenUngroundedPosOffMul * IKWhenUngroundedPosOffEvaluate.Evaluate(progr));

                newIKPos = ikMotionEdit;// + constOff;


                #region Leg stretching limiting when ungrounded

                float maxStr = MaxLegStretch * MaxLegStretchEvaluation.Evaluate(progr);

                if (maxStr < 1.1f)
                {
                    float stretch = processor.GetStretchValue(newIKPos);

                    if (stretch > maxStr)
                    {
                        float len = (maxStr * processor.GetLimbLength());
                        newIKPos = processor.StartIKBone.srcPosition + (newIKPos - processor.StartIKBone.srcPosition).normalized * len;
                    }
                }

                #endregion


                if (groundingBlendIn > 0f)
                {
                    Vector3 groundIKPos = newIKPos;


                    #region Foot Analyze - from Local to World

                    Vector3 localPos = root.InverseTransformPoint(groundIKPos);

                    float staticX = FootDataAnalyze.approximateFootLocalXPosDuringPush;
                    if (LegGrounding != ELegGroundingView.Analyze) localPos.x = Mathf.LerpUnclamped(localPos.x, staticX, HoldXPosWhenGrounded);

                    float staticZ = FootDataAnalyze.approximateFootLocalZPosDuringPush;
                    if (LegGrounding != ELegGroundingView.Analyze) localPos.z = Mathf.LerpUnclamped(localPos.z, staticZ, HoldZPosWhenGrounded);

                    groundIKPos = root.TransformPoint(localPos);

                    #endregion


                    // Multiply offsets
                    if (IKWhenGroundedPosOffMul > 0f) groundIKPos = Vector3.LerpUnclamped(groundIKPos, groundIKPos + root.TransformVector(IKWhenGroundedPositionOffset), IKWhenGroundedPosOffMul * IKWhenGroundedPosOffEvaluate.Evaluate(progr));

                    // Constant offset for grounded ik pos
                    //groundIKPos += constOff;

                    Vector3 localGroundPlanePos = root.InverseTransformPoint(groundIKPos);
                    Vector3 posOnGroundPlane = Vector3.ProjectOnPlane(localGroundPlanePos, (refToMainSet.IKGroundNormal));
                    localGroundPlanePos.y = posOnGroundPlane.y + FootDataAnalyze._groundToFootHeight + refToMainSet.IKGroundLevel;
                    groundIKPos = root.TransformPoint(localGroundPlanePos);


                    #region Foot Linearinze

                    if (FootLinearize > 0f)
                    {

                        float preUngr, nextUngr;
                        Vector3 newLinPos = root.InverseTransformPoint(groundIKPos);

                        if (!ComputeGroundedIn(progr)) // Not Grounded Yet - Transitioning
                        {
                            preUngr = FindPreviousUngroundedProgr(progr, true);
                            nextUngr = FindNextUngroundedProgr(progr, true);

                            float nearerProgr = preUngr;
                            if (Mathf.Abs(preUngr - progr) > Mathf.Abs(nextUngr - progr)) nearerProgr = nextUngr;

                            Vector3 transitionInPos = FootDataAnalyze.GetFootLocalPosition(nearerProgr);
                            Vector3 localCurr = root.InverseTransformPoint(groundIKPos);
                            transitionInPos.y = localCurr.y;

                            if (HoldXPosWhenGrounded > 0.1f)
                            {
                                float mild = 1f - FootMildMotionZ;
                                transitionInPos.z = Mathf.Lerp(localCurr.z, transitionInPos.z * mild, groundingBlendIn);
                            }
                            else
                                transitionInPos.z = localCurr.z;

                            if (HoldZPosWhenGrounded > 0.1f)
                            {
                                float mild = 1f - FootMildMotionX;
                                transitionInPos.x = Mathf.Lerp(localCurr.x, transitionInPos.x * mild, groundingBlendIn);
                            }
                            else
                                transitionInPos.x = localCurr.x;

                            newLinPos = Vector3.Lerp(localCurr, transitionInPos, groundingBlendIn);
                        }
                        else
                        {
                            preUngr = FindPreviousUngroundedProgr(progr, false);
                            nextUngr = FindNextUngroundedProgr(progr, false);

                            Vector3 startPos = FootDataAnalyze.GetFootLocalPosition(preUngr);
                            Vector3 endPos = FootDataAnalyze.GetFootLocalPosition(nextUngr);

                            float progrBetween = Mathf.InverseLerp(preUngr, nextUngr, progr);
                            //UnityEngine.Debug.Log("Pre " + preUngr + "  next  " + nextUngr + "  curr: " + progr + " between: " + progrBetween);

                            if (HoldXPosWhenGrounded > 0.1f)
                            {
                                float mild = 1f - FootMildMotionZ;
                                newLinPos.z = Mathf.Lerp(startPos.z * mild * FootLinearizeSpeedMul, endPos.z * mild * FootLinearizeSpeedMul, progrBetween);
                            }

                            if (HoldZPosWhenGrounded > 0.1f)
                            {
                                float mild = 1f - FootMildMotionX;
                                newLinPos.x = Mathf.Lerp(startPos.x * mild * FootLinearizeSpeedMul, endPos.x * mild * FootLinearizeSpeedMul, progrBetween);
                            }

                            if (HoldXPosWhenGrounded < 0.02f && HoldZPosWhenGrounded < 0.02f)
                            {
                                newLinPos.x = Mathf.Lerp(startPos.x * FootLinearizeSpeedMul, endPos.x * FootLinearizeSpeedMul, progrBetween);
                                newLinPos.z = Mathf.Lerp(startPos.z * FootLinearizeSpeedMul, endPos.z * FootLinearizeSpeedMul, progrBetween);
                            }


                        }


                        newLinPos = root.TransformPoint(newLinPos);

                        groundIKPos = Vector3.Lerp(groundIKPos, newLinPos, FootLinearize * groundingBlendIn);


                        #region Backup

                        //if (!ComputeGroundedIn(progr)) // Not Grounded Yet - Transitioning
                        //{
                        //    preUngr = FindPreviousUngroundedProgr(progr, true);
                        //    nextUngr = FindNextUngroundedProgr(progr, true);
                        //    //preUngr = (progr - blendDurInProgr * grOneMin);
                        //    //nextUngr = (progr + blendDurInProgr * grOneMin);

                        //    float nearerProgr = preUngr;
                        //    if (Mathf.Abs(preUngr - progr) > Mathf.Abs(nextUngr - progr)) nearerProgr = nextUngr;

                        //    Vector3 transitionInPos = root.TransformPoint(FootDataAnalyze.GetFootLocalPosition(nearerProgr));
                        //    groundIKPos = Vector3.Lerp(groundIKPos, transitionInPos, FootLinearize * groundingBlendIn);
                        //}
                        //else
                        //{
                        //    float blendDurInProgr = SecToProgr(FootGroundBlendDuration) * 0.6f;
                        //    float grOneMin = 1f - groundingBlendIn;

                        //    preUngr = FindPreviousUngroundedProgr(progr, false);
                        //    nextUngr = FindNextUngroundedProgr(progr, false);

                        //    Vector3 startPos = FootDataAnalyze.GetFootLocalPosition(preUngr);
                        //    Vector3 endPos = FootDataAnalyze.GetFootLocalPosition(nextUngr);

                        //    float progrBetween = Mathf.InverseLerp(preUngr, nextUngr, progr);

                        //    Vector3 newLinPos = root.InverseTransformPoint(groundIKPos);

                        //    if (HoldXPosWhenGrounded > 0.1f)
                        //    {
                        //        float mild = 1f - FootMildMotionZ;
                        //        newLinPos.z = Mathf.Lerp(startPos.z * mild, endPos.z * mild, progrBetween);
                        //    }

                        //    if (HoldZPosWhenGrounded > 0.1f)
                        //    {
                        //        float mild = 1f - FootMildMotionX;
                        //        newLinPos.x = Mathf.Lerp(startPos.x * mild, endPos.x * mild, progrBetween);
                        //    }

                        //    newLinPos = root.TransformPoint(newLinPos);

                        //    groundIKPos = Vector3.Lerp(groundIKPos, newLinPos, FootLinearize * groundingBlendIn);
                        //}
                        #endregion


                    }

                    #endregion

                    groundIKPos += constOff;


                    Quaternion footRot;

                    if (UseFootRotationMapping)
                    {
                        //Quaternion footRot = FootDataAnalyze._initFootRot;
                        Vector3 projectedPlaneNormal = root.TransformDirection(refToMainSet.IKGroundNormal);
                        footRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(root.forward, projectedPlaneNormal), projectedPlaneNormal) * FootDataAnalyze._footRotMapping;
                    }
                    else
                    {
                        footRot = processor.IKTargetRotation;
                    }

                    if (IKRotOffMul > 0f) footRot = Quaternion.SlerpUnclamped(footRot, footRot * Quaternion.Euler(IKRotationOffset), IKRotOffMul * IKRotOffEvaluate.Evaluate(progr));

                    newIKPos = Vector3.Lerp(newIKPos, groundIKPos, groundingBlendIn);
                    newIKRot = Quaternion.Slerp(newIKRot, footRot, groundingBlendIn);
                }

            }


            float FindPreviousUngroundedProgr(float from, bool findGrounded)
            {
                float blendDurInProgr = SecToProgr(FootGroundBlendDuration);
                int toSampleCount = Mathf.CeilToInt(blendDurInProgr * clipFramerate);

                float progrStep = blendDurInProgr / (float)toSampleCount;
                float boostDensity = 32f;

                for (int i = 1; i <= toSampleCount * boostDensity * 256; i++)
                {
                    float progr = from + progrStep * ((float)i / boostDensity);
                    float cycProgr = progr;
                    if (progr > 1.5f) return -1f;
                    if (cycProgr > 1f) cycProgr -= 1f;

                    if (ComputeGroundedIn(cycProgr) == findGrounded)
                    {
                        return progr;
                    }
                }

                return -1f;
            }

            float FindNextUngroundedProgr(float from, bool findGrounded)
            {
                float blendDurInProgr = SecToProgr(FootGroundBlendDuration);
                int toSampleCount = Mathf.CeilToInt(blendDurInProgr * clipFramerate);

                float progrStep = blendDurInProgr / (float)toSampleCount;
                float boostDensity = 32f;

                for (int i = 1; i <= toSampleCount * boostDensity * 256; i++)
                {
                    float progr = from - progrStep * ((float)i / boostDensity);
                    float cycProgr = progr;
                    if (progr < -0.5f) return -1f;
                    if (cycProgr < 0f) cycProgr += 1f;
                    if (ComputeGroundedIn(cycProgr) == findGrounded)
                    {
                        return progr;
                    }
                }

                return -1f;
            }

            //Vector3 ToWorldOff(Vector3 localOff)
            //{
            //    if (refToMainSet == null) return localOff;
            //    return refToMainSet.LatestAnimator.transform.TransformVector(localOff);
            //}
            //Vector3 ToWorldPoint(Vector3 localPos)
            //{
            //    if (refToMainSet == null) return localPos;
            //    return refToMainSet.LatestAnimator.transform.TransformPoint(localPos);
            //}


            internal Vector3 GetTargetIKPosition(Vector3 newIKPos, Transform root, float progr, FimpIK_Arm armIK, FIK_IKProcessor footIK)
            {

                Vector3 initIk = newIKPos;

                if (alignTo != null) if (AlignToBlend > 0f) newIKPos = Vector3.LerpUnclamped(newIKPos, alignTo.position, AlignToBlend);
                if (IKPosStillMul > 0f) newIKPos = Vector3.LerpUnclamped(newIKPos, root.TransformPoint(IKStillPosition), IKPosStillMul * IKPosStillEvaluate.Evaluate(progr));
                if (IKPosOffMul > 0f) newIKPos = Vector3.LerpUnclamped(newIKPos, newIKPos + root.TransformVector(IKPositionOffset), IKPosOffMul * IKPosOffEvaluate.Evaluate(progr));
                if (IKPosYOriginal > 0f) newIKPos.y = Mathf.LerpUnclamped(newIKPos.y, initIk.y + IKPositionOffset.y * IKPosOffMul * IKPosOffEvaluate.Evaluate(progr), IKPosYOriginal);



                if (IKType == EIKType.ArmIK)
                {
                    if (refToMainSet != null)
                    {
                        newIKPos += refToMainSet.LatestAnimator.transform.TransformVector(refToMainSet.GetHipsOffset(progr) * (refToMainSet.OffsetHandsIKBlend * OffsetWithPelvisBlend));
                    }


                    #region Arm stretching limiting 

                    if (armIK != null)
                    {
                        float maxStr = MaxLegStretch * MaxLegStretchEvaluation.Evaluate(progr);

                        if (maxStr < 1.1f)
                        {
                            float stretch = armIK.GetStretchValue(newIKPos);

                            if (stretch > maxStr)
                            {
                                float len = (maxStr * armIK.limbLength);
                                newIKPos = armIK.UpperArmIKBone.srcPosition + (newIKPos - armIK.UpperArmIKBone.transform.position).normalized * len;
                            }
                        }
                    }

                    #endregion

                }

                if (IKType == EIKType.FootIK && LegMode == ELegMode.BasicIK)
                    if (footIK != null)
                    {

                        #region Leg stretching limiting when ungrounded

                        float maxStr = MaxLegStretch * MaxLegStretchEvaluation.Evaluate(progr);

                        if (maxStr < 1.1f)
                        {
                            float stretch = footIK.GetStretchValue(newIKPos);

                            if (stretch > maxStr)
                            {
                                float len = (maxStr * footIK.GetLimbLength());
                                newIKPos = footIK.StartIKBone.srcPosition + (newIKPos - footIK.StartIKBone.srcPosition).normalized * len;
                            }
                        }

                        #endregion
                    }


                return newIKPos;
            }

            internal Quaternion GetTargetIKRotation(Quaternion newIKRot, Transform root, float progr)
            {
                Quaternion initIk = newIKRot;
                if (IKRotStillMul > 0f) newIKRot = Quaternion.SlerpUnclamped(newIKRot, root.rotation * Quaternion.Euler(IKStillRotation), IKRotStillMul * IKRotStillEvaluate.Evaluate(progr));
                if (IKRotOffMul > 0f) newIKRot = Quaternion.SlerpUnclamped(newIKRot, newIKRot * Quaternion.Euler(IKRotationOffset), IKRotOffMul * IKRotOffEvaluate.Evaluate(progr));

                return newIKRot;
            }



            #region Foot Grounding Analyze


            [NonSerialized] public Vector3[] _b_ground = new Vector3[2];
            [NonSerialized] public Vector3[] _b_groundX = new Vector3[2];
            [NonSerialized] public Vector3[] _b_foot = new Vector3[4];

            //[NonSerialized] public Vector3[] _b_hf_detect = new Vector3[5];
            //[NonSerialized] public Vector3[] _b_heel = new Vector3[2];
            //[NonSerialized] public Vector3[] _b_hold = new Vector3[2];

            public void RefreshGizmosBuffers()
            {
                if (_b_foot == null) _b_foot = new Vector3[4];
                if (_b_ground == null) _b_ground = new Vector3[2];
                if (_b_groundX == null) _b_groundX = new Vector3[2];
                //if (_b_hf_detect == null) _b_hf_detect = new Vector3[5];
                //if (_b_heel == null) _b_heel = new Vector3[2];
                //if (_b_hold == null) _b_hold = new Vector3[2];
            }


            public LegAnimationClipMotion FootDataAnalyze = null;

            private void SyncSettingsIfSwitched(AnimationDesignerSave save, ADArmatureLimb limb, ADClipSettings_IK ikSets)
            {
                if (syncGrLegIKSettingsToOtherLegs)
                {
                    for (int i = 0; i < save.Limbs.Count; i++)
                    {
                        if (save.Limbs[i] == limb) continue;
                        var set = ikSets.GetIKSettingsForLimb(save.Limbs[i], save);
                        if (set == null) continue;
                        if (set.IsLegGroundingMode == false) continue;
                        set.syncGrLegIKSettingsToOtherLegs = false;
                        PasteGroundingValuesTo(this, set);
                    }
                }
            }

            private void GenerateFootAnalyzeData(AnimationDesignerSave save, ADArmatureLimb limb, AnimationClip clip, ADClipSettings_IK ikSets, float footTreshold, float toesTreshold, float holdOnGround)
            {
                GetClipFootAnalyze(save, limb, clip, footTreshold, toesTreshold, holdOnGround);

                if (syncGrLegIKSettingsToOtherLegs)
                {
                    for (int i = 0; i < save.Limbs.Count; i++)
                    {
                        if (save.Limbs[i] == limb) continue;
                        var set = ikSets.GetIKSettingsForLimb(save.Limbs[i], save);
                        if (set == null) continue;
                        if (set.IsLegGroundingMode == false) continue;
                        set.GetClipFootAnalyze(save, save.Limbs[i], clip, footTreshold, toesTreshold, holdOnGround);
                    }
                }

            }

            /// <summary> Character must be in T-Pose when calling this method! </summary>
            private void GetClipFootAnalyze(AnimationDesignerSave save, ADArmatureLimb limb, AnimationClip clip, float heelTreshold, float toesTreshold, float holdOnGround)
            {
                AnimationDesignerWindow.ForceTPose();

                FootDataAnalyze = new LegAnimationClipMotion(clip);

                FootDataAnalyze.AnalyzeClip(save.LatestAnimator.transform, save.ReferencePelvis.transform, limb.Bones[0].T, limb.Bones[1].T, limb.Bones[2].T, null,
                    save.LatestAnimator, Mathf.RoundToInt(clip.frameRate * clip.length),
                    heelTreshold, toesTreshold, holdOnGround,
                    true);
            }


            public Vector3 GetAnalysisTreshHeelPoint(Transform foot, float heelTresh)
            {
                return FootDataAnalyze.GetSamplingHeelPoint(foot, heelTresh);
            }

            public Vector3 GetAnalysisTreshToesPoint(Transform foot, float toesTresh)
            {
                return FootDataAnalyze.GetSamplingToesPoint(foot, toesTresh);
            }

            public Vector3 GetAnalysisHeelPoint(Transform foot)
            {
                return foot.position + foot.TransformVector(FootDataAnalyze._footLocalToGround);
            }

            public Vector3 GetAnalysisToesPoint(Transform foot, Vector3 heelPoint)
            {
                return heelPoint + foot.TransformVector(FootDataAnalyze._footToToesForw);
            }



            internal void DrawFootGroundingAnalyzeGizmos(float animationProgress, ADArmatureLimb limb, float alphaMul = 1f)
            {
                RefreshGizmosBuffers();

                bool grounded = ComputeGroundedIn(animationProgress);

                if (grounded)
                    Handles.color = new Color(0.1f, 1f, 0.1f, alphaMul);
                else
                    Handles.color = new Color(1f, 0.7f, 0.1f, 0.4f * alphaMul);

                Vector3 footP = GetAnalysisHeelPoint(limb.LastBone.T);
                _b_foot[0] = limb.LastBone.pos;
                _b_foot[1] = footP;
                _b_foot[2] = GetAnalysisToesPoint(limb.LastBone.T, footP);
                _b_foot[3] = limb.LastBone.pos;

                Handles.DrawAAPolyLine(grounded ? 5f : 2.5f, _b_foot);

                if (refToMainSet != null)
                    if (refToMainSet.LatestAnimator != null)
                        if (LegGrounding == ELegGroundingView.Processing)
                        {
                            Vector3 floorOff = Vector3.up * refToMainSet.IKGroundLevel;
                            Vector3 cross = Vector3.Cross(refToMainSet.LatestAnimator.transform.forward, refToMainSet.IKGroundNormal).normalized;
                            Vector3 cross2 = Vector3.Cross(refToMainSet.LatestAnimator.transform.right, refToMainSet.IKGroundNormal).normalized;

                            _b_ground[0] = floorOff + (-cross);
                            _b_ground[1] = floorOff + (cross);

                            _b_groundX[0] = floorOff + (-cross2);
                            _b_groundX[1] = floorOff + (cross2);

                            Handles.matrix = refToMainSet.LatestAnimator.transform.localToWorldMatrix;
                            Handles.color = new Color(0.7f, 0.4f, 0.1f, alphaMul);
                            Handles.DrawAAPolyLine(3f, _b_ground);
                            Handles.DrawAAPolyLine(3f, _b_groundX);
                            Handles.matrix = Matrix4x4.identity;
                        }

            }

            internal void PrepareFor(ADArmatureLimb selectedLimb)
            {
                if (selectedLimb == null) return;

                if (selectedLimb.LimbType == ADArmatureLimb.ELimbType.Arm)
                {
                    if (selectedLimb.Bones.Count == 4) IKType = EIKType.ArmIK;
                }
                else if (selectedLimb.LimbType == ADArmatureLimb.ELimbType.Leg)
                {
                    if (selectedLimb.Bones.Count == 3) IKType = EIKType.FootIK;
                }
                else if (selectedLimb.LimbType == ADArmatureLimb.ELimbType.Spine)
                {
                    IKType = EIKType.ChainIK;
                }
                else if (selectedLimb.Bones.Count == 3) IKType = EIKType.FootIK;
                else if (selectedLimb.Bones.Count == 4) IKType = EIKType.ArmIK;
                else IKType = EIKType.ChainIK;
            }


            #endregion



            #region Animation Clip Additional Data Generating



            internal void GenerateGroundingEventsFor(AnimationClip newGeneratedClip, List<AnimationEvent> aEvents)
            {
                if (string.IsNullOrEmpty(GenerateFootStepEvents)) return;
                if (newGeneratedClip.frameRate < 2) return;
                if (newGeneratedClip.length < 0.01f) return;
                if (FootDataAnalyze == null) return;

                int iters = 0;
                float elapsed = 0f;
                bool? wasGrounded = null;
                while (elapsed < newGeneratedClip.length)
                {
                    elapsed += 0.01f; // 0.01sec 

                    float progr = elapsed / newGeneratedClip.length;
                    bool isGrounded = ComputeGroundedIn(progr);

                    if (wasGrounded == false)
                    {
                        if (isGrounded)
                        {
                            wasGrounded = true;
                            float eventTime = elapsed + GeneratedStepEventsTimeOffset;
                            if (eventTime < 0f) eventTime = 0f;
                            if (eventTime >= newGeneratedClip.length) eventTime = newGeneratedClip.length - 0.001f;

                            AnimationEvent ev = new AnimationEvent();
                            ev.functionName = GenerateFootStepEvents;
                            ev.time = eventTime;
                            aEvents.Add(ev);
                        }
                    }
                    else
                    {
                        if (isGrounded == false) wasGrounded = false;
                    }

                    iters += 1;
                    if (iters > 10000) break;
                }


            }

            internal bool ExportGroundingCurveFor(AnimationClip clip)
            {
                #region Initial Requirements

                if (FootDataAnalyze == null) return false;
                if (!clip) return false;
                if (!GenerateFootGroundCurve) return false;
                if (string.IsNullOrEmpty(LatestLimbName)) return false;

                string filePath = (AssetDatabase.GetAssetPath(clip));
                if (string.IsNullOrEmpty(filePath)) return false;

                var obj = AssetDatabase.LoadMainAssetAtPath(filePath);
                if (obj == null) return false;

                #endregion

                ModelImporter mdl = (ModelImporter)AssetImporter.GetAtPath(filePath);
                if (mdl == null || mdl.clipAnimations == null) return false;

                ModelImporterClipAnimation clipFile = null;
                int clipIndex = 0;

                #region Finding target AnimationClip in the model file

                if (mdl.clipAnimations.Length == 1)
                {
                    clipFile = mdl.clipAnimations[0]; clipIndex = 0;
                }
                else
                {
                    // First Checking by take name
                    for (int cl = 0; cl < mdl.clipAnimations.Length; cl++)
                        if (mdl.clipAnimations[cl].takeName == clip.name)
                        {
                            clipFile = mdl.clipAnimations[cl]; clipIndex = cl; break;
                        }

                    // If not found then checking by name
                    if (clipFile == null)
                    {
                        for (int cl = 0; cl < mdl.clipAnimations.Length; cl++)
                            if (mdl.clipAnimations[cl].name == clip.name)
                            {
                                clipFile = mdl.clipAnimations[cl]; clipIndex = cl; break;
                            }
                    }
                }

                #endregion

                if (clipFile == null) { return false; }
                AnimationCurve fc = AnimationDesignerWindow.CopyCurve(FootDataAnalyze.GroundingCurve);

                // We need to access clip data through unity serialization to avoid clip references loses
                SerializedObject so = new SerializedObject(mdl);
                SerializedProperty clips = so.FindProperty("m_ClipAnimations");
                var clpProp = clips.GetArrayElementAtIndex(clipIndex);
                var curves = clpProp.FindPropertyRelative("curves");

                // Add curve to clip file
                bool applied = false;
                string targetName = LatestLimbName + "-Grounding";

                for (int clpc = 0; clpc < curves.arraySize; clpc++)
                {
                    if (curves.GetArrayElementAtIndex(clpc).FindPropertyRelative("name").stringValue == targetName)
                    {
                        curves.GetArrayElementAtIndex(clpc).FindPropertyRelative("curve").animationCurveValue = fc;
                        applied = true;
                        break;
                    }
                }

                if (!applied) // We need to add new array element of curve
                {
                    int index = curves.arraySize;
                    curves.InsertArrayElementAtIndex(index);

                    var nCurve = curves.GetArrayElementAtIndex(index);
                    nCurve.FindPropertyRelative("name").stringValue = targetName;
                    nCurve.FindPropertyRelative("curve").animationCurveValue = fc;
                }

                EditorUtility.SetDirty(so.targetObject);
                so.ApplyModifiedProperties();

                return true;
            }


            #endregion


        }


        #endregion



    }
}