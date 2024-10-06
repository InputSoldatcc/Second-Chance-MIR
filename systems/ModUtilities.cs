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
public struct ModUtilities
{
    /// <summary>
    /// The time it takes to revive a character.
    /// </summary>
    private const float ReviveTime = 1.8f;

    /// <summary>
    /// The character's look before reviving.
    /// </summary>
    private static CharacterLook? characterLook;

    /// <summary>
    /// If the player was bandaged before going to the main menu.
    /// </summary>
    private static bool bandaged = false;

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
            BandageCharacter(character);
        
        Util_revive();
    }

    /// <summary>
    /// Changes the look of the given <paramref name="bodyPart"/>
    /// </summary>
    public static void BandageCharacter(CharacterComponent character)
    {
        Random random = new();
        int headOrBody = random.Next(0, 2);

        if (!bandaged)
            characterLook = character.Look;
        
        if (headOrBody == 1) //Head
        {
            Registries.Armour.HeadAccessory.TryGet("bandages1_hat", out ArmourPiece? armourPiece);
            if (armourPiece != null)
                character.Look.HeadLayer1 = armourPiece;
        }
        else                 //Body
        {
            Registries.Armour.BodyAccessory.TryGet("bandages1_over", out ArmourPiece? armourPiece);
            if (armourPiece != null)
                character.Look.BodyLayer1 = armourPiece;
        }

        bandaged = true;
    }
}