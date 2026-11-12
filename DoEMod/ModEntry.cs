using MelonLoader;

[assembly: MelonInfo(typeof(HANDSHIFT.DoE.ModEntry), "HANDSHIFT DoE Compatibility Probe", "0.1.0", "HANDSHIFT")]
[assembly: MelonGame(null, null)]

namespace HANDSHIFT.DoE;

/// <summary>
/// Safe loader smoke-test only. It deliberately does not hook DoE gameplay,
/// synthesize controller input, access cameras, or alter multiplayer state.
/// Game-specific integration is blocked until the exact Quest build and
/// generated IL2CPP interop assemblies have been verified.
/// </summary>
public sealed class ModEntry : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("HANDSHIFT loaded. Compatibility probe only; no gameplay hooks are active.");
        LoggerInstance.Warning("DoE integration is not enabled: target game build and supported input hooks have not been verified.");
    }
}
