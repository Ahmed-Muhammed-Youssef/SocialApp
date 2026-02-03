-- Seeding Roles
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Admin', 'ADMIN', NEWID());
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Moderator')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Moderator', 'MODERATOR', NEWID());
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'User')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'User', 'USER', NEWID());
END

-- Seeding Countries (Egypt)
DECLARE @CountryId INT;

IF NOT EXISTS (SELECT 1 FROM Countries WHERE Code = 'EG')
BEGIN
    INSERT INTO Countries (Name, Code, Language)
    VALUES ('Egypt', 'EG', 'ar');
    
    SET @CountryId = SCOPE_IDENTITY();

    -- Regions and Cities
    DECLARE @RegionId INT;

    -- Cairo
    INSERT INTO Regions (Name, CountryId) VALUES ('Cairo', @CountryId);
    SET @RegionId = SCOPE_IDENTITY();
    INSERT INTO Cities (Name, RegionId) VALUES 
    ('Nasr City', @RegionId),
    ('Heliopolis', @RegionId),
    ('Maadi', @RegionId),
    ('Downtown Cairo', @RegionId);

    -- Alexandria
    INSERT INTO Regions (Name, CountryId) VALUES ('Alexandria', @CountryId);
    SET @RegionId = SCOPE_IDENTITY();
    INSERT INTO Cities (Name, RegionId) VALUES 
    ('Montaza', @RegionId),
    ('Sidi Gaber', @RegionId),
    ('Smouha', @RegionId),
    ('Mansheya', @RegionId);

    -- Giza
    INSERT INTO Regions (Name, CountryId) VALUES ('Giza', @CountryId);
    SET @RegionId = SCOPE_IDENTITY();
    INSERT INTO Cities (Name, RegionId) VALUES 
    ('Dokki', @RegionId),
    ('Mohandessin', @RegionId),
    ('Haram', @RegionId),
    ('Sheikh Zayed', @RegionId);
END
