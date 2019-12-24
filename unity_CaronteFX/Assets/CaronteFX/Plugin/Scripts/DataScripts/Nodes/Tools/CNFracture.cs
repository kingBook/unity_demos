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
using System;
using System.Collections;
using System.Collections.Generic;

namespace CaronteFX
{
  /// <summary>
  /// Holds the data of a fracturer node.
  /// </summary>
 
  [AddComponentMenu("")]  
  public class CNFracture : CNMonoField
  {
    public enum CHOP_MODE
    {
      VORONOI_UNIFORM,
      VORONOI_BY_GEOMETRY,
      VORONOI_RADIAL
    };

    public enum FOCUS_MODE
    {
      INSIDE,       // biggest concentration is required inside the focus geometry
      OUTSIDE,      // biggest concentration is required outside the focus geometry
      BOUNDARY,     // biggest concentration is required along the boundary of the focus geometry
      OUT_BOUNDARY  // biggest concentration is required out of the boundary of the focus geometry
    };

    public enum CROP_MODE
    {
      INSIDE,
      OUTSIDE
    };

    public static Material commonMaterial_;

    public override CNField Field
    {
      get
      {
        if (field_ == null)
        {
          field_ = new CNField( false, false );
        }
        return field_;
      }
    }

    [SerializeField]
    protected CNField fieldRestrictionGeometry_;
    public CNField FieldRestrictionGeometry
    {
      get
      {
        if (fieldRestrictionGeometry_ == null)
        {
          fieldRestrictionGeometry_ = new CNField( false, false );
        }
        return fieldRestrictionGeometry_;
      }
    }

    [SerializeField]
    protected CNField fieldSteeringGeometry_;
    public CNField FieldSteeringGeometry
    {
      get
      {
        if (fieldSteeringGeometry_ == null)
        {
          fieldSteeringGeometry_ = new CNField( false, false );
        }
        return fieldSteeringGeometry_;
      }
    }

    [SerializeField]
    CHOP_MODE chopMode_ = CHOP_MODE.VORONOI_UNIFORM;
    public CHOP_MODE ChopMode
    {
      get { return chopMode_; }
      set { chopMode_ = value; }
    }
  
    [SerializeField]
    bool doGlobalPattern_ = true;
    public bool DoGlobalPattern
    {
      get { return doGlobalPattern_; }
      set { doGlobalPattern_ = value; }
    }

    [SerializeField]
    int nDesiredPieces_ = 3;
    public int NDesiredPieces
    {
      get { return nDesiredPieces_; }
      set { nDesiredPieces_ = Mathf.Clamp(value, 1, int.MaxValue); }
    }

    [SerializeField]
    int seed_ = 61047;
    public int Seed
    {
      get { return seed_; }
      set { seed_ = value; }
    }

    [SerializeField]
    GameObject chopGeometry_ = null;
    public GameObject ChopGeometry
    {
      get { return chopGeometry_; }
      set { chopGeometry_ = value; }
    }

    [SerializeField]
    int gridResolution_ = 15000;
    public int GridResolution
    {
      get { return gridResolution_; }
      set { gridResolution_ = Mathf.Clamp(value, 5000, int.MaxValue); } 
    }

    [SerializeField]
    FOCUS_MODE focusMode_ = FOCUS_MODE.INSIDE;
    public FOCUS_MODE FocusMode
    {
      get { return focusMode_; }
      set { focusMode_ = value; }
    }

    [SerializeField]
    float densityRate_ = 0.01f;
    public float DensityRate
    {
      get { return densityRate_; }
      set { densityRate_ = Mathf.Clamp(value, 0f, float.MaxValue); }
    }

    [SerializeField]
    float transitionLength_ = 0.0f;
    public float TransitionLength
    {
      get { return transitionLength_; }
      set { transitionLength_ = Mathf.Clamp(value, 0f, float.MaxValue); }
    }

