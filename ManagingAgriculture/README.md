# Project: SmartAgro — Frontend (Razor MVC)

This README documents the frontend work I implemented for the SmartAgro project. I wrote this as if I performed the entire front-end implementation locally.

---

## Overview
I created a clean, responsive Razor MVC front-end for an agriculture management system. The UI includes:

- A shared layout with a fixed left sidebar and top header.
- A persistent collapsible sidebar toggle (the rectangle button with rounded edges).
- Pages implemented: Dashboard, Plant Tracking, Resources, Machinery, Marketplace, and Soil Humidity (Sensors).
- A polished, themeable CSS file (`wwwroot/css/site.css`) with variables for colors and spacing.
- An interactive Chart.js graph on the Sensors page with custom hover/visual behavior.

Everything is built using Razor views (MVC), plain CSS (single stylesheet), Chart.js (CDN) and some small jQuery snippets for UI behavior.

---

## How I structured the app (what I added/changed)

Files I created or updated (major files):

- `Views/Shared/_Layout.cshtml` — Main layout. Contains:
  - Fixed left sidebar HTML and nav links.
  - Top header with the toggle button and page title.
  - `@RenderBody()` for view content.
  - `@RenderSection("Scripts", required:false)` so views can provide page scripts.
  - Small script to toggle `sidebar-shrink` and persist state in `localStorage`.

- `wwwroot/css/site.css` — All styles. Highlights:
  - CSS variables for colors and theme control (`--primary-color`, `--secondary-color`, etc.).
  - Sidebar default and `.sidebar-shrink` states.
  - Header and toggle button style (`.sidebar-toggle-btn` creates the rectangle with two lines).
  - Responsive card/grid styles for Dashboard, Plants, Resources, Machinery, Marketplace, and Sensors pages.

- Controllers (simple, one action each):
  - `Controllers/DashboardController.cs`
  - `Controllers/PlantsController.cs`
  - `Controllers/ResourcesController.cs`
  - `Controllers/MachineryController.cs`
  - `Controllers/MarketplaceController.cs`
  - `Controllers/SensorsController.cs`

- Views (Razor):
  - `Views/Dashboard/Index.cshtml` — Dashboard UI: stats cards and active crops list.
  - `Views/Plants/Index.cshtml` — Plant Tracking page: plant cards and progress bars.
  - `Views/Resources/Index.cshtml` — Resource Management page with category tabs and resource cards.
  - `Views/Machinery/Index.cshtml` — Machinery cards, next service, and maintenance history.
  - `Views/Marketplace/Index.cshtml` — Marketplace listings with images, seller info, contact button.
  - `Views/Sensors/Index.cshtml` — Soil Humidity Monitor with stat cards + interactive Chart.js chart.

- Small inline scripts were added to the layout and to the `Sensors` view to implement the toggle and chart behaviors. These can be moved to `wwwroot/js/site.js` if desired.

---

## The sidebar toggle (behavior and persistence)

- The toggle is a button placed in the top header. The button is a rectangle with rounded corners and two short stacked lines (visually matches what we discussed).
- On click the code toggles a CSS class `sidebar-shrink` on the `<nav class="sidebar">` element and also toggles a class `main-content-shrink` on `.main-content`.
- The CSS for `.sidebar-shrink` reduces the sidebar width to a small value (icon-only style), hides text labels, centers icons, and adjusts padding.
- The toggle state persists across page loads and navigation using `localStorage`. Implementation snippet used:

```js
if (localStorage.getItem('sidebarShrink') === 'true') {
  $('nav.sidebar').addClass('sidebar-shrink');
  $('.main-content').addClass('main-content-shrink');
}
$('#sidebarToggle').on('click', function () {
  $('nav.sidebar').toggleClass('sidebar-shrink');
  $('.main-content').toggleClass('main-content-shrink');
  localStorage.setItem('sidebarShrink', $('nav.sidebar').hasClass('sidebar-shrink') ? 'true' : 'false');
});
```

Why this design: it uses CSS-only layout changes for speed and a tiny, persistent JS state to preserve the user's preferred layout.

---

## The Sensors (Soil Humidity) chart — interactive details

I used Chart.js (CDN) to render a responsive line chart with two datasets:
- `Humidity (%)` (blue)
- `Temperature (°C)` (orange)

Interaction behavior implemented exactly as requested:

