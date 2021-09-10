﻿using System.Runtime.CompilerServices;

namespace CUE4Parse.UE4.Versions
{
    public enum EGame
    {
        // bytes: 01.00.0N.NX : 01=UE4, 00=masked by GAME_ENGINE, NN=UE4 subversion, X=game (4 bits, 0=base engine)
        GAME_UE4_0 = GameUtils.GameUe4Base + 0 << 4,
        GAME_UE4_1 = GameUtils.GameUe4Base + 1 << 4,
        GAME_UE4_2 = GameUtils.GameUe4Base + 2 << 4,
        GAME_UE4_3 = GameUtils.GameUe4Base + 3 << 4,
        GAME_UE4_4 = GameUtils.GameUe4Base + 4 << 4,
        GAME_UE4_5 = GameUtils.GameUe4Base + 5 << 4,
        GAME_UE4_6 = GameUtils.GameUe4Base + 6 << 4,
        GAME_UE4_7 = GameUtils.GameUe4Base + 7 << 4,
        GAME_UE4_8 = GameUtils.GameUe4Base + 8 << 4,
        GAME_UE4_9 = GameUtils.GameUe4Base + 9 << 4,
        GAME_UE4_10 = GameUtils.GameUe4Base + 10 << 4,
        GAME_UE4_11 = GameUtils.GameUe4Base + 11 << 4,
        GAME_UE4_12 = GameUtils.GameUe4Base + 12 << 4,
        GAME_UE4_13 = GameUtils.GameUe4Base + 13 << 4,
            GAME_SOD2 = GAME_UE4_13 + 1,
        GAME_UE4_14 = GameUtils.GameUe4Base + 14 << 4,
        GAME_UE4_15 = GameUtils.GameUe4Base + 15 << 4,
        GAME_UE4_16 = GameUtils.GameUe4Base + 16 << 4,
        GAME_UE4_17 = GameUtils.GameUe4Base + 17 << 4,
        GAME_UE4_18 = GameUtils.GameUe4Base + 18 << 4,
        GAME_UE4_19 = GameUtils.GameUe4Base + 19 << 4,
        GAME_UE4_20 = GameUtils.GameUe4Base + 20 << 4,
            GAME_BORDERLANDS3 = GAME_UE4_20 + 1,
        GAME_UE4_21 = GameUtils.GameUe4Base + 21 << 4,
        GAME_UE4_22 = GameUtils.GameUe4Base + 22 << 4,
        GAME_UE4_23 = GameUtils.GameUe4Base + 23 << 4,
        GAME_UE4_24 = GameUtils.GameUe4Base + 24 << 4,
            GAME_VALORANT = GAME_UE4_24 + 1,
        GAME_UE4_25 = GameUtils.GameUe4Base + 25 << 4,
        GAME_UE4_26 = GameUtils.GameUe4Base + 26 << 4,
        GAME_UE4_27 = GameUtils.GameUe4Base + 27 << 4,
        
        GAME_UE4_LAST,
        GAME_UE4_LATEST = GAME_UE4_LAST - 1
    }
    
    public static class GameUtils
    {
        public const int GameUe4Base = 0x1000000;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GAME_UE4(int x)
        {
            return GameUe4Base + x << 4;
        }

        public static UE4Version GetVersion(this EGame game)
        {
            // Custom UE Games
            // If a game needs a even more specific custom version than the major release version you can add it below
            // if (game == EGame.GAME_VALORANT)
            //     return UE4Version.VER_UE4_24;

            return game switch
            {
                // General UE4 Versions
                < EGame.GAME_UE4_1 => UE4Version.VER_UE4_0,
                < EGame.GAME_UE4_2 => UE4Version.VER_UE4_1,
                < EGame.GAME_UE4_3 => UE4Version.VER_UE4_2,
                < EGame.GAME_UE4_4 => UE4Version.VER_UE4_3,
                < EGame.GAME_UE4_5 => UE4Version.VER_UE4_4,
                < EGame.GAME_UE4_6 => UE4Version.VER_UE4_5,
                < EGame.GAME_UE4_7 => UE4Version.VER_UE4_6,
                < EGame.GAME_UE4_8 => UE4Version.VER_UE4_7,
                < EGame.GAME_UE4_9 => UE4Version.VER_UE4_8,
                < EGame.GAME_UE4_10 => UE4Version.VER_UE4_9,
                < EGame.GAME_UE4_11 => UE4Version.VER_UE4_10,
                < EGame.GAME_UE4_12 => UE4Version.VER_UE4_11,
                < EGame.GAME_UE4_13 => UE4Version.VER_UE4_12,
                < EGame.GAME_UE4_14 => UE4Version.VER_UE4_13,
                < EGame.GAME_UE4_15 => UE4Version.VER_UE4_14,
                < EGame.GAME_UE4_16 => UE4Version.VER_UE4_15,
                < EGame.GAME_UE4_17 => UE4Version.VER_UE4_16,
                < EGame.GAME_UE4_18 => UE4Version.VER_UE4_17,
                < EGame.GAME_UE4_19 => UE4Version.VER_UE4_18,
                < EGame.GAME_UE4_20 => UE4Version.VER_UE4_19,
                < EGame.GAME_UE4_21 => UE4Version.VER_UE4_20,
                < EGame.GAME_UE4_22 => UE4Version.VER_UE4_21,
                < EGame.GAME_UE4_23 => UE4Version.VER_UE4_22,
                < EGame.GAME_UE4_24 => UE4Version.VER_UE4_23,
                < EGame.GAME_UE4_25 => UE4Version.VER_UE4_24,
                < EGame.GAME_UE4_26 => UE4Version.VER_UE4_25,
                < EGame.GAME_UE4_27 => UE4Version.VER_UE4_26,
                _ => UE4Version.VER_UE4_LATEST
            };
        }
    }
}