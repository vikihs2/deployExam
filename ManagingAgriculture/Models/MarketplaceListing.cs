using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagingAgriculture.Models
{
	public class MarketplaceListing
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string ItemName { get; set; } = string.Empty;

		[Required]
		[StringLength(50)]
		public string Category { get; set; } = string.Empty;

		[Required]
		[StringLength(20)]
		public string ConditionStatus { get; set; } = string.Empty;

		public string? Description { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? SalePrice { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal? RentalPricePerDay { get; set; }

		[Required]
		[StringLength(100)]
		public string SellerName { get; set; } = string.Empty;

		[Required]
		[StringLength(20)]
		public string SellerPhone { get; set; } = string.Empty;

		public string? ImageUrl { get; set; }

		[Required]
		[StringLength(20)]
		public string ListingType { get; set; } = string.Empty;

		[StringLength(20)]
		public string ListingStatus { get; set; } = "Active";

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }
	}
}
