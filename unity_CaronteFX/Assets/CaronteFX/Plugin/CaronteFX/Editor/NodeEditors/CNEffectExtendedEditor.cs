using UnityEngine;
using UnityEditor;
using System.Collections;


namespace CaronteFX
{
  public class CNEffectExtendedEditor : CNEffectEditor
  {  
    protected enum tabType
    {
      General = 0,
      Advanced = 1,
    }

    private static readonly GUIContent efTotalSimulationTimeCt_       = new GUIContent( CarStringManager.GetString("EfTotalSimulationTime"), CarStringManager.GetString("EfTotalSimulationTimeTooltip") );
    private static readonly GUIContent efTotalSimulationTimeResetCt_  = new GUIContent( CarStringManager.GetString("EfTotalSimulationTimeReset"), CarStringManager.GetString("EfTotalSimulationTimeResetTooltip") );
    private static readonly GUIContent efQualityCt_                   = new GUIContent( CarStringManager.GetString("EfQuality"), CarStringManager.GetString("EfQualityTooltip") );
    private static readonly GUIContent efAntiJitteringCt_             = new GUIContent( CarStringManager.GetString("EfAntiJittering"), CarStringManager.GetString("EfAntiJitteringTooltip") );
    private static readonly GUIContent efFramesPerSecondCt_           = new GUIContent( CarStringManager.GetString("EfFramesPerSecond"), CarStringManager.GetString("EfFramesPerSecondTooltip") );
    private static readonly GUIContent efUserDefinedDeltaTimeCt_      = new GUIContent( CarStringManager.GetString("EfUserDefinedDeltaTime"), CarStringManager.GetString("EfUserDefinedDeltaTimeTooltip"));
    private static readonly GUIContent efDeltaTimeCt_                 = new GUIContent( CarStringManager.GetString("EfDeltaTime"), CarStringManager.GetString("EfDeltaTimeTooltip"));
    private static readonly GUIContent efLastDeltaTimeCt_             = new GUIContent( CarStringManager.GetString("EfLastDeltaTime"), CarStringManager.GetString("EfLastDeltaTimeTooltip"));
    private static readonly GUIContent efUserDefinedCharDistancesCt_  = new GUIContent( CarStringManager.GetString("EfUserDefinedCharacteristicDistances"), CarStringManager.GetString("EfUserDefinedCharacteristicDistancesTooltip") );
    private static readonly GUIContent efThicknessCt_                 = new GUIContent( CarStringManager.GetString("EfThickness"), CarStringManager.GetString("EfThicknessTooltip"));
    private static readonly GUIContent efLastThicknessCt_             = new GUIContent( CarStringManager.GetString("EfLastThickness"), CarStringManager.GetString("EfLastThicknessTooltip"));
    private static readonly GUIContent efLenghtCt_                    = new GUIContent( CarStringManager.GetString("EfLength"), CarStringManager.GetString("EfLengthTooltip"));
    private static readonly GUIContent efLastLengthCt_                = new GUIContent( CarStringManager.GetString("EfLastLength"), CarStringManager.GetString("EfLastLengthTooltip") );

    private GUIContent[] arrTabNameCt_ = new GUIContent[] { new GUIContent( CarStringManager.GetString("EfGeneral") ), new GUIContent( CarStringManager.GetString("EfAdvanced") )};
    private int tabIndex = 0;

    public CNEffectExtendedEditor( CNGroup data, CommandNodeEditorState state )
      : base( data, state )
    {

    }

    public override void RenderGUI(Rect area, bool isEditable)
    {
      GUILayout.BeginArea(area);
      
      RenderTitle(isEditable);

      EditorGUILayout.Space();
      float originalLabelwidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 100f;

      EditorGUI.BeginDisabledGroup( !isEditable );
      EditorGUI.BeginChangeCheck();
      selectedScopeIdx_ = EditorGUILayout.Popup(efScopeCt_, selectedScopeIdx_, arrScopeTypeCt_);
      if (EditorGUI.EndChangeCheck())
      {
        ChangeScope( (CNGroup.CARONTEFX_SCOPE)selectedScopeIdx_ );
      }

      EditorGUI.EndDisabledGroup();
      EditorGUILayout.Space();
      EditorGUILayout.Space();
      if ( GUILayout.Button(efSelectScopeCt_, GUILayout.Height(30f)) )
      {
        SceneSelection();
      }
      EditorGUIUtility.labelWidth = originalLabelwidth;

      CarGUIUtils.DrawSeparator();
      CarGUIUtils.Splitter();

      DrawEffectGUIWindow(isEditable);

      GUILayout.EndArea();
    }

