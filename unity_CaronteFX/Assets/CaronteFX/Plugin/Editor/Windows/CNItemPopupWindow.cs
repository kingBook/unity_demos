﻿// ***********************************************************
//	Copyright 2016 Next Limit Technologies, http://www.nextlimit.com
//	All rights reserved.
//
//	THIS SOFTWARE IS PROVIDED 'AS IS' AND WITHOUT ANY EXPRESS OR
//	IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
//	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
//
// ***********************************************************

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace CaronteFX
{
  public class CNItemPopupWindow : CarWindow<CNItemPopupWindow>
  {
    public IView View { get; private set; }
    //---------------------------------------------------------------------------------- 
    const float height_ = 90;
    const float width_ = 300;
    //-----------------------------------------------------------------------------------
    void OnEnable()
    {
    }
    //-----------------------------------------------------------------------------------
    public static CNItemPopupWindow ShowWindow(string windowTitle, Rect position = new Rect() )
    {  
      Instance = CNItemPopupWindow.CreateInstance<CNItemPopupWindow>(); 

      Instance.titleContent = new GUIContent(windowTitle);
      Instance.position  = position;
      Instance.minSize   = new Vector2(width_, height_);
      Instance.maxSize   = new Vector2(width_, height_);

      Instance.ShowAuxWindow();
     
      return (Instance);
    }
    //-----------------------------------------------------------------------------------
    public void Init(IView view)
    {
      View = view;
    }
    //-----------------------------------------------------------------------------------
    void OnGUI()
    {
      View.RenderGUI(position, true);
    }
    //-----------------------------------------------------------------------------------
  }
}


