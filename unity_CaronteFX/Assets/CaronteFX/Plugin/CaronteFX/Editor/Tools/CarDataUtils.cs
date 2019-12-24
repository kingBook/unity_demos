// ***********************************************************
//	Copyright 2016 Next Limit Technologies, http://www.nextlimit.com
//	All rights reserved.
//
//	THIS SOFTWARE IS PROVIDED 'AS IS' AND WITHOUT ANY EXPRESS OR
//	IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
//	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//
// ***********************************************************

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using CaronteSharp;

namespace CaronteFX
{
  public static class CarDataUtils
  {
    
    public static void GetCaronteFxGameObjects(UnityEngine.Object[] arrObjectReference, out List<GameObject> listGameObject)
    {
      listGameObject = new List<GameObject>();
      int arrObjectReference_size = arrObjectReference.Length;

      for (int i = 0; i < arrObjectReference_size; i++)
      {
        GameObject go = arrObjectReference[i] as GameObject;
        if (go != null)
        {
          if (go.IsInScene())
          {
            if (go.GetComponent<Caronte_Fx>() != null)
            {
              listGameObject.Add(go);
            }
            GameObject[] arrGameObject = CarEditorUtils.GetAllChildObjects(go, true);
            foreach (GameObject childGo in arrGameObject)
            {
              if (childGo.GetComponent<Caronte_Fx>() != null)
              {
                listGameObject.Add(childGo);
              }
            }
          }
        }
      }
    }

    public static void GetCaronteFxsRelations(Caronte_Fx caronteFx, out List<Tuple2<Caronte_Fx, int>> listCaronteFx )
    {
      listCaronteFx = new List<Tuple2<Caronte_Fx, int>>();
      GameObject go = caronteFx.gameObject;
      if ( go.IsInScene() )
      {     
        GameObject[] arrChild = CarEditorUtils.GetAllGameObjectsInScene();
        AddRelations( go, arrChild, listCaronteFx );
      }
    }

    public static void AddRelations(GameObject parentFx, GameObject[] arrGameObject, List<Tuple2<Caronte_Fx, int>> listCaronteFx)
    {
      for (int i = 0; i < arrGameObject.Length; i++)
      {
        GameObject go = arrGameObject[i];
        Caronte_Fx fxChild = go.GetComponent<Caronte_Fx>();
        if (fxChild != null)
        {
          int depth = go.GetFxHierachyDepthFrom(parentFx);
          listCaronteFx.Add(Tuple2.New(fxChild, depth));
        }
      }
    }

