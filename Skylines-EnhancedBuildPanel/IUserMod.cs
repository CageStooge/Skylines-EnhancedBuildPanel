using System;
using System.Collections.Generic;
using ColossalFramework.UI;
using EnhancedBuildPanel.GUI_2;
using ICities;
using UnityEngine;
using Object = UnityEngine.Object;


namespace EnhancedBuildPanel
{

    public class EnhancedBuildPanel : LoadingExtensionBase, IUserMod
    {
        public static Transform transform;
        private static readonly string ModName = "Enhanced Build Panel";

        public static string Version
        {
            get { return "0.5.5"; }
        }
        
        public static string Acronym
        {
            get { return "EBP"; }
        }
        
        public string Name
        {
            get { return ModName; }
        }
        
        public string Description
        {
            get { return "Load a save then hit Ctrl + M to get information about asset meshes"; }
        }



        public override void OnLevelLoaded(LoadMode mode)
        {
            myThreading2.DestroyMod();
            //myThreading2.BuildMod();
        }

        /// <summary>
        ///     Called when the level is unloaded
        /// </summary>
        public override void OnLevelUnloading()
        {
            //myThreading2.DestroyMod();
        }
    }

    public class myThreading2 : ThreadingExtensionBase
    {
        
        private static GameObject _gameObject;
        private static UIMainPanel _mainPanel;
        private static bool isCreated;

        public override void OnCreated(IThreading threading)
        {

            DestroyMod();

        }

        public static void BuildMod()
        {
            try
            {
                
                var view = UIView.GetAView();
                /*
                if (isCreated)
                {
                    Debug.Log(
                        "Panel is already created, please press Ctrl+Shift+D to destroy it before creating a new one");
                    return;
                }
                */
                _gameObject = new GameObject();
                if (_gameObject == null) throw new NullReferenceException();
                _gameObject.transform.SetParent(view.transform);
                Debug.Log("Adding Component");
                _mainPanel = _gameObject.AddComponent<UIMainPanel>();
                isCreated = true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Couldn't create the UI. Try relaunching the game.");
                Debug.LogException(e);
            }   
        }

        public static void DestroyMod()
        {
            /*
            while (true)
                    try
                    {


                        var view = UIView.GetAView();
                        {
                            
                            Debug.Log(string.Format("Checking to see if there is any existing panel in {0}",view));
                            UIComponent component = view.FindUIComponent<UIMainPanel>("EBP");

                            if (component == null)
                            {
                                return;
                            }
                            Debug.Log("Removing Existing Mod version " + EnhancedBuildPanel.Version);

                            GameObject.DestroyImmediate(component.gameObject);
                            Debug.Log("Removed Existing Mod version " + EnhancedBuildPanel.Version);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(
                            string.Format("Error during onUpdate while trying to find any UIMainPanel objects \n {0}", e));
                    }

            }
*/
                    try
                    {
                        Debug.Log("Removing Existing Mod version " + EnhancedBuildPanel.Version);
                        var view = UIView.GetAView();
                        
                        
                        var deleteObjects = GameObject.Find(EnhancedBuildPanel.Acronym);
                        if (deleteObjects == null)
                        {
                            Debug.Log("No Panel Found");
                            BuildMod();
                        }
                        for (int i = deleteObjects.transform.childCount - 1; i >= 0; --i)
                        {
                            if (deleteObjects.transform.GetChild(i).name == null) { return; }
                            Debug.Log(string.Format("Deleting child item {0}", deleteObjects.transform.GetChild(i).name));
                            GameObject.DestroyImmediate(deleteObjects.transform.GetChild(i).gameObject);
                        }
                        deleteObjects.transform.DetachChildren();
                        
                        GameObject.DestroyImmediate(GameObject.Find(EnhancedBuildPanel.Acronym));
                        Debug.Log("Refreshing UI");
                        UIView.RefreshAll(true);
                        //isCreated = false;


                    }
                    catch (Exception e)
                    {
                        Debug.Log("Failed to Remove Existing Mod \n Exception: " + e);
                    }

                }


        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    BuildMod();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    DestroyMod();
                }
            }
        }
    }
}