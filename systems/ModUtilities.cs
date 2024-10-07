using System.Collections;
using System.ComponentModel;
using System.Data;
using MIR;
using Walgelijk;
using Walgelijk.AssetManager;

namespace Inputsoldat.SecondChance.systems;

/// <summary>
/// Includes documented methods for mods by inputsoldatcc
/// </summary>
public class ModUtilities
{
    /// <summary>
    /// The time it takes to revive a character.
    /// </summary>
    private const float ReviveTime = 1.9f;

    /// <summary>
    /// The character's look before reviving.
    /// </summary>
    private static CharacterLook? characterLook;

    /// <summary>
    /// If the player was bandaged before going to the main menu.
    /// </summary>
    private static bool bandaged = false;

    /// <summary>
    /// Head bandages 
    /// </summary>
    private static readonly ArmourPiece[] headBandages = [];

    /// <summary>
    /// Random for use in this class
    /// </summary>
    private static readonly Random utilRandom = new();

    /// <summary>
    /// Get the player's look before going to the main menu.
    /// </summary>
    /// <returns>The <see cref="CharacterLook"/> object.</returns>
    public static CharacterLook? GetCharacterLook() => characterLook;

    /// <summary>
    /// Get if the player was bandaged.
    /// </summary>
    /// <returns> The player <see cref="bandaged"/> state before going to the main menu.</returns>
    public static bool GetBandaged() => bandaged;

    /// <summary>
    /// Reset both <see cref="bandaged"/> and <see cref="characterLook"/> vars.
    /// </summary>
    public static void ResetStates()
    {
        bandaged = false;
        characterLook = null;
    }

    
    /// <summary>
    /// Private <see langword="method"/> for <see langword="public"/> "Revive" methods to use.
    /// </summary>
    private static void Util_revive()
    {
        MadnessUtils.Flash(Colors.White, ReviveTime);
        var snd = SoundCache.Instance.LoadMusicNonLoop(Assets.Load<FixedAudioData>("sounds/tricky_revive.wav"));
        Game.Main.AudioRenderer.Play(snd, 4);

        MadnessUtils.Delay(ReviveTime, static () =>
        {
            MadnessCommands.Revive();
        });
    }

    //overload
    /// <summary>
    /// Revives the player and bandages them up.
    /// </summary>
    public static void Revive(CharacterComponent character, Scene scene)
    {
        if (ImprobabilityDisks.IsEnabled("Bandage Disk SC"))
            {
                BodyPartComponent head = scene.GetComponentFrom<BodyPartComponent>(character.Positioning.Head.Entity);
                bool isHead = head.Health == 0;

                BandageCharacter(character, isHead);
            }
        
        Util_revive();
    }

    /// <summary>
    /// Bandages the given player <paramref name="character"/>.
    /// </summary>
    /// <param name="character"> the character to bandage</param>
    /// <param name="limbType"> the limb to bandage</param>
    public static void BandageCharacter(CharacterComponent character, bool head)
    {
        if (headBandages.Length == 0)
            return;
            
        if (bandaged == false)
        {
            characterLook = new();
            character.Look.CopyTo(characterLook);
        }

        bandaged = true;
        if (head)
        {
            ArmourPiece armourPiece = headBandages[utilRandom.Next(0, headBandages.Length)];
            if (armourPiece != null) //Do not override 3rd layer
            {
                ArmourPiece? originalHat1 = character.Look.HeadLayer1;
                ArmourPiece? originalHat2 = character.Look.HeadLayer2;

                character.Look.HeadLayer1 = armourPiece;

                character.Look.HeadLayer2 = originalHat1;
                character.Look.HeadLayer3 = originalHat2;
            }
        }
        else
        {
            Registries.Armour.BodyAccessory.TryGet("bandages1_over", out ArmourPiece? armourPiece);
            if (armourPiece != null) //Do not override 2nd layer
            {
                ArmourPiece? originalArmor = character.Look.BodyLayer1;

                character.Look.BodyLayer1 = armourPiece;
                character.Look.BodyLayer2 = originalArmor;
            }
        }
    }

    /// <summary>
    /// Assign <see cref="ArmourPiece"/> to <see cref="headBandages"/>.
    /// </summary>
    public static void SetBandageTable()
    {
        headBandages[0] = Registries.Armour.HeadAccessory.Get("bandages1_hat");
        headBandages[1] = Registries.Armour.HeadAccessory.Get("mouth_bandages_mouth");
    }
}