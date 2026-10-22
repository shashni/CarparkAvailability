(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating IStructure 'AvailabilityMeta' ***/
    var availabilityMeta = eSpace.CreateStructure("AvailabilityMeta");
    availabilityMeta.Description = "Freshness and integrity metadata returned alongside the carpark rows.";
    availabilityMeta.Folder = null; // default value
    availabilityMeta.Public = false; // default value
    availabilityMeta.Shareable = false; // default value
    
    var freshness = availabilityMeta.CreateAttribute("Freshness");
    freshness.DataType = eSpace.TextType;
    freshness.SetDefaultValue(null); // default value
    freshness.Description = null; // default value
    freshness.IsMandatory = false; // default value
    freshness.Label = "Freshness";
    freshness.Length = 20;
    freshness.NameInJSON = null; // default value
    
    var hasData = availabilityMeta.CreateAttribute("HasData");
    hasData.DataType = eSpace.BooleanType;
    hasData.SetDefaultValue(null); // default value
    hasData.Description = null; // default value
    hasData.IsMandatory = false; // default value
    hasData.Label = "Has Data";
    hasData.NameInJSON = null; // default value
    
    var sourceTimestamp = availabilityMeta.CreateAttribute("SourceTimestamp");
    sourceTimestamp.DataType = eSpace.DateTimeType;
    sourceTimestamp.SetDefaultValue(null); // default value
    sourceTimestamp.Description = null; // default value
    sourceTimestamp.IsMandatory = false; // default value
    sourceTimestamp.Label = "Source Timestamp";
    sourceTimestamp.NameInJSON = null; // default value
    
    var dataAgeSeconds = availabilityMeta.CreateAttribute("DataAgeSeconds");
    dataAgeSeconds.DataType = eSpace.IntegerType;
    dataAgeSeconds.SetDefaultValue(null); // default value
    dataAgeSeconds.Description = null; // default value
    dataAgeSeconds.IsMandatory = false; // default value
    dataAgeSeconds.Label = "Data Age Seconds";
    dataAgeSeconds.NameInJSON = null; // default value
    
    var staleAfterMinutes = availabilityMeta.CreateAttribute("StaleAfterMinutes");
    staleAfterMinutes.DataType = eSpace.IntegerType;
    staleAfterMinutes.SetDefaultValue(null); // default value
    staleAfterMinutes.Description = null; // default value
    staleAfterMinutes.IsMandatory = false; // default value
    staleAfterMinutes.Label = "Stale After Minutes";
    staleAfterMinutes.NameInJSON = null; // default value
    
    var carparkCount = availabilityMeta.CreateAttribute("CarparkCount");
    carparkCount.DataType = eSpace.IntegerType;
    carparkCount.SetDefaultValue(null); // default value
    carparkCount.Description = null; // default value
    carparkCount.IsMandatory = false; // default value
    carparkCount.Label = "Carpark Count";
    carparkCount.NameInJSON = null; // default value
    
    var rejectedCount = availabilityMeta.CreateAttribute("RejectedCount");
    rejectedCount.DataType = eSpace.IntegerType;
    rejectedCount.SetDefaultValue(null); // default value
    rejectedCount.Description = null; // default value
    rejectedCount.IsMandatory = false; // default value
    rejectedCount.Label = "Rejected Count";
    rejectedCount.NameInJSON = null; // default value
    
    var duplicateCount = availabilityMeta.CreateAttribute("DuplicateCount");
    duplicateCount.DataType = eSpace.IntegerType;
    duplicateCount.SetDefaultValue(null); // default value
    duplicateCount.Description = null; // default value
    duplicateCount.IsMandatory = false; // default value
    duplicateCount.Label = "Duplicate Count";
    duplicateCount.NameInJSON = null; // default value
    
    var anomalyCount = availabilityMeta.CreateAttribute("AnomalyCount");
    anomalyCount.DataType = eSpace.IntegerType;
    anomalyCount.SetDefaultValue(null); // default value
    anomalyCount.Description = null; // default value
    anomalyCount.IsMandatory = false; // default value
    anomalyCount.Label = "Anomaly Count";
    anomalyCount.NameInJSON = null; // default value
    
    var isPartial = availabilityMeta.CreateAttribute("IsPartial");
    isPartial.DataType = eSpace.BooleanType;
    isPartial.SetDefaultValue(null); // default value
    isPartial.Description = null; // default value
    isPartial.IsMandatory = false; // default value
    isPartial.Label = "Is Partial";
    isPartial.NameInJSON = null; // default value
    
    var hasLocationData = availabilityMeta.CreateAttribute("HasLocationData");
    hasLocationData.DataType = eSpace.BooleanType;
    hasLocationData.SetDefaultValue(null); // default value
    hasLocationData.Description = null; // default value
    hasLocationData.IsMandatory = false; // default value
    hasLocationData.Label = "Has Location Data";
    hasLocationData.NameInJSON = null; // default value
    
    var message = availabilityMeta.CreateAttribute("Message");
    message.DataType = eSpace.TextType;
    message.SetDefaultValue(null); // default value
    message.Description = null; // default value
    message.IsMandatory = false; // default value
    message.Label = "Message";
    message.Length = 300;
    message.NameInJSON = null; // default value
    
    var sourceTimestampText = availabilityMeta.CreateAttribute("SourceTimestampText");
    sourceTimestampText.DataType = eSpace.TextType;
    sourceTimestampText.SetDefaultValue(null); // default value
    sourceTimestampText.Description = null; // default value
    sourceTimestampText.IsMandatory = false; // default value
    sourceTimestampText.Label = "Source Timestamp Text";
    sourceTimestampText.Length = 40;
    sourceTimestampText.NameInJSON = null; // default value
    
    var thresholdSummary = availabilityMeta.CreateAttribute("ThresholdSummary");
    thresholdSummary.DataType = eSpace.TextType;
    thresholdSummary.SetDefaultValue(null); // default value
    thresholdSummary.Description = null; // default value
    thresholdSummary.IsMandatory = false; // default value
    thresholdSummary.Label = "Threshold Summary";
    thresholdSummary.Length = 300;
    thresholdSummary.NameInJSON = null; // default value
    // IStructureSignature OutSystemsCharts/AxisLabel
    var axisLabel = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*PMnEbiR+8UWA9TPQv0lzIw"));
    // IStructureSignature OutSystemsUI/BulkActions
    var bulkActions = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*Nin0Z0JPKUedfb_eUCXCnA"));
    // IStructureSignature OutSystemsUI/CarouselItems
    var carouselItems = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*3HUi56hX10erbaugSaKF9g"));
    // IStructureSignature OutSystemsUI/CarouselOptionalConfigs
    var carouselOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*yukJtDn+o0uipSQPZip3mQ"));
    
    /*** creating IStructure 'CarparkLotRow' ***/
    var carparkLotRow = eSpace.CreateStructure("CarparkLotRow");
    carparkLotRow.Description = "Validated availability of one lot type at one carpark. A carpark with several lot types has several rows; exactly one row per carpark has IsPrimary = True (its Car lots when present, otherwise its first valid lot type).";
    carparkLotRow.Folder = null; // default value
    carparkLotRow.Public = false; // default value
    carparkLotRow.Shareable = false; // default value
    
    var carparkNumber = carparkLotRow.CreateAttribute("CarparkNumber");
    carparkNumber.DataType = eSpace.TextType;
    carparkNumber.SetDefaultValue(null); // default value
    carparkNumber.Description = null; // default value
    carparkNumber.IsMandatory = false; // default value
    carparkNumber.Label = "Carpark Number";
    carparkNumber.Length = 20;
    carparkNumber.NameInJSON = null; // default value
    
    var lotTypeCode = carparkLotRow.CreateAttribute("LotTypeCode");
    lotTypeCode.DataType = eSpace.TextType;
    lotTypeCode.SetDefaultValue(null); // default value
    lotTypeCode.Description = null; // default value
    lotTypeCode.IsMandatory = false; // default value
    lotTypeCode.Label = "Lot Type Code";
    lotTypeCode.Length = 5;
    lotTypeCode.NameInJSON = null; // default value
    
    var lotTypeLabel = carparkLotRow.CreateAttribute("LotTypeLabel");
    lotTypeLabel.DataType = eSpace.TextType;
    lotTypeLabel.SetDefaultValue(null); // default value
    lotTypeLabel.Description = null; // default value
    lotTypeLabel.IsMandatory = false; // default value
    lotTypeLabel.Label = "Lot Type Label";
    lotTypeLabel.Length = 50;
    lotTypeLabel.NameInJSON = null; // default value
    
    var totalLots = carparkLotRow.CreateAttribute("TotalLots");
    totalLots.DataType = eSpace.IntegerType;
    totalLots.SetDefaultValue(null); // default value
    totalLots.Description = null; // default value
    totalLots.IsMandatory = false; // default value
    totalLots.Label = "Total Lots";
    totalLots.NameInJSON = null; // default value
    
    var availableLots = carparkLotRow.CreateAttribute("AvailableLots");
    availableLots.DataType = eSpace.IntegerType;
    availableLots.SetDefaultValue(null); // default value
    availableLots.Description = null; // default value
    availableLots.IsMandatory = false; // default value
    availableLots.Label = "Available Lots";
    availableLots.NameInJSON = null; // default value
    
    var availabilityPct = carparkLotRow.CreateAttribute("AvailabilityPct");
    availabilityPct.DataType = eSpace.DecimalType;
    availabilityPct.Decimals = null; // default value
    availabilityPct.SetDefaultValue(null); // default value
    availabilityPct.Description = null; // default value
    availabilityPct.IsMandatory = false; // default value
    availabilityPct.Label = "Availability Pct";
    availabilityPct.Length = null; // default value
    availabilityPct.NameInJSON = null; // default value
    
    var statusId = carparkLotRow.CreateAttribute("StatusId");
    statusId.SetDefaultValue(null); // default value
    statusId.Description = null; // default value
    statusId.IsMandatory = false; // default value
    statusId.Label = "Status";
    statusId.NameInJSON = null; // default value
    
    /*** creating IStaticEntity 'AvailabilityStatus' ***/
    var availabilityStatus = eSpace.CreateStaticEntity("AvailabilityStatus");
    availabilityStatus.Description = "Availability bands shown on the dashboard. The numeric thresholds live in the site properties HighAvailabilityMinPct and LowAvailabilityMaxPct and are applied only in AvailabilityStatus_Classify.";
    availabilityStatus.ExposeProcessEvents = false; // default value
    availabilityStatus.Folder = null; // default value
    availabilityStatus.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.No; // default value
    availabilityStatus.IsRestrictedInQueries = false; // default value
    availabilityStatus.Label = "Availability Status";
    availabilityStatus.LabelPlural = "Availability Statuses";
    availabilityStatus.Public = false; // default value
    availabilityStatus.Shareable = false; // default value
    availabilityStatus.UpdateBehavior = OutSystems.Model.Enumerations.UpdateEntityBehaviorType.Optimized;
    availabilityStatus.UseTranslations = true;
    
    var id = availabilityStatus.CreateAttribute("Id");
    id.DataType = eSpace.TextType;
    id.SetDefaultValue(null); // default value
    id.Description = null; // default value
    id.IsSearchable = false; // default value
    id.Label = "Id";
    id.Length = 20;
    id.OriginalName = null; // default value
    id.OriginalType = null; // default value
    
    var label = availabilityStatus.CreateAttribute("Label");
    label.DataType = eSpace.TextType;
    label.SetDefaultValue(null); // default value
    label.Description = null; // default value
    label.IsMandatory = true;
    label.IsSearchable = false; // default value
    label.Label = "Label";
    label.Length = 50;
    label.OriginalName = null; // default value
    label.OriginalType = null; // default value
    
    var description = availabilityStatus.CreateAttribute("Description");
    description.DataType = eSpace.TextType;
    description.SetDefaultValue(null); // default value
    description.Description = null; // default value
    description.IsMandatory = false; // default value
    description.IsSearchable = false; // default value
    description.Label = "Description";
    description.Length = 200;
    description.OriginalName = null; // default value
    description.OriginalType = null; // default value
    
    var styleClass = availabilityStatus.CreateAttribute("StyleClass");
    styleClass.DataType = eSpace.TextType;
    styleClass.SetDefaultValue(null); // default value
    styleClass.Description = null; // default value
    styleClass.IsMandatory = false; // default value
    styleClass.IsSearchable = false; // default value
    styleClass.Label = "Style Class";
    styleClass.Length = 50;
    styleClass.OriginalName = null; // default value
    styleClass.OriginalType = null; // default value
    
    var icon = availabilityStatus.CreateAttribute("Icon");
    icon.DataType = eSpace.TextType;
    icon.SetDefaultValue(null); // default value
    icon.Description = null; // default value
    icon.IsMandatory = false; // default value
    icon.IsSearchable = false; // default value
    icon.Label = "Icon";
    icon.Length = 30;
    icon.OriginalName = null; // default value
    icon.OriginalType = null; // default value
    
    var order = availabilityStatus.CreateAttribute("Order");
    order.DataType = eSpace.IntegerType;
    order.SetDefaultValue(null); // default value
    order.Description = null; // default value
    order.IsMandatory = true;
    order.IsSearchable = false; // default value
    order.Label = "Order";
    order.OriginalName = null; // default value
    order.OriginalType = null; // default value
    
    var is_Active = availabilityStatus.CreateAttribute("Is_Active");
    is_Active.DataType = eSpace.BooleanType;
    is_Active.SetDefaultValue(null); // default value
    is_Active.Description = null; // default value
    is_Active.IsMandatory = true;
    is_Active.IsSearchable = false; // default value
    is_Active.Label = "Is Active";
    is_Active.OriginalName = null; // default value
    is_Active.OriginalType = null; // default value
    
    var staticEntityRecord = availabilityStatus.CreateRecord();
    staticEntityRecord.Icon = null; // default value
    staticEntityRecord.Identifier = "High";
    staticEntityRecord.SetAttributeValue(is_Active, "True");
    staticEntityRecord.SetAttributeValue(styleClass, "\"cp-status-high\"");
    staticEntityRecord.SetAttributeValue(icon, "\"check-circle\"");
    staticEntityRecord.SetAttributeValue(label, "\"High availability\"");
    staticEntityRecord.SetAttributeValue(description, "\"More than the high threshold of lots available\"");
    staticEntityRecord.SetAttributeValue(order, "1");
    staticEntityRecord.SetAttributeValue(id, "\"High\"");
    
    var staticEntityRecord2 = availabilityStatus.CreateRecord();
    staticEntityRecord2.Icon = null; // default value
    staticEntityRecord2.Identifier = "Unknown";
    staticEntityRecord2.SetAttributeValue(styleClass, "\"cp-status-unknown\"");
    staticEntityRecord2.SetAttributeValue(is_Active, "True");
    staticEntityRecord2.SetAttributeValue(label, "\"Data unavailable\"");
    staticEntityRecord2.SetAttributeValue(description, "\"The reading is incomplete or inconsistent, so no status is shown\"");
    staticEntityRecord2.SetAttributeValue(icon, "\"question-circle\"");
    staticEntityRecord2.SetAttributeValue(id, "\"Unknown\"");
    staticEntityRecord2.SetAttributeValue(order, "5");
    
    var staticEntityRecord3 = availabilityStatus.CreateRecord();
    staticEntityRecord3.Icon = null; // default value
    staticEntityRecord3.Identifier = "Low";
    staticEntityRecord3.SetAttributeValue(id, "\"Low\"");
    staticEntityRecord3.SetAttributeValue(is_Active, "True");
    staticEntityRecord3.SetAttributeValue(order, "3");
    staticEntityRecord3.SetAttributeValue(icon, "\"exclamation-triangle\"");
    staticEntityRecord3.SetAttributeValue(label, "\"Low availability\"");
    staticEntityRecord3.SetAttributeValue(styleClass, "\"cp-status-low\"");
    staticEntityRecord3.SetAttributeValue(description, "\"At least one lot free, but below the low threshold\"");
    
    var staticEntityRecord4 = availabilityStatus.CreateRecord();
    staticEntityRecord4.Icon = null; // default value
    staticEntityRecord4.Identifier = "Full";
    staticEntityRecord4.SetAttributeValue(id, "\"Full\"");
    staticEntityRecord4.SetAttributeValue(icon, "\"ban\"");
    staticEntityRecord4.SetAttributeValue(order, "4");
    staticEntityRecord4.SetAttributeValue(label, "\"Full\"");
    staticEntityRecord4.SetAttributeValue(is_Active, "True");
    staticEntityRecord4.SetAttributeValue(styleClass, "\"cp-status-full\"");
    staticEntityRecord4.SetAttributeValue(description, "\"No lots available\"");
    
    var staticEntityRecord5 = availabilityStatus.CreateRecord();
    staticEntityRecord5.Icon = null; // default value
    staticEntityRecord5.Identifier = "Moderate";
    staticEntityRecord5.SetAttributeValue(description, "\"Between the low and high thresholds available\"");
    staticEntityRecord5.SetAttributeValue(order, "2");
    staticEntityRecord5.SetAttributeValue(id, "\"Moderate\"");
    staticEntityRecord5.SetAttributeValue(icon, "\"adjust\"");
    staticEntityRecord5.SetAttributeValue(is_Active, "True");
    staticEntityRecord5.SetAttributeValue(styleClass, "\"cp-status-moderate\"");
    staticEntityRecord5.SetAttributeValue(label, "\"Moderate availability\"");
    availabilityStatus.IdentifierAttribute = id;
    availabilityStatus.IsActiveAttribute = is_Active;
    availabilityStatus.LabelAttribute = label;
    availabilityStatus.OrderByAttribute = order;
    
    var statusLabel = carparkLotRow.CreateAttribute("StatusLabel");
    statusLabel.DataType = eSpace.TextType;
    statusLabel.SetDefaultValue(null); // default value
    statusLabel.Description = null; // default value
    statusLabel.IsMandatory = false; // default value
    statusLabel.Label = "Status Label";
    statusLabel.Length = 50;
    statusLabel.NameInJSON = null; // default value
    statusId.DataType = availabilityStatus.IdentifierType;
    
    var isPrimary = carparkLotRow.CreateAttribute("IsPrimary");
    isPrimary.DataType = eSpace.BooleanType;
    isPrimary.SetDefaultValue(null); // default value
    isPrimary.Description = null; // default value
    isPrimary.IsMandatory = false; // default value
    isPrimary.Label = "Is Primary";
    isPrimary.NameInJSON = null; // default value
    
    var isAnomalous = carparkLotRow.CreateAttribute("IsAnomalous");
    isAnomalous.DataType = eSpace.BooleanType;
    isAnomalous.SetDefaultValue(null); // default value
    isAnomalous.Description = null; // default value
    isAnomalous.IsMandatory = false; // default value
    isAnomalous.Label = "Is Anomalous";
    isAnomalous.NameInJSON = null; // default value
    
    var lotTypeCount = carparkLotRow.CreateAttribute("LotTypeCount");
    lotTypeCount.DataType = eSpace.IntegerType;
    lotTypeCount.SetDefaultValue(null); // default value
    lotTypeCount.Description = null; // default value
    lotTypeCount.IsMandatory = false; // default value
    lotTypeCount.Label = "Lot Type Count";
    lotTypeCount.NameInJSON = null; // default value
    
    var updatedAt = carparkLotRow.CreateAttribute("UpdatedAt");
    updatedAt.DataType = eSpace.DateTimeType;
    updatedAt.SetDefaultValue(null); // default value
    updatedAt.Description = null; // default value
    updatedAt.IsMandatory = false; // default value
    updatedAt.Label = "Updated At";
    updatedAt.NameInJSON = null; // default value
    
    var isReadingOutdated = carparkLotRow.CreateAttribute("IsReadingOutdated");
    isReadingOutdated.DataType = eSpace.BooleanType;
    isReadingOutdated.SetDefaultValue(null); // default value
    isReadingOutdated.Description = null; // default value
    isReadingOutdated.IsMandatory = false; // default value
    isReadingOutdated.Label = "Is Reading Outdated";
    isReadingOutdated.NameInJSON = null; // default value
    
    var hasLocation = carparkLotRow.CreateAttribute("HasLocation");
    hasLocation.DataType = eSpace.BooleanType;
    hasLocation.SetDefaultValue(null); // default value
    hasLocation.Description = null; // default value
    hasLocation.IsMandatory = false; // default value
    hasLocation.Label = "Has Location";
    hasLocation.NameInJSON = null; // default value
    
    var address = carparkLotRow.CreateAttribute("Address");
    address.DataType = eSpace.TextType;
    address.SetDefaultValue(null); // default value
    address.Description = null; // default value
    address.IsMandatory = false; // default value
    address.Label = "Address";
    address.Length = 250;
    address.NameInJSON = null; // default value
    
    var carparkType = carparkLotRow.CreateAttribute("CarparkType");
    carparkType.DataType = eSpace.TextType;
    carparkType.SetDefaultValue(null); // default value
    carparkType.Description = null; // default value
    carparkType.IsMandatory = false; // default value
    carparkType.Label = "Carpark Type";
    carparkType.Length = 80;
    carparkType.NameInJSON = null; // default value
    
    var parkingSystem = carparkLotRow.CreateAttribute("ParkingSystem");
    parkingSystem.DataType = eSpace.TextType;
    parkingSystem.SetDefaultValue(null); // default value
    parkingSystem.Description = null; // default value
    parkingSystem.IsMandatory = false; // default value
    parkingSystem.Label = "Parking System";
    parkingSystem.Length = 80;
    parkingSystem.NameInJSON = null; // default value
    
    var shortTermParking = carparkLotRow.CreateAttribute("ShortTermParking");
    shortTermParking.DataType = eSpace.TextType;
    shortTermParking.SetDefaultValue(null); // default value
    shortTermParking.Description = null; // default value
    shortTermParking.IsMandatory = false; // default value
    shortTermParking.Label = "Short Term Parking";
    shortTermParking.Length = 80;
    shortTermParking.NameInJSON = null; // default value
    
    var freeParking = carparkLotRow.CreateAttribute("FreeParking");
    freeParking.DataType = eSpace.TextType;
    freeParking.SetDefaultValue(null); // default value
    freeParking.Description = null; // default value
    freeParking.IsMandatory = false; // default value
    freeParking.Label = "Free Parking";
    freeParking.Length = 80;
    freeParking.NameInJSON = null; // default value
    
    var nightParking = carparkLotRow.CreateAttribute("NightParking");
    nightParking.DataType = eSpace.TextType;
    nightParking.SetDefaultValue(null); // default value
    nightParking.Description = null; // default value
    nightParking.IsMandatory = false; // default value
    nightParking.Label = "Night Parking";
    nightParking.Length = 20;
    nightParking.NameInJSON = null; // default value
    
    var gantryHeight = carparkLotRow.CreateAttribute("GantryHeight");
    gantryHeight.DataType = eSpace.TextType;
    gantryHeight.SetDefaultValue(null); // default value
    gantryHeight.Description = null; // default value
    gantryHeight.IsMandatory = false; // default value
    gantryHeight.Label = "Gantry Height";
    gantryHeight.Length = 10;
    gantryHeight.NameInJSON = null; // default value
    
    var searchText = carparkLotRow.CreateAttribute("SearchText");
    searchText.DataType = eSpace.TextType;
    searchText.SetDefaultValue(null); // default value
    searchText.Description = null; // default value
    searchText.IsMandatory = false; // default value
    searchText.Label = "Search Text";
    searchText.Length = 400;
    searchText.NameInJSON = null; // default value
    
    var updatedAtText = carparkLotRow.CreateAttribute("UpdatedAtText");
    updatedAtText.DataType = eSpace.TextType;
    updatedAtText.SetDefaultValue(null); // default value
    updatedAtText.Description = null; // default value
    updatedAtText.IsMandatory = false; // default value
    updatedAtText.Label = "Updated At Text";
    updatedAtText.Length = 40;
    updatedAtText.NameInJSON = null; // default value
    // IStructureSignature OutSystemsCharts/ChartExportMenuOptions
    var chartExportMenuOptions = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*QMEZMhC+wkiBtdmavx+22A"));
    // IStructureSignature OutSystemsCharts/ChartExportOption
    var chartExportOption = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*10yZaFGrEEuzzj8OJyEmOQ"));
    // IStructureSignature OutSystemsCharts/ChartExportOptionalConfigs
    var chartExportOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*ZIvhqJjgtEa0RQTlapF+ww"));
    // IStructureSignature OutSystemsCharts/ChartLegendOptionalConfigs
    var chartLegendOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*paF_yF+UTUuPIOKkkmyixg"));
    // IStructureSignature OutSystemsCharts/ChartLegendStyling
    var chartLegendStyling = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*W+QAg3aYgUegoRHmRXQsig"));
    // IStructureSignature OutSystemsMaps/Coordinates
    var coordinates = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*WZx1aHJQ0UWsUfXtv2pX2w"));
    // IStructureSignature OutSystemsCharts/DataPoint
    var dataPoint = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*cxjG6+C620ONyhWlajy1gg"));
    // IStructureSignature OutSystemsUI/DatePickerOptionalConfigs
    var datePickerOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*JNLyX_5nIkWxxM8qnqUZow"));
    // IStructureSignature OutSystemsUI/DatePickerRangeOptionalConfigs
    var datePickerRangeOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*grM32LlBXU+cqCKpKb0jsw"));
    // IStructureSignature OutSystemsUI/DeviceConfiguration
    var deviceConfiguration = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*uUnElVRZ7EeykU8Op0Iq9A"));
    // IStructureSignature OutSystemsUI/DropdownItem
    var dropdownItem = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*GsxolpT5U0Gckg0MkAMyPQ"));
    // IStructureSignature OutSystemsUI/DropdownOption
    var dropdownOption = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*S7xoxEqbCUG_BCxpreTOYw"));
    // IStructureSignature OutSystemsUI/DropdownOptionalConfigs
    var dropdownOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*X1pGkDp6nkSgQbNaytI6Pg"));
    // IStructureSignature OutSystemsUI/DropdownTagsOptionalConfigs
    var dropdownTagsOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*do9jkzG6BUq50KGz_s009w"));
    // IStructureSignature OutSystemsUI/ErrorMessage
    var errorMessage = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*5nlHOt6drUmqeh9sD2bQBA"));
    // IStructureSignature OutSystemsCharts/ErrorMessage2
    var errorMessage2 = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*GN8lvtIwZkSHL0Z3vcEAPQ"));
    // IStructureSignature OutSystemsUI/FlatpickrConfigs
    var flatpickrConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*O+U0HyabQ0yxBLMCogSRKg"));
    // IStructureSignature OutSystemsUI/GradientColor
    var gradientColor = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*IxDFM_PyIUSuvuT8UjkOpA"));
    
    /*** creating IStructure 'HealthReport' ***/
    var healthReport = eSpace.CreateStructure("HealthReport");
    healthReport.Description = "Read-only operational status returned by CarparkOps/Health. Contains no secrets: the API key is reported only as configured or not.";
    healthReport.Folder = null; // default value
    healthReport.Public = false; // default value
    healthReport.Shareable = false; // default value
    
    var status = healthReport.CreateAttribute("Status");
    status.DataType = eSpace.TextType;
    status.SetDefaultValue(null); // default value
    status.Description = null; // default value
    status.IsMandatory = false; // default value
    status.Label = "Status";
    status.Length = 20;
    status.NameInJSON = "status";
    
    var module = healthReport.CreateAttribute("Module");
    module.DataType = eSpace.TextType;
    module.SetDefaultValue(null); // default value
    module.Description = null; // default value
    module.IsMandatory = false; // default value
    module.Label = "Module";
    module.Length = 50;
    module.NameInJSON = "module";
    
    var snapshotAvailable = healthReport.CreateAttribute("SnapshotAvailable");
    snapshotAvailable.DataType = eSpace.BooleanType;
    snapshotAvailable.SetDefaultValue(null); // default value
    snapshotAvailable.Description = null; // default value
    snapshotAvailable.IsMandatory = false; // default value
    snapshotAvailable.Label = "Snapshot Available";
    snapshotAvailable.NameInJSON = "snapshotAvailable";
    
    var snapshotAgeSeconds = healthReport.CreateAttribute("SnapshotAgeSeconds");
    snapshotAgeSeconds.DataType = eSpace.IntegerType;
    snapshotAgeSeconds.SetDefaultValue(null); // default value
    snapshotAgeSeconds.Description = null; // default value
    snapshotAgeSeconds.IsMandatory = false; // default value
    snapshotAgeSeconds.Label = "Snapshot Age Seconds";
    snapshotAgeSeconds.NameInJSON = "snapshotAgeSeconds";
    
    var dataAgeSeconds2 = healthReport.CreateAttribute("DataAgeSeconds");
    dataAgeSeconds2.DataType = eSpace.IntegerType;
    dataAgeSeconds2.SetDefaultValue(null); // default value
    dataAgeSeconds2.Description = null; // default value
    dataAgeSeconds2.IsMandatory = false; // default value
    dataAgeSeconds2.Label = "Data Age Seconds";
    dataAgeSeconds2.NameInJSON = "dataAgeSeconds";
    
    var sourceTimestamp2 = healthReport.CreateAttribute("SourceTimestamp");
    sourceTimestamp2.DataType = eSpace.TextType;
    sourceTimestamp2.SetDefaultValue(null); // default value
    sourceTimestamp2.Description = null; // default value
    sourceTimestamp2.IsMandatory = false; // default value
    sourceTimestamp2.Label = "Source Timestamp";
    sourceTimestamp2.Length = 40;
    sourceTimestamp2.NameInJSON = "sourceTimestamp";
    
    var carparkCount2 = healthReport.CreateAttribute("CarparkCount");
    carparkCount2.DataType = eSpace.IntegerType;
    carparkCount2.SetDefaultValue(null); // default value
    carparkCount2.Description = null; // default value
    carparkCount2.IsMandatory = false; // default value
    carparkCount2.Label = "Carpark Count";
    carparkCount2.NameInJSON = "carparkCount";
    
    var rejectedCount2 = healthReport.CreateAttribute("RejectedCount");
    rejectedCount2.DataType = eSpace.IntegerType;
    rejectedCount2.SetDefaultValue(null); // default value
    rejectedCount2.Description = null; // default value
    rejectedCount2.IsMandatory = false; // default value
    rejectedCount2.Label = "Rejected Count";
    rejectedCount2.NameInJSON = "rejectedCount";
    
    var duplicateCount2 = healthReport.CreateAttribute("DuplicateCount");
    duplicateCount2.DataType = eSpace.IntegerType;
    duplicateCount2.SetDefaultValue(null); // default value
    duplicateCount2.Description = null; // default value
    duplicateCount2.IsMandatory = false; // default value
    duplicateCount2.Label = "Duplicate Count";
    duplicateCount2.NameInJSON = "mergedRecordCount";
    
    var anomalyCount2 = healthReport.CreateAttribute("AnomalyCount");
    anomalyCount2.DataType = eSpace.IntegerType;
    anomalyCount2.SetDefaultValue(null); // default value
    anomalyCount2.Description = null; // default value
    anomalyCount2.IsMandatory = false; // default value
    anomalyCount2.Label = "Anomaly Count";
    anomalyCount2.NameInJSON = "inconsistentLotCount";
    
    var hasLocationData2 = healthReport.CreateAttribute("HasLocationData");
    hasLocationData2.DataType = eSpace.BooleanType;
    hasLocationData2.SetDefaultValue(null); // default value
    hasLocationData2.Description = null; // default value
    hasLocationData2.IsMandatory = false; // default value
    hasLocationData2.Label = "Has Location Data";
    hasLocationData2.NameInJSON = "hasLocationData";
    
    var carparkInfoCount = healthReport.CreateAttribute("CarparkInfoCount");
    carparkInfoCount.DataType = eSpace.IntegerType;
    carparkInfoCount.SetDefaultValue(null); // default value
    carparkInfoCount.Description = null; // default value
    carparkInfoCount.IsMandatory = false; // default value
    carparkInfoCount.Label = "Carpark Info Count";
    carparkInfoCount.NameInJSON = "carparkInfoCount";
    
    var carparkInfoSyncAgeHours = healthReport.CreateAttribute("CarparkInfoSyncAgeHours");
    carparkInfoSyncAgeHours.DataType = eSpace.IntegerType;
    carparkInfoSyncAgeHours.SetDefaultValue(null); // default value
    carparkInfoSyncAgeHours.Description = null; // default value
    carparkInfoSyncAgeHours.IsMandatory = false; // default value
    carparkInfoSyncAgeHours.Label = "Carpark Info Sync Age Hours";
    carparkInfoSyncAgeHours.NameInJSON = "carparkInfoSyncAgeHours";
    
    var highAvailabilityMinPct = healthReport.CreateAttribute("HighAvailabilityMinPct");
    highAvailabilityMinPct.DataType = eSpace.DecimalType;
    highAvailabilityMinPct.Decimals = null; // default value
    highAvailabilityMinPct.SetDefaultValue(null); // default value
    highAvailabilityMinPct.Description = null; // default value
    highAvailabilityMinPct.IsMandatory = false; // default value
    highAvailabilityMinPct.Label = "High Availability Min Pct";
    highAvailabilityMinPct.Length = null; // default value
    highAvailabilityMinPct.NameInJSON = "highAvailabilityMinPct";
    
    var lowAvailabilityMaxPct = healthReport.CreateAttribute("LowAvailabilityMaxPct");
    lowAvailabilityMaxPct.DataType = eSpace.DecimalType;
    lowAvailabilityMaxPct.Decimals = null; // default value
    lowAvailabilityMaxPct.SetDefaultValue(null); // default value
    lowAvailabilityMaxPct.Description = null; // default value
    lowAvailabilityMaxPct.IsMandatory = false; // default value
    lowAvailabilityMaxPct.Label = "Low Availability Max Pct";
    lowAvailabilityMaxPct.Length = null; // default value
    lowAvailabilityMaxPct.NameInJSON = "lowAvailabilityMaxPct";
    
    var staleAfterMinutes2 = healthReport.CreateAttribute("StaleAfterMinutes");
    staleAfterMinutes2.DataType = eSpace.IntegerType;
    staleAfterMinutes2.SetDefaultValue(null); // default value
    staleAfterMinutes2.Description = null; // default value
    staleAfterMinutes2.IsMandatory = false; // default value
    staleAfterMinutes2.Label = "Stale After Minutes";
    staleAfterMinutes2.NameInJSON = "staleAfterMinutes";
    
    var readingOutdatedMinutes = healthReport.CreateAttribute("ReadingOutdatedMinutes");
    readingOutdatedMinutes.DataType = eSpace.IntegerType;
    readingOutdatedMinutes.SetDefaultValue(null); // default value
    readingOutdatedMinutes.Description = null; // default value
    readingOutdatedMinutes.IsMandatory = false; // default value
    readingOutdatedMinutes.Label = "Reading Outdated Minutes";
    readingOutdatedMinutes.NameInJSON = "readingOutdatedMinutes";
    
    var cacheTtlSeconds = healthReport.CreateAttribute("CacheTtlSeconds");
    cacheTtlSeconds.DataType = eSpace.IntegerType;
    cacheTtlSeconds.SetDefaultValue(null); // default value
    cacheTtlSeconds.Description = null; // default value
    cacheTtlSeconds.IsMandatory = false; // default value
    cacheTtlSeconds.Label = "Cache Ttl Seconds";
    cacheTtlSeconds.NameInJSON = "cacheTtlSeconds";
    
    var minRefreshIntervalSeconds = healthReport.CreateAttribute("MinRefreshIntervalSeconds");
    minRefreshIntervalSeconds.DataType = eSpace.IntegerType;
    minRefreshIntervalSeconds.SetDefaultValue(null); // default value
    minRefreshIntervalSeconds.Description = null; // default value
    minRefreshIntervalSeconds.IsMandatory = false; // default value
    minRefreshIntervalSeconds.Label = "Min Refresh Interval Seconds";
    minRefreshIntervalSeconds.NameInJSON = "minRefreshIntervalSeconds";
    
    var carparkInfoMaxAgeHours = healthReport.CreateAttribute("CarparkInfoMaxAgeHours");
    carparkInfoMaxAgeHours.DataType = eSpace.IntegerType;
    carparkInfoMaxAgeHours.SetDefaultValue(null); // default value
    carparkInfoMaxAgeHours.Description = null; // default value
    carparkInfoMaxAgeHours.IsMandatory = false; // default value
    carparkInfoMaxAgeHours.Label = "Carpark Info Max Age Hours";
    carparkInfoMaxAgeHours.NameInJSON = "carparkInfoMaxAgeHours";
    
    var hdbCarparkInfoResourceId = healthReport.CreateAttribute("HdbCarparkInfoResourceId");
    hdbCarparkInfoResourceId.DataType = eSpace.TextType;
    hdbCarparkInfoResourceId.SetDefaultValue(null); // default value
    hdbCarparkInfoResourceId.Description = null; // default value
    hdbCarparkInfoResourceId.IsMandatory = false; // default value
    hdbCarparkInfoResourceId.Label = "Hdb Carpark Info Resource";
    hdbCarparkInfoResourceId.Length = 100;
    hdbCarparkInfoResourceId.NameInJSON = "hdbCarparkInfoResourceId";
    
    var dataGovApiKeyConfigured = healthReport.CreateAttribute("DataGovApiKeyConfigured");
    dataGovApiKeyConfigured.DataType = eSpace.BooleanType;
    dataGovApiKeyConfigured.SetDefaultValue(null); // default value
    dataGovApiKeyConfigured.Description = null; // default value
    dataGovApiKeyConfigured.IsMandatory = false; // default value
    dataGovApiKeyConfigured.Label = "Data Gov Api Key Configured";
    dataGovApiKeyConfigured.NameInJSON = "dataGovApiKeyConfigured";
    // IStructureSignature OutSystemsMaps/IconSize
    var iconSize = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*0K72Qk3W8UKfwN50QYd4Xw"));
    // IStructureSignature OutSystemsUI/LightboxImageOptionalConfigs
    var lightboxImageOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*nH0s8+IJh0KOba4Bye9HBg"));
    // IStructureSignature OutSystemsMaps/Localization
    var localization = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*yihIBQrI706bmMXHr3P3DQ"));
    // IStructureSignature OutSystemsMaps/Marker
    var marker = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*NiCGoYbA+0qb4lPY4N633w"));
    // IStructureSignature OutSystemsMaps/MarkerClusterer
    var markerClusterer = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*8G2shutUB0Oe7ahWNAesaw"));
    // IStructureSignature OutSystemsUI/MonthOrderYear
    var monthOrderYear = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*IEKMqKLYpkqqO+CGuDduBw"));
    // IStructureSignature OutSystemsUI/MonthYear
    var monthYear = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*0fcWF_jbQ0ilugRe0GBAnQ"));
    // IStructureSignature OutSystemsUI/NotificationOptionalConfigs
    var notificationOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*PmxFCOpovUiOxQ+CO48nVQ"));
    // IStructureSignature OutSystemsUI/NoUiSliderConfigs
    var noUiSliderConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*OHFQpHsvwky+_PPPj80ixA"));
    // IStructureSignature OutSystemsUI/NoUiSliderConfigs_Range
    var noUiSliderConfigs_Range = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*2cQY0DjpYU6CzF+YJYSuBw"));
    // IStructureSignature OutSystemsMaps/Offset
    var offset = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*Vvpmd9jHNUqpUNrgJ9MmAw"));
    // IStructureSignature OutSystemsCharts/OptionalConfigs
    var optionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*_CcUVq6LpE6z1g1xn392SQ"));
    // IStructureSignature OutSystemsMaps/OptionalMapConfigs
    var optionalMapConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*Z6bJHsxUF0GE4UXTJclJew"));
    // IStructureSignature OutSystemsMaps/OptionalMarkerConfigs
    var optionalMarkerConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*NXdd1rRCLkez78cYLP3_qg"));
    // IStructureSignature OutSystemsMaps/OptionalStaticMapConfigs
    var optionalStaticMapConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*JZvz4YYXi0KWFxIRoNqGBQ"));
    // IStructureSignature OutSystemsUI/ProgressBarOptionalConfigs
    var progressBarOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*yr_QLdz3gUSapQFUagrtsQ"));
    // IStructureSignature OutSystemsUI/ProgressCircleOptionalConfigs
    var progressCircleOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*IX4mK1jQ+kaQWhxlHnzoMA"));
    // IStructureSignature OutSystemsCharts/PropertyValue
    var propertyValue = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*HwYhgBgvAUKeq0C5fagyDg"));
    // IStructureSignature OutSystemsUI/RangeSliderOptionalConfigs
    var rangeSliderOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*YH6NTPqYJkq2DoxNHSzraQ"));
    // IStructureSignature OutSystemsCharts/SeriesStyling
    var seriesStyling = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*uHnvkJbeQkasY7byITpeMg"));
    // IStructureSignature OutSystemsCharts/SeriesStylingMarker
    var seriesStylingMarker = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*LqcKXUXMNkKFd+T0BuO0fA"));
    // IStructureSignature OutSystemsCharts/SeriesStylingOptionalConfigs
    var seriesStylingOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*AWHYK1eo4kCoYBcQhH0C1g"));
    // IStructureSignature OutSystemsUI/SplideConfigs
    var splideConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*F9dhjiLK3Ue591n_3JFJlQ"));
    // IStructureSignature OutSystemsUI/SplideProviderClasses
    var splideProviderClasses = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*dbAI3uYXZkaqDM_imgqiHQ"));
    // IStructureSignature OutSystemsUI/SplideProvideri18n
    var splideProvideri18n = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*XVQm8JHa9E24D2KOuMKTVA"));
    // IStructureSignature OutSystemsMaps/StaticMarker
    var staticMarker = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*HWKV7+6mrUKd5B5htFPGKw"));
    // IStructureSignature OutSystemsUI/TabsOptionalConfigs
    var tabsOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*OVD8IhsywU+PQi603Zw0AQ"));
    // IStructureSignature OutSystemsUI/TimePickerOptionalConfigs
    var timePickerOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*S86ifK4baUy+ZoPeWdAqqQ"));
    // IStructureSignature OutSystemsUI/VideoCaptions
    var videoCaptions = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*+3w9d276xkuitQeiG1EGag"));
    // IStructureSignature OutSystemsUI/VideoOptionalConfigs
    var videoOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*f9HbDthd0ECNETfjDFOmow"));
    // IStructureSignature OutSystemsUI/VirtualSelectConfigs
    var virtualSelectConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*x3IZFA2oh0WqcttA4dlGzQ"));
    // IStructureSignature OutSystemsCharts/XAxisGridLinesStyling
    var xAxisGridLinesStyling = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*2ecbARfP0EuxBFu1lcvJYQ"));
    // IStructureSignature OutSystemsCharts/XAxisOptionalConfigs
    var xAxisOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*JVC_Qn0eAk24+pFY7LTEvg"));
    // IStructureSignature OutSystemsCharts/XAxisStyling
    var xAxisStyling = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*Q84RaFYnQkaw+8XJozSh6Q"));
    // IStructureSignature OutSystemsCharts/YAxisGridLinesStyling
    var yAxisGridLinesStyling = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*1iCDZ_Vny0WyN3XnHpCm6A"));
    // IStructureSignature OutSystemsCharts/YAxisOptionalConfigs
    var yAxisOptionalConfigs = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*GOJW1amidk2So2VaUKw45w"));
    // IStructureSignature OutSystemsCharts/YAxisStyling
    var yAxisStyling = (OutSystems.Model.Data.IStructureSignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*nI9fPDxZG06RzCgkWOrnUw"));
}
