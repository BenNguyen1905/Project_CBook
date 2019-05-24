﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class DonDatHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonDatHang()
        {
            this.ChiTietDDHs = new HashSet<ChiTietDDH>();
        }

        [Key]
        [Display(Name = "Mã đơn hàng")]
        public int MaDDH { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Ngày đặt")]
        public Nullable<System.DateTime> NgayDat { get; set; }

        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Ngày giao")]
        public Nullable<System.DateTime> NgayGiao { get; set; }

        
    }
}
