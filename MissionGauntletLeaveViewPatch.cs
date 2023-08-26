using HarmonyLib;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI.Mission.Singleplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace InstantLeaveLoadingBar
{
    [HarmonyPatch]
    public class MissionGauntletLeaveViewPatch
    {
        protected static bool isFirstPress = true;
        protected static float doubleTapThreshold = 0.5f;
        protected static float lastPressTime = 0f;
        protected static GameKey leaveKey = HotKeyManager.GetCategory("Generic").GetGameKey("Leave");

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MissionGauntletLeaveView), "OnMissionTick")]
        static void ShouldPlayerLeaveInstantly(ref MissionLeaveVM ____dataSource)
        {
            Mission currentMission = Mission.Current;
            float currentTime = currentMission.CurrentTime;
            bool leaveKeyPressed = Input.IsPressed(leaveKey.KeyboardKey.InputKey) || Input.IsPressed(leaveKey.ControllerKey.InputKey);
            bool ctrlKeyPressed = Input.IsDown(InputKey.LeftControl) || Input.IsDown(InputKey.RightControl);
            bool shouldLeave = checkDoubleTap(currentTime, leaveKeyPressed) || checkInstantLeave(ctrlKeyPressed, leaveKeyPressed);

            if (!shouldLeave)
            {
                return;
            }

            LeaveMission(currentMission, ____dataSource);
        }

        private static bool checkDoubleTap(float currentTime, bool leavePressed)
        {
            if (!leavePressed)
            {
                return false;
            }

            if (isFirstPress)
            {
                isFirstPress = false;
                lastPressTime = currentTime;
                return false;
            }

            if (!(currentTime - lastPressTime < doubleTapThreshold))
            {
                isFirstPress = true;
                lastPressTime = currentTime;
                return false;
            }

            isFirstPress = true;
            lastPressTime = 0f;

            return true;
        }

        private static bool checkInstantLeave(bool ctrlPressed, bool leavePressed)
        {
            return ctrlPressed && leavePressed;
        }

        private static void LeaveMission(Mission currentMission, MissionLeaveVM dataSource)
        {
            // Check if player is not allowed to access characters & party
            if (!currentMission.IsCharacterWindowAccessAllowed || !currentMission.IsPartyWindowAccessAllowed)
            {
                return;
            }

            dataSource.CurrentTime = dataSource.MaxTime;
            currentMission.EndMission();
        }
    }
}