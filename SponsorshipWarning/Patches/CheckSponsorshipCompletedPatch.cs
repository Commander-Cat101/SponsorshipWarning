using HarmonyLib;
using Photon.Pun;
using SponsorshipWarning.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace SponsorshipWarning.Patches
{
    [HarmonyPatch(typeof(UploadCompleteUI), nameof(UploadCompleteUI.DisplayVideoEval))]
    public static class CheckSponsorshipCompletedPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log("Patching VideoEval...");

            var codes = new List<CodeInstruction>(instructions);

            int index = codes.FindIndex(a => a.opcode == OpCodes.Ldfld && (MethodInfo)a.operand == AccessTools.Field(typeof(UploadCompleteUI), nameof(UploadCompleteUI.m_onPlayed)));
            index += 5;

            Debug.Log($"Index: {index}");

            codes.InsertRange(index, new List<CodeInstruction>()
            {
                new CodeInstruction(OpCodes.Ldarg_3),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CheckSponsorshipCompletedPatch), nameof(CheckSponsorshipCompletedPatch.CheckIfComplete)))
            });
            return codes.AsEnumerable();
        }
        public static void CheckIfComplete(CameraRecording recording)
        {
            var frames = recording.m_clips.SelectMany(a => a.m_contentBuffer.buffer.Select(a => a.frame)).ToList();

            if (SponsorHandler.sponsor.CompletedSponsor(frames))
            {
                UserInterface.ShowMoneyNotification("Sponsor Money", $"${SponsorHandler.sponsor.CashReward}", true);

                if (PhotonNetwork.IsMasterClient)
                {
                    SurfaceNetworkHandler.RoomStats.AddMoney(SponsorHandler.sponsor.CashReward);
                }
            }
        }
    }
}
