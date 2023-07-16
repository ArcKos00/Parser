CREATE SCHEMA kpanfilenko_Parser;
GO

CREATE TABLE kpanfilenko_Parser.Brand (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Name NVARCHAR(200) NOT NULL,
  INDEX brandIndex (Name)
);

CREATE TABLE kpanfilenko_Parser.CarName (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Name NVARCHAR(200) NOT NULL,
  BrandId INT,
  FOREIGN KEY (BrandId) REFERENCES kpanfilenko_Parser.Brand(Id)
);

CREATE TABLE kpanfilenko_Parser.Car (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  CarCode NVARCHAR(200) NOT NULL,
  DateStart DATE NOT NULL,
  DateEnd DATE NULL,
  Complectation NVARCHAR(200) NOT NULL,
  CarNameId INT,
  FOREIGN KEY (CarNameId) REFERENCES kpanfilenko_Parser.CarName(Id),
  INDEX CarCode (CarCode)
);

CREATE TABLE kpanfilenko_Parser.CarComplectation (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Complectation NVARCHAR(200) NOT NULL,
  DateStart DATE NOT NULL,
  DateEnd DATE NULL,
  CarId INT,
  FOREIGN KEY (CarId) REFERENCES kpanfilenko_Parser.Car(Id),
  INDEX ComplectationCode (Complectation)
);

CREATE TABLE kpanfilenko_Parser.Attribute (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Name NVARCHAR(200) NOT NULL,
  Value NVARCHAR(200) NULL,
);

CREATE TABLE kpanfilenko_Parser.Attribute_Complectation (
  ComplectationId INT NULL,
  AttributeId INT NULL,
  FOREIGN KEY (ComplectationId) REFERENCES kpanfilenko_Parser.CarComplectation(Id),
  FOREIGN KEY (AttributeId) REFERENCES kpanfilenko_Parser.Attribute(Id),
);

CREATE TABLE kpanfilenko_Parser.[Group] (
  Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
  Name NVARCHAR(200) NOT NULL,
  INDEX Name (Name)
);

CREATE TABLE kpanfilenko_Parser.SubGroup (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Name NVARCHAR(200) NOT NULL,
  INDEX NAme (Name)
);

CREATE TABLE kpanfilenko_Parser.Group_SubGroup (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  GroupId INT,
  SubGroupId INT,
  FOREIGN KEY (GroupId) REFERENCES kpanfilenko_Parser.[Group](Id),
  FOREIGN KEY (SubGroupId) REFERENCES kpanfilenko_Parser.SubGroup(Id)
);

CREATE TABLE kpanfilenko_Parser.Image (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  ImageName NVARCHAR(200) NOT NULL
);

CREATE TABLE kpanfilenko_Parser.Complectation_Group_SubGroup (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  GroupSubGroupId INT,
  ComplectationId INT,
  ImageId INT,
  FOREIGN KEY (ImageId) REFERENCES kpanfilenko_Parser.[Image](Id),
  FOREIGN KEY (GroupSubGroupId) REFERENCES kpanfilenko_Parser.Group_SubGroup(Id),
  FOREIGN KEY (ComplectationId) REFERENCES kpanfilenko_Parser.CarComplectation(Id)
);

CREATE TABLE kpanfilenko_Parser.Detail (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  CodeTree NVARCHAR(200) NOT NULL,
  NameTree NVARCHAR(200) NOT NULL,
  ComplectationGroupSubrGoupId INT,
  FOREIGN KEY (ComplectationGroupSubrGoupId) REFERENCES kpanfilenko_Parser.Complectation_Group_SubGroup(Id),
);

CREATE TABLE kpanfilenko_Parser.InfoForCode (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  [Count] INT NOT NULL,
  DateStart DATE NOT NULL,
  DateEnd DATE,
  Info NVARCHAR(255) NOT NULL,
);

CREATE TABLE kpanfilenko_Parser.DetailInfoCode (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Code NVARCHAR(200) NOT NULL,
  DetailId INT,
  DetailInfoId INT,
  FOREIGN KEY (DetailId) REFERENCES kpanfilenko_Parser.Detail(Id),
  FOREIGN KEY (DetailInfoId) REFERENCES kpanfilenko_Parser.InfoForCode(Id),
  INDEX code (Code)
);
GO

