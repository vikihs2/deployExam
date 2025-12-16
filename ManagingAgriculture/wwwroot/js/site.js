// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Debounce helper
function debounce(fn, wait) {
	let t;
	return function(...args) {
		clearTimeout(t);
		t = setTimeout(() => fn.apply(this, args), wait);
	};
}

// Marketplace live filter (search + category)
document.addEventListener('DOMContentLoaded', function () {
	const search = document.querySelector('.marketplace-search');
	const category = document.querySelector('.marketplace-category');
	const grid = document.getElementById('marketplace-grid');

	if (search && category && grid) {
		const doFilter = debounce(() => {
			const q = encodeURIComponent(search.value || '');
			const c = encodeURIComponent(category.value || 'All');
			fetch(`/Marketplace/Filter?category=${c}&q=${q}`)
				.then(r => r.text())
				.then(html => { grid.innerHTML = html; })
				.catch(() => {});
		}, 250);

		search.addEventListener('input', doFilter);
		category.addEventListener('change', doFilter);
	}

	// Resources: AJAX adjust buttons
	document.querySelectorAll('.adjust-ajax').forEach(btn => {
		btn.addEventListener('click', function (e) {
			const id = this.getAttribute('data-id');
			const delta = this.getAttribute('data-delta');
			fetch('/Resources/AdjustQuantityAjax', {
				method: 'POST',
				headers: { 'Content-Type': 'application/json', 'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value },
				body: JSON.stringify({ id: parseInt(id), delta: parseInt(delta) })
			})
			.then(r => r.json())
			.then(json => {
				if (json && json.success) {
					// update displayed quantity in the containing card
					const card = btn.closest('.resource-card');
					if (card) {
						const amount = card.querySelector('.amount-number');
						if (amount) amount.textContent = json.quantity;
					}
				}
			})
			.catch(() => {});
		});
	});
});

// Sidebar hover behavior: shrink when not hovered, expand on hover (persisted in localStorage)
document.addEventListener('DOMContentLoaded', function () {
	const sidebar = document.querySelector('nav.sidebar');
	const main = document.querySelector('.main-content');
	if (!sidebar) return;

	// default: shrunk
	sidebar.classList.add('sidebar-shrink');
	if (main) main.classList.add('main-content-shrink');

	let _collapseTimeout = null;

	sidebar.addEventListener('mouseenter', () => {
		sidebar.classList.remove('sidebar-shrink');
		sidebar.classList.add('sidebar-expanded');
		if (main) { main.classList.remove('main-content-shrink'); main.classList.add('main-content-expanded'); }
	});
	sidebar.addEventListener('mouseleave', () => {
		sidebar.classList.add('sidebar-shrink');
		sidebar.classList.remove('sidebar-expanded');
		if (main) { main.classList.add('main-content-shrink'); main.classList.remove('main-content-expanded'); }
	});

	// When user clicks inside the sidebar, briefly keep it expanded so it doesn't "snap" or get stuck
	sidebar.addEventListener('click', (e) => {
		clearTimeout(_collapseTimeout);
		sidebar.classList.remove('sidebar-shrink');
		sidebar.classList.add('sidebar-expanded');
		if (main) { main.classList.remove('main-content-shrink'); main.classList.add('main-content-expanded'); }

		// After a short delay, allow it to collapse again if the cursor isn't hovering
		_collapseTimeout = setTimeout(() => {
			if (!sidebar.matches(':hover')) {
				sidebar.classList.add('sidebar-shrink');
				sidebar.classList.remove('sidebar-expanded');
				if (main) { main.classList.add('main-content-shrink'); main.classList.remove('main-content-expanded'); }
			}
		}, 700);
	});
});