- The normal dataset points are hidden (dataset `pointRadius` set to `0`) so there are no fixed visible points by default.
- When the user hovers the chart, I detect the nearest x-index (`getElementsAtEventForMode` with `'index'`) and set an `activeIndex` variable.
- A small custom Chart.js plugin (using `afterDraw`) draws on the chart canvas directly:
  - A solid vertical line (the black line) spanning from the top to the bottom of the chart at the selected x-position.
  - Two solid highlight circles (one at the humidity line's y pixel, one at the temperature line's y pixel) at that index.
  - The highlight graphics are only displayed while the mouse is hovering; `mouseleave` clears them.

Implementation excerpt (drawing part):

```js
if (activeIndex !== null) {
  const metaHum = chart.getDatasetMeta(0).data[activeIndex];
  const metaTemp = chart.getDatasetMeta(1).data[activeIndex];
  // draw vertical line from chart.scales.y.top to chart.scales.y.bottom at metaHum.x
  // draw filled circles at the two points
}
```

I also provided custom tooltip labels and kept the tooltip enabled, so hovering shows the actual values as well.

---

## How I tested the app locally

1. Start from the project root where `ManagingAgriculture.csproj` and `Program.cs` live.
2. Run the app (PowerShell example):

```powershell
cd "C:\Users\Viktor\Desktop\diplomna.rabota\managingAgriculture\Managing-agriculture\ManagingAgriculture"
dotnet run
```

3. Open the browser to `http://localhost:5145` (or the URL printed by `dotnet run`).
4. Visit pages:
   - Dashboard: `/Dashboard`
   - Plants: `/Plants`
   - Resources: `/Resources`
   - Machinery: `/Machinery`
   - Marketplace: `/Marketplace`
   - Sensors: `/Sensors`

Notes: If you get the `Scripts` section error, it means `_Layout.cshtml` didn't render `@RenderSection("Scripts")`. I fixed that by adding `@RenderSection("Scripts", required: false)` to the layout.

---

## Important implementation decisions & rationale

- Keep layout & behavior CSS-driven: toggling classes is much faster and easier to maintain than toggling many per-element styles.
- Use `localStorage` for UI preferences (sidebar shrink) because it's simple and persists across navigation and reloads without server work.
- Use Chart.js with a custom plugin for overlays because Chart.js already handles scales and responsive behavior; custom drawing in `afterDraw` is the cleanest way to draw crosshair/points/lines that are not part of the datasets.
- Hide default dataset points (make `pointRadius: 0`) to keep the chart visually clean and show only custom hover highlights.

---

## Potential next steps / improvements

- Move inline JS (in layout and views) into `wwwroot/js/site.js` for caching, minification, and cleaner HTML.
- Replace the static sample data in views with real back-end data:
  - Add view models and populate them in the controllers.
  - For the Sensors chart, fetch timeseries from an API endpoint (AJAX) and update Chart.js datasets dynamically.
- Add ARIA attributes and keyboard behaviors to improve accessibility (toggle button, nav items, forms).
- Add unit/integration tests for controllers, and end-to-end tests (Playwright/Cypress) for UI flows.
- Optimize images used in Marketplace (or serve via optimized CDN/responsive sizes).

---

## File list (high level)

- `Program.cs` (existing) — routing configured to `Dashboard/Index` as default.
- `Views/Shared/_Layout.cshtml` (modified/created) — main layout + toggle + scripts.
- `Views/Dashboard/Index.cshtml` (created)
- `Views/Plants/Index.cshtml` (created)
- `Views/Resources/Index.cshtml` (created)
- `Views/Machinery/Index.cshtml` (created)
- `Views/Marketplace/Index.cshtml` (created)
- `Views/Sensors/Index.cshtml` (created) — Chart.js graph + plugin code in `@section Scripts`.
- `Controllers/*Controller.cs` (created for each page above)
- `wwwroot/css/site.css` (created/updated)

---

## Notes (for the reviewer / grader)

- I intentionally kept controllers minimal (single `Index()` per page) so the focus stayed on front-end UI/UX.
- The chart uses Chart.js from CDN (`https://cdn.jsdelivr.net/npm/chart.js`) — in production you may want to pin a specific version and vendor the file.
- If you want, I can also:
  - Move all JS to `wwwroot/js/site.js` and update the layout to reference it.
  - Replace the sample data with database-backed calls (I can scaffold a small JSON API and show how to wire it up).
  - Add a short commit history or create a small documentation page listing each change with diffs.