    [SerializeField]
    Transform referenceSystem_ = null;
    public Transform ReferenceSystem
    {
      get { return referenceSystem_; }
      set { referenceSystem_ = value; }
    }

    public enum AxisDir
    {
      x,
      y,
      z
    }

    [SerializeField]
    AxisDir referenceSystemAxis_ = AxisDir.x;
    public AxisDir ReferenceSystemAxis
    {
      get { return referenceSystemAxis_; }
      set { referenceSystemAxis_ = value; }
    }

    [SerializeField]
    int rays_number_ = 15;
    public int RaysNumber
    {
      get { return rays_number_; }
      set { rays_number_ = Mathf.Clamp(value, 1, int.MaxValue); }
    }


    [SerializeField]
    float rays_rateRand_ = 0.0f;
    public float RaysRateRand
    {
      get { return rays_rateRand_; }
      set { rays_rateRand_ = Mathf.Clamp01(value); }
    }

    [SerializeField]
    int rings_numberInsideAnnulus_ = 8; //!< Number of rings around vFocusPoint_. Only those inside the annulus are taken into account.
    public int RingsNumberInsideAnnulus
    {
      get { return rings_numberInsideAnnulus_; }
      set { rings_numberInsideAnnulus_ = Mathf.Clamp(value, 1, int.MaxValue); }
    }

    [SerializeField]
    float rings_intRadius_ = 0.0f; //!< Internal radius of annulus where rings are concetrated. 
    public float RingsIntRadius
    {
      get { return rings_intRadius_; }
      set { rings_intRadius_ = Mathf.Clamp(value, 0f, float.MaxValue); }
    }

    [SerializeField]
    float rings_extRadius_ = 1.0f; //!< External radius of annulus where rings are concetrated.
    public float RingsExtRadius
    {
      get { return rings_extRadius_; }
      set { rings_extRadius_ = Mathf.Clamp(value, 0f, float.MaxValue); }
    }

    [SerializeField]
    float rings_intTransitionLength_ = 0.0f; //!< Distance of rings decay from rings_intRadius_ down.
    public float RingsIntTransitionLength
    {
      get { return rings_intTransitionLength_; }
      set { rings_intTransitionLength_ = Mathf.Clamp(value, 0f, float.MaxValue); }
    }

    [SerializeField]
    float rings_extTransitionLength_ = 0.0f; //!< Distance of rings decay from rings_extRadius_ up.
    public float RingsExtTransitionLength
    {
      get {  return rings_extTransitionLength_; }
      set { rings_extTransitionLength_ = Mathf.Clamp(value, 0f, float.MaxValue); }
    }

    [SerializeField]
    float rings_intTransitionDecay_ = 0.3f;  //!< In [0,1]. Decay speed of rings density in internal transition zone.
    public float RingsIntTransitionDecay
    {
      get { return rings_intTransitionDecay_; }
      set { rings_intTransitionDecay_ = Mathf.Clamp01(value); }
    }

    [SerializeField]
    float rings_extTransitionDecay_ = 0.3f; //!< In [0,1]. Decay speed of rings density in external transition zone.
    public float RingsExtTransitionDecay
    {
      get { return rings_extTransitionDecay_; }
      set { rings_extTransitionDecay_ = Mathf.Clamp01(value); }
    }

    [SerializeField]
    float rings_rateRand_ = 0.0f; //!< In [0,1]. 0 means that rings are equispaced; 1 means that rings are placed totally random.
    public float RingsRateRand
    {
      get { return rings_rateRand_; }
      set { rings_rateRand_ = Mathf.Clamp01(value); }
    }

    [SerializeField]
    bool  doCentralPiece_ = false;
    public bool DoCentralPiece
    {
      get { return doCentralPiece_; }
      set { doCentralPiece_ = value; }
    }

    [SerializeField]
    float noiseRate_ = 0.0f;
    public float NoiseRate
    {
      get { return noiseRate_; }
      set { noiseRate_ = Mathf.Clamp01(value); }
    }