    public static void UpdateFxDataVersionIfNeeded(Caronte_Fx fxData)
    {
      GameObject dataHolder = fxData.GetDataGameObject();
      if (fxData.DataVersion < 1)
      {
        CNBody[] arrBodyNode = dataHolder.GetComponents<CNBody>();
        
        foreach(CNBody bodyNode in arrBodyNode)
        {
          bodyNode.OmegaStart_inDegSeg *= Mathf.Rad2Deg;
          EditorUtility.SetDirty(bodyNode);
        }

        fxData.DataVersion = 1;
        EditorUtility.SetDirty(fxData);
        CarDebug.Log("Updated " + fxData.name + " definitions to version 1.");
      }
      if (fxData.DataVersion < 2)
      {
        CNJointGroups[] arrJgGroups = dataHolder.GetComponents<CNJointGroups>();
        foreach(CNJointGroups jgGroups in arrJgGroups)
        {
          jgGroups.ContactAngleMaxInDegrees *= Mathf.Rad2Deg;
          if (jgGroups.ContactAngleMaxInDegrees > 180f ||
              jgGroups.ContactAngleMaxInDegrees < 0f      )
          {
            jgGroups.ContactAngleMaxInDegrees -= (jgGroups.ContactAngleMaxInDegrees % 180f) * 180f;
            EditorUtility.SetDirty(jgGroups);
          }
        }
        fxData.DataVersion = 2;
        EditorUtility.SetDirty(fxData);
        CarDebug.Log("Updated " + fxData.name + " definitions to version 2.");
      }
      if (fxData.DataVersion < 3)
      {
        CNSoftbody[] arrSoftbodyNode = dataHolder.GetComponents<CNSoftbody>();
        foreach(CNSoftbody sbNode in arrSoftbodyNode)
        {
          sbNode.LengthStiffness = Mathf.Clamp(sbNode.LengthStiffness, 0f, 30f);
          EditorUtility.SetDirty(sbNode);
        }
        fxData.DataVersion = 3;
        EditorUtility.SetDirty(fxData);
        CarDebug.Log("Updated " + fxData.name + " definitions to version 3.");
      }
      if (fxData.DataVersion < 4)
      {
        CNFracture[] arrFractureNode = dataHolder.GetComponents<CNFracture>();
        foreach(CNFracture frNode in arrFractureNode)
        {
          if (frNode.ChopGeometry != null)
          {
            frNode.FieldSteeringGeometry.GameObjects.Add(frNode.ChopGeometry);
            EditorUtility.SetDirty(frNode);
          }
          if (frNode.CropGeometry != null)
          {
            frNode.FieldRestrictionGeometry.GameObjects.Add(frNode.CropGeometry);
            EditorUtility.SetDirty(frNode);
          }       
        }
        fxData.DataVersion = 4;
        EditorUtility.SetDirty(fxData);
        CarDebug.Log("Updated " + fxData.name + " definitions to version 4.");
      }
      if (fxData.DataVersion < 5)
      {
        CNServos[] arrServos = dataHolder.GetComponents<CNServos>();
        foreach(CNServos svNode in arrServos)
        {
          svNode.TargetExternal_LOCAL_NEW = new CarVector3Curve(svNode.TargetExternal_LOCAL, fxData.effect.totalTime_);
          EditorUtility.SetDirty(svNode);
        }
        fxData.DataVersion = 5;
        EditorUtility.SetDirty(fxData);
        CarDebug.Log("Updated " + fxData.name + " definitions to version 5.");
      }
    }

    public static void SetParameterModifierCommand(ref CaronteSharp.PmCommand pmCommand, ParameterModifierCommand parameterModifierCommand)
    {
      switch ( parameterModifierCommand.target_ )
      {
        case PARAMETER_MODIFIER_PROPERTY.VELOCITY_LINEAL:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.VELOCITY_LINEAL;
          pmCommand.valueVector3_ = parameterModifierCommand.valueVector3_;
        break;

        case PARAMETER_MODIFIER_PROPERTY.VELOCITY_ANGULAR:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.VELOCITY_ANGULAR;
          pmCommand.valueVector3_ = parameterModifierCommand.valueVector3_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.ACTIVITY:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.ACTIVITY;
          pmCommand.valueIndex_ = (uint)parameterModifierCommand.valueInt_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.VISIBILITY:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.VISIBILITY;
          pmCommand.valueIndex_ = (uint)parameterModifierCommand.valueInt_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.FORCE_MULTIPLIER:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.FORCE_MULTIPLIER;
          pmCommand.valueVector3_ = parameterModifierCommand.valueVector3_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.EXTERNAL_DAMPING:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.EXTERNAL_DAMPING;
          pmCommand.valueVector3_ = parameterModifierCommand.valueVector3_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.FREEZE:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.FREEZE;
          pmCommand.valueIndex_ = (uint)parameterModifierCommand.valueInt_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.PLASTICITY:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.PLASTICITY;
          pmCommand.valueIndex_ = (uint)parameterModifierCommand.valueInt_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.TARGET_POSITION:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.TARGET_POSITION;
          pmCommand.valueVector3_ = parameterModifierCommand.valueVector3_;
          break;

        case PARAMETER_MODIFIER_PROPERTY.TARGET_ANGLE:
          pmCommand.target_ = CaronteSharp.PARAMETER_MODIFIER_PROPERTY.TARGET_ANGLE;
          pmCommand.valueVector3_ = parameterModifierCommand.valueVector3_;
          break;

        default:
          throw new NotImplementedException();
      }
    }
  }
}
