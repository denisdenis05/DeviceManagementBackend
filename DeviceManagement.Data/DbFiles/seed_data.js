const databaseName = "DeviceManagementDb";
db = db.getSiblingDB(databaseName);
const requiredCollectionName = "Devices";
const initialDataCount = db[requiredCollectionName].countDocuments();
const hasNoData = initialDataCount === 0;
if (hasNoData) {
    db[requiredCollectionName].insertMany([
        { 
            Name: "Galaxy S23", 
            Manufacturer: "Samsung", 
            Type: "phone", 
            OperatingSystem: "Android", 
            OsVersion: "13.0", 
            Processor: "Snapdragon 8 Gen 2", 
            RamAmount: 8, 
            Description: "Standard edition flagship" 
        },
        { 
            Name: "iPad Air", 
            Manufacturer: "Apple", 
            Type: "tablet", 
            OperatingSystem: "iPadOS", 
            OsVersion: "16.4", 
            Processor: "M1", 
            RamAmount: 8, 
            Description: "Mid-tier tablet device" 
        }
    ]);
}
