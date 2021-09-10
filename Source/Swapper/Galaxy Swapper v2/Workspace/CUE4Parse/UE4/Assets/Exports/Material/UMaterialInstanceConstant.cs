﻿using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Assets.Exports.Material
{
    public class UMaterialInstanceConstant : UMaterialInstance
    {
        public FScalarParameterValue[] ScalarParameterValues;
        public FTextureParameterValue[] TextureParameterValues;
        public FVectorParameterValue[] VectorParameterValues;

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);
            ScalarParameterValues = GetOrDefault<FScalarParameterValue[]>(nameof(ScalarParameterValues)) ?? new FScalarParameterValue[0];
            TextureParameterValues = GetOrDefault<FTextureParameterValue[]>(nameof(TextureParameterValues)) ?? new FTextureParameterValue[0];
            VectorParameterValues = GetOrDefault<FVectorParameterValue[]>(nameof(VectorParameterValues)) ?? new FVectorParameterValue[0];
        }

        public override void GetParams(CMaterialParams parameters)
        {
            // get params from linked UMaterial3
            if (Parent != null && Parent != this)
                Parent.GetParams(parameters);
            
            base.GetParams(parameters);
            
            // get local parameters
            var diffWeight = 0;
            var normWeight = 0;
            var specWeight = 0;
            var specPowWeight = 0;
            var opWeight = 0;
            var emWeight = 0;
            var emcWeight = 0;
            var cubeWeight = 0;
            var maskWeight = 0;

            void Diffuse(bool check, int weight, UTexture tex)
            {
                if (check && weight > diffWeight) {
                    parameters.Diffuse = tex;
                    diffWeight = weight;
                }
            }
            
            void Normal(bool check, int weight, UTexture tex)
            {
                if (check && weight > normWeight) {
                    parameters.Normal = tex;
                    normWeight = weight;
                }
            }
            
            void Specular(bool check, int weight, UTexture tex)
            {
                if (check && weight > specWeight) {
                    parameters.Specular = tex;
                    specWeight = weight;
                }
            }
            
            void SpecPower(bool check, int weight, UTexture tex)
            {
                if (check && weight > specPowWeight) {
                    parameters.SpecPower = tex;
                    specPowWeight = weight;
                }
            }
            
            void Opacity(bool check, int weight, UTexture tex)
            {
                if (check && weight > opWeight) {
                    parameters.Opacity = tex;
                    opWeight = weight;
                }
            }
            
            void Emissive(bool check, int weight, UTexture tex)
            {
                if (check && weight > emWeight) {
                    parameters.Emissive = tex;
                    emWeight = weight;
                }
            }
            
            void CubeMap(bool check, int weight, UTexture tex)
            {
                if (check && weight > cubeWeight) {
                    parameters.Cube = tex;
                    cubeWeight = weight;
                }
            }
            
            void BakedMask(bool check, int weight, UTexture tex)
            {
                if (check && weight > maskWeight) {
                    parameters.Mask = tex;
                    maskWeight = weight;
                }
            }
            
            void EmissiveColor(bool check, int weight, FLinearColor color)
            {
                if (check && weight > emcWeight) {
                    parameters.EmissiveColor = color;
                    emcWeight = weight;
                }
            }

            if (TextureParameterValues.Length > 0)
                parameters.Opacity = null;     // it's better to disable opacity mask from parent material
            
            foreach (var p in TextureParameterValues)
            {
                var name = p.Name;
                var tex = p.ParameterValue;
                if (tex == null) continue;
                
                if (name.Contains("detail", StringComparison.CurrentCultureIgnoreCase)) continue;

                Diffuse(name.Contains("dif", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                Diffuse(name.Contains("albedo", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                Diffuse(name.Contains("color", StringComparison.CurrentCultureIgnoreCase), 80, tex);
                Normal(name.Contains("norm", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("fx", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                SpecPower(name.Contains("specpow", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                Specular(name.Contains("spec", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                Emissive(name.Contains("emiss", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                CubeMap(name.Contains("cube", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                CubeMap(name.Contains("refl", StringComparison.CurrentCultureIgnoreCase), 90, tex);
                Opacity(name.Contains("opac", StringComparison.CurrentCultureIgnoreCase), 90, tex);
                Opacity(name.Contains("trans", StringComparison.CurrentCultureIgnoreCase) && !name.Contains("transm", StringComparison.CurrentCultureIgnoreCase), 80, tex);
                Opacity(name.Contains("opacity", StringComparison.CurrentCultureIgnoreCase), 100, tex);
                Opacity(name.Contains("alpha", StringComparison.CurrentCultureIgnoreCase), 100, tex);
            }
            
            foreach (var p in VectorParameterValues)
            {
                var name = p.Name;
                var color = p.ParameterValue;
                if (color != null)
                    EmissiveColor(name.Contains("Emissive", StringComparison.CurrentCultureIgnoreCase), 100, color.Value);
            }
            
            // try to get diffuse texture when nothing found
            if (parameters.Diffuse == null && TextureParameterValues.Length == 1)
                parameters.Diffuse = TextureParameterValues[0].ParameterValue;
        }

        public override void AppendReferencedTextures(IList<UUnrealMaterial> outTextures, bool onlyRendered)
        {
            if (onlyRendered)
            {
                // default implementation does that
                base.AppendReferencedTextures(outTextures, true);
            }
            else
            {
                foreach (var value in TextureParameterValues)
                {
                    if (value.ParameterValue != null && !outTextures.Contains(value.ParameterValue))
                        outTextures.Add(value.ParameterValue);
                }

                if (Parent != null && Parent != this)
                    Parent.AppendReferencedTextures(outTextures, onlyRendered);
            }
        }
    }
}