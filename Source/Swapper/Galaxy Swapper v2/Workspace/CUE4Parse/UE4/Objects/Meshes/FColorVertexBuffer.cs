﻿using System;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Meshes
{
    [JsonConverter(typeof(FColorVertexBufferConverter))]
    public class FColorVertexBuffer
    {
        public readonly FColor[] Data;
        public readonly int Stride;
        public readonly int NumVertices;

        public FColorVertexBuffer(FArchive Ar)
        {
            var stripDataFlags = Ar.Ver >= UE4Version.VER_UE4_STATIC_SKELETAL_MESH_SERIALIZATION_FIX ? Ar.Read<FStripDataFlags>() : new FStripDataFlags();

            Stride = Ar.Read<int>();
            NumVertices = Ar.Read<int>();

            if (!stripDataFlags.IsDataStrippedForServer() & NumVertices > 0)
            {
                Data = Ar.ReadBulkArray(Ar.Read<FColor>);
            }
            else
            {
                Data = new FColor[0];
            }
        }
    }
    
    public class FColorVertexBufferConverter : JsonConverter<FColorVertexBuffer>
    {
        public override void WriteJson(JsonWriter writer, FColorVertexBuffer value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Data");
            serializer.Serialize(writer, value.Data);

            writer.WritePropertyName("Stride");
            writer.WriteValue(value.Stride);
            
            writer.WritePropertyName("NumVertices");
            writer.WriteValue(value.NumVertices);

            writer.WriteEndObject();
        }

        public override FColorVertexBuffer ReadJson(JsonReader reader, Type objectType, FColorVertexBuffer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
