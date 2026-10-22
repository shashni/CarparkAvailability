(OutSystems.Model.UI.Mobile.IMobileFlow mainFlow) => {
    
    /*** creating IMobileScreen 'Dashboard' ***/
    var dashboard = mainFlow.CreateScreen("Dashboard");
    /*** Deleting Widgets that are created by default when instantiating an IMobileScreen ***/
    dashboard.Widgets.ToList().ForEach(x => x.Delete());
    dashboard.GetESpace().Roles.ToList().ForEach(x => x.Delete());
    var eSpace = mainFlow.GetESpace();
    var anonymous = eSpace.SystemRoles.OfType<OutSystems.Model.Logic.ISystemRole>().Named("Anonymous");
    dashboard.Roles.Add(anonymous);
    var registered = eSpace.SystemRoles.OfType<OutSystems.Model.Logic.ISystemRole>().Named("Registered");
    dashboard.Roles.Add(registered);
    dashboard.AnonymousAccess = false; // default value
    dashboard.CustomURL = false; // default value
    dashboard.Description = "Singapore Carpark Availability dashboard: live availability from data.gov.sg with search, filters, sorting, freshness labelling and a per-carpark detail view.";
    dashboard.HorizontalPosition = 800;
    dashboard.Icon = null; // default value
    dashboard.IsDisabled = false; // default value
    dashboard.PageName = null; // default value
    dashboard.Public = false; // default value
    dashboard.Shareable = false; // default value
    dashboard.StyleSheet = null; // default value
    dashboard.SetTitle("\"Singapore Carpark Availability\"");
    dashboard.URLStructure = OutSystems.Model.Enumerations.URLStructure.QueryString; // default value
    dashboard.VerticalPosition = 700;
    
    var getLotTypeOptions = dashboard.CreateScreenAggregate(false, "GetLotTypeOptions");
    getLotTypeOptions.CacheInMinutes = null; // default value
    getLotTypeOptions.Description = null; // default value
    getLotTypeOptions.Fetch = OutSystems.Model.Enumerations.DataSourceFetch.AtStart; // default value
    getLotTypeOptions.SetMaxRecords("50"); // default value
    getLotTypeOptions.SetStartIndex(null); // default value
    getLotTypeOptions.Timeout = null; // default value
    var lotType2 = eSpace.Entities.OfType<OutSystems.Model.Data.IStaticEntity>().Named("LotType");
    var lotType = getLotTypeOptions.AsDatabaseAggregate.CreateSource(lotType2);
    getLotTypeOptions.AsDatabaseAggregate.CreateSort().SetAttribute("LotType.Order");
    
    
    var getSortOptions = dashboard.CreateScreenAggregate(false, "GetSortOptions");
    getSortOptions.CacheInMinutes = null; // default value
    getSortOptions.Description = null; // default value
    getSortOptions.Fetch = OutSystems.Model.Enumerations.DataSourceFetch.AtStart; // default value
    getSortOptions.SetMaxRecords("50"); // default value
    getSortOptions.SetStartIndex(null); // default value
    getSortOptions.Timeout = null; // default value
    var sortOption2 = eSpace.Entities.OfType<OutSystems.Model.Data.IStaticEntity>().Named("SortOption");
    var sortOption = getSortOptions.AsDatabaseAggregate.CreateSource(sortOption2);
    getSortOptions.AsDatabaseAggregate.CreateSort().SetAttribute("SortOption.Order");
    
    
    var getStatusOptions = dashboard.CreateScreenAggregate(false, "GetStatusOptions");
    getStatusOptions.CacheInMinutes = null; // default value
    getStatusOptions.Description = null; // default value
    getStatusOptions.Fetch = OutSystems.Model.Enumerations.DataSourceFetch.AtStart; // default value
    getStatusOptions.SetMaxRecords("50"); // default value
    getStatusOptions.SetStartIndex(null); // default value
    getStatusOptions.Timeout = null; // default value
    var availabilityStatus2 = eSpace.Entities.OfType<OutSystems.Model.Data.IStaticEntity>().Named("AvailabilityStatus");
    var availabilityStatus = getStatusOptions.AsDatabaseAggregate.CreateSource(availabilityStatus2);
    getStatusOptions.AsDatabaseAggregate.CreateSort().SetAttribute("AvailabilityStatus.Order");
    
    
    var allRows = dashboard.CreateLocalVariable("AllRows");
    var carparkLotRow = eSpace.Structures.Named("CarparkLotRow");
    allRows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    allRows.Description = null; // default value
    
    var filteredRows = dashboard.CreateLocalVariable("FilteredRows");
    filteredRows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    filteredRows.Description = null; // default value
    
    var pageRows = dashboard.CreateLocalVariable("PageRows");
    pageRows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    pageRows.Description = null; // default value
    
    var detailRows = dashboard.CreateLocalVariable("DetailRows");
    detailRows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    detailRows.Description = null; // default value
    
    var meta = dashboard.CreateLocalVariable("Meta");
    var availabilityMeta = eSpace.Structures.Named("AvailabilityMeta");
    meta.DataType = availabilityMeta;
    meta.Description = null; // default value
    
    var isLoading = dashboard.CreateLocalVariable("IsLoading");
    isLoading.DataType = eSpace.BooleanType;
    isLoading.SetDefaultValue("True");
    isLoading.Description = null; // default value
    
    var isRefreshing = dashboard.CreateLocalVariable("IsRefreshing");
    isRefreshing.DataType = eSpace.BooleanType;
    isRefreshing.SetDefaultValue(null); // default value
    isRefreshing.Description = null; // default value
    
    var hasLoadError = dashboard.CreateLocalVariable("HasLoadError");
    hasLoadError.DataType = eSpace.BooleanType;
    hasLoadError.SetDefaultValue(null); // default value
    hasLoadError.Description = null; // default value
    
    var loadedAt = dashboard.CreateLocalVariable("LoadedAt");
    loadedAt.DataType = eSpace.DateTimeType;
    loadedAt.SetDefaultValue(null); // default value
    loadedAt.Description = null; // default value
    
    var freshness = dashboard.CreateLocalVariable("Freshness");
    freshness.DataType = eSpace.TextType;
    freshness.SetDefaultValue("\"Loading\"");
    freshness.Description = null; // default value
    
    var ageMinutes = dashboard.CreateLocalVariable("AgeMinutes");
    ageMinutes.DataType = eSpace.IntegerType;
    ageMinutes.SetDefaultValue(null); // default value
    ageMinutes.Description = null; // default value
    
    var searchText = dashboard.CreateLocalVariable("SearchText");
    searchText.DataType = eSpace.TextType;
    searchText.SetDefaultValue(null); // default value
    searchText.Description = null; // default value
    
    var statusFilter = dashboard.CreateLocalVariable("StatusFilter");
    statusFilter.DataType = availabilityStatus2.IdentifierType;
    statusFilter.SetDefaultValue(null); // default value
    statusFilter.Description = null; // default value
    
    var lotTypeFilter = dashboard.CreateLocalVariable("LotTypeFilter");
    lotTypeFilter.DataType = lotType2.IdentifierType;
    lotTypeFilter.SetDefaultValue(null); // default value
    lotTypeFilter.Description = null; // default value
    
    var minAvailable = dashboard.CreateLocalVariable("MinAvailable");
    minAvailable.DataType = eSpace.IntegerType;
    minAvailable.SetDefaultValue(null); // default value
    minAvailable.Description = null; // default value
    
    var sortBy = dashboard.CreateLocalVariable("SortBy");
    sortBy.DataType = sortOption2.IdentifierType;
    sortBy.SetDefaultValue(null); // default value
    sortBy.Description = null; // default value
    
    var pageSize = dashboard.CreateLocalVariable("PageSize");
    pageSize.DataType = eSpace.IntegerType;
    pageSize.SetDefaultValue("48");
    pageSize.Description = null; // default value
    
    var visibleCount = dashboard.CreateLocalVariable("VisibleCount");
    visibleCount.DataType = eSpace.IntegerType;
    visibleCount.SetDefaultValue("48");
    visibleCount.Description = null; // default value
    
    var sumCarparks = dashboard.CreateLocalVariable("SumCarparks");
    sumCarparks.DataType = eSpace.IntegerType;
    sumCarparks.SetDefaultValue(null); // default value
    sumCarparks.Description = null; // default value
    
    var sumTotalLots = dashboard.CreateLocalVariable("SumTotalLots");
    sumTotalLots.DataType = eSpace.IntegerType;
    sumTotalLots.SetDefaultValue(null); // default value
    sumTotalLots.Description = null; // default value
    
    var sumAvailableLots = dashboard.CreateLocalVariable("SumAvailableLots");
    sumAvailableLots.DataType = eSpace.IntegerType;
    sumAvailableLots.SetDefaultValue(null); // default value
    sumAvailableLots.Description = null; // default value
    
    var sumFullOrLow = dashboard.CreateLocalVariable("SumFullOrLow");
    sumFullOrLow.DataType = eSpace.IntegerType;
    sumFullOrLow.SetDefaultValue(null); // default value
    sumFullOrLow.Description = null; // default value
    
    var sumValidForAvg = dashboard.CreateLocalVariable("SumValidForAvg");
    sumValidForAvg.DataType = eSpace.IntegerType;
    sumValidForAvg.SetDefaultValue(null); // default value
    sumValidForAvg.Description = null; // default value
    
    var sumPctTotal = dashboard.CreateLocalVariable("SumPctTotal");
    sumPctTotal.DataType = eSpace.DecimalType;
    sumPctTotal.SetDefaultValue(null); // default value
    sumPctTotal.Description = null; // default value
    
    var avgPct = dashboard.CreateLocalVariable("AvgPct");
    avgPct.DataType = eSpace.DecimalType;
    avgPct.SetDefaultValue(null); // default value
    avgPct.Description = null; // default value
    
    var showDetail = dashboard.CreateLocalVariable("ShowDetail");
    showDetail.DataType = eSpace.BooleanType;
    showDetail.SetDefaultValue(null); // default value
    showDetail.Description = null; // default value
    
    var selectedCarpark = dashboard.CreateLocalVariable("SelectedCarpark");
    selectedCarpark.DataType = eSpace.TextType;
    selectedCarpark.SetDefaultValue(null); // default value
    selectedCarpark.Description = null; // default value
    
    var liveMessage = dashboard.CreateLocalVariable("LiveMessage");
    liveMessage.DataType = eSpace.TextType;
    liveMessage.SetDefaultValue(null); // default value
    liveMessage.Description = null; // default value
    dashboard.OnRender.Destination = null; // default value
    
    var applyFilters = dashboard.CreateScreenAction("ApplyFilters");
    applyFilters.Description = "Single pass over AllRows: the lot-type view (each carpark\'s primary lot, or the chosen lot type) feeds the summary metrics; search, status and minimum-available filters then select the result rows, which are sorted (SortRows) and paged. Inconsistent readings are excluded from totals and averages.";
    
    var query = applyFilters.CreateLocalVariable("Query");
    query.DataType = eSpace.TextType;
    query.SetDefaultValue(null); // default value
    query.Description = null; // default value
    
    var onDestroy = dashboard.CreateScreenAction("OnDestroy");
    onDestroy.Description = null; // default value
    
    var startNode = onDestroy.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode.HorizontalPosition = 3200;
    startNode.IsDisabled = false; // default value
    startNode.VerticalPosition = 800;
    
    var stopFreshnessTimer = onDestroy.CreateNode<OutSystems.Model.Logic.Nodes.IJavaScriptNode>("StopFreshnessTimer").ConnectedBelow(startNode, 1371);
    stopFreshnessTimer.Description = null; // default value
    stopFreshnessTimer.IsDisabled = false; // default value
    stopFreshnessTimer.JavaScript = "if (window.cpFreshnessTimer) { clearInterval(window.cpFreshnessTimer); window.cpFreshnessTimer = null; }";
    
    var endNode = onDestroy.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(stopFreshnessTimer, 1371);
    endNode.IsDisabled = false; // default value
    dashboard.OnDestroy.Destination = onDestroy;
    
    var onInitialize = dashboard.CreateScreenAction("OnInitialize");
    onInitialize.Description = null; // default value
    
    var startNode2 = onInitialize.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode2.HorizontalPosition = 3200;
    startNode2.IsDisabled = false; // default value
    startNode2.VerticalPosition = 800;
    
    var assignNode = onInitialize.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(startNode2, 1371);
    assignNode.IsDisabled = false; // default value
    assignNode.Label = null; // default value
    
    var assignment = assignNode.CreateAssignment("SortBy", "Entities.SortOption.AvailableDesc");
    
    var endNode2 = onInitialize.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode, 1371);
    endNode2.IsDisabled = false; // default value
    dashboard.OnInitialize.Destination = onInitialize;
    
    var onReady = dashboard.CreateScreenAction("OnReady");
    onReady.Description = null; // default value
    
    var startNode3 = onReady.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode3.HorizontalPosition = 3200;
    startNode3.IsDisabled = false; // default value
    startNode3.VerticalPosition = 800;
    
    var startFreshnessTimer = onReady.CreateNode<OutSystems.Model.Logic.Nodes.IJavaScriptNode>("StartFreshnessTimer").ConnectedBelow(startNode3, 1371);
    startFreshnessTimer.Description = null; // default value
    startFreshnessTimer.IsDisabled = false; // default value
    startFreshnessTimer.JavaScript = "// Re-evaluate the freshness label every minute so data never silently ages while the page is open.\nif (window.cpFreshnessTimer) { clearInterval(window.cpFreshnessTimer); }\nwindow.cpFreshnessTimer = setInterval(function () { $actions.UpdateFreshness(); }, 60000);";
    
    var initialLoad = onReady.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("InitialLoad").ConnectedBelow(startFreshnessTimer, 1371);
    initialLoad.IsDisabled = false; // default value
    
    var endNode3 = onReady.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(initialLoad, 1371);
    endNode3.IsDisabled = false; // default value
    dashboard.OnReady.Destination = onReady;
    
    var loadData = dashboard.CreateScreenAction("LoadData");
    loadData.Description = "Loads availability through Carpark_GetAvailability. Ignored while a load is already running (duplicate-refresh guard). Meta (and so the visible \'Last updated\') is only replaced by a successful server response; a client-side failure keeps the previous data and marks it as not refreshed.";
    
    var forceRefresh = loadData.CreateInputParameter("ForceRefresh");
    forceRefresh.DataType = eSpace.BooleanType;
    forceRefresh.SetDefaultValue(null); // default value
    forceRefresh.Description = null; // default value
    forceRefresh.IsMandatory = false;
    
    var startNode4 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode4.HorizontalPosition = 3200;
    startNode4.IsDisabled = false; // default value
    startNode4.VerticalPosition = 800;
    
    var allExceptions = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IExceptionHandlerNode>("AllExceptions");
    allExceptions.AbortTransaction = true; // default value
    allExceptions.Exception = eSpace.AllExceptions;
    allExceptions.HorizontalPosition = 10512;
    allExceptions.IsDisabled = false; // default value
    allExceptions.LogError = true; // default value
    allExceptions.VerticalPosition = 800;
    
    var ifNode = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(startNode4, 1371);
    ifNode.SetCondition("IsRefreshing");
    ifNode.IsDisabled = false; // default value
    ifNode.Label = "Already refreshing?";
    
    var assignNode2 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(allExceptions, 1371);
    assignNode2.IsDisabled = false; // default value
    assignNode2.Label = null; // default value
    
    var assignment2 = assignNode2.CreateAssignment("IsRefreshing", "False");
    
    var assignment3 = assignNode2.CreateAssignment("IsLoading", "False");
    
    var assignment4 = assignNode2.CreateAssignment("HasLoadError", "True");
    
    var assignment5 = assignNode2.CreateAssignment("LiveMessage", "\"Carpark availability could not be refreshed.\"");
    
    var endNode4 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().Below(ifNode, 1371);
    endNode4.IsDisabled = false; // default value
    
    var assignNode3 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>();
    assignNode3.HorizontalPosition = 5028;
    assignNode3.IsDisabled = false; // default value
    assignNode3.Label = null; // default value
    assignNode3.VerticalPosition = 3542;
    
    var assignment6 = assignNode3.CreateAssignment("IsRefreshing", "True");
    
    var assignment7 = assignNode3.CreateAssignment("LiveMessage", "If(ForceRefresh, \"Refreshing carpark availability.\", \"Loading carpark availability.\")");
    
    var updateFreshnessAfterError = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("UpdateFreshnessAfterError").ConnectedBelow(assignNode2, 1371);
    updateFreshnessAfterError.IsDisabled = false; // default value
    
    var getAvailability = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteServerActionNode>("GetAvailability").ConnectedBelow(assignNode3, 1371);
    var carpark_GetAvailability = eSpace.ServerActions.Named("Carpark_GetAvailability");
    getAvailability.Action = carpark_GetAvailability;
    getAvailability.AnimationEffect = OutSystems.Model.Enumerations.ExecuteActionAnimationEffectType.None; // default value
    getAvailability.IsDisabled = false; // default value
    getAvailability.ServerRequestTimeout = 60;
    var forceRefresh2 = carpark_GetAvailability.InputParameters.Named("ForceRefresh");
    getAvailability.SetArgumentValue(forceRefresh2, "ForceRefresh");
    
    var showRefreshError = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("ShowRefreshError").ConnectedBelow(updateFreshnessAfterError, 1371);
    var outSystemsUI = eSpace.References.Named("OutSystemsUI");
    var feedbackMessageShow = outSystemsUI.ClientActions.Named("FeedbackMessageShow");
    showRefreshError.Action = feedbackMessageShow;
    showRefreshError.IsDisabled = false; // default value
    var encodedHTML = feedbackMessageShow.InputParameters.Named("EncodedHTML");
    showRefreshError.SetArgumentValue(encodedHTML, null);
    var extendedClass = feedbackMessageShow.InputParameters.Named("ExtendedClass");
    showRefreshError.SetArgumentValue(extendedClass, null); // default value
    var closeOnClick = feedbackMessageShow.InputParameters.Named("CloseOnClick");
    showRefreshError.SetArgumentValue(closeOnClick, null);
    var messageType = feedbackMessageShow.InputParameters.Named("MessageType");
    showRefreshError.SetArgumentValue(messageType, "3");
    var message = feedbackMessageShow.InputParameters.Named("Message");
    showRefreshError.SetArgumentValue(message, "If(Meta.HasData, \"We couldn\'t reach the server. The data on screen has not been updated.\", \"We couldn\'t load carpark availability. Please try again.\")");
    
    var assignNode4 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(getAvailability, 1371);
    assignNode4.IsDisabled = false; // default value
    assignNode4.Label = "Accept server result";
    
    var assignment8 = assignNode4.CreateAssignment("AllRows", "GetAvailability.Rows");
    
    var assignment9 = assignNode4.CreateAssignment("Meta", "GetAvailability.Meta");
    
    var assignment10 = assignNode4.CreateAssignment("LoadedAt", "CurrDateTime()");
    
    var assignment11 = assignNode4.CreateAssignment("IsLoading", "False");
    
    var assignment12 = assignNode4.CreateAssignment("HasLoadError", "False");
    
    var endNode5 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(showRefreshError, 1371);
    endNode5.IsDisabled = false; // default value
    
    var updateFreshness = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("UpdateFreshness").ConnectedBelow(assignNode4, 1371);
    updateFreshness.IsDisabled = false; // default value
    
    var applyFilters2 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("ApplyFilters").ConnectedBelow(updateFreshness, 1371);
    applyFilters2.Action = applyFilters;
    applyFilters2.IsDisabled = false; // default value
    
    var assignNode5 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(applyFilters2, 1371);
    assignNode5.IsDisabled = false; // default value
    assignNode5.Label = null; // default value
    
    var assignment13 = assignNode5.CreateAssignment("IsRefreshing", "False");
    
    var ifNode2 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(assignNode5, 1371);
    ifNode2.SetCondition("ForceRefresh");
    ifNode2.IsDisabled = false; // default value
    ifNode2.Label = "User asked?";
    
    var showRefreshResult = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("ShowRefreshResult").Below(ifNode2, 1371);
    showRefreshResult.Action = feedbackMessageShow;
    showRefreshResult.IsDisabled = false; // default value
    showRefreshResult.SetArgumentValue(messageType, "If(Meta.Freshness = \"Live\", 1, If(Meta.Freshness = \"Cached\", 2, 3))");
    showRefreshResult.SetArgumentValue(encodedHTML, null);
    showRefreshResult.SetArgumentValue(extendedClass, null); // default value
    showRefreshResult.SetArgumentValue(message, "If(Meta.Freshness = \"Live\", \"Carpark availability updated. Data as at \" + Meta.SourceTimestampText + \" SGT.\", If(Meta.Freshness = \"Cached\", \"Couldn\'t reach data.gov.sg. Showing the last data received.\", \"Carpark availability is temporarily unavailable.\"))");
    showRefreshResult.SetArgumentValue(closeOnClick, null);
    
    var endNode6 = loadData.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedToTheRightOf(showRefreshResult, 1828);
    endNode6.IsDisabled = false; // default value
    initialLoad.Action = loadData;
    initialLoad.SetArgumentValue(forceRefresh, "False");
    ifNode.FalseTarget = assignNode3;
    ifNode.TrueTarget = endNode4;
    
    var updateFreshness2 = dashboard.CreateScreenAction("UpdateFreshness");
    updateFreshness2.Description = "Derives the visible freshness label. Live = within Meta.StaleAfterMinutes of the data.gov.sg reading time; Stale = older; Cached = server fell back to the last good retrieval or this page\'s latest refresh failed; Unavailable = no data. Re-evaluated every minute by a timer started in OnReady.";
    
    var startNode5 = updateFreshness2.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode5.HorizontalPosition = 3200;
    startNode5.IsDisabled = false; // default value
    startNode5.VerticalPosition = 800;
    
    var assignNode6 = updateFreshness2.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(startNode5, 1371);
    assignNode6.IsDisabled = false; // default value
    assignNode6.Label = null; // default value
    
    var assignment14 = assignNode6.CreateAssignment("AgeMinutes", "If(Meta.HasData, DecimalToInteger(Trunc((If(Meta.DataAgeSeconds < 0, 0, Meta.DataAgeSeconds) + DiffSeconds(LoadedAt, CurrDateTime())) / 60.0)), 0)");
    
    var assignment15 = assignNode6.CreateAssignment("Freshness", "If(not Meta.HasData, If(IsLoading, \"Loading\", \"Unavailable\"), If(Meta.Freshness = \"Cached\" or HasLoadError, \"Cached\", If(AgeMinutes > Meta.StaleAfterMinutes, \"Stale\", \"Live\")))");
    
    var endNode7 = updateFreshness2.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode6, 1371);
    endNode7.IsDisabled = false; // default value
    updateFreshnessAfterError.Action = updateFreshness2;
    updateFreshness.Action = updateFreshness2;
    ifNode2.FalseTarget = endNode6;
    ifNode2.TrueTarget = showRefreshResult;
    
    var startNode6 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode6.HorizontalPosition = 3200;
    startNode6.IsDisabled = false; // default value
    startNode6.VerticalPosition = 800;
    
    var assignNode7 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(startNode6, 1371);
    assignNode7.IsDisabled = false; // default value
    assignNode7.Label = "Reset";
    
    var assignment16 = assignNode7.CreateAssignment("SumCarparks", "0");
    
    var assignment17 = assignNode7.CreateAssignment("SumTotalLots", "0");
    
    var assignment18 = assignNode7.CreateAssignment("SumAvailableLots", "0");
    
    var assignment19 = assignNode7.CreateAssignment("SumFullOrLow", "0");
    
    var assignment20 = assignNode7.CreateAssignment("SumValidForAvg", "0");
    
    var assignment21 = assignNode7.CreateAssignment("SumPctTotal", "0.0");
    
    var assignment22 = assignNode7.CreateAssignment("VisibleCount", "PageSize");
    
    var assignment23 = assignNode7.CreateAssignment("Query", "ToLower(Trim(SearchText))");
    
    var clearFiltered = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("ClearFiltered").ConnectedBelow(assignNode7, 1371);
    var _System_ = eSpace.References.Named("(System)");
    var listClear = _System_.ClientActions.Named("ListClear");
    clearFiltered.Action = listClear;
    clearFiltered.IsDisabled = false; // default value
    var list = listClear.InputParameters.Named("List");
    clearFiltered.SetArgumentValue(list, "FilteredRows");
    
    var forEachNode = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().ConnectedBelow(clearFiltered, 1371);
    forEachNode.IsDisabled = false; // default value
    forEachNode.Label = null; // default value
    forEachNode.SetMaximumIterations(null); // default value
    forEachNode.SetRecordList("AllRows");
    forEachNode.SetStartIndex(null); // default value
    
    var ifNode3 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ToTheRightOf(forEachNode, 1828);
    ifNode3.SetCondition("If(LotTypeFilter = NullTextIdentifier(), AllRows.Current.IsPrimary, AllRows.Current.LotTypeCode = IdentifierToText(LotTypeFilter))");
    ifNode3.FalseTarget = forEachNode;
    ifNode3.IsDisabled = false; // default value
    ifNode3.Label = "In lot-type view?";
    forEachNode.CycleTarget = ifNode3;
    
    var assignNode8 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().Below(forEachNode, 1371);
    assignNode8.IsDisabled = false; // default value
    assignNode8.Label = null; // default value
    
    var assignment24 = assignNode8.CreateAssignment("AvgPct", "If(SumValidForAvg > 0, SumPctTotal / SumValidForAvg, 0.0)");
    forEachNode.Target = assignNode8;
    
    var assignNode9 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().Below(ifNode3, 1371);
    assignNode9.IsDisabled = false; // default value
    assignNode9.Label = "Summary";
    
    var assignment25 = assignNode9.CreateAssignment("SumCarparks", "SumCarparks + 1");
    
    var assignment26 = assignNode9.CreateAssignment("SumTotalLots", "SumTotalLots + If(AllRows.Current.IsAnomalous, 0, AllRows.Current.TotalLots)");
    
    var assignment27 = assignNode9.CreateAssignment("SumAvailableLots", "SumAvailableLots + If(AllRows.Current.IsAnomalous, 0, AllRows.Current.AvailableLots)");
    
    var assignment28 = assignNode9.CreateAssignment("SumPctTotal", "SumPctTotal + If(AllRows.Current.IsAnomalous, 0.0, AllRows.Current.AvailabilityPct)");
    
    var assignment29 = assignNode9.CreateAssignment("SumValidForAvg", "SumValidForAvg + If(AllRows.Current.IsAnomalous, 0, 1)");
    
    var assignment30 = assignNode9.CreateAssignment("SumFullOrLow", "SumFullOrLow + If(AllRows.Current.StatusId = Entities.AvailabilityStatus.Full or AllRows.Current.StatusId = Entities.AvailabilityStatus.Low, 1, 0)");
    ifNode3.TrueTarget = assignNode9;
    
    var serializeFiltered = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IJSONSerializeNode>("SerializeFiltered").ConnectedBelow(assignNode8, 1371);
    serializeFiltered.SetData("FilteredRows");
    serializeFiltered.DateFormat = OutSystems.Model.Enumerations.JSONDateFormat.ISO; // default value
    serializeFiltered.Description = null; // default value
    serializeFiltered.IsDisabled = false; // default value
    serializeFiltered.SerializeDefaultValues = true;
    
    var ifNode4 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ConnectedBelow(assignNode9, 1371);
    ifNode4.SetCondition("(Query = \"\" or Index(AllRows.Current.SearchText, Query) >= 0) and (StatusFilter = NullTextIdentifier() or AllRows.Current.StatusId = StatusFilter) and (MinAvailable <= 0 or (not AllRows.Current.IsAnomalous and AllRows.Current.AvailableLots >= MinAvailable))");
    ifNode4.FalseTarget = forEachNode;
    ifNode4.IsDisabled = false; // default value
    ifNode4.Label = "Matches filters?";
    
    var sortRows = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IJavaScriptNode>("SortRows").ConnectedBelow(serializeFiltered, 1371);
    sortRows.Description = "Sorts the filtered rows by the selected SortOption. Done in JavaScript because System ListSort created through the Model API compiles its By expression against List.Current instead of the compared element and leaves the list unsorted. Inconsistent readings always sort last; carpark number breaks ties.";
    sortRows.IsDisabled = false; // default value
    sortRows.JavaScript = "var rows = JSON.parse($parameters.RowsJson || \"[]\");\nvar specs = {\n    AvailableDesc: [\"AvailableLots\", -1], AvailableAsc: [\"AvailableLots\", 1],\n    PctDesc: [\"AvailabilityPct\", -1], PctAsc: [\"AvailabilityPct\", 1],\n    NumberAsc: [\"CarparkNumber\", 1], UpdatedDesc: [\"UpdatedAt\", -1]\n};\nvar spec = specs[$parameters.SortKey] || specs.AvailableDesc;\nvar field = spec[0], dir = spec[1];\nvar numeric = field === \"AvailableLots\" || field === \"AvailabilityPct\";\nrows.sort(function (a, b) {\n    // Inconsistent readings never compete with real figures in a numeric sort.\n    if (numeric && a.IsAnomalous !== b.IsAnomalous) { return a.IsAnomalous ? 1 : -1; }\n    var x = a[field], y = b[field];\n    if (x < y) { return -dir; }\n    if (x > y) { return dir; }\n    return a.CarparkNumber < b.CarparkNumber ? -1 : (a.CarparkNumber > b.CarparkNumber ? 1 : 0);\n});\n$parameters.SortedJson = JSON.stringify(rows);";
    
    var rowsJson = sortRows.CreateInputParameter("RowsJson");
    rowsJson.DataType = eSpace.TextType;
    rowsJson.Description = null; // default value
    rowsJson.IsMandatory = true; // default value
    
    var sortKey = sortRows.CreateInputParameter("SortKey");
    sortKey.DataType = eSpace.TextType;
    sortKey.Description = null; // default value
    sortKey.IsMandatory = true; // default value
    
    var sortedJson = sortRows.CreateOutputParameter("SortedJson");
    sortedJson.DataType = eSpace.TextType;
    sortedJson.SetDefaultValue(null); // default value
    sortedJson.Description = null; // default value
    
    var addResult = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("AddResult").Below(ifNode4, 1371);
    var listAppend = _System_.ClientActions.Named("ListAppend");
    addResult.Action = listAppend;
    addResult.IsDisabled = false; // default value
    addResult.Target = forEachNode;
    var element = listAppend.InputParameters.Named("Element");
    addResult.SetArgumentValue(element, "AllRows.Current");
    var list2 = listAppend.InputParameters.Named("List");
    addResult.SetArgumentValue(list2, "FilteredRows");
    ifNode4.TrueTarget = addResult;
    
    var deserializeSorted = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IJSONDeserializeNode>("DeserializeSorted").ConnectedBelow(sortRows, 1371);
    deserializeSorted.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    deserializeSorted.DateFormat = OutSystems.Model.Enumerations.JSONDateFormat.ISO; // default value
    deserializeSorted.Description = null; // default value
    deserializeSorted.IsDisabled = false; // default value
    deserializeSorted.SetJSONString("SortRows.SortedJson");
    sortRows.SetArgumentValue(sortKey, "IdentifierToText(SortBy)");
    sortRows.SetArgumentValue(rowsJson, "SerializeFiltered.JSON");
    
    var assignNode10 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(deserializeSorted, 1371);
    assignNode10.IsDisabled = false; // default value
    assignNode10.Label = "Sorted rows";
    
    var assignment31 = assignNode10.CreateAssignment("FilteredRows", "DeserializeSorted.Data");
    
    var rebuildPage = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("RebuildPage").ConnectedBelow(assignNode10, 1371);
    rebuildPage.IsDisabled = false; // default value
    
    var buildPage = dashboard.CreateScreenAction("BuildPage");
    buildPage.Description = "Copies the first VisibleCount filtered rows into PageRows so only a page of cards is rendered at a time.";
    
    var startNode7 = buildPage.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode7.HorizontalPosition = 3200;
    startNode7.IsDisabled = false; // default value
    startNode7.VerticalPosition = 800;
    
    var clearPage = buildPage.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("ClearPage").ConnectedBelow(startNode7, 1371);
    clearPage.Action = listClear;
    clearPage.IsDisabled = false; // default value
    clearPage.SetArgumentValue(list, "PageRows");
    
    var forEachNode2 = buildPage.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().ConnectedBelow(clearPage, 1371);
    forEachNode2.IsDisabled = false; // default value
    forEachNode2.Label = null; // default value
    forEachNode2.SetMaximumIterations("VisibleCount");
    forEachNode2.SetRecordList("FilteredRows");
    forEachNode2.SetStartIndex(null); // default value
    
    var appendToPage = buildPage.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("AppendToPage").ToTheRightOf(forEachNode2, 1828);
    appendToPage.Action = listAppend;
    appendToPage.IsDisabled = false; // default value
    appendToPage.Target = forEachNode2;
    appendToPage.SetArgumentValue(list2, "PageRows");
    appendToPage.SetArgumentValue(element, "FilteredRows.Current");
    
    var endNode8 = buildPage.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().Below(forEachNode2, 1371);
    endNode8.IsDisabled = false; // default value
    rebuildPage.Action = buildPage;
    
    var assignNode11 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(rebuildPage, 1371);
    assignNode11.IsDisabled = false; // default value
    assignNode11.Label = null; // default value
    
    var assignment32 = assignNode11.CreateAssignment("LiveMessage", "FormatDecimal(IntegerToDecimal(FilteredRows.Length), 0, \".\", \",\") + If(FilteredRows.Length = 1, \" carpark matches\", \" carparks match\") + \" the current filters.\"");
    forEachNode2.CycleTarget = appendToPage;
    forEachNode2.Target = endNode8;
    
    var endNode9 = applyFilters.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode11, 1371);
    endNode9.IsDisabled = false; // default value
    
    var clearFilters = dashboard.CreateScreenAction("ClearFilters");
    clearFilters.Description = null; // default value
    
    var startNode8 = clearFilters.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode8.HorizontalPosition = 3200;
    startNode8.IsDisabled = false; // default value
    startNode8.VerticalPosition = 800;
    
    var assignNode12 = clearFilters.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(startNode8, 1371);
    assignNode12.IsDisabled = false; // default value
    assignNode12.Label = null; // default value
    
    var assignment33 = assignNode12.CreateAssignment("SearchText", "\"\"");
    
    var assignment34 = assignNode12.CreateAssignment("StatusFilter", "NullTextIdentifier()");
    
    var assignment35 = assignNode12.CreateAssignment("LotTypeFilter", "NullTextIdentifier()");
    
    var assignment36 = assignNode12.CreateAssignment("MinAvailable", "0");
    
    var assignment37 = assignNode12.CreateAssignment("SortBy", "Entities.SortOption.AvailableDesc");
    
    var applyFilters3 = clearFilters.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("ApplyFilters").ConnectedBelow(assignNode12, 1371);
    applyFilters3.Action = applyFilters;
    applyFilters3.IsDisabled = false; // default value
    
    var endNode10 = clearFilters.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(applyFilters3, 1371);
    endNode10.IsDisabled = false; // default value
    
    var closeDetail = dashboard.CreateScreenAction("CloseDetail");
    closeDetail.Description = null; // default value
    
    var startNode9 = closeDetail.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode9.HorizontalPosition = 3200;
    startNode9.IsDisabled = false; // default value
    startNode9.VerticalPosition = 800;
    
    var assignNode13 = closeDetail.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(startNode9, 1371);
    assignNode13.IsDisabled = false; // default value
    assignNode13.Label = null; // default value
    
    var assignment38 = assignNode13.CreateAssignment("ShowDetail", "False");
    
    var endNode11 = closeDetail.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode13, 1371);
    endNode11.IsDisabled = false; // default value
    
    var openDetail = dashboard.CreateScreenAction("OpenDetail");
    openDetail.Description = "Collects every lot-type row of the chosen carpark into DetailRows (a plain loop: System ListFilter created through the Model API evaluates its condition against List.Current and returns the wrong rows) and opens the detail popup.";
    
    var carpark = openDetail.CreateInputParameter("Carpark");
    carpark.DataType = eSpace.TextType;
    carpark.Description = null; // default value
    carpark.IsMandatory = true; // default value
    
    var startNode10 = openDetail.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode10.IsDisabled = false; // default value
    
    var clearDetail = openDetail.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("ClearDetail").ConnectedBelow(startNode10);
    clearDetail.Action = listClear;
    clearDetail.IsDisabled = false; // default value
    clearDetail.SetArgumentValue(list, "DetailRows");
    
    var forEachNode3 = openDetail.CreateNode<OutSystems.Model.Logic.Nodes.IForEachNode>().ConnectedBelow(clearDetail);
    forEachNode3.IsDisabled = false; // default value
    forEachNode3.Label = null; // default value
    forEachNode3.SetMaximumIterations(null); // default value
    forEachNode3.SetRecordList("AllRows");
    forEachNode3.SetStartIndex(null); // default value
    
    var ifNode5 = openDetail.CreateNode<OutSystems.Model.Logic.Nodes.IIfNode>().ToTheRightOf(forEachNode3, 1828);
    ifNode5.SetCondition("AllRows.Current.CarparkNumber = Carpark");
    ifNode5.FalseTarget = forEachNode3;
    ifNode5.IsDisabled = false; // default value
    ifNode5.Label = "Row of this carpark?";
    forEachNode3.CycleTarget = ifNode5;
    
    var assignNode14 = openDetail.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().Below(forEachNode3, 2742);
    assignNode14.IsDisabled = false; // default value
    assignNode14.Label = null; // default value
    
    var assignment39 = assignNode14.CreateAssignment("SelectedCarpark", "Carpark");
    
    var assignment40 = assignNode14.CreateAssignment("ShowDetail", "True");
    forEachNode3.Target = assignNode14;
    
    var addDetailRow = openDetail.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("AddDetailRow").Below(ifNode5, 1371);
    addDetailRow.Action = listAppend;
    addDetailRow.IsDisabled = false; // default value
    addDetailRow.Target = forEachNode3;
    addDetailRow.SetArgumentValue(element, "AllRows.Current");
    addDetailRow.SetArgumentValue(list2, "DetailRows");
    ifNode5.TrueTarget = addDetailRow;
    
    var endNode12 = openDetail.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode14, 1371);
    endNode12.IsDisabled = false; // default value
    
    var showMore = dashboard.CreateScreenAction("ShowMore");
    showMore.Description = null; // default value
    
    var startNode11 = showMore.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode11.HorizontalPosition = 3200;
    startNode11.IsDisabled = false; // default value
    startNode11.VerticalPosition = 800;
    
    var assignNode15 = showMore.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(startNode11, 1371);
    assignNode15.IsDisabled = false; // default value
    assignNode15.Label = null; // default value
    
    var assignment41 = assignNode15.CreateAssignment("VisibleCount", "VisibleCount + PageSize");
    
    var buildPage2 = showMore.CreateNode<OutSystems.Model.Logic.Nodes.IExecuteClientActionNode>("BuildPage").ConnectedBelow(assignNode15, 1371);
    buildPage2.Action = buildPage;
    buildPage2.IsDisabled = false; // default value
    
    var endNode13 = showMore.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(buildPage2, 1371);
    endNode13.IsDisabled = false; // default value
    
    var layoutTopMenuInstance = dashboard.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    layoutTopMenuInstance.CustomStyle = null; // default value
    layoutTopMenuInstance.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    var layouts = eSpace.MobileFlows.Named("Layouts");
    var layoutTopMenu = layouts.Nodes.OfType<OutSystems.Model.UI.Mobile.IMobileBlock>().Named("LayoutTopMenu");
    layoutTopMenuInstance.SourceBlock = layoutTopMenu;
    // delete default content in Header placeholder
    var placeholderContentWidget = layoutTopMenuInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Header");
    placeholderContentWidget.Widgets.ToList().ForEach(w => w.Delete());
    var enableAccessibilityFeatures = layoutTopMenu.InputParameters.Named("EnableAccessibilityFeatures");
    layoutTopMenuInstance.SetArgumentValue(enableAccessibilityFeatures, null); // default value
    var extendedClass2 = layoutTopMenu.InputParameters.Named("ExtendedClass");
    layoutTopMenuInstance.SetArgumentValue(extendedClass2, null);
    var hasFixedHeader = layoutTopMenu.InputParameters.Named("HasFixedHeader");
    layoutTopMenuInstance.SetArgumentValue(hasFixedHeader, null);
    
    var menuInstance = placeholderContentWidget.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    menuInstance.CustomStyle = null; // default value
    menuInstance.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    var common = eSpace.MobileFlows.Named("Common");
    var menu = common.Nodes.OfType<OutSystems.Model.UI.Mobile.IMobileBlock>().Named("Menu");
    menuInstance.SourceBlock = menu;
    var activeItem = menu.InputParameters.Named("ActiveItem");
    menuInstance.SetArgumentValue(activeItem, null);
    var activeSubItem = menu.InputParameters.Named("ActiveSubItem");
    menuInstance.SetArgumentValue(activeSubItem, null);
    
    var placeholderContentWidget2 = layoutTopMenuInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Title");
    var container = placeholderContentWidget2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container.Animate = false; // default value
    container.ColumnGap = null; // default value
    container.ColumnSpan = null; // default value
    container.CustomStyle = null; // default value
    container.FlexBasis = null; // default value
    container.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container.Gap = null;
    container.GridColumns = null; // default value
    container.GridRows = null; // default value
    container.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container.RowGap = null; // default value
    container.RowSpan = null; // default value
    container.SetStyle("\"cp-page-title\"");
    container.SetVisible("True"); // default value
    container.VisibleWasExpression = false; // default value
    container.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container.Width = "(fill parent)"; // default value
    
    var textWidget = container.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget.CustomStyle = null; // default value
    textWidget.SetStyleClasses(null); // default value
    textWidget.Text = "Singapore Carpark Availability";
    
    var extendedProperty = container.CreateExtendedProperty();
    extendedProperty.Property = "role";
    extendedProperty.SetValue("\"heading\"");
    
    var extendedProperty2 = container.CreateExtendedProperty();
    extendedProperty2.Property = "aria-level";
    extendedProperty2.SetValue("\"1\"");
    
    var placeholderContentWidget3 = layoutTopMenuInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "MainContent");
    var container2 = placeholderContentWidget3.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container2.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container2.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container2.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container2.Animate = false; // default value
    container2.ColumnGap = null; // default value
    container2.ColumnSpan = null; // default value
    container2.CustomStyle = null; // default value
    container2.FlexBasis = null; // default value
    container2.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container2.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container2.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container2.Gap = null;
    container2.GridColumns = null; // default value
    container2.GridRows = null; // default value
    container2.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container2.RowGap = null; // default value
    container2.RowSpan = null; // default value
    container2.SetStyle("\"cp-dashboard\"");
    container2.SetVisible("True"); // default value
    container2.VisibleWasExpression = false; // default value
    container2.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container2.Width = "(fill parent)"; // default value
    
    var container3 = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container3.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container3.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container3.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container3.Animate = false; // default value
    container3.ColumnGap = null; // default value
    container3.ColumnSpan = null; // default value
    container3.CustomStyle = null; // default value
    container3.FlexBasis = null; // default value
    container3.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container3.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container3.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container3.Gap = null;
    container3.GridColumns = null; // default value
    container3.GridRows = null; // default value
    container3.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container3.RowGap = null; // default value
    container3.RowSpan = null; // default value
    container3.SetStyle("\"cp-intro\"");
    container3.SetVisible("True"); // default value
    container3.VisibleWasExpression = false; // default value
    container3.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container3.Width = "(fill parent)"; // default value
    
    var container4 = container3.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container4.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container4.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container4.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container4.Animate = false; // default value
    container4.ColumnGap = null; // default value
    container4.ColumnSpan = null; // default value
    container4.CustomStyle = null; // default value
    container4.FlexBasis = null; // default value
    container4.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container4.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container4.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container4.Gap = null;
    container4.GridColumns = null; // default value
    container4.GridRows = null; // default value
    container4.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container4.RowGap = null; // default value
    container4.RowSpan = null; // default value
    container4.SetStyle("\"cp-subtitle\"");
    container4.SetVisible("True"); // default value
    container4.VisibleWasExpression = false; // default value
    container4.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container4.Width = "(fill parent)"; // default value
    
    var textWidget2 = container4.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget2.CustomStyle = null; // default value
    textWidget2.SetStyleClasses(null); // default value
    textWidget2.Text = "Real-time availability of HDB carpark lots across Singapore, from data.gov.sg. Figures refresh about once a minute at source.";
    
    var container5 = container3.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container5.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container5.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container5.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container5.Animate = false; // default value
    container5.ColumnGap = null; // default value
    container5.ColumnSpan = null; // default value
    container5.CustomStyle = null; // default value
    container5.FlexBasis = null; // default value
    container5.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container5.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container5.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container5.Gap = null;
    container5.GridColumns = null; // default value
    container5.GridRows = null; // default value
    container5.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container5.RowGap = null; // default value
    container5.RowSpan = null; // default value
    container5.SetStyle("\"cp-freshness-row\"");
    container5.SetVisible("True"); // default value
    container5.VisibleWasExpression = false; // default value
    container5.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container5.Width = "(fill parent)"; // default value
    
    var container6 = container5.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container6.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container6.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container6.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container6.Animate = false; // default value
    container6.ColumnGap = null; // default value
    container6.ColumnSpan = null; // default value
    container6.CustomStyle = null; // default value
    container6.FlexBasis = null; // default value
    container6.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container6.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container6.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container6.Gap = null;
    container6.GridColumns = null; // default value
    container6.GridRows = null; // default value
    container6.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container6.RowGap = null; // default value
    container6.RowSpan = null; // default value
    container6.SetStyle("\"cp-fresh cp-fresh-\" + ToLower(Freshness)");
    container6.SetVisible("True"); // default value
    container6.VisibleWasExpression = false; // default value
    container6.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container6.Width = "(fill parent)"; // default value
    
    var expression = container6.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression.ColumnSpan = null; // default value
    expression.CustomStyle = null; // default value
    expression.Example = null; // default value
    expression.FlexBasis = null; // default value
    expression.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression.RowSpan = null; // default value
    expression.SetStyle(null); // default value
    expression.SetValue("If(Freshness = \"Live\", \"Live data\", If(Freshness = \"Stale\", \"Stale data\", If(Freshness = \"Cached\", \"Not refreshed\", If(Freshness = \"Loading\", \"Loading\", \"Data unavailable\"))))");
    expression.Width = ""; // default value
    
    var container7 = container5.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container7.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container7.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container7.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container7.Animate = false; // default value
    container7.ColumnGap = null; // default value
    container7.ColumnSpan = null; // default value
    container7.CustomStyle = null; // default value
    container7.FlexBasis = null; // default value
    container7.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container7.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container7.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container7.Gap = null;
    container7.GridColumns = null; // default value
    container7.GridRows = null; // default value
    container7.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container7.RowGap = null; // default value
    container7.RowSpan = null; // default value
    container7.SetStyle("\"cp-updated\"");
    container7.SetVisible("True"); // default value
    container7.VisibleWasExpression = false; // default value
    container7.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container7.Width = "(fill parent)"; // default value
    
    var expression2 = container7.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression2.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression2.ColumnSpan = null; // default value
    expression2.CustomStyle = null; // default value
    expression2.Example = null; // default value
    expression2.FlexBasis = null; // default value
    expression2.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression2.RowSpan = null; // default value
    expression2.SetStyle(null); // default value
    expression2.SetValue("\"Last updated: \" + If(Meta.HasData, Meta.SourceTimestampText + \" SGT\", \"not yet available\") + If(Meta.HasData and AgeMinutes >= 1, \" (\" + AgeMinutes + If(AgeMinutes = 1, \" minute ago)\", \" minutes ago)\"), \"\")");
    expression2.Width = ""; // default value
    
    var refreshButton = container5.CreateWidget<ServiceStudio.Plugin.NRWidgets.IButton>("RefreshButton");
    /*** Deleting Widgets that are created by default when instantiating an IButton ***/
    refreshButton.Widgets.ToList().ForEach(x => x.Delete());
    refreshButton.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    refreshButton.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    refreshButton.ColumnGap = null; // default value
    refreshButton.ColumnSpan = null; // default value
    refreshButton.SetConfirmationMessage(null); // default value
    refreshButton.CustomStyle = null; // default value
    refreshButton.SetEnabled("not IsRefreshing");
    refreshButton.FlexBasis = null; // default value
    refreshButton.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    refreshButton.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    refreshButton.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    refreshButton.Gap = null;
    refreshButton.GridColumns = null; // default value
    refreshButton.GridRows = null; // default value
    refreshButton.IsDefault = false; // default value
    refreshButton.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    refreshButton.RowGap = null; // default value
    refreshButton.RowSpan = null; // default value
    refreshButton.SetStyle("\"btn btn-primary cp-refresh-btn\"");
    refreshButton.SetVisible("True"); // default value
    refreshButton.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    refreshButton.Width = ""; // default value
    
    var expression3 = refreshButton.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression3.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression3.ColumnSpan = null; // default value
    expression3.CustomStyle = null; // default value
    expression3.Example = null; // default value
    expression3.FlexBasis = null; // default value
    expression3.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression3.RowSpan = null; // default value
    expression3.SetStyle(null); // default value
    expression3.SetValue("If(IsRefreshing, \"Refreshing…\", \"Refresh data\")");
    expression3.Width = ""; // default value
    var builtinEvent = refreshButton.OnClick;
    builtinEvent.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent.Destination = loadData;
    builtinEvent.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    builtinEvent.SetArgumentValue(forceRefresh, "True");
    
    var extendedProperty3 = refreshButton.CreateExtendedProperty();
    extendedProperty3.Property = "aria-busy";
    extendedProperty3.SetValue("If(IsRefreshing, \"true\", \"false\")");
    
    var extendedProperty4 = refreshButton.CreateExtendedProperty();
    extendedProperty4.Property = "title";
    extendedProperty4.SetValue("\"Fetch the latest availability from data.gov.sg\"");
    
    var extendedProperty5 = container5.CreateExtendedProperty();
    extendedProperty5.Property = "aria-live";
    extendedProperty5.SetValue("\"polite\"");
    
    var container8 = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container8.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container8.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container8.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container8.Animate = false; // default value
    container8.ColumnGap = null; // default value
    container8.ColumnSpan = null; // default value
    container8.CustomStyle = null; // default value
    container8.FlexBasis = null; // default value
    container8.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container8.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container8.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container8.Gap = null;
    container8.GridColumns = null; // default value
    container8.GridRows = null; // default value
    container8.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container8.RowGap = null; // default value
    container8.RowSpan = null; // default value
    container8.SetStyle("\"cp-banners\"");
    container8.SetVisible("True"); // default value
    container8.VisibleWasExpression = false; // default value
    container8.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container8.Width = "(fill parent)"; // default value
    
    var ifWidget = container8.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget.Animate = false; // default value
    ifWidget.SetCondition("Freshness = \"Cached\"");
    ifWidget.CustomStyle = null; // default value
    ifWidget.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container9 = ifWidget.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container9.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container9.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container9.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container9.Animate = false; // default value
    container9.ColumnGap = null; // default value
    container9.ColumnSpan = null; // default value
    container9.CustomStyle = null; // default value
    container9.FlexBasis = null; // default value
    container9.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container9.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container9.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container9.Gap = null;
    container9.GridColumns = null; // default value
    container9.GridRows = null; // default value
    container9.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container9.RowGap = null; // default value
    container9.RowSpan = null; // default value
    container9.SetStyle(null); // default value
    container9.SetVisible("True"); // default value
    container9.VisibleWasExpression = false; // default value
    container9.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container9.Width = "(fill parent)"; // default value
    
    var alertInstance = container9.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    alertInstance.CustomStyle = null; // default value
    alertInstance.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    var content = outSystemsUI.MobileFlows.Named("Content");
    var alert = content.Nodes.OfType<OutSystems.Model.UI.Mobile.IMobileBlockSignature>().Named("Alert");
    alertInstance.SourceBlock = alert;
    var extendedClass3 = alert.InputParameters.Named("ExtendedClass");
    alertInstance.SetArgumentValue(extendedClass3, null); // default value
    var alertType = alert.InputParameters.Named("AlertType");
    alertInstance.SetArgumentValue(alertType, "Entities.Alert.Warning");
    
    var placeholderContentWidget4 = alertInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "MessageText");
    var container10 = placeholderContentWidget4.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container10.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container10.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container10.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container10.Animate = false; // default value
    container10.ColumnGap = null; // default value
    container10.ColumnSpan = null; // default value
    container10.CustomStyle = null; // default value
    container10.FlexBasis = null; // default value
    container10.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container10.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container10.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container10.Gap = null;
    container10.GridColumns = null; // default value
    container10.GridRows = null; // default value
    container10.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container10.RowGap = null; // default value
    container10.RowSpan = null; // default value
    container10.SetStyle(null); // default value
    container10.SetVisible("True"); // default value
    container10.VisibleWasExpression = false; // default value
    container10.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container10.Width = "(fill parent)"; // default value
    
    var expression4 = container10.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression4.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression4.ColumnSpan = null; // default value
    expression4.CustomStyle = null; // default value
    expression4.Example = null; // default value
    expression4.FlexBasis = null; // default value
    expression4.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression4.RowSpan = null; // default value
    expression4.SetStyle(null); // default value
    expression4.SetValue("If(HasLoadError, \"The latest refresh failed, so the figures below have not been updated.\", Meta.Message) + \" Data shown is as at \" + Meta.SourceTimestampText + \" SGT.\"");
    expression4.Width = ""; // default value
    
    var ifWidget2 = container8.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget2.Animate = false; // default value
    ifWidget2.SetCondition("Freshness = \"Stale\"");
    ifWidget2.CustomStyle = null; // default value
    ifWidget2.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container11 = ifWidget2.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container11.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container11.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container11.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container11.Animate = false; // default value
    container11.ColumnGap = null; // default value
    container11.ColumnSpan = null; // default value
    container11.CustomStyle = null; // default value
    container11.FlexBasis = null; // default value
    container11.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container11.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container11.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container11.Gap = null;
    container11.GridColumns = null; // default value
    container11.GridRows = null; // default value
    container11.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container11.RowGap = null; // default value
    container11.RowSpan = null; // default value
    container11.SetStyle(null); // default value
    container11.SetVisible("True"); // default value
    container11.VisibleWasExpression = false; // default value
    container11.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container11.Width = "(fill parent)"; // default value
    
    var alertInstance2 = container11.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    alertInstance2.CustomStyle = null; // default value
    alertInstance2.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    alertInstance2.SourceBlock = alert;
    alertInstance2.SetArgumentValue(extendedClass3, null); // default value
    alertInstance2.SetArgumentValue(alertType, "Entities.Alert.Warning");
    
    var placeholderContentWidget5 = alertInstance2.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "MessageText");
    var container12 = placeholderContentWidget5.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container12.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container12.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container12.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container12.Animate = false; // default value
    container12.ColumnGap = null; // default value
    container12.ColumnSpan = null; // default value
    container12.CustomStyle = null; // default value
    container12.FlexBasis = null; // default value
    container12.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container12.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container12.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container12.Gap = null;
    container12.GridColumns = null; // default value
    container12.GridRows = null; // default value
    container12.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container12.RowGap = null; // default value
    container12.RowSpan = null; // default value
    container12.SetStyle(null); // default value
    container12.SetVisible("True"); // default value
    container12.VisibleWasExpression = false; // default value
    container12.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container12.Width = "(fill parent)"; // default value
    
    var expression5 = container12.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression5.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression5.ColumnSpan = null; // default value
    expression5.CustomStyle = null; // default value
    expression5.Example = null; // default value
    expression5.FlexBasis = null; // default value
    expression5.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression5.RowSpan = null; // default value
    expression5.SetStyle(null); // default value
    expression5.SetValue("\"These figures are \" + AgeMinutes + \" minutes old, beyond the \" + Meta.StaleAfterMinutes + \"-minute freshness limit, and may not reflect current availability. Select Refresh data to check for newer readings.\"");
    expression5.Width = ""; // default value
    
    var ifWidget3 = container8.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget3.Animate = false; // default value
    ifWidget3.SetCondition("Meta.HasData and (Meta.RejectedCount + Meta.DuplicateCount + Meta.AnomalyCount > 0 or not Meta.HasLocationData)");
    ifWidget3.CustomStyle = null; // default value
    ifWidget3.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container13 = ifWidget3.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container13.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container13.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container13.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container13.Animate = false; // default value
    container13.ColumnGap = null; // default value
    container13.ColumnSpan = null; // default value
    container13.CustomStyle = null; // default value
    container13.FlexBasis = null; // default value
    container13.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container13.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container13.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container13.Gap = null;
    container13.GridColumns = null; // default value
    container13.GridRows = null; // default value
    container13.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container13.RowGap = null; // default value
    container13.RowSpan = null; // default value
    container13.SetStyle("\"cp-quality-note\"");
    container13.SetVisible("True"); // default value
    container13.VisibleWasExpression = false; // default value
    container13.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container13.Width = "(fill parent)"; // default value
    
    var expression6 = container13.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression6.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression6.ColumnSpan = null; // default value
    expression6.CustomStyle = null; // default value
    expression6.Example = null; // default value
    expression6.FlexBasis = null; // default value
    expression6.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression6.RowSpan = null; // default value
    expression6.SetStyle(null); // default value
    expression6.SetValue("\"Data quality: \" + Meta.RejectedCount + \" incomplete records omitted, \" + Meta.DuplicateCount + \" duplicate records merged, \" + Meta.AnomalyCount + \" inconsistent lot readings shown as Data unavailable.\" + If(Meta.HasLocationData, \"\", \" Address details are temporarily unavailable.\")");
    expression6.Width = ""; // default value
    
    var extendedProperty6 = container8.CreateExtendedProperty();
    extendedProperty6.Property = "aria-live";
    extendedProperty6.SetValue("\"polite\"");
    
    var container14 = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container14.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container14.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container14.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container14.Animate = false; // default value
    container14.ColumnGap = null; // default value
    container14.ColumnSpan = null; // default value
    container14.CustomStyle = null; // default value
    container14.FlexBasis = null; // default value
    container14.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container14.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container14.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container14.Gap = null;
    container14.GridColumns = null; // default value
    container14.GridRows = null; // default value
    container14.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container14.RowGap = null; // default value
    container14.RowSpan = null; // default value
    container14.SetStyle("\"cp-kpis\"");
    container14.SetVisible("True"); // default value
    container14.VisibleWasExpression = false; // default value
    container14.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container14.Width = "(fill parent)"; // default value
    
    var cardInstance = container14.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    cardInstance.CustomStyle = null; // default value
    cardInstance.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    var card = content.Nodes.OfType<OutSystems.Model.UI.Mobile.IMobileBlockSignature>().Named("Card");
    cardInstance.SourceBlock = card;
    var extendedClass4 = card.InputParameters.Named("ExtendedClass");
    cardInstance.SetArgumentValue(extendedClass4, "\"cp-kpi cp-kpi-carparks\"");
    var usePadding = card.InputParameters.Named("UsePadding");
    cardInstance.SetArgumentValue(usePadding, "True");
    
    var placeholderContentWidget6 = cardInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    var container15 = placeholderContentWidget6.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container15.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container15.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container15.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container15.Animate = false; // default value
    container15.ColumnGap = null; // default value
    container15.ColumnSpan = null; // default value
    container15.CustomStyle = null; // default value
    container15.FlexBasis = null; // default value
    container15.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container15.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container15.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container15.Gap = null;
    container15.GridColumns = null; // default value
    container15.GridRows = null; // default value
    container15.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container15.RowGap = null; // default value
    container15.RowSpan = null; // default value
    container15.SetStyle(null); // default value
    container15.SetVisible("True"); // default value
    container15.VisibleWasExpression = false; // default value
    container15.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container15.Width = "(fill parent)"; // default value
    
    var container16 = container15.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container16.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container16.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container16.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container16.Animate = false; // default value
    container16.ColumnGap = null; // default value
    container16.ColumnSpan = null; // default value
    container16.CustomStyle = null; // default value
    container16.FlexBasis = null; // default value
    container16.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container16.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container16.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container16.Gap = null;
    container16.GridColumns = null; // default value
    container16.GridRows = null; // default value
    container16.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container16.RowGap = null; // default value
    container16.RowSpan = null; // default value
    container16.SetStyle("\"cp-kpi-label\"");
    container16.SetVisible("True"); // default value
    container16.VisibleWasExpression = false; // default value
    container16.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container16.Width = "(fill parent)"; // default value
    
    var textWidget3 = container16.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget3.CustomStyle = null; // default value
    textWidget3.SetStyleClasses(null); // default value
    textWidget3.Text = "Carparks";
    
    var container17 = container15.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container17.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container17.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container17.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container17.Animate = false; // default value
    container17.ColumnGap = null; // default value
    container17.ColumnSpan = null; // default value
    container17.CustomStyle = null; // default value
    container17.FlexBasis = null; // default value
    container17.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container17.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container17.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container17.Gap = null;
    container17.GridColumns = null; // default value
    container17.GridRows = null; // default value
    container17.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container17.RowGap = null; // default value
    container17.RowSpan = null; // default value
    container17.SetStyle("\"cp-kpi-value\"");
    container17.SetVisible("True"); // default value
    container17.VisibleWasExpression = false; // default value
    container17.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container17.Width = "(fill parent)"; // default value
    
    var expression7 = container17.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression7.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression7.ColumnSpan = null; // default value
    expression7.CustomStyle = null; // default value
    expression7.Example = null; // default value
    expression7.FlexBasis = null; // default value
    expression7.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression7.RowSpan = null; // default value
    expression7.SetStyle(null); // default value
    expression7.SetValue("If(IsLoading or not Meta.HasData, \"–\", FormatDecimal(IntegerToDecimal(SumCarparks), 0, \".\", \",\"))");
    expression7.Width = ""; // default value
    
    var container18 = container15.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container18.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container18.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container18.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container18.Animate = false; // default value
    container18.ColumnGap = null; // default value
    container18.ColumnSpan = null; // default value
    container18.CustomStyle = null; // default value
    container18.FlexBasis = null; // default value
    container18.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container18.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container18.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container18.Gap = null;
    container18.GridColumns = null; // default value
    container18.GridRows = null; // default value
    container18.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container18.RowGap = null; // default value
    container18.RowSpan = null; // default value
    container18.SetStyle("\"cp-kpi-hint\"");
    container18.SetVisible("True"); // default value
    container18.VisibleWasExpression = false; // default value
    container18.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container18.Width = "(fill parent)"; // default value
    
    var expression8 = container18.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression8.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression8.ColumnSpan = null; // default value
    expression8.CustomStyle = null; // default value
    expression8.Example = null; // default value
    expression8.FlexBasis = null; // default value
    expression8.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression8.RowSpan = null; // default value
    expression8.SetStyle(null); // default value
    expression8.SetValue("\"With current availability data\"");
    expression8.Width = ""; // default value
    
    var cardInstance2 = container14.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    cardInstance2.CustomStyle = null; // default value
    cardInstance2.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    cardInstance2.SourceBlock = card;
    cardInstance2.SetArgumentValue(extendedClass4, "\"cp-kpi cp-kpi-total\"");
    cardInstance2.SetArgumentValue(usePadding, "True");
    
    var placeholderContentWidget7 = cardInstance2.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    var container19 = placeholderContentWidget7.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container19.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container19.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container19.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container19.Animate = false; // default value
    container19.ColumnGap = null; // default value
    container19.ColumnSpan = null; // default value
    container19.CustomStyle = null; // default value
    container19.FlexBasis = null; // default value
    container19.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container19.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container19.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container19.Gap = null;
    container19.GridColumns = null; // default value
    container19.GridRows = null; // default value
    container19.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container19.RowGap = null; // default value
    container19.RowSpan = null; // default value
    container19.SetStyle(null); // default value
    container19.SetVisible("True"); // default value
    container19.VisibleWasExpression = false; // default value
    container19.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container19.Width = "(fill parent)"; // default value
    
    var container20 = container19.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container20.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container20.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container20.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container20.Animate = false; // default value
    container20.ColumnGap = null; // default value
    container20.ColumnSpan = null; // default value
    container20.CustomStyle = null; // default value
    container20.FlexBasis = null; // default value
    container20.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container20.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container20.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container20.Gap = null;
    container20.GridColumns = null; // default value
    container20.GridRows = null; // default value
    container20.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container20.RowGap = null; // default value
    container20.RowSpan = null; // default value
    container20.SetStyle("\"cp-kpi-label\"");
    container20.SetVisible("True"); // default value
    container20.VisibleWasExpression = false; // default value
    container20.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container20.Width = "(fill parent)"; // default value
    
    var textWidget4 = container20.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget4.CustomStyle = null; // default value
    textWidget4.SetStyleClasses(null); // default value
    textWidget4.Text = "Total lots";
    
    var container21 = container19.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container21.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container21.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container21.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container21.Animate = false; // default value
    container21.ColumnGap = null; // default value
    container21.ColumnSpan = null; // default value
    container21.CustomStyle = null; // default value
    container21.FlexBasis = null; // default value
    container21.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container21.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container21.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container21.Gap = null;
    container21.GridColumns = null; // default value
    container21.GridRows = null; // default value
    container21.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container21.RowGap = null; // default value
    container21.RowSpan = null; // default value
    container21.SetStyle("\"cp-kpi-value\"");
    container21.SetVisible("True"); // default value
    container21.VisibleWasExpression = false; // default value
    container21.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container21.Width = "(fill parent)"; // default value
    
    var expression9 = container21.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression9.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression9.ColumnSpan = null; // default value
    expression9.CustomStyle = null; // default value
    expression9.Example = null; // default value
    expression9.FlexBasis = null; // default value
    expression9.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression9.RowSpan = null; // default value
    expression9.SetStyle(null); // default value
    expression9.SetValue("If(IsLoading or not Meta.HasData, \"–\", FormatDecimal(IntegerToDecimal(SumTotalLots), 0, \".\", \",\"))");
    expression9.Width = ""; // default value
    
    var container22 = container19.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container22.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container22.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container22.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container22.Animate = false; // default value
    container22.ColumnGap = null; // default value
    container22.ColumnSpan = null; // default value
    container22.CustomStyle = null; // default value
    container22.FlexBasis = null; // default value
    container22.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container22.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container22.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container22.Gap = null;
    container22.GridColumns = null; // default value
    container22.GridRows = null; // default value
    container22.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container22.RowGap = null; // default value
    container22.RowSpan = null; // default value
    container22.SetStyle("\"cp-kpi-hint\"");
    container22.SetVisible("True"); // default value
    container22.VisibleWasExpression = false; // default value
    container22.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container22.Width = "(fill parent)"; // default value
    
    var expression10 = container22.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression10.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression10.ColumnSpan = null; // default value
    expression10.CustomStyle = null; // default value
    expression10.Example = null; // default value
    expression10.FlexBasis = null; // default value
    expression10.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression10.RowSpan = null; // default value
    expression10.SetStyle(null); // default value
    expression10.SetValue("\"Across these carparks\"");
    expression10.Width = ""; // default value
    
    var cardInstance3 = container14.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    cardInstance3.CustomStyle = null; // default value
    cardInstance3.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    cardInstance3.SourceBlock = card;
    cardInstance3.SetArgumentValue(extendedClass4, "\"cp-kpi cp-kpi-available\"");
    cardInstance3.SetArgumentValue(usePadding, "True");
    
    var placeholderContentWidget8 = cardInstance3.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    var container23 = placeholderContentWidget8.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container23.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container23.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container23.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container23.Animate = false; // default value
    container23.ColumnGap = null; // default value
    container23.ColumnSpan = null; // default value
    container23.CustomStyle = null; // default value
    container23.FlexBasis = null; // default value
    container23.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container23.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container23.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container23.Gap = null;
    container23.GridColumns = null; // default value
    container23.GridRows = null; // default value
    container23.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container23.RowGap = null; // default value
    container23.RowSpan = null; // default value
    container23.SetStyle(null); // default value
    container23.SetVisible("True"); // default value
    container23.VisibleWasExpression = false; // default value
    container23.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container23.Width = "(fill parent)"; // default value
    
    var container24 = container23.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container24.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container24.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container24.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container24.Animate = false; // default value
    container24.ColumnGap = null; // default value
    container24.ColumnSpan = null; // default value
    container24.CustomStyle = null; // default value
    container24.FlexBasis = null; // default value
    container24.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container24.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container24.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container24.Gap = null;
    container24.GridColumns = null; // default value
    container24.GridRows = null; // default value
    container24.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container24.RowGap = null; // default value
    container24.RowSpan = null; // default value
    container24.SetStyle("\"cp-kpi-label\"");
    container24.SetVisible("True"); // default value
    container24.VisibleWasExpression = false; // default value
    container24.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container24.Width = "(fill parent)"; // default value
    
    var textWidget5 = container24.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget5.CustomStyle = null; // default value
    textWidget5.SetStyleClasses(null); // default value
    textWidget5.Text = "Available lots";
    
    var container25 = container23.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container25.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container25.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container25.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container25.Animate = false; // default value
    container25.ColumnGap = null; // default value
    container25.ColumnSpan = null; // default value
    container25.CustomStyle = null; // default value
    container25.FlexBasis = null; // default value
    container25.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container25.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container25.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container25.Gap = null;
    container25.GridColumns = null; // default value
    container25.GridRows = null; // default value
    container25.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container25.RowGap = null; // default value
    container25.RowSpan = null; // default value
    container25.SetStyle("\"cp-kpi-value\"");
    container25.SetVisible("True"); // default value
    container25.VisibleWasExpression = false; // default value
    container25.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container25.Width = "(fill parent)"; // default value
    
    var expression11 = container25.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression11.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression11.ColumnSpan = null; // default value
    expression11.CustomStyle = null; // default value
    expression11.Example = null; // default value
    expression11.FlexBasis = null; // default value
    expression11.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression11.RowSpan = null; // default value
    expression11.SetStyle(null); // default value
    expression11.SetValue("If(IsLoading or not Meta.HasData, \"–\", FormatDecimal(IntegerToDecimal(SumAvailableLots), 0, \".\", \",\"))");
    expression11.Width = ""; // default value
    
    var container26 = container23.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container26.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container26.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container26.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container26.Animate = false; // default value
    container26.ColumnGap = null; // default value
    container26.ColumnSpan = null; // default value
    container26.CustomStyle = null; // default value
    container26.FlexBasis = null; // default value
    container26.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container26.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container26.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container26.Gap = null;
    container26.GridColumns = null; // default value
    container26.GridRows = null; // default value
    container26.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container26.RowGap = null; // default value
    container26.RowSpan = null; // default value
    container26.SetStyle("\"cp-kpi-hint\"");
    container26.SetVisible("True"); // default value
    container26.VisibleWasExpression = false; // default value
    container26.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container26.Width = "(fill parent)"; // default value
    
    var expression12 = container26.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression12.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression12.ColumnSpan = null; // default value
    expression12.CustomStyle = null; // default value
    expression12.Example = null; // default value
    expression12.FlexBasis = null; // default value
    expression12.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression12.RowSpan = null; // default value
    expression12.SetStyle(null); // default value
    expression12.SetValue("If(SumTotalLots > 0, FormatDecimal(SumAvailableLots * 100.0 / SumTotalLots, 1, \".\", \",\") + \"% of all lots free\", \"No lots reported\")");
    expression12.Width = ""; // default value
    
    var cardInstance4 = container14.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    cardInstance4.CustomStyle = null; // default value
    cardInstance4.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    cardInstance4.SourceBlock = card;
    cardInstance4.SetArgumentValue(extendedClass4, "\"cp-kpi cp-kpi-avg\"");
    cardInstance4.SetArgumentValue(usePadding, "True");
    
    var placeholderContentWidget9 = cardInstance4.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    var container27 = placeholderContentWidget9.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container27.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container27.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container27.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container27.Animate = false; // default value
    container27.ColumnGap = null; // default value
    container27.ColumnSpan = null; // default value
    container27.CustomStyle = null; // default value
    container27.FlexBasis = null; // default value
    container27.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container27.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container27.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container27.Gap = null;
    container27.GridColumns = null; // default value
    container27.GridRows = null; // default value
    container27.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container27.RowGap = null; // default value
    container27.RowSpan = null; // default value
    container27.SetStyle(null); // default value
    container27.SetVisible("True"); // default value
    container27.VisibleWasExpression = false; // default value
    container27.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container27.Width = "(fill parent)"; // default value
    
    var container28 = container27.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container28.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container28.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container28.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container28.Animate = false; // default value
    container28.ColumnGap = null; // default value
    container28.ColumnSpan = null; // default value
    container28.CustomStyle = null; // default value
    container28.FlexBasis = null; // default value
    container28.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container28.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container28.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container28.Gap = null;
    container28.GridColumns = null; // default value
    container28.GridRows = null; // default value
    container28.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container28.RowGap = null; // default value
    container28.RowSpan = null; // default value
    container28.SetStyle("\"cp-kpi-label\"");
    container28.SetVisible("True"); // default value
    container28.VisibleWasExpression = false; // default value
    container28.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container28.Width = "(fill parent)"; // default value
    
    var textWidget6 = container28.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget6.CustomStyle = null; // default value
    textWidget6.SetStyleClasses(null); // default value
    textWidget6.Text = "Average availability";
    
    var container29 = container27.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container29.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container29.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container29.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container29.Animate = false; // default value
    container29.ColumnGap = null; // default value
    container29.ColumnSpan = null; // default value
    container29.CustomStyle = null; // default value
    container29.FlexBasis = null; // default value
    container29.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container29.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container29.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container29.Gap = null;
    container29.GridColumns = null; // default value
    container29.GridRows = null; // default value
    container29.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container29.RowGap = null; // default value
    container29.RowSpan = null; // default value
    container29.SetStyle("\"cp-kpi-value\"");
    container29.SetVisible("True"); // default value
    container29.VisibleWasExpression = false; // default value
    container29.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container29.Width = "(fill parent)"; // default value
    
    var expression13 = container29.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression13.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression13.ColumnSpan = null; // default value
    expression13.CustomStyle = null; // default value
    expression13.Example = null; // default value
    expression13.FlexBasis = null; // default value
    expression13.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression13.RowSpan = null; // default value
    expression13.SetStyle(null); // default value
    expression13.SetValue("If(IsLoading or not Meta.HasData, \"–\", FormatDecimal(AvgPct, 1, \".\", \",\") + \"%\")");
    expression13.Width = ""; // default value
    
    var container30 = container27.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container30.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container30.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container30.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container30.Animate = false; // default value
    container30.ColumnGap = null; // default value
    container30.ColumnSpan = null; // default value
    container30.CustomStyle = null; // default value
    container30.FlexBasis = null; // default value
    container30.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container30.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container30.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container30.Gap = null;
    container30.GridColumns = null; // default value
    container30.GridRows = null; // default value
    container30.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container30.RowGap = null; // default value
    container30.RowSpan = null; // default value
    container30.SetStyle("\"cp-kpi-hint\"");
    container30.SetVisible("True"); // default value
    container30.VisibleWasExpression = false; // default value
    container30.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container30.Width = "(fill parent)"; // default value
    
    var expression14 = container30.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression14.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression14.ColumnSpan = null; // default value
    expression14.CustomStyle = null; // default value
    expression14.Example = null; // default value
    expression14.FlexBasis = null; // default value
    expression14.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression14.RowSpan = null; // default value
    expression14.SetStyle(null); // default value
    expression14.SetValue("\"Mean of each carpark\'s availability\"");
    expression14.Width = ""; // default value
    
    var cardInstance5 = container14.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    cardInstance5.CustomStyle = null; // default value
    cardInstance5.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    cardInstance5.SourceBlock = card;
    cardInstance5.SetArgumentValue(usePadding, "True");
    cardInstance5.SetArgumentValue(extendedClass4, "\"cp-kpi cp-kpi-full\"");
    
    var placeholderContentWidget10 = cardInstance5.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    var container31 = placeholderContentWidget10.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container31.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container31.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container31.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container31.Animate = false; // default value
    container31.ColumnGap = null; // default value
    container31.ColumnSpan = null; // default value
    container31.CustomStyle = null; // default value
    container31.FlexBasis = null; // default value
    container31.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container31.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container31.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container31.Gap = null;
    container31.GridColumns = null; // default value
    container31.GridRows = null; // default value
    container31.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container31.RowGap = null; // default value
    container31.RowSpan = null; // default value
    container31.SetStyle(null); // default value
    container31.SetVisible("True"); // default value
    container31.VisibleWasExpression = false; // default value
    container31.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container31.Width = "(fill parent)"; // default value
    
    var container32 = container31.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container32.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container32.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container32.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container32.Animate = false; // default value
    container32.ColumnGap = null; // default value
    container32.ColumnSpan = null; // default value
    container32.CustomStyle = null; // default value
    container32.FlexBasis = null; // default value
    container32.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container32.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container32.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container32.Gap = null;
    container32.GridColumns = null; // default value
    container32.GridRows = null; // default value
    container32.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container32.RowGap = null; // default value
    container32.RowSpan = null; // default value
    container32.SetStyle("\"cp-kpi-label\"");
    container32.SetVisible("True"); // default value
    container32.VisibleWasExpression = false; // default value
    container32.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container32.Width = "(fill parent)"; // default value
    
    var textWidget7 = container32.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget7.CustomStyle = null; // default value
    textWidget7.SetStyleClasses(null); // default value
    textWidget7.Text = "Full or low";
    
    var container33 = container31.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container33.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container33.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container33.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container33.Animate = false; // default value
    container33.ColumnGap = null; // default value
    container33.ColumnSpan = null; // default value
    container33.CustomStyle = null; // default value
    container33.FlexBasis = null; // default value
    container33.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container33.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container33.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container33.Gap = null;
    container33.GridColumns = null; // default value
    container33.GridRows = null; // default value
    container33.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container33.RowGap = null; // default value
    container33.RowSpan = null; // default value
    container33.SetStyle("\"cp-kpi-value\"");
    container33.SetVisible("True"); // default value
    container33.VisibleWasExpression = false; // default value
    container33.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container33.Width = "(fill parent)"; // default value
    
    var expression15 = container33.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression15.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression15.ColumnSpan = null; // default value
    expression15.CustomStyle = null; // default value
    expression15.Example = null; // default value
    expression15.FlexBasis = null; // default value
    expression15.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression15.RowSpan = null; // default value
    expression15.SetStyle(null); // default value
    expression15.SetValue("If(IsLoading or not Meta.HasData, \"–\", FormatDecimal(IntegerToDecimal(SumFullOrLow), 0, \".\", \",\"))");
    expression15.Width = ""; // default value
    
    var container34 = container31.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container34.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container34.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container34.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container34.Animate = false; // default value
    container34.ColumnGap = null; // default value
    container34.ColumnSpan = null; // default value
    container34.CustomStyle = null; // default value
    container34.FlexBasis = null; // default value
    container34.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container34.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container34.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container34.Gap = null;
    container34.GridColumns = null; // default value
    container34.GridRows = null; // default value
    container34.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container34.RowGap = null; // default value
    container34.RowSpan = null; // default value
    container34.SetStyle("\"cp-kpi-hint\"");
    container34.SetVisible("True"); // default value
    container34.VisibleWasExpression = false; // default value
    container34.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container34.Width = "(fill parent)"; // default value
    
    var expression16 = container34.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression16.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression16.ColumnSpan = null; // default value
    expression16.CustomStyle = null; // default value
    expression16.Example = null; // default value
    expression16.FlexBasis = null; // default value
    expression16.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression16.RowSpan = null; // default value
    expression16.SetStyle(null); // default value
    expression16.SetValue("\"Carparks with few or no lots left\"");
    expression16.Width = ""; // default value
    
    var extendedProperty7 = container14.CreateExtendedProperty();
    extendedProperty7.Property = "role";
    extendedProperty7.SetValue("\"region\"");
    
    var extendedProperty8 = container14.CreateExtendedProperty();
    extendedProperty8.Property = "aria-label";
    extendedProperty8.SetValue("\"Summary metrics\"");
    
    var container35 = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container35.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container35.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container35.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container35.Animate = false; // default value
    container35.ColumnGap = null; // default value
    container35.ColumnSpan = null; // default value
    container35.CustomStyle = null; // default value
    container35.FlexBasis = null; // default value
    container35.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container35.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container35.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container35.Gap = null;
    container35.GridColumns = null; // default value
    container35.GridRows = null; // default value
    container35.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container35.RowGap = null; // default value
    container35.RowSpan = null; // default value
    container35.SetStyle("\"cp-kpi-scope\"");
    container35.SetVisible("True"); // default value
    container35.VisibleWasExpression = false; // default value
    container35.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container35.Width = "(fill parent)"; // default value
    
    var expression17 = container35.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression17.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression17.ColumnSpan = null; // default value
    expression17.CustomStyle = null; // default value
    expression17.Example = null; // default value
    expression17.FlexBasis = null; // default value
    expression17.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression17.RowSpan = null; // default value
    expression17.SetStyle(null); // default value
    expression17.SetValue("\"Figures use \" + If(LotTypeFilter = NullTextIdentifier(), \"each carpark\'s main lot type (car lots where available)\", \"the selected lot type only\") + \". Different lot types are never added together. Inconsistent readings are excluded.\"");
    expression17.Width = ""; // default value
    
    var cardInstance6 = container2.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    cardInstance6.CustomStyle = null; // default value
    cardInstance6.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    cardInstance6.SourceBlock = card;
    cardInstance6.SetArgumentValue(extendedClass4, "\"cp-filters\"");
    cardInstance6.SetArgumentValue(usePadding, "True");
    
    var placeholderContentWidget11 = cardInstance6.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    var container36 = placeholderContentWidget11.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container36.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container36.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container36.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container36.Animate = false; // default value
    container36.ColumnGap = null; // default value
    container36.ColumnSpan = null; // default value
    container36.CustomStyle = null; // default value
    container36.FlexBasis = null; // default value
    container36.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container36.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container36.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container36.Gap = null;
    container36.GridColumns = null; // default value
    container36.GridRows = null; // default value
    container36.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container36.RowGap = null; // default value
    container36.RowSpan = null; // default value
    container36.SetStyle("\"cp-filter-grid\"");
    container36.SetVisible("True"); // default value
    container36.VisibleWasExpression = false; // default value
    container36.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container36.Width = "(fill parent)"; // default value
    
    var container37 = container36.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container37.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container37.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container37.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container37.Animate = false; // default value
    container37.ColumnGap = null; // default value
    container37.ColumnSpan = null; // default value
    container37.CustomStyle = null; // default value
    container37.FlexBasis = null; // default value
    container37.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container37.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container37.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container37.Gap = null;
    container37.GridColumns = null; // default value
    container37.GridRows = null; // default value
    container37.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container37.RowGap = null; // default value
    container37.RowSpan = null; // default value
    container37.SetStyle("\"cp-field cp-field-search\"");
    container37.SetVisible("True"); // default value
    container37.VisibleWasExpression = false; // default value
    container37.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container37.Width = "(fill parent)"; // default value
    
    var label = container37.CreateWidget<ServiceStudio.Plugin.NRWidgets.ILabel>();
    label.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    label.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    label.ColumnGap = null; // default value
    label.ColumnSpan = null; // default value
    label.CustomStyle = null; // default value
    label.FlexBasis = null; // default value
    label.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    label.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    label.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    label.Gap = null;
    label.GridColumns = null; // default value
    label.GridRows = null; // default value
    label.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    label.RowGap = null; // default value
    label.RowSpan = null; // default value
    label.SetStyle(null); // default value
    label.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    label.Width = "(fill parent)"; // default value
    
    var textWidget8 = (OutSystems.Model.UI.Mobile.Widgets.ITextWidget)label.Widgets.First();
    textWidget8.CustomStyle = null; // default value
    textWidget8.Name = null; // default value
    textWidget8.SetStyleClasses(null); // default value
    textWidget8.Text = "Search carparks";
    
    var searchInput = container37.CreateWidget<ServiceStudio.Plugin.NRWidgets.IInput>("SearchInput");
    searchInput.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    searchInput.ColumnSpan = null; // default value
    searchInput.CustomStyle = null; // default value
    searchInput.SetEnabled("True");
    searchInput.FlexBasis = null; // default value
    searchInput.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    searchInput.InputType = ServiceStudio.Plugin.NRWidgets.Enumerations.InputType.Search;
    searchInput.SetMandatory("False");
    searchInput.MaxLength = 100;
    searchInput.SetPrompt("\"Carpark number, address or type\"");
    searchInput.RowSpan = null; // default value
    searchInput.SetStyle("\"form-control\""); // default value
    searchInput.SetVariable("SearchText");
    searchInput.Width = "(fill parent)"; // default value
    var builtinEvent2 = searchInput.OnChange;
    builtinEvent2.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent2.Destination = applyFilters;
    builtinEvent2.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    label.TargetWidget = searchInput;
    
    var container38 = container36.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container38.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container38.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container38.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container38.Animate = false; // default value
    container38.ColumnGap = null; // default value
    container38.ColumnSpan = null; // default value
    container38.CustomStyle = null; // default value
    container38.FlexBasis = null; // default value
    container38.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container38.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container38.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container38.Gap = null;
    container38.GridColumns = null; // default value
    container38.GridRows = null; // default value
    container38.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container38.RowGap = null; // default value
    container38.RowSpan = null; // default value
    container38.SetStyle("\"cp-field cp-field-status\"");
    container38.SetVisible("True"); // default value
    container38.VisibleWasExpression = false; // default value
    container38.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container38.Width = "(fill parent)"; // default value
    
    var label2 = container38.CreateWidget<ServiceStudio.Plugin.NRWidgets.ILabel>();
    label2.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    label2.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    label2.ColumnGap = null; // default value
    label2.ColumnSpan = null; // default value
    label2.CustomStyle = null; // default value
    label2.FlexBasis = null; // default value
    label2.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    label2.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    label2.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    label2.Gap = null;
    label2.GridColumns = null; // default value
    label2.GridRows = null; // default value
    label2.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    label2.RowGap = null; // default value
    label2.RowSpan = null; // default value
    label2.SetStyle(null); // default value
    label2.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    label2.Width = "(fill parent)"; // default value
    
    var textWidget9 = (OutSystems.Model.UI.Mobile.Widgets.ITextWidget)label2.Widgets.First();
    textWidget9.CustomStyle = null; // default value
    textWidget9.Name = null; // default value
    textWidget9.SetStyleClasses(null); // default value
    textWidget9.Text = "Availability status";
    
    var statusDropdown = container38.CreateWidget<ServiceStudio.Plugin.NRWidgets.IDropdown>("StatusDropdown");
    statusDropdown.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    statusDropdown.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    statusDropdown.ColumnGap = null; // default value
    statusDropdown.ColumnSpan = null; // default value
    statusDropdown.CustomStyle = null; // default value
    statusDropdown.DropdownMode = ServiceStudio.Plugin.NRWidgets.Enumerations.DropdownMode.Text;
    statusDropdown.SetEmptyValue("\"All statuses\"");
    statusDropdown.SetEnabled("True");
    statusDropdown.FlexBasis = null; // default value
    statusDropdown.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    statusDropdown.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    statusDropdown.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    statusDropdown.Gap = null;
    statusDropdown.GridColumns = null; // default value
    statusDropdown.GridRows = null; // default value
    statusDropdown.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    statusDropdown.SetLabels("AvailabilityStatus.Label");
    statusDropdown.SetList("GetStatusOptions.List");
    statusDropdown.SetMandatory("False");
    statusDropdown.RowGap = null; // default value
    statusDropdown.RowSpan = null; // default value
    statusDropdown.SetStyle("\"dropdown\""); // default value
    statusDropdown.SetValues("AvailabilityStatus.Id");
    statusDropdown.SetVariable("StatusFilter");
    statusDropdown.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    statusDropdown.Width = ""; // default value
    var builtinEvent3 = statusDropdown.OnChange;
    builtinEvent3.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent3.Destination = applyFilters;
    builtinEvent3.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    label2.TargetWidget = statusDropdown;
    
    var container39 = container36.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container39.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container39.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container39.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container39.Animate = false; // default value
    container39.ColumnGap = null; // default value
    container39.ColumnSpan = null; // default value
    container39.CustomStyle = null; // default value
    container39.FlexBasis = null; // default value
    container39.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container39.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container39.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container39.Gap = null;
    container39.GridColumns = null; // default value
    container39.GridRows = null; // default value
    container39.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container39.RowGap = null; // default value
    container39.RowSpan = null; // default value
    container39.SetStyle("\"cp-field cp-field-lot\"");
    container39.SetVisible("True"); // default value
    container39.VisibleWasExpression = false; // default value
    container39.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container39.Width = "(fill parent)"; // default value
    
    var label3 = container39.CreateWidget<ServiceStudio.Plugin.NRWidgets.ILabel>();
    label3.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    label3.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    label3.ColumnGap = null; // default value
    label3.ColumnSpan = null; // default value
    label3.CustomStyle = null; // default value
    label3.FlexBasis = null; // default value
    label3.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    label3.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    label3.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    label3.Gap = null;
    label3.GridColumns = null; // default value
    label3.GridRows = null; // default value
    label3.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    label3.RowGap = null; // default value
    label3.RowSpan = null; // default value
    label3.SetStyle(null); // default value
    label3.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    label3.Width = "(fill parent)"; // default value
    
    var textWidget10 = (OutSystems.Model.UI.Mobile.Widgets.ITextWidget)label3.Widgets.First();
    textWidget10.CustomStyle = null; // default value
    textWidget10.Name = null; // default value
    textWidget10.SetStyleClasses(null); // default value
    textWidget10.Text = "Lot type";
    
    var lotTypeDropdown = container39.CreateWidget<ServiceStudio.Plugin.NRWidgets.IDropdown>("LotTypeDropdown");
    lotTypeDropdown.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    lotTypeDropdown.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    lotTypeDropdown.ColumnGap = null; // default value
    lotTypeDropdown.ColumnSpan = null; // default value
    lotTypeDropdown.CustomStyle = null; // default value
    lotTypeDropdown.DropdownMode = ServiceStudio.Plugin.NRWidgets.Enumerations.DropdownMode.Text;
    lotTypeDropdown.SetEmptyValue("\"Main lot type (all carparks)\"");
    lotTypeDropdown.SetEnabled("True");
    lotTypeDropdown.FlexBasis = null; // default value
    lotTypeDropdown.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    lotTypeDropdown.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    lotTypeDropdown.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    lotTypeDropdown.Gap = null;
    lotTypeDropdown.GridColumns = null; // default value
    lotTypeDropdown.GridRows = null; // default value
    lotTypeDropdown.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    lotTypeDropdown.SetLabels("LotType.Label");
    lotTypeDropdown.SetList("GetLotTypeOptions.List");
    lotTypeDropdown.SetMandatory("False");
    lotTypeDropdown.RowGap = null; // default value
    lotTypeDropdown.RowSpan = null; // default value
    lotTypeDropdown.SetStyle("\"dropdown\""); // default value
    lotTypeDropdown.SetValues("LotType.Id");
    lotTypeDropdown.SetVariable("LotTypeFilter");
    lotTypeDropdown.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    lotTypeDropdown.Width = ""; // default value
    var builtinEvent4 = lotTypeDropdown.OnChange;
    builtinEvent4.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent4.Destination = applyFilters;
    builtinEvent4.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    label3.TargetWidget = lotTypeDropdown;
    
    var container40 = container36.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container40.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container40.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container40.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container40.Animate = false; // default value
    container40.ColumnGap = null; // default value
    container40.ColumnSpan = null; // default value
    container40.CustomStyle = null; // default value
    container40.FlexBasis = null; // default value
    container40.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container40.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container40.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container40.Gap = null;
    container40.GridColumns = null; // default value
    container40.GridRows = null; // default value
    container40.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container40.RowGap = null; // default value
    container40.RowSpan = null; // default value
    container40.SetStyle("\"cp-field cp-field-min\"");
    container40.SetVisible("True"); // default value
    container40.VisibleWasExpression = false; // default value
    container40.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container40.Width = "(fill parent)"; // default value
    
    var label4 = container40.CreateWidget<ServiceStudio.Plugin.NRWidgets.ILabel>();
    label4.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    label4.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    label4.ColumnGap = null; // default value
    label4.ColumnSpan = null; // default value
    label4.CustomStyle = null; // default value
    label4.FlexBasis = null; // default value
    label4.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    label4.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    label4.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    label4.Gap = null;
    label4.GridColumns = null; // default value
    label4.GridRows = null; // default value
    label4.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    label4.RowGap = null; // default value
    label4.RowSpan = null; // default value
    label4.SetStyle(null); // default value
    label4.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    label4.Width = "(fill parent)"; // default value
    
    var textWidget11 = (OutSystems.Model.UI.Mobile.Widgets.ITextWidget)label4.Widgets.First();
    textWidget11.CustomStyle = null; // default value
    textWidget11.Name = null; // default value
    textWidget11.SetStyleClasses(null); // default value
    textWidget11.Text = "Minimum available lots";
    
    var minAvailableInput = container40.CreateWidget<ServiceStudio.Plugin.NRWidgets.IInput>("MinAvailableInput");
    minAvailableInput.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    minAvailableInput.ColumnSpan = null; // default value
    minAvailableInput.CustomStyle = null; // default value
    minAvailableInput.SetEnabled("True");
    minAvailableInput.FlexBasis = null; // default value
    minAvailableInput.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    minAvailableInput.InputType = ServiceStudio.Plugin.NRWidgets.Enumerations.InputType.Number;
    minAvailableInput.SetMandatory("False");
    minAvailableInput.MaxLength = null;
    minAvailableInput.SetPrompt("\"0\"");
    minAvailableInput.RowSpan = null; // default value
    minAvailableInput.SetStyle("\"form-control\""); // default value
    minAvailableInput.SetVariable("MinAvailable");
    minAvailableInput.Width = "(fill parent)"; // default value
    var builtinEvent5 = minAvailableInput.OnChange;
    builtinEvent5.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent5.Destination = applyFilters;
    builtinEvent5.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    label4.TargetWidget = minAvailableInput;
    
    var container41 = container36.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container41.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container41.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container41.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container41.Animate = false; // default value
    container41.ColumnGap = null; // default value
    container41.ColumnSpan = null; // default value
    container41.CustomStyle = null; // default value
    container41.FlexBasis = null; // default value
    container41.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container41.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container41.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container41.Gap = null;
    container41.GridColumns = null; // default value
    container41.GridRows = null; // default value
    container41.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container41.RowGap = null; // default value
    container41.RowSpan = null; // default value
    container41.SetStyle("\"cp-field cp-field-sort\"");
    container41.SetVisible("True"); // default value
    container41.VisibleWasExpression = false; // default value
    container41.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container41.Width = "(fill parent)"; // default value
    
    var label5 = container41.CreateWidget<ServiceStudio.Plugin.NRWidgets.ILabel>();
    label5.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    label5.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    label5.ColumnGap = null; // default value
    label5.ColumnSpan = null; // default value
    label5.CustomStyle = null; // default value
    label5.FlexBasis = null; // default value
    label5.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    label5.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    label5.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    label5.Gap = null;
    label5.GridColumns = null; // default value
    label5.GridRows = null; // default value
    label5.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    label5.RowGap = null; // default value
    label5.RowSpan = null; // default value
    label5.SetStyle(null); // default value
    label5.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    label5.Width = "(fill parent)"; // default value
    
    var textWidget12 = (OutSystems.Model.UI.Mobile.Widgets.ITextWidget)label5.Widgets.First();
    textWidget12.CustomStyle = null; // default value
    textWidget12.Name = null; // default value
    textWidget12.SetStyleClasses(null); // default value
    textWidget12.Text = "Sort by";
    
    var sortDropdown = container41.CreateWidget<ServiceStudio.Plugin.NRWidgets.IDropdown>("SortDropdown");
    sortDropdown.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    sortDropdown.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    sortDropdown.ColumnGap = null; // default value
    sortDropdown.ColumnSpan = null; // default value
    sortDropdown.CustomStyle = null; // default value
    sortDropdown.DropdownMode = ServiceStudio.Plugin.NRWidgets.Enumerations.DropdownMode.Text;
    sortDropdown.SetEmptyValue(null); // default value
    sortDropdown.SetEnabled("True");
    sortDropdown.FlexBasis = null; // default value
    sortDropdown.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    sortDropdown.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    sortDropdown.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    sortDropdown.Gap = null;
    sortDropdown.GridColumns = null; // default value
    sortDropdown.GridRows = null; // default value
    sortDropdown.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    sortDropdown.SetLabels("SortOption.Label");
    sortDropdown.SetList("GetSortOptions.List");
    sortDropdown.SetMandatory("False");
    sortDropdown.RowGap = null; // default value
    sortDropdown.RowSpan = null; // default value
    sortDropdown.SetStyle("\"dropdown\""); // default value
    sortDropdown.SetValues("SortOption.Id");
    sortDropdown.SetVariable("SortBy");
    sortDropdown.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    sortDropdown.Width = ""; // default value
    var builtinEvent6 = sortDropdown.OnChange;
    builtinEvent6.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent6.Destination = applyFilters;
    builtinEvent6.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    label5.TargetWidget = sortDropdown;
    
    var container42 = container36.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container42.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container42.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container42.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container42.Animate = false; // default value
    container42.ColumnGap = null; // default value
    container42.ColumnSpan = null; // default value
    container42.CustomStyle = null; // default value
    container42.FlexBasis = null; // default value
    container42.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container42.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container42.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container42.Gap = null;
    container42.GridColumns = null; // default value
    container42.GridRows = null; // default value
    container42.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container42.RowGap = null; // default value
    container42.RowSpan = null; // default value
    container42.SetStyle("\"cp-field cp-field-clear\"");
    container42.SetVisible("True"); // default value
    container42.VisibleWasExpression = false; // default value
    container42.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container42.Width = "(fill parent)"; // default value
    
    var clearFiltersButton = container42.CreateWidget<ServiceStudio.Plugin.NRWidgets.IButton>("ClearFiltersButton");
    /*** Deleting Widgets that are created by default when instantiating an IButton ***/
    clearFiltersButton.Widgets.ToList().ForEach(x => x.Delete());
    clearFiltersButton.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    clearFiltersButton.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    clearFiltersButton.ColumnGap = null; // default value
    clearFiltersButton.ColumnSpan = null; // default value
    clearFiltersButton.SetConfirmationMessage(null); // default value
    clearFiltersButton.CustomStyle = null; // default value
    clearFiltersButton.SetEnabled("True");
    clearFiltersButton.FlexBasis = null; // default value
    clearFiltersButton.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    clearFiltersButton.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    clearFiltersButton.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    clearFiltersButton.Gap = null;
    clearFiltersButton.GridColumns = null; // default value
    clearFiltersButton.GridRows = null; // default value
    clearFiltersButton.IsDefault = false; // default value
    clearFiltersButton.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    clearFiltersButton.RowGap = null; // default value
    clearFiltersButton.RowSpan = null; // default value
    clearFiltersButton.SetStyle("\"btn cp-btn-secondary\"");
    clearFiltersButton.SetVisible("True"); // default value
    clearFiltersButton.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    clearFiltersButton.Width = ""; // default value
    
    var expression18 = clearFiltersButton.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression18.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression18.ColumnSpan = null; // default value
    expression18.CustomStyle = null; // default value
    expression18.Example = null; // default value
    expression18.FlexBasis = null; // default value
    expression18.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression18.RowSpan = null; // default value
    expression18.SetStyle(null); // default value
    expression18.SetValue("\"Clear filters\"");
    expression18.Width = ""; // default value
    var builtinEvent7 = clearFiltersButton.OnClick;
    builtinEvent7.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent7.Destination = clearFilters;
    builtinEvent7.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    
    var extendedProperty9 = container36.CreateExtendedProperty();
    extendedProperty9.Property = "role";
    extendedProperty9.SetValue("\"search\"");
    
    var extendedProperty10 = container36.CreateExtendedProperty();
    extendedProperty10.Property = "aria-label";
    extendedProperty10.SetValue("\"Search and filter carparks\"");
    
    var container43 = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container43.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container43.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container43.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container43.Animate = false; // default value
    container43.ColumnGap = null; // default value
    container43.ColumnSpan = null; // default value
    container43.CustomStyle = null; // default value
    container43.FlexBasis = null; // default value
    container43.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container43.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container43.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container43.Gap = null;
    container43.GridColumns = null; // default value
    container43.GridRows = null; // default value
    container43.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container43.RowGap = null; // default value
    container43.RowSpan = null; // default value
    container43.SetStyle("\"cp-legend\"");
    container43.SetVisible("True"); // default value
    container43.VisibleWasExpression = false; // default value
    container43.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container43.Width = "(fill parent)"; // default value
    
    var container44 = container43.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container44.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container44.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container44.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container44.Animate = false; // default value
    container44.ColumnGap = null; // default value
    container44.ColumnSpan = null; // default value
    container44.CustomStyle = null; // default value
    container44.FlexBasis = null; // default value
    container44.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container44.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container44.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container44.Gap = null;
    container44.GridColumns = null; // default value
    container44.GridRows = null; // default value
    container44.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container44.RowGap = null; // default value
    container44.RowSpan = null; // default value
    container44.SetStyle("\"cp-legend-title\"");
    container44.SetVisible("True"); // default value
    container44.VisibleWasExpression = false; // default value
    container44.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container44.Width = "(fill parent)"; // default value
    
    var textWidget13 = container44.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget13.CustomStyle = null; // default value
    textWidget13.SetStyleClasses(null); // default value
    textWidget13.Text = "Status bands";
    
    var container45 = container43.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container45.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container45.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container45.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container45.Animate = false; // default value
    container45.ColumnGap = null; // default value
    container45.ColumnSpan = null; // default value
    container45.CustomStyle = null; // default value
    container45.FlexBasis = null; // default value
    container45.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container45.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container45.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container45.Gap = null;
    container45.GridColumns = null; // default value
    container45.GridRows = null; // default value
    container45.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container45.RowGap = null; // default value
    container45.RowSpan = null; // default value
    container45.SetStyle("\"cp-legend-text\"");
    container45.SetVisible("True"); // default value
    container45.VisibleWasExpression = false; // default value
    container45.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container45.Width = "(fill parent)"; // default value
    
    var expression19 = container45.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression19.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression19.ColumnSpan = null; // default value
    expression19.CustomStyle = null; // default value
    expression19.Example = null; // default value
    expression19.FlexBasis = null; // default value
    expression19.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression19.RowSpan = null; // default value
    expression19.SetStyle(null); // default value
    expression19.SetValue("If(Meta.ThresholdSummary = \"\", \"Status bands are shown once data has loaded.\", Meta.ThresholdSummary)");
    expression19.Width = ""; // default value
    
    var container46 = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container46.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container46.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container46.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container46.Animate = false; // default value
    container46.ColumnGap = null; // default value
    container46.ColumnSpan = null; // default value
    container46.CustomStyle = null; // default value
    container46.FlexBasis = null; // default value
    container46.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container46.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container46.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container46.Gap = null;
    container46.GridColumns = null; // default value
    container46.GridRows = null; // default value
    container46.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container46.RowGap = null; // default value
    container46.RowSpan = null; // default value
    container46.SetStyle("\"cp-results-head\"");
    container46.SetVisible("True"); // default value
    container46.VisibleWasExpression = false; // default value
    container46.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container46.Width = "(fill parent)"; // default value
    
    var container47 = container46.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container47.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container47.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container47.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container47.Animate = false; // default value
    container47.ColumnGap = null; // default value
    container47.ColumnSpan = null; // default value
    container47.CustomStyle = null; // default value
    container47.FlexBasis = null; // default value
    container47.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container47.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container47.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container47.Gap = null;
    container47.GridColumns = null; // default value
    container47.GridRows = null; // default value
    container47.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container47.RowGap = null; // default value
    container47.RowSpan = null; // default value
    container47.SetStyle("\"cp-section-title\"");
    container47.SetVisible("True"); // default value
    container47.VisibleWasExpression = false; // default value
    container47.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container47.Width = "(fill parent)"; // default value
    
    var textWidget14 = container47.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget14.CustomStyle = null; // default value
    textWidget14.SetStyleClasses(null); // default value
    textWidget14.Text = "Carparks";
    
    var extendedProperty11 = container47.CreateExtendedProperty();
    extendedProperty11.Property = "role";
    extendedProperty11.SetValue("\"heading\"");
    
    var extendedProperty12 = container47.CreateExtendedProperty();
    extendedProperty12.Property = "aria-level";
    extendedProperty12.SetValue("\"2\"");
    
    var container48 = container46.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container48.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container48.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container48.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container48.Animate = false; // default value
    container48.ColumnGap = null; // default value
    container48.ColumnSpan = null; // default value
    container48.CustomStyle = null; // default value
    container48.FlexBasis = null; // default value
    container48.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container48.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container48.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container48.Gap = null;
    container48.GridColumns = null; // default value
    container48.GridRows = null; // default value
    container48.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container48.RowGap = null; // default value
    container48.RowSpan = null; // default value
    container48.SetStyle("\"cp-results-count\"");
    container48.SetVisible("True"); // default value
    container48.VisibleWasExpression = false; // default value
    container48.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container48.Width = "(fill parent)"; // default value
    
    var expression20 = container48.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression20.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression20.ColumnSpan = null; // default value
    expression20.CustomStyle = null; // default value
    expression20.Example = null; // default value
    expression20.FlexBasis = null; // default value
    expression20.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression20.RowSpan = null; // default value
    expression20.SetStyle(null); // default value
    expression20.SetValue("If(IsLoading, \"Loading carparks…\", \"Showing \" + FormatDecimal(IntegerToDecimal(PageRows.Length), 0, \".\", \",\") + \" of \" + FormatDecimal(IntegerToDecimal(FilteredRows.Length), 0, \".\", \",\") + If(FilteredRows.Length = 1, \" carpark\", \" carparks\") + If(FilteredRows.Length < SumCarparks, \" (filtered from \" + FormatDecimal(IntegerToDecimal(SumCarparks), 0, \".\", \",\") + \")\", \"\"))");
    expression20.Width = ""; // default value
    
    var extendedProperty13 = container48.CreateExtendedProperty();
    extendedProperty13.Property = "role";
    extendedProperty13.SetValue("\"status\"");
    
    var extendedProperty14 = container48.CreateExtendedProperty();
    extendedProperty14.Property = "aria-live";
    extendedProperty14.SetValue("\"polite\"");
    
    var ifWidget4 = container2.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget4.Animate = false; // default value
    ifWidget4.SetCondition("IsLoading");
    ifWidget4.CustomStyle = null; // default value
    ifWidget4.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container49 = ifWidget4.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container49.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container49.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container49.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container49.Animate = false; // default value
    container49.ColumnGap = null; // default value
    container49.ColumnSpan = null; // default value
    container49.CustomStyle = null; // default value
    container49.FlexBasis = null; // default value
    container49.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container49.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container49.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container49.Gap = null;
    container49.GridColumns = null; // default value
    container49.GridRows = null; // default value
    container49.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container49.RowGap = null; // default value
    container49.RowSpan = null; // default value
    container49.SetStyle("\"cp-loading\"");
    container49.SetVisible("True"); // default value
    container49.VisibleWasExpression = false; // default value
    container49.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container49.Width = "(fill parent)"; // default value
    
    var container50 = container49.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container50.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container50.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container50.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container50.Animate = false; // default value
    container50.ColumnGap = null; // default value
    container50.ColumnSpan = null; // default value
    container50.CustomStyle = null; // default value
    container50.FlexBasis = null; // default value
    container50.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container50.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container50.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container50.Gap = null;
    container50.GridColumns = null; // default value
    container50.GridRows = null; // default value
    container50.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container50.RowGap = null; // default value
    container50.RowSpan = null; // default value
    container50.SetStyle("\"cp-sr-only\"");
    container50.SetVisible("True"); // default value
    container50.VisibleWasExpression = false; // default value
    container50.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container50.Width = "(fill parent)"; // default value
    
    var textWidget15 = container50.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget15.CustomStyle = null; // default value
    textWidget15.SetStyleClasses(null); // default value
    textWidget15.Text = "Loading carpark availability";
    
    var container51 = container49.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container51.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container51.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container51.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container51.Animate = false; // default value
    container51.ColumnGap = null; // default value
    container51.ColumnSpan = null; // default value
    container51.CustomStyle = null; // default value
    container51.FlexBasis = null; // default value
    container51.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container51.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container51.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container51.Gap = null;
    container51.GridColumns = null; // default value
    container51.GridRows = null; // default value
    container51.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container51.RowGap = null; // default value
    container51.RowSpan = null; // default value
    container51.SetStyle("\"cp-skeleton-grid\"");
    container51.SetVisible("True"); // default value
    container51.VisibleWasExpression = false; // default value
    container51.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container51.Width = "(fill parent)"; // default value
    
    var container52 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container52.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container52.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container52.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container52.Animate = false; // default value
    container52.ColumnGap = null; // default value
    container52.ColumnSpan = null; // default value
    container52.CustomStyle = null; // default value
    container52.FlexBasis = null; // default value
    container52.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container52.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container52.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container52.Gap = null;
    container52.GridColumns = null; // default value
    container52.GridRows = null; // default value
    container52.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container52.RowGap = null; // default value
    container52.RowSpan = null; // default value
    container52.SetStyle("\"cp-skeleton\"");
    container52.SetVisible("True"); // default value
    container52.VisibleWasExpression = false; // default value
    container52.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container52.Width = "(fill parent)"; // default value
    
    var container53 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container53.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container53.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container53.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container53.Animate = false; // default value
    container53.ColumnGap = null; // default value
    container53.ColumnSpan = null; // default value
    container53.CustomStyle = null; // default value
    container53.FlexBasis = null; // default value
    container53.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container53.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container53.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container53.Gap = null;
    container53.GridColumns = null; // default value
    container53.GridRows = null; // default value
    container53.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container53.RowGap = null; // default value
    container53.RowSpan = null; // default value
    container53.SetStyle("\"cp-skeleton\"");
    container53.SetVisible("True"); // default value
    container53.VisibleWasExpression = false; // default value
    container53.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container53.Width = "(fill parent)"; // default value
    
    var container54 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container54.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container54.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container54.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container54.Animate = false; // default value
    container54.ColumnGap = null; // default value
    container54.ColumnSpan = null; // default value
    container54.CustomStyle = null; // default value
    container54.FlexBasis = null; // default value
    container54.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container54.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container54.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container54.Gap = null;
    container54.GridColumns = null; // default value
    container54.GridRows = null; // default value
    container54.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container54.RowGap = null; // default value
    container54.RowSpan = null; // default value
    container54.SetStyle("\"cp-skeleton\"");
    container54.SetVisible("True"); // default value
    container54.VisibleWasExpression = false; // default value
    container54.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container54.Width = "(fill parent)"; // default value
    
    var container55 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container55.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container55.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container55.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container55.Animate = false; // default value
    container55.ColumnGap = null; // default value
    container55.ColumnSpan = null; // default value
    container55.CustomStyle = null; // default value
    container55.FlexBasis = null; // default value
    container55.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container55.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container55.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container55.Gap = null;
    container55.GridColumns = null; // default value
    container55.GridRows = null; // default value
    container55.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container55.RowGap = null; // default value
    container55.RowSpan = null; // default value
    container55.SetStyle("\"cp-skeleton\"");
    container55.SetVisible("True"); // default value
    container55.VisibleWasExpression = false; // default value
    container55.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container55.Width = "(fill parent)"; // default value
    
    var container56 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container56.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container56.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container56.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container56.Animate = false; // default value
    container56.ColumnGap = null; // default value
    container56.ColumnSpan = null; // default value
    container56.CustomStyle = null; // default value
    container56.FlexBasis = null; // default value
    container56.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container56.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container56.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container56.Gap = null;
    container56.GridColumns = null; // default value
    container56.GridRows = null; // default value
    container56.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container56.RowGap = null; // default value
    container56.RowSpan = null; // default value
    container56.SetStyle("\"cp-skeleton\"");
    container56.SetVisible("True"); // default value
    container56.VisibleWasExpression = false; // default value
    container56.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container56.Width = "(fill parent)"; // default value
    
    var container57 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container57.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container57.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container57.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container57.Animate = false; // default value
    container57.ColumnGap = null; // default value
    container57.ColumnSpan = null; // default value
    container57.CustomStyle = null; // default value
    container57.FlexBasis = null; // default value
    container57.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container57.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container57.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container57.Gap = null;
    container57.GridColumns = null; // default value
    container57.GridRows = null; // default value
    container57.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container57.RowGap = null; // default value
    container57.RowSpan = null; // default value
    container57.SetStyle("\"cp-skeleton\"");
    container57.SetVisible("True"); // default value
    container57.VisibleWasExpression = false; // default value
    container57.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container57.Width = "(fill parent)"; // default value
    
    var container58 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container58.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container58.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container58.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container58.Animate = false; // default value
    container58.ColumnGap = null; // default value
    container58.ColumnSpan = null; // default value
    container58.CustomStyle = null; // default value
    container58.FlexBasis = null; // default value
    container58.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container58.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container58.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container58.Gap = null;
    container58.GridColumns = null; // default value
    container58.GridRows = null; // default value
    container58.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container58.RowGap = null; // default value
    container58.RowSpan = null; // default value
    container58.SetStyle("\"cp-skeleton\"");
    container58.SetVisible("True"); // default value
    container58.VisibleWasExpression = false; // default value
    container58.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container58.Width = "(fill parent)"; // default value
    
    var container59 = container51.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container59.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container59.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container59.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container59.Animate = false; // default value
    container59.ColumnGap = null; // default value
    container59.ColumnSpan = null; // default value
    container59.CustomStyle = null; // default value
    container59.FlexBasis = null; // default value
    container59.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container59.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container59.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container59.Gap = null;
    container59.GridColumns = null; // default value
    container59.GridRows = null; // default value
    container59.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container59.RowGap = null; // default value
    container59.RowSpan = null; // default value
    container59.SetStyle("\"cp-skeleton\"");
    container59.SetVisible("True"); // default value
    container59.VisibleWasExpression = false; // default value
    container59.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container59.Width = "(fill parent)"; // default value
    
    var extendedProperty15 = container51.CreateExtendedProperty();
    extendedProperty15.Property = "aria-hidden";
    extendedProperty15.SetValue("\"true\"");
    
    var extendedProperty16 = container49.CreateExtendedProperty();
    extendedProperty16.Property = "role";
    extendedProperty16.SetValue("\"status\"");
    
    var container60 = ifWidget4.FalseBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container60.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container60.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container60.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container60.Animate = false; // default value
    container60.ColumnGap = null; // default value
    container60.ColumnSpan = null; // default value
    container60.CustomStyle = null; // default value
    container60.FlexBasis = null; // default value
    container60.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container60.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container60.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container60.Gap = null;
    container60.GridColumns = null; // default value
    container60.GridRows = null; // default value
    container60.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container60.RowGap = null; // default value
    container60.RowSpan = null; // default value
    container60.SetStyle(null); // default value
    container60.SetVisible("True"); // default value
    container60.VisibleWasExpression = false; // default value
    container60.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container60.Width = "(fill parent)"; // default value
    
    var ifWidget5 = container60.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget5.Animate = false; // default value
    ifWidget5.SetCondition("FilteredRows.Empty");
    ifWidget5.CustomStyle = null; // default value
    ifWidget5.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container61 = ifWidget5.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container61.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container61.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container61.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container61.Animate = false; // default value
    container61.ColumnGap = null; // default value
    container61.ColumnSpan = null; // default value
    container61.CustomStyle = null; // default value
    container61.FlexBasis = null; // default value
    container61.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container61.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container61.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container61.Gap = null;
    container61.GridColumns = null; // default value
    container61.GridRows = null; // default value
    container61.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container61.RowGap = null; // default value
    container61.RowSpan = null; // default value
    container61.SetStyle(null); // default value
    container61.SetVisible("True"); // default value
    container61.VisibleWasExpression = false; // default value
    container61.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container61.Width = "(fill parent)"; // default value
    
    var blankSlateInstance = container61.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    blankSlateInstance.CustomStyle = null; // default value
    blankSlateInstance.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    var blankSlate = content.Nodes.OfType<OutSystems.Model.UI.Mobile.IMobileBlockSignature>().Named("BlankSlate");
    blankSlateInstance.SourceBlock = blankSlate;
    // delete default content in Icon placeholder
    var placeholderContentWidget12 = blankSlateInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Icon");
    placeholderContentWidget12.Widgets.ToList().ForEach(w => w.Delete());
    var extendedClass5 = blankSlate.InputParameters.Named("ExtendedClass");
    blankSlateInstance.SetArgumentValue(extendedClass5, "\"cp-empty\"");
    var fullHeight = blankSlate.InputParameters.Named("FullHeight");
    blankSlateInstance.SetArgumentValue(fullHeight, null);
    
    var icon = placeholderContentWidget12.CreateWidget<ServiceStudio.Plugin.NRWidgets.IIcon>();
    icon.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    icon.ColumnSpan = null; // default value
    icon.CustomStyle = null; // default value
    icon.FlexBasis = null; // default value
    icon.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    icon.Icon = "comments-o";
    icon.IconSize = ServiceStudio.Plugin.NRWidgets.Enumerations.IconSize.Twotimes; // default value
    icon.RowSpan = null; // default value
    icon.SetStyle("\"icon\""); // default value
    icon.SetVisible("True"); // default value
    icon.Weight = null; // default value
    
    var placeholderContentWidget13 = blankSlateInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    var container62 = placeholderContentWidget13.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container62.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container62.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container62.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container62.Animate = false; // default value
    container62.ColumnGap = null; // default value
    container62.ColumnSpan = null; // default value
    container62.CustomStyle = null; // default value
    container62.FlexBasis = null; // default value
    container62.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container62.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container62.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container62.Gap = null;
    container62.GridColumns = null; // default value
    container62.GridRows = null; // default value
    container62.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container62.RowGap = null; // default value
    container62.RowSpan = null; // default value
    container62.SetStyle(null); // default value
    container62.SetVisible("True"); // default value
    container62.VisibleWasExpression = false; // default value
    container62.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container62.Width = "(fill parent)"; // default value
    
    var container63 = container62.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container63.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container63.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container63.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container63.Animate = false; // default value
    container63.ColumnGap = null; // default value
    container63.ColumnSpan = null; // default value
    container63.CustomStyle = null; // default value
    container63.FlexBasis = null; // default value
    container63.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container63.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container63.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container63.Gap = null;
    container63.GridColumns = null; // default value
    container63.GridRows = null; // default value
    container63.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container63.RowGap = null; // default value
    container63.RowSpan = null; // default value
    container63.SetStyle("\"cp-empty-title\"");
    container63.SetVisible("True"); // default value
    container63.VisibleWasExpression = false; // default value
    container63.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container63.Width = "(fill parent)"; // default value
    
    var expression21 = container63.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression21.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression21.ColumnSpan = null; // default value
    expression21.CustomStyle = null; // default value
    expression21.Example = null; // default value
    expression21.FlexBasis = null; // default value
    expression21.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression21.RowSpan = null; // default value
    expression21.SetStyle(null); // default value
    expression21.SetValue("If(Meta.HasData, \"No carparks match your search and filters.\", \"Carpark availability is temporarily unavailable.\")");
    expression21.Width = ""; // default value
    
    var container64 = container62.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container64.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container64.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container64.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container64.Animate = false; // default value
    container64.ColumnGap = null; // default value
    container64.ColumnSpan = null; // default value
    container64.CustomStyle = null; // default value
    container64.FlexBasis = null; // default value
    container64.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container64.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container64.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container64.Gap = null;
    container64.GridColumns = null; // default value
    container64.GridRows = null; // default value
    container64.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container64.RowGap = null; // default value
    container64.RowSpan = null; // default value
    container64.SetStyle("\"cp-empty-text\"");
    container64.SetVisible("True"); // default value
    container64.VisibleWasExpression = false; // default value
    container64.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container64.Width = "(fill parent)"; // default value
    
    var expression22 = container64.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression22.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression22.ColumnSpan = null; // default value
    expression22.CustomStyle = null; // default value
    expression22.Example = null; // default value
    expression22.FlexBasis = null; // default value
    expression22.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression22.RowSpan = null; // default value
    expression22.SetStyle(null); // default value
    expression22.SetValue("If(Meta.HasData, \"Try a different carpark number or address, or clear the filters.\", \"We could not retrieve data from data.gov.sg. Please try again in a few minutes.\")");
    expression22.Width = ""; // default value
    
    var placeholderContentWidget14 = blankSlateInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Actions");
    var container65 = placeholderContentWidget14.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container65.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container65.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container65.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container65.Animate = false; // default value
    container65.ColumnGap = null; // default value
    container65.ColumnSpan = null; // default value
    container65.CustomStyle = null; // default value
    container65.FlexBasis = null; // default value
    container65.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container65.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container65.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container65.Gap = null;
    container65.GridColumns = null; // default value
    container65.GridRows = null; // default value
    container65.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container65.RowGap = null; // default value
    container65.RowSpan = null; // default value
    container65.SetStyle(null); // default value
    container65.SetVisible("True"); // default value
    container65.VisibleWasExpression = false; // default value
    container65.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container65.Width = "(fill parent)"; // default value
    
    var button = container65.CreateWidget<ServiceStudio.Plugin.NRWidgets.IButton>();
    /*** Deleting Widgets that are created by default when instantiating an IButton ***/
    button.Widgets.ToList().ForEach(x => x.Delete());
    button.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    button.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    button.ColumnGap = null; // default value
    button.ColumnSpan = null; // default value
    button.SetConfirmationMessage(null); // default value
    button.CustomStyle = null; // default value
    button.SetEnabled("True");
    button.FlexBasis = null; // default value
    button.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    button.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    button.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    button.Gap = null;
    button.GridColumns = null; // default value
    button.GridRows = null; // default value
    button.IsDefault = false; // default value
    button.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    button.RowGap = null; // default value
    button.RowSpan = null; // default value
    button.SetStyle("\"btn cp-btn-secondary\"");
    button.SetVisible("Meta.HasData");
    button.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    button.Width = ""; // default value
    
    var expression23 = button.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression23.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression23.ColumnSpan = null; // default value
    expression23.CustomStyle = null; // default value
    expression23.Example = null; // default value
    expression23.FlexBasis = null; // default value
    expression23.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression23.RowSpan = null; // default value
    expression23.SetStyle(null); // default value
    expression23.SetValue("\"Clear filters\"");
    expression23.Width = ""; // default value
    var builtinEvent8 = button.OnClick;
    builtinEvent8.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent8.Destination = clearFilters;
    builtinEvent8.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    
    var button2 = container65.CreateWidget<ServiceStudio.Plugin.NRWidgets.IButton>();
    /*** Deleting Widgets that are created by default when instantiating an IButton ***/
    button2.Widgets.ToList().ForEach(x => x.Delete());
    button2.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    button2.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    button2.ColumnGap = null; // default value
    button2.ColumnSpan = null; // default value
    button2.SetConfirmationMessage(null); // default value
    button2.CustomStyle = null; // default value
    button2.SetEnabled("not IsRefreshing");
    button2.FlexBasis = null; // default value
    button2.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    button2.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    button2.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    button2.Gap = null;
    button2.GridColumns = null; // default value
    button2.GridRows = null; // default value
    button2.IsDefault = false; // default value
    button2.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    button2.RowGap = null; // default value
    button2.RowSpan = null; // default value
    button2.SetStyle("\"btn btn-primary\"");
    button2.SetVisible("not Meta.HasData");
    button2.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    button2.Width = ""; // default value
    
    var expression24 = button2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression24.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression24.ColumnSpan = null; // default value
    expression24.CustomStyle = null; // default value
    expression24.Example = null; // default value
    expression24.FlexBasis = null; // default value
    expression24.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression24.RowSpan = null; // default value
    expression24.SetStyle(null); // default value
    expression24.SetValue("\"Try again\"");
    expression24.Width = ""; // default value
    var builtinEvent9 = button2.OnClick;
    builtinEvent9.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent9.Destination = loadData;
    builtinEvent9.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    builtinEvent9.SetArgumentValue(forceRefresh, "True");
    
    var container66 = ifWidget5.FalseBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container66.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container66.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container66.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container66.Animate = false; // default value
    container66.ColumnGap = null; // default value
    container66.ColumnSpan = null; // default value
    container66.CustomStyle = null; // default value
    container66.FlexBasis = null; // default value
    container66.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container66.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container66.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container66.Gap = null;
    container66.GridColumns = null; // default value
    container66.GridRows = null; // default value
    container66.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container66.RowGap = null; // default value
    container66.RowSpan = null; // default value
    container66.SetStyle("\"cp-results\"");
    container66.SetVisible("True"); // default value
    container66.VisibleWasExpression = false; // default value
    container66.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container66.Width = "(fill parent)"; // default value
    
    var galleryInstance = container66.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IMobileBlockInstanceWidget>();
    galleryInstance.CustomStyle = null; // default value
    galleryInstance.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    var adaptive = outSystemsUI.MobileFlows.Named("Adaptive");
    var gallery = adaptive.Nodes.OfType<OutSystems.Model.UI.Mobile.IMobileBlockSignature>().Named("Gallery");
    galleryInstance.SourceBlock = gallery;
    // delete default content in Content placeholder
    var placeholderContentWidget15 = galleryInstance.PlaceholdersContent.FirstOrDefault(p => p.Placeholder?.Name == "Content");
    placeholderContentWidget15.Widgets.ToList().ForEach(w => w.Delete());
    var rowItemsDesktop = gallery.InputParameters.Named("RowItemsDesktop");
    galleryInstance.SetArgumentValue(rowItemsDesktop, "4");
    var rowItemsTablet = gallery.InputParameters.Named("RowItemsTablet");
    galleryInstance.SetArgumentValue(rowItemsTablet, "2");
    var itemsGap = gallery.InputParameters.Named("ItemsGap");
    galleryInstance.SetArgumentValue(itemsGap, null);
    var rowItemsPhone = gallery.InputParameters.Named("RowItemsPhone");
    galleryInstance.SetArgumentValue(rowItemsPhone, "1");
    var extendedClass6 = gallery.InputParameters.Named("ExtendedClass");
    galleryInstance.SetArgumentValue(extendedClass6, "\"cp-gallery\"");
    var eventHandler = galleryInstance.EventHandlers.FirstOrDefault(e => e.Event.Name == "Initialized");
    eventHandler.Handler = null; // default value
    
    var carparkList = placeholderContentWidget15.CreateWidget<ServiceStudio.Plugin.NRWidgets.IList>("CarparkList");
    carparkList.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    carparkList.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    carparkList.AnimateItems = true; // default value
    carparkList.ColumnGap = null; // default value
    carparkList.ColumnSpan = null; // default value
    carparkList.CustomStyle = null; // default value
    carparkList.ExpandedInWebEditor = true; // default value
    carparkList.FlexBasis = null; // default value
    carparkList.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    carparkList.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    carparkList.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    carparkList.Gap = null;
    carparkList.GridColumns = null; // default value
    carparkList.GridRows = null; // default value
    carparkList.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    carparkList.Mode = ServiceStudio.Plugin.NRWidgets.Enumerations.Mode.Default; // default value
    carparkList.RowGap = null; // default value
    carparkList.RowSpan = null; // default value
    carparkList.SetSource("PageRows");
    carparkList.SetStyle("\"list list-group\""); // default value
    carparkList.Tag = "div"; // default value
    carparkList.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    carparkList.Width = "(fill parent)"; // default value
    
    var container67 = carparkList.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container67.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container67.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container67.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container67.Animate = false; // default value
    container67.ColumnGap = null; // default value
    container67.ColumnSpan = null; // default value
    container67.CustomStyle = null; // default value
    container67.FlexBasis = null; // default value
    container67.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container67.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container67.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container67.Gap = null;
    container67.GridColumns = null; // default value
    container67.GridRows = null; // default value
    container67.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container67.RowGap = null; // default value
    container67.RowSpan = null; // default value
    container67.SetStyle("\"cp-card-wrap\"");
    container67.SetVisible("True"); // default value
    container67.VisibleWasExpression = false; // default value
    container67.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container67.Width = "(fill parent)"; // default value
    
    var link = container67.CreateWidget<ServiceStudio.Plugin.NRWidgets.ILink>();
    /*** Deleting Widgets that are created by default when instantiating an ILink ***/
    link.Widgets.ToList().ForEach(x => x.Delete());
    link.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    link.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    link.ColumnGap = null; // default value
    link.ColumnSpan = null; // default value
    link.SetConfirmationMessage(null); // default value
    link.CustomStyle = null; // default value
    link.SetEnabled("True");
    link.FlexBasis = null; // default value
    link.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    link.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    link.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    link.Gap = null;
    link.GridColumns = null; // default value
    link.GridRows = null; // default value
    link.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    link.RowGap = null; // default value
    link.RowSpan = null; // default value
    link.SetStyle("\"cp-card\"");
    link.SetVisible("True"); // default value
    link.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    link.Width = ""; // default value
    
    var container68 = link.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container68.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container68.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container68.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container68.Animate = false; // default value
    container68.ColumnGap = null; // default value
    container68.ColumnSpan = null; // default value
    container68.CustomStyle = null; // default value
    container68.FlexBasis = null; // default value
    container68.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container68.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container68.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container68.Gap = null;
    container68.GridColumns = null; // default value
    container68.GridRows = null; // default value
    container68.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container68.RowGap = null; // default value
    container68.RowSpan = null; // default value
    container68.SetStyle("\"cp-card-inner\"");
    container68.SetVisible("True"); // default value
    container68.VisibleWasExpression = false; // default value
    container68.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container68.Width = "(fill parent)"; // default value
    
    var container69 = container68.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container69.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container69.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container69.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container69.Animate = false; // default value
    container69.ColumnGap = null; // default value
    container69.ColumnSpan = null; // default value
    container69.CustomStyle = null; // default value
    container69.FlexBasis = null; // default value
    container69.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container69.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container69.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container69.Gap = null;
    container69.GridColumns = null; // default value
    container69.GridRows = null; // default value
    container69.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container69.RowGap = null; // default value
    container69.RowSpan = null; // default value
    container69.SetStyle("\"cp-card-top\"");
    container69.SetVisible("True"); // default value
    container69.VisibleWasExpression = false; // default value
    container69.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container69.Width = "(fill parent)"; // default value
    
    var container70 = container69.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container70.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container70.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container70.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container70.Animate = false; // default value
    container70.ColumnGap = null; // default value
    container70.ColumnSpan = null; // default value
    container70.CustomStyle = null; // default value
    container70.FlexBasis = null; // default value
    container70.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container70.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container70.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container70.Gap = null;
    container70.GridColumns = null; // default value
    container70.GridRows = null; // default value
    container70.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container70.RowGap = null; // default value
    container70.RowSpan = null; // default value
    container70.SetStyle("\"cp-card-id\"");
    container70.SetVisible("True"); // default value
    container70.VisibleWasExpression = false; // default value
    container70.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container70.Width = "(fill parent)"; // default value
    
    var expression25 = container70.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression25.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression25.ColumnSpan = null; // default value
    expression25.CustomStyle = null; // default value
    expression25.Example = null; // default value
    expression25.FlexBasis = null; // default value
    expression25.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression25.RowSpan = null; // default value
    expression25.SetStyle(null); // default value
    expression25.SetValue("PageRows.Current.CarparkNumber");
    expression25.Width = ""; // default value
    
    var container71 = container69.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container71.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container71.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container71.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container71.Animate = false; // default value
    container71.ColumnGap = null; // default value
    container71.ColumnSpan = null; // default value
    container71.CustomStyle = null; // default value
    container71.FlexBasis = null; // default value
    container71.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container71.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container71.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container71.Gap = null;
    container71.GridColumns = null; // default value
    container71.GridRows = null; // default value
    container71.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container71.RowGap = null; // default value
    container71.RowSpan = null; // default value
    container71.SetStyle("\"cp-badge cp-status-\" + ToLower(IdentifierToText(PageRows.Current.StatusId))");
    container71.SetVisible("True"); // default value
    container71.VisibleWasExpression = false; // default value
    container71.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container71.Width = "(fill parent)"; // default value
    
    var expression26 = container71.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression26.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression26.ColumnSpan = null; // default value
    expression26.CustomStyle = null; // default value
    expression26.Example = null; // default value
    expression26.FlexBasis = null; // default value
    expression26.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression26.RowSpan = null; // default value
    expression26.SetStyle(null); // default value
    expression26.SetValue("PageRows.Current.StatusLabel");
    expression26.Width = ""; // default value
    
    var container72 = container68.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container72.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container72.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container72.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container72.Animate = false; // default value
    container72.ColumnGap = null; // default value
    container72.ColumnSpan = null; // default value
    container72.CustomStyle = null; // default value
    container72.FlexBasis = null; // default value
    container72.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container72.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container72.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container72.Gap = null;
    container72.GridColumns = null; // default value
    container72.GridRows = null; // default value
    container72.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container72.RowGap = null; // default value
    container72.RowSpan = null; // default value
    container72.SetStyle("\"cp-card-address\"");
    container72.SetVisible("True"); // default value
    container72.VisibleWasExpression = false; // default value
    container72.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container72.Width = "(fill parent)"; // default value
    
    var expression27 = container72.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression27.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression27.ColumnSpan = null; // default value
    expression27.CustomStyle = null; // default value
    expression27.Example = null; // default value
    expression27.FlexBasis = null; // default value
    expression27.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression27.RowSpan = null; // default value
    expression27.SetStyle(null); // default value
    expression27.SetValue("If(PageRows.Current.HasLocation, PageRows.Current.Address, \"Address not available\")");
    expression27.Width = ""; // default value
    
    var container73 = container68.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container73.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container73.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container73.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container73.Animate = false; // default value
    container73.ColumnGap = null; // default value
    container73.ColumnSpan = null; // default value
    container73.CustomStyle = null; // default value
    container73.FlexBasis = null; // default value
    container73.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container73.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container73.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container73.Gap = null;
    container73.GridColumns = null; // default value
    container73.GridRows = null; // default value
    container73.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container73.RowGap = null; // default value
    container73.RowSpan = null; // default value
    container73.SetStyle("\"cp-card-figure\"");
    container73.SetVisible("True"); // default value
    container73.VisibleWasExpression = false; // default value
    container73.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container73.Width = "(fill parent)"; // default value
    
    var container74 = container73.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container74.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container74.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container74.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container74.Animate = false; // default value
    container74.ColumnGap = null; // default value
    container74.ColumnSpan = null; // default value
    container74.CustomStyle = null; // default value
    container74.FlexBasis = null; // default value
    container74.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container74.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container74.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container74.Gap = null;
    container74.GridColumns = null; // default value
    container74.GridRows = null; // default value
    container74.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container74.RowGap = null; // default value
    container74.RowSpan = null; // default value
    container74.SetStyle("\"cp-avail-number\"");
    container74.SetVisible("True"); // default value
    container74.VisibleWasExpression = false; // default value
    container74.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container74.Width = "(fill parent)"; // default value
    
    var expression28 = container74.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression28.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression28.ColumnSpan = null; // default value
    expression28.CustomStyle = null; // default value
    expression28.Example = null; // default value
    expression28.FlexBasis = null; // default value
    expression28.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression28.RowSpan = null; // default value
    expression28.SetStyle(null); // default value
    expression28.SetValue("If(PageRows.Current.IsAnomalous, \"–\", FormatDecimal(IntegerToDecimal(PageRows.Current.AvailableLots), 0, \".\", \",\"))");
    expression28.Width = ""; // default value
    
    var container75 = container73.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container75.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container75.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container75.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container75.Animate = false; // default value
    container75.ColumnGap = null; // default value
    container75.ColumnSpan = null; // default value
    container75.CustomStyle = null; // default value
    container75.FlexBasis = null; // default value
    container75.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container75.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container75.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container75.Gap = null;
    container75.GridColumns = null; // default value
    container75.GridRows = null; // default value
    container75.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container75.RowGap = null; // default value
    container75.RowSpan = null; // default value
    container75.SetStyle("\"cp-avail-caption\"");
    container75.SetVisible("True"); // default value
    container75.VisibleWasExpression = false; // default value
    container75.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container75.Width = "(fill parent)"; // default value
    
    var expression29 = container75.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression29.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression29.ColumnSpan = null; // default value
    expression29.CustomStyle = null; // default value
    expression29.Example = null; // default value
    expression29.FlexBasis = null; // default value
    expression29.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression29.RowSpan = null; // default value
    expression29.SetStyle(null); // default value
    expression29.SetValue("If(PageRows.Current.IsAnomalous, \"Lot data unavailable\", \"available of \" + FormatDecimal(IntegerToDecimal(PageRows.Current.TotalLots), 0, \".\", \",\") + \" lots · \" + PageRows.Current.LotTypeLabel)");
    expression29.Width = ""; // default value
    
    var container76 = container68.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container76.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container76.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container76.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container76.Animate = false; // default value
    container76.ColumnGap = null; // default value
    container76.ColumnSpan = null; // default value
    container76.CustomStyle = null; // default value
    container76.FlexBasis = null; // default value
    container76.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container76.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container76.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container76.Gap = null;
    container76.GridColumns = null; // default value
    container76.GridRows = null; // default value
    container76.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container76.RowGap = null; // default value
    container76.RowSpan = null; // default value
    container76.SetStyle("\"cp-meter\"");
    container76.SetVisible("True"); // default value
    container76.VisibleWasExpression = false; // default value
    container76.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container76.Width = "(fill parent)"; // default value
    
    var container77 = container76.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container77.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container77.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container77.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container77.Animate = false; // default value
    container77.ColumnGap = null; // default value
    container77.ColumnSpan = null; // default value
    container77.CustomStyle = null; // default value
    container77.FlexBasis = null; // default value
    container77.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container77.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container77.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container77.Gap = null;
    container77.GridColumns = null; // default value
    container77.GridRows = null; // default value
    container77.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container77.RowGap = null; // default value
    container77.RowSpan = null; // default value
    container77.SetStyle("\"cp-meter-bar cp-status-\" + ToLower(IdentifierToText(PageRows.Current.StatusId))");
    container77.SetVisible("True"); // default value
    container77.VisibleWasExpression = false; // default value
    container77.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container77.Width = "(fill parent)"; // default value
    
    var extendedProperty17 = container77.CreateExtendedProperty();
    extendedProperty17.Property = "style";
    extendedProperty17.SetValue("\"width: \" + If(PageRows.Current.IsAnomalous, 0.0, PageRows.Current.AvailabilityPct) + \"%\"");
    
    var extendedProperty18 = container76.CreateExtendedProperty();
    extendedProperty18.Property = "aria-hidden";
    extendedProperty18.SetValue("\"true\"");
    
    var container78 = container68.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container78.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container78.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container78.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container78.Animate = false; // default value
    container78.ColumnGap = null; // default value
    container78.ColumnSpan = null; // default value
    container78.CustomStyle = null; // default value
    container78.FlexBasis = null; // default value
    container78.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container78.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container78.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container78.Gap = null;
    container78.GridColumns = null; // default value
    container78.GridRows = null; // default value
    container78.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container78.RowGap = null; // default value
    container78.RowSpan = null; // default value
    container78.SetStyle("\"cp-card-pct\"");
    container78.SetVisible("True"); // default value
    container78.VisibleWasExpression = false; // default value
    container78.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container78.Width = "(fill parent)"; // default value
    
    var expression30 = container78.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression30.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression30.ColumnSpan = null; // default value
    expression30.CustomStyle = null; // default value
    expression30.Example = null; // default value
    expression30.FlexBasis = null; // default value
    expression30.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression30.RowSpan = null; // default value
    expression30.SetStyle(null); // default value
    expression30.SetValue("If(PageRows.Current.IsAnomalous, \"Reading inconsistent\", FormatDecimal(PageRows.Current.AvailabilityPct, 1, \".\", \",\") + \"% available\")");
    expression30.Width = ""; // default value
    
    var container79 = container68.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container79.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container79.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container79.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container79.Animate = false; // default value
    container79.ColumnGap = null; // default value
    container79.ColumnSpan = null; // default value
    container79.CustomStyle = null; // default value
    container79.FlexBasis = null; // default value
    container79.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container79.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container79.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container79.Gap = null;
    container79.GridColumns = null; // default value
    container79.GridRows = null; // default value
    container79.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container79.RowGap = null; // default value
    container79.RowSpan = null; // default value
    container79.SetStyle("\"cp-card-foot\"");
    container79.SetVisible("True"); // default value
    container79.VisibleWasExpression = false; // default value
    container79.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container79.Width = "(fill parent)"; // default value
    
    var container80 = container79.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container80.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container80.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container80.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container80.Animate = false; // default value
    container80.ColumnGap = null; // default value
    container80.ColumnSpan = null; // default value
    container80.CustomStyle = null; // default value
    container80.FlexBasis = null; // default value
    container80.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container80.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container80.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container80.Gap = null;
    container80.GridColumns = null; // default value
    container80.GridRows = null; // default value
    container80.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container80.RowGap = null; // default value
    container80.RowSpan = null; // default value
    container80.SetStyle("\"cp-card-updated\"");
    container80.SetVisible("True"); // default value
    container80.VisibleWasExpression = false; // default value
    container80.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container80.Width = "(fill parent)"; // default value
    
    var expression31 = container80.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression31.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression31.ColumnSpan = null; // default value
    expression31.CustomStyle = null; // default value
    expression31.Example = null; // default value
    expression31.FlexBasis = null; // default value
    expression31.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression31.RowSpan = null; // default value
    expression31.SetStyle(null); // default value
    expression31.SetValue("\"Updated \" + PageRows.Current.UpdatedAtText");
    expression31.Width = ""; // default value
    
    var ifWidget6 = container79.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget6.Animate = false; // default value
    ifWidget6.SetCondition("PageRows.Current.IsReadingOutdated");
    ifWidget6.CustomStyle = null; // default value
    ifWidget6.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container81 = ifWidget6.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container81.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container81.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container81.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container81.Animate = false; // default value
    container81.ColumnGap = null; // default value
    container81.ColumnSpan = null; // default value
    container81.CustomStyle = null; // default value
    container81.FlexBasis = null; // default value
    container81.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container81.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container81.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container81.Gap = null;
    container81.GridColumns = null; // default value
    container81.GridRows = null; // default value
    container81.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container81.RowGap = null; // default value
    container81.RowSpan = null; // default value
    container81.SetStyle("\"cp-flag cp-flag-outdated\"");
    container81.SetVisible("True"); // default value
    container81.VisibleWasExpression = false; // default value
    container81.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container81.Width = "(fill parent)"; // default value
    
    var textWidget16 = container81.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget16.CustomStyle = null; // default value
    textWidget16.SetStyleClasses(null); // default value
    textWidget16.Text = "Reading not recently updated";
    
    var ifWidget7 = container79.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget7.Animate = false; // default value
    ifWidget7.SetCondition("PageRows.Current.LotTypeCount > 1 and LotTypeFilter = NullTextIdentifier()");
    ifWidget7.CustomStyle = null; // default value
    ifWidget7.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container82 = ifWidget7.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container82.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container82.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container82.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container82.Animate = false; // default value
    container82.ColumnGap = null; // default value
    container82.ColumnSpan = null; // default value
    container82.CustomStyle = null; // default value
    container82.FlexBasis = null; // default value
    container82.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container82.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container82.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container82.Gap = null;
    container82.GridColumns = null; // default value
    container82.GridRows = null; // default value
    container82.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container82.RowGap = null; // default value
    container82.RowSpan = null; // default value
    container82.SetStyle("\"cp-flag\"");
    container82.SetVisible("True"); // default value
    container82.VisibleWasExpression = false; // default value
    container82.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container82.Width = "(fill parent)"; // default value
    
    var expression32 = container82.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression32.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression32.ColumnSpan = null; // default value
    expression32.CustomStyle = null; // default value
    expression32.Example = null; // default value
    expression32.FlexBasis = null; // default value
    expression32.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression32.RowSpan = null; // default value
    expression32.SetStyle(null); // default value
    expression32.SetValue("\"+\" + (PageRows.Current.LotTypeCount - 1) + If(PageRows.Current.LotTypeCount > 2, \" other lot types\", \" other lot type\")");
    expression32.Width = ""; // default value
    var builtinEvent10 = link.OnClick;
    builtinEvent10.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent10.Destination = openDetail;
    builtinEvent10.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    builtinEvent10.SetArgumentValue(carpark, "PageRows.Current.CarparkNumber");
    
    var extendedProperty19 = link.CreateExtendedProperty();
    extendedProperty19.Property = "aria-label";
    extendedProperty19.SetValue("\"Carpark \" + PageRows.Current.CarparkNumber + \": \" + If(PageRows.Current.IsAnomalous, \"availability data unavailable\", FormatDecimal(IntegerToDecimal(PageRows.Current.AvailableLots), 0, \".\", \",\") + \" of \" + FormatDecimal(IntegerToDecimal(PageRows.Current.TotalLots), 0, \".\", \",\") + \" lots available (\" + PageRows.Current.LotTypeLabel + \"), \" + PageRows.Current.StatusLabel) + \". View details.\"");
    
    var extendedProperty20 = container67.CreateExtendedProperty();
    extendedProperty20.Property = "role";
    extendedProperty20.SetValue("\"listitem\"");
    var builtinEvent11 = carparkList.OnScrollEnding;
    builtinEvent11.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent11.Destination = null; // default value
    builtinEvent11.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    
    var extendedProperty21 = carparkList.CreateExtendedProperty();
    extendedProperty21.Property = "role";
    extendedProperty21.SetValue("\"list\"");
    
    var ifWidget8 = container66.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget8.Animate = false; // default value
    ifWidget8.SetCondition("PageRows.Length < FilteredRows.Length");
    ifWidget8.CustomStyle = null; // default value
    ifWidget8.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container83 = ifWidget8.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container83.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container83.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container83.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container83.Animate = false; // default value
    container83.ColumnGap = null; // default value
    container83.ColumnSpan = null; // default value
    container83.CustomStyle = null; // default value
    container83.FlexBasis = null; // default value
    container83.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container83.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container83.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container83.Gap = null;
    container83.GridColumns = null; // default value
    container83.GridRows = null; // default value
    container83.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container83.RowGap = null; // default value
    container83.RowSpan = null; // default value
    container83.SetStyle("\"cp-more\"");
    container83.SetVisible("True"); // default value
    container83.VisibleWasExpression = false; // default value
    container83.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container83.Width = "(fill parent)"; // default value
    
    var showMoreButton = container83.CreateWidget<ServiceStudio.Plugin.NRWidgets.IButton>("ShowMoreButton");
    /*** Deleting Widgets that are created by default when instantiating an IButton ***/
    showMoreButton.Widgets.ToList().ForEach(x => x.Delete());
    showMoreButton.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    showMoreButton.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    showMoreButton.ColumnGap = null; // default value
    showMoreButton.ColumnSpan = null; // default value
    showMoreButton.SetConfirmationMessage(null); // default value
    showMoreButton.CustomStyle = null; // default value
    showMoreButton.SetEnabled("True");
    showMoreButton.FlexBasis = null; // default value
    showMoreButton.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    showMoreButton.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    showMoreButton.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    showMoreButton.Gap = null;
    showMoreButton.GridColumns = null; // default value
    showMoreButton.GridRows = null; // default value
    showMoreButton.IsDefault = false; // default value
    showMoreButton.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    showMoreButton.RowGap = null; // default value
    showMoreButton.RowSpan = null; // default value
    showMoreButton.SetStyle("\"btn cp-btn-secondary\"");
    showMoreButton.SetVisible("True"); // default value
    showMoreButton.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    showMoreButton.Width = ""; // default value
    
    var expression33 = showMoreButton.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression33.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression33.ColumnSpan = null; // default value
    expression33.CustomStyle = null; // default value
    expression33.Example = null; // default value
    expression33.FlexBasis = null; // default value
    expression33.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression33.RowSpan = null; // default value
    expression33.SetStyle(null); // default value
    expression33.SetValue("\"Show more carparks (\" + FormatDecimal(IntegerToDecimal(FilteredRows.Length - PageRows.Length), 0, \".\", \",\") + \" more)\"");
    expression33.Width = ""; // default value
    var builtinEvent12 = showMoreButton.OnClick;
    builtinEvent12.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent12.Destination = showMore;
    builtinEvent12.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Inherited; // default value
    
    var container84 = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container84.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container84.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container84.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container84.Animate = false; // default value
    container84.ColumnGap = null; // default value
    container84.ColumnSpan = null; // default value
    container84.CustomStyle = null; // default value
    container84.FlexBasis = null; // default value
    container84.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container84.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container84.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container84.Gap = null;
    container84.GridColumns = null; // default value
    container84.GridRows = null; // default value
    container84.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container84.RowGap = null; // default value
    container84.RowSpan = null; // default value
    container84.SetStyle("\"cp-source\"");
    container84.SetVisible("True"); // default value
    container84.VisibleWasExpression = false; // default value
    container84.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container84.Width = "(fill parent)"; // default value
    
    var textWidget17 = container84.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget17.CustomStyle = null; // default value
    textWidget17.SetStyleClasses(null); // default value
    textWidget17.Text = "Source: data.gov.sg – Carpark Availability and HDB Carpark Information datasets. Availability is reported separately for each lot type; carpark names and locations come only from the HDB dataset.";
    
    var detailPopup = container2.CreateWidget<ServiceStudio.Plugin.NRWidgets.IPopup>("DetailPopup");
    detailPopup.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    detailPopup.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    detailPopup.ColumnGap = null; // default value
    detailPopup.ColumnSpan = null; // default value
    detailPopup.CustomStyle = null; // default value
    detailPopup.ExpandedInWebEditor = true; // default value
    detailPopup.FlexBasis = null; // default value
    detailPopup.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    detailPopup.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    detailPopup.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    detailPopup.Gap = null;
    detailPopup.GridColumns = null; // default value
    detailPopup.GridRows = null; // default value
    detailPopup.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    detailPopup.RowGap = null; // default value
    detailPopup.RowSpan = null; // default value
    detailPopup.SetShowPopup("ShowDetail");
    detailPopup.SetStyle("\"popup-dialog cp-detail\"");
    detailPopup.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    
    var container85 = detailPopup.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container85.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container85.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container85.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container85.Animate = false; // default value
    container85.ColumnGap = null; // default value
    container85.ColumnSpan = null; // default value
    container85.CustomStyle = null; // default value
    container85.FlexBasis = null; // default value
    container85.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container85.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container85.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container85.Gap = null;
    container85.GridColumns = null; // default value
    container85.GridRows = null; // default value
    container85.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container85.RowGap = null; // default value
    container85.RowSpan = null; // default value
    container85.SetStyle("\"cp-detail-body\"");
    container85.SetVisible("True"); // default value
    container85.VisibleWasExpression = false; // default value
    container85.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container85.Width = "(fill parent)"; // default value
    
    var container86 = container85.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container86.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container86.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container86.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container86.Animate = false; // default value
    container86.ColumnGap = null; // default value
    container86.ColumnSpan = null; // default value
    container86.CustomStyle = null; // default value
    container86.FlexBasis = null; // default value
    container86.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container86.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container86.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container86.Gap = null;
    container86.GridColumns = null; // default value
    container86.GridRows = null; // default value
    container86.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container86.RowGap = null; // default value
    container86.RowSpan = null; // default value
    container86.SetStyle("\"cp-detail-head\"");
    container86.SetVisible("True"); // default value
    container86.VisibleWasExpression = false; // default value
    container86.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container86.Width = "(fill parent)"; // default value
    
    var container87 = container86.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container87.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container87.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container87.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container87.Animate = false; // default value
    container87.ColumnGap = null; // default value
    container87.ColumnSpan = null; // default value
    container87.CustomStyle = null; // default value
    container87.FlexBasis = null; // default value
    container87.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container87.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container87.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container87.Gap = null;
    container87.GridColumns = null; // default value
    container87.GridRows = null; // default value
    container87.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container87.RowGap = null; // default value
    container87.RowSpan = null; // default value
    container87.SetStyle("\"cp-detail-title\"");
    container87.SetVisible("True"); // default value
    container87.VisibleWasExpression = false; // default value
    container87.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container87.Width = "(fill parent)"; // default value
    
    var expression34 = container87.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression34.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression34.ColumnSpan = null; // default value
    expression34.CustomStyle = null; // default value
    expression34.Example = null; // default value
    expression34.FlexBasis = null; // default value
    expression34.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression34.RowSpan = null; // default value
    expression34.SetStyle(null); // default value
    expression34.SetValue("\"Carpark \" + SelectedCarpark");
    expression34.Width = ""; // default value
    
    var extendedProperty22 = container87.CreateExtendedProperty();
    extendedProperty22.Property = "role";
    extendedProperty22.SetValue("\"heading\"");
    
    var extendedProperty23 = container87.CreateExtendedProperty();
    extendedProperty23.Property = "aria-level";
    extendedProperty23.SetValue("\"2\"");
    
    var closeDetailButton = container86.CreateWidget<ServiceStudio.Plugin.NRWidgets.IButton>("CloseDetailButton");
    /*** Deleting Widgets that are created by default when instantiating an IButton ***/
    closeDetailButton.Widgets.ToList().ForEach(x => x.Delete());
    closeDetailButton.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    closeDetailButton.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    closeDetailButton.ColumnGap = null; // default value
    closeDetailButton.ColumnSpan = null; // default value
    closeDetailButton.SetConfirmationMessage(null); // default value
    closeDetailButton.CustomStyle = null; // default value
    closeDetailButton.SetEnabled("True");
    closeDetailButton.FlexBasis = null; // default value
    closeDetailButton.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    closeDetailButton.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    closeDetailButton.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    closeDetailButton.Gap = null;
    closeDetailButton.GridColumns = null; // default value
    closeDetailButton.GridRows = null; // default value
    closeDetailButton.IsDefault = false; // default value
    closeDetailButton.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    closeDetailButton.RowGap = null; // default value
    closeDetailButton.RowSpan = null; // default value
    closeDetailButton.SetStyle("\"btn cp-btn-secondary cp-btn-close\"");
    closeDetailButton.SetVisible("True"); // default value
    closeDetailButton.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    closeDetailButton.Width = ""; // default value
    
    var expression35 = closeDetailButton.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression35.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression35.ColumnSpan = null; // default value
    expression35.CustomStyle = null; // default value
    expression35.Example = null; // default value
    expression35.FlexBasis = null; // default value
    expression35.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression35.RowSpan = null; // default value
    expression35.SetStyle(null); // default value
    expression35.SetValue("\"Close\"");
    expression35.Width = ""; // default value
    var builtinEvent13 = closeDetailButton.OnClick;
    builtinEvent13.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent13.Destination = closeDetail;
    builtinEvent13.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Fade;
    
    var extendedProperty24 = closeDetailButton.CreateExtendedProperty();
    extendedProperty24.Property = "aria-label";
    extendedProperty24.SetValue("\"Close carpark details\"");
    
    var ifWidget9 = container85.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget9.Animate = false; // default value
    ifWidget9.SetCondition("DetailRows.Empty");
    ifWidget9.CustomStyle = null; // default value
    ifWidget9.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container88 = ifWidget9.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container88.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container88.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container88.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container88.Animate = false; // default value
    container88.ColumnGap = null; // default value
    container88.ColumnSpan = null; // default value
    container88.CustomStyle = null; // default value
    container88.FlexBasis = null; // default value
    container88.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container88.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container88.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container88.Gap = null;
    container88.GridColumns = null; // default value
    container88.GridRows = null; // default value
    container88.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container88.RowGap = null; // default value
    container88.RowSpan = null; // default value
    container88.SetStyle("\"cp-detail-note\"");
    container88.SetVisible("True"); // default value
    container88.VisibleWasExpression = false; // default value
    container88.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container88.Width = "(fill parent)"; // default value
    
    var textWidget18 = container88.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget18.CustomStyle = null; // default value
    textWidget18.SetStyleClasses(null); // default value
    textWidget18.Text = "Details for this carpark are no longer available. Refresh the data and try again.";
    
    var container89 = ifWidget9.FalseBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container89.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container89.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container89.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container89.Animate = false; // default value
    container89.ColumnGap = null; // default value
    container89.ColumnSpan = null; // default value
    container89.CustomStyle = null; // default value
    container89.FlexBasis = null; // default value
    container89.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container89.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container89.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container89.Gap = null;
    container89.GridColumns = null; // default value
    container89.GridRows = null; // default value
    container89.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container89.RowGap = null; // default value
    container89.RowSpan = null; // default value
    container89.SetStyle(null); // default value
    container89.SetVisible("True"); // default value
    container89.VisibleWasExpression = false; // default value
    container89.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container89.Width = "(fill parent)"; // default value
    
    var container90 = container89.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container90.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container90.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container90.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container90.Animate = false; // default value
    container90.ColumnGap = null; // default value
    container90.ColumnSpan = null; // default value
    container90.CustomStyle = null; // default value
    container90.FlexBasis = null; // default value
    container90.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container90.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container90.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container90.Gap = null;
    container90.GridColumns = null; // default value
    container90.GridRows = null; // default value
    container90.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container90.RowGap = null; // default value
    container90.RowSpan = null; // default value
    container90.SetStyle("\"cp-detail-address\"");
    container90.SetVisible("True"); // default value
    container90.VisibleWasExpression = false; // default value
    container90.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container90.Width = "(fill parent)"; // default value
    
    var expression36 = container90.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression36.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression36.ColumnSpan = null; // default value
    expression36.CustomStyle = null; // default value
    expression36.Example = null; // default value
    expression36.FlexBasis = null; // default value
    expression36.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression36.RowSpan = null; // default value
    expression36.SetStyle(null); // default value
    expression36.SetValue("If(DetailRows[0].HasLocation, DetailRows[0].Address, \"Address not available in the HDB Carpark Information dataset.\")");
    expression36.Width = ""; // default value
    
    var container91 = container89.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container91.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container91.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container91.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container91.Animate = false; // default value
    container91.ColumnGap = null; // default value
    container91.ColumnSpan = null; // default value
    container91.CustomStyle = null; // default value
    container91.FlexBasis = null; // default value
    container91.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container91.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container91.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container91.Gap = null;
    container91.GridColumns = null; // default value
    container91.GridRows = null; // default value
    container91.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container91.RowGap = null; // default value
    container91.RowSpan = null; // default value
    container91.SetStyle("\"cp-detail-section\"");
    container91.SetVisible("True"); // default value
    container91.VisibleWasExpression = false; // default value
    container91.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container91.Width = "(fill parent)"; // default value
    
    var container92 = container91.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container92.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container92.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container92.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container92.Animate = false; // default value
    container92.ColumnGap = null; // default value
    container92.ColumnSpan = null; // default value
    container92.CustomStyle = null; // default value
    container92.FlexBasis = null; // default value
    container92.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container92.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container92.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container92.Gap = null;
    container92.GridColumns = null; // default value
    container92.GridRows = null; // default value
    container92.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container92.RowGap = null; // default value
    container92.RowSpan = null; // default value
    container92.SetStyle("\"cp-detail-subtitle\"");
    container92.SetVisible("True"); // default value
    container92.VisibleWasExpression = false; // default value
    container92.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container92.Width = "(fill parent)"; // default value
    
    var textWidget19 = container92.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget19.CustomStyle = null; // default value
    textWidget19.SetStyleClasses(null); // default value
    textWidget19.Text = "Availability by lot type";
    
    var extendedProperty25 = container92.CreateExtendedProperty();
    extendedProperty25.Property = "role";
    extendedProperty25.SetValue("\"heading\"");
    
    var extendedProperty26 = container92.CreateExtendedProperty();
    extendedProperty26.Property = "aria-level";
    extendedProperty26.SetValue("\"3\"");
    
    var lotBreakdownList = container91.CreateWidget<ServiceStudio.Plugin.NRWidgets.IList>("LotBreakdownList");
    lotBreakdownList.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    lotBreakdownList.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    lotBreakdownList.AnimateItems = true; // default value
    lotBreakdownList.ColumnGap = null; // default value
    lotBreakdownList.ColumnSpan = null; // default value
    lotBreakdownList.CustomStyle = null; // default value
    lotBreakdownList.ExpandedInWebEditor = true; // default value
    lotBreakdownList.FlexBasis = null; // default value
    lotBreakdownList.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    lotBreakdownList.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    lotBreakdownList.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    lotBreakdownList.Gap = null;
    lotBreakdownList.GridColumns = null; // default value
    lotBreakdownList.GridRows = null; // default value
    lotBreakdownList.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    lotBreakdownList.Mode = ServiceStudio.Plugin.NRWidgets.Enumerations.Mode.Default; // default value
    lotBreakdownList.RowGap = null; // default value
    lotBreakdownList.RowSpan = null; // default value
    lotBreakdownList.SetSource("DetailRows");
    lotBreakdownList.SetStyle("\"list list-group\""); // default value
    lotBreakdownList.Tag = "div"; // default value
    lotBreakdownList.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    lotBreakdownList.Width = "(fill parent)"; // default value
    
    var container93 = lotBreakdownList.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container93.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container93.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container93.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container93.Animate = false; // default value
    container93.ColumnGap = null; // default value
    container93.ColumnSpan = null; // default value
    container93.CustomStyle = null; // default value
    container93.FlexBasis = null; // default value
    container93.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container93.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container93.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container93.Gap = null;
    container93.GridColumns = null; // default value
    container93.GridRows = null; // default value
    container93.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container93.RowGap = null; // default value
    container93.RowSpan = null; // default value
    container93.SetStyle("\"cp-lot-row\"");
    container93.SetVisible("True"); // default value
    container93.VisibleWasExpression = false; // default value
    container93.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container93.Width = "(fill parent)"; // default value
    
    var container94 = container93.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container94.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container94.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container94.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container94.Animate = false; // default value
    container94.ColumnGap = null; // default value
    container94.ColumnSpan = null; // default value
    container94.CustomStyle = null; // default value
    container94.FlexBasis = null; // default value
    container94.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container94.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container94.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container94.Gap = null;
    container94.GridColumns = null; // default value
    container94.GridRows = null; // default value
    container94.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container94.RowGap = null; // default value
    container94.RowSpan = null; // default value
    container94.SetStyle("\"cp-lot-name\"");
    container94.SetVisible("True"); // default value
    container94.VisibleWasExpression = false; // default value
    container94.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container94.Width = "(fill parent)"; // default value
    
    var expression37 = container94.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression37.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression37.ColumnSpan = null; // default value
    expression37.CustomStyle = null; // default value
    expression37.Example = null; // default value
    expression37.FlexBasis = null; // default value
    expression37.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression37.RowSpan = null; // default value
    expression37.SetStyle(null); // default value
    expression37.SetValue("DetailRows.Current.LotTypeLabel");
    expression37.Width = ""; // default value
    
    var container95 = container93.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container95.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container95.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container95.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container95.Animate = false; // default value
    container95.ColumnGap = null; // default value
    container95.ColumnSpan = null; // default value
    container95.CustomStyle = null; // default value
    container95.FlexBasis = null; // default value
    container95.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container95.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container95.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container95.Gap = null;
    container95.GridColumns = null; // default value
    container95.GridRows = null; // default value
    container95.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container95.RowGap = null; // default value
    container95.RowSpan = null; // default value
    container95.SetStyle("\"cp-lot-figure\"");
    container95.SetVisible("True"); // default value
    container95.VisibleWasExpression = false; // default value
    container95.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container95.Width = "(fill parent)"; // default value
    
    var expression38 = container95.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression38.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression38.ColumnSpan = null; // default value
    expression38.CustomStyle = null; // default value
    expression38.Example = null; // default value
    expression38.FlexBasis = null; // default value
    expression38.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression38.RowSpan = null; // default value
    expression38.SetStyle(null); // default value
    expression38.SetValue("If(DetailRows.Current.IsAnomalous, \"Data unavailable (\" + DetailRows.Current.AvailableLots + \" of \" + DetailRows.Current.TotalLots + \" reported)\", FormatDecimal(IntegerToDecimal(DetailRows.Current.AvailableLots), 0, \".\", \",\") + \" of \" + FormatDecimal(IntegerToDecimal(DetailRows.Current.TotalLots), 0, \".\", \",\") + \" available (\" + FormatDecimal(DetailRows.Current.AvailabilityPct, 1, \".\", \",\") + \"%)\")");
    expression38.Width = ""; // default value
    
    var container96 = container93.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container96.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container96.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container96.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container96.Animate = false; // default value
    container96.ColumnGap = null; // default value
    container96.ColumnSpan = null; // default value
    container96.CustomStyle = null; // default value
    container96.FlexBasis = null; // default value
    container96.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container96.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container96.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container96.Gap = null;
    container96.GridColumns = null; // default value
    container96.GridRows = null; // default value
    container96.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container96.RowGap = null; // default value
    container96.RowSpan = null; // default value
    container96.SetStyle("\"cp-badge cp-status-\" + ToLower(IdentifierToText(DetailRows.Current.StatusId))");
    container96.SetVisible("True"); // default value
    container96.VisibleWasExpression = false; // default value
    container96.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container96.Width = "(fill parent)"; // default value
    
    var expression39 = container96.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression39.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression39.ColumnSpan = null; // default value
    expression39.CustomStyle = null; // default value
    expression39.Example = null; // default value
    expression39.FlexBasis = null; // default value
    expression39.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression39.RowSpan = null; // default value
    expression39.SetStyle(null); // default value
    expression39.SetValue("DetailRows.Current.StatusLabel");
    expression39.Width = ""; // default value
    
    var extendedProperty27 = container93.CreateExtendedProperty();
    extendedProperty27.Property = "role";
    extendedProperty27.SetValue("\"listitem\"");
    var builtinEvent14 = lotBreakdownList.OnScrollEnding;
    builtinEvent14.BuiltInValidations = OutSystems.Model.Enumerations.ValidationBehavior.None; // default value
    builtinEvent14.Destination = null; // default value
    builtinEvent14.Transition = OutSystems.Model.Enumerations.TransitionTypeWithInheritance.Fade;
    
    var extendedProperty28 = lotBreakdownList.CreateExtendedProperty();
    extendedProperty28.Property = "role";
    extendedProperty28.SetValue("\"list\"");
    
    var container97 = container91.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container97.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container97.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container97.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container97.Animate = false; // default value
    container97.ColumnGap = null; // default value
    container97.ColumnSpan = null; // default value
    container97.CustomStyle = null; // default value
    container97.FlexBasis = null; // default value
    container97.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container97.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container97.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container97.Gap = null;
    container97.GridColumns = null; // default value
    container97.GridRows = null; // default value
    container97.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container97.RowGap = null; // default value
    container97.RowSpan = null; // default value
    container97.SetStyle("\"cp-detail-note\"");
    container97.SetVisible("True"); // default value
    container97.VisibleWasExpression = false; // default value
    container97.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container97.Width = "(fill parent)"; // default value
    
    var textWidget20 = container97.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget20.CustomStyle = null; // default value
    textWidget20.SetStyleClasses(null); // default value
    textWidget20.Text = "Each lot type is reported separately by data.gov.sg; they are not added together.";
    
    var ifWidget10 = container89.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget10.Animate = false; // default value
    ifWidget10.SetCondition("DetailRows[0].HasLocation");
    ifWidget10.CustomStyle = null; // default value
    ifWidget10.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container98 = ifWidget10.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container98.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container98.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container98.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container98.Animate = false; // default value
    container98.ColumnGap = null; // default value
    container98.ColumnSpan = null; // default value
    container98.CustomStyle = null; // default value
    container98.FlexBasis = null; // default value
    container98.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container98.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container98.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container98.Gap = null;
    container98.GridColumns = null; // default value
    container98.GridRows = null; // default value
    container98.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container98.RowGap = null; // default value
    container98.RowSpan = null; // default value
    container98.SetStyle("\"cp-detail-section\"");
    container98.SetVisible("True"); // default value
    container98.VisibleWasExpression = false; // default value
    container98.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container98.Width = "(fill parent)"; // default value
    
    var container99 = container98.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container99.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container99.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container99.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container99.Animate = false; // default value
    container99.ColumnGap = null; // default value
    container99.ColumnSpan = null; // default value
    container99.CustomStyle = null; // default value
    container99.FlexBasis = null; // default value
    container99.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container99.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container99.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container99.Gap = null;
    container99.GridColumns = null; // default value
    container99.GridRows = null; // default value
    container99.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container99.RowGap = null; // default value
    container99.RowSpan = null; // default value
    container99.SetStyle("\"cp-detail-subtitle\"");
    container99.SetVisible("True"); // default value
    container99.VisibleWasExpression = false; // default value
    container99.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container99.Width = "(fill parent)"; // default value
    
    var textWidget21 = container99.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget21.CustomStyle = null; // default value
    textWidget21.SetStyleClasses(null); // default value
    textWidget21.Text = "Carpark information";
    
    var extendedProperty29 = container99.CreateExtendedProperty();
    extendedProperty29.Property = "role";
    extendedProperty29.SetValue("\"heading\"");
    
    var extendedProperty30 = container99.CreateExtendedProperty();
    extendedProperty30.Property = "aria-level";
    extendedProperty30.SetValue("\"3\"");
    
    var container100 = container98.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container100.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container100.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container100.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container100.Animate = false; // default value
    container100.ColumnGap = null; // default value
    container100.ColumnSpan = null; // default value
    container100.CustomStyle = null; // default value
    container100.FlexBasis = null; // default value
    container100.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container100.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container100.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container100.Gap = null;
    container100.GridColumns = null; // default value
    container100.GridRows = null; // default value
    container100.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container100.RowGap = null; // default value
    container100.RowSpan = null; // default value
    container100.SetStyle("\"cp-kv\"");
    container100.SetVisible("True"); // default value
    container100.VisibleWasExpression = false; // default value
    container100.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container100.Width = "(fill parent)"; // default value
    
    var container101 = container100.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container101.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container101.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container101.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container101.Animate = false; // default value
    container101.ColumnGap = null; // default value
    container101.ColumnSpan = null; // default value
    container101.CustomStyle = null; // default value
    container101.FlexBasis = null; // default value
    container101.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container101.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container101.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container101.Gap = null;
    container101.GridColumns = null; // default value
    container101.GridRows = null; // default value
    container101.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container101.RowGap = null; // default value
    container101.RowSpan = null; // default value
    container101.SetStyle("\"cp-k\"");
    container101.SetVisible("True"); // default value
    container101.VisibleWasExpression = false; // default value
    container101.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container101.Width = "(fill parent)"; // default value
    
    var textWidget22 = container101.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget22.CustomStyle = null; // default value
    textWidget22.SetStyleClasses(null); // default value
    textWidget22.Text = "Carpark type";
    
    var container102 = container100.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container102.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container102.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container102.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container102.Animate = false; // default value
    container102.ColumnGap = null; // default value
    container102.ColumnSpan = null; // default value
    container102.CustomStyle = null; // default value
    container102.FlexBasis = null; // default value
    container102.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container102.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container102.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container102.Gap = null;
    container102.GridColumns = null; // default value
    container102.GridRows = null; // default value
    container102.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container102.RowGap = null; // default value
    container102.RowSpan = null; // default value
    container102.SetStyle("\"cp-v\"");
    container102.SetVisible("True"); // default value
    container102.VisibleWasExpression = false; // default value
    container102.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container102.Width = "(fill parent)"; // default value
    
    var expression40 = container102.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression40.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression40.ColumnSpan = null; // default value
    expression40.CustomStyle = null; // default value
    expression40.Example = null; // default value
    expression40.FlexBasis = null; // default value
    expression40.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression40.RowSpan = null; // default value
    expression40.SetStyle(null); // default value
    expression40.SetValue("If(DetailRows[0].CarparkType = \"\", \"Not stated\", DetailRows[0].CarparkType)");
    expression40.Width = ""; // default value
    
    var container103 = container98.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container103.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container103.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container103.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container103.Animate = false; // default value
    container103.ColumnGap = null; // default value
    container103.ColumnSpan = null; // default value
    container103.CustomStyle = null; // default value
    container103.FlexBasis = null; // default value
    container103.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container103.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container103.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container103.Gap = null;
    container103.GridColumns = null; // default value
    container103.GridRows = null; // default value
    container103.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container103.RowGap = null; // default value
    container103.RowSpan = null; // default value
    container103.SetStyle("\"cp-kv\"");
    container103.SetVisible("True"); // default value
    container103.VisibleWasExpression = false; // default value
    container103.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container103.Width = "(fill parent)"; // default value
    
    var container104 = container103.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container104.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container104.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container104.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container104.Animate = false; // default value
    container104.ColumnGap = null; // default value
    container104.ColumnSpan = null; // default value
    container104.CustomStyle = null; // default value
    container104.FlexBasis = null; // default value
    container104.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container104.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container104.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container104.Gap = null;
    container104.GridColumns = null; // default value
    container104.GridRows = null; // default value
    container104.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container104.RowGap = null; // default value
    container104.RowSpan = null; // default value
    container104.SetStyle("\"cp-k\"");
    container104.SetVisible("True"); // default value
    container104.VisibleWasExpression = false; // default value
    container104.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container104.Width = "(fill parent)"; // default value
    
    var textWidget23 = container104.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget23.CustomStyle = null; // default value
    textWidget23.SetStyleClasses(null); // default value
    textWidget23.Text = "Parking system";
    
    var container105 = container103.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container105.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container105.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container105.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container105.Animate = false; // default value
    container105.ColumnGap = null; // default value
    container105.ColumnSpan = null; // default value
    container105.CustomStyle = null; // default value
    container105.FlexBasis = null; // default value
    container105.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container105.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container105.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container105.Gap = null;
    container105.GridColumns = null; // default value
    container105.GridRows = null; // default value
    container105.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container105.RowGap = null; // default value
    container105.RowSpan = null; // default value
    container105.SetStyle("\"cp-v\"");
    container105.SetVisible("True"); // default value
    container105.VisibleWasExpression = false; // default value
    container105.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container105.Width = "(fill parent)"; // default value
    
    var expression41 = container105.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression41.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression41.ColumnSpan = null; // default value
    expression41.CustomStyle = null; // default value
    expression41.Example = null; // default value
    expression41.FlexBasis = null; // default value
    expression41.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression41.RowSpan = null; // default value
    expression41.SetStyle(null); // default value
    expression41.SetValue("If(DetailRows[0].ParkingSystem = \"\", \"Not stated\", DetailRows[0].ParkingSystem)");
    expression41.Width = ""; // default value
    
    var container106 = container98.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container106.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container106.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container106.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container106.Animate = false; // default value
    container106.ColumnGap = null; // default value
    container106.ColumnSpan = null; // default value
    container106.CustomStyle = null; // default value
    container106.FlexBasis = null; // default value
    container106.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container106.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container106.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container106.Gap = null;
    container106.GridColumns = null; // default value
    container106.GridRows = null; // default value
    container106.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container106.RowGap = null; // default value
    container106.RowSpan = null; // default value
    container106.SetStyle("\"cp-kv\"");
    container106.SetVisible("True"); // default value
    container106.VisibleWasExpression = false; // default value
    container106.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container106.Width = "(fill parent)"; // default value
    
    var container107 = container106.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container107.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container107.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container107.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container107.Animate = false; // default value
    container107.ColumnGap = null; // default value
    container107.ColumnSpan = null; // default value
    container107.CustomStyle = null; // default value
    container107.FlexBasis = null; // default value
    container107.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container107.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container107.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container107.Gap = null;
    container107.GridColumns = null; // default value
    container107.GridRows = null; // default value
    container107.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container107.RowGap = null; // default value
    container107.RowSpan = null; // default value
    container107.SetStyle("\"cp-k\"");
    container107.SetVisible("True"); // default value
    container107.VisibleWasExpression = false; // default value
    container107.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container107.Width = "(fill parent)"; // default value
    
    var textWidget24 = container107.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget24.CustomStyle = null; // default value
    textWidget24.SetStyleClasses(null); // default value
    textWidget24.Text = "Short-term parking";
    
    var container108 = container106.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container108.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container108.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container108.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container108.Animate = false; // default value
    container108.ColumnGap = null; // default value
    container108.ColumnSpan = null; // default value
    container108.CustomStyle = null; // default value
    container108.FlexBasis = null; // default value
    container108.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container108.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container108.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container108.Gap = null;
    container108.GridColumns = null; // default value
    container108.GridRows = null; // default value
    container108.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container108.RowGap = null; // default value
    container108.RowSpan = null; // default value
    container108.SetStyle("\"cp-v\"");
    container108.SetVisible("True"); // default value
    container108.VisibleWasExpression = false; // default value
    container108.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container108.Width = "(fill parent)"; // default value
    
    var expression42 = container108.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression42.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression42.ColumnSpan = null; // default value
    expression42.CustomStyle = null; // default value
    expression42.Example = null; // default value
    expression42.FlexBasis = null; // default value
    expression42.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression42.RowSpan = null; // default value
    expression42.SetStyle(null); // default value
    expression42.SetValue("If(DetailRows[0].ShortTermParking = \"\", \"Not stated\", DetailRows[0].ShortTermParking)");
    expression42.Width = ""; // default value
    
    var container109 = container98.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container109.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container109.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container109.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container109.Animate = false; // default value
    container109.ColumnGap = null; // default value
    container109.ColumnSpan = null; // default value
    container109.CustomStyle = null; // default value
    container109.FlexBasis = null; // default value
    container109.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container109.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container109.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container109.Gap = null;
    container109.GridColumns = null; // default value
    container109.GridRows = null; // default value
    container109.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container109.RowGap = null; // default value
    container109.RowSpan = null; // default value
    container109.SetStyle("\"cp-kv\"");
    container109.SetVisible("True"); // default value
    container109.VisibleWasExpression = false; // default value
    container109.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container109.Width = "(fill parent)"; // default value
    
    var container110 = container109.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container110.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container110.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container110.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container110.Animate = false; // default value
    container110.ColumnGap = null; // default value
    container110.ColumnSpan = null; // default value
    container110.CustomStyle = null; // default value
    container110.FlexBasis = null; // default value
    container110.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container110.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container110.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container110.Gap = null;
    container110.GridColumns = null; // default value
    container110.GridRows = null; // default value
    container110.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container110.RowGap = null; // default value
    container110.RowSpan = null; // default value
    container110.SetStyle("\"cp-k\"");
    container110.SetVisible("True"); // default value
    container110.VisibleWasExpression = false; // default value
    container110.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container110.Width = "(fill parent)"; // default value
    
    var textWidget25 = container110.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget25.CustomStyle = null; // default value
    textWidget25.SetStyleClasses(null); // default value
    textWidget25.Text = "Free parking";
    
    var container111 = container109.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container111.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container111.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container111.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container111.Animate = false; // default value
    container111.ColumnGap = null; // default value
    container111.ColumnSpan = null; // default value
    container111.CustomStyle = null; // default value
    container111.FlexBasis = null; // default value
    container111.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container111.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container111.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container111.Gap = null;
    container111.GridColumns = null; // default value
    container111.GridRows = null; // default value
    container111.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container111.RowGap = null; // default value
    container111.RowSpan = null; // default value
    container111.SetStyle("\"cp-v\"");
    container111.SetVisible("True"); // default value
    container111.VisibleWasExpression = false; // default value
    container111.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container111.Width = "(fill parent)"; // default value
    
    var expression43 = container111.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression43.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression43.ColumnSpan = null; // default value
    expression43.CustomStyle = null; // default value
    expression43.Example = null; // default value
    expression43.FlexBasis = null; // default value
    expression43.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression43.RowSpan = null; // default value
    expression43.SetStyle(null); // default value
    expression43.SetValue("If(DetailRows[0].FreeParking = \"\", \"Not stated\", DetailRows[0].FreeParking)");
    expression43.Width = ""; // default value
    
    var container112 = container98.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container112.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container112.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container112.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container112.Animate = false; // default value
    container112.ColumnGap = null; // default value
    container112.ColumnSpan = null; // default value
    container112.CustomStyle = null; // default value
    container112.FlexBasis = null; // default value
    container112.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container112.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container112.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container112.Gap = null;
    container112.GridColumns = null; // default value
    container112.GridRows = null; // default value
    container112.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container112.RowGap = null; // default value
    container112.RowSpan = null; // default value
    container112.SetStyle("\"cp-kv\"");
    container112.SetVisible("True"); // default value
    container112.VisibleWasExpression = false; // default value
    container112.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container112.Width = "(fill parent)"; // default value
    
    var container113 = container112.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container113.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container113.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container113.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container113.Animate = false; // default value
    container113.ColumnGap = null; // default value
    container113.ColumnSpan = null; // default value
    container113.CustomStyle = null; // default value
    container113.FlexBasis = null; // default value
    container113.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container113.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container113.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container113.Gap = null;
    container113.GridColumns = null; // default value
    container113.GridRows = null; // default value
    container113.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container113.RowGap = null; // default value
    container113.RowSpan = null; // default value
    container113.SetStyle("\"cp-k\"");
    container113.SetVisible("True"); // default value
    container113.VisibleWasExpression = false; // default value
    container113.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container113.Width = "(fill parent)"; // default value
    
    var textWidget26 = container113.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget26.CustomStyle = null; // default value
    textWidget26.SetStyleClasses(null); // default value
    textWidget26.Text = "Night parking";
    
    var container114 = container112.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container114.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container114.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container114.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container114.Animate = false; // default value
    container114.ColumnGap = null; // default value
    container114.ColumnSpan = null; // default value
    container114.CustomStyle = null; // default value
    container114.FlexBasis = null; // default value
    container114.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container114.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container114.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container114.Gap = null;
    container114.GridColumns = null; // default value
    container114.GridRows = null; // default value
    container114.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container114.RowGap = null; // default value
    container114.RowSpan = null; // default value
    container114.SetStyle("\"cp-v\"");
    container114.SetVisible("True"); // default value
    container114.VisibleWasExpression = false; // default value
    container114.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container114.Width = "(fill parent)"; // default value
    
    var expression44 = container114.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression44.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression44.ColumnSpan = null; // default value
    expression44.CustomStyle = null; // default value
    expression44.Example = null; // default value
    expression44.FlexBasis = null; // default value
    expression44.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression44.RowSpan = null; // default value
    expression44.SetStyle(null); // default value
    expression44.SetValue("If(DetailRows[0].NightParking = \"\", \"Not stated\", DetailRows[0].NightParking)");
    expression44.Width = ""; // default value
    
    var container115 = container98.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container115.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container115.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container115.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container115.Animate = false; // default value
    container115.ColumnGap = null; // default value
    container115.ColumnSpan = null; // default value
    container115.CustomStyle = null; // default value
    container115.FlexBasis = null; // default value
    container115.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container115.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container115.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container115.Gap = null;
    container115.GridColumns = null; // default value
    container115.GridRows = null; // default value
    container115.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container115.RowGap = null; // default value
    container115.RowSpan = null; // default value
    container115.SetStyle("\"cp-kv\"");
    container115.SetVisible("True"); // default value
    container115.VisibleWasExpression = false; // default value
    container115.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container115.Width = "(fill parent)"; // default value
    
    var container116 = container115.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container116.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container116.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container116.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container116.Animate = false; // default value
    container116.ColumnGap = null; // default value
    container116.ColumnSpan = null; // default value
    container116.CustomStyle = null; // default value
    container116.FlexBasis = null; // default value
    container116.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container116.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container116.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container116.Gap = null;
    container116.GridColumns = null; // default value
    container116.GridRows = null; // default value
    container116.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container116.RowGap = null; // default value
    container116.RowSpan = null; // default value
    container116.SetStyle("\"cp-k\"");
    container116.SetVisible("True"); // default value
    container116.VisibleWasExpression = false; // default value
    container116.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container116.Width = "(fill parent)"; // default value
    
    var textWidget27 = container116.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget27.CustomStyle = null; // default value
    textWidget27.SetStyleClasses(null); // default value
    textWidget27.Text = "Gantry height (m)";
    
    var container117 = container115.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container117.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container117.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container117.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container117.Animate = false; // default value
    container117.ColumnGap = null; // default value
    container117.ColumnSpan = null; // default value
    container117.CustomStyle = null; // default value
    container117.FlexBasis = null; // default value
    container117.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container117.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container117.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container117.Gap = null;
    container117.GridColumns = null; // default value
    container117.GridRows = null; // default value
    container117.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container117.RowGap = null; // default value
    container117.RowSpan = null; // default value
    container117.SetStyle("\"cp-v\"");
    container117.SetVisible("True"); // default value
    container117.VisibleWasExpression = false; // default value
    container117.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container117.Width = "(fill parent)"; // default value
    
    var expression45 = container117.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression45.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression45.ColumnSpan = null; // default value
    expression45.CustomStyle = null; // default value
    expression45.Example = null; // default value
    expression45.FlexBasis = null; // default value
    expression45.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression45.RowSpan = null; // default value
    expression45.SetStyle(null); // default value
    expression45.SetValue("If(DetailRows[0].GantryHeight = \"\", \"Not stated\", DetailRows[0].GantryHeight)");
    expression45.Width = ""; // default value
    
    var container118 = container89.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container118.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container118.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container118.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container118.Animate = false; // default value
    container118.ColumnGap = null; // default value
    container118.ColumnSpan = null; // default value
    container118.CustomStyle = null; // default value
    container118.FlexBasis = null; // default value
    container118.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container118.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container118.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container118.Gap = null;
    container118.GridColumns = null; // default value
    container118.GridRows = null; // default value
    container118.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container118.RowGap = null; // default value
    container118.RowSpan = null; // default value
    container118.SetStyle("\"cp-detail-section\"");
    container118.SetVisible("True"); // default value
    container118.VisibleWasExpression = false; // default value
    container118.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container118.Width = "(fill parent)"; // default value
    
    var container119 = container118.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container119.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container119.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container119.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container119.Animate = false; // default value
    container119.ColumnGap = null; // default value
    container119.ColumnSpan = null; // default value
    container119.CustomStyle = null; // default value
    container119.FlexBasis = null; // default value
    container119.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container119.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container119.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container119.Gap = null;
    container119.GridColumns = null; // default value
    container119.GridRows = null; // default value
    container119.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container119.RowGap = null; // default value
    container119.RowSpan = null; // default value
    container119.SetStyle("\"cp-detail-subtitle\"");
    container119.SetVisible("True"); // default value
    container119.VisibleWasExpression = false; // default value
    container119.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container119.Width = "(fill parent)"; // default value
    
    var textWidget28 = container119.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget28.CustomStyle = null; // default value
    textWidget28.SetStyleClasses(null); // default value
    textWidget28.Text = "Update information";
    
    var extendedProperty31 = container119.CreateExtendedProperty();
    extendedProperty31.Property = "role";
    extendedProperty31.SetValue("\"heading\"");
    
    var extendedProperty32 = container119.CreateExtendedProperty();
    extendedProperty32.Property = "aria-level";
    extendedProperty32.SetValue("\"3\"");
    
    var container120 = container118.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container120.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container120.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container120.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container120.Animate = false; // default value
    container120.ColumnGap = null; // default value
    container120.ColumnSpan = null; // default value
    container120.CustomStyle = null; // default value
    container120.FlexBasis = null; // default value
    container120.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container120.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container120.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container120.Gap = null;
    container120.GridColumns = null; // default value
    container120.GridRows = null; // default value
    container120.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container120.RowGap = null; // default value
    container120.RowSpan = null; // default value
    container120.SetStyle("\"cp-detail-meta\"");
    container120.SetVisible("True"); // default value
    container120.VisibleWasExpression = false; // default value
    container120.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container120.Width = "(fill parent)"; // default value
    
    var expression46 = container120.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression46.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression46.ColumnSpan = null; // default value
    expression46.CustomStyle = null; // default value
    expression46.Example = null; // default value
    expression46.FlexBasis = null; // default value
    expression46.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression46.RowSpan = null; // default value
    expression46.SetStyle(null); // default value
    expression46.SetValue("\"This carpark\'s reading was updated \" + DetailRows[0].UpdatedAtText + \" SGT.\"");
    expression46.Width = ""; // default value
    
    var container121 = container118.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container121.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container121.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container121.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container121.Animate = false; // default value
    container121.ColumnGap = null; // default value
    container121.ColumnSpan = null; // default value
    container121.CustomStyle = null; // default value
    container121.FlexBasis = null; // default value
    container121.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container121.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container121.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container121.Gap = null;
    container121.GridColumns = null; // default value
    container121.GridRows = null; // default value
    container121.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container121.RowGap = null; // default value
    container121.RowSpan = null; // default value
    container121.SetStyle("\"cp-detail-meta\"");
    container121.SetVisible("True"); // default value
    container121.VisibleWasExpression = false; // default value
    container121.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container121.Width = "(fill parent)"; // default value
    
    var expression47 = container121.CreateWidget<ServiceStudio.Plugin.NRWidgets.IExpression>();
    expression47.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    expression47.ColumnSpan = null; // default value
    expression47.CustomStyle = null; // default value
    expression47.Example = null; // default value
    expression47.FlexBasis = null; // default value
    expression47.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    expression47.RowSpan = null; // default value
    expression47.SetStyle(null); // default value
    expression47.SetValue("\"The dataset is as at \" + Meta.SourceTimestampText + \" SGT (\" + If(Freshness = \"Live\", \"live\", ToLower(Freshness)) + \").\"");
    expression47.Width = ""; // default value
    
    var ifWidget11 = container118.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.IIfWidget>();
    ifWidget11.Animate = false; // default value
    ifWidget11.SetCondition("DetailRows[0].IsReadingOutdated");
    ifWidget11.CustomStyle = null; // default value
    ifWidget11.DesignMode = OutSystems.Model.Enumerations.DesignMode.ShowAll; // default value
    
    var container122 = ifWidget11.TrueBranch.CreateWidget<ServiceStudio.Plugin.NRWidgets.IContainer>();
    container122.Align = ServiceStudio.Plugin.NRWidgets.Enumerations.Align.Default; // default value
    container122.AlignItems = OutSystems.Model.Enumerations.AlignItems.Stretch; // default value
    container122.AlignSelf = OutSystems.Model.Enumerations.AlignSelf.Auto; // default value
    container122.Animate = false; // default value
    container122.ColumnGap = null; // default value
    container122.ColumnSpan = null; // default value
    container122.CustomStyle = null; // default value
    container122.FlexBasis = null; // default value
    container122.FlexDirection = OutSystems.Model.Enumerations.FlexDirection.Row; // default value
    container122.Flexible = OutSystems.Model.Enumerations.Flexible.Shrink; // default value
    container122.FlexWrap = OutSystems.Model.Enumerations.FlexWrap.NoWrap; // default value
    container122.Gap = null;
    container122.GridColumns = null; // default value
    container122.GridRows = null; // default value
    container122.JustifyContent = OutSystems.Model.Enumerations.JustifyContent.Start; // default value
    container122.RowGap = null; // default value
    container122.RowSpan = null; // default value
    container122.SetStyle("\"cp-flag cp-flag-outdated\"");
    container122.SetVisible("True"); // default value
    container122.VisibleWasExpression = false; // default value
    container122.WidgetGridType = OutSystems.Model.Enumerations.WidgetGridType.None; // default value
    container122.Width = "(fill parent)"; // default value
    
    var textWidget29 = container122.CreateWidget<OutSystems.Model.UI.Mobile.Widgets.ITextWidget>();
    textWidget29.CustomStyle = null; // default value
    textWidget29.SetStyleClasses(null); // default value
    textWidget29.Text = "This carpark\'s reading is much older than the rest of the dataset and may not reflect current availability.";
    
    var extendedProperty33 = container85.CreateExtendedProperty();
    extendedProperty33.Property = "role";
    extendedProperty33.SetValue("\"dialog\"");
    
    var extendedProperty34 = container85.CreateExtendedProperty();
    extendedProperty34.Property = "aria-modal";
    extendedProperty34.SetValue("\"true\"");
    
    var extendedProperty35 = container85.CreateExtendedProperty();
    extendedProperty35.Property = "aria-label";
    extendedProperty35.SetValue("\"Carpark \" + SelectedCarpark + \" details\"");
    
    var extendedProperty36 = detailPopup.CreateExtendedProperty();
    extendedProperty36.Property = "aria-label";
    extendedProperty36.SetValue("\"Carpark details\"");
}
