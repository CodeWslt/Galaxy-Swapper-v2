﻿using System;
using System.Runtime.CompilerServices;

namespace CUE4Parse.Utils
{
    public static class MathUtils
    {
        
        public static bool IsNumericType(this object o)
        {   
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
        
        public static float InvSqrt(this float x)
        {
            float xhalf = 0.5f * x;
            int i = BitConverter.SingleToInt32Bits(x);
            i = 0x5f3759df - (i >> 1);
            x = BitConverter.Int32BitsToSingle(i);
            x = x * (1.5f - xhalf * x * x);
            return x;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivideAndRoundUp(this int dividend, int divisor) => (dividend + divisor - 1) / divisor;
        
        public static float ToRadians(this float val)
        {
            return ((float) Math.PI / 180f) * val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Square(this float val) => val * val;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TruncToInt(this float f) => (int) f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TruncToInt(this double f) => (int) f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this float f) => Math.Floor(f).TruncToInt();
        public static int RoundToInt(this float f) => FloorToInt(f + 0.5f);
    }
}