# Application Flow & Navigation Guide

## Site Navigation Map

```
🏠 Home Page
├── Hero Section (background: /images/homeBackground) ✅
├── Navigation Bar
│   ├── Dashboard
│   ├── Plants ← (existing, fully functional)
│   ├── Resources ← (UPDATED: dynamic data)
│   ├── Machinery ← (NEW: full CRUD)
│   ├── Marketplace ← (UPDATED: full CRUD)
│   ├── Sensors
│   ├── Account (Login/Register)
│   └── Sidebar Toggle
└── Tour Modules & Features

📦 Resources Module (Category Filtering)
├── Index Page
│   ├── Category Tabs: All, Fertilizer, Seed, Chemical, Water, Fuel ✅
│   ├── Resource Cards (Dynamic @foreach)
│   │   ├── NPK Fertilizer (150 kg) ✓ In Stock
│   │   ├── Corn Seeds (25 kg) ⚠️ LOW STOCK → Alert shown
│   │   ├── Pesticide Spray (80 L)
│   │   ├── Irrigation Water (5000 L)
│   │   └── Diesel Fuel (200 L)
│   └── Filter Functionality
│       └── Click "Fertilizer" → Shows only fertilizer resources
└── Add Resource (extensible - form ready)

🚜 Machinery Module (Full CRUD)
├── Index Page
│   ├── Equipment List (Dynamic @foreach)
│   │   ├── John Deere 5075E (Excellent)
│   │   │   ├── Edit Button → Edit Form
│   │   │   └── Delete Button → Confirm → Remove
│   │   ├── Kubota M5-091 (Good)
│   │   └── Claas Lexion 780 (Fair)
│   └── 🟢 Add Equipment Button (Bootstrap styled - FIXED)
│       └── Links to Machinery/Add
├── Add Equipment Form
│   ├── Name, Type (dropdown), Purchase Date
│   ├── Status (Excellent/Good/Fair/Poor)
│   ├── Price & Maintenance Schedule
│   ├── Save Button → New item added to list
│   └── Cancel Button → Back to Index
├── Edit Equipment Form
│   ├── Pre-filled with existing data
│   ├── All fields editable
│   ├── Save Button → Updates list
│   └── Cancel Button → Back to Index
└── Delete Operation
    ├── Click Delete → Confirmation dialog
    ├── Confirm → Item removed
    └── Cancel → Stay on page

🏪 Marketplace Module (Full CRUD + Pricing)
├── Index Page
│   ├── Listing Cards (Dynamic @foreach)
│   │   ├── Used John Deere Tractor
│   │   │   ├── $32,000 (Sale)
│   │   │   ├── Seller: Farm Co. Ltd
│   │   │   ├── 📞 555-1234 (Clickable tel: link - FIXED)
│   │   │   ├── Edit Button
│   │   │   └── Delete Button
│   │   ├── Round Baler Rental
│   │   │   ├── $150/day (Rent)
│   │   │   ├── Seller: John's Farm Services
│   │   │   ├── 📞 555-5678 (Clickable)
│   │   │   ├── Edit Button
│   │   │   └── Delete Button
│   │   └── Organic Tomato Seeds
│   │       ├── $25 (Sale)
│   │       ├── Seller: Heritage Seeds
│   │       ├── 📞 555-9012 (Clickable)
│   │       ├── Edit Button
│   │       └── Delete Button
│   └── 🟢 Post Listing Button (Bootstrap styled - FIXED)
│       └── Links to Marketplace/Add
├── Post Listing Form
│   ├── Item Name, Category, Condition
│   ├── Description, Images
│   ├── Pricing Options
│   │   ├── Sale Price (for "Sell only")
│   │   ├── Rental Price (for "Rent only")
│   │   └── Both (for "Sell and/or Rent")
│   ├── Seller Name & Phone
│   ├── Save Button → New listing posted
│   └── Cancel Button → Back to Index
├── Edit Listing Form
│   ├── Pre-filled with existing data
│   ├── Edit Condition & Pricing
│   ├── Edit Listing Status (Active/Sold/Expired)
│   ├── Save Button → Updates listing
│   └── Cancel Button → Back to Index
└── Delete Operation
    ├── Click Delete → Confirmation dialog
    ├── Confirm → Listing removed
    └── Cancel → Stay on page
```

