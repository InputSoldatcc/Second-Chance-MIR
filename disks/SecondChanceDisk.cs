using System;
using MIR;
using Walgelijk;
using Walgelijk.AssetManager;

namespace Inputsoldat.SecondChance.Disks;

/// <summary>
/// Second Chance disk class and info
/// </summary>
public class SecondChanceDisk : ImprobabilityDisk
{
    public SecondChanceDisk() : base(
        "Second Chance",
        Assets.Load<Texture>("textures/revival.png").Value,
        "Fetching S3LFs...")
    { }
}

