using UnityEditor;
using UnityEngine;

namespace FIMSpace.AnimationTools
{
    public partial class AnimationDesignerWindow : EditorWindow
    {
        bool playPreview = false;
        float playbackSpeed = 1f;
        float animationElapsed = 0f;
        float animationProgress = 0f;
        float timeElapsed = 0f;
        float timeSin = 0f;
        float timeSin01 = 0f;
        float timePerlin = 0f;

        GameObject _latest_SceneObj = null;
        int _latest_SceneObjRepaintFor = -1;

        private float _play_mod_clipOrigLen = 1f;
        private float _play_mod_clipLenMul = 1f;
        private float _play_mod_clipStartTrim = 0f;
        private float _play_mod_trimmedStart = 0f;
        //private float _play_mod_trimmedEnd = 0f;
        private float _play_mod_clipEndTrim = 0f;
        private float _play_mod_dtMod = 1f;

        /// <summary> Clip length in seconds after export modifications (with trimming) </summary>
        private float _play_mod_Length = 1f;
        /// <summary> Clip length in seconds trimmed, without duration multiplier </summary>
        private float _play_mod_TrimmedLength = 1f;
        /// <summary> Clip length in seconds after export modifications </summary>
        private float _play_mod_Length_PlusJustMul = 1f;
        public float GetClipLengthModified()
        {
            if (!TargetClip) return 1f;

            _play_mod_clipLenMul = GetClipDurationMul();
            _play_mod_clipStartTrim = GetClipLeftTrim();
            _play_mod_clipEndTrim = GetClipRightTrim(false);

            _play_mod_TrimmedLength = TargetClip.length - (TargetClip.length * (_play_mod_clipStartTrim + _play_mod_clipEndTrim));

            return _play_mod_TrimmedLength * _play_mod_clipLenMul;
        }

        private void Update()
        {
            if (!wasSceneRepaint) Repaint();
            wasSceneRepaint = false;

            if (_switchingReferences) return;
            if (_serializationChanges) return;


            #region Forcing window repaint when switching scene selections

            if (_latest_SceneObj != Selection.activeGameObject)
            {
                _latest_SceneObj = Selection.activeGameObject;
                _latest_SceneObjRepaintFor = 10;
            }

            if (_latest_SceneObjRepaintFor > 0)
            {
                _latest_SceneObjRepaintFor -= 1;
                if (!_serializationChanges) Repaint();
            }

            #endregion


            #region Return Conditions + TPoseRestore

            if (!S) return;
            if (!IsReady) return;
            if (!S.SkelRootBone) return;
            if (!S.ReferencePelvis) return;
            if (!TargetClip)
            {
                if (!restoredTPose)
                {
                    S.RestoreTPose();
                    restoredTPose = true;
                }

                return;
            }

            if (isBaking) return;

            restoredTPose = false;

            #endregion


            _dtForcingUpdate = true;


            UpdateEditorDeltaTime();


            UtilsUpdate();


            timeElapsed += dt;
            timeSin = Mathf.Sin(timeElapsed * 4f);
            timeSin01 = (timeSin + 1f) * 0.5f;
            timePerlin = Mathf.PerlinNoise(timeElapsed * 5f, 1000f + timeElapsed * 6f);


            if (debugTabFoldout) return;

            RefreshClipLengthModValues();

            if (TargetClip)
            {
                if (animationElapsed > _play_mod_Length * 2f) animationElapsed = 0f;
            }

            if (playPreview)
            {
                animationElapsed += dt * playbackSpeed;
                if (animationElapsed >= _play_mod_Length) animationElapsed -= _play_mod_Length;
            }

            if (_latestClip != TargetClip)
            {
                _latestClip = TargetClip;
            }


            CheckComponentsInitialization(false);
            SampleCurrentAnimation();

            if (updateDesigner)
            {
                UpdateSimulationAfterAnimators(null);
                LateUpdateSimulation();
            }

            SectionsUpdateLoop();
        }


        void SectionsUpdateLoop()
        {
            switch (Category)
            {
                case ECategory.Setup: _Update_SetupCategory(); break;
                case ECategory.IK: _Update_IKCategory(); break;
                case ECategory.Modificators: _Update_ModsCategory(); break;
                case ECategory.Elasticness: _Update_ElasticnessCategory(); break;
                case ECategory.Morphing: _Gizmos_MorphingCategory(); break;
            }
        }