CREATE SCHEMA kpanfilenko_GetView;
GO

Alter VIEW kpanfilenko_GetView.vw_global_view AS 
SELECT
    cc.Id,
    cc.Complectation AS 'ComplectationName', 
    cc.DateStart AS 'ComplectationStartDate', 
    cc.DateEnd AS 'ComplectationEndDate',
    a.Name AS 'ColumnName',
    a.[Value] AS 'ColumnValue',
    g.Name AS 'GroupName',
    sg.Name AS 'SubGroupName',
    i.ImageName AS 'ImageName',
    d.CodeTree AS 'TreeCode',
    d.NameTree AS 'TreeName',
    dic.Code AS 'DetailCode',
    dic.DetailInfoId,
    ifc.Id AS 'InfoId',
    ifc.DateStart AS 'DetailStartDate',
    ifc.DateEnd AS 'DetailEndDate',
    ifc.Info  AS 'Usings',
    ifc.[Count] AS 'Count'
FROM kpanfilenko_Parser.CarComplectation cc
JOIN kpanfilenko_Parser.Attribute_Complectation ac ON cc.Id = ac.ComplectationId
JOIN kpanfilenko_Parser.Attribute a ON ac.AttributeId = a.Id
JOIN kpanfilenko_Parser.Complectation_Group_SubGroup cgsg ON cc.Id = cgsg.ComplectationId
JOIN kpanfilenko_Parser.Group_SubGroup gsg ON cgsg.GroupSubGroupId = gsg.Id
JOIN kpanfilenko_Parser.[Group] g ON gsg.GroupId = g.Id
JOIN kpanfilenko_Parser.SubGroup sg ON gsg.SubGroupId = sg.Id
JOIN kpanfilenko_Parser.Detail d ON cgsg.Id = d.ComplectationGroupSubrGoupId
JOIN kpanfilenko_Parser.[Image] i ON cgsg.ImageId = i.Id
JOIN kpanfilenko_Parser.DetailInfoCode dic ON d.Id = dic.DetailId
JOIN kpanfilenko_Parser.InfoForCode ifc ON dic.DetailInfoId = ifc.Id
GO

CREATE VIEW kpanfilenko_GetView.vw_models
AS
    SELECT
        b.Id AS 'BrandId',
        c.Id AS 'CarId',
        cn.Name AS 'Name',
        c.CarCode,
        c.DateStart,
        c.DateEnd,
        c.Complectation
    FROM kpanfilenko_Parser.Brand b
    JOIN kpanfilenko_Parser.CarName cn ON cn.BrandId = b.Id
    JOin kpanfilenko_Parser.Car c ON cn.Id = c.CarNameId
GO

CREATE VIEW kpanfilenko_GetView.vw_complectation
AS
    SELECT 
        cc.CarId,
        cc.Id,
        cc.Complectation AS 'Name',
        cc.DateStart AS 'StartDate',
        cc.DateEnd AS 'EndDate',
        a.Name AS 'ColumnName',
        a.[Value] AS 'ColumnValue'
    FROM kpanfilenko_Parser.CarComplectation cc
    JOIN kpanfilenko_Parser.Attribute_Complectation ac ON ac.ComplectationId = cc.Id
    JOIN kpanfilenko_Parser.Attribute a ON a.Id = ac.AttributeId
GO

CREATE VIEW kpanfilenko_GetView.vw_groups
AS
    SELECT DISTINCT
        cgsg.ComplectationId AS 'ComplecataionId',
        g.Id,
        g.Name
    FROM kpanfilenko_Parser.[Group] g
    JOIN kpanfilenko_Parser.Group_SubGroup gsg ON g.Id = gsg.GroupId
    JOIN kpanfilenko_Parser.Complectation_Group_SubGroup cgsg ON gsg.Id = cgsg.GroupSubGroupId
GO

CREATE VIEW kpanfilenko_GetView.vw_subGroups
AS
    SELECT
        cgsg.ComplectationId AS 'ComplectationId',
        gsg.GroupId AS 'GroupId',
        sg.Id,
        sg.Name
    FROM kpanfilenko_Parser.SubGroup sg
    JOIN kpanfilenko_Parser.Group_SubGroup gsg ON sg.Id = gsg.SubGroupId
    JOIN kpanfilenko_Parser.Complectation_Group_SubGroup cgsg ON gsg.Id = cgsg.GroupSubGroupId
