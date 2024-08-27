﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PedagioPayPortalUsuario.Models
{
    [Table("USUARIO_PLACA")]
    public partial class PLACAs
    {
        [Key]
        public int ID_USUARIO_PLACA { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string USUARIO_EMAIL { get; set; }
        [Required]
        [StringLength(7)]
        [Unicode(false)]
        public string PLACA { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string MARCA { get; set; }
        [StringLength(40)]
        [Unicode(false)]
        public string MODELO { get; set; }
        public int? ANO { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string COR { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string TOKEN_USUARIO { get; set; }
        public bool? CD_STATUS { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DH_TIMESTAMP { get; set; }
    }
}