    [SerializeField]
    float twistRate_ = 0.0f;
    public float TwistRate
    {
      get { return twistRate_; }
      set { twistRate_ = Mathf.Clamp(value, -1.0f, 1.0f); }
    }
     
    [SerializeField]
    bool doExtrusionEffect_ = false;
    public bool DoExtrusionEffect
    {
      get { return doExtrusionEffect_; }
      set { doExtrusionEffect_ = value; }
    }

    [SerializeField]
    bool doCoordinate_ = false;
    public bool DoCoordinate
    {
      get { return doCoordinate_; }
      set { doCoordinate_ = value; }
    }

    [SerializeField]
    float pushRate_ = 0.0f;
    public float PushRate
    {
      get { return pushRate_; }
      set { pushRate_ = value; }
    }

    [SerializeField]
    float pushMultiplier_ = 1.0f;
    public float PushMultiplier
    {
      get { return pushMultiplier_; }
      set { pushMultiplier_ = value; }
    }

    [SerializeField]
    private UnityEngine.GameObject[] arrGameObject_chopped_;
    public UnityEngine.GameObject[] ArrChoppedGameObject
    {
      get { return arrGameObject_chopped_; }
      set { arrGameObject_chopped_ = value; }
    }

    [SerializeField]
    private UnityEngine.Mesh[] arrMesh_chopped_;
    public UnityEngine.Mesh[] ArrChoppedMesh
    {
      get { return arrMesh_chopped_; }
      set { arrMesh_chopped_ = value; }
    }

    [SerializeField]
    private int[] arrInteriorSubmeshIdx_;
    public int[] ArrInteriorSubmeshIdx
    {
      get { return arrInteriorSubmeshIdx_; }
      set { arrInteriorSubmeshIdx_ = value; }
    }

    [SerializeField]
    private int[] arrInteriorTrianglesStartIdx_;
    public int[] ArrInteriorTrianglesRange
    {
      get { return arrInteriorTrianglesStartIdx_; }
      set { arrInteriorTrianglesStartIdx_ = value; }
    }

    [SerializeField]
    private UnityEngine.GameObject gameObject_chopped_root_;
    public UnityEngine.GameObject  GameObjectChoppedRoot
    {
      get { return gameObject_chopped_root_; }
      set { gameObject_chopped_root_ = value; }
    }

    [SerializeField]
    private UnityEngine.Bounds[] arrGameObject_bounds_chopped_;
    public UnityEngine.Bounds[] ArrGameObject_Bounds_Chopped
    {
      get { return arrGameObject_bounds_chopped_; }
      set { arrGameObject_bounds_chopped_ = value; }
    }

    [SerializeField]
    private UnityEngine.Vector3[] arrGameObject_chopped_positions_;
    public UnityEngine.Vector3[] ArrGameObject_Chopped_Positions
    {
      get { return arrGameObject_chopped_positions_; }
      set { arrGameObject_chopped_positions_ = value; }
    }

    [SerializeField]
    private Material interiorMaterial_ = commonMaterial_;
    public Material InteriorMaterial
    {
      get { return interiorMaterial_; }
      set { interiorMaterial_ = value; }
    }

    [SerializeField]
    private bool hideParentObjectsAuto_ = true;
    public bool HideParentObjectAuto
    {
      get { return hideParentObjectsAuto_; }
      set { hideParentObjectsAuto_ = value; }
    }

    [SerializeField]
    private bool restrictFractureZone_ = false;
    public bool RestrictFractureZone
    {
      get { return restrictFractureZone_; }
      set { restrictFractureZone_ = value; }
    }

    [SerializeField]
    private GameObject cropGeometry_ = null;
    public GameObject CropGeometry
    {
      get { return cropGeometry_; }
      set { cropGeometry_ = value; }
    }