GO

CREATE VIEW kpanfilenko_GetView.vw_details
AS
    SELECT
        cgsg.Id,
        cgsg.ComplectationId,
        gsg.GroupId,
        gsg.SubGroupId,
        i.ImageName,
        d.Id AS 'DetailId',
        d.CodeTree AS 'TreeCode',
        d.NameTree AS 'TreeName',
        dic.Code,
        ifc.[Count],
        ifc.DateStart AS 'StartDate',
        ifc.DateEnd AS 'EndDate',
        ifc.Info AS 'Usings'
    FROM kpanfilenko_Parser.Complectation_Group_SubGroup cgsg
    JOIN kpanfilenko_Parser.Detail d ON cgsg.Id = d.ComplectationGroupSubrGoupId
    JOin kpanfilenko_Parser.[Image] i ON cgsg.ImageId = i.Id
    JOIN kpanfilenko_Parser.Group_SubGroup gsg ON cgsg.GroupSubGroupId = gsg.Id
    JOIN kpanfilenko_Parser.DetailInfoCode dic ON d.Id = dic.DetailId
    JOIN kpanfilenko_Parser.InfoForCode ifc ON dic.DetailInfoId = ifc.Id
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_carPage_insert_page
    @brand NVARCHAR(200),
    @name NVARCHAR(200),
    @code NVARCHAR(200),
    @startDate DATE,
    @endDate DATE = NULL,
    @assembly NVARCHAR(200)
AS
BEGIN
    DECLARE @brandId INT;
    SET @brandId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.Brand br
        WHERE br.Name = @brand
    )

    IF @brandId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.Brand(Name) VALUES(@brand)
        SET @brandId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    DECLARE @carNameId INT;
    SET @carNameId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.CarName cn
        WHERE cn.Name = @name AND cn.BrandId = @brandId
    )

    IF @carNameId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.CarName(Name, BrandId) VALUES(@name, @brandId)
        SET @carNameId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    DECLARE @carId INT;
    SET @carId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.Car car
        WHERE car.CarCode = @code
    )
    
    IF @carId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.Car(CarCode, DateStart, DateEnd, Complectation, CarNameId) VALUES(@code, @startDate, @endDate, @assembly, @carNameId);
        SET @carId = (
            SELECT SCOPE_IDENTITY()
        )
    END
    SELECT @carId as 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_complectationPage_insert_complectation
    @complectation NVARCHAR(200),
    @startDate DATE,
    @endDate DATE = NULL,
    @carId INT
AS
BEGIN
    DECLARE @complectationId INT;
    SET @complectationId =(
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.CarComplectation comp
        WHERE comp.Complectation = @complectation AND comp.DateEnd = @endDate AND comp.DateStart = @startDate
    )

    IF @complectationId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.CarComplectation(Complectation, DateStart, DateEnd, CarId) VALUES(@complectation, @startDate, @endDate, @carId)
        SET @complectationId = (
            SELECT SCOPE_IDENTITY() 
        )
    END

    SELECT @complectationId AS 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_complectationPage_insert_attribute
    @name NVARCHAR(200),
    @value NVARCHAR(200)
AS
BEGIN
    DECLARE @attributeId INT;
    SET @attributeId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.Attribute item
        WHERE item.Name = @name and item.[Value] = @value
    )

    IF @attributeId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.Attribute(Name, [Value]) VALUES(@name, @value)
        SET @attributeId = (
            SCOPE_IDENTITY()
        )
    END

    SELECT @attributeId AS 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_complectationPage_insert_attribute_complectation
    @attributeId INT,
    @complectationId INT
AS
BEGIN
    DECLARE @id INT;
    SET @id = (
        SELECT TOP(1) ComplectationId
        FROM kpanfilenko_Parser.Attribute_Complectation icom
        WHERE icom.ComplectationId = @complectationId AND icom.AttributeId = @attributeId
    )

    IF @id IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.Attribute_Complectation(ComplectationId, AttributeId) VALUES(@complectationId, @attributeId)
    END

    SELECT @id
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_group_insert_page
    @name NVARCHAR(200)
