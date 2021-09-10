﻿using System;
using System.IO;
using System.Linq;
using CUE4Parse.UE4.Assets.Exports.Material;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Texture
{
    public class UTexture2D : UTexture
    {
        public FTexture2DMipMap[] Mips { get; private set; }
        public int FirstMip { get; private set; }
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public int NumSlices { get; private set; } // important only while UTextureCube4 is derived from UTexture2D in out implementation
        public bool IsVirtual { get; private set; }
        public EPixelFormat Format { get; private set; } = EPixelFormat.PF_Unknown;
        public FIntPoint ImportedSize { get; private set; }

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);
            // UObject Properties
            ImportedSize = GetOrDefault<FIntPoint>(nameof(ImportedSize));

            var stripDataFlags = Ar.Read<FStripDataFlags>();
            var bCooked = Ar.Ver >= UE4Version.VER_UE4_ADD_COOKED_TO_TEXTURE2D && Ar.ReadBoolean();
            if (Ar.Ver < UE4Version.VER_UE4_TEXTURE_SOURCE_ART_REFACTOR)
            {
                // https://github.com/gildor2/UEViewer/blob/master/Unreal/UnrealMaterial/UnTexture4.cpp#L166
                // This code lives in UTexture2D::LegacySerialize(). It relies on some deprecated properties, and modern
                // code UE4 can't read cooked packages prepared with pre-VER_UE4_TEXTURE_SOURCE_ART_REFACTOR version of
                // the engine. So, it's not possible to know what should happen there unless we'll get some working game
                // which uses old UE4 version.bDisableDerivedDataCache_DEPRECATED in UE4 serialized as property, when set
                // to true - has serialization of TArray<FTexture2DMipMap>. We suppose here that it's 'false'.
                var textureFileCacheGuidDeprecated = Ar.Read<FGuid>();
            }

            // Formats are added in UE4 in Source/Developer/<Platform>TargetPlatform/Private/<Platform>TargetPlatform.h,
            // in TTargetPlatformBase::GetTextureFormats(). Formats are choosen depending on platform settings (for example,
            // for Android) or depending on UTexture::CompressionSettings. Windows, Linux and Mac platform uses the same
            // texture format (see FTargetPlatformBase::GetDefaultTextureFormatName()). Other platforms uses different formats,
            // but all of them (except Android) uses single texture format per object.

            if (bCooked)
            {
                var pixelFormatEnum = Ar.ReadFName();
                while (!pixelFormatEnum.IsNone)
                {
                    var skipOffset = Ar.Game >= EGame.GAME_UE4_20 ? Ar.Read<long>() : Ar.Read<int>();

                    var pixelFormat = EPixelFormat.PF_Unknown;
                    Enum.TryParse(pixelFormatEnum.Text, out pixelFormat);

                    if (Format == EPixelFormat.PF_Unknown)
                    {
                        //?? check whether we can support this pixel format
                        var data = new FTexturePlatformData(Ar);
                        if (Ar.AbsolutePosition != skipOffset)
                        {
                            Ar.SeekAbsolute(skipOffset, SeekOrigin.Begin);
                        }

                        // copy data to UTexture2D
                        Mips = data.Mips;
                        FirstMip = data.FirstMip;
                        SizeX = data.SizeX;
                        SizeY = data.SizeY;
                        NumSlices = data.NumSlices;
                        IsVirtual = data.bIsVirtual;
                        Format = pixelFormat;
                    }
                    else
                    {
                        Ar.SeekAbsolute(skipOffset, SeekOrigin.Begin);
                    }
                    // read next format name
                    pixelFormatEnum = Ar.ReadFName();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FTexture2DMipMap? GetFirstMip() => Mips.FirstOrDefault(x => x.Data.Data != null);

        public override void GetParams(CMaterialParams parameters)
        {
            // ???
        }

        protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            base.WriteJson(writer, serializer);

            writer.WritePropertyName("Mips");
            writer.WriteStartArray();
            {
                foreach (var mip in Mips)
                {
                    serializer.Serialize(writer, mip);
                }
            }
            writer.WriteEndArray();

            writer.WritePropertyName("FirstMip");
            writer.WriteValue(FirstMip);

            writer.WritePropertyName("SizeX");
            writer.WriteValue(SizeX);

            writer.WritePropertyName("SizeY");
            writer.WriteValue(SizeY);

            writer.WritePropertyName("NumSlices");
            writer.WriteValue(NumSlices);

            writer.WritePropertyName("IsVirtual");
            writer.WriteValue(IsVirtual);

            writer.WritePropertyName("Format");
            writer.WriteValue(Format.ToString());
        }
    }
}