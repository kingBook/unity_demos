﻿using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CaronteFX
{
  public class CNRigidGlueEditor : CNJointGroupsEditor
  {
    public static Texture icon_rigid_glue_;

    public override Texture TexIcon
    {
      get
      { 
        return icon_rigid_glue_;
      } 
    }

    bool isMutated_ = false;
    //-----------------------------------------------------------------------------------
    public CNRigidGlueEditor( CNJointGroups data, CommandNodeEditorState state )
      : base( data, state )
    {

    }
    //-----------------------------------------------------------------------------------
    public override void Init()
    {
      base.Init();

      CNFieldContentType allowedTypes =   CNFieldContentType.Geometry 
                                        | CNFieldContentType.RigidBodyNode
                                        | CNFieldContentType.IrresponsiveNode;

      FieldControllerA.SetFieldContentType( allowedTypes );
      FieldControllerB.SetFieldContentType( allowedTypes );
    }
    //-----------------------------------------------------------------------------------
    public override void DestroyEntities()
    {
      GameObject[] arrGameObjectA;
      GameObject[] arrGameObjectB;

      GetFieldGameObjects( FieldControllerA, out arrGameObjectA );
      GetFieldGameObjects( FieldControllerB, out arrGameObjectB );

      if (isMutated_)
      {
        eManager.DestroyServoGroup( Data );
        isMutated_ = false;
      }
      else
      {
        eManager.DestroyMultiJoint( Data, arrGameObjectA, arrGameObjectB );
      }
    }
    //-----------------------------------------------------------------------------------
    public void RecreateEntitiesAsServos()
    {
      RecreateEntities();
      isMutated_ = eManager.CreateServoGroupReplacingJointGroups(Data);
    }
    //-----------------------------------------------------------------------------------
    public override void RenderGUI( Rect area, bool isEditable )
    {
      GUILayout.BeginArea( area );
      
      RenderTitle(isEditable, true, true, true);
        
      EditorGUI.BeginDisabledGroup(!isEditable);
      {
        RenderFieldObjects( jgObjectsAFieldCt_, FieldControllerA, true, true, CNFieldWindow.Type.extended );
        RenderFieldObjects( jgObjectsBFieldCt_, FieldControllerB, true, true, CNFieldWindow.Type.extended );

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button(Data.NeedsUpdate ? CarStringManager.GetString("CreateRecreate*") : CarStringManager.GetString("CreateRecreate"), GUILayout.Height(30f) ) )
        {
          RecreateEntities();
        }
      }
      EditorGUI.EndDisabledGroup();

      CarGUIUtils.DrawSeparator();
      CarGUIUtils.Splitter();

      scroller_ = EditorGUILayout.BeginScrollView(scroller_);

      float originalLabelwidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 200f;

      EditorGUILayout.Space();
      EditorGUILayout.Space();

      EditorGUI.BeginDisabledGroup(!isEditable);
      {
        EditorGUI.BeginChangeCheck();
        {
          RenderCreationParams();
        }
        if( EditorGUI.EndChangeCheck() && eManager.IsMultiJointCreated(Data) )
        {
          DestroyEntities();
        }

        EditorGUILayout.Space();

        CarGUIUtils.Splitter();    
        EditorGUILayout.Space();

        //FORCES
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(MaximumForce);
        DrawForceMax();
        EditorGUI.EndDisabledGroup();
        DrawMaximumForce();   
        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(MaximumForce);
        DrawForceMaxRand();
        DrawForceRange();
        DrawForceProfile();
        EditorGUI.EndDisabledGroup();
      }
      EditorGUI.EndDisabledGroup();

      CarGUIUtils.Splitter();
      EditorGUILayout.Space();

      //BREAK
      EditorGUILayout.BeginHorizontal();
      Data.BreakFoldout = EditorGUILayout.Foldout(Data.BreakFoldout, "Break if distance exceeded" );

      EditorGUI.BeginDisabledGroup(!isEditable);
      {
        GUILayout.Space(145f);
        DrawBreakIfDistanceExceeded();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        {
          if (Data.BreakFoldout)
          {
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(!Data.BreakIfDistExcedeed);
            DrawDistanceForBreak();
            DrawDistanceForBreakRand();
            EditorGUI.EndDisabledGroup();
            DrawEnableCollisionIfBreak();
          }
        }
      }
      EditorGUI.EndDisabledGroup();

      CarGUIUtils.Splitter();
      EditorGUILayout.Space();

      //PLASTICITY
      EditorGUILayout.BeginHorizontal();
      Data.PlasticityFoldout = EditorGUILayout.Foldout(Data.PlasticityFoldout, "Plasticity");

      EditorGUI.BeginDisabledGroup(!isEditable);
      {
        GUILayout.Space(145f);
        DrawPlasticity();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (Data.PlasticityFoldout)
        {
          EditorGUILayout.Space();
          
          EditorGUI.BeginDisabledGroup(!Data.Plasticity);
          DrawDistanceForPlasticity();
          DrawDistanceForPlasticityRand();
          DrawPlasticityRateAcquired();
          EditorGUI.EndDisabledGroup();
        }
      }  
      EditorGUI.EndDisabledGroup();

      CarGUIUtils.Splitter();
      EditorGUIUtility.labelWidth = originalLabelwidth;

      EditorGUILayout.EndScrollView();
      GUILayout.EndArea();
    } // RenderGUI
    //-----------------------------------------------------------------------------------
    private void RenderCreationParams()
    {
      DrawContactDistanceSearch();
      DrawContactAreaMin();
      DrawContactAngleMaxInDegrees();
      GUILayout.Space(simple_space);

      EditorGUILayout.Space();

      DrawDisableCollisionsByPairs();
      DrawDisableAllCollisionsOfAsWithBs();
    }
    //-----------------------------------------------------------------------------------
  }
}
