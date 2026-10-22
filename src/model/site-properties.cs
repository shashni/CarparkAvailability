(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating ISiteProperty 'CacheTtlSeconds' ***/
    var cacheTtlSeconds = eSpace.CreateSiteProperty(false, "CacheTtlSeconds");
    cacheTtlSeconds.DataType = eSpace.IntegerType;
    cacheTtlSeconds.SetDefaultValue("60");
    cacheTtlSeconds.Description = "How long a successful retrieval is reused before calling data.gov.sg again. The source dataset updates about once a minute.";
    cacheTtlSeconds.Folder = null; // default value
    cacheTtlSeconds.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    cacheTtlSeconds.IsSecret = false; // default value
    
    /*** creating ISiteProperty 'CarparkInfoMaxAgeHours' ***/
    var carparkInfoMaxAgeHours = eSpace.CreateSiteProperty(false, "CarparkInfoMaxAgeHours");
    carparkInfoMaxAgeHours.DataType = eSpace.IntegerType;
    carparkInfoMaxAgeHours.SetDefaultValue("24");
    carparkInfoMaxAgeHours.Description = "How often the HDB Carpark Information (addresses, carpark types) is re-synchronised from data.gov.sg.";
    carparkInfoMaxAgeHours.Folder = null; // default value
    carparkInfoMaxAgeHours.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    carparkInfoMaxAgeHours.IsSecret = false; // default value
    
    /*** creating ISiteProperty 'DataGovApiKey' ***/
    var dataGovApiKey = eSpace.CreateSiteProperty(false, "DataGovApiKey");
    dataGovApiKey.DataType = eSpace.TextType;
    dataGovApiKey.SetDefaultValue(null); // default value
    dataGovApiKey.Description = "Optional data.gov.sg API key, sent as the x-api-key header when set. Leave empty to use the anonymous (rate-limited) tier.";
    dataGovApiKey.Folder = null; // default value
    dataGovApiKey.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    dataGovApiKey.IsSecret = true;
    
    /*** creating ISiteProperty 'HdbCarparkInfoResourceId' ***/
    var hdbCarparkInfoResourceId = eSpace.CreateSiteProperty(false, "HdbCarparkInfoResourceId");
    hdbCarparkInfoResourceId.DataType = eSpace.TextType;
    hdbCarparkInfoResourceId.SetDefaultValue("\"d_23f946fa557947f93a8043bbef41dd09\"");
    hdbCarparkInfoResourceId.Description = "data.gov.sg datastore resource id of the \'HDB Carpark Information\' dataset.";
    hdbCarparkInfoResourceId.Folder = null; // default value
    hdbCarparkInfoResourceId.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    hdbCarparkInfoResourceId.IsSecret = false; // default value
    
    /*** creating ISiteProperty 'HighAvailabilityMinPct' ***/
    var highAvailabilityMinPct = eSpace.CreateSiteProperty(false, "HighAvailabilityMinPct");
    highAvailabilityMinPct.DataType = eSpace.DecimalType;
    highAvailabilityMinPct.SetDefaultValue("50");
    highAvailabilityMinPct.Description = "A lot type is \'High availability\' when MORE than this percentage of its lots are available.";
    highAvailabilityMinPct.Folder = null; // default value
    highAvailabilityMinPct.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    highAvailabilityMinPct.IsSecret = false; // default value
    
    /*** creating ISiteProperty 'LowAvailabilityMaxPct' ***/
    var lowAvailabilityMaxPct = eSpace.CreateSiteProperty(false, "LowAvailabilityMaxPct");
    lowAvailabilityMaxPct.DataType = eSpace.DecimalType;
    lowAvailabilityMaxPct.SetDefaultValue("20");
    lowAvailabilityMaxPct.Description = "A lot type is \'Low availability\' when it has at least one free lot but LESS than this percentage available. Between this and HighAvailabilityMinPct (inclusive) it is \'Moderate\'.";
    lowAvailabilityMaxPct.Folder = null; // default value
    lowAvailabilityMaxPct.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    lowAvailabilityMaxPct.IsSecret = false; // default value
    
    /*** creating ISiteProperty 'MinRefreshIntervalSeconds' ***/
    var minRefreshIntervalSeconds = eSpace.CreateSiteProperty(false, "MinRefreshIntervalSeconds");
    minRefreshIntervalSeconds.DataType = eSpace.IntegerType;
    minRefreshIntervalSeconds.SetDefaultValue("15");
    minRefreshIntervalSeconds.Description = "Even a user-forced refresh reuses the cached data when it is younger than this, protecting the upstream API from bursts.";
    minRefreshIntervalSeconds.Folder = null; // default value
    minRefreshIntervalSeconds.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    minRefreshIntervalSeconds.IsSecret = false; // default value
    
    /*** creating ISiteProperty 'ReadingOutdatedMinutes' ***/
    var readingOutdatedMinutes = eSpace.CreateSiteProperty(false, "ReadingOutdatedMinutes");
    readingOutdatedMinutes.DataType = eSpace.IntegerType;
    readingOutdatedMinutes.SetDefaultValue("60");
    readingOutdatedMinutes.Description = "A single carpark\'s reading is flagged as outdated when its own update time is this many minutes older than the dataset timestamp.";
    readingOutdatedMinutes.Folder = null; // default value
    readingOutdatedMinutes.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    readingOutdatedMinutes.IsSecret = false; // default value
    
    /*** creating ISiteProperty 'StaleAfterMinutes' ***/
    var staleAfterMinutes = eSpace.CreateSiteProperty(false, "StaleAfterMinutes");
    staleAfterMinutes.DataType = eSpace.IntegerType;
    staleAfterMinutes.SetDefaultValue("15");
    staleAfterMinutes.Description = "Data older than this (measured from the data.gov.sg reading timestamp) is labelled Stale on the dashboard.";
    staleAfterMinutes.Folder = null; // default value
    staleAfterMinutes.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    staleAfterMinutes.IsSecret = false; // default value
}