    private void DrawTotalTime()
    {  
      GUILayout.Space(width_indent);
      EditorGUILayout.LabelField(efTotalSimulationTimeCt_);
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);

      EditorGUI.BeginChangeCheck();
      var value = EditorGUILayout.FloatField(effectData_.totalTime_);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(fxData_, "Change " + efTotalSimulationTimeCt_.text + " - " + Data.Name);
        effectData_.totalTime_ = Mathf.Clamp( value, 0f, 3600f);
        EditorUtility.SetDirty( fxData_ );
      }
    }

    private void DrawTotalTimeReset()
    {
      if ( GUILayout.Button( efTotalSimulationTimeResetCt_, EditorStyles.miniButton, GUILayout.Width(50f) ) )
      {
        Undo.RecordObject(fxData_, "Change " + efTotalSimulationTimeResetCt_.text + " - " + Data.Name);
        effectData_.totalTime_ = 10f;
        EditorUtility.SetDirty( fxData_ );
      }
    }

    private void DrawQuality()
    {
      EditorGUI.BeginDisabledGroup(effectData_.byUserDeltaTime_);
      EditorGUILayout.Space();
      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUILayout.LabelField(efQualityCt_, GUILayout.Width(80f));
      EditorGUI.BeginChangeCheck();
      var value = EditorGUILayout.Slider(effectData_.quality_, 1f, 100f);
      if ( EditorGUI.EndChangeCheck() )
      {
        Undo.RecordObject(fxData_, "Change " + efQualityCt_.text + " - " + Data.Name);
        effectData_.quality_ = value;
        EditorUtility.SetDirty( fxData_ );
      }
      EditorGUILayout.EndHorizontal();
      EditorGUI.EndDisabledGroup();
    }

    private void DrawAntiJittering()
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUILayout.LabelField(efAntiJitteringCt_, GUILayout.Width(80f));
      EditorGUI.BeginChangeCheck();
      var value = EditorGUILayout.Slider(effectData_.antiJittering_, 1, 100);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(fxData_, "Change " + efAntiJitteringCt_.text + " - " + Data.Name);
        effectData_.antiJittering_ = value;
        EditorUtility.SetDirty( fxData_ );
      }
      EditorGUILayout.EndHorizontal();
    }

    private void DrawFrameRate()
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUILayout.LabelField(efFramesPerSecondCt_);
      EditorGUILayout.EndHorizontal();

      EditorGUI.BeginChangeCheck();
      var value = EditorGUILayout.IntField(effectData_.frameRate_);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(fxData_, "Change " + efFramesPerSecondCt_.text + " - " + Data.Name);
        effectData_.frameRate_ = Mathf.Clamp(value, 0, 2000);
        EditorUtility.SetDirty( fxData_ );
      }

      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUILayout.EndHorizontal();
    }

    private void DrawByUserDetalTime()
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUI.BeginChangeCheck();
      var value = EditorGUILayout.ToggleLeft( efUserDefinedDeltaTimeCt_, effectData_.byUserDeltaTime_ );
      if ( EditorGUI.EndChangeCheck() )
      {
        Undo.RecordObject(fxData_, "Change " + efUserDefinedDeltaTimeCt_.text + " - " + Data.Name);
        effectData_.byUserDeltaTime_ = value;
        if (value)
        {
          effectData_.byUserCharacteristicObjectProperties_ = false;
        }
        EditorUtility.SetDirty( fxData_ );
      }
      EditorGUILayout.EndHorizontal();
    }

    private void DrawDeltaTime()
    {
      EditorGUI.BeginDisabledGroup( !effectData_.byUserDeltaTime_ );
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUILayout.LabelField( efDeltaTimeCt_ );
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(20f);

      EditorGUILayout.LabelField(efLastDeltaTimeCt_);
      EditorGUILayout.EndHorizontal();
      EditorGUI.BeginDisabledGroup(!effectData_.byUserDeltaTime_);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUI.BeginChangeCheck();
      var deltaTime = CarGUIExtension.FloatTextField(effectData_.deltaTime_, 0.000001f, 10f, -1f, "-");
      if ( EditorGUI.EndChangeCheck() )
      {
        Undo.RecordObject(fxData_, "Change " + efDeltaTimeCt_.text + " - "+ Data.Name);
        effectData_.deltaTime_ = deltaTime;
        EditorUtility.SetDirty( fxData_ );
      }
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(20f);
      EditorGUILayout.LabelField( (effectData_.calculatedDeltaTime_ < 0f) ? "-" : effectData_.calculatedDeltaTime_.ToString(), EditorStyles.textField);
      EditorGUILayout.EndHorizontal();
    }

    private void DrawByUserCharacteristicObjectProperties()
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);

      EditorGUI.BeginChangeCheck();
      var value = EditorGUILayout.ToggleLeft(efUserDefinedCharDistancesCt_, effectData_.byUserCharacteristicObjectProperties_);
      if ( EditorGUI.EndChangeCheck() )
      {
        Undo.RecordObject(fxData_, "Change " + efUserDefinedCharDistancesCt_.text + " - " + Data.Name);
        effectData_.byUserCharacteristicObjectProperties_ = value;
        if (value)
        {
          effectData_.byUserDeltaTime_ = false;
        }
      }
      EditorGUILayout.EndHorizontal();
    }

    private void DrawThickness()
    {

      EditorGUI.BeginDisabledGroup(!effectData_.byUserCharacteristicObjectProperties_);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUILayout.LabelField(efThicknessCt_);
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(20f);
      EditorGUILayout.LabelField(efLastThicknessCt_);
      EditorGUILayout.EndHorizontal();
      EditorGUI.BeginDisabledGroup(!effectData_.byUserCharacteristicObjectProperties_);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUI.BeginChangeCheck();
      var thickness = CarGUIExtension.FloatTextField(effectData_.thickness_, 0f, 10f, -1f, "-");
      if ( EditorGUI.EndChangeCheck() )
      {
        Undo.RecordObject( fxData_, "Change " + efLastThicknessCt_.text + " - " + Data.Name);
        effectData_.thickness_ = thickness;
      }
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(20f);
      EditorGUILayout.LabelField( (effectData_.calculatedThickness_ < 0f) ? "-" : effectData_.calculatedThickness_.ToString(), EditorStyles.textField);
      EditorGUILayout.EndHorizontal();
    }

    private void DrawLength()
    { 
      EditorGUI.BeginDisabledGroup(!effectData_.byUserCharacteristicObjectProperties_);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUILayout.LabelField(efLenghtCt_);
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(20f);
      EditorGUILayout.LabelField(efLastLengthCt_);
      EditorGUILayout.EndHorizontal();

      EditorGUI.BeginDisabledGroup(!effectData_.byUserCharacteristicObjectProperties_);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(width_indent);
      EditorGUI.BeginChangeCheck();
      var length = CarGUIExtension.FloatTextField(effectData_.length_, 0f, 10f, -1f, "-");
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject( fxData_, "Change " + efLenghtCt_.text + " - " + Data.Name);
        effectData_.length_ = length;
      }
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(20f);
      EditorGUILayout.LabelField((effectData_.calculatedLength_ < 0f) ? "-" : effectData_.calculatedLength_.ToString(), EditorStyles.textField);
      EditorGUILayout.EndHorizontal();
    }

    private void DrawEffectGUIWindow(bool isEditable)
    {
      GUIStyle styleTabButton = new GUIStyle(EditorStyles.toolbarButton);
      styleTabButton.fontSize    = 10;
      styleTabButton.fixedHeight = 18f;
      styleTabButton.onNormal.background  = styleTabButton.onActive.background;
      tabIndex = GUILayout.SelectionGrid( tabIndex, arrTabNameCt_, 2, styleTabButton);
      CarGUIUtils.Splitter();

      scroller_ = EditorGUILayout.BeginScrollView( scroller_ );
      
      EditorGUI.BeginDisabledGroup( !isEditable );
     
      EditorGUILayout.Space();

      #region General
      if (tabIndex == (int)tabType.General)
      {
        //TIME
        EditorGUILayout.BeginHorizontal();
        DrawTotalTime();
        DrawTotalTimeReset();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //QUALITY
        DrawQuality();

        GUILayout.Space(20f);
        //ANTI-JITTERING
        DrawAntiJittering();

        GUILayout.Space(20f);
        //FRAMERATE
        DrawFrameRate();
      }
      #endregion

      #region Advanced
      else if (tabIndex == (int)tabType.Advanced)
      {
        EditorGUILayout.Space();
        DrawByUserDetalTime();
        GUILayout.Space(10f);
        DrawDeltaTime();
        GUILayout.Space(20f);
        DrawByUserCharacteristicObjectProperties();
        GUILayout.Space(10f);
        DrawThickness();
        DrawLength();
      }

      EditorGUILayout.Space();
      #endregion

      EditorGUILayout.Space();

      EditorGUI.EndDisabledGroup();

      EditorGUILayout.EndScrollView();
    }


  }
}

