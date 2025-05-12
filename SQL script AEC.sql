-- Database: AgriEnergyConnect

-- DROP DATABASE IF EXISTS "AgriEnergyConnect";

CREATE DATABASE "AgriEnergyConnect"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'C.UTF-8'
    LC_CTYPE = 'C.UTF-8'
    LOCALE_PROVIDER = 'libc'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

	CREATE TABLE Farmers (
    FarmerID SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Location VARCHAR(100),
    PasswordHash VARCHAR(255) NOT NULL
);

CREATE TABLE Products (
    ProductID SERIAL,
    FarmerID INT,
    Name VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    ProductionDate DATE NOT NULL,
    Description VARCHAR(255),
    
    CONSTRAINT pk_products PRIMARY KEY (ProductID),
    CONSTRAINT fk_farmer FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID)
);