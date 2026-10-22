(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating IServerAction 'Carpark_FetchAndMap' ***/
    var carpark_FetchAndMap = eSpace.CreateServerAction("Carpark_FetchAndMap");
    carpark_FetchAndMap.CacheInMinutes = null; // default value
    carpark_FetchAndMap.Description = "Retrieves the latest availability from data.gov.sg, validates it and maps it to CarparkLotRow. Rules: records without a carpark number or lot data are rejected; data.gov.sg sometimes splits one carpark\'s lot types across two records - these are merged into one carpark (DuplicateCount); each lot type is validated on its own (non-numeric counts are skipped, totals <= 0 or available > total are kept but flagged IsAnomalous with status Unknown); exactly one row per carpark is marked IsPrimary (valid Car lots when present, otherwise its first lot type). Does not rely on server-side ListSort. Raises CarparkDataUnavailable when nothing usable is returned.";
    carpark_FetchAndMap.Folder = null; // default value
    carpark_FetchAndMap.Function = false; // default value
    carpark_FetchAndMap.Icon = null; // default value
    carpark_FetchAndMap.Public = false; // default value
    carpark_FetchAndMap.Shareable = false; // default value
    
    var data = carpark_FetchAndMap.CreateLocalVariable("Data");
    var dataGovSG_Transport = eSpace.Integrations.OfType<ServiceStudio.Plugin.REST.IRestClient>().Named("DataGovSG_Transport");
    var carparkDataItem = dataGovSG_Transport.Structures.OfType<ServiceStudio.Plugin.REST.IRestStructure>().Named("CarparkDataItem");
    data.DataType = eSpace.GetOrCreateListType(carparkDataItem);
    data.Description = null; // default value
    
    var tempRows = carpark_FetchAndMap.CreateLocalVariable("TempRows");
    var carparkLotRow = eSpace.Structures.Named("CarparkLotRow");
    tempRows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    tempRows.Description = null; // default value
    
    var row = carpark_FetchAndMap.CreateLocalVariable("Row");
    row.DataType = carparkLotRow;
    row.Description = null; // default value
    
    var curNumber = carpark_FetchAndMap.CreateLocalVariable("CurNumber");
    curNumber.DataType = eSpace.TextType;
    curNumber.SetDefaultValue(null); // default value
    curNumber.Description = null; // default value
    
    var updatedText = carpark_FetchAndMap.CreateLocalVariable("UpdatedText");
    updatedText.DataType = eSpace.TextType;
    updatedText.SetDefaultValue(null); // default value
    updatedText.Description = null; // default value
    
    var updatedAt = carpark_FetchAndMap.CreateLocalVariable("UpdatedAt");
    updatedAt.DataType = eSpace.DateTimeType;
    updatedAt.SetDefaultValue(null); // default value
    updatedAt.Description = null; // default value
    
    var infoIdx = carpark_FetchAndMap.CreateLocalVariable("InfoIdx");
    infoIdx.DataType = eSpace.IntegerType;
    infoIdx.SetDefaultValue(null); // default value
    infoIdx.Description = null; // default value
    
    var serverDateText = carpark_FetchAndMap.CreateLocalVariable("ServerDateText");
    serverDateText.DataType = eSpace.TextType;
    serverDateText.SetDefaultValue(null); // default value
    serverDateText.Description = null; // default value
    
    var mergedNumbers = carpark_FetchAndMap.CreateLocalVariable("MergedNumbers");
    mergedNumbers.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    mergedNumbers.Description = null; // default value
    
    var primaryIdx = carpark_FetchAndMap.CreateLocalVariable("PrimaryIdx");
    primaryIdx.DataType = eSpace.IntegerType;
    primaryIdx.SetDefaultValue(null); // default value
    primaryIdx.Description = null; // default value
    
    var mergedLotCount = carpark_FetchAndMap.CreateLocalVariable("MergedLotCount");
    mergedLotCount.DataType = eSpace.IntegerType;
    mergedLotCount.SetDefaultValue(null); // default value
    mergedLotCount.Description = null; // default value
    
    var startNode = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode.HorizontalPosition = 3200;
    startNode.IsDisabled = false; // default value
    startNode.VerticalPosition = 800;
    
    var getCarparkAvailability = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("GetCarparkAvailability").ConnectedBelow(startNode, 1371);
    var getCarparkAvailability2 = dataGovSG_Transport.Actions.OfType<ServiceStudio.Plugin.REST.IRestAction>().Named("GetCarparkAvailability");
    getCarparkAvailability.Action = getCarparkAvailability2;
    getCarparkAvailability.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    getCarparkAvailability.IsDisabled = false; // default value
    getCarparkAvailability.ServerRequestTimeout = null; // default value
    var apiKey = getCarparkAvailability2.InputParameters.OfType<ServiceStudio.Plugin.REST.IRestActionInput>().Named("ApiKey");
    getCarparkAvailability.SetArgumentValue(apiKey, "Site.DataGovApiKey");
    
    var ifNode = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(getCarparkAvailability, 1371);
    ifNode.SetCondition("GetCarparkAvailability.Response.Items.Empty");
    ifNode.IsDisabled = false; // default value
    ifNode.Label = "No items?";
    
    var ifNode2 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>();
    ifNode2.SetCondition("not GetCarparkAvailability.Response.Items[0].CarparkData.Empty and TextToDateTimeValidate(Replace(Substr(GetCarparkAvailability.Response.Items[0].Timestamp, 0, 19), \"T\", \" \"))");
    ifNode2.HorizontalPosition = 5028;
    ifNode2.IsDisabled = false; // default value
    ifNode2.Label = "Valid batch?";
    ifNode2.VerticalPosition = 4913;
    ifNode.FalseTarget = ifNode2;
    
    var raiseExceptionNode = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IRaiseExceptionNode>().Below(ifNode, 1371);
    var carparkDataUnavailable = eSpace.UserExceptions.Named("CarparkDataUnavailable");
    raiseExceptionNode.Exception = carparkDataUnavailable;
    raiseExceptionNode.SetExceptionMessage("\"data.gov.sg carpark-availability returned no items.\"");
    raiseExceptionNode.IsDisabled = false; // default value
    ifNode.TrueTarget = raiseExceptionNode;
    
    var raiseExceptionNode2 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IRaiseExceptionNode>();
    raiseExceptionNode2.Exception = carparkDataUnavailable;
    raiseExceptionNode2.SetExceptionMessage("\"data.gov.sg carpark-availability returned an empty or undated batch (timestamp \'\" + GetCarparkAvailability.Response.Items[0].Timestamp + \"\').\"");
    raiseExceptionNode2.HorizontalPosition = 6856;
    raiseExceptionNode2.IsDisabled = false; // default value
    raiseExceptionNode2.VerticalPosition = 6284;
    ifNode2.FalseTarget = raiseExceptionNode2;
    
    var assignNode = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().Below(ifNode2, 1371);
    assignNode.IsDisabled = false; // default value
    assignNode.Label = "Batch metadata";
    
    var assignment = assignNode.CreateAssignment("Data", "GetCarparkAvailability.Response.Items[0].CarparkData");
    
    var assignment2 = assignNode.CreateAssignment("SourceTimestamp", "TextToDateTime(Replace(Substr(GetCarparkAvailability.Response.Items[0].Timestamp, 0, 19), \"T\", \" \"))");
    
    var assignment3 = assignNode.CreateAssignment("ServerDateText", "If(Length(GetCarparkAvailability.ServerDate) >= 25 and Index(\"JanFebMarAprMayJunJulAugSepOctNovDec\", Substr(GetCarparkAvailability.ServerDate, 8, 3)) >= 0, Substr(GetCarparkAvailability.ServerDate, 12, 4) + \"-\" + FormatText(IntegerToText(DecimalToInteger(Index(\"JanFebMarAprMayJunJulAugSepOctNovDec\", Substr(GetCarparkAvailability.ServerDate, 8, 3)) / 3) + 1), 2, 2, True, \"0\") + \"-\" + Substr(GetCarparkAvailability.ServerDate, 5, 2) + \" \" + Substr(GetCarparkAvailability.ServerDate, 17, 8), \"\")");
    
    var assignment4 = assignNode.CreateAssignment("DataAgeSeconds", "If(ServerDateText <> \"\" and TextToDateTimeValidate(ServerDateText), DiffSeconds(SourceTimestamp, AddHours(TextToDateTime(ServerDateText), 8)), -1)");
    ifNode2.TrueTarget = assignNode;
    
    var carparkInfo_EnsureFresh = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("CarparkInfo_EnsureFresh").ConnectedBelow(assignNode, 1371);
    var carparkInfo_EnsureFresh2 = eSpace.ServerActions.Named("CarparkInfo_EnsureFresh");
    carparkInfo_EnsureFresh.Action = carparkInfo_EnsureFresh2;
    carparkInfo_EnsureFresh.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    carparkInfo_EnsureFresh.IsDisabled = false; // default value
    carparkInfo_EnsureFresh.ServerRequestTimeout = null; // default value
    
    var getAllCarparkInfo = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAggregateNode>("GetAllCarparkInfo").ConnectedBelow(carparkInfo_EnsureFresh, 1371);
    getAllCarparkInfo.CacheInMinutes = null; // default value
    getAllCarparkInfo.Description = null; // default value
    getAllCarparkInfo.IsDisabled = false; // default value
    getAllCarparkInfo.SetMaxRecords(null); // default value
    getAllCarparkInfo.SetStartIndex(null); // default value
    getAllCarparkInfo.Timeout = null; // default value
    var carparkInfo2 = eSpace.Entities.OfType<OutSystems.Model.Data.IServerEntity>().Named("CarparkInfo");
    var carparkInfo = getAllCarparkInfo.AsDatabaseAggregate.CreateSource(carparkInfo2);
    
    
    var getLotTypes = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAggregateNode>("GetLotTypes").ConnectedBelow(getAllCarparkInfo, 1371);
    getLotTypes.CacheInMinutes = null; // default value
    getLotTypes.Description = null; // default value
    getLotTypes.IsDisabled = false; // default value
    getLotTypes.SetMaxRecords(null); // default value
    getLotTypes.SetStartIndex(null); // default value
    getLotTypes.Timeout = null; // default value
    var lotType2 = eSpace.Entities.OfType<OutSystems.Model.Data.IStaticEntity>().Named("LotType");
    var lotType = getLotTypes.AsDatabaseAggregate.CreateSource(lotType2);
    
    
    var getStatuses = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAggregateNode>("GetStatuses").ConnectedBelow(getLotTypes, 1371);
    getStatuses.CacheInMinutes = null; // default value
    getStatuses.Description = null; // default value
    getStatuses.IsDisabled = false; // default value
    getStatuses.SetMaxRecords(null); // default value
    getStatuses.SetStartIndex(null); // default value
    getStatuses.Timeout = null; // default value
    var availabilityStatus2 = eSpace.Entities.OfType<OutSystems.Model.Data.IStaticEntity>().Named("AvailabilityStatus");
    var availabilityStatus = getStatuses.AsDatabaseAggregate.CreateSource(availabilityStatus2);
    
    
    var assignNode2 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(getStatuses, 1371);
    assignNode2.IsDisabled = false; // default value
    assignNode2.Label = null; // default value
    
    var assignment5 = assignNode2.CreateAssignment("HasLocationData", "not GetAllCarparkInfo.List.Empty");
    
    var forEachNode = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().ConnectedBelow(assignNode2, 1371);
    forEachNode.IsDisabled = false; // default value
    forEachNode.Label = "Each carpark";
    forEachNode.SetMaximumIterations(null); // default value
    forEachNode.SetRecordList("Data");
    forEachNode.SetStartIndex(null); // default value
    
    var assignNode3 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ToTheRightOf(forEachNode, 1828);
    assignNode3.IsDisabled = false; // default value
    assignNode3.Label = null; // default value
    
    var assignment6 = assignNode3.CreateAssignment("CurNumber", "ToUpper(Trim(Data.Current.CarparkNumber))");
    forEachNode.CycleTarget = assignNode3;
    
    var forEachNode2 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().Below(forEachNode, 1371);
    forEachNode2.IsDisabled = false; // default value
    forEachNode2.Label = "Each merged carpark";
    forEachNode2.SetMaximumIterations(null); // default value
    forEachNode2.SetRecordList("MergedNumbers");
    forEachNode2.SetStartIndex(null); // default value
    forEachNode.Target = forEachNode2;
    
    var ifNode3 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>();
    ifNode3.SetCondition("CurNumber <> \"\" and not Data.Current.CarparkInfo.Empty");
    ifNode3.HorizontalPosition = 8684;
    ifNode3.IsDisabled = false; // default value
    ifNode3.Label = "Usable record?";
    ifNode3.VerticalPosition = 15881;
    assignNode3.Target = ifNode3;
    
    var findMergedCarLots = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("FindMergedCarLots").ToTheRightOf(forEachNode2, 1828);
    var _System_ = eSpace.References.Named("(System)");
    var listIndexOf = _System_.ServerActions.Named("ListIndexOf");
    findMergedCarLots.Action = listIndexOf;
    findMergedCarLots.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    findMergedCarLots.IsDisabled = false; // default value
    findMergedCarLots.ServerRequestTimeout = null; // default value
    var condition = listIndexOf.InputParameters.Named("Condition");
    findMergedCarLots.SetArgumentValue(condition, "Rows.Current.CarparkNumber = MergedNumbers.Current.CarparkNumber and Rows.Current.LotTypeCode = \"C\" and not Rows.Current.IsAnomalous");
    var list = listIndexOf.InputParameters.Named("List");
    findMergedCarLots.SetArgumentValue(list, "Rows");
    forEachNode2.CycleTarget = findMergedCarLots;
    
    var ifNode4 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().Below(forEachNode2, 1371);
    ifNode4.SetCondition("CarparkCount = 0");
    ifNode4.IsDisabled = false; // default value
    ifNode4.Label = "Nothing usable?";
    forEachNode2.Target = ifNode4;
    
    var assignNode4 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>();
    assignNode4.HorizontalPosition = 10512;
    assignNode4.IsDisabled = false; // default value
    assignNode4.Label = null; // default value
    assignNode4.Target = forEachNode;
    assignNode4.VerticalPosition = 17252;
    
    var assignment7 = assignNode4.CreateAssignment("RejectedCount", "RejectedCount + 1");
    ifNode3.FalseTarget = assignNode4;
    
    var findExistingCarpark = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("FindExistingCarpark").Below(ifNode3, 1371);
    findExistingCarpark.Action = listIndexOf;
    findExistingCarpark.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    findExistingCarpark.IsDisabled = false; // default value
    findExistingCarpark.ServerRequestTimeout = null; // default value
    findExistingCarpark.SetArgumentValue(condition, "Rows.Current.CarparkNumber = CurNumber");
    findExistingCarpark.SetArgumentValue(list, "Rows");
    ifNode3.TrueTarget = findExistingCarpark;
    
    var findMergedFirstRow = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("FindMergedFirstRow").ConnectedBelow(findMergedCarLots, 1371);
    findMergedFirstRow.Action = listIndexOf;
    findMergedFirstRow.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    findMergedFirstRow.IsDisabled = false; // default value
    findMergedFirstRow.ServerRequestTimeout = null; // default value
    findMergedFirstRow.SetArgumentValue(condition, "Rows.Current.CarparkNumber = MergedNumbers.Current.CarparkNumber");
    findMergedFirstRow.SetArgumentValue(list, "Rows");
    
    var endNode = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>();
    endNode.HorizontalPosition = 6856;
    endNode.IsDisabled = false; // default value
    endNode.VerticalPosition = 18623;
    ifNode4.FalseTarget = endNode;
    
    var raiseExceptionNode3 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IRaiseExceptionNode>().Below(ifNode4, 1371);
    raiseExceptionNode3.Exception = carparkDataUnavailable;
    raiseExceptionNode3.SetExceptionMessage("\"data.gov.sg carpark-availability: no valid carpark records after validation (rejected \" + RejectedCount + \").\"");
    raiseExceptionNode3.IsDisabled = false; // default value
    ifNode4.TrueTarget = raiseExceptionNode3;
    
    var findInfo = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("FindInfo");
    findInfo.Action = listIndexOf;
    findInfo.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    findInfo.HorizontalPosition = 10512;
    findInfo.IsDisabled = false; // default value
    findInfo.ServerRequestTimeout = null; // default value
    findInfo.VerticalPosition = 18623;
    findInfo.SetArgumentValue(condition, "GetAllCarparkInfo.List.Current.CarparkInfo.CarparkNumber = CurNumber");
    findInfo.SetArgumentValue(list, "GetAllCarparkInfo.List");
    findExistingCarpark.Target = findInfo;
    
    var mergedCarparkRows = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("MergedCarparkRows");
    var listFilter = _System_.ServerActions.Named("ListFilter");
    mergedCarparkRows.Action = listFilter;
    mergedCarparkRows.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    mergedCarparkRows.HorizontalPosition = 8684;
    mergedCarparkRows.IsDisabled = false; // default value
    mergedCarparkRows.ServerRequestTimeout = null; // default value
    mergedCarparkRows.VerticalPosition = 18623;
    var condition2 = listFilter.InputParameters.Named("Condition");
    mergedCarparkRows.SetArgumentValue(condition2, "Rows.Current.CarparkNumber = MergedNumbers.Current.CarparkNumber");
    var sourceList = listFilter.InputParameters.Named("SourceList");
    mergedCarparkRows.SetArgumentValue(sourceList, "Rows");
    findMergedFirstRow.Target = mergedCarparkRows;
    
    var assignNode5 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(findInfo, 1371);
    assignNode5.IsDisabled = false; // default value
    assignNode5.Label = null; // default value
    
    var assignment8 = assignNode5.CreateAssignment("UpdatedText", "Replace(Data.Current.UpdateDatetime, \"T\", \" \")");
    
    var assignment9 = assignNode5.CreateAssignment("UpdatedAt", "If(TextToDateTimeValidate(UpdatedText), TextToDateTime(UpdatedText), NullDate())");
    
    var assignment10 = assignNode5.CreateAssignment("InfoIdx", "FindInfo.Position");
    
    var assignNode6 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(mergedCarparkRows, 1371);
    assignNode6.IsDisabled = false; // default value
    assignNode6.Label = "Primary for merged carpark";
    
    var assignment11 = assignNode6.CreateAssignment("PrimaryIdx", "If(FindMergedCarLots.Position >= 0, FindMergedCarLots.Position, FindMergedFirstRow.Position)");
    
    var assignment12 = assignNode6.CreateAssignment("MergedLotCount", "MergedCarparkRows.FilteredList.Length");
    
    var clearTempRows = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("ClearTempRows").ConnectedBelow(assignNode5, 1371);
    var listClear = _System_.ServerActions.Named("ListClear");
    clearTempRows.Action = listClear;
    clearTempRows.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    clearTempRows.IsDisabled = false; // default value
    clearTempRows.ServerRequestTimeout = null; // default value
    var list2 = listClear.InputParameters.Named("List");
    clearTempRows.SetArgumentValue(list2, "TempRows");
    
    var forEachNode3 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().ConnectedBelow(assignNode6, 1371);
    forEachNode3.IsDisabled = false; // default value
    forEachNode3.Label = "Each row";
    forEachNode3.SetMaximumIterations(null); // default value
    forEachNode3.SetRecordList("Rows");
    forEachNode3.SetStartIndex(null); // default value
    forEachNode3.Target = forEachNode2;
    
    var forEachNode4 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().ConnectedBelow(clearTempRows, 1371);
    forEachNode4.IsDisabled = false; // default value
    forEachNode4.Label = "Each lot type";
    forEachNode4.SetMaximumIterations(null); // default value
    forEachNode4.SetRecordList("Data.Current.CarparkInfo");
    forEachNode4.SetStartIndex(null); // default value
    
    var ifNode5 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ToTheRightOf(forEachNode3, 3656);
    ifNode5.SetCondition("Rows.Current.CarparkNumber = MergedNumbers.Current.CarparkNumber");
    ifNode5.FalseTarget = forEachNode3;
    ifNode5.IsDisabled = false; // default value
    ifNode5.Label = "Row of this carpark?";
    forEachNode3.CycleTarget = ifNode5;
    
    var ifNode6 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ToTheRightOf(forEachNode4, 3656);
    ifNode6.SetCondition("TextToIntegerValidate(Data.Current.CarparkInfo.Current.TotalLots) and TextToIntegerValidate(Data.Current.CarparkInfo.Current.LotsAvailable)");
    ifNode6.IsDisabled = false; // default value
    ifNode6.Label = "Numeric counts?";
    forEachNode4.CycleTarget = ifNode6;
    
    var ifNode7 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().Below(forEachNode4, 1371);
    ifNode7.SetCondition("TempRows.Empty");
    ifNode7.IsDisabled = false; // default value
    ifNode7.Label = "No valid lot types?";
    forEachNode4.Target = ifNode7;
    
    var assignNode7 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().Below(ifNode5, 1371);
    assignNode7.IsDisabled = false; // default value
    assignNode7.Label = null; // default value
    assignNode7.Target = forEachNode3;
    
    var assignment13 = assignNode7.CreateAssignment("Rows.Current.IsPrimary", "Rows.CurrentRowNumber = PrimaryIdx");
    
    var assignment14 = assignNode7.CreateAssignment("Rows.Current.LotTypeCount", "MergedLotCount");
    ifNode5.TrueTarget = assignNode7;
    
    var assignNode8 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>();
    assignNode8.HorizontalPosition = 15996;
    assignNode8.IsDisabled = false; // default value
    assignNode8.Label = null; // default value
    assignNode8.Target = forEachNode4;
    assignNode8.VerticalPosition = 24107;
    
    var assignment15 = assignNode8.CreateAssignment("AnomalyCount", "AnomalyCount + 1");
    ifNode6.FalseTarget = assignNode8;
    
    var findLotType = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("FindLotType").Below(ifNode6, 1371);
    findLotType.Action = listIndexOf;
    findLotType.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    findLotType.IsDisabled = false; // default value
    findLotType.ServerRequestTimeout = null; // default value
    findLotType.SetArgumentValue(condition, "IdentifierToText(GetLotTypes.List.Current.LotType.Id) = ToUpper(Trim(Data.Current.CarparkInfo.Current.LotType))");
    findLotType.SetArgumentValue(list, "GetLotTypes.List");
    ifNode6.TrueTarget = findLotType;
    
    var ifNode8 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>();
    ifNode8.SetCondition("FindExistingCarpark.Position >= 0");
    ifNode8.HorizontalPosition = 12340;
    ifNode8.IsDisabled = false; // default value
    ifNode8.Label = "Continuation of a split record?";
    ifNode8.VerticalPosition = 25478;
    ifNode7.FalseTarget = ifNode8;
    
    var assignNode9 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().Below(ifNode7, 1371);
    assignNode9.IsDisabled = false; // default value
    assignNode9.Label = null; // default value
    assignNode9.Target = forEachNode;
    
    var assignment16 = assignNode9.CreateAssignment("RejectedCount", "RejectedCount + If(FindExistingCarpark.Position >= 0, 0, 1)");
    ifNode7.TrueTarget = assignNode9;
    
    var assignNode10 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(findLotType, 1371);
    assignNode10.IsDisabled = false; // default value
    assignNode10.Label = "Map lot";
    
    var assignment17 = assignNode10.CreateAssignment("Row.CarparkNumber", "CurNumber");
    
    var assignment18 = assignNode10.CreateAssignment("Row.LotTypeCode", "ToUpper(Trim(Data.Current.CarparkInfo.Current.LotType))");
    
    var assignment19 = assignNode10.CreateAssignment("Row.LotTypeLabel", "If(FindLotType.Position >= 0, GetLotTypes.List[FindLotType.Position].LotType.Label, \"Lot type \" + Row.LotTypeCode)");
    
    var assignment20 = assignNode10.CreateAssignment("Row.TotalLots", "TextToInteger(Data.Current.CarparkInfo.Current.TotalLots)");
    
    var assignment21 = assignNode10.CreateAssignment("Row.AvailableLots", "TextToInteger(Data.Current.CarparkInfo.Current.LotsAvailable)");
    
    var assignment22 = assignNode10.CreateAssignment("Row.IsAnomalous", "Row.TotalLots <= 0 or Row.AvailableLots < 0 or Row.AvailableLots > Row.TotalLots");
    
    var assignment23 = assignNode10.CreateAssignment("Row.AvailabilityPct", "If(Row.IsAnomalous, 0.0, Round(Row.AvailableLots * 100.0 / Row.TotalLots, 1))");
    
    var assignment24 = assignNode10.CreateAssignment("Row.StatusId", "AvailabilityStatus_Classify(Row.AvailableLots, Row.TotalLots)");
    
    var assignment25 = assignNode10.CreateAssignment("Row.IsPrimary", "False");
    
    var assignment26 = assignNode10.CreateAssignment("Row.LotTypeCount", "Data.Current.CarparkInfo.Length");
    
    var assignment27 = assignNode10.CreateAssignment("Row.UpdatedAt", "UpdatedAt");
    
    var assignment28 = assignNode10.CreateAssignment("Row.IsReadingOutdated", "UpdatedAt = NullDate() or DiffMinutes(UpdatedAt, SourceTimestamp) > Site.ReadingOutdatedMinutes");
    
    var assignment29 = assignNode10.CreateAssignment("Row.HasLocation", "InfoIdx >= 0");
    
    var assignment30 = assignNode10.CreateAssignment("Row.Address", "If(InfoIdx >= 0, GetAllCarparkInfo.List[InfoIdx].CarparkInfo.Address, \"\")");
    
    var assignment31 = assignNode10.CreateAssignment("Row.CarparkType", "If(InfoIdx >= 0, GetAllCarparkInfo.List[InfoIdx].CarparkInfo.CarparkType, \"\")");
    
    var assignment32 = assignNode10.CreateAssignment("Row.ParkingSystem", "If(InfoIdx >= 0, GetAllCarparkInfo.List[InfoIdx].CarparkInfo.ParkingSystem, \"\")");
    
    var assignment33 = assignNode10.CreateAssignment("Row.ShortTermParking", "If(InfoIdx >= 0, GetAllCarparkInfo.List[InfoIdx].CarparkInfo.ShortTermParking, \"\")");
    
    var assignment34 = assignNode10.CreateAssignment("Row.FreeParking", "If(InfoIdx >= 0, GetAllCarparkInfo.List[InfoIdx].CarparkInfo.FreeParking, \"\")");
    
    var assignment35 = assignNode10.CreateAssignment("Row.NightParking", "If(InfoIdx >= 0, GetAllCarparkInfo.List[InfoIdx].CarparkInfo.NightParking, \"\")");
    
    var assignment36 = assignNode10.CreateAssignment("Row.GantryHeight", "If(InfoIdx >= 0, GetAllCarparkInfo.List[InfoIdx].CarparkInfo.GantryHeight, \"\")");
    
    var assignment37 = assignNode10.CreateAssignment("Row.SearchText", "ToLower(CurNumber + \" \" + Row.Address + \" \" + Row.CarparkType)");
    
    var assignment38 = assignNode10.CreateAssignment("Row.UpdatedAtText", "If(UpdatedAt = NullDate(), \"time unknown\", If(DateTimeToDate(UpdatedAt) = DateTimeToDate(SourceTimestamp), FormatDateTime(UpdatedAt, \"h:mm tt\"), FormatDateTime(UpdatedAt, \"d MMM yyyy, h:mm tt\")))");
    
    var findCarLots = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("FindCarLots");
    findCarLots.Action = listIndexOf;
    findCarLots.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    findCarLots.HorizontalPosition = 14168;
    findCarLots.IsDisabled = false; // default value
    findCarLots.ServerRequestTimeout = null; // default value
    findCarLots.VerticalPosition = 26849;
    findCarLots.SetArgumentValue(condition, "TempRows.Current.LotTypeCode = \"C\" and not TempRows.Current.IsAnomalous");
    findCarLots.SetArgumentValue(list, "TempRows");
    ifNode8.FalseTarget = findCarLots;
    
    var assignNode11 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().Below(ifNode8, 1371);
    assignNode11.IsDisabled = false; // default value
    assignNode11.Label = "Count merge";
    
    var assignment39 = assignNode11.CreateAssignment("DuplicateCount", "DuplicateCount + 1");
    ifNode8.TrueTarget = assignNode11;
    
    var findStatus = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("FindStatus");
    findStatus.Action = listIndexOf;
    findStatus.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    findStatus.HorizontalPosition = 15996;
    findStatus.IsDisabled = false; // default value
    findStatus.ServerRequestTimeout = null; // default value
    findStatus.VerticalPosition = 26849;
    findStatus.SetArgumentValue(condition, "GetStatuses.List.Current.AvailabilityStatus.Id = Row.StatusId");
    findStatus.SetArgumentValue(list, "GetStatuses.List");
    assignNode10.Target = findStatus;
    
    var assignNode12 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(findCarLots, 1371);
    assignNode12.IsDisabled = false; // default value
    assignNode12.Label = "Mark primary lot";
    
    var assignment40 = assignNode12.CreateAssignment("TempRows[If(FindCarLots.Position >= 0, FindCarLots.Position, 0)].IsPrimary", "True");
    
    var assignment41 = assignNode12.CreateAssignment("CarparkCount", "CarparkCount + 1");
    
    var appendSplitLots = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("AppendSplitLots").ConnectedBelow(assignNode11, 1371);
    var listAppendAll = _System_.ServerActions.Named("ListAppendAll");
    appendSplitLots.Action = listAppendAll;
    appendSplitLots.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    appendSplitLots.IsDisabled = false; // default value
    appendSplitLots.ServerRequestTimeout = null; // default value
    var sourceList2 = listAppendAll.InputParameters.Named("SourceList");
    appendSplitLots.SetArgumentValue(sourceList2, "TempRows");
    var list3 = listAppendAll.InputParameters.Named("List");
    appendSplitLots.SetArgumentValue(list3, "Rows");
    
    var assignNode13 = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(findStatus, 1371);
    assignNode13.IsDisabled = false; // default value
    assignNode13.Label = null; // default value
    
    var assignment42 = assignNode13.CreateAssignment("Row.StatusLabel", "If(FindStatus.Position >= 0, GetStatuses.List[FindStatus.Position].AvailabilityStatus.Label, \"Data unavailable\")");
    
    var assignment43 = assignNode13.CreateAssignment("AnomalyCount", "AnomalyCount + If(Row.IsAnomalous, 1, 0)");
    
    var appendCarpark = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("AppendCarpark").ConnectedBelow(assignNode12, 1371);
    appendCarpark.Action = listAppendAll;
    appendCarpark.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    appendCarpark.IsDisabled = false; // default value
    appendCarpark.ServerRequestTimeout = null; // default value
    appendCarpark.Target = forEachNode;
    appendCarpark.SetArgumentValue(list3, "Rows");
    appendCarpark.SetArgumentValue(sourceList2, "TempRows");
    
    var rememberMergedCarpark = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("RememberMergedCarpark").ConnectedBelow(appendSplitLots, 1371);
    var listAppend = _System_.ServerActions.Named("ListAppend");
    rememberMergedCarpark.Action = listAppend;
    rememberMergedCarpark.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    rememberMergedCarpark.IsDisabled = false; // default value
    rememberMergedCarpark.ServerRequestTimeout = null; // default value
    rememberMergedCarpark.Target = forEachNode;
    var list4 = listAppend.InputParameters.Named("List");
    rememberMergedCarpark.SetArgumentValue(list4, "MergedNumbers");
    var element = listAppend.InputParameters.Named("Element");
    rememberMergedCarpark.SetArgumentValue(element, "TempRows[0]");
    
    var appendLot = carpark_FetchAndMap.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("AppendLot").ConnectedBelow(assignNode13, 1371);
    appendLot.Action = listAppend;
    appendLot.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    appendLot.IsDisabled = false; // default value
    appendLot.ServerRequestTimeout = null; // default value
    appendLot.Target = forEachNode4;
    appendLot.SetArgumentValue(list4, "TempRows");
    appendLot.SetArgumentValue(element, "Row");
    
    var rows = carpark_FetchAndMap.CreateOutputParameter("Rows");
    rows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    rows.Description = null; // default value
    
    var sourceTimestamp = carpark_FetchAndMap.CreateOutputParameter("SourceTimestamp");
    sourceTimestamp.DataType = eSpace.DateTimeType;
    sourceTimestamp.SetDefaultValue(null); // default value
    sourceTimestamp.Description = null; // default value
    
    var dataAgeSeconds = carpark_FetchAndMap.CreateOutputParameter("DataAgeSeconds");
    dataAgeSeconds.DataType = eSpace.IntegerType;
    dataAgeSeconds.SetDefaultValue(null); // default value
    dataAgeSeconds.Description = null; // default value
    
    var carparkCount = carpark_FetchAndMap.CreateOutputParameter("CarparkCount");
    carparkCount.DataType = eSpace.IntegerType;
    carparkCount.SetDefaultValue(null); // default value
    carparkCount.Description = null; // default value
    
    var rejectedCount = carpark_FetchAndMap.CreateOutputParameter("RejectedCount");
    rejectedCount.DataType = eSpace.IntegerType;
    rejectedCount.SetDefaultValue(null); // default value
    rejectedCount.Description = null; // default value
    
    var duplicateCount = carpark_FetchAndMap.CreateOutputParameter("DuplicateCount");
    duplicateCount.DataType = eSpace.IntegerType;
    duplicateCount.SetDefaultValue(null); // default value
    duplicateCount.Description = null; // default value
    
    var anomalyCount = carpark_FetchAndMap.CreateOutputParameter("AnomalyCount");
    anomalyCount.DataType = eSpace.IntegerType;
    anomalyCount.SetDefaultValue(null); // default value
    anomalyCount.Description = null; // default value
    
    var hasLocationData = carpark_FetchAndMap.CreateOutputParameter("HasLocationData");
    hasLocationData.DataType = eSpace.BooleanType;
    hasLocationData.SetDefaultValue(null); // default value
    hasLocationData.Description = null; // default value
}
