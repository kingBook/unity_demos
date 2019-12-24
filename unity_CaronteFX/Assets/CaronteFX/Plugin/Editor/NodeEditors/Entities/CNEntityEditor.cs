﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CaronteFX
{
  public abstract class CNEntityEditor : CNMonoFieldEditor
  {
    public void CreateEntity()
    {
      if (!IsExcludedInHierarchy)
      {
        CreateEntitySpec();
        LoadState();
      }
    }

    public void ApplyEntity()
    {
      if (!IsExcludedInHierarchy)
      {
        ApplyEntitySpec();
        LoadState();
      }
    }

    public abstract void CreateEntitySpec();

    public abstract void ApplyEntitySpec();

    new CNEntity Data { get; set; }

    public CNEntityEditor( CNEntity data, CommandNodeEditorState state )
      : base( data, state )
    {
      Data = (CNEntity)data;
    }

    public override void Init()
    {
      base.Init();
      
      CNFieldContentType allowedTypes =   CNFieldContentType.Geometry 
                                        | CNFieldContentType.BodyNode;

      FieldController.SetFieldContentType(allowedTypes);
      FieldController.IsBodyField = true;
    }

    public override void FreeResources()
    {
      FieldController.DestroyField();
      eManager.DestroyEntity( Data );
    }

    public override void SetActivityState()
    {
      base.SetActivityState();
      eManager.SetActivity(Data);
    }

    public override void SetVisibilityState()
    {
      base.SetVisibilityState();
      eManager.SetVisibility(Data);
    }

    public override void SetExcludedState()
    {
      base.SetExcludedState();
      if (IsExcludedInHierarchy)
      {
        eManager.DestroyEntity(Data);
      }
      else
      {
        CreateEntity();
      }
    }    

    protected void DrawTimer()
    {
      EditorGUI.BeginChangeCheck();
      var value = EditorGUILayout.FloatField( "Timer (s)", Data.Timer );
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(Data, "Change timer - " + Data.Name);
        Data.Timer = value;
        EditorUtility.SetDirty(Data);
      }
    }
  }

}

