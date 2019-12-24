﻿// ***********************************************************
//	Copyright 2016 Next Limit Technologies, http://www.nextlimit.com
//	All rights reserved.
//
//	THIS SOFTWARE IS PROVIDED 'AS IS' AND WITHOUT ANY EXPRESS OR
//	IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
//	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//
// ***********************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CaronteFX
{
  /// <summary>
  /// Holds the data of a trigger by detector node.
  /// </summary>
  [AddComponentMenu("")]
  public class CNTriggerByDetector : CNTrigger
  {
    [SerializeField]
           CNField fieldA_;
    public CNField FieldA
    {
      get
      {
        if (fieldA_ == null)
        {
          CNFieldContentType allowedTypes =   CNFieldContentType.Geometry
                                            | CNFieldContentType.BodyNode;
                      
          fieldA_ = new CNField( false, allowedTypes, false );
        }
        return fieldA_;
      }
    }

    [SerializeField]
           CNField fieldB_;
    public CNField FieldB
    {
      get
      {
        if (fieldB_ == null)
        {
          CNFieldContentType allowedTypes =  CNFieldContentType.Geometry
                                           | CNFieldContentType.BodyNode;
                      
          fieldB_ = new CNField( false, allowedTypes, false );
        }
        return fieldB_;
      }
    }

    [SerializeField]
    bool triggerForInvolvedBodies_ = false;
    public bool TriggerForInvolvedBodies
    {
      get { return triggerForInvolvedBodies_; }
      set { triggerForInvolvedBodies_ = value; }
    }

    public override CNFieldContentType FieldContentType { get { return CNFieldContentType.TriggerByDetectorNode; } }

    protected override void CloneData(CommandNode node)
    {
      base.CloneData(node);

      CNTriggerByDetector clone = (CNTriggerByDetector)node;
      clone.fieldA_ = fieldA_.DeepClone();
      clone.fieldB_ = fieldB_.DeepClone();

      clone.triggerForInvolvedBodies_ = TriggerForInvolvedBodies;
    }

    public override CommandNode DeepClone(GameObject dataHolder)
    {
      CNTriggerByDetector clone = CommandNode.CreateInstance<CNTriggerByDetector>(dataHolder);
      CloneData(clone);
      return clone;
    }

    public override bool UpdateNodeReferences(Dictionary<CommandNode, CommandNode> dictNodeToClonedNode)
    {
      bool updateEntityField = field_.UpdateNodeReferences(dictNodeToClonedNode);
      bool updateA = fieldA_.UpdateNodeReferences(dictNodeToClonedNode);
      bool updateB = fieldB_.UpdateNodeReferences(dictNodeToClonedNode);

      return (updateEntityField || updateA || updateB);
    }

  }


}
