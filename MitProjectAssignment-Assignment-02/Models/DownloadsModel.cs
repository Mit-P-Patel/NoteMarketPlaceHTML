using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MitProjectAssignment.Models
{
    public class DownloadsModel
    {
        public int ID { get; set; }
        public int NoteID { get; set; }
        public int Seller { get; set; }
        public int Downloader { get; set; }
        public bool IsSellerHasAllowedDownload { get; set; }
        public string AttachmentPath { get; set; }
        public bool IsAttachmentDownload { get; set; }
        public Nullable<System.DateTime> AttachmentDownloadedDate { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<decimal> PurchasedPrice { get; set; }
        public string NoteTitle { get; set; }
        public string NoteCategory { get; set; }

    }

}
