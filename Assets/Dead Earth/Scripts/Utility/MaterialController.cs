using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialController
{
    //Inspector Assigned

    [SerializeField]
    protected Material _material = null;

    [SerializeField]
    protected Texture _diffuseTexture = null;

    [SerializeField]
    private Color _diffuseColor = Color.white;

    [SerializeField]
    protected Texture _normalMap = null;

    [SerializeField]
    protected float _normalStrength = 1.0f;

    [SerializeField]
    protected Texture _emissiveTexture = null;

    [SerializeField]
    protected Color _emissionColor = Color.black;

    [SerializeField]
    protected float _emissionScale = 1.0f;

    //Internals /protected

    protected MaterialController _backup = null;
    protected bool _started = false;

    //property to fetch underlying material
    public Material material { get { return _material; } }

    public void OnStart()
    {
        if (_material == null || _started) return;

        _started = true;
        _backup = new MaterialController();

        // Backup settings in a temp controller
        _backup._diffuseColor = _material.GetColor("_Color");
        _backup._diffuseTexture = _material.GetTexture("_MainTex");
        _backup._emissionColor = _material.GetColor("_EmissionColor");
        _backup._emissionScale = 1;
        _backup._emissiveTexture = _material.GetTexture("_EmissionMap");
        _backup._normalMap = _material.GetTexture("_BumpMap");
        _backup._normalStrength = _material.GetFloat("_BumpScale");

        // Register this controller with the game scene manager using material instance ID. The GameScene manager will reset
        // all registered materials when the scene closes
        if (GameSceneManager.instance)
            GameSceneManager.instance.RegisterMaterialController(_material.GetInstanceID(), this);
    }

    public void Activate(bool activate)
    {
        // Can't call this function until it's start has been called
        if (!_started || _material == null) return;

        // Set the _material to the assigned properties
        if (activate)
        {
            _material.SetColor("_Color", _diffuseColor);
            _material.SetTexture("_MainTex", _diffuseTexture);
            _material.SetColor("_EmissionColor", _emissionColor * _emissionScale);
            _material.SetTexture("_EmissionMap", _emissiveTexture);
            _material.SetTexture("_BumpMap", _normalMap);
            _material.SetFloat("_BumpScale", _normalStrength);
        }
        else
        {
            _material.SetColor("_Color", _backup._diffuseColor);
            _material.SetTexture("_MainTex", _backup._diffuseTexture);
            _material.SetColor("_EmissionColor", _backup._emissionColor * _backup._emissionScale);
            _material.SetTexture("_EmissionMap", _backup._emissiveTexture);
            _material.SetTexture("_BumpMap", _backup._normalMap);
            _material.SetFloat("_BumpScale", _backup._normalStrength);
        }
    }

    // ------------------------------------------------------------------------------------------------
    // Name	:	OnReset
    // Desc	:	Called to reset the _material. This should be called only by the game scene manager
    //			otherwise you could overwrite the properties of your _material asset
    // ------------------------------------------------------------------------------------------------
    public void OnReset()
    {
        if (_backup == null || _material == null) return;

        _material.SetColor("_Color", _backup._diffuseColor);
        _material.SetTexture("_MainTex", _backup._diffuseTexture);
        _material.SetColor("_EmissionColor", _backup._emissionColor * _backup._emissionScale);
        _material.SetTexture("_EmissionMap", _backup._emissiveTexture);
        _material.SetTexture("_BumpMap", _backup._normalMap);
        _material.SetFloat("_BumpScale", _backup._normalStrength);
    }

    // ------------------------------------------------------------------------------------------------
    // Name	:	GetInstanceID
    // Desc	:	Returns the instance ID of the managed _material
    // ------------------------------------------------------------------------------------------------
    public int GetInstanceID()
    {
        if (_material == null) return -1;
        return _material.GetInstanceID();
    }
}