        //public float SetTimeByProgress(float animProgress)
        //{

        //}

        //public float SetAnimationTimeBySeconds(float animSeconds)
        //{

        //}





        #region Clip export mods

        public float GetCurrentAnimationProgress()
        {
            return 0f;
        }


        public float GetClipDurationMul()
        {
            if (S == null) return 1f;
            if (TargetClip == null) return 1f;
            if (_anim_MainSet == null) { _anim_MainSet = S.GetSetupForClip(S.MainSetupsForClips, TargetClip, _toSet_SetSwitchToHash); return 1f; }
            if (_anim_MainSet.ClipDurationMultiplier < 0.1f) return 1f;
            if (_anim_MainSet.ClipDurationMultiplier > 5f) return 1f;
            return _anim_MainSet.ClipDurationMultiplier;
        }

        public float GetClipLeftTrim()
        {
            if (S == null) return 0f;
            if (TargetClip == null) return 0f;
            if (_anim_MainSet == null) { _anim_MainSet = S.GetSetupForClip(S.MainSetupsForClips, TargetClip, _toSet_SetSwitchToHash); return 0f; }
            if (_anim_MainSet.ClipTrimFirstFrames < 0f) return 0f;
            if (_anim_MainSet.ClipTrimFirstFrames > 1f) return 0f;
            return _anim_MainSet.ClipTrimFirstFrames;
        }

        public float GetClipRightTrim(bool oneMinus)
        {
            float defa = oneMinus ? 1f : 0f;
            if (S == null) return defa;
            if (TargetClip == null) return defa;
            if (_anim_MainSet == null) { _anim_MainSet = S.GetSetupForClip(S.MainSetupsForClips, TargetClip, _toSet_SetSwitchToHash); return defa; }
            if (_anim_MainSet.ClipTrimLastFrames < 0f) return defa;
            if (_anim_MainSet.ClipTrimLastFrames > 1f) return defa;
            if (oneMinus) return 1f - _anim_MainSet.ClipTrimLastFrames; else return _anim_MainSet.ClipTrimLastFrames;
        }

        public void RefreshClipLengthModValues()
        {
            if (TargetClip) _play_mod_clipOrigLen = TargetClip.length;
            _play_mod_clipLenMul = GetClipDurationMul();
            _play_mod_clipStartTrim = GetClipLeftTrim();
            _play_mod_clipEndTrim = GetClipRightTrim(false);
            _play_mod_Length = GetClipLengthModified();
            _play_mod_Length_PlusJustMul = _play_mod_clipOrigLen * _play_mod_clipLenMul;
            _play_mod_trimmedStart = _play_mod_clipOrigLen * _play_mod_clipStartTrim;
            _play_mod_dtMod = 1f / _play_mod_clipLenMul;
        }

        #endregion


        void SampleCurrentAnimation(bool autoElapse = true)
        {

            #region Not sampling if humanoid playing generic animation and vice versa

            if (latestAnimator.IsHuman())
            {
                if (TargetClip.isHumanMotion == false) return;
            }
            else
            {
                if (TargetClip.isHumanMotion) return;
            }

            #endregion

            PreCalibrateLimbs();

            float sampleTime = animationElapsed / _play_mod_clipLenMul;
            if (autoElapse) sampleTime = (_play_mod_trimmedStart * _play_mod_clipLenMul + animationElapsed) / _play_mod_clipLenMul;

            TargetClip.SampleAnimation(latestAnimator.gameObject, sampleTime);

            if (_play_mod_Length > 0f)
            {
                if (autoElapse) animationProgress = animationElapsed / _play_mod_Length;
                else animationProgress = FLogicMethods.InverseLerpUnclamped(_play_mod_clipStartTrim * _play_mod_Length_PlusJustMul, _play_mod_Length_PlusJustMul - (_play_mod_clipEndTrim * _play_mod_Length_PlusJustMul), animationElapsed);

                //if (!autoElapse)
                //{
                //    UnityEngine.Debug.Log("elapsed  = " + sampleTime + "   " + animationElapsed);
                //    UnityEngine.Debug.Log("trim < " + (_play_mod_clipStartTrim * _play_mod_Length_PlusJustMul) + "  " + (_play_mod_Length_PlusJustMul - (_play_mod_clipEndTrim * _play_mod_Length_PlusJustMul)) + " > = " + (animationElapsed));
                //}
            }

        }

