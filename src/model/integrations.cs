(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating IRestService 'CarparkOps' ***/
    var carparkOps = eSpace.CreateIntegration<ServiceStudio.Plugin.RESTService.IRestService>("CarparkOps");
    carparkOps.Authentication = ServiceStudio.Plugin.RESTService.Enumerations.Authentication.None; // default value
    carparkOps.BaseURL = "/rest/CarparkOps";
    carparkOps.CrossSiteRequests = ServiceStudio.Plugin.RESTService.Enumerations.CrossSiteRequests.Allow; // default value
    carparkOps.DefaultValueBehavior = ServiceStudio.Plugin.RESTService.Enumerations.DefaultValueBehavior.DontSend; // default value
    carparkOps.Description = "Operational endpoints for monitoring and deployment validation. Read-only, no personal data, no secrets.";
    carparkOps.HttpSecurity = ServiceStudio.Plugin.RESTService.Enumerations.HttpSecurity.SSL; // default value
    carparkOps.Icon = null; // default value
    carparkOps.InternalAccess = ServiceStudio.Plugin.RESTService.Enumerations.InternalAccess.No; // default value
    carparkOps.Interoperability = ServiceStudio.Plugin.RESTService.Enumerations.Interoperability.No; // default value
    carparkOps.ShowDocumentation = ServiceStudio.Plugin.RESTService.Enumerations.ShowDocumentation.Yes; // default value
    carparkOps.URL = "/CarparkAvailability/rest/CarparkOps";
    
    var health = carparkOps.CreateAction("Health");
    /*** Deleting Nodes that are created by default when instantiating an IRestServiceAction ***/
    health.Nodes.ToList().ForEach(x => x.Delete());
    health.Description = "GET /rest/CarparkOps/Health[?Refresh=true]. Reports data freshness, data-quality counters, address-cache state and the effective non-secret configuration. Refresh=true first loads availability through the normal cache (never bypasses the upstream rate protection).";
    health.HTTPMethod = ServiceStudio.Plugin.RESTService.Enumerations.HTTPMethod.GET; // default value
    health.URL = "/CarparkAvailability/rest/CarparkOps/Health";
    health.URLPath = "/Health";
    
    var refresh = health.CreateInputParameter("Refresh");
    refresh.DataType = eSpace.BooleanType;
    refresh.SetDefaultValue(null); // default value
    refresh.Description = null; // default value
    refresh.IsMandatory = false;
    refresh.IsSensitive = ServiceStudio.Plugin.RESTService.Enumerations.IsSensitive.NotSensitive; // default value
    refresh.ReceiveAs = null; // default value
    refresh.ReceiveIn = ServiceStudio.Plugin.RESTService.Enumerations.ReceiveIn.URL; // default value
    
    var startNode = health.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode.IsDisabled = false; // default value
    
    var ifNode = health.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(startNode);
    ifNode.SetCondition("Refresh");
    ifNode.IsDisabled = false; // default value
    ifNode.Label = "Refresh requested?";
    
    var getLatestSnapshot = health.CreateNode<OutSystems.Model.Logic.Nodes.IAggregateNode>("GetLatestSnapshot").Below(ifNode, 1371);
    getLatestSnapshot.CacheInMinutes = null; // default value
    getLatestSnapshot.Description = null; // default value
    getLatestSnapshot.IsDisabled = false; // default value
    getLatestSnapshot.SetMaxRecords("1");
    getLatestSnapshot.SetStartIndex(null); // default value
    getLatestSnapshot.Timeout = null; // default value
    var availabilitySnapshot2 = eSpace.Entities.OfType<OutSystems.Model.Data.IServerEntity>().Named("AvailabilitySnapshot");
    var availabilitySnapshot = getLatestSnapshot.AsDatabaseAggregate.CreateSource(availabilitySnapshot2);
    getLatestSnapshot.AsDatabaseAggregate.CreateSort().SetAttribute("AvailabilitySnapshot.Id");
    
    ifNode.FalseTarget = getLatestSnapshot;
    
    var loadThroughCache = health.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("LoadThroughCache").ToTheRightOf(ifNode, 1828);
    var carpark_GetAvailability = eSpace.ServerActions.Named("Carpark_GetAvailability");
    loadThroughCache.Action = carpark_GetAvailability;
    loadThroughCache.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    loadThroughCache.IsDisabled = false; // default value
    loadThroughCache.ServerRequestTimeout = null; // default value
    loadThroughCache.Target = getLatestSnapshot;
    var forceRefresh = carpark_GetAvailability.InputParameters.Named("ForceRefresh");
    loadThroughCache.SetArgumentValue(forceRefresh, "False");
    ifNode.TrueTarget = loadThroughCache;
    
    var getLatestCarparkInfo = health.CreateNode<OutSystems.Model.Logic.Nodes.IAggregateNode>("GetLatestCarparkInfo").ConnectedBelow(getLatestSnapshot, 1371);
    getLatestCarparkInfo.CacheInMinutes = null; // default value
    getLatestCarparkInfo.Description = null; // default value
    getLatestCarparkInfo.IsDisabled = false; // default value
    getLatestCarparkInfo.SetMaxRecords("1");
    getLatestCarparkInfo.SetStartIndex(null); // default value
    getLatestCarparkInfo.Timeout = null; // default value
    var carparkInfo2 = eSpace.Entities.OfType<OutSystems.Model.Data.IServerEntity>().Named("CarparkInfo");
    var carparkInfo = getLatestCarparkInfo.AsDatabaseAggregate.CreateSource(carparkInfo2);
    getLatestCarparkInfo.AsDatabaseAggregate.CreateSort().SetAttribute("CarparkInfo.SyncedOn");
    
    
    var assignNode = health.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(getLatestCarparkInfo, 1371);
    assignNode.IsDisabled = false; // default value
    assignNode.Label = "Build report";
    
    var assignment = assignNode.CreateAssignment("Health.Module", "\"CarparkAvailability\"");
    
    var assignment2 = assignNode.CreateAssignment("Health.SnapshotAvailable", "not GetLatestSnapshot.List.Empty");
    
    var assignment3 = assignNode.CreateAssignment("Health.SnapshotAgeSeconds", "If(Health.SnapshotAvailable, DiffSeconds(GetLatestSnapshot.List.Current.AvailabilitySnapshot.RetrievedOn, CurrDateTime()), -1)");
    
    var assignment4 = assignNode.CreateAssignment("Health.DataAgeSeconds", "If(Health.SnapshotAvailable and GetLatestSnapshot.List.Current.AvailabilitySnapshot.DataAgeSecondsAtRetrieval >= 0, GetLatestSnapshot.List.Current.AvailabilitySnapshot.DataAgeSecondsAtRetrieval + Health.SnapshotAgeSeconds, -1)");
    
    var assignment5 = assignNode.CreateAssignment("Health.SourceTimestamp", "If(Health.SnapshotAvailable, FormatDateTime(GetLatestSnapshot.List.Current.AvailabilitySnapshot.SourceTimestamp, \"yyyy-MM-ddTHH:mm:ss\") + \"+08:00\", \"\")");
    
    var assignment6 = assignNode.CreateAssignment("Health.CarparkCount", "GetLatestSnapshot.List.Current.AvailabilitySnapshot.CarparkCount");
    
    var assignment7 = assignNode.CreateAssignment("Health.RejectedCount", "GetLatestSnapshot.List.Current.AvailabilitySnapshot.RejectedCount");
    
    var assignment8 = assignNode.CreateAssignment("Health.DuplicateCount", "GetLatestSnapshot.List.Current.AvailabilitySnapshot.DuplicateCount");
    
    var assignment9 = assignNode.CreateAssignment("Health.AnomalyCount", "GetLatestSnapshot.List.Current.AvailabilitySnapshot.AnomalyCount");
    
    var assignment10 = assignNode.CreateAssignment("Health.HasLocationData", "GetLatestSnapshot.List.Current.AvailabilitySnapshot.HasLocationData");
    
    var assignment11 = assignNode.CreateAssignment("Health.CarparkInfoCount", "LongIntegerToInteger(GetLatestCarparkInfo.Count)");
    
    var assignment12 = assignNode.CreateAssignment("Health.CarparkInfoSyncAgeHours", "If(GetLatestCarparkInfo.List.Empty, -1, DiffHours(GetLatestCarparkInfo.List.Current.CarparkInfo.SyncedOn, CurrDateTime()))");
    
    var assignment13 = assignNode.CreateAssignment("Health.HighAvailabilityMinPct", "Site.HighAvailabilityMinPct");
    
    var assignment14 = assignNode.CreateAssignment("Health.LowAvailabilityMaxPct", "Site.LowAvailabilityMaxPct");
    
    var assignment15 = assignNode.CreateAssignment("Health.StaleAfterMinutes", "Site.StaleAfterMinutes");
    
    var assignment16 = assignNode.CreateAssignment("Health.ReadingOutdatedMinutes", "Site.ReadingOutdatedMinutes");
    
    var assignment17 = assignNode.CreateAssignment("Health.CacheTtlSeconds", "Site.CacheTtlSeconds");
    
    var assignment18 = assignNode.CreateAssignment("Health.MinRefreshIntervalSeconds", "Site.MinRefreshIntervalSeconds");
    
    var assignment19 = assignNode.CreateAssignment("Health.CarparkInfoMaxAgeHours", "Site.CarparkInfoMaxAgeHours");
    
    var assignment20 = assignNode.CreateAssignment("Health.HdbCarparkInfoResourceId", "Site.HdbCarparkInfoResourceId");
    
    var assignment21 = assignNode.CreateAssignment("Health.DataGovApiKeyConfigured", "Site.DataGovApiKey <> \"\"");
    
    var assignment22 = assignNode.CreateAssignment("Health.Status", "If(not Health.SnapshotAvailable, \"no-data\", If(If(Health.DataAgeSeconds >= 0, Health.DataAgeSeconds, Health.SnapshotAgeSeconds) > Site.StaleAfterMinutes * 60, \"stale\", \"ok\"))");
    
    var endNode = health.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode, 1371);
    endNode.IsDisabled = false; // default value
    
    var health2 = health.CreateOutputParameter("Health");
    var healthReport = eSpace.Structures.Named("HealthReport");
    health2.DataType = healthReport;
    health2.Description = null; // default value
    health2.IsSensitive = ServiceStudio.Plugin.RESTService.Enumerations.IsSensitive.NotSensitive; // default value
    health2.SendAs = null; // default value
    health2.SendIn = ServiceStudio.Plugin.RESTService.Enumerations.SendIn.Body; // default value
    
    /*** creating IRestClient 'DataGovSG_Datastore' ***/
    var dataGovSG_Datastore = eSpace.CreateIntegration<ServiceStudio.Plugin.REST.IRestClient>("DataGovSG_Datastore");
    dataGovSG_Datastore.AccessTokenURL = null; // default value
    dataGovSG_Datastore.AuthenticationType = ServiceStudio.Plugin.REST.Enumerations.AuthenticationType.None; // default value
    dataGovSG_Datastore.BaseURL = "https://data.gov.sg/api/action";
    dataGovSG_Datastore.ClientAuthentication = ServiceStudio.Plugin.REST.Enumerations.ClientAuthentication.SendAsBasicAuthHeader; // default value
    dataGovSG_Datastore.ClientID = null; // default value
    dataGovSG_Datastore.ClientSecret = null; // default value
    dataGovSG_Datastore.DateFormat = ServiceStudio.Plugin.REST.Enumerations.DateFormat.ISO; // default value
    dataGovSG_Datastore.Description = "data.gov.sg CKAN datastore API (external dependency), used for the HDB Carpark Information dataset.";
    dataGovSG_Datastore.Icon = null; // default value
    dataGovSG_Datastore.Interoperability = ServiceStudio.Plugin.REST.Enumerations.Interoperability.No; // default value
    dataGovSG_Datastore.Password = null; // default value
    dataGovSG_Datastore.Scopes = null; // default value
    dataGovSG_Datastore.SwaggerDefinitionURL = null; // default value
    dataGovSG_Datastore.Username = null; // default value
    
    var searchHdbCarparkInfo = dataGovSG_Datastore.CreateAction("SearchHdbCarparkInfo");
    searchHdbCarparkInfo.CacheInMinutes = null; // default value
    searchHdbCarparkInfo.DateFormats = null; // default value
    searchHdbCarparkInfo.Description = "GET /datastore_search for the HDB Carpark Information resource.";
    searchHdbCarparkInfo.HTTPMethod = ServiceStudio.Plugin.REST.Enumerations.HTTPMethod.GET; // default value
    searchHdbCarparkInfo.RequestFormat = ServiceStudio.Plugin.REST.Enumerations.RequestFormat.None; // default value
    searchHdbCarparkInfo.ResponseFormat = ServiceStudio.Plugin.REST.Enumerations.ResponseFormat.JSON;
    searchHdbCarparkInfo.SampleRequest = null; // default value
    searchHdbCarparkInfo.SampleResponse = null; // default value
    searchHdbCarparkInfo.TimeoutinSeconds = 60;
    searchHdbCarparkInfo.URLPath = "/datastore_search?resource_id={ResourceId}&limit={Limit}";
    
    var resourceId = searchHdbCarparkInfo.CreateInputParameter("ResourceId");
    resourceId.ContentType = null; // default value
    resourceId.DataType = eSpace.TextType;
    resourceId.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    resourceId.Description = null; // default value
    resourceId.InputPlacement = ServiceStudio.Plugin.REST.Enumerations.InputPlacement.UrlPlacement; // default value
    resourceId.IsMandatory = true; // default value
    resourceId.IsSensitive = ServiceStudio.Plugin.REST.Enumerations.IsSensitive.NotSensitive; // default value
    resourceId.OriginalName = "resource_id";
    
    var limit = searchHdbCarparkInfo.CreateInputParameter("Limit");
    limit.ContentType = null; // default value
    limit.DataType = eSpace.IntegerType;
    limit.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    limit.Description = null; // default value
    limit.InputPlacement = ServiceStudio.Plugin.REST.Enumerations.InputPlacement.UrlPlacement; // default value
    limit.IsMandatory = true; // default value
    limit.IsSensitive = ServiceStudio.Plugin.REST.Enumerations.IsSensitive.NotSensitive; // default value
    limit.OriginalName = "limit";
    
    var apiKey = searchHdbCarparkInfo.CreateInputParameter("ApiKey");
    apiKey.ContentType = null; // default value
    apiKey.DataType = eSpace.TextType;
    apiKey.SetDefaultValue(null); // default value
    apiKey.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    apiKey.Description = null; // default value
    apiKey.InputPlacement = ServiceStudio.Plugin.REST.Enumerations.InputPlacement.HeaderPlacement;
    apiKey.IsMandatory = false;
    apiKey.IsSensitive = ServiceStudio.Plugin.REST.Enumerations.IsSensitive.Sensitive;
    apiKey.OriginalName = "x-api-key";
    
    var response = searchHdbCarparkInfo.CreateOutputParameter("Response");
    response.Description = null; // default value
    response.IsSensitive = ServiceStudio.Plugin.REST.Enumerations.IsSensitive.NotSensitive; // default value
    response.OriginalName = null; // default value
    response.OutputPlacement = ServiceStudio.Plugin.REST.Enumerations.OutputPlacement.BodyPlacement; // default value
    
    var datastoreSearchResponse = dataGovSG_Datastore.CreateStructure("DatastoreSearchResponse");
    datastoreSearchResponse.Description = null; // default value
    datastoreSearchResponse.Folder = null; // default value
    datastoreSearchResponse.Public = false; // default value
    datastoreSearchResponse.Shareable = false; // default value
    
    var success = datastoreSearchResponse.CreateAttribute("Success");
    success.DataType = eSpace.BooleanType;
    success.SetDefaultValue(null); // default value
    success.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    success.Description = null; // default value
    success.IsMandatory = false; // default value
    success.Label = "Success";
    success.NameInJSON = "success";
    
    var result = datastoreSearchResponse.CreateAttribute("Result");
    result.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    result.Description = null; // default value
    result.IsMandatory = false; // default value
    result.Label = "Result";
    result.NameInJSON = "result";
    response.DataType = datastoreSearchResponse;
    
    var datastoreResult = dataGovSG_Datastore.CreateStructure("DatastoreResult");
    datastoreResult.Description = null; // default value
    datastoreResult.Folder = null; // default value
    datastoreResult.Public = false; // default value
    datastoreResult.Shareable = false; // default value
    
    var total = datastoreResult.CreateAttribute("Total");
    total.DataType = eSpace.IntegerType;
    total.SetDefaultValue(null); // default value
    total.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    total.Description = null; // default value
    total.IsMandatory = false; // default value
    total.Label = "Total";
    total.NameInJSON = "total";
    
    var records = datastoreResult.CreateAttribute("Records");
    var hdbCarparkRecord = dataGovSG_Datastore.Structures.OfType<ServiceStudio.Plugin.REST.IRestStructure>().Named("HdbCarparkRecord");
    records.DataType = eSpace.GetOrCreateListType(hdbCarparkRecord);
    records.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    records.Description = null; // default value
    records.IsMandatory = false; // default value
    records.Label = "Records";
    records.NameInJSON = "records";
    result.DataType = datastoreResult;
    
    /*** creating IRestClient 'DataGovSG_Transport' ***/
    var dataGovSG_Transport = eSpace.CreateIntegration<ServiceStudio.Plugin.REST.IRestClient>("DataGovSG_Transport");
    dataGovSG_Transport.AccessTokenURL = null; // default value
    dataGovSG_Transport.AuthenticationType = ServiceStudio.Plugin.REST.Enumerations.AuthenticationType.None; // default value
    dataGovSG_Transport.BaseURL = "https://api.data.gov.sg/v1";
    dataGovSG_Transport.ClientAuthentication = ServiceStudio.Plugin.REST.Enumerations.ClientAuthentication.SendAsBasicAuthHeader; // default value
    dataGovSG_Transport.ClientID = null; // default value
    dataGovSG_Transport.ClientSecret = null; // default value
    dataGovSG_Transport.DateFormat = ServiceStudio.Plugin.REST.Enumerations.DateFormat.ISO; // default value
    dataGovSG_Transport.Description = "data.gov.sg real-time transport APIs (external dependency). Responses are validated and mapped in Carpark_FetchAndMap before use.";
    dataGovSG_Transport.Icon = null; // default value
    dataGovSG_Transport.Interoperability = ServiceStudio.Plugin.REST.Enumerations.Interoperability.No; // default value
    dataGovSG_Transport.Password = null; // default value
    dataGovSG_Transport.Scopes = null; // default value
    dataGovSG_Transport.SwaggerDefinitionURL = null; // default value
    dataGovSG_Transport.Username = null; // default value
    
    var getCarparkAvailability = dataGovSG_Transport.CreateAction("GetCarparkAvailability");
    getCarparkAvailability.CacheInMinutes = null; // default value
    getCarparkAvailability.DateFormats = null; // default value
    getCarparkAvailability.Description = "GET /transport/carpark-availability - latest availability of HDB and other carparks (refreshed about every minute).";
    getCarparkAvailability.HTTPMethod = ServiceStudio.Plugin.REST.Enumerations.HTTPMethod.GET; // default value
    getCarparkAvailability.RequestFormat = ServiceStudio.Plugin.REST.Enumerations.RequestFormat.None; // default value
    getCarparkAvailability.ResponseFormat = ServiceStudio.Plugin.REST.Enumerations.ResponseFormat.JSON;
    getCarparkAvailability.SampleRequest = null; // default value
    getCarparkAvailability.SampleResponse = null; // default value
    getCarparkAvailability.TimeoutinSeconds = 20;
    getCarparkAvailability.URLPath = "/transport/carpark-availability";
    
    var apiKey2 = getCarparkAvailability.CreateInputParameter("ApiKey");
    apiKey2.ContentType = null; // default value
    apiKey2.DataType = eSpace.TextType;
    apiKey2.SetDefaultValue(null); // default value
    apiKey2.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    apiKey2.Description = null; // default value
    apiKey2.InputPlacement = ServiceStudio.Plugin.REST.Enumerations.InputPlacement.HeaderPlacement;
    apiKey2.IsMandatory = false;
    apiKey2.IsSensitive = ServiceStudio.Plugin.REST.Enumerations.IsSensitive.Sensitive;
    apiKey2.OriginalName = "x-api-key";
    
    var response2 = getCarparkAvailability.CreateOutputParameter("Response");
    response2.Description = null; // default value
    response2.IsSensitive = ServiceStudio.Plugin.REST.Enumerations.IsSensitive.NotSensitive; // default value
    response2.OriginalName = null; // default value
    response2.OutputPlacement = ServiceStudio.Plugin.REST.Enumerations.OutputPlacement.BodyPlacement; // default value
    
    var carparkAvailabilityResponse = dataGovSG_Transport.CreateStructure("CarparkAvailabilityResponse");
    carparkAvailabilityResponse.Description = null; // default value
    carparkAvailabilityResponse.Folder = null; // default value
    carparkAvailabilityResponse.Public = false; // default value
    carparkAvailabilityResponse.Shareable = false; // default value
    
    var items = carparkAvailabilityResponse.CreateAttribute("Items");
    var carparkAvailabilityItem = dataGovSG_Transport.Structures.OfType<ServiceStudio.Plugin.REST.IRestStructure>().Named("CarparkAvailabilityItem");
    items.DataType = eSpace.GetOrCreateListType(carparkAvailabilityItem);
    items.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    items.Description = null; // default value
    items.IsMandatory = false; // default value
    items.Label = "Items";
    items.NameInJSON = "items";
    response2.DataType = carparkAvailabilityResponse;
    
    var serverDate = getCarparkAvailability.CreateOutputParameter("ServerDate");
    serverDate.DataType = eSpace.TextType;
    serverDate.SetDefaultValue(null); // default value
    serverDate.Description = null; // default value
    serverDate.IsSensitive = ServiceStudio.Plugin.REST.Enumerations.IsSensitive.NotSensitive; // default value
    serverDate.OriginalName = "Date";
    serverDate.OutputPlacement = ServiceStudio.Plugin.REST.Enumerations.OutputPlacement.HeaderPlacement;
    
    var carparkDataItem = dataGovSG_Transport.CreateStructure("CarparkDataItem");
    carparkDataItem.Description = null; // default value
    carparkDataItem.Folder = null; // default value
    carparkDataItem.Public = false; // default value
    carparkDataItem.Shareable = false; // default value
    
    var carparkNumber = carparkDataItem.CreateAttribute("CarparkNumber");
    carparkNumber.DataType = eSpace.TextType;
    carparkNumber.SetDefaultValue(null); // default value
    carparkNumber.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    carparkNumber.Description = null; // default value
    carparkNumber.IsMandatory = false; // default value
    carparkNumber.Label = "Carpark Number";
    carparkNumber.Length = null; // default value
    carparkNumber.NameInJSON = "carpark_number";
    
    var updateDatetime = carparkDataItem.CreateAttribute("UpdateDatetime");
    updateDatetime.DataType = eSpace.TextType;
    updateDatetime.SetDefaultValue(null); // default value
    updateDatetime.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    updateDatetime.Description = null; // default value
    updateDatetime.IsMandatory = false; // default value
    updateDatetime.Label = "Update Datetime";
    updateDatetime.Length = null; // default value
    updateDatetime.NameInJSON = "update_datetime";
    
    var carparkInfo3 = carparkDataItem.CreateAttribute("CarparkInfo");
    var carparkLotInfo = dataGovSG_Transport.Structures.OfType<ServiceStudio.Plugin.REST.IRestStructure>().Named("CarparkLotInfo");
    carparkInfo3.DataType = eSpace.GetOrCreateListType(carparkLotInfo);
    carparkInfo3.DefaultValueBehavior = ServiceStudio.Plugin.REST.Enumerations.DefaultValueBehavior.DontSend; // default value
    carparkInfo3.Description = null; // default value
    carparkInfo3.IsMandatory = false; // default value
    carparkInfo3.Label = "Carpark Info";
    carparkInfo3.NameInJSON = "carpark_info";
}