    [SerializeField]
    private CROP_MODE cropMode_ = CROP_MODE.INSIDE;
    public CROP_MODE CropMode
    {
      get { return cropMode_; }
      set { cropMode_ = value; }
    }

    [SerializeField]
    private bool frontierPieces_ = false;
    public bool FrontierPieces
    {
      get { return frontierPieces_; }
      set { frontierPieces_ = value; }
    }

    [SerializeField]
    private bool weldInOnePiece_ = false;
    public bool WeldInOnePiece
    {
      get { return weldInOnePiece_; }
      set { weldInOnePiece_ = value; }
    }

    [SerializeField]
    private bool doNoise_ = false;
    public bool DoNoise
    {
      get { return doNoise_;  }
      set { doNoise_ = value;  }
    }

    [SerializeField]
    private float noiseResolutionRate_ = 0.4f;
    public float NoiseResolutionRate
    {
      get { return noiseResolutionRate_; }
      set { noiseResolutionRate_ = value; }
    }

    [SerializeField]
    private float noiseReliefAmplitudeRate_ = 0.2f;
    public float NoiseReliefAmplitudeRate
    {
      get { return noiseReliefAmplitudeRate_;  }
      set { noiseReliefAmplitudeRate_ = value;  }
    }

    [SerializeField]
    private float noiseReliefSerrationRate_ = 0.7f;
    public float NoiseReliefSerrationRate
    {
      get { return noiseReliefSerrationRate_;  }
      set { noiseReliefSerrationRate_ = value;  }
    }

    [SerializeField]
    private uint noiseSeed0_ = 32451;
    public uint NoiseSeed0
    {
      get { return noiseSeed0_; }
      set { noiseSeed0_ = value;  }
    }

    [SerializeField]
    private bool noiseDoExtrusionEffect_ = false;
    public bool NoiseDoExtrusionEffect
    {
      get { return noiseDoExtrusionEffect_; }
      set { noiseDoExtrusionEffect_ = value; }
    }

    [SerializeField]
    private int inputObjects_ = 0;
    public int InputObjects
    {
      get { return inputObjects_; }
      set { inputObjects_ = value; }
    }

    [SerializeField]
    private int inputVertices_ = 0;
    public int InputVertices
    {
      get { return inputVertices_; }
      set { inputVertices_ = value; }
    }

    [SerializeField]
    private int inputTriangles_ = 0;
    public int InputTriangles
    {
      get { return inputTriangles_; }
      set { inputTriangles_ = value; }
    }

    [SerializeField]
    private int outputPieces_ = 0;
    public int OutputPieces
    {
      get { return outputPieces_; }
      set { outputPieces_ = value; }
    }

    [SerializeField]
    private int outputVertices_ = 0;
    public int OutputVertices
    {
      get { return outputVertices_; }
      set { outputVertices_ = value; }
    }

    [SerializeField]
    private int outputTriangles_ = 0;
    public int OutputTriangles
    {
      get { return outputTriangles_; }
      set { outputTriangles_ = value; }
    }

    [SerializeField]
    private float internalFacesSmoothingAngle_ = (Mathf.PI / 8) * Mathf.Rad2Deg;
    public float InternalFacesSmoothingAngle
    {
      get { return internalFacesSmoothingAngle_; }
      set { internalFacesSmoothingAngle_ = value; }
    }

    [SerializeField]
    private Vector2 interiorFacesTiling_ = Vector2.one;
    public Vector2 InteriorFacesTiling
    {
      get { return interiorFacesTiling_; }
      set { interiorFacesTiling_ = value; }
    }

    [SerializeField]
    private Vector2 interiorFacesOffset_ = Vector2.zero;
    public Vector2 InteriorFacesOffset
    {
      get { return interiorFacesOffset_; }
      set { interiorFacesOffset_ = value; }
    }

