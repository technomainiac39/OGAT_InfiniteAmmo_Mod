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
using Steamworks;

namespace OGAT_InfiniteAmmo_Mod
{
    [BepInPlugin("OGAT_InfiniteAmmo_mod", "infinite ammo mod", MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("OGAT_modding_API", "1.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static bool Toggled = false;
        public static bool Active = false;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            var harmony = new Harmony("com.technomainiac.OGAT_InfiniteAmmo_mod");
            harmony.PatchAll();
        }

        public void Start()
        {
            OGAT_modding_API.API_Methods.commands.Add("InAmmo", InfiniteAmmo_Methods.Toggle_InfiniteAmmo);
            ServerPatches.ConnectedToNewServer += s_ConnectedToNewServer;       //subscribes the method to the event
        }
        public void Update()
        {
            if(Active)
            {
                StartCoroutine(InfiniteAmmo_Methods.InfiniteAmmo());
            }
        }

        protected static void s_ConnectedToNewServer()
        {
            Active = false;


            var myLogSource = new ManualLogSource("OGAT_INFINITE_AMMO_MOD");
            BepInEx.Logging.Logger.Sources.Add(myLogSource);
            myLogSource.LogInfo($"Toggled command: {Plugin.Active}");
            BepInEx.Logging.Logger.Sources.Remove(myLogSource);
        }


    }

    public class InfiniteAmmo_Methods
    {
        public static bool Toggle_InfiniteAmmo(List<string> comm_and_user)
        {
            //will need to find host username
            if (comm_and_user[1] == ServerPatches.Hostname)
            { 
                if (Plugin.Active == false)
                {
                    Plugin.Active = true;
                    Plugin.Toggled = true;
                }
                else if (Plugin.Active == true)
                {
                    Plugin.Active = false;
                }

                var myLogSource = new ManualLogSource("OGAT_INFINITE_AMMO_MOD");
                BepInEx.Logging.Logger.Sources.Add(myLogSource);
                myLogSource.LogInfo($"Toggled command: {Plugin.Active}");
                BepInEx.Logging.Logger.Sources.Remove(myLogSource);

                return true;


            }
            else 
            {
                API_Methods.SendLobbyMessage(string.Empty, "You are not the host, only the host can activate this command", true);
                return true;
            }
           
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