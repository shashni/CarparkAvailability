(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating IServerAction 'CarparkInfo_EnsureFresh' ***/
    var carparkInfo_EnsureFresh = eSpace.CreateServerAction("CarparkInfo_EnsureFresh");
    carparkInfo_EnsureFresh.CacheInMinutes = null; // default value
    carparkInfo_EnsureFresh.Description = "Re-synchronises the CarparkInfo cache from the HDB Carpark Information dataset when it is empty or older than Site.CarparkInfoMaxAgeHours. Failures are logged and the previous cache is kept, so availability never depends on this call.";
    carparkInfo_EnsureFresh.Folder = null; // default value
    carparkInfo_EnsureFresh.Function = false; // default value
    carparkInfo_EnsureFresh.Icon = null; // default value
    carparkInfo_EnsureFresh.Public = false; // default value
    carparkInfo_EnsureFresh.Shareable = false; // default value
    
    var newInfo = carparkInfo_EnsureFresh.CreateLocalVariable("NewInfo");
    var carparkInfo = eSpace.Entities.OfType<OutSystems.Model.Data.IServerEntity>().Named("CarparkInfo");
    newInfo.DataType = carparkInfo;
    newInfo.Description = null; // default value
    
    var startNode = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode.HorizontalPosition = 3200;
    startNode.IsDisabled = false; // default value
    startNode.VerticalPosition = 800;
    
    var getLatestCarparkInfo = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IAggregateNode>("GetLatestCarparkInfo").ConnectedBelow(startNode, 1371);
    getLatestCarparkInfo.CacheInMinutes = null; // default value
    getLatestCarparkInfo.Description = null; // default value
    getLatestCarparkInfo.IsDisabled = false; // default value
    getLatestCarparkInfo.SetMaxRecords("1");
    getLatestCarparkInfo.SetStartIndex(null); // default value
    getLatestCarparkInfo.Timeout = null; // default value
    var carparkInfo2 = getLatestCarparkInfo.AsDatabaseAggregate.CreateSource(carparkInfo);
    getLatestCarparkInfo.AsDatabaseAggregate.CreateSort().SetAttribute("CarparkInfo.SyncedOn");
    
    
    var ifNode = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(getLatestCarparkInfo, 1371);
    ifNode.SetCondition("GetLatestCarparkInfo.List.Empty or DiffHours(GetLatestCarparkInfo.List.Current.CarparkInfo.SyncedOn, CurrDateTime()) >= Site.CarparkInfoMaxAgeHours");
    ifNode.IsDisabled = false; // default value
    ifNode.Label = "Needs sync?";
    
    var endNode = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>();
    endNode.HorizontalPosition = 5028;
    endNode.IsDisabled = false; // default value
    endNode.VerticalPosition = 4913;
    ifNode.FalseTarget = endNode;
    
    var searchHdbCarparkInfo = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("SearchHdbCarparkInfo").Below(ifNode, 1371);
    var dataGovSG_Datastore = eSpace.Integrations.OfType<ServiceStudio.Plugin.REST.IRestClient>().Named("DataGovSG_Datastore");
    var searchHdbCarparkInfo2 = dataGovSG_Datastore.Actions.OfType<ServiceStudio.Plugin.REST.IRestAction>().Named("SearchHdbCarparkInfo");
    searchHdbCarparkInfo.Action = searchHdbCarparkInfo2;
    searchHdbCarparkInfo.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    searchHdbCarparkInfo.IsDisabled = false; // default value
    searchHdbCarparkInfo.ServerRequestTimeout = null; // default value
    var limit = searchHdbCarparkInfo2.InputParameters.OfType<ServiceStudio.Plugin.REST.IRestActionInput>().Named("Limit");
    searchHdbCarparkInfo.SetArgumentValue(limit, "5000");
    var resourceId = searchHdbCarparkInfo2.InputParameters.OfType<ServiceStudio.Plugin.REST.IRestActionInput>().Named("ResourceId");
    searchHdbCarparkInfo.SetArgumentValue(resourceId, "Site.HdbCarparkInfoResourceId");
    var apiKey = searchHdbCarparkInfo2.InputParameters.OfType<ServiceStudio.Plugin.REST.IRestActionInput>().Named("ApiKey");
    searchHdbCarparkInfo.SetArgumentValue(apiKey, "Site.DataGovApiKey");
    ifNode.TrueTarget = searchHdbCarparkInfo;
    
    var ifNode2 = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(searchHdbCarparkInfo, 1371);
    ifNode2.SetCondition("SearchHdbCarparkInfo.Response.Success and SearchHdbCarparkInfo.Response.Result.Records.Length >= 100");
    ifNode2.IsDisabled = false; // default value
    ifNode2.Label = "Valid response?";
    
    var logMessage = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("LogMessage");
    var _System_ = eSpace.References.Named("(System)");
    var logMessage2 = _System_.ServerActions.Named("LogMessage");
    logMessage.Action = logMessage2;
    logMessage.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    logMessage.HorizontalPosition = 5028;
    logMessage.IsDisabled = false; // default value
    logMessage.ServerRequestTimeout = null; // default value
    logMessage.VerticalPosition = 7655;
    var message = logMessage2.InputParameters.Named("Message");
    logMessage.SetArgumentValue(message, "\"HDB carpark info sync skipped: response not usable (success=\" + SearchHdbCarparkInfo.Response.Success + \", records=\" + SearchHdbCarparkInfo.Response.Result.Records.Length + \"). Keeping the previous cache.\"");
    var moduleName = logMessage2.InputParameters.Named("ModuleName");
    logMessage.SetArgumentValue(moduleName, "\"CarparkIntegration\"");
    ifNode2.FalseTarget = logMessage;
    
    var clearCarparkInfo = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.ISQLNode>("ClearCarparkInfo").Below(ifNode2, 1371);
    clearCarparkInfo.CacheInMinutes = null; // default value
    clearCarparkInfo.Description = null; // default value
    clearCarparkInfo.IsDisabled = false; // default value
    clearCarparkInfo.SetMaxRecords(null); // default value
    clearCarparkInfo.Statement = "DELETE FROM {CarparkInfo}";
    clearCarparkInfo.Timeout = null; // default value
    
    var carparkInfo3 = clearCarparkInfo.CreateOutput(carparkInfo, "CarparkInfo");
    ifNode2.TrueTarget = clearCarparkInfo;
    
    var endNode2 = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(logMessage, 1371);
    endNode2.IsDisabled = false; // default value
    
    var forEachNode = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().ConnectedBelow(clearCarparkInfo, 1371);
    forEachNode.IsDisabled = false; // default value
    forEachNode.Label = null; // default value
    forEachNode.SetMaximumIterations(null); // default value
    forEachNode.SetRecordList("SearchHdbCarparkInfo.Response.Result.Records");
    forEachNode.SetStartIndex(null); // default value
    
    var assignNode = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ToTheRightOf(forEachNode, 3656);
    assignNode.IsDisabled = false; // default value
    assignNode.Label = null; // default value
    
    var assignment = assignNode.CreateAssignment("NewInfo.Id", "NullIdentifier()");
    
    var assignment2 = assignNode.CreateAssignment("NewInfo.CarparkNumber", "ToUpper(Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.CarparkNumber))");
    
    var assignment3 = assignNode.CreateAssignment("NewInfo.Address", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.Address)");
    
    var assignment4 = assignNode.CreateAssignment("NewInfo.CarparkType", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.CarparkType)");
    
    var assignment5 = assignNode.CreateAssignment("NewInfo.ParkingSystem", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.ParkingSystem)");
    
    var assignment6 = assignNode.CreateAssignment("NewInfo.ShortTermParking", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.ShortTermParking)");
    
    var assignment7 = assignNode.CreateAssignment("NewInfo.FreeParking", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.FreeParking)");
    
    var assignment8 = assignNode.CreateAssignment("NewInfo.NightParking", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.NightParking)");
    
    var assignment9 = assignNode.CreateAssignment("NewInfo.Decks", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.Decks)");
    
    var assignment10 = assignNode.CreateAssignment("NewInfo.GantryHeight", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.GantryHeight)");
    
    var assignment11 = assignNode.CreateAssignment("NewInfo.HasBasement", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.HasBasement)");
    
    var assignment12 = assignNode.CreateAssignment("NewInfo.XCoord", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.XCoord)");
    
    var assignment13 = assignNode.CreateAssignment("NewInfo.YCoord", "Trim(SearchHdbCarparkInfo.Response.Result.Records.Current.YCoord)");
    
    var assignment14 = assignNode.CreateAssignment("NewInfo.SyncedOn", "CurrDateTime()");
    forEachNode.CycleTarget = assignNode;
    
    var logMessage22 = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("LogMessage2").Below(forEachNode, 1371);
    logMessage22.Action = logMessage2;
    logMessage22.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    logMessage22.IsDisabled = false; // default value
    logMessage22.ServerRequestTimeout = null; // default value
    logMessage22.SetArgumentValue(moduleName, "\"CarparkIntegration\"");
    logMessage22.SetArgumentValue(message, "\"HDB carpark info synchronised: \" + SearchHdbCarparkInfo.Response.Result.Records.Length + \" carparks.\"");
    forEachNode.Target = logMessage22;
    
    var createCarparkInfo = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("CreateCarparkInfo").ConnectedBelow(assignNode, 1371);
    createCarparkInfo.Action = carparkInfo.CreateAction;
    createCarparkInfo.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    createCarparkInfo.IsDisabled = false; // default value
    createCarparkInfo.ServerRequestTimeout = null; // default value
    createCarparkInfo.Target = forEachNode;
    var createCarparkInfo2 = carparkInfo.CreateAction;
    var source = createCarparkInfo2.InputParameters.Named("Source");
    createCarparkInfo.SetArgumentValue(source, "NewInfo");
    
    var endNode3 = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(logMessage22, 1371);
    endNode3.IsDisabled = false; // default value
    
    var allExceptions = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IExceptionHandlerNode>("AllExceptions");
    allExceptions.AbortTransaction = true; // default value
    allExceptions.Exception = eSpace.AllExceptions;
    allExceptions.HorizontalPosition = 10512;
    allExceptions.IsDisabled = false; // default value
    allExceptions.LogError = true; // default value
    allExceptions.VerticalPosition = 800;
    
    var logMessage3 = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("LogMessage3").ConnectedBelow(allExceptions, 1371);
    logMessage3.Action = logMessage2;
    logMessage3.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    logMessage3.IsDisabled = false; // default value
    logMessage3.ServerRequestTimeout = null; // default value
    logMessage3.SetArgumentValue(message, "\"HDB carpark info sync failed; keeping the previous cache. \" + AllExceptions.ExceptionMessage");
    logMessage3.SetArgumentValue(moduleName, "\"CarparkIntegration\"");
    
    var endNode4 = carparkInfo_EnsureFresh.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(logMessage3, 1371);
    endNode4.IsDisabled = false; // default value
}
