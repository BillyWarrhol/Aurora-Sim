﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using OpenSim.Framework;
using Nini.Config;

namespace Aurora.Framework
{
    public interface IProfileData
    {
        List<string> ReadClassifiedInfoRow(string classifiedID);
        Dictionary<UUID, string> ReadClassifedRow(string creatoruuid);
        Dictionary<UUID, string> ReadPickRow(string creator);
        List<string> ReadInterestsInfoRow(string agentID);
        List<string> ReadPickInfoRow(string creator, string pickID);
        AuroraProfileData GetProfileNotes(UUID agentID, UUID target);
        bool InvalidateProfileNotes(UUID target);
        bool FullUpdateUserProfile(AuroraProfileData Profile);
        AuroraProfileData GetProfileInfo(UUID agentID);

        bool UpdateUserProfile(AuroraProfileData Profile);

        AuroraProfileData CreateTemperaryAccount(string client, string first, string last);
        
		DirPlacesReplyData[] PlacesQuery(string queryText, string category, string table, string wantedValue, int StartQuery);
        DirLandReplyData[] LandForSaleQuery(string searchType, string price, string area, string table, string wantedValue, int StartQuery);
        DirClassifiedReplyData[] ClassifiedsQuery(string queryText, string category, string queryFlags, int StartQuery);
        DirEventsReplyData[] EventQuery(string queryText, string flags, string table, string wantedValue, int StartQuery);
        EventData GetEventInfo(string p);
        DirEventsReplyData[] GetAllEventsNearXY(string table, int X, int Y);
        EventData[] GetEvents();
        Classified[] GetClassifieds();

    }
    public class Classified
    {
        public string UUID;
        public string CreatorUUID;
        public string CreationDate;
        public string ExpirationDate;
        public string Category;
        public string Name;
        public string Description;
        public string ParcelUUID;
        public string ParentEstate;
        public string SnapshotUUID;
        public string SimName;
        public string PosGlobal;
        public string ParcelName;
        public string ClassifiedFlags;
        public string PriceForListing;
    }
    public interface IRegionData
    {
        Dictionary<string, string> GetRegionHidden();
        string AbuseReports();
        ObjectMediaURLInfo[] getObjectMediaInfo(string objectID);
        bool GetIsRegionMature(string region);
        bool StoreRegionWindlightSettings(RegionLightShareData wl);
        RegionLightShareData LoadRegionWindlightSettings(UUID regionUUID);

        AbuseReport GetAbuseReport(int formNumber);

        OfflineMessage[] GetOfflineMessages(string agentID);

        bool AddOfflineMessage(string fromUUID, string fromName, string toUUID, string message);
    }

    public interface IEstateData
    {
        EstateSettings LoadEstateSettings(UUID regionID, bool create);
        EstateSettings LoadEstateSettings(int estateID);
        bool StoreEstateSettings(EstateSettings es);
        List<int> GetEstates(string search);
        bool LinkRegion(UUID regionID, int estateID);
        List<UUID> GetRegions(int estateID);
        bool DeleteEstate(int estateID);
    }

    public class OfflineMessage
    {
        public string FromUUID;
        public string ToUUID;
        public string FromName;
        public string Message;
    }

    public class AbuseReport
    {
        public string Category;
        public string Reporter;
        public string ObjectName;
        public string ObjectUUID;
        public string Abuser;
        public string Location;
        public string Details;
        public string Position;
        public string Estate;
        public string Summary;
        public string ReportNumber;
        public string AssignedTo;
        public string Active;
        public string Checked;
        public string Notes;
    }
    
    public interface IGenericData
    {
        /// <summary>
        /// update table set setRow = setValue WHERE keyRow = keyValue
        /// </summary>
        bool Update(string table, string[] setValues, string[] setRows, string[] keyRows, string[] keyValues);
        /// <summary>
        /// select wantedValue from table where keyRow = keyValue
        /// </summary>
        List<string> Query(string keyRow, string keyValue, string table, string wantedValue);
        List<string> Query(string[] keyRow, string[] keyValue, string table, string wantedValue);
        bool Insert(string table, string[] values);
        bool Delete(string table, string[] keys, string[] values);
        bool Insert(string table, string[] values, string updateKey, string updateValue);
    }

    public interface IDataConnector : IGenericData
    {
        void ConnectToDatabase(string connectionString);
        void CloseDatabase();
        bool TableExists(string table);
        void CreateTable(string table, ColumnDefinition[] columns);
        Version GetAuroraVersion();
        void WriteAuroraVersion(Version version);
        void CopyTableToTable(string sourceTableName, string destinationTableName, ColumnDefinition[] columnDefinitions);
        bool VerifyTableExists(string tableName, ColumnDefinition[] columnDefinitions);
        void EnsureTableExists(string tableName, ColumnDefinition[] columnDefinitions);
        void DropTable(string tableName);
    }

    public enum DataManagerTechnology
    {
        SQLite,
        MySql
    }

    public enum ColumnTypes
    {
        Integer,
        String,
        Date,
        String50,
        String512,
        String45,
        String1024,
        String2,
        String1,
        String100
    }
    public class ColumnDefinition
    {
        public string Name { get; set; }
        public ColumnTypes Type { get; set; }
        public bool IsPrimary { get; set; }

        public override bool Equals(object obj)
        {
            var cdef = obj as ColumnDefinition;
            if (cdef != null)
            {
                return cdef.Name == Name && cdef.Type == Type && cdef.IsPrimary == IsPrimary;
            }
            return false;
        }
    }

    public class ObjectMediaURLInfo
    {
        public string alt_image_enable = "";
        public bool auto_loop = true;
        public bool auto_play = true;
        public bool auto_scale = true;
        public bool auto_zoom = false;
        public int controls = 0;
        public string current_url = "http://www.google.com/";
        public bool first_click_interact = false;
        public int height_pixels = 0;
        public string home_url = "http://www.google.com/";
        public int perms_control = 7;
        public int perms_interact = 7;
        public string whitelist = "";
        public bool whitelist_enable = false;
        public int width_pixels = 0;
        public string object_media_version;
    }
    public interface IDataService
    {
        void Initialise(IScene scene, Nini.Config.IConfigSource source);
        IGenericData GetGenericPlugin();
        IEstateData GetEstatePlugin();
        IProfileData GetProfilePlugin();
        IRegionData GetRegionPlugin();
        void SetGenericDataPlugin(IGenericData Plugin);
        void SetEstatePlugin(IEstateData Plugin);
        void SetProfilePlugin(IProfileData Plugin);
        void SetRegionPlugin(IRegionData Plugin);
    }
}