---

## CRUD Operation Flows

### ✅ CREATE (Add New Item)

**Resources (Can extend)**:
```
1. Click "Add Resource" button (if added)
2. Fill form: Name, Category, Quantity, Unit, LowStockThreshold, Supplier
3. Click Save
4. Redirect to Resources/Index
5. New resource appears in list
6. ✅ Complete
```

**Machinery**:
```
1. Click "Add Equipment" button (GREEN, Bootstrap styled)
2. Fill form:
   - Name (e.g., "New Tractor")
   - Type (dropdown: Tractor, Baler, etc.)
   - Purchase Date (date picker)
   - Purchase Price ($)
   - Status (Excellent/Good/Fair/Poor)
   - Last Service Date (date picker)
   - Next Service Date (date picker)
3. Click "Save Equipment"
4. Redirect to Machinery/Index
5. New equipment appears in list, sorted by status
6. ✅ Complete
```

**Marketplace**:
```
1. Click "Post Listing" button (GREEN, Bootstrap styled)
2. Fill form:
   - Item Name (e.g., "John Deere Tractor")
   - Category (Equipment, Seeds, Supplies, Tools, Other)
   - Condition (New, Excellent, Good, Fair, Poor)
   - Description (textarea)
   - Listing Type (Sell/Rent/Both) - dropdown
   - Sale Price (if selling)
   - Rental Price/Day (if renting)
   - Your Name/Business
   - Your Phone Number (clickable tel: link)
   - Image URL (optional)
3. Click "Post Listing"
4. Redirect to Marketplace/Index
5. New listing appears at top (sorted by creation date newest first)
6. ✅ Complete
```

---

### ✅ READ (View List)

**Resources**:
```
1. Click "Resources" in navbar
2. All 5 resources display as cards
3. Click category tab (e.g., "Fertilizer")
4. URL changes to: Resources/Index?category=Fertilizer
5. View shows only fertilizer resources (NPK)
6. ✅ Complete
```

**Machinery**:
```
1. Click "Machinery" in navbar
2. All equipment displays as cards
3. Sorted by Status: Excellent → Good → Fair → Poor
4. Each card shows:
   - Equipment Name
   - Type
   - Status badge
   - Purchase & Service Dates
   - Days until next service
   - Edit & Delete buttons
5. ✅ Complete
```

**Marketplace**:
```
1. Click "Marketplace" in navbar
2. All active listings display as cards
3. Sorted by creation date (newest first)
4. Each card shows:
   - Item Name
   - Condition badge
   - Category
   - Description
   - Pricing (Sale and/or Rental)
   - Seller Name
   - Clickable Phone Number (tel: link)
   - Edit & Delete buttons
5. ✅ Complete
```

---

### ✅ UPDATE (Edit Item)

**Machinery**:
```
1. View Machinery/Index
2. Click "Edit" button on any equipment card
3. Navigate to: Machinery/Edit/1 (or other ID)
4. Form pre-fills with existing data
5. Modify any field (Name, Type, Dates, Status, etc.)
6. Click "Save Changes"
7. Redirect to Machinery/Index
8. Equipment card shows updated information
9. UpdatedDate timestamp updated
10. ✅ Complete
```

**Marketplace**:
```
1. View Marketplace/Index
2. Click "Edit" button on any listing card
3. Navigate to: Marketplace/Edit/1 (or other ID)
4. Form pre-fills with existing data
5. Modify Item Name, Pricing, Seller Info, etc.
6. Update ListingStatus if needed (Active/Sold/Expired)
7. Click "Save Changes"
8. Redirect to Marketplace/Index
9. Listing card shows updated information
10. UpdatedDate timestamp updated
11. ✅ Complete
```

---

### ✅ DELETE (Remove Item)

