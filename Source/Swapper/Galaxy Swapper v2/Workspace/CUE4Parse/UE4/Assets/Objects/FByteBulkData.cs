﻿using System;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects
{
    [JsonConverter(typeof(FByteBulkDataConverter))]
    public class FByteBulkData
    {
        public readonly FByteBulkDataHeader Header;
        public readonly EBulkData BulkDataFlag;
        public readonly byte[] Data;

        public FByteBulkData(FAssetArchive Ar)
        {
            Header = new FByteBulkDataHeader(Ar);
            var bulkDataFlags = Header.BulkDataFlags;

            if (Header.ElementCount == 0)
            {
                // Nothing to do here
            }
            else if (EBulkData.BULKDATA_Unused.Check(bulkDataFlags))
            {
                BulkDataFlag = EBulkData.BULKDATA_Unused;
            }
            else if (EBulkData.BULKDATA_ForceInlinePayload.Check(bulkDataFlags))
            {
                BulkDataFlag = EBulkData.BULKDATA_ForceInlinePayload;
                Data = new byte[Header.ElementCount];
                Ar.Read(Data, 0, Header.ElementCount);
            }
            else if (EBulkData.BULKDATA_OptionalPayload.Check(bulkDataFlags))
            {
                BulkDataFlag = EBulkData.BULKDATA_OptionalPayload;
                if (!Ar.TryGetPayload(PayloadType.UPTNL, out var uptnlAr) || uptnlAr == null) return;
                
                Data = new byte[Header.ElementCount];
                uptnlAr.Position = Header.OffsetInFile;
                uptnlAr.Read(Data, 0, Header.ElementCount);
            }
            else if (EBulkData.BULKDATA_PayloadInSeperateFile.Check(bulkDataFlags))
            {
                BulkDataFlag = EBulkData.BULKDATA_PayloadInSeperateFile;
                if (!Ar.TryGetPayload(PayloadType.UBULK, out var ubulkAr) || ubulkAr == null) return;
                
                Data = new byte[Header.ElementCount];
                ubulkAr.Position = Header.OffsetInFile;
                ubulkAr.Read(Data, 0, Header.ElementCount);
            }
            else if (EBulkData.BULKDATA_PayloadAtEndOfFile.Check(bulkDataFlags))
            {
                BulkDataFlag = EBulkData.BULKDATA_PayloadAtEndOfFile;
                //stored in same file, but at different position
                //save archive position
                var savePos = Ar.Position;
                if (Header.OffsetInFile + Header.ElementCount <= Ar.Length)
                {
                    Data = new byte[Header.ElementCount];
                    Ar.Position = Header.OffsetInFile;
                    Ar.Read(Data, 0, Header.ElementCount);
                } else throw new ParserException(Ar, $"Failed to read PayloadAtEndOfFile, {Header.OffsetInFile} is out of range");

                Ar.Position = savePos;
            }
            else if (EBulkData.BULKDATA_CompressedZlib.Check(bulkDataFlags))
            {
                BulkDataFlag = EBulkData.BULKDATA_CompressedZlib;
                throw new ParserException(Ar, "TODO: CompressedZlib");
            }
        }
    }
    
    public class FByteBulkDataConverter : JsonConverter<FByteBulkData>
    {
        public override void WriteJson(JsonWriter writer, FByteBulkData value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("BulkDataFlag");
            writer.WriteValue(value.BulkDataFlag.ToString());
            
            serializer.Serialize(writer, value.Header);
            
            writer.WriteEndObject();
        }

        public override FByteBulkData ReadJson(JsonReader reader, Type objectType, FByteBulkData existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}