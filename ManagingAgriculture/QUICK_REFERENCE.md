# Quick Reference - Class and File Structure

## Model Classes (All with XML Documentation)

```
Models/
├── Plant.cs ✅ COMMENTED
│   └── Properties: Id, Name, PlantType, PlantedDate, ExpectedHarvestDate, GrowthStagePercent
│       NextTask, Notes, Location, Status, SoilType, SunlightExposure, IsIndoor, AvgTemperatureCelsius
│       WateringFrequencyDays, CreatedDate, UpdatedDate
│
├── Resource.cs ✅ COMMENTED
│   └── Properties: Id, Name, Category, Quantity, Unit, LowStockThreshold, Supplier
│       CreatedDate, UpdatedDate
│
├── Machinery.cs ✅ COMMENTED
│   └── Properties: Id, Name, Type, Status, PurchaseDate, LastServiceDate, NextServiceDate
│       PurchasePrice, CreatedDate, UpdatedDate
│
└── MarketplaceListing.cs ✅ COMMENTED
    └── Properties: Id, ItemName, Category, ConditionStatus, Description, SalePrice
        RentalPricePerDay, SellerName, SellerPhone, ImageUrl, ListingType
        ListingStatus, CreatedDate, UpdatedDate
```

## Controllers (All with Full CRUD Support)

```
Controllers/
├── ResourcesController.cs ✅ IMPLEMENTED
│   ├── In-Memory Data: 5 sample resources
│   └── Methods: Index(category), Add [GET], Add [POST]
│
├── MachineryController.cs ✅ IMPLEMENTED
│   ├── In-Memory Data: 3 sample machinery items
│   └── Methods: Index, Add [GET], Add [POST], Edit [GET], Edit [POST], Delete [POST]
│
└── MarketplaceController.cs ✅ IMPLEMENTED
    ├── In-Memory Data: 3 sample marketplace listings
    └── Methods: Index, Add [GET], Add [POST], Edit [GET], Edit [POST], Delete [POST]
```

## Views (Updated/Created)

```
Views/
├── Resources/
│   └── Index.cshtml ✅ UPDATED - Dynamic @foreach loop
│
├── Machinery/
│   ├── Index.cshtml ✅ UPDATED - Dynamic @foreach loop + styled button
│   ├── Add.cshtml ✅ CREATED - Complete add form
│   └── Edit.cshtml ✅ CREATED - Complete edit form
│
├── Marketplace/
│   ├── Index.cshtml ✅ UPDATED - Dynamic @foreach loop + styled button
│   ├── Add.cshtml ✅ CREATED - Complete post form
│   └── Edit.cshtml ✅ CREATED - Complete edit form
│
└── Home/
    └── Index.cshtml ✅ UPDATED - Background image to /images/homeBackground
```

## Sample Data Pre-Loaded

### Resources (5 items in ResourcesController)
- NPK Fertilizer 20-20-20 (150 kg) - Category: Fertilizer
- Corn Seeds (25 kg) - Category: Seed - LOW STOCK ⚠️
- Pesticide Spray (80 liters) - Category: Chemical
- Irrigation Water (5000 liters) - Category: Water
- Diesel Fuel (200 liters) - Category: Fuel

### Machinery (3 items in MachineryController)
- John Deere 5075E - Type: Tractor - Status: Excellent
- Kubota M5-091 - Type: Tractor - Status: Good
- Claas Lexion 780 - Type: Combine Harvester - Status: Fair

### Marketplace (3 items in MarketplaceController)
- Used John Deere Tractor ($32,000) - Type: Sale
- Equipment Rental - Round Baler ($150/day) - Type: Rent
- Organic Tomato Seeds ($25) - Type: Sale

---

## Feature Checklist

✅ Resources page with category filtering
✅ Machinery page with Add Equipment button (styled)
✅ Machinery Add form (GET & POST)
✅ Machinery Edit form (GET & POST)
✅ Machinery Delete function
✅ Marketplace page with Post Listing button (styled)
✅ Marketplace Add form (GET & POST)
✅ Marketplace Edit form (GET & POST)
✅ Marketplace Delete function
✅ Home page background image updated
✅ All models documented with XML comments
✅ All controllers documented with XML comments
✅ Dynamic data binding in all views
✅ Consistent CRUD pattern across modules

---

## How to Use

### Add New Resource
1. Navigate to Resources page
2. Resources are displayed from in-memory data
3. Filter by category using tab buttons
4. To add: Would need to extend ResourcesController with Add view

### Add New Equipment (Machinery)
1. Navigate to Machinery page
2. Click "Add Equipment" button (green, Bootstrap styled)
3. Fill form: Name, Type, PurchaseDate, Status, PurchasePrice, Maintenance dates
4. Submit → Equipment added to list

### Edit Equipment
1. On Machinery page, click "Edit" button on any card
2. Modify fields as needed
3. Submit → Changes saved (timestamps updated)

### Delete Equipment
1. On Machinery page, click "Delete" button
2. Confirm deletion in dialog
3. Equipment removed from list

### Post Marketplace Listing
1. Navigate to Marketplace page
2. Click "Post Listing" button (green, Bootstrap styled)
3. Fill form: ItemName, Category, ConditionStatus, Description, Pricing, Seller info
4. Submit → Listing added and appears at top of list

### Edit Listing
1. On Marketplace page, click "Edit" button on any card
2. Modify fields including ListingStatus
3. Submit → Changes saved

---

## Architecture Note

All three modules use **in-memory static List<T>** collections for prototype testing.
This data will reset when the application restarts.
For production, replace with Entity Framework Core and SQL database.