        void PreCalibrateLimbs()
        {
            for (int i = 0; i < Limbs.Count; i++) Limbs[i].ComponentBlendingPreLateUpdateCalibrate(_anim_blendSet);
        }

        void StartBake(AnimationClip originalClip)
        {
            Ar.Bake_Prepare(latestAnimator, originalClip, S, _anim_MainSet);
        }


        void BakingLoop(ref AnimationClip clip, int i, int keys, int startKey, int endKey)
        {
            if (bakeFramerateRatio <= 1.05f || i == startKey || i == endKey - 1) // No need for oversimulating
            {
                animationElapsed = (((float)(i) / (float)(keys - 1)) * _play_mod_Length_PlusJustMul);
                deltaTime = dt;

                SampleCurrentAnimation(false);
                UpdateSimulationAfterAnimators(Ar.rootBake);

                LateUpdateSimulation();

                Ar.Bake_CaptureFramePose(animationElapsed);
            }
            else // Oversimulating for 60fps density
            {
                float overSimulates = Mathf.Ceil(bakeFramerateRatio); // Density of additional frames if required

                for (int o = 0; o < overSimulates; o++)
                {
                    deltaTime = dt / overSimulates;
                    float overSimProgr = (float)o / overSimulates;

                    animationElapsed = (((float)((float)(i) + overSimProgr) / (float)(keys - 1)) * _play_mod_Length_PlusJustMul);

                    SampleCurrentAnimation(false);
                    UpdateSimulationAfterAnimators(Ar.rootBake);
                    LateUpdateSimulation();
                }

                Ar.Bake_CaptureFramePose(animationElapsed);
            }
        }


        void FinishBake(ref AnimationClip clip, AnimationClip originalClip)
        {
            Ar.Bake_Complete(ref clip, S, originalClip);
        }

        /// <summary> Simulate to calm down model after export </summary>
        void CalmModel()
        {
            try
            {
                dt = 0.2f;
                animationElapsed = 0f;
                deltaTime = dt;

                for (int i = 0; i < 10; i++)
                {
                    BakingPreSimulation();
                }

            }
            catch (System.Exception exc)
            {
                UnityEngine.Debug.Log("[Animation Designer] Something wrong happened during post-simulating (not important stage of baking)");
                UnityEngine.Debug.LogException(exc);
            }
        }

        #region Editor Window Delta Time

        // Delta time for window in editor

        /// <summary> Static delta time computed on clip length and framerate </summary>
        protected float dt = 0.1f;
        /// <summary> Static delta time computed on clip length and framerate but adapted to 60fps </summary>
        protected float adaptDt = 0.1f;

        /// <summary> Delta time which should be used in simulation methods - it's active and changing when oversimulating desnity frames for 60fps </summary>
        protected float deltaTime = 0.1f;

        /// <summary> When clip is 30fps, we simulate bake in 60fps so we need [60f / framerate] (2f) times more simulation steps</summary>
        protected float bakeFramerateRatio = 1f;

        double lastUpdateTime = 0f;
        protected bool _dtWasUpdating = false;
        protected bool _dtForcingUpdate = false;

        protected virtual void UpdateEditorDeltaTime()
        {
            if (_dtForcingUpdate)
            {
                if (!_dtWasUpdating)
                {
                    lastUpdateTime = EditorApplication.timeSinceStartup;
                    _dtWasUpdating = true;
                }
            }
            else
            {
                _dtWasUpdating = false;
            }

            if (_dtWasUpdating)
            {
                if (isBaking == false)
                {
                    dt = (float)(EditorApplication.timeSinceStartup - lastUpdateTime);
                    adaptDt = dt;
                    deltaTime = dt;
                    bakeFramerateRatio = 1f;
                }

                lastUpdateTime = EditorApplication.timeSinceStartup;
            }


            _dtForcingUpdate = false;
        }


        #endregion

    }
}