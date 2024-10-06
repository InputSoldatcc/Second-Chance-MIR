using System;
using MIR;
using Walgelijk;
using Walgelijk.AssetManager;

namespace Inputsoldat.SecondChance.Disks;

/// <summary>
/// Second Chance disk with bandages.
/// </summary>
public class FirstAidKitDisk : ImprobabilityDisk
{
    public FirstAidKitDisk() : base(
        "First Aid Kit",
        Assets.Load<Texture>("textures/bandages.png").Value,
        "A couple of bandages will do the trick! Signed by HJW himself! This is basically the second chance disk with bandages.")
    { }
}
