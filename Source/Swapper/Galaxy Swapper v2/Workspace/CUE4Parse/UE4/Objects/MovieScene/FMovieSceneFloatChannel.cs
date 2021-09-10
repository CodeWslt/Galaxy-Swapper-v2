﻿using System;
using System.Runtime.CompilerServices;

using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.MovieScene
{
	public readonly struct FMovieSceneFloatChannel : IUStruct
    {
        public readonly ERichCurveExtrapolation PreInfinityExtrap;
        public readonly ERichCurveExtrapolation PostInfinityExtrap;
        public readonly FFrameNumber[] Times;
        public readonly FMovieSceneFloatValue[] Values;
        public readonly float DefaultValue;
        public readonly bool bHasDefaultValue; // 4 bytes
        public readonly FFrameRate TickResolution;

        public FMovieSceneFloatChannel(FArchive Ar)
        {
            PreInfinityExtrap = Ar.Read<ERichCurveExtrapolation>();
            PostInfinityExtrap = Ar.Read<ERichCurveExtrapolation>();

            var CurrentSerializedElementSize = Unsafe.SizeOf<FFrameNumber>();
            var SerializedElementSize = Ar.Read<int>();

            if (SerializedElementSize == CurrentSerializedElementSize)
            {
                Times = Ar.ReadArray<FFrameNumber>();
            }
            else
            {
                var ArrayNum = Ar.Read<int>();

                if (ArrayNum > 0)
                {
                    var padding = SerializedElementSize - CurrentSerializedElementSize;
                    Times = new FFrameNumber[ArrayNum];

                    for (var i = 0; i < ArrayNum; i++)
                    {
                        Ar.Position += padding;
                        Times[i] = Ar.Read<FFrameNumber>();
                        //Ar.Position += padding; TODO check this
                    }
                }
                else
                {
                    Times = Array.Empty<FFrameNumber>();
                }
            }

            CurrentSerializedElementSize = Unsafe.SizeOf<FMovieSceneFloatValue>();
            SerializedElementSize = Ar.Read<int>();

            if (SerializedElementSize == CurrentSerializedElementSize)
            {
                Values = Ar.ReadArray<FMovieSceneFloatValue>();
            }
            else
            {
                var ArrayNum = Ar.Read<int>();

                if (ArrayNum > 0)
                {
                    var padding = SerializedElementSize - CurrentSerializedElementSize;
                    Values = new FMovieSceneFloatValue[ArrayNum];

                    for (var i = 0; i < ArrayNum; i++)
                    {
                        Ar.Position += padding;
                        Values[i] = Ar.Read<FMovieSceneFloatValue>();
                        //Ar.Position += padding; TODO check this
                    }
                }
                else
                {
                    Values = Array.Empty<FMovieSceneFloatValue>();
                }
            }

            DefaultValue = Ar.Read<float>();
            bHasDefaultValue = Ar.ReadBoolean();
            TickResolution = Ar.Read<FFrameRate>();
            Ar.Position += 4; // Mysterious 4 byte padding, could this be KeyHandles which is inside if (Ar.IsTransacting())?
        }
    }
}
