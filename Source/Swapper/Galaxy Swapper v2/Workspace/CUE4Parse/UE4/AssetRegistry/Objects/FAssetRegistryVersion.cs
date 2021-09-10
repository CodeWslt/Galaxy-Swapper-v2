﻿using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.AssetRegistry.Objects
{
    public enum FAssetRegistryVersionType : int
    {
        PreVersioning = 0,		// From before file versioning was implemented
        HardSoftDependencies,	// The first version of the runtime asset registry to include file versioning.
        AddAssetRegistryState,	// Added FAssetRegistryState and support for piecemeal serialization
        ChangedAssetData,		// AssetData serialization format changed, versions before this are not readable
        RemovedMD5Hash,			// Removed MD5 hash from package data
        AddedHardManage,		// Added hard/soft manage references
        AddedCookedMD5Hash,		// Added MD5 hash of cooked package to package data
        AddedDependencyFlags,   // Added UE::AssetRegistry::EDependencyProperty to each dependency
        FixedTags,				// Major tag format change that replaces USE_COMPACT_ASSET_REGISTRY:
        // * Target tag INI settings cooked into tag data
        // * Instead of FString values are stored directly as one of:
        //		- Narrow / wide string
        //		- [Numberless] FName
        //		- [Numberless] export path
        //		- Localized string
        // * All value types are deduplicated
        // * All key-value maps are cooked into a single contiguous range 
        // * Switched from FName table to seek-free and more optimized FName batch loading
        // * Removed global tag storage, a tag map reference-counts one store per asset registry
        // * All configs can mix fixed and loose tag maps 

        // -----<new versions can be added above this line>-------------------------------------------------
        VersionPlusOne,
        LatestVersion = VersionPlusOne - 1
    }
    
    public static class FAssetRegistryVersion
    {
        private static readonly FGuid _GUID = new (0x717F9EE7, 0xE9B0493A, 0x88B39132, 0x1B388107);

        public static bool TrySerializeVersion(FArchive Ar, out FAssetRegistryVersionType version)
        {
            var guid = Ar.Read<FGuid>();
            if (guid == _GUID)
            {
                version = Ar.Read<FAssetRegistryVersionType>();
                return true;
            }

            version = FAssetRegistryVersionType.LatestVersion;
            return false;
        }
    }
}