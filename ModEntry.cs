using System;
using Godot;
using GunslingerMod.Framework.Bootstrap;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

[ModInitializer("Initialize")]
public class ModEntry
{
    public static void Initialize()
    {
        try
        {
            GD.Print("[Gunslinger] ModEntry.Initialize start");

            GunslingerBootstrap.Initialize();

            var harmony = new Harmony("gunslingermod.patch");
            harmony.PatchAll();

            GD.Print("[Gunslinger] Harmony.PatchAll complete");
        }
        catch (Exception ex)
        {
            GD.Print($"[Gunslinger] ModEntry.Initialize exception: {ex}");
        }
    }
}