    [SerializeField]
    private bool generateNewSubmeshForInteriorFaces_ = true;
    public bool GenerateNewSubmeshForInteriorFaces
    {
      get { return generateNewSubmeshForInteriorFaces_; }
      set { generateNewSubmeshForInteriorFaces_ = value; }
    }

    public override CNFieldContentType FieldContentType { get { return CNFieldContentType.None; } }

    protected override void CloneData(CommandNode node)
    {
      base.CloneData(node);
      CNFracture clone = (CNFracture)node;

      if (fieldRestrictionGeometry_ != null)
      {
        clone.fieldRestrictionGeometry_ = fieldRestrictionGeometry_.DeepClone();
      }    

      if (fieldSteeringGeometry_ != null)
      {
        clone.fieldSteeringGeometry_ = fieldSteeringGeometry_.DeepClone();
      }

      clone.chopMode_                   = chopMode_;
      clone.doGlobalPattern_            = doGlobalPattern_;
      clone.nDesiredPieces_             = nDesiredPieces_;
      clone.seed_                       = seed_;
      clone.chopGeometry_               = chopGeometry_;
      clone.gridResolution_             = gridResolution_;
      clone.focusMode_                  = focusMode_;
      clone.densityRate_                = densityRate_;
      clone.transitionLength_           = transitionLength_;
      clone.referenceSystem_            = referenceSystem_;
      clone.referenceSystemAxis_        = referenceSystemAxis_;
      clone.rays_number_                = rays_number_;
      clone.rays_rateRand_              = rays_number_;
      clone.rings_numberInsideAnnulus_  = rings_numberInsideAnnulus_; 
      clone.rings_intRadius_            = rings_numberInsideAnnulus_;
      clone.rings_extRadius_            = rings_extRadius_;
      clone.rings_intTransitionLength_  = rings_intTransitionLength_;
      clone.rings_extTransitionLength_  = rings_extTransitionLength_;
      clone.rings_intTransitionDecay_   = rings_intTransitionDecay_;
      clone.rings_extTransitionDecay_   = rings_extTransitionDecay_;
      clone.rings_rateRand_             = rings_rateRand_;
      clone.doCentralPiece_             = doCentralPiece_;
      clone.noiseRate_                  = noiseRate_;
      clone.twistRate_                  = twistRate_;
      clone.doExtrusionEffect_          = doExtrusionEffect_;
      clone.doCoordinate_               = doCoordinate_;
      clone.arrGameObject_chopped_      = arrGameObject_chopped_;
      clone.interiorMaterial_           = interiorMaterial_;
                                        
      clone.hideParentObjectsAuto_      = hideParentObjectsAuto_;
      clone.restrictFractureZone_       = restrictFractureZone_;
      clone.cropGeometry_               = cropGeometry_;
      clone.cropMode_                   = cropMode_;
      clone.frontierPieces_             = frontierPieces_;
      clone.weldInOnePiece_             = weldInOnePiece_;

      clone.doNoise_                    = doNoise_;
      clone.noiseResolutionRate_        = noiseResolutionRate_;
      clone.noiseReliefAmplitudeRate_   = noiseReliefAmplitudeRate_;
      clone.noiseReliefSerrationRate_   = noiseReliefSerrationRate_; ;
      clone.noiseSeed0_                 = noiseSeed0_;
      clone.noiseDoExtrusionEffect_     = noiseDoExtrusionEffect_;

      clone.internalFacesSmoothingAngle_ = internalFacesSmoothingAngle_;
      clone.interiorFacesTiling_         = interiorFacesTiling_;
      clone.interiorFacesOffset_         = interiorFacesOffset_;

      clone.generateNewSubmeshForInteriorFaces_ = generateNewSubmeshForInteriorFaces_;
    }

    public override CommandNode DeepClone( GameObject dataHolder)
    {
      CNFracture clone = CRTreeNode.CreateInstance<CNFracture>(dataHolder);
      CloneData(clone);
      return clone;
    }

  }
}