AS
BEGIN
    DECLARE @groupId INT;
    SET @groupId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.[Group] gr
        WHERE gr.Name = @name
    )

    IF @groupId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.[Group](Name) VALUES(@name)
        SET @groupId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    SELECT @groupId AS 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_subGroup_insert_page
    @name NVARCHAR(200),
    @groupId INT,
    @complectationId INT,
    @imageId INT
AS
BEGIN
    DECLARE @subGroupId INT;
    SET @subGroupId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.[SubGroup] gr
        WHERE gr.Name = @name
    )

    IF @subGroupId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.[SubGroup](Name) VALUES(@name)
        SET @subGroupId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    DECLARE @group_subGroupId INT;
    SET @group_subGroupId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.Group_SubGroup gsg
        WHERE gsg.GroupId = @groupId AND gsg.SubGroupId = @subGroupId
    )

    IF @group_subGroupId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.Group_SubGroup(GroupId, SubGroupId) VALUES(@groupId, @subGroupId)
        SET @group_subGroupId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    DECLARE @group_subGroup_complectationId INT;
    SET @group_subGroup_complectationId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.Complectation_Group_SubGroup cgsg
        WHERE cgsg.ComplectationId = @complectationId AND cgsg.GroupSubGroupId = @group_subGroupId AND cgsg.ImageId = @imageId
    )

    IF @group_subGroup_complectationId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.Complectation_Group_SubGroup(ComplectationId, GroupSubGroupId, ImageId) VALUES(@complectationId, @group_subGroupId, @imageId)
        SET @group_subGroup_complectationId = (
            SCOPE_IDENTITY()
        )
    END

    SELECT @group_subGroup_complectationId AS 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_image_insert
    @imageName NVARCHAR(200)
AS
BEGIN
    DECLARE @imageId INT
    SET @imageId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.[Image] im
        WHERE im.ImageName = @imageName
    )

    IF @imageId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.[Image](ImageName) VALUES(@imageName)
        SET @imageId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    SELECT @imageId AS 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_detail_insert_page
    @codeTree NVARCHAR(200),
    @nameTree NVARCHAR(200),
    @groupSubGroupComplectationId INT
AS
BEGIN
    DECLARE @detailId INT;
    SET @detailId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.Detail det
        WHERE det.CodeTree = @codeTree AND det.NameTree = @nameTree AND det.ComplectationGroupSubrGoupId = @groupSubGroupComplectationId
    )

    IF @detailId IS NULL
    BEGIN 
        INSERT INTO kpanfilenko_Parser.Detail(CodeTree, NameTree, ComplectationGroupSubrGoupId) VALUES(@codeTree, @nameTree, @groupSubGroupComplectationId)
        SET @detailId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    SELECT @detailId AS 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_detailInfo_insert_info
    @count INT,
    @startDate DATE,
    @endDate DATE = NULL,
    @info NVARCHAR(255)
AS
BEGIN
    DECLARE @infoId INT
    SET @infoId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.InfoForCode ifc
        WHERE ifc.[Count] = @count AND ifc.Info = @info AND ifc.DateStart = @startDate AND ifc.DateEnd = @endDate
    )

    IF @infoId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.InfoForCode([Count], DateStart, DateEnd, Info) VALUES(@count, @startDate, @endDate, @info)
        SET @infoId = (
            SELECT SCOPE_IDENTITY()
        )
    END

    SELECT @infoId AS 'Id'
END
GO

CREATE PROCEDURE kpanfilenko_Parser.p_parser_detailCode_insert_code
    @code NVARCHAR(200),
    @detailId INT,
    @infoForCodeId INT
AS
BEGIN
    DECLARE @codeId INT;
    SET @codeId = (
        SELECT TOP(1) Id
        FROM kpanfilenko_Parser.DetailInfoCode infc
        WHERE infc.Code = @code
    )

    IF @codeId IS NULL
    BEGIN
        INSERT INTO kpanfilenko_Parser.DetailInfoCode(Code, DetailInfoId, DetailId) VALUES(@code, @infoForCodeId, @detailId)
        SET @codeId = (
            SELECT SCOPE_IDENTITY()
        )
    END
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_brand_getBrands
AS
BEGIN
    SELECT 
        b.Id,
        b.Name 
    FROM kpanfilenko_Parser.Brand b
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_models_getModels
    @brandId INT
