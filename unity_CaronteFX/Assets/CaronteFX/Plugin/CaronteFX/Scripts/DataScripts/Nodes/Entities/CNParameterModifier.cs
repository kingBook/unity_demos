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
using System;
using System.Collections;
using System.Collections.Generic;

namespace CaronteFX
{
  public enum PARAMETER_MODIFIER_PROPERTY
  {
    VELOCITY_LINEAL   = 0,
    VELOCITY_ANGULAR  = 1,
    ACTIVITY          = 2,
    PLASTICITY        = 3,
    FREEZE            = 4,
    VISIBILITY        = 5,
    FORCE_MULTIPLIER  = 6,
    EXTERNAL_DAMPING  = 7,
    TARGET_POSITION   = 8,
    TARGET_ANGLE      = 9,
    UNKNOWN
  };

  [Serializable]
  public class ParameterModifierCommand : IDeepClonable<ParameterModifierCommand>
  {
    public PARAMETER_MODIFIER_PROPERTY target_;
    public Vector3 valueVector3_;
    public int     valueInt_;

    public ParameterModifierCommand()
    {
      target_       = PARAMETER_MODIFIER_PROPERTY.ACTIVITY;
      valueVector3_ = new Vector3( 0.0f, 0.0f, 0.0f );
      valueInt_     = 0;
    }

    public ParameterModifierCommand(PARAMETER_MODIFIER_PROPERTY target)
    {
      target_ = target;
      valueVector3_ = new Vector3( 0.0f, 0.0f, 0.0f );
      valueInt_     = 0;
    }

    public ParameterModifierCommand DeepClone()
    {
      ParameterModifierCommand clone = new ParameterModifierCommand();

      clone.target_       = target_;
      clone.valueVector3_ = valueVector3_;
      clone.valueInt_     = valueInt_;
      
      return clone;
    }

    public void SetValue(CarParameter carParameter)
    {
      valueVector3_ = carParameter.valueVector3_;
      valueInt_     = carParameter.valueInt_;
    }
  }

  /// <summary>
  /// Holds the data of a parameter modifier node.
  /// </summary>
  [AddComponentMenu("")]
  public class CNParameterModifier : CNEntity
  {   
 
    public override CNField Field
    {
      get
      {
        if (field_ == null)
        {
          CNFieldContentType allowedTypes =    CNFieldContentType.Bodies
                                             | CNFieldContentType.JointServosNode
                                             | CNFieldContentType.DaemonNode
                                             | CNFieldContentType.TriggerNode;

          field_ = new CNField(false, allowedTypes, false);
        }
        return field_;
      }
    }

    [SerializeField]
    private List<ParameterModifierCommand> listPmCommand_ = new List<ParameterModifierCommand>();
    public List<ParameterModifierCommand> ListPmCommand
    {
      get { return listPmCommand_; }
    }

    public override CNFieldContentType FieldContentType { get { return CNFieldContentType.ParameterModifierNode; } }

    protected override void CloneData(CommandNode node)
    {
      base.CloneData(node);
      CNParameterModifier clone = (CNParameterModifier)node;
      
      clone.listPmCommand_ = new List<ParameterModifierCommand>();
      foreach( ParameterModifierCommand pmCommand in listPmCommand_ )
      {
        clone.listPmCommand_.Add( pmCommand.DeepClone() );
      }
    }

    public override CommandNode DeepClone(GameObject dataHolder)
    {
      CNParameterModifier clone = CommandNode.CreateInstance<CNParameterModifier>(dataHolder);     
      CloneData(clone);  
      return clone;
    }

  }
}
