using System;
using UnityEngine;

public class SCR_FunctionUpdater : MonoBehaviour
{
   private static GameObject updaterOject;
   private static SCR_FunctionUpdater instance;
   
   private Func<bool> updateFunction;

   public static SCR_FunctionUpdater Create(Func<bool> updateFunc)
   {
      if (instance == null)
      {
         updaterOject = new GameObject("FunctionUpdater");
         instance = updaterOject.AddComponent<SCR_FunctionUpdater>();
      }
      
      return instance.AddUpdater(updateFunc);
   }

   private SCR_FunctionUpdater AddUpdater(Func<bool> updateFunc)
   {
      updateFunction = updateFunc;
      return this;
   }

   private void Update()
   {
      if (updateFunction != null && updateFunction.Invoke())
      {
         Destroy(this);
      }
   }
}

