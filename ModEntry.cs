using MIR;
using Inputsoldat.SecondChance.Disks;
using HarmonyLib;
using Walgelijk;
using Inputsoldat.SecondChance.Systems;
using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using Inputsoldat.SecondChance.systems;
//best mods at inputsoldatindustries

namespace Inputsoldat.SecondChance;

public class ModEntry : IModEntry
{
    /// <summary>
    /// Called immediately after this mod is loaded. Beware that some resources might not yet be present.
    /// </summary>
    /// <param name="mod">Your mod instance</param>
    /// <param name="harmony">Your Harmony instance</param>
    public void OnLoad(Mod mod, Harmony harmony) {}

    /// <summary>
    /// Called when everything is ready.
    /// </summary>
    public void OnReady()
    {
        //Insert System and reset bandage character
        Game.Main.OnSceneChange.AddListener(scene =>
        {
            if (scene.New != null)
            {
                //Insert system
                if (!scene.New.HasSystem<SecondChanceRevive>())
                    scene.New.AddSystem(new SecondChanceRevive());

                //Reset bandaged character
                if (Game.Main.Scene.HasSystem<MainMenuSystem>())
                {
                    bool bandaged = ModUtilities.GetBandaged();
                    CharacterLook? characterLook = ModUtilities.GetCharacterLook();
                    MadnessUtils.FindPlayer(Game.Main.Scene, out var _, out CharacterComponent? character);

                    ModUtilities.ResetStates();

                    if (characterLook != null && character != null)
                    {
                        character.Look = characterLook;
                    }
                }
            }
        });

        
        //Insert Improbability and destruction
        ImprobabilityDisks.All.Add("Second Chance", new SecondChanceDisk());
        ImprobabilityDisks.All.Add("Bandage Disk SC", new FirstAidKitDisk());
        ImprobabilityDisks.SetIncompatible("Second Chance", "Bandage Disk SC", "tricky");
    }

    /// <summary>
    /// Called when the game closes and this mod is unloaded.
    /// </summary>
    public void OnUnload() {}
}