AS
BEGIN
    SELECT
        vm.CarId AS 'Id',
        vm.Name,
        vm.CarCode AS 'Code',
        vm.DateStart AS 'StartDate',
        vm.DateEnd AS 'EndDate',
        vm.Complectation AS 'Complectations'
    FROM kpanfilenko_GetView.vw_models vm
    WHERE vm.BrandId = @brandId
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_complectations_getComplectations
    @carId INT
AS
BEGIN
    SELECT
        comp.Id,
        comp.Name,
        comp.StartDate,
        comp.EndDate,
        comp.ColumnName,
        comp.ColumnValue
    FROM kpanfilenko_GetView.vw_complectation comp
    WHERE comp.CarId = @carId
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_attributes_getAttributes
    @complectationId INT
AS
BEGIN
    SELECT
        comp.ColumnName,
        comp.ColumnValue
    FROM kpanfilenko_GetView.vw_complectation comp
    WHERE comp.Id = @complectationId
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_groups_getGroups
    @complectationId INT
AS
BEGIN
    SELECT DISTINCT
        g.Id,
        g.Name
    FROM kpanfilenko_GetView.vw_groups g
    WHERE g.ComplecataionId = @complectationid
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_subGroups_getSubGroups
    @complectationId INT,
    @groupId INT
AS
BEGIN
    SELECT
        sg.Id,
        sg.Name
    FROM kpanfilenko_GetView.vw_subGroups sg
    WHERE sg.GroupId = @groupid AND sg.ComplectationId = @complectationid
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_details_getShemaInfo
    @complectationId INT,
    @groupId INT,
    @subGroupId INT
AS
BEGIN
    SELECT DISTINCT
        d.Id,
        d.ImageName
    FROM kpanfilenko_GetView.vw_details d
    WHERE d.ComplectationId = @complectationId AND d.GroupId = @groupId AND d.SubGroupId = @subGroupId
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_details_getDetailInfo
    @comp_gr_subgrId INT
AS
BEGIN
    SELECT DISTINCT
        d.TreeCode,
        d.TreeName,
        d.StartDate,
        d.EndDate,
        d.[Count],
        d.Usings
    FROM kpanfilenko_GetView.vw_details d
    WHERE d.Id = @comp_gr_subgrId
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_details_getDetailIds
    @comp_gr_sugrId INT
AS
BEGIN
    SELECT DISTINCT
        d.DetailId AS 'Id'
    FROM kpanfilenko_GetView.vw_details d
    WHERE d.Id = @comp_gr_sugrId
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_details_getDetailCodes
    @detailId INT
AS
BEGIN
    SELECT DISTINCT
        d.Code
    FROM kpanfilenko_GetView.vw_details d
    WHERE d.DetailId = @detailId
END
GO

CREATE PROCEDURE kpanfilenko_GetView.p_parser_globalView_get
    @complectationId INT
AS
BEGIN
    SELECT
        gvw.Id,
        gvw.ComplectationName,
        gvw.ComplectationStartDate,
        gvw.ComplectationEndDate,
        gvw.ColumnName,
        gvw.columnValue,
        gvw.GroupName,
        gvw.SubGroupName,
        gvw.ImageName,
        gvw.TreeCode,
        gvw.TreeName,
        gvw.DetailCode,
        gvw.DetailStartDate,
        gvw.DetailEndDate,
        gvw.[Count],
        gvw.Usings
    FROM kpanfilenko_GetView.vw_global_view gvw
    WHERE gvw.Id = @complectationId
END
GO

CREATE SCHEMA kpanfilenko_TableInfo;
GO

CREATE PROCEDURE kpanfilenko_TableInfo.p_tables_getTableInfo
AS
BEGIN
    SELECT TOP 10
        s.name AS SchemaName,
        t.name AS TableName,
        p.rows AS [RowCount],
        SUM(a.total_pages * 8) AS TableSizeKB
    FROM sys.tables t
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
    INNER JOIN sys.indexes i ON t.object_id = i.object_id
    INNER JOIN sys.partitions p ON i.object_id = p.object_id AND i.index_id = p.index_id
    INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
    WHERE t.is_ms_shipped = 0
    GROUP BY t.name, s.name, p.rows
    ORDER BY TableSizeKB DESC;
END
GO