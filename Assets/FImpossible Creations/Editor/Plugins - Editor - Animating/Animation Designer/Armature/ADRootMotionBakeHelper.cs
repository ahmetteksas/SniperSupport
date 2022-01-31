using System;
using UnityEngine;

namespace FIMSpace.AnimationTools
{
    /// <summary>
    /// Just for generic rigs
    /// </summary>
    public class ADRootMotionBakeHelper
    {
        public Transform AnimatorTransform { get; private set; }
        public Transform RootMotionTransform { get; private set; }
        public AnimationDesignerSave Save { get; private set; }
        public ADClipSettings_Main MainSet { get; private set; }
        public ADBoneReference RootRef { get; private set; }

        Quaternion rootMapping;

        public ADRootMotionBakeHelper(Transform animatorTr, ADBoneReference rootRef, AnimationDesignerSave save, ADClipSettings_Main main)
        {
            AnimatorTransform = animatorTr;
            RootRef = rootRef;
            RootMotionTransform = rootRef.TempTransform;
            Save = save;
            MainSet = main;
        }

        public void ResetForBaking()
        {
            AnimationDesignerWindow.ForceTPose();

            PrepareRootMotionPosition();
            PrepareRootMotionRotation();

            startBakePos = RootMotionTransform.position;
            startBakeRot = RootMotionTransform.rotation;
            latestRootMotionPos = Vector3.zero;
            latestRootMotionRot = Quaternion.identity;
            latestRootMotionRotEnsure = Quaternion.identity;

            rootMapping = Quaternion.FromToRotation(RootMotionTransform.InverseTransformDirection(AnimatorTransform.right), Vector3.right);
            rootMapping *= Quaternion.FromToRotation(RootMotionTransform.InverseTransformDirection(AnimatorTransform.up), Vector3.up);
        }

        Vector3 startBakePos;
        Quaternion startBakeRot;

        Vector3 latestAnimatorPos;
        Quaternion latestAnimatorRot;

        Vector3 latestPos;
        Quaternion latestRot;

        Vector3 latestRootMotionPos;
        Quaternion latestRootMotionRot;
        Quaternion latestRootMotionRotEnsure;

        public void PostAnimator()
        {
            latestAnimatorPos = RootMotionTransform.position;
            latestAnimatorRot = RootMotionTransform.rotation;
        }

        public void PostRootMotion()
        {
            Vector3 diff = RootMotionTransform.position - (latestAnimatorPos);
            Vector3 local = AnimatorTransform.InverseTransformVector(diff);

            //UnityEngine.Debug.Log("diff = " + diff);
            latestRootMotionPos = local;

            //Quaternion rDiff = RootMotionTransform.rotation * Quaternion.Inverse(latestAnimatorRot);
            //rDiff = RootMotionTransform.rotation;
            //latestRootMotionRot = (rDiff) * Quaternion.Inverse(rootMapping);
            latestRootMotionRot = RootMotionTransform.rotation * Quaternion.Inverse(latestAnimatorRot);
            //Debug.Log("latestRootMotionRot = " + latestRootMotionRot.eulerAngles + " rootEul = " + latestRootMotionRot);
            //latestRootMotionRot = latestRootMotionRot;// * Quaternion.Inverse(rootMapping);
            latestPos = RootMotionTransform.position;


            // Stripping root motion out of keyframed animation
            RootMotionTransform.position = latestAnimatorPos;
            RootMotionTransform.rotation = latestAnimatorRot;
        }


        #region Just initializing curves


        [NonSerialized] public AnimationCurve _Bake_RootMPosX;
        [NonSerialized] public AnimationCurve _Bake_RootMPosY;
        [NonSerialized] public AnimationCurve _Bake_RootMPosZ;

        /// <summary> Just for generic rigs root motion </summary>
        void PrepareRootMotionPosition()
        {
            _Bake_RootMPosX = new AnimationCurve();
            _Bake_RootMPosY = new AnimationCurve();
            _Bake_RootMPosZ = new AnimationCurve();
        }


        [NonSerialized] public AnimationCurve _Bake_RootMRotX;
        [NonSerialized] public AnimationCurve _Bake_RootMRotY;
        [NonSerialized] public AnimationCurve _Bake_RootMRotZ;
        [NonSerialized] public AnimationCurve _Bake_RootMRotW;

        /// <summary> Just for generic rigs root motion </summary>
        void PrepareRootMotionRotation()
        {
            _Bake_RootMRotX = new AnimationCurve();
            _Bake_RootMRotY = new AnimationCurve();
            _Bake_RootMRotZ = new AnimationCurve();
            _Bake_RootMRotW = new AnimationCurve();
        }

        #endregion


        /// <summary> Just for generic rigs </summary>
        public void SaveRootMotionPositionCurves(ref AnimationClip clip)
        {
            float magn = ADBoneReference.ComputePositionMagn(_Bake_RootMPosX);
            magn += ADBoneReference.ComputePositionMagn(_Bake_RootMPosY);
            magn += ADBoneReference.ComputePositionMagn(_Bake_RootMPosZ);

            //UnityEngine.Debug.Log("pos magn " + magn);
            if (magn < 0.0001f) return;
            clip.SetCurve("", typeof(Animator), "MotionT.x", _Bake_RootMPosX);
            clip.SetCurve("", typeof(Animator), "MotionT.y", _Bake_RootMPosY);
            clip.SetCurve("", typeof(Animator), "MotionT.z", _Bake_RootMPosZ);
        }

        /// <summary> Just for generic rigs </summary>
        public void SaveRootMotionRotationCurves(ref AnimationClip clip)
        {
            float magn = ADBoneReference.ComputePositionMagn(_Bake_RootMRotX);
            magn += ADBoneReference.ComputePositionMagn(_Bake_RootMRotY);
            magn += ADBoneReference.ComputePositionMagn(_Bake_RootMRotZ);
            magn += ADBoneReference.ComputePositionMagn(_Bake_RootMRotW);
            //UnityEngine.Debug.Log("rot magn " + magn);

            if (magn < 0.0001f) return;
            clip.SetCurve("", typeof(Animator), "MotionQ.x", _Bake_RootMRotX);
            clip.SetCurve("", typeof(Animator), "MotionQ.y", _Bake_RootMRotY);
            clip.SetCurve("", typeof(Animator), "MotionQ.z", _Bake_RootMRotZ);
            clip.SetCurve("", typeof(Animator), "MotionQ.w", _Bake_RootMRotW);
        }

        internal void BakeCurrentState(float keyTime)
        {
            Vector3 pos = latestRootMotionPos;

            _Bake_RootMPosX.AddKey(keyTime, pos.x);
            _Bake_RootMPosY.AddKey(keyTime, pos.y);
            _Bake_RootMPosZ.AddKey(keyTime, pos.z);

            Quaternion rot = AnimationGenerateUtils.EnsureQuaternionContinuity(latestRootMotionRotEnsure, latestRootMotionRot);
            latestRootMotionRotEnsure = rot;
            _Bake_RootMRotX.AddKey(keyTime, rot.x);
            _Bake_RootMRotY.AddKey(keyTime, rot.y);
            _Bake_RootMRotZ.AddKey(keyTime, rot.z);
            _Bake_RootMRotW.AddKey(keyTime, rot.w);
        }


    }
}