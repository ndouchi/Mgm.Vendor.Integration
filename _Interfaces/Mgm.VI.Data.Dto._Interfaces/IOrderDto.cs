using System.Collections.Generic;

namespace Mgm.VI.Data.Dto
{
    public interface IOrderDto : IDto
    {
        #region Properties
        string ID { get; set; }
        string ServicingStatus { get; set; }
        string Version { get; set; }

        string SRDueDate { get; set; }
        string EmbargoDate { get; set; }
        string PrimaryVideo { get; set; }
        string SecondaryAudio { get; set; }
        string SubtitlesFull { get; set; }
        string SubtitlesForced { get; set; }
        string ClosedCaptions { get; set; }
        string Trailer { get; set; }
        string RebillType { get; set; }
        string Amount { get; set; }
        string MetaData { get; set; }
        string ArtWork { get; set; }
        string Document { get; set; }
        string EOPPO { get; set; }
        string EOPDueDate { get; set; }
        string EOPStatus { get; set; }
        string EOPResource { get; set; }
        string EOPNotes { get; set; }
        string PPSPO { get; set; }
        string PPSDueDate { get; set; }
        string PPSStatus { get; set; }
        string PPSResource { get; set; }
        string PPSNotes { get; set; }
        string MaterialNotes { get; set; }
        string Other { get; set; }
        string IPMMedia { get; set; }

        List<IRejectionDto> Rejections { get; set; }
        #endregion Properties
    }
}