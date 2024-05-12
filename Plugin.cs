using BepInEx;
using OGAT_modding_API;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using CodeStage.AntiCheat.Detectors;
using SG.OGAT;
using SG.OGAT.State;
using SG.Util;
using System.Reflection;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Collections;


namespace OGAT_InfiniteAmmo_Mod
{
    [BepInPlugin("OGAT_InfiniteAmmo_mod", "infinite ammo mod", "1.0.0")]
    [BepInDependency("OGAT_modding_API")]
    public class Plugin : BaseUnityPlugin
    {
        public static bool Toggled = false;
        public static bool Active = false;
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        public void Start()
        {
            OGAT_modding_API.API_Methods.commands.Add("InAmmo", InfiniteAmmo_Methods.Toggle_InfiniteAmmo);
        }
        public void Update()
        {
            if(Active)
            {
                StartCoroutine(InfiniteAmmo_Methods.InfiniteAmmo());
            }
        }


    }

    public class InfiniteAmmo_Methods
    {
        public static bool Toggle_InfiniteAmmo(List<string> comm_and_user)
        {
            //will need to find host username


            if (Plugin.Active == false)
            {
                Plugin.Active = true;
                Plugin.Toggled = true;
            }
            else if(Plugin.Active == true)
            {
                Plugin.Active = false;
            }

            var myLogSource = new ManualLogSource("OGAT_INFINITE_AMMO_MOD");
            BepInEx.Logging.Logger.Sources.Add(myLogSource);
            myLogSource.LogInfo($"Toggled command: {Plugin.Active}");
            BepInEx.Logging.Logger.Sources.Remove(myLogSource);

            return true;
        }

        static public IEnumerator InfiniteAmmo()
        {

            foreach (NetPlayer player in NetPlayer.All)
                {
                    Weapon wep = player.BLIEEKHAIPK;
                    if (wep.bulletsLeft.val == 0)
                    {
                        if (wep.settings.magazineType == null)
                        {
                            wep.bulletsLeft.val = 3;
                        }
                        else
                        {
                            wep.bulletsLeft.val = (float)wep.settings.magazineType.maxNumBullets;
                        }
                    }
                }

            yield return null;
        }
    }
}