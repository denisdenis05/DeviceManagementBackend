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
            Description: "",
            AssignedUserId: "",
            AssignedUserEmail: ""
        },
        { 
            Name: "iPad Air", 
            Manufacturer: "Apple", 
            Type: "tablet", 
            OperatingSystem: "iPadOS", 
            OsVersion: "16.4", 
            Processor: "M1", 
            RamAmount: 8, 
            Description: "",
            AssignedUserId: "",
            AssignedUserEmail: ""
        }
    ]);
}

const usersCollectionName = "Users";
const initialUsersCount = db[usersCollectionName].countDocuments();
if (initialUsersCount === 0) {
    db[usersCollectionName].insertOne({
        Email: "admin@admin.com",
        PasswordHash: "$2a$11$0wO0M1iK./AetqNofNntUetK5L6SGEW31pBohPTrXb8M4X368L1V2" 
    });
}
