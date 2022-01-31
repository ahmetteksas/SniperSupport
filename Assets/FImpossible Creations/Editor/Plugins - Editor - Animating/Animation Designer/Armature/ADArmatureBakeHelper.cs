using UnityEngine;

namespace FIMSpace.AnimationTools
{
    public partial class ADArmatureBakeHelper
    {
        public ADArmatureSetup Armature;
        public Transform Root { get { return Armature.Root; } }
        public ADArmatureSetup Ar { get { return Armature; } }
        public Transform anim { get { return Ar.LatestAnimator; } }

        private HumanPose humanoidPose = new HumanPose();
        private HumanPoseHandler humanoidPoseHandler;

        public Vector3 bodyPosition { get; private set; }
        public Quaternion bodyRotation { get; private set; }
        public Quaternion? lastBodyRotation { get; private set; }

        public Vector3 initRootBonePosition { get; private set; }
        public Quaternion initRootBoneRotation { get; private set; }


        private float[] muscles = new float[0];
        private ADHumanoidMuscle[] muscleHelpers;
        public AnimationClip OriginalBakedClip { get; private set; }
        public bool Humanoid { get; private set; }
        public bool BakeRoot { get; private set; }

        public ADArmatureBakeHelper(ADArmatureSetup armature, AnimationClip originalClip)
        {
            Armature = armature;
            OriginalBakedClip = originalClip;
        }


        public void PrepareAndDefine()
        {
            BakeRoot = false;
            Humanoid = false;

            if (anim.IsHuman())
            {
                Humanoid = true;
                BakeRoot = true;

                muscles = new float[HumanTrait.MuscleCount];

                muscleHelpers = new ADHumanoidMuscle[HumanTrait.MuscleCount];
                for (int i = 0; i < muscleHelpers.Length; i++) muscleHelpers[i] = new ADHumanoidMuscle(i);

                humanoidPoseHandler = new HumanPoseHandler(anim.GetAvatar(), Ar.LatestAnimator.transform);
                initRootBoneRotation = Armature.Root.rotation;
            }
            else
            {
                //if (OriginalBakedClip.hasRootCurves)
                {
                    BakeRoot = true;
                }

                //initRootBoneRotation = FEngineering.QToLocal(Root.parent.rotation, Armature.Root.rotation);
                initRootBoneRotation = Quaternion.FromToRotation(Armature.Root.InverseTransformDirection(anim.right), Vector3.right);
                initRootBoneRotation *= Quaternion.FromToRotation(Armature.Root.InverseTransformDirection(anim.up), Vector3.up);
            }

            lastBodyRotation = null;
            initRootBonePosition = Armature.Root.position;
        }


        public void UpdateHumanoidBodyPose()
        {
            humanoidPoseHandler.GetHumanPose(ref humanoidPose);

            bodyPosition = humanoidPose.bodyPosition;
            bodyRotation = humanoidPose.bodyRotation;

            for (int i = 0; i < humanoidPose.muscles.Length; i++)
            {
                muscles[i] = humanoidPose.muscles[i];
            }
        }

        public void CaptureArmaturePoseFrame(float elapsed)
        {

            if (Humanoid)
            {
                UpdateHumanoidBodyPose();
                for (int i = 0; i < muscleHelpers.Length; i++) muscleHelpers[i].SetKeyframe(elapsed, muscles);
            }
            else
            {
                bodyPosition = Root.position - initRootBonePosition;
                //bodyRotation = Quaternion.Inverse(Root.rotation) * (initRootBoneRotation);
                //bodyRotation = FEngineering.QToLocal(anim.rotation, bodyRotation);
                //Quaternion diff = FEngineering.QToLocal(Root.parent.rotation, Root.rotation);
                //bodyRotation = Quaternion.Inverse(diff) * (initRootBoneRotation);
                bodyRotation = (Root.rotation) * Quaternion.Inverse(initRootBoneRotation) ;
                //bodyRotation = FEngineering.QToLocal(anim.rotation,Root.rotation) * (initRootBoneRotation);
                //UnityEngine.Debug.Log("initrot = " + initRootBoneRotation.eulerAngles + " vs curr " + Root.eulerAngles + " rootbodyrot = " + bodyRotation.eulerAngles);
            }

            if (lastBodyRotation != null) AnimationGenerateUtils.EnsureQuaternionContinuity(lastBodyRotation.Value, bodyRotation);
            lastBodyRotation = bodyRotation;

        }

        public void SaveHumanoidCurves(ref AnimationClip clip, float reduction)
        {
            if (Humanoid)
            {
                for (int i = 0; i < muscleHelpers.Length; i++)
                {
                    muscleHelpers[i].SaveCurves(ref clip, reduction);
                }
            }
        }


    }
}