﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FIMSpace.AnimationTools
{
    public partial class ADArmatureBakeHelper
    {
        private class ADHumanoidMuscle
        {
            public AnimationCurve curve;
            private int muscleIndex = -1;
            private string propertyName;

            public ADHumanoidMuscle(int muscleIndex)
            {
                this.muscleIndex = muscleIndex;
                propertyName = CorrectFingersPropertyNames(HumanTrait.MuscleName[muscleIndex]);
                Reset();
            }

            public void SaveCurves(ref AnimationClip clip, float maxError)
            {
                if ( ADBoneReference.LoopBakedPose)
                {
                    ADBoneReference.WrapBake(curve);
                }

                if (maxError > 0f) curve = AnimationGenerateUtils.ReduceKeyframes(curve, maxError);
               
                clip.SetCurve(string.Empty, typeof(Animator), propertyName, curve);
            }

            public void Reset()
            {
                curve = new AnimationCurve();
            }

            public void SetKeyframe(float time, float[] muscles)
            {
                curve.AddKey(time, muscles[muscleIndex]);
            }

            public void MultiplyLength(AnimationCurve curve, float mlp)
            {
                Keyframe[] keys = curve.keys;
                for (int i = 0; i < keys.Length; i++) keys[i].time *= mlp;
                curve.keys = keys;
            }

            #region Utils

            private string CorrectFingersPropertyNames(string b)
            {
                if (b == "Left Index 1 Stretched") return "LeftHand.Index.1 Stretched";
                if (b == "Left Index 2 Stretched") return "LeftHand.Index.2 Stretched";
                if (b == "Left Index 3 Stretched") return "LeftHand.Index.3 Stretched";

                if (b == "Left Middle 1 Stretched") return "LeftHand.Middle.1 Stretched";
                if (b == "Left Middle 2 Stretched") return "LeftHand.Middle.2 Stretched";
                if (b == "Left Middle 3 Stretched") return "LeftHand.Middle.3 Stretched";

                if (b == "Left Ring 1 Stretched") return "LeftHand.Ring.1 Stretched";
                if (b == "Left Ring 2 Stretched") return "LeftHand.Ring.2 Stretched";
                if (b == "Left Ring 3 Stretched") return "LeftHand.Ring.3 Stretched";

                if (b == "Left Little 1 Stretched") return "LeftHand.Little.1 Stretched";
                if (b == "Left Little 2 Stretched") return "LeftHand.Little.2 Stretched";
                if (b == "Left Little 3 Stretched") return "LeftHand.Little.3 Stretched";

                if (b == "Left Thumb 1 Stretched") return "LeftHand.Thumb.1 Stretched";
                if (b == "Left Thumb 2 Stretched") return "LeftHand.Thumb.2 Stretched";
                if (b == "Left Thumb 3 Stretched") return "LeftHand.Thumb.3 Stretched";

                if (b == "Left Index Spread") return "LeftHand.Index.Spread";
                if (b == "Left Middle Spread") return "LeftHand.Middle.Spread";
                if (b == "Left Ring Spread") return "LeftHand.Ring.Spread";
                if (b == "Left Little Spread") return "LeftHand.Little.Spread";
                if (b == "Left Thumb Spread") return "LeftHand.Thumb.Spread";

                if (b == "Right Index 1 Stretched") return "RightHand.Index.1 Stretched";
                if (b == "Right Index 2 Stretched") return "RightHand.Index.2 Stretched";
                if (b == "Right Index 3 Stretched") return "RightHand.Index.3 Stretched";

                if (b == "Right Middle 1 Stretched") return "RightHand.Middle.1 Stretched";
                if (b == "Right Middle 2 Stretched") return "RightHand.Middle.2 Stretched";
                if (b == "Right Middle 3 Stretched") return "RightHand.Middle.3 Stretched";

                if (b == "Right Ring 1 Stretched") return "RightHand.Ring.1 Stretched";
                if (b == "Right Ring 2 Stretched") return "RightHand.Ring.2 Stretched";
                if (b == "Right Ring 3 Stretched") return "RightHand.Ring.3 Stretched";

                if (b == "Right Little 1 Stretched") return "RightHand.Little.1 Stretched";
                if (b == "Right Little 2 Stretched") return "RightHand.Little.2 Stretched";
                if (b == "Right Little 3 Stretched") return "RightHand.Little.3 Stretched";

                if (b == "Right Thumb 1 Stretched") return "RightHand.Thumb.1 Stretched";
                if (b == "Right Thumb 2 Stretched") return "RightHand.Thumb.2 Stretched";
                if (b == "Right Thumb 3 Stretched") return "RightHand.Thumb.3 Stretched";

                if (b == "Right Index Spread") return "RightHand.Index.Spread";
                if (b == "Right Middle Spread") return "RightHand.Middle.Spread";
                if (b == "Right Ring Spread") return "RightHand.Ring.Spread";
                if (b == "Right Little Spread") return "RightHand.Little.Spread";
                if (b == "Right Thumb Spread") return "RightHand.Thumb.Spread";

                return b;
            }

            internal void SaveCurves(ref AnimationClip clip, object bakeReduction, float lengthMlp)
            {
                throw new NotImplementedException();
            }


            #endregion

        }
    }
}