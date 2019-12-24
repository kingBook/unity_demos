// ***********************************************************
//	Copyright 2016 Next Limit Technologies, http://www.nextlimit.com
//	All rights reserved.
//
//	THIS SOFTWARE IS PROVIDED 'AS IS' AND WITHOUT ANY EXPRESS OR
//	IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
//	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//
// ***********************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaronteFX
{
  [AddComponentMenu("")]
  public abstract class CNCorpuscles : CNMonoField
  {
    public override CNField Field
    {
      get
      {
        if (field_ == null)
        {
          field_ = new CNField(true, false);
        }
        return field_;
      }
    }

    [SerializeField]
    protected float corpusclesRadius_ = 0.02f;
    public float CorpusclesRadius
    {
      get { return corpusclesRadius_; }
      set { corpusclesRadius_ = value; }
    }
    //-----------------------------------------------------------------------------------
    public override CNFieldContentType FieldContentType
    {
      get { return CNFieldContentType.Corpuscles; }
    }
    //-----------------------------------------------------------------------------------
    protected override void CloneData(CommandNode node)
    {
      base.CloneData(node);

      CNCorpuscles clone = (CNCorpuscles)node;
      clone.corpusclesRadius_ = corpusclesRadius_;
    }
    //-----------------------------------------------------------------------------------
    public override CommandNode DeepClone(GameObject dataHolder)
    {
      CNCorpuscles clone = CommandNode.CreateInstance<CNCorpuscles>(dataHolder);   
      CloneData(clone);
      return clone;
    }

  }// class CNCorpuscles

} // namespace CaronteFX...

