namespace mojoPortal.Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mp_BannedIPAddresses")]
    public partial class BannedIPAddresses
    {
        [Key]
        public int RowID { get; set; }

        [Required]
        [StringLength(50)]
        public string BannedIP { get; set; }

        public DateTime BannedUTC { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string BannedReason { get; set; }
    }
}
