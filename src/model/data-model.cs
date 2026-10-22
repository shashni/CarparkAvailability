(OutSystems.Model.IReference outSystemsUI) => {
    // IStaticEntitySignature OutSystemsUI/AccordionIconPosition
    var eSpace = outSystemsUI.GetESpace();
    var accordionIconPosition = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*LdtIqGz+8U2MEIvbPC6Uig"));
    // IStaticEntitySignature OutSystemsUI/AccordionIconType
    var accordionIconType = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*3sFcY972ZUu68cZhvFGCZg"));
    // IServerEntitySignature (System)/Activity
    var activity = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*hg460+gk0UGzMRTFlEBTOQ"));
    // IStaticEntitySignature OutSystemsUI/Alert
    var alert = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*wIZEkprd6katdCz5rAyE2Q"));
    // IStaticEntitySignature OutSystemsUI/AnimationType
    var animationType = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*SdRjBDRo1UCBez100acbsg"));
    // IStaticEntitySignature OutSystemsUI/Autoplay
    var autoplay = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*tFiqgruLGU2StilEr_rQLA"));
    
    /*** creating IServerEntity 'AvailabilitySnapshot' ***/
    var availabilitySnapshot = eSpace.CreateServerEntity("AvailabilitySnapshot");
    availabilitySnapshot.Description = "The last successfully validated availability retrieval. Served (labelled as cached) when data.gov.sg cannot be reached. Only the newest record is used.";
    availabilitySnapshot.ExposeProcessEvents = false; // default value
    availabilitySnapshot.Folder = null; // default value
    availabilitySnapshot.IsActiveAttribute = null; // default value
    availabilitySnapshot.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    availabilitySnapshot.IsRestrictedInQueries = false; // default value
    availabilitySnapshot.Label = "Availability Snapshot";
    availabilitySnapshot.LabelAttribute = null; // default value
    availabilitySnapshot.LabelPlural = "Availability Snapshots";
    availabilitySnapshot.OrderByAttribute = null; // default value
    availabilitySnapshot.Public = false; // default value
    availabilitySnapshot.Shareable = false; // default value
    availabilitySnapshot.TableNameOverride = null; // default value
    availabilitySnapshot.UpdateBehavior = OutSystems.Model.Enumerations.UpdateEntityBehaviorType.Optimized;
    availabilitySnapshot.UseTranslations = false; // default value
    availabilitySnapshot.ViewNameOverride = null; // default value
    
    var id = availabilitySnapshot.CreateAttribute("Id");
    id.DataType = eSpace.LongIntegerType;
    id.SetDefaultValue(null); // default value
    id.Description = null; // default value
    id.IsAutoNumber = OutSystems.Model.Enumerations.AutoNumber.Yes;
    id.IsSearchable = false; // default value
    id.Label = "Id";
    id.OriginalName = null; // default value
    id.OriginalType = null; // default value
    availabilitySnapshot.IdentifierAttribute = id;
    
    var sourceTimestamp = availabilitySnapshot.CreateAttribute("SourceTimestamp");
    sourceTimestamp.DataType = eSpace.DateTimeType;
    sourceTimestamp.SetDefaultValue(null); // default value
    sourceTimestamp.Description = null; // default value
    sourceTimestamp.IsMandatory = false; // default value
    sourceTimestamp.IsSearchable = false; // default value
    sourceTimestamp.Label = "Source Timestamp";
    sourceTimestamp.OriginalName = null; // default value
    sourceTimestamp.OriginalType = null; // default value
    
    var retrievedOn = availabilitySnapshot.CreateAttribute("RetrievedOn");
    retrievedOn.DataType = eSpace.DateTimeType;
    retrievedOn.SetDefaultValue(null); // default value
    retrievedOn.Description = null; // default value
    retrievedOn.IsMandatory = false; // default value
    retrievedOn.IsSearchable = false; // default value
    retrievedOn.Label = "Retrieved On";
    retrievedOn.OriginalName = null; // default value
    retrievedOn.OriginalType = null; // default value
    
    var dataAgeSecondsAtRetrieval = availabilitySnapshot.CreateAttribute("DataAgeSecondsAtRetrieval");
    dataAgeSecondsAtRetrieval.DataType = eSpace.IntegerType;
    dataAgeSecondsAtRetrieval.SetDefaultValue(null); // default value
    dataAgeSecondsAtRetrieval.Description = null; // default value
    dataAgeSecondsAtRetrieval.IsAutoNumber = OutSystems.Model.Enumerations.AutoNumber.No; // default value
    dataAgeSecondsAtRetrieval.IsMandatory = false; // default value
    dataAgeSecondsAtRetrieval.IsSearchable = false; // default value
    dataAgeSecondsAtRetrieval.Label = "Data Age Seconds At Retrieval";
    dataAgeSecondsAtRetrieval.OriginalName = null; // default value
    dataAgeSecondsAtRetrieval.OriginalType = null; // default value
    
    var carparkCount = availabilitySnapshot.CreateAttribute("CarparkCount");
    carparkCount.DataType = eSpace.IntegerType;
    carparkCount.SetDefaultValue(null); // default value
    carparkCount.Description = null; // default value
    carparkCount.IsAutoNumber = OutSystems.Model.Enumerations.AutoNumber.No; // default value
    carparkCount.IsMandatory = false; // default value
    carparkCount.IsSearchable = false; // default value
    carparkCount.Label = "Carpark Count";
    carparkCount.OriginalName = null; // default value
    carparkCount.OriginalType = null; // default value
    
    var rejectedCount = availabilitySnapshot.CreateAttribute("RejectedCount");
    rejectedCount.DataType = eSpace.IntegerType;
    rejectedCount.SetDefaultValue(null); // default value
    rejectedCount.Description = null; // default value
    rejectedCount.IsAutoNumber = OutSystems.Model.Enumerations.AutoNumber.No; // default value
    rejectedCount.IsMandatory = false; // default value
    rejectedCount.IsSearchable = false; // default value
    rejectedCount.Label = "Rejected Count";
    rejectedCount.OriginalName = null; // default value
    rejectedCount.OriginalType = null; // default value
    
    var duplicateCount = availabilitySnapshot.CreateAttribute("DuplicateCount");
    duplicateCount.DataType = eSpace.IntegerType;
    duplicateCount.SetDefaultValue(null); // default value
    duplicateCount.Description = null; // default value
    duplicateCount.IsAutoNumber = OutSystems.Model.Enumerations.AutoNumber.No; // default value
    duplicateCount.IsMandatory = false; // default value
    duplicateCount.IsSearchable = false; // default value
    duplicateCount.Label = "Duplicate Count";
    duplicateCount.OriginalName = null; // default value
    duplicateCount.OriginalType = null; // default value
    
    var anomalyCount = availabilitySnapshot.CreateAttribute("AnomalyCount");
    anomalyCount.DataType = eSpace.IntegerType;
    anomalyCount.SetDefaultValue(null); // default value
    anomalyCount.Description = null; // default value
    anomalyCount.IsAutoNumber = OutSystems.Model.Enumerations.AutoNumber.No; // default value
    anomalyCount.IsMandatory = false; // default value
    anomalyCount.IsSearchable = false; // default value
    anomalyCount.Label = "Anomaly Count";
    anomalyCount.OriginalName = null; // default value
    anomalyCount.OriginalType = null; // default value
    
    var hasLocationData = availabilitySnapshot.CreateAttribute("HasLocationData");
    hasLocationData.DataType = eSpace.BooleanType;
    hasLocationData.SetDefaultValue(null); // default value
    hasLocationData.Description = null; // default value
    hasLocationData.IsMandatory = false; // default value
    hasLocationData.IsSearchable = false; // default value
    hasLocationData.Label = "Has Location Data";
    hasLocationData.OriginalName = null; // default value
    hasLocationData.OriginalType = null; // default value
    
    var rowsJson = availabilitySnapshot.CreateAttribute("RowsJson");
    rowsJson.DataType = eSpace.TextType;
    rowsJson.SetDefaultValue(null); // default value
    rowsJson.Description = null; // default value
    rowsJson.IsMandatory = false; // default value
    rowsJson.IsSearchable = false; // default value
    rowsJson.Label = "Rows Json";
    rowsJson.Length = 2000000;
    rowsJson.OriginalName = null; // default value
    rowsJson.OriginalType = null; // default value
    
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
    
    var id2 = availabilityStatus.CreateAttribute("Id");
    id2.DataType = eSpace.TextType;
    id2.SetDefaultValue(null); // default value
    id2.Description = null; // default value
    id2.IsSearchable = false; // default value
    id2.Label = "Id";
    id2.Length = 20;
    id2.OriginalName = null; // default value
    id2.OriginalType = null; // default value
    availabilityStatus.IdentifierAttribute = id2;
    
    var is_Active = availabilityStatus.CreateAttribute("Is_Active");
    is_Active.DataType = eSpace.BooleanType;
    is_Active.SetDefaultValue(null); // default value
    is_Active.Description = null; // default value
    is_Active.IsMandatory = true;
    is_Active.IsSearchable = false; // default value
    is_Active.Label = "Is Active";
    is_Active.OriginalName = null; // default value
    is_Active.OriginalType = null; // default value
    availabilityStatus.IsActiveAttribute = is_Active;
    
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
    availabilityStatus.LabelAttribute = label;
    
    var order = availabilityStatus.CreateAttribute("Order");
    order.DataType = eSpace.IntegerType;
    order.SetDefaultValue(null); // default value
    order.Description = null; // default value
    order.IsMandatory = true;
    order.IsSearchable = false; // default value
    order.Label = "Order";
    order.OriginalName = null; // default value
    order.OriginalType = null; // default value
    availabilityStatus.OrderByAttribute = order;
    
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
    
    var staticEntityRecord = availabilityStatus.CreateRecord();
    staticEntityRecord.Icon = null; // default value
    staticEntityRecord.Identifier = "High";
    staticEntityRecord.SetAttributeValue(is_Active, "True");
    staticEntityRecord.SetAttributeValue(styleClass, "\"cp-status-high\"");
    staticEntityRecord.SetAttributeValue(icon, "\"check-circle\"");
    staticEntityRecord.SetAttributeValue(label, "\"High availability\"");
    staticEntityRecord.SetAttributeValue(description, "\"More than the high threshold of lots available\"");
    staticEntityRecord.SetAttributeValue(order, "1");
    staticEntityRecord.SetAttributeValue(id2, "\"High\"");
    
    var staticEntityRecord2 = availabilityStatus.CreateRecord();
    staticEntityRecord2.Icon = null; // default value
    staticEntityRecord2.Identifier = "Unknown";
    staticEntityRecord2.SetAttributeValue(styleClass, "\"cp-status-unknown\"");
    staticEntityRecord2.SetAttributeValue(is_Active, "True");
    staticEntityRecord2.SetAttributeValue(label, "\"Data unavailable\"");
    staticEntityRecord2.SetAttributeValue(description, "\"The reading is incomplete or inconsistent, so no status is shown\"");
    staticEntityRecord2.SetAttributeValue(icon, "\"question-circle\"");
    staticEntityRecord2.SetAttributeValue(id2, "\"Unknown\"");
    staticEntityRecord2.SetAttributeValue(order, "5");
    
    var staticEntityRecord3 = availabilityStatus.CreateRecord();
    staticEntityRecord3.Icon = null; // default value
    staticEntityRecord3.Identifier = "Low";
    staticEntityRecord3.SetAttributeValue(id2, "\"Low\"");
    staticEntityRecord3.SetAttributeValue(is_Active, "True");
    staticEntityRecord3.SetAttributeValue(order, "3");
    staticEntityRecord3.SetAttributeValue(icon, "\"exclamation-triangle\"");
    staticEntityRecord3.SetAttributeValue(label, "\"Low availability\"");
    staticEntityRecord3.SetAttributeValue(styleClass, "\"cp-status-low\"");
    staticEntityRecord3.SetAttributeValue(description, "\"At least one lot free, but below the low threshold\"");
    
    var staticEntityRecord4 = availabilityStatus.CreateRecord();
    staticEntityRecord4.Icon = null; // default value
    staticEntityRecord4.Identifier = "Full";
    staticEntityRecord4.SetAttributeValue(id2, "\"Full\"");
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
    staticEntityRecord5.SetAttributeValue(id2, "\"Moderate\"");
    staticEntityRecord5.SetAttributeValue(icon, "\"adjust\"");
    staticEntityRecord5.SetAttributeValue(is_Active, "True");
    staticEntityRecord5.SetAttributeValue(styleClass, "\"cp-status-moderate\"");
    staticEntityRecord5.SetAttributeValue(label, "\"Moderate availability\"");
    // IStaticEntitySignature OutSystemsCharts/AxisValuesType
    var axisValuesType = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*eX10_X1B+k6PiS15u43oNQ"));
    // IStaticEntitySignature OutSystemsUI/BooleanTypes
    var booleanTypes = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*_MiCmNYNLkm2uXpoBZ7MBg"));
    // IStaticEntitySignature OutSystemsUI/BreakColumns
    var breakColumns = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*k0X3aMSlZU6Fi_RCEerxIg"));
    // IStaticEntitySignature OutSystemsUI/CarouselDirection
    var carouselDirection = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*ZuLLccX3+0K+rna8Ml2Z6g"));
    // IStaticEntitySignature OutSystemsUI/CarouselNavigation
    var carouselNavigation = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*ye867jfk+0mGogwtBlbtQw"));
    
    /*** creating IServerEntity 'CarparkInfo' ***/
    var carparkInfo = eSpace.CreateServerEntity("CarparkInfo");
    carparkInfo.Description = "Local cache of the data.gov.sg \'HDB Carpark Information\' dataset (addresses and carpark characteristics), re-synchronised every CarparkInfoMaxAgeHours.";
    carparkInfo.ExposeProcessEvents = false; // default value
    carparkInfo.Folder = null; // default value
    carparkInfo.IsActiveAttribute = null; // default value
    carparkInfo.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.Inherited; // default value
    carparkInfo.IsRestrictedInQueries = false; // default value
    carparkInfo.Label = "Carpark Info";
    carparkInfo.LabelAttribute = null; // default value
    carparkInfo.LabelPlural = "Carpark Infoes";
    carparkInfo.OrderByAttribute = null; // default value
    carparkInfo.Public = false; // default value
    carparkInfo.Shareable = false; // default value
    carparkInfo.TableNameOverride = null; // default value
    carparkInfo.UpdateBehavior = OutSystems.Model.Enumerations.UpdateEntityBehaviorType.Optimized;
    carparkInfo.UseTranslations = false; // default value
    carparkInfo.ViewNameOverride = null; // default value
    
    var id3 = carparkInfo.CreateAttribute("Id");
    id3.DataType = eSpace.LongIntegerType;
    id3.SetDefaultValue(null); // default value
    id3.Description = null; // default value
    id3.IsAutoNumber = OutSystems.Model.Enumerations.AutoNumber.Yes;
    id3.IsSearchable = false; // default value
    id3.Label = "Id";
    id3.OriginalName = null; // default value
    id3.OriginalType = null; // default value
    carparkInfo.IdentifierAttribute = id3;
    
    var carparkNumber = carparkInfo.CreateAttribute("CarparkNumber");
    carparkNumber.DataType = eSpace.TextType;
    carparkNumber.SetDefaultValue(null); // default value
    carparkNumber.Description = null; // default value
    carparkNumber.IsMandatory = true;
    carparkNumber.IsSearchable = false; // default value
    carparkNumber.Label = "Carpark Number";
    carparkNumber.Length = 20;
    carparkNumber.OriginalName = null; // default value
    carparkNumber.OriginalType = null; // default value
    
    var address = carparkInfo.CreateAttribute("Address");
    address.DataType = eSpace.TextType;
    address.SetDefaultValue(null); // default value
    address.Description = null; // default value
    address.IsMandatory = false; // default value
    address.IsSearchable = false; // default value
    address.Label = "Address";
    address.Length = 250;
    address.OriginalName = null; // default value
    address.OriginalType = null; // default value
    
    var carparkType = carparkInfo.CreateAttribute("CarparkType");
    carparkType.DataType = eSpace.TextType;
    carparkType.SetDefaultValue(null); // default value
    carparkType.Description = null; // default value
    carparkType.IsMandatory = false; // default value
    carparkType.IsSearchable = false; // default value
    carparkType.Label = "Carpark Type";
    carparkType.Length = 80;
    carparkType.OriginalName = null; // default value
    carparkType.OriginalType = null; // default value
    
    var parkingSystem = carparkInfo.CreateAttribute("ParkingSystem");
    parkingSystem.DataType = eSpace.TextType;
    parkingSystem.SetDefaultValue(null); // default value
    parkingSystem.Description = null; // default value
    parkingSystem.IsMandatory = false; // default value
    parkingSystem.IsSearchable = false; // default value
    parkingSystem.Label = "Parking System";
    parkingSystem.Length = 80;
    parkingSystem.OriginalName = null; // default value
    parkingSystem.OriginalType = null; // default value
    
    var shortTermParking = carparkInfo.CreateAttribute("ShortTermParking");
    shortTermParking.DataType = eSpace.TextType;
    shortTermParking.SetDefaultValue(null); // default value
    shortTermParking.Description = null; // default value
    shortTermParking.IsMandatory = false; // default value
    shortTermParking.IsSearchable = false; // default value
    shortTermParking.Label = "Short Term Parking";
    shortTermParking.Length = 80;
    shortTermParking.OriginalName = null; // default value
    shortTermParking.OriginalType = null; // default value
    
    var freeParking = carparkInfo.CreateAttribute("FreeParking");
    freeParking.DataType = eSpace.TextType;
    freeParking.SetDefaultValue(null); // default value
    freeParking.Description = null; // default value
    freeParking.IsMandatory = false; // default value
    freeParking.IsSearchable = false; // default value
    freeParking.Label = "Free Parking";
    freeParking.Length = 80;
    freeParking.OriginalName = null; // default value
    freeParking.OriginalType = null; // default value
    
    var nightParking = carparkInfo.CreateAttribute("NightParking");
    nightParking.DataType = eSpace.TextType;
    nightParking.SetDefaultValue(null); // default value
    nightParking.Description = null; // default value
    nightParking.IsMandatory = false; // default value
    nightParking.IsSearchable = false; // default value
    nightParking.Label = "Night Parking";
    nightParking.Length = 20;
    nightParking.OriginalName = null; // default value
    nightParking.OriginalType = null; // default value
    
    var decks = carparkInfo.CreateAttribute("Decks");
    decks.DataType = eSpace.TextType;
    decks.SetDefaultValue(null); // default value
    decks.Description = null; // default value
    decks.IsMandatory = false; // default value
    decks.IsSearchable = false; // default value
    decks.Label = "Decks";
    decks.Length = 10;
    decks.OriginalName = null; // default value
    decks.OriginalType = null; // default value
    
    var gantryHeight = carparkInfo.CreateAttribute("GantryHeight");
    gantryHeight.DataType = eSpace.TextType;
    gantryHeight.SetDefaultValue(null); // default value
    gantryHeight.Description = null; // default value
    gantryHeight.IsMandatory = false; // default value
    gantryHeight.IsSearchable = false; // default value
    gantryHeight.Label = "Gantry Height";
    gantryHeight.Length = 10;
    gantryHeight.OriginalName = null; // default value
    gantryHeight.OriginalType = null; // default value
    
    var hasBasement = carparkInfo.CreateAttribute("HasBasement");
    hasBasement.DataType = eSpace.TextType;
    hasBasement.SetDefaultValue(null); // default value
    hasBasement.Description = null; // default value
    hasBasement.IsMandatory = false; // default value
    hasBasement.IsSearchable = false; // default value
    hasBasement.Label = "Has Basement";
    hasBasement.Length = 5;
    hasBasement.OriginalName = null; // default value
    hasBasement.OriginalType = null; // default value
    
    var xCoord = carparkInfo.CreateAttribute("XCoord");
    xCoord.DataType = eSpace.TextType;
    xCoord.SetDefaultValue(null); // default value
    xCoord.Description = null; // default value
    xCoord.IsMandatory = false; // default value
    xCoord.IsSearchable = false; // default value
    xCoord.Label = "X Coord";
    xCoord.Length = 20;
    xCoord.OriginalName = null; // default value
    xCoord.OriginalType = null; // default value
    
    var yCoord = carparkInfo.CreateAttribute("YCoord");
    yCoord.DataType = eSpace.TextType;
    yCoord.SetDefaultValue(null); // default value
    yCoord.Description = null; // default value
    yCoord.IsMandatory = false; // default value
    yCoord.IsSearchable = false; // default value
    yCoord.Label = "Y Coord";
    yCoord.Length = 20;
    yCoord.OriginalName = null; // default value
    yCoord.OriginalType = null; // default value
    
    var syncedOn = carparkInfo.CreateAttribute("SyncedOn");
    syncedOn.DataType = eSpace.DateTimeType;
    syncedOn.SetDefaultValue(null); // default value
    syncedOn.Description = null; // default value
    syncedOn.IsMandatory = false; // default value
    syncedOn.IsSearchable = false; // default value
    syncedOn.Label = "Synced On";
    syncedOn.OriginalName = null; // default value
    syncedOn.OriginalType = null; // default value
    // IStaticEntitySignature OutSystemsUI/Color
    var color = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*px1ty9Qm2UW8IppMSC5s4g"));
    // IStaticEntitySignature OutSystemsUI/ColumnBreak
    var columnBreak = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*IazmzCqUL0mLRmTF5uoFew"));
    // IStaticEntitySignature OutSystemsUI/DatePickerLanguage
    var datePickerLanguage = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*5C5ARb9zokK1+Dca6UC_Tw"));
    // IStaticEntitySignature OutSystemsUI/DatePickerTimeFormat
    var datePickerTimeFormat = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*GkvlVf4O70uuEMJ2kSljTQ"));
    // IStaticEntitySignature OutSystemsUI/DatePickerWeekDay
    var datePickerWeekDay = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*utBO_bkaPkqisRj0F5O+LQ"));
    // IStaticEntitySignature OutSystemsUI/DeviceResponsive
    var deviceResponsive = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*HY5GKVqiw0K_U7IyrMwVDQ"));
    // IStaticEntitySignature OutSystemsUI/Direction
    var direction = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*hoBiZ4t_hE+peKnT9UeWnA"));
    // IServerEntitySignature (System)/Espace
    var espace = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*Peat6jeAoS7k0hgUWu_7Nw"));
    // IStaticEntitySignature OutSystemsUI/FloatingPosition
    var floatingPosition = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*KotrkgYJb06O9eGP3OS1rw"));
    // IStaticEntitySignature OutSystemsUI/Gradient
    var gradient = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*XrX0FejJDU+gQwCT4X+gTQ"));
    // IServerEntitySignature (System)/Group
    var group = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*77Uo0HdEN06fos3dfg5xRg"));
    // IStaticEntitySignature OutSystemsUI/GutterSize
    var gutterSize = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*6KGBLqRP0EupRWU1KZmwvg"));
    // IStaticEntitySignature OutSystemsCharts/HighchartModules
    var highchartModules = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*xdGASEDTGECq_FfOVwaSLg"));
    // IStaticEntitySignature OutSystemsCharts/LegendLayout
    var legendLayout = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*pW0A8z8iV0CJSolm4Bujnw"));
    // IStaticEntitySignature OutSystemsCharts/LegendPosition
    var legendPosition = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*l3rhqgo_y0GJN+Sdc3qXcQ"));
    
    /*** creating IStaticEntity 'LotType' ***/
    var lotType = eSpace.CreateStaticEntity("LotType");
    lotType.Description = "Lot type codes used by the data.gov.sg carpark-availability dataset.";
    lotType.ExposeProcessEvents = false; // default value
    lotType.Folder = null; // default value
    lotType.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.No; // default value
    lotType.IsRestrictedInQueries = false; // default value
    lotType.Label = "Lot Type";
    lotType.LabelPlural = "Lot Types";
    lotType.Public = false; // default value
    lotType.Shareable = false; // default value
    lotType.UpdateBehavior = OutSystems.Model.Enumerations.UpdateEntityBehaviorType.Optimized;
    lotType.UseTranslations = true;
    
    var id4 = lotType.CreateAttribute("Id");
    id4.DataType = eSpace.TextType;
    id4.SetDefaultValue(null); // default value
    id4.Description = null; // default value
    id4.IsSearchable = false; // default value
    id4.Label = "Id";
    id4.Length = 5;
    id4.OriginalName = null; // default value
    id4.OriginalType = null; // default value
    lotType.IdentifierAttribute = id4;
    
    var is_Active2 = lotType.CreateAttribute("Is_Active");
    is_Active2.DataType = eSpace.BooleanType;
    is_Active2.SetDefaultValue(null); // default value
    is_Active2.Description = null; // default value
    is_Active2.IsMandatory = true;
    is_Active2.IsSearchable = false; // default value
    is_Active2.Label = "Is Active";
    is_Active2.OriginalName = null; // default value
    is_Active2.OriginalType = null; // default value
    lotType.IsActiveAttribute = is_Active2;
    
    var label2 = lotType.CreateAttribute("Label");
    label2.DataType = eSpace.TextType;
    label2.SetDefaultValue(null); // default value
    label2.Description = null; // default value
    label2.IsMandatory = true;
    label2.IsSearchable = false; // default value
    label2.Label = "Label";
    label2.Length = 50;
    label2.OriginalName = null; // default value
    label2.OriginalType = null; // default value
    lotType.LabelAttribute = label2;
    
    var order2 = lotType.CreateAttribute("Order");
    order2.DataType = eSpace.IntegerType;
    order2.SetDefaultValue(null); // default value
    order2.Description = null; // default value
    order2.IsMandatory = true;
    order2.IsSearchable = false; // default value
    order2.Label = "Order";
    order2.OriginalName = null; // default value
    order2.OriginalType = null; // default value
    lotType.OrderByAttribute = order2;
    
    var staticEntityRecord6 = lotType.CreateRecord();
    staticEntityRecord6.Icon = null; // default value
    staticEntityRecord6.Identifier = "Motorcycle";
    staticEntityRecord6.SetAttributeValue(is_Active2, "True");
    staticEntityRecord6.SetAttributeValue(order2, "2");
    staticEntityRecord6.SetAttributeValue(label2, "\"Motorcycles\"");
    staticEntityRecord6.SetAttributeValue(id4, "\"Y\"");
    
    var staticEntityRecord7 = lotType.CreateRecord();
    staticEntityRecord7.Icon = null; // default value
    staticEntityRecord7.Identifier = "Car";
    staticEntityRecord7.SetAttributeValue(is_Active2, "True");
    staticEntityRecord7.SetAttributeValue(order2, "1");
    staticEntityRecord7.SetAttributeValue(label2, "\"Cars\"");
    staticEntityRecord7.SetAttributeValue(id4, "\"C\"");
    
    var staticEntityRecord8 = lotType.CreateRecord();
    staticEntityRecord8.Icon = null; // default value
    staticEntityRecord8.Identifier = "HeavyVehicle";
    staticEntityRecord8.SetAttributeValue(order2, "3");
    staticEntityRecord8.SetAttributeValue(is_Active2, "True");
    staticEntityRecord8.SetAttributeValue(id4, "\"H\"");
    staticEntityRecord8.SetAttributeValue(label2, "\"Heavy vehicles\"");
    
    var staticEntityRecord9 = lotType.CreateRecord();
    staticEntityRecord9.Icon = null; // default value
    staticEntityRecord9.Identifier = "MotorcycleWithSideCar";
    staticEntityRecord9.SetAttributeValue(is_Active2, "True");
    staticEntityRecord9.SetAttributeValue(id4, "\"S\"");
    staticEntityRecord9.SetAttributeValue(order2, "4");
    staticEntityRecord9.SetAttributeValue(label2, "\"Motorcycles with side car\"");
    // IStaticEntitySignature OutSystemsMaps/MapErrors
    var mapErrors = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*5BXgClPJM06uwJFnTQXL9Q"));
    // IStaticEntitySignature OutSystemsMaps/MapEventTriggered
    var mapEventTriggered = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*ZKnyfPheAUmr5uhMtmFqYg"));
    // IStaticEntitySignature OutSystemsMaps/MarkerEventTriggered
    var markerEventTriggered = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*shJYz8m9sk6CQAn2Nnxq1g"));
    // IStaticEntitySignature OutSystemsUI/MenuAction
    var menuAction = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*gyjBnKsG8EyZl+3nxsAtZw"));
    // IStaticEntitySignature OutSystemsUI/MessageStatus
    var messageStatus = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*wF8BwRzIzECgRr+ZzyGigA"));
    // IStaticEntitySignature OutSystemsUI/Month
    var month = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*JLfZM7GgCkuR+ychQ8RKhQ"));
    // IStaticEntitySignature OutSystemsUI/Orientation
    var orientation = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*JPhxTWOlY0+MdmJuTgvd2w"));
    // IStaticEntitySignature OutSystemsUI/Position
    var position = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*6HPAg8K6IkGXcqpuEipdNg"));
    // IServerEntitySignature (System)/Process
    var process = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*rk1aJNDvckKT91HxgCaO6w"));
    // IServerEntitySignature (System)/Role
    var role = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*NuqV5Rtc2D6TqrunUsf4kg"));
    // IStaticEntitySignature (System)/RuntimeKind
    var runtimeKind = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*64ECB3l5wUuxUhWl1Ubk4A"));
    // IStaticEntitySignature OutSystemsUI/ScrollbarStyle
    var scrollbarStyle = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*6t_Ww6JtikaA5FzmaNQ_cA"));
    // IServerEntitySignature (System)/Sent_Email
    var sent_Email = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*CWv+3dcKHk6ctXgJ81F2tg"));
    // IStaticEntitySignature OutSystemsCharts/SeriesType
    var seriesType = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*6GPGqVRjAk+THGNvxYt9UQ"));
    // IStaticEntitySignature OutSystemsUI/Shape
    var shape = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*qWjhb23B20ybb6nmxreTJg"));
    // IStaticEntitySignature OutSystemsUI/SideMenuBehavior
    var sideMenuBehavior = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*e5OlEU3pUEKZqJyDWNPZZQ"));
    // IStaticEntitySignature OutSystemsUI/Size
    var size = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*tBDBGmSJC0emsoGuSmtb3g"));
    
    /*** creating IStaticEntity 'SortOption' ***/
    var sortOption = eSpace.CreateStaticEntity("SortOption");
    sortOption.Description = "Sort orders offered on the dashboard. Applied in Dashboard.ApplyFilters.";
    sortOption.ExposeProcessEvents = false; // default value
    sortOption.Folder = null; // default value
    sortOption.IsMultiTenant = OutSystems.Model.Enumerations.BooleanWithInheritance.No; // default value
    sortOption.IsRestrictedInQueries = false; // default value
    sortOption.Label = "Sort Option";
    sortOption.LabelPlural = "Sort Options";
    sortOption.Public = false; // default value
    sortOption.Shareable = false; // default value
    sortOption.UpdateBehavior = OutSystems.Model.Enumerations.UpdateEntityBehaviorType.Optimized;
    sortOption.UseTranslations = true;
    
    var id5 = sortOption.CreateAttribute("Id");
    id5.DataType = eSpace.TextType;
    id5.SetDefaultValue(null); // default value
    id5.Description = null; // default value
    id5.IsSearchable = false; // default value
    id5.Label = "Id";
    id5.Length = 20;
    id5.OriginalName = null; // default value
    id5.OriginalType = null; // default value
    sortOption.IdentifierAttribute = id5;
    
    var is_Active3 = sortOption.CreateAttribute("Is_Active");
    is_Active3.DataType = eSpace.BooleanType;
    is_Active3.SetDefaultValue(null); // default value
    is_Active3.Description = null; // default value
    is_Active3.IsMandatory = true;
    is_Active3.IsSearchable = false; // default value
    is_Active3.Label = "Is Active";
    is_Active3.OriginalName = null; // default value
    is_Active3.OriginalType = null; // default value
    sortOption.IsActiveAttribute = is_Active3;
    
    var label3 = sortOption.CreateAttribute("Label");
    label3.DataType = eSpace.TextType;
    label3.SetDefaultValue(null); // default value
    label3.Description = null; // default value
    label3.IsMandatory = true;
    label3.IsSearchable = false; // default value
    label3.Label = "Label";
    label3.Length = 50;
    label3.OriginalName = null; // default value
    label3.OriginalType = null; // default value
    sortOption.LabelAttribute = label3;
    
    var order3 = sortOption.CreateAttribute("Order");
    order3.DataType = eSpace.IntegerType;
    order3.SetDefaultValue(null); // default value
    order3.Description = null; // default value
    order3.IsMandatory = true;
    order3.IsSearchable = false; // default value
    order3.Label = "Order";
    order3.OriginalName = null; // default value
    order3.OriginalType = null; // default value
    sortOption.OrderByAttribute = order3;
    
    var staticEntityRecord10 = sortOption.CreateRecord();
    staticEntityRecord10.Icon = null; // default value
    staticEntityRecord10.Identifier = "UpdatedDesc";
    staticEntityRecord10.SetAttributeValue(label3, "\"Most recently updated\"");
    staticEntityRecord10.SetAttributeValue(is_Active3, "True");
    staticEntityRecord10.SetAttributeValue(order3, "6");
    staticEntityRecord10.SetAttributeValue(id5, "\"UpdatedDesc\"");
    
    var staticEntityRecord11 = sortOption.CreateRecord();
    staticEntityRecord11.Icon = null; // default value
    staticEntityRecord11.Identifier = "NumberAsc";
    staticEntityRecord11.SetAttributeValue(is_Active3, "True");
    staticEntityRecord11.SetAttributeValue(label3, "\"Carpark number (A to Z)\"");
    staticEntityRecord11.SetAttributeValue(id5, "\"NumberAsc\"");
    staticEntityRecord11.SetAttributeValue(order3, "5");
    
    var staticEntityRecord12 = sortOption.CreateRecord();
    staticEntityRecord12.Icon = null; // default value
    staticEntityRecord12.Identifier = "PctDesc";
    staticEntityRecord12.SetAttributeValue(order3, "3");
    staticEntityRecord12.SetAttributeValue(label3, "\"Highest availability %\"");
    staticEntityRecord12.SetAttributeValue(is_Active3, "True");
    staticEntityRecord12.SetAttributeValue(id5, "\"PctDesc\"");
    
    var staticEntityRecord13 = sortOption.CreateRecord();
    staticEntityRecord13.Icon = null; // default value
    staticEntityRecord13.Identifier = "AvailableAsc";
    staticEntityRecord13.SetAttributeValue(is_Active3, "True");
    staticEntityRecord13.SetAttributeValue(label3, "\"Fewest available lots\"");
    staticEntityRecord13.SetAttributeValue(id5, "\"AvailableAsc\"");
    staticEntityRecord13.SetAttributeValue(order3, "2");
    
    var staticEntityRecord14 = sortOption.CreateRecord();
    staticEntityRecord14.Icon = null; // default value
    staticEntityRecord14.Identifier = "AvailableDesc";
    staticEntityRecord14.SetAttributeValue(id5, "\"AvailableDesc\"");
    staticEntityRecord14.SetAttributeValue(order3, "1");
    staticEntityRecord14.SetAttributeValue(is_Active3, "True");
    staticEntityRecord14.SetAttributeValue(label3, "\"Most available lots\"");
    
    var staticEntityRecord15 = sortOption.CreateRecord();
    staticEntityRecord15.Icon = null; // default value
    staticEntityRecord15.Identifier = "PctAsc";
    staticEntityRecord15.SetAttributeValue(is_Active3, "True");
    staticEntityRecord15.SetAttributeValue(order3, "4");
    staticEntityRecord15.SetAttributeValue(label3, "\"Highest occupancy %\"");
    staticEntityRecord15.SetAttributeValue(id5, "\"PctAsc\"");
    // IStaticEntitySignature OutSystemsUI/Space
    var space = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*cdSzj6SCnEOcwQVV36kUUQ"));
    // IStaticEntitySignature OutSystemsUI/Speed
    var speed = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*rsRcCk+1o06azp9Jx3JGBg"));
    // IStaticEntitySignature OutSystemsUI/StackedCardsPosition
    var stackedCardsPosition = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*tF6Gcf5VpUC8f0WwBaOgtQ"));
    // IStaticEntitySignature OutSystemsCharts/StackingType
    var stackingType = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*MtWj9wu4+EyoihsTikMiZg"));
    // IStaticEntitySignature OutSystemsUI/Steps
    var steps = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*n47d5CCm9U22GZuKdxvlow"));
    // IStaticEntitySignature OutSystemsUI/StepsBehaviour
    var stepsBehaviour = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*nuMDocxDtUiF1wAzZJFSKQ"));
    // IStaticEntitySignature OutSystemsMaps/Style
    var style = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*O7itoAjiOUC8kpHQG44ggQ"));
    // IServerEntitySignature (System)/Tenant
    var tenant = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*WQMM07oXpk63b_WESl+a5Q"));
    // IStaticEntitySignature OutSystemsUI/Trigger
    var trigger = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*uhIJjybGlUeQplMR4rlwyw"));
    // IStaticEntitySignature OutSystemsMaps/Type
    var type = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*w2QUMibLL0G2ChxxQNyMGw"));
    // IServerEntitySignature (System)/User
    var user = (OutSystems.Model.Data.IServerEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("uXCIR2Atc0+es3zYuZSnNw*LYIWpF+Ey0aicHD144NUTA"));
    // IStaticEntitySignature OutSystemsCharts/ValuesType
    var valuesType = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Iw63OPxQEEeAzzaCqdyZig*cCFW0FbPSkOF77jTPyAj8g"));
    // IStaticEntitySignature OutSystemsUI/VideoState
    var videoState = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("Kn_hixxDWEm4lMd7mIpycQ*N49Q8acBbUWd0GTf9i9tOQ"));
    // IStaticEntitySignature OutSystemsMaps/Zoom
    var zoom = (OutSystems.Model.Data.IStaticEntitySignature)eSpace.AddDependency(Services.ModelServices.ParseGlobalKey("0TG7lXnw1k+rLlyDJoVaqg*fPCs5IqzcUinwkAX6qvpXA"));
}
