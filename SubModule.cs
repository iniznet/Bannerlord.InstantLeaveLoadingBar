using System.Reflection;
using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace InstantLeaveLoadingBar
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Messenger.Prefix("InstantLeaveLoadingBar");
            new Harmony("mod.bannerlord.InstantLeaveLoadingBar").PatchAll(Assembly.GetExecutingAssembly());
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            Messenger.Log("Mod loaded", Messenger.Level.Success);
        }
    }
}
