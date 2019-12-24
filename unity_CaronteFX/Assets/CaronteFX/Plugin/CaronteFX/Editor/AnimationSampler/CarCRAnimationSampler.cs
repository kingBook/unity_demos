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
using System.Collections.Generic;
using CaronteSharp;

namespace CaronteFX
{
  public class CarCRAnimationSampler : CarBodyAnimationSampler
  {
    CRAnimation crAnimation_;

    public CarCRAnimationSampler(GameObject rootGameObject)
    {
      crAnimation_ = rootGameObject.GetComponent<CRAnimation>();

      CarEditorUtils.GetRenderersFromRoot(rootGameObject, out arrNormalMeshRenderer_, out arrSkinnedMeshRenderer_);
      AssignBodyIds();
    }

    public void AssignTmpAnimationController(GameObject rootGameObject)
    {
      CarEditorUtils.GetRenderersFromRoot(rootGameObject, out arrNormalMeshRenderer_, out arrSkinnedMeshRenderer_);

      crAnimation_ = rootGameObject.GetComponent<CRAnimation>();
      crAnimation_.LoadAnimation(true);
    }

    public void UpdateSimulating(CarAnimationData animData, UnityEngine.Mesh animBakingMesh, float eventTime, float deltaTime)
    {
      if (crAnimation_ == null)
      {
        return;
      }

      crAnimation_.Update(deltaTime);
      float targetTime = eventTime + deltaTime;

      for (int i = 0; i < arrSkinnedMeshRenderer_.Length; ++i)
      {
        uint idBody = arrIdBodySkinnedGameObjects_[i];
        SkinnedMeshRenderer smRenderer = arrSkinnedMeshRenderer_[i];

        GameObject gameObject = smRenderer.gameObject;

        smRenderer.BakeMesh(animBakingMesh);

        if (idBody != uint.MaxValue)
        {
          Matrix4x4 m_MODEL_TO_WORLD = gameObject.transform.localToWorldMatrix;
          RigidbodyManager.Rg_addEventTargetArrPos_WORLD((double)eventTime, (double)targetTime, idBody, ref m_MODEL_TO_WORLD, animBakingMesh.vertices);
        }
      }

      for (int i = 0; i < arrNormalMeshRenderer_.Length; ++i)
      {
        uint idBody = arrIdBodyNormalGameObjects_[i];
        MeshRenderer renderer = arrNormalMeshRenderer_[i];
        GameObject gameObject = renderer.gameObject;

        if (idBody != uint.MaxValue)
        {
          Matrix4x4 m_MODEL_TO_WORLD = gameObject.transform.localToWorldMatrix;

          if ( crAnimation_.IsVertexAnimated(gameObject) )
          {
            MeshFilter mf = gameObject.GetComponent<MeshFilter>();
            Mesh mesh = mf.sharedMesh;
            RigidbodyManager.Rg_addEventTargetArrPos_WORLD((double)eventTime, (double)targetTime, idBody, ref m_MODEL_TO_WORLD, mesh.vertices);
          }
          else
          {
            RigidbodyManager.Rg_addEventTargetPos_WORLD((double)eventTime, (double)targetTime, idBody, ref m_MODEL_TO_WORLD, 0.01);
          }

        }
      }
      animData.timeAnimated_ += deltaTime;
    }
  }
}

