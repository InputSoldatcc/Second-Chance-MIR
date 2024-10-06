using Inputsoldat.SecondChance.systems;
using MIR;
using OpenTK.Mathematics;
using Walgelijk;
using Walgelijk.AssetManager;

namespace Inputsoldat.SecondChance.Systems;

/// <summary>
/// SecondChanceRevive <see langword="class"/> for reviving the player when SecondChance disk is enabled
/// </summary>
public class SecondChanceRevive : Walgelijk.System
{
    /// <summary>
    /// Whether the system should listen to death events or not.
    /// </summary>
    private bool shouldListen = true;
    
    /// <summary>
    /// Looping method.
    /// </summary>
    public override void Update()
    {
        var experimentMode = MadnessUtils.EditingInExperimentMode(Scene);
        bool SecondChanceEnabled = ImprobabilityDisks.IsEnabled("Second Chance") || ImprobabilityDisks.IsEnabled("Bandage Disk SC");

        if (MadnessUtils.IsPaused(Scene) || MadnessUtils.IsCutscenePlaying(Scene))
            return;
        if (!MadnessUtils.FindPlayer(Scene, out var player, out var character))
            {
                shouldListen = true; //"In case of emergency, break glass" ahh tactic :skull:
                return;
            }   
        if (!SecondChanceEnabled)
            return;

        if (Scene.HasSystem<MainMenuSystem>())
        {
            bool bandaged = ModUtilities.GetBandaged();

            if (bandaged) //Then reset the character's look
            {
                CharacterLook? characterLook = ModUtilities.GetCharacterLook();
                MadnessUtils.FindPlayer(Game.Main.Scene, out var _, out CharacterComponent? ch);

                ModUtilities.ResetStates();

                if (characterLook != null && ch != null)
                {
                    ch.Look = characterLook;
                }
            }
        }

        //Music Reenable
        if (PersistentSoundHandles.LevelMusic != null && !Audio.IsPlaying(PersistentSoundHandles.LevelMusic))
        {
            Audio.Play(PersistentSoundHandles.LevelMusic);
        }

        //Character death listening
        if (character.IsAlive && player.RespondToUserInput && shouldListen == true)
        {
            shouldListen = false;
            
            character.OnDeath.AddListener(ch =>
            {
                ModUtilities.Revive(character, Scene);
                shouldListen = true;
            });
        }
    }
}
