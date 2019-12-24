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

namespace CaronteFX
{

  public class CarAboutWindow: CarWindow<CarAboutWindow>
  {
    public enum CompressionType
    {
      Normal,
      Advanced
    }
    
    Texture companyIcon_;
    string versionString_;

    [Flags]
    public enum VersionType
    {
      Free       = 1,
      Evaluation = 2,
      Pro        = 4,
      Premium    = 8,
      None       = 0
    }

    VersionType versionType_;
    CompressionType compressionType_; 
    DateTime expirationDateTime_;

    public static CarAboutWindow ShowWindow()
    {
      if (Instance == null)
      {
        Instance = (CarAboutWindow)EditorWindow.GetWindow(typeof(CarAboutWindow), true, "About CaronteFX");
      }

      float width = 380f;
      float height = 210f;

      Instance.minSize = new Vector2(width, height);
      Instance.maxSize = new Vector2(width, height);
    
      Instance.Focus();    
      return Instance;
    }

    void OnEnable()
    {
      string version = CaronteSharp.Caronte.GetNativeDllVersion();
      string versionTypeName;

      if (CarVersionChecker.IsFreeVersion() )
      {
        versionTypeName = " FREE";
        versionType_ = VersionType.Free;
      }
      else if ( CarVersionChecker.IsPremiumVersion() )
      {
        if ( CarVersionChecker.IsEvaluationVersion() )
        {
          versionTypeName = " PREMIUM TRIAL";
          versionType_ = VersionType.Premium | VersionType.Evaluation;
        }
        else
        {
          versionTypeName = " PREMIUM";
          versionType_ = VersionType.Premium;
        }
      }
      else // PRO VERSION
      {
        if (CarVersionChecker.IsEvaluationVersion() )
        {
          versionTypeName = " PRO TRIAL";
          versionType_ = VersionType.Pro | VersionType.Evaluation;
        }
        else
        {
          versionTypeName = " PRO";
          versionType_ = VersionType.Pro;
        }
      }

      if (CarVersionChecker.DoVersionExpires())
      {
        expirationDateTime_ = CarVersionChecker.GetExpirationDateDateInSeconds();
      }

      if (CarVersionChecker.IsAdvanceCompressionVersion())
      {
        compressionType_ = CompressionType.Advanced;
      }
      else
      {
        compressionType_ = CompressionType.Normal;
      }

      companyIcon_ = CarEditorResource.LoadEditorTexture(CarVersionChecker.CompanyIconName);
      versionString_ = "Version: " + version + versionTypeName + " \n(Compression type: " + compressionType_.ToString() + ")";
    }

    void OnLostFocus()
    {
      Close();
    }

    public void OnGUI()
    {
      GUI.DrawTexture(new Rect(-30f, -10f, 230f, 230f), CarManagerEditor.ic_logoCaronte_);

      GUILayout.BeginArea( new Rect( 180f, 5f, 195f, 210f ) );   
      GUILayout.FlexibleSpace();

      if ( versionType_.IsFlagSet(VersionType.Evaluation) )
      {
        GUILayout.Label( new GUIContent("EVALUATION version.\n\nAny commercial use, \ncopying, or redistribution of \nthis plugin is strictly forbidden.\n" ), EditorStyles.miniLabel );
      }

      if (CarVersionChecker.DoVersionExpires())
      {
        GUILayout.Label( new GUIContent("Expiration date of this version is:\n\n" + expirationDateTime_.ToShortDateString() + " (month/day/year).\nUse of this software is forbidden\nafter the expiration date."), EditorStyles.miniLabel );
      }

      if (CarVersionChecker.CompanyName != string.Empty)
      {
        GUILayout.Label( new GUIContent("This version is exclusive for " + CarVersionChecker.CompanyName + "\ninternal use.\n"), EditorStyles.miniLabel );
        GUILayout.Label( new GUIContent(companyIcon_), GUILayout.MaxWidth(69.7f), GUILayout.MaxHeight(32f) );
      }

      GUILayout.FlexibleSpace();

      GUILayout.Label( new GUIContent("Powered by Caronte physics engine."), EditorStyles.miniLabel );
      GUILayout.Label( new GUIContent("(c) 2017 Next Limit Technologies."), EditorStyles.miniLabel );
      GUILayout.Label( new GUIContent( versionString_ ), EditorStyles.miniLabel );

      EditorGUILayout.Space();

      GUILayout.EndArea();
    }

  }
}
