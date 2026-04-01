const databaseName = "DeviceManagementDb";
db = db.getSiblingDB(databaseName);
const requiredCollectionName = "Devices";
const existingCollections = db.getCollectionNames();
const collectionExists = existingCollections.includes(requiredCollectionName);
if (!collectionExists) {
    db.createCollection(requiredCollectionName, {
        validator: {
            $jsonSchema: {
                bsonType: "object",
                required: ["Name", "Manufacturer", "Type", "OperatingSystem", "OsVersion", "Processor", "RamAmount"],
                properties: {
                    Name: { bsonType: "string" },
                    Manufacturer: { bsonType: "string" },
                    Type: { bsonType: "string" },
                    OperatingSystem: { bsonType: "string" },
                    OsVersion: { bsonType: "string" },
                    Processor: { bsonType: "string" },
                    RamAmount: { bsonType: "int" },
                    Description: { bsonType: "string" },
                    AssignedUserId: { bsonType: "string" },
                    AssignedUserEmail: { bsonType: "string" }
                }
            }
        }
    });
}

const usersCollectionName = "Users";
const usersCollectionExists = existingCollections.includes(usersCollectionName);
if (!usersCollectionExists) {
    db.createCollection(usersCollectionName, {
        validator: {
            $jsonSchema: {
                bsonType: "object",
                required: ["Email", "PasswordHash"],
                properties: {
                    Email: { bsonType: "string" },
                    PasswordHash: { bsonType: "string" }
                }
            }
        }
    });
}