**Machinery**:
```
1. View Machinery/Index
2. Click "Delete" button on any equipment card
3. Browser confirmation: "Are you sure?"
4. If YES:
   - POST request to Machinery/Delete/1
   - Equipment removed from _machineryList
   - Redirect to Machinery/Index
   - Equipment card no longer appears
5. If NO:
   - Cancel
   - Stay on page
6. ✅ Complete
```

**Marketplace**:
```
1. View Marketplace/Index
2. Click "Delete" button on any listing card
3. Browser confirmation: "Are you sure?"
4. If YES:
   - POST request to Marketplace/Delete/1
   - Listing removed from _listingsList
   - Redirect to Marketplace/Index
   - Listing card no longer appears
5. If NO:
   - Cancel
   - Stay on page
6. ✅ Complete
```

---

## Form Validation Flow

### Add Equipment Form Example
```
1. User clicks "Add Equipment"
2. GET Machinery/Add triggered
3. Empty form displays with 8 required fields:
   - Name * (required)
   - Type * (required, dropdown)
   - PurchaseDate * (required, date picker)
   - PurchasePrice * (required, decimal)
   - Status * (required, dropdown)
   - LastServiceDate * (required, date picker)
   - NextServiceDate * (required, date picker)

4. User submits form incomplete or with invalid data
5. POST Machinery/Add triggered
6. Controller validates ModelState
7. If !ModelState.IsValid:
   - Return View(machinery)
   - Form re-displays with submitted data
   - Error messages shown for each invalid field
   - Example: "The Status field is required"
8. If ModelState.IsValid:
   - Assign new ID
   - Set CreatedDate = DateTime.Now
   - Set UpdatedDate = DateTime.Now
   - Add to _machineryList
   - Redirect to Machinery/Index
   - New equipment appears in list

✅ Complete
```

---

## Data Model Relationships

```
ResourceUsage (Future)
└── Many Resources (Categories)
    ├── Fertilizer
    ├── Seed
    ├── Chemical
    ├── Water
    └── Fuel

MaintenanceHistory (Future)
└── Many Machinery Items
    ├── Tractor
    ├── Combine
    ├── Baler
    └── ...

MarketplaceListing (Standalone)
├── Categories
│   ├── Equipment
│   ├── Seeds
│   ├── Supplies
│   └── Tools
└── Listing Types
    ├── Sale
    ├── Rent
    └── Both
```

---

## Data Persistence (Current)

All three modules use **static in-memory Lists**:

```
⚠️ Data Reset on Application Restart
┌─────────────────────────────────┐
│ Static List<Resource>           │  5 items pre-loaded
│ Static List<Machinery>          │  3 items pre-loaded
│ Static List<MarketplaceListing> │  3 items pre-loaded
└─────────────────────────────────┘
         ↓ Each Time App Restarts ↓
┌─────────────────────────────────┐
│ Reset to Default Sample Data    │
└─────────────────────────────────┘
```

**For Production**: Replace with Entity Framework Core + SQL Database

---

## Error Handling

### Model Validation
```
Invalid Input → ModelState.IsValid = false
              → Form re-displays with error messages
              → User sees: "The [Field] field is required"
              → User can correct and resubmit
```

### Not Found
```
GET Machinery/Edit/999 (ID doesn't exist)
                    → return NotFound()
                    → 404 page displayed
```

### Delete Confirmation
```
User clicks Delete
           → JavaScript confirmation dialog: "Are you sure?"
           → User confirms
           → POST sent to Delete action
           → Item removed from collection
           → Redirect to Index
```

---

## Navigation Breadcrumbs (Proposed Enhancement)

Could add breadcrumbs to forms:
```
Resources > Add Resource > Form
Machinery > Add Equipment > Form
Marketplace > Post Listing > Form
```

Currently: Only direct links via buttons.

---

## Responsive Design

All views use Bootstrap grid system:
- Views are responsive on mobile, tablet, desktop
- Cards stack on small screens
- Forms use col-md-6 for side-by-side on large screens
- Buttons are touch-friendly on mobile

