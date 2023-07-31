using System;
using System.Collections.Generic;

namespace Mgm.VI.Data.Model
{
    public class OrderModel : IOrderModel
    {
        public string ID { get; set; }
        public string ServicingStatus { get; set; }
        public string Version { get; set; }
        public string SRDueDate { get; set; }
        public string EmbargoDate { get; set; }
        public string PrimaryVideo { get; set; }
        public string SecondaryAudio { get; set; }
        public string SubtitlesFull { get; set; }
        public string SubtitlesForced { get; set; }
        public string ClosedCaptions { get; set; }
        public string Trailer { get; set; }
        public string RebillType { get; set; }
        public string Amount { get; set; }
        public string MetaData { get; set; }
        public string ArtWork { get; set; }
        public string Document { get; set; }
        public string EOPPO { get; set; }
        public string EOPDueDate { get; set; }
        public string EOPStatus { get; set; }
        public string EOPResource { get; set; }
        public string EOPNotes { get; set; }
        public string PPSPO { get; set; }
        public string PPSDueDate { get; set; }
        public string PPSStatus { get; set; }
        public string PPSResource { get; set; }
        public string PPSNotes { get; set; }
        public string MaterialNotes { get; set; }
        public string Other { get; set; }
        public string IPMMedia { get; set; }
        public List<IRejectionModel> Rejections { get; set; }
    }
}
