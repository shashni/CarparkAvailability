(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating IServerAction 'Carpark_GetAvailability' ***/
    var carpark_GetAvailability = eSpace.CreateServerAction("Carpark_GetAvailability");
    carpark_GetAvailability.CacheInMinutes = null; // default value
    carpark_GetAvailability.Description = "Single entry point for the dashboard. Reuses the last good retrieval when it is younger than Site.CacheTtlSeconds (Site.MinRefreshIntervalSeconds when the user forces a refresh); otherwise fetches, validates and stores a new snapshot. If data.gov.sg fails, the last good snapshot is returned labelled Cached, or Unavailable when none exists. Technical errors are logged under module CarparkIntegration and never returned to the user.";
    carpark_GetAvailability.Folder = null; // default value
    carpark_GetAvailability.Function = false; // default value
    carpark_GetAvailability.Icon = null; // default value
    carpark_GetAvailability.Public = false; // default value
    carpark_GetAvailability.Shareable = false; // default value
    
    var forceRefresh = carpark_GetAvailability.CreateInputParameter("ForceRefresh");
    forceRefresh.DataType = eSpace.BooleanType;
    forceRefresh.SetDefaultValue(null); // default value
    forceRefresh.Description = null; // default value
    forceRefresh.IsMandatory = false;
    
    var newSnapshot = carpark_GetAvailability.CreateLocalVariable("NewSnapshot");
    var availabilitySnapshot = eSpace.Entities.OfType<OutSystems.Model.Data.IServerEntity>().Named("AvailabilitySnapshot");
    newSnapshot.DataType = availabilitySnapshot;
    newSnapshot.Description = null; // default value
    
    var lastSnapshot = carpark_GetAvailability.CreateLocalVariable("LastSnapshot");
    lastSnapshot.DataType = availabilitySnapshot;
    lastSnapshot.Description = null; // default value
    
    var hasSnapshot = carpark_GetAvailability.CreateLocalVariable("HasSnapshot");
    hasSnapshot.DataType = eSpace.BooleanType;
    hasSnapshot.SetDefaultValue(null); // default value
    hasSnapshot.Description = null; // default value
    
    var snapshotAgeSeconds = carpark_GetAvailability.CreateLocalVariable("SnapshotAgeSeconds");
    snapshotAgeSeconds.DataType = eSpace.IntegerType;
    snapshotAgeSeconds.SetDefaultValue(null); // default value
    snapshotAgeSeconds.Description = null; // default value
    
    var startNode = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode.HorizontalPosition = 3200;
    startNode.IsDisabled = false; // default value
    startNode.VerticalPosition = 800;
    
    var getLatestSnapshot = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IAggregateNode>("GetLatestSnapshot").ConnectedBelow(startNode, 1371);
    getLatestSnapshot.CacheInMinutes = null; // default value
    getLatestSnapshot.Description = null; // default value
    getLatestSnapshot.IsDisabled = false; // default value
    getLatestSnapshot.SetMaxRecords("1");
    getLatestSnapshot.SetStartIndex(null); // default value
    getLatestSnapshot.Timeout = null; // default value
    var availabilitySnapshot2 = getLatestSnapshot.AsDatabaseAggregate.CreateSource(availabilitySnapshot);
    getLatestSnapshot.AsDatabaseAggregate.CreateSort().SetAttribute("AvailabilitySnapshot.Id");
    
    
    var assignNode = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(getLatestSnapshot, 1371);
    assignNode.IsDisabled = false; // default value
    assignNode.Label = null; // default value
    
    var assignment = assignNode.CreateAssignment("HasSnapshot", "not GetLatestSnapshot.List.Empty");
    
    var assignment2 = assignNode.CreateAssignment("LastSnapshot", "GetLatestSnapshot.List.Current.AvailabilitySnapshot");
    
    var assignment3 = assignNode.CreateAssignment("SnapshotAgeSeconds", "If(HasSnapshot, DiffSeconds(LastSnapshot.RetrievedOn, CurrDateTime()), 999999)");
    
    var ifNode = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(assignNode, 1371);
    ifNode.SetCondition("HasSnapshot and SnapshotAgeSeconds >= 0 and SnapshotAgeSeconds < If(ForceRefresh, Site.MinRefreshIntervalSeconds, Site.CacheTtlSeconds)");
    ifNode.IsDisabled = false; // default value
    ifNode.Label = "Reuse recent retrieval?";
    
    var fetch = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("Fetch");
    var carpark_FetchAndMap = eSpace.ServerActions.Named("Carpark_FetchAndMap");
    fetch.Action = carpark_FetchAndMap;
    fetch.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    fetch.HorizontalPosition = 5028;
    fetch.IsDisabled = false; // default value
    fetch.ServerRequestTimeout = null; // default value
    fetch.VerticalPosition = 6284;
    ifNode.FalseTarget = fetch;
    
    var serveRecent = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("ServeRecent").Below(ifNode, 1371);
    var snapshot_ToResult = eSpace.ServerActions.Named("Snapshot_ToResult");
    serveRecent.Action = snapshot_ToResult;
    serveRecent.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    serveRecent.IsDisabled = false; // default value
    serveRecent.ServerRequestTimeout = null; // default value
    var snapshot = snapshot_ToResult.InputParameters.Named("Snapshot");
    serveRecent.SetArgumentValue(snapshot, "LastSnapshot");
    var message = snapshot_ToResult.InputParameters.Named("Message");
    serveRecent.SetArgumentValue(message, "\"\"");
    var freshness = snapshot_ToResult.InputParameters.Named("Freshness");
    serveRecent.SetArgumentValue(freshness, "\"Live\"");
    ifNode.TrueTarget = serveRecent;
    
    var serializeRows = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IJSONSerializeNode>("SerializeRows").ConnectedBelow(fetch, 1371);
    serializeRows.SetData("Fetch.Rows");
    serializeRows.DateFormat = OutSystems.Model.Enumerations.JSONDateFormat.ISO; // default value
    serializeRows.Description = null; // default value
    serializeRows.IsDisabled = false; // default value
    serializeRows.SerializeDefaultValues = false; // default value
    
    var assignNode2 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(serveRecent, 1371);
    assignNode2.IsDisabled = false; // default value
    assignNode2.Label = null; // default value
    
    var assignment4 = assignNode2.CreateAssignment("Rows", "ServeRecent.Rows");
    
    var assignment5 = assignNode2.CreateAssignment("Meta", "ServeRecent.Meta");
    
    var assignNode3 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(serializeRows, 1371);
    assignNode3.IsDisabled = false; // default value
    assignNode3.Label = "Build snapshot";
    
    var assignment6 = assignNode3.CreateAssignment("NewSnapshot.Id", "If(HasSnapshot, LastSnapshot.Id, NullIdentifier())");
    
    var assignment7 = assignNode3.CreateAssignment("NewSnapshot.SourceTimestamp", "Fetch.SourceTimestamp");
    
    var assignment8 = assignNode3.CreateAssignment("NewSnapshot.RetrievedOn", "CurrDateTime()");
    
    var assignment9 = assignNode3.CreateAssignment("NewSnapshot.DataAgeSecondsAtRetrieval", "Fetch.DataAgeSeconds");
    
    var assignment10 = assignNode3.CreateAssignment("NewSnapshot.CarparkCount", "Fetch.CarparkCount");
    
    var assignment11 = assignNode3.CreateAssignment("NewSnapshot.RejectedCount", "Fetch.RejectedCount");
    
    var assignment12 = assignNode3.CreateAssignment("NewSnapshot.DuplicateCount", "Fetch.DuplicateCount");
    
    var assignment13 = assignNode3.CreateAssignment("NewSnapshot.AnomalyCount", "Fetch.AnomalyCount");
    
    var assignment14 = assignNode3.CreateAssignment("NewSnapshot.HasLocationData", "Fetch.HasLocationData");
    
    var assignment15 = assignNode3.CreateAssignment("NewSnapshot.RowsJson", "SerializeRows.JSON");
    
    var endNode = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode2, 1371);
    endNode.IsDisabled = false; // default value
    
    var createOrUpdateAvailabilitySnapshot = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("CreateOrUpdateAvailabilitySnapshot").ConnectedBelow(assignNode3, 1371);
    createOrUpdateAvailabilitySnapshot.Action = availabilitySnapshot.CreateOrUpdateAction;
    createOrUpdateAvailabilitySnapshot.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    createOrUpdateAvailabilitySnapshot.IsDisabled = false; // default value
    createOrUpdateAvailabilitySnapshot.ServerRequestTimeout = null; // default value
    var createOrUpdateAvailabilitySnapshot2 = availabilitySnapshot.CreateOrUpdateAction;
    var source = createOrUpdateAvailabilitySnapshot2.InputParameters.Named("Source");
    createOrUpdateAvailabilitySnapshot.SetArgumentValue(source, "NewSnapshot");
    
    var assignNode4 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(createOrUpdateAvailabilitySnapshot, 1371);
    assignNode4.IsDisabled = false; // default value
    assignNode4.Label = "Live result";
    
    var assignment16 = assignNode4.CreateAssignment("Rows", "Fetch.Rows");
    
    var assignment17 = assignNode4.CreateAssignment("Meta.Freshness", "\"Live\"");
    
    var assignment18 = assignNode4.CreateAssignment("Meta.HasData", "True");
    
    var assignment19 = assignNode4.CreateAssignment("Meta.SourceTimestamp", "Fetch.SourceTimestamp");
    
    var assignment20 = assignNode4.CreateAssignment("Meta.DataAgeSeconds", "Fetch.DataAgeSeconds");
    
    var assignment21 = assignNode4.CreateAssignment("Meta.StaleAfterMinutes", "Site.StaleAfterMinutes");
    
    var assignment22 = assignNode4.CreateAssignment("Meta.CarparkCount", "Fetch.CarparkCount");
    
    var assignment23 = assignNode4.CreateAssignment("Meta.RejectedCount", "Fetch.RejectedCount");
    
    var assignment24 = assignNode4.CreateAssignment("Meta.DuplicateCount", "Fetch.DuplicateCount");
    
    var assignment25 = assignNode4.CreateAssignment("Meta.AnomalyCount", "Fetch.AnomalyCount");
    
    var assignment26 = assignNode4.CreateAssignment("Meta.IsPartial", "Fetch.RejectedCount > 0");
    
    var assignment27 = assignNode4.CreateAssignment("Meta.HasLocationData", "Fetch.HasLocationData");
    
    var assignment28 = assignNode4.CreateAssignment("Meta.Message", "\"\"");
    
    var assignment29 = assignNode4.CreateAssignment("Meta.SourceTimestampText", "FormatDateTime(Fetch.SourceTimestamp, \"d MMM yyyy, h:mm tt\")");
    
    var assignment30 = assignNode4.CreateAssignment("Meta.ThresholdSummary", "\"High: more than \" + FormatDecimal(Site.HighAvailabilityMinPct, 0, \".\", \"\") + \"% of lots available · Moderate: \" + FormatDecimal(Site.LowAvailabilityMaxPct, 0, \".\", \"\") + \"–\" + FormatDecimal(Site.HighAvailabilityMinPct, 0, \".\", \"\") + \"% · Low: under \" + FormatDecimal(Site.LowAvailabilityMaxPct, 0, \".\", \"\") + \"% · Full: no lots available\"");
    
    var logMessage = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("LogMessage").ConnectedBelow(assignNode4, 1371);
    var _System_ = eSpace.References.Named("(System)");
    var logMessage2 = _System_.ServerActions.Named("LogMessage");
    logMessage.Action = logMessage2;
    logMessage.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    logMessage.IsDisabled = false; // default value
    logMessage.ServerRequestTimeout = null; // default value
    var moduleName = logMessage2.InputParameters.Named("ModuleName");
    logMessage.SetArgumentValue(moduleName, "\"CarparkIntegration\"");
    var message2 = logMessage2.InputParameters.Named("Message");
    logMessage.SetArgumentValue(message2, "\"Carpark availability refreshed: \" + Fetch.CarparkCount + \" carparks, \" + Fetch.RejectedCount + \" rejected, \" + Fetch.DuplicateCount + \" superseded duplicates, \" + Fetch.AnomalyCount + \" inconsistent lot readings, data age \" + Fetch.DataAgeSeconds + \"s.\"");
    
    var endNode2 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(logMessage, 1371);
    endNode2.IsDisabled = false; // default value
    
    var allExceptions = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IExceptionHandlerNode>("AllExceptions");
    allExceptions.AbortTransaction = false;
    allExceptions.Exception = eSpace.AllExceptions;
    allExceptions.HorizontalPosition = 8684;
    allExceptions.IsDisabled = false; // default value
    allExceptions.LogError = true; // default value
    allExceptions.VerticalPosition = 800;
    
    var logMessage22 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("LogMessage2").ConnectedBelow(allExceptions, 1371);
    logMessage22.Action = logMessage2;
    logMessage22.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    logMessage22.IsDisabled = false; // default value
    logMessage22.ServerRequestTimeout = null; // default value
    logMessage22.SetArgumentValue(moduleName, "\"CarparkIntegration\"");
    logMessage22.SetArgumentValue(message2, "\"Carpark availability refresh failed: \" + AllExceptions.ExceptionMessage");
    
    var ifNode2 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(logMessage22, 1371);
    ifNode2.SetCondition("HasSnapshot");
    ifNode2.IsDisabled = false; // default value
    ifNode2.Label = "Have last good data?";
    
    var assignNode5 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>();
    assignNode5.HorizontalPosition = 10512;
    assignNode5.IsDisabled = false; // default value
    assignNode5.Label = null; // default value
    assignNode5.VerticalPosition = 4913;
    
    var assignment31 = assignNode5.CreateAssignment("Meta.Freshness", "\"Unavailable\"");
    
    var assignment32 = assignNode5.CreateAssignment("Meta.HasData", "False");
    
    var assignment33 = assignNode5.CreateAssignment("Meta.StaleAfterMinutes", "Site.StaleAfterMinutes");
    
    var assignment34 = assignNode5.CreateAssignment("Meta.Message", "\"Carpark availability is temporarily unavailable. Please try again in a few minutes.\"");
    
    var assignment35 = assignNode5.CreateAssignment("Meta.ThresholdSummary", "\"High: more than \" + FormatDecimal(Site.HighAvailabilityMinPct, 0, \".\", \"\") + \"% of lots available · Moderate: \" + FormatDecimal(Site.LowAvailabilityMaxPct, 0, \".\", \"\") + \"–\" + FormatDecimal(Site.HighAvailabilityMinPct, 0, \".\", \"\") + \"% · Low: under \" + FormatDecimal(Site.LowAvailabilityMaxPct, 0, \".\", \"\") + \"% · Full: no lots available\"");
    ifNode2.FalseTarget = assignNode5;
    
    var serveCached = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("ServeCached").Below(ifNode2, 1371);
    serveCached.Action = snapshot_ToResult;
    serveCached.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    serveCached.IsDisabled = false; // default value
    serveCached.ServerRequestTimeout = null; // default value
    serveCached.SetArgumentValue(freshness, "\"Cached\"");
    serveCached.SetArgumentValue(snapshot, "LastSnapshot");
    serveCached.SetArgumentValue(message, "\"We couldn\'t refresh from data.gov.sg just now. You are seeing the last data we received.\"");
    ifNode2.TrueTarget = serveCached;
    
    var endNode3 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode5, 1371);
    endNode3.IsDisabled = false; // default value
    
    var assignNode6 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(serveCached, 1371);
    assignNode6.IsDisabled = false; // default value
    assignNode6.Label = null; // default value
    
    var assignment36 = assignNode6.CreateAssignment("Rows", "ServeCached.Rows");
    
    var assignment37 = assignNode6.CreateAssignment("Meta", "ServeCached.Meta");
    
    var endNode4 = carpark_GetAvailability.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode6, 1371);
    endNode4.IsDisabled = false; // default value
    
    var rows = carpark_GetAvailability.CreateOutputParameter("Rows");
    var carparkLotRow = eSpace.Structures.Named("CarparkLotRow");
    rows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    rows.Description = null; // default value
    
    var meta = carpark_GetAvailability.CreateOutputParameter("Meta");
    var availabilityMeta = eSpace.Structures.Named("AvailabilityMeta");
    meta.DataType = availabilityMeta;
    meta.Description = null; // default value